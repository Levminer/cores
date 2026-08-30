use axum::body::Bytes;
use axum::extract::connect_info::ConnectInfo;
use axum::extract::ws::{CloseFrame, Utf8Bytes};
use axum::extract::State;
use axum::{
    extract::ws::{Message, WebSocket, WebSocketUpgrade},
    http::Method,
    response::IntoResponse,
    routing::get,
    Router,
};
use clap::Parser;
use ezrtc::host::EzRTCHost;
use ezrtc::protocol::{SignalMessage, Status, UserId};
use ezrtc::socket::{DataChannelHandler, WSHost};
use ezrtc::{RTCDataChannel, RTCDataChannelState, RTCIceServer};
use futures::{sink::SinkExt, stream::StreamExt};
use hardwareinfo::settings::{get_settings, get_settings_path, Settings};
use hardwareinfo::{refresh_hardware_info, Data, HardwareInfo, Networks, Nvml, System};
use log::{error, info, warn, LevelFilter};
use r2d2;
use r2d2_sqlite::SqliteConnectionManager;
use rusqlite::Connection;
use serde::{Deserialize, Serialize};
use simplelog::{ColorChoice, CombinedLogger, Config, TermLogger, TerminalMode, WriteLogger};
use std::fs::OpenOptions;
use std::net::SocketAddr;
use std::ops::ControlFlow;
use std::process::Command;
use std::str::FromStr;
use std::sync::Arc;
use std::time::Duration;
use tower_http::{
    cors::{Any, CorsLayer},
    trace::{DefaultMakeSpan, TraceLayer},
};
use wol::{send_wol, MacAddr};

mod db;
mod service;

#[derive(Serialize, Deserialize)]
pub struct GenericMessage<T> {
    pub r#type: String,
    pub data: T,
}

pub struct AppState {
    hardware_info_receiver: tokio::sync::broadcast::Receiver<HardwareInfo>,
    settings: Settings,
    pool: r2d2::Pool<SqliteConnectionManager>,
}

/// Modern hardware monitor with remote monitoring.
#[derive(Parser, Debug)]
#[command(version, about, long_about = None)]
struct Args {
    /// Setup coresd to run as a service
    #[arg(required = false, long, short = 's')]
    service: bool,
    /// Enable file logging to the settings folder
    #[arg(required = false, long, short = 'l')]
    logs: bool,
}

