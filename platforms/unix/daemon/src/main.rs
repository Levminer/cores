use axum::extract::connect_info::ConnectInfo;
use axum::extract::ws::CloseFrame;
use axum::extract::State;
use axum::{
    extract::ws::{Message, WebSocket, WebSocketUpgrade},
    http::Method,
    response::IntoResponse,
    routing::get,
    Router,
};
use ezrtc::host::EzRTCHost;
use ezrtc::socket::DataChannelHandler;
use futures::{sink::SinkExt, stream::StreamExt};
use hardwareinfo::settings::{get_settings, Settings};
use hardwareinfo::{refresh_hardware_info, Data, HardwareInfo, Networks, Nvml, System};
use log::{error, info, warn, LevelFilter};
use serde::{Deserialize, Serialize};
use simplelog::{ColorChoice, CombinedLogger, Config, TermLogger, TerminalMode};
use std::borrow::Cow;
use std::net::SocketAddr;
use std::ops::ControlFlow;
use std::process::Command;
use std::str::FromStr;
use std::sync::{Arc, Mutex};
use tower_http::{
    cors::{Any, CorsLayer},
    trace::{DefaultMakeSpan, TraceLayer},
};
use webrtc::data_channel::data_channel_state::RTCDataChannelState;
use webrtc::data_channel::RTCDataChannel;
use webrtc::ice_transport::ice_server::RTCIceServer;
use wol::{send_wol, MacAddr};

#[derive(Serialize, Deserialize)]
pub struct GenericMessage<T> {
    pub r#type: String,
    pub data: T,
}

pub struct AppState {
    hardware_info_receiver: async_channel::Receiver<HardwareInfo>,
    last_60s_hardware_info: Mutex<Vec<HardwareInfo>>,
    last_60m_hardware_info: Mutex<Vec<HardwareInfo>>,
    settings: Settings,
}

#[tokio::main]
async fn main() {
    // Logger
    CombinedLogger::init(vec![TermLogger::new(
        LevelFilter::Info,
        Config::default(),
        TerminalMode::Mixed,
        ColorChoice::Auto,
    )])
    .unwrap();

    // Get settings
    let settings = get_settings();
    info!("Connection code: {:?}", settings.connection_code);

    // Hardware info channel
    let (s, r) = async_channel::unbounded();

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
        hardware_info_receiver: r.clone(),
        last_60s_hardware_info: Mutex::new(Vec::new()),
        last_60m_hardware_info: Mutex::new(Vec::new()),
        settings: settings.clone(),
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

    // Refresh hardware info every 5 seconds
    let app_state_clone = app_state.clone();
    let hw_task = tokio::spawn(async move {
        loop {
            data.sys.refresh_all();
            std::thread::sleep(hardwareinfo::MINIMUM_CPU_UPDATE_INTERVAL);
            data.sys.refresh_all();
            data.network.refresh();
            refresh_hardware_info(&mut data);

            s.send(data.hw_info.clone()).await.unwrap();

            if app_state_clone.last_60s_hardware_info.lock().unwrap().len() > 60 {
                app_state_clone
                    .last_60s_hardware_info
                    .lock()
                    .unwrap()
                    .remove(0);
                app_state_clone
                    .last_60s_hardware_info
                    .lock()
                    .unwrap()
                    .push(data.hw_info.clone());
            } else {
                app_state_clone
                    .last_60s_hardware_info
                    .lock()
                    .unwrap()
                    .push(data.hw_info.clone());
            }

            tokio::time::sleep(std::time::Duration::from_secs(settings.interval as u64)).await;
        }
    });

    // Save last 60m hardware info
    let app_state_clone = app_state.clone();
    let rcv = r.clone();
    let last_60m_hardware_info_task = tokio::spawn(async move {
        loop {
            let data = rcv.recv().await.unwrap();

            if app_state_clone.last_60m_hardware_info.lock().unwrap().len() > 60 {
                app_state_clone
                    .last_60m_hardware_info
                    .lock()
                    .unwrap()
                    .remove(0);
                app_state_clone
                    .last_60m_hardware_info
                    .lock()
                    .unwrap()
                    .push(data);
            } else {
                app_state_clone
                    .last_60m_hardware_info
                    .lock()
                    .unwrap()
                    .push(data);
            }

            tokio::time::sleep(std::time::Duration::from_secs(60)).await;
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
    let rtc_task = tokio::spawn(async move {
        // Define your STUN and TURN servers here
        let ice_servers = vec![RTCIceServer {
            urls: vec!["stun:stun.cloudflare.com:3478".to_owned()],
            ..Default::default()
        }];

        // Define your data channel handler
        struct MyDataChannelHandler {
            receiver: async_channel::Receiver<HardwareInfo>,
            state: Arc<AppState>,
        }

        impl DataChannelHandler for MyDataChannelHandler {
            fn handle_data_channel_open(&self, dc: Arc<RTCDataChannel>) {
                warn!("Data channel opened!");

                let receiver = self.receiver.clone();
                let state = self.state.clone();

                tokio::spawn(async move {
                    if dc.ready_state() == RTCDataChannelState::Open {
                        let last60s_hardware_info =
                            state.last_60s_hardware_info.lock().unwrap().clone();
                        let last60m_hardware_info =
                            state.last_60m_hardware_info.lock().unwrap().clone();

                        // get every third element from the last 60s hardware info
                        let last60s_hardware_info = last60s_hardware_info
                            .iter()
                            .step_by(3)
                            .cloned()
                            .collect::<Vec<HardwareInfo>>();
                        let last60m_hardware_info = last60m_hardware_info
                            .iter()
                            .step_by(3)
                            .cloned()
                            .collect::<Vec<HardwareInfo>>();

                        // Send initial data
                        let hw_message = receiver.recv().await.unwrap();
                        let network_data = GenericMessage::<HardwareInfo> {
                            r#type: "initialData".to_string(),
                            data: hw_message.clone(),
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

                        // Send data every 2 second
                        loop {
                            let hw_message = receiver.recv().await.unwrap();
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

                            tokio::time::sleep(std::time::Duration::from_secs(2)).await;
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
        }

        // Start the connection
        let _host = EzRTCHost::new(
            "wss://rtc-usw.levminer.com/one-to-many".to_string(),
            settings.connection_code,
            ice_servers,
            Arc::new(Box::new(MyDataChannelHandler {
                receiver: r.clone(),
                state: app_state.clone(),
            })),
        )
        .await;

        info!("RTC started");

        loop {
            tokio::time::sleep(std::time::Duration::from_secs(100)).await;
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
        _ = last_60m_hardware_info_task => {
            info!("Last 60s hardware info stopped");
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
    if socket.send(Message::Ping(vec![1, 2, 3])).await.is_ok() {
        info!("Pinged {addr}...");
    } else {
        info!("Could not send ping {addr}!");

        return;
    }

    // Split socket into sender and receiver
    let (mut sender, mut receiver) = socket.split();

    let state2 = state.clone();
    let last60s_hardware_info = state2.last_60s_hardware_info.lock().unwrap().clone();
    let last60m_hardware_info = state2.last_60m_hardware_info.lock().unwrap().clone();

    // Get every third element from the last 60s and 60m hardware info
    let last60s_hardware_info = last60s_hardware_info
        .iter()
        .step_by(3)
        .cloned()
        .collect::<Vec<HardwareInfo>>();
    let last60m_hardware_info = last60m_hardware_info
        .iter()
        .step_by(3)
        .cloned()
        .collect::<Vec<HardwareInfo>>();

    // Send last 60s hardware info
    for hw_info in last60s_hardware_info {
        let network_data = GenericMessage::<HardwareInfo> {
            r#type: "secondsData".to_string(),
            data: hw_info.clone(),
        };

        if sender
            .send(Message::Text(serde_json::to_string(&network_data).unwrap()))
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
            .send(Message::Text(serde_json::to_string(&network_data).unwrap()))
            .await
            .is_err()
        {
            break;
        }
    }

    // Spawn a sender task to send data to the client
    let mut send_task = tokio::spawn(async move {
        let receiver = state.hardware_info_receiver.clone();

        loop {
            let hw_message = receiver.recv().await.unwrap();
            let network_data = GenericMessage::<HardwareInfo> {
                r#type: "data".to_string(),
                data: hw_message.clone(),
            };

            if sender
                .send(Message::Text(serde_json::to_string(&network_data).unwrap()))
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
                reason: Cow::from("Goodbye"),
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