#[tokio::main]
async fn main() {
    // Parse arguments
    let args = Args::parse();

    // Logger
    let mut loggers: Vec<Box<dyn simplelog::SharedLogger>> = vec![TermLogger::new(
        LevelFilter::Info,
        Config::default(),
        TerminalMode::Mixed,
        ColorChoice::Auto,
    )];

    // Add file logger if --logs flag is set
    if args.logs {
        let log_folder = get_settings_path().join("Cores");
        std::fs::create_dir_all(&log_folder).expect("Failed to create settings folder");
        let log_file = log_folder.join("coresd.log");

        // delete old log file
        if log_file.exists() {
            std::fs::remove_file(&log_file).expect("Failed to delete old log file");
        }

        match OpenOptions::new().create(true).append(true).open(&log_file) {
            Ok(file) => {
                loggers.push(WriteLogger::new(LevelFilter::Info, Config::default(), file));
                info!("File logging enabled: {:?}", log_file);
            }
            Err(e) => {
                error!("Failed to open log file: {}", e);
            }
        }
    }

    CombinedLogger::init(loggers).expect("Failed to initialize logger");

    // Check if service setup is requested
    if args.service {
        service::setup_service();
    } else {
        warn!(
            "You are running coresd as an executable, to setup it as a service, run `sudo ./coresd --service`"
        );
    }

    // Get settings
    let settings = get_settings();
    info!("Connection code: {:?}", settings.connection_code);

    // Init database
    let folder = get_settings_path().join("Cores");
    let connection_manager = match Connection::open(folder.join("stats.sqlite")) {
        Ok(conn) => {
            conn.close().expect("Failed to close database connection");
            SqliteConnectionManager::file(folder.join("stats.sqlite"))
        }
        Err(_) => {
            warn!("Failed to open file database, using memory database");
            SqliteConnectionManager::memory()
        }
    };

    let pool = r2d2::Pool::builder()
        .max_size(10)
        .build(connection_manager)
        .expect("Failed to create connection pool");
    db::seed(&pool.get().expect("Failed to get connection"));
    db::cleanup(&pool.get().expect("Failed to get connection"));

    // Hardware info channel
    let (channel_sender, channel_receiver) = tokio::sync::broadcast::channel(10);

    let mut data = Data {
        first_run: true,
        sys: System::new_all(),
        network: Networks::new_with_refreshed_list(),
        hw_info: HardwareInfo::default(),
        nvml: Nvml::init(),
        nvml_available: true,
        interval: settings.interval as f64,
    };

    let app_state = Arc::new(AppState {
        hardware_info_receiver: channel_receiver.resubscribe(),
        settings: settings.clone(),
        pool,
    });

    // Setup HTTP server routes
    let app = Router::new()
        .route("/", get(handle_root_request))
        .route("/ws", get(ws_handler))
        .layer(
            CorsLayer::new()
                .allow_methods([Method::GET, Method::POST])
                .allow_origin(Any),
        )
        .layer(
            TraceLayer::new_for_http()
                .make_span_with(DefaultMakeSpan::default().include_headers(true)),
        )
        .with_state(app_state.clone());

    let listener = tokio::net::TcpListener::bind("127.0.0.1:5390")
        .await
        .expect("Port already in use, check if coresd is already running");

    info!(
        "HTTP server listening on http://{}",
        listener.local_addr().unwrap()
    );

    // Refresh hardware info every specified interval
    let app_state_clone = app_state.clone();
    let hw_task = tokio::spawn(async move {
        loop {
            data.sys.refresh_all();
            std::thread::sleep(hardwareinfo::MINIMUM_CPU_UPDATE_INTERVAL);
            data.sys.refresh_all();
            data.network.refresh(true);
            refresh_hardware_info(&mut data);

            // Send hardware info, ignore if queue is lagged (receivers can't keep up)
            if let Err(err) = channel_sender.send(data.hw_info.clone()) {
                error!("Failed to send hardware info: {}", err);
                continue;
            }

            let conn = app_state_clone
                .pool
                .get()
                .expect("Failed to get connection");
            db::insert_data(
                &conn,
                &serde_json::to_string(&data.hw_info).expect("Failed to serialize HardwareInfo"),
            );

            tokio::time::sleep(std::time::Duration::from_millis(
                (settings.interval as u64 * 1000) - 300,
            ))
            .await;
        }
    });

    // Start HTTP server
    let server_task = tokio::spawn(async move {
        axum::serve(
            listener,
            app.into_make_service_with_connect_info::<SocketAddr>(),
        )
        .await
        .unwrap();
    });

    // Start RTC server
    let app_state_clone = app_state.clone();
    let rtc_task = tokio::spawn(async move {
        // Define your STUN and TURN servers here
        let ice_servers = vec![RTCIceServer {
            urls: vec!["stun:stun.cloudflare.com:3478".to_owned()],
            ..Default::default()
        }];

        // Define your data channel handler
        struct MyDataChannelHandler {
            receiver: tokio::sync::broadcast::Receiver<HardwareInfo>,
            state: Arc<AppState>,
        }

        impl DataChannelHandler for MyDataChannelHandler {
            fn handle_data_channel_open(&self, dc: Arc<RTCDataChannel>) {
                warn!("Data channel opened!");

                let mut receiver = self.receiver.resubscribe();
                let state = self.state.clone();

                tokio::spawn(async move {
                    if dc.ready_state() == RTCDataChannelState::Open {
                        // Get every third element from the last 60s and 60m hardware info
                        let last60s_hardware_info = {
                            db::select_seconds_data(
                                &state.pool.get().expect("Failed to get connection"),
                            )
                            .iter()
                            .cloned()
                            .collect::<Vec<HardwareInfo>>()
                        };
                        let last60m_hardware_info = {
                            db::select_minutes_data(
                                &state.pool.get().expect("Failed to get connection"),
                            )
                            .iter()
                            .cloned()
                            .collect::<Vec<HardwareInfo>>()
                        };

                        // Send initial data
                        let network_data = GenericMessage::<HardwareInfo> {
                            r#type: "initialData".to_string(),
                            data: last60s_hardware_info
                                .last()
                                .cloned()
                                .unwrap_or(HardwareInfo::default()),
                        };

                        if dc
                            .send_text(serde_json::to_string(&network_data).unwrap())
                            .await
                            .is_err()
                        {
                            info!("Failed to send initialData to client");
                        };

                        // Send 60s data
                        for hw_info in last60s_hardware_info {
                            let network_data = GenericMessage::<HardwareInfo> {
                                r#type: "secondsData".to_string(),
                                data: hw_info.clone(),
                            };

                            if dc
                                .send_text(serde_json::to_string(&network_data).unwrap())
                                .await
                                .is_err()
                            {
                                info!("Failed to send secondsData to client");
                                break;
                            };
                        }

                        // Send 60m data
                        for hw_info in last60m_hardware_info {
                            let network_data = GenericMessage::<HardwareInfo> {
                                r#type: "minutesData".to_string(),
                                data: hw_info.clone(),
                            };

                            if dc
                                .send_text(serde_json::to_string(&network_data).unwrap())
                                .await
                                .is_err()
                            {
                                info!("Failed to send minutesData to client");
                                break;
                            };
                        }

                        // Send data every interval
                        loop {
                            let hw_message = match receiver.recv().await {
                                Ok(data) => data,
                                Err(tokio::sync::broadcast::error::RecvError::Lagged(_)) => {
                                    continue;
                                }
                                Err(_) => {
                                    break;
                                }
                            };
                            let network_data = GenericMessage::<HardwareInfo> {
                                r#type: "data".to_string(),
                                data: hw_message.clone(),
                            };

                            if dc
                                .send_text(serde_json::to_string(&network_data).unwrap())
                                .await
                                .is_err()
                            {
                                info!("Failed to send data to client");
                                break;
                            };

                            tokio::time::sleep(std::time::Duration::from_secs(
                                state.settings.interval as u64,
                            ))
                            .await;
                        }
                    }
                });
            }

            fn handle_data_channel_message(&self, message: String) {
                warn!("Data channel message received: {:?}", message);

                let network_message: Result<GenericMessage<String>, _> =
                    serde_json::from_str(&message);

                match network_message {
                    Ok(message) => match message.r#type.as_str() {
                        "shutdown" => {
                            if Command::new("sudo")
                                .arg("shutdown")
                                .arg("-h")
                                .arg("+1")
                                .spawn()
                                .is_err()
                            {
                                error!("Failed to execute shutdown command");
                            }
                        }
                        "sleep" => {
                            if Command::new("sudo")
                                .arg("systemctl")
                                .arg("suspend")
                                .spawn()
                                .is_err()
                            {
                                error!("Failed to execute sleep command");
                            }
                        }
                        "restart" => {
                            if Command::new("sudo")
                                .arg("shutdown")
                                .arg("-r")
                                .arg("+1")
                                .spawn()
                                .is_err()
                            {
                                error!("Failed to execute restart command");
                            }
                        }
                        "wol" => {
                            let formatted_mac = format_mac_address(&message.data);
                            let mac_addr = MacAddr::from_str(&formatted_mac);

                            match mac_addr {
                                Ok(mac) => {
                                    send_wol(mac, None, None).expect("Failed to send WOL packet");
                                    info!("Sending WOL packet to: {:?}", mac);
                                }
                                Err(e) => {
                                    warn!("Failed to parse MAC address: {:?}", e);
                                }
                            }
                        }
                        _ => {
                            warn!("Unknown message type: {:?}", message.r#type);
                        }
                    },
                    Err(e) => {
                        warn!("Failed to parse message: {:?}", e);
                    }
                }
            }

            fn handle_keep_alive(&self, handle: &mut WSHost, user_id: UserId) {
                let state = self.state.clone();

                let hw_info = {
                    let last_60s_data = db::select_seconds_data(
                        &state.pool.get().expect("Failed to get connection"),
                    );
                    last_60s_data
                        .into_iter()
                        .last()
                        .unwrap_or(HardwareInfo::default())
                };

                let cpu_usage = hw_info.cpu.max_load;
                let gpu_usage = if let Some(gpu) = hw_info.gpu.cards.get(0) {
                    gpu.max_load
                } else {
                    0.0
                };
                let memory_usage = if let Some(memory) = hw_info.ram.load.get(2) {
                    memory.value
                } else {
                    0.0
                };

                let ping_message = SignalMessage::KeepAlive(
                    user_id,
                    Status {
                        session_id: Some(handle.session_id.clone()),
                        is_host: Some(true),
                        version: Some(env!("CARGO_PKG_VERSION").to_string()),
                        metadata: Some(
                            serde_json::json!({"cpu": cpu_usage, "gpu": gpu_usage, "ram": memory_usage}),
                        ),
                    },
                );
                handle
                    .handle
                    .text(serde_json::to_string(&ping_message).unwrap())
                    .unwrap();

                info!("Sending pong to server");
            }
        }

        // Start the connection
        let url = format!("wss://{}/one-to-many", settings.connection_url);
        let _host = EzRTCHost::new(
            url,
            settings.connection_code,
            ice_servers,
            Arc::new(Box::new(MyDataChannelHandler {
                receiver: channel_receiver.resubscribe(),
                state: app_state_clone.clone(),
            })),
        )
        .await;

        info!("RTC started");

        std::future::pending::<()>().await;
    });

    let app_state_clone = app_state.clone();
    let cleanup_task = tokio::spawn(async move {
        loop {
            let conn = app_state_clone
                .pool
                .get()
                .expect("Failed to get connection");
            db::cleanup(&conn);

            info!("Cleanup completed");
            tokio::time::sleep(Duration::from_secs(60 * 60)).await;
        }
    });

    // Start tasks
    tokio::select! {
        _ = server_task => {
            info!("Server stopped");
        },
        _ = rtc_task => {
            info!("RTC stopped");
        }
        _ = hw_task => {
            info!("HW stopped");
        }
        _ = cleanup_task => {
            info!("Cleanup task stopped");
        }
    };

    error!("Daemon stopped");
}

async fn handle_root_request() -> impl IntoResponse {
    "OK"
}

async fn ws_handler(
    ws: WebSocketUpgrade,
    ConnectInfo(addr): ConnectInfo<SocketAddr>,
    State(state): State<Arc<AppState>>,
) -> impl IntoResponse {
    info!("WS Client connected: {addr}");

    ws.on_upgrade(move |socket| handle_socket(socket, addr, state))
}

// Handle websocket connection and send data to the client
// One websocket connection will be spawned per client
async fn handle_socket(mut socket: WebSocket, addr: SocketAddr, state: Arc<AppState>) {
    // Send initial ping
    if socket
        .send(Message::Ping(Bytes::from_static(&[1, 2, 3])))
        .await
        .is_ok()
    {
        info!("Pinged {addr}...");
    } else {
        info!("Could not send ping {addr}!");

        return;
    }

    // Split socket into sender and receiver
    let (mut sender, mut receiver) = socket.split();

    // Get every third element from the last 60s and 60m hardware info
    let last60s_hardware_info = {
        db::select_seconds_data(&state.pool.get().expect("Failed to get connection"))
            .iter()
            .cloned()
            .collect::<Vec<HardwareInfo>>()
    };
    let last60m_hardware_info = {
        db::select_minutes_data(&state.pool.get().expect("Failed to get connection"))
            .iter()
            .cloned()
            .collect::<Vec<HardwareInfo>>()
    };

    // Send last 60s hardware info
    for hw_info in last60s_hardware_info {
        let network_data = GenericMessage::<HardwareInfo> {
            r#type: "secondsData".to_string(),
            data: hw_info.clone(),
        };

        if sender
            .send(Message::Text(
                serde_json::to_string(&network_data).unwrap().into(),
            ))
            .await
            .is_err()
        {
            break;
        }
    }

    // Send last 60m hardware info
    for hw_info in last60m_hardware_info {
        let network_data = GenericMessage::<HardwareInfo> {
            r#type: "minutesData".to_string(),
            data: hw_info.clone(),
        };

        if sender
            .send(Message::Text(
                serde_json::to_string(&network_data).unwrap().into(),
            ))
            .await
            .is_err()
        {
            break;
        }
    }

    // Spawn a sender task to send data to the client
    let mut send_task = tokio::spawn(async move {
        let mut receiver = state.hardware_info_receiver.resubscribe();

        loop {
            let hw_message = match receiver.recv().await {
                Ok(data) => data,
                Err(tokio::sync::broadcast::error::RecvError::Lagged(_)) => {
                    continue;
                }
                Err(_) => {
                    break;
                }
            };
            let network_data = GenericMessage::<HardwareInfo> {
                r#type: "data".to_string(),
                data: hw_message.clone(),
            };

            if sender
                .send(Message::Text(
                    serde_json::to_string(&network_data).unwrap().into(),
                ))
                .await
                .is_err()
            {
                break;
            }

            tokio::time::sleep(std::time::Duration::from_secs(
                state.settings.interval as u64,
            ))
            .await;
        }

        match sender
            .send(Message::Close(Some(CloseFrame {
                code: axum::extract::ws::close_code::NORMAL,
                reason: Utf8Bytes::from_static("Goodbye"),
            })))
            .await
        {
            Ok(_) => info!("Sent close to {addr}"),
            Err(e) => info!("Failed to close: {e}"),
        }
    });

    // Spawn a receiver task to receive data from the client
    let mut recv_task = tokio::spawn(async move {
        while let Some(Ok(msg)) = receiver.next().await {
            if process_message(msg, addr).is_break() {
                break;
            }
        }
    });

    // If any one of the tasks exit, abort the other.
    tokio::select! {
        rv_a = (&mut send_task) => {
            match rv_a {
                Ok(_) => info!("Sender task stopped"),
                Err(a) => info!("Error sending messages {a:?}")
            }
            recv_task.abort();
        },
        rv_b = (&mut recv_task) => {
            match rv_b {
                Ok(_) => info!("Receiver task stopped"),
                Err(b) => info!("Error receiving messages {b:?}")
            }
            send_task.abort();
        }
    }

    info!("Websocket context {addr} destroyed");
}

// Process incoming messages
fn process_message(msg: Message, who: SocketAddr) -> ControlFlow<(), ()> {
    match msg {
        Message::Text(t) => {
            info!(">>> {who} sent str: {t:?}");
        }
        Message::Binary(d) => {
            info!(">>> {} sent {} bytes: {:?}", who, d.len(), d);
        }
        Message::Close(c) => {
            if let Some(cf) = c {
                info!(
                    ">>> {} sent close with code {} and reason `{}`",
                    who, cf.code, cf.reason
                );
            } else {
                info!(">>> {who} somehow sent close message without CloseFrame");
            }
            return ControlFlow::Break(());
        }
        Message::Pong(v) => {
            info!(">>> {who} sent pong with {v:?}");
        }
        Message::Ping(v) => {
            info!(">>> {who} sent ping with {v:?}");
        }
    }
    ControlFlow::Continue(())
}

fn format_mac_address(mac: &str) -> String {
    mac.chars()
        .collect::<Vec<char>>()
        .chunks(2)
        .map(|chunk| chunk.iter().collect::<String>())
        .collect::<Vec<String>>()
        .join(":")
}
