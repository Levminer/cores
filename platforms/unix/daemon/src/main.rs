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
use hardwareinfo::{refresh_hardware_info, Data, HardwareInfo, Networks, Nvml, System};
use log::{error, info, warn, LevelFilter};
use serde::{Deserialize, Serialize};
use simplelog::{ColorChoice, CombinedLogger, Config, TermLogger, TerminalMode};
use std::borrow::Cow;
use std::net::SocketAddr;
use std::ops::ControlFlow;
use std::sync::Arc;
use tower_http::{
    cors::{Any, CorsLayer},
    trace::{DefaultMakeSpan, TraceLayer},
};
use webrtc::data_channel::data_channel_state::RTCDataChannelState;
use webrtc::data_channel::RTCDataChannel;
use webrtc::ice_transport::ice_server::RTCIceServer;

#[derive(Serialize, Deserialize)]
struct NetworkData {
    pub r#type: String,
    pub data: HardwareInfo,
}

struct AppState {
    hardware_info_receiver: async_channel::Receiver<HardwareInfo>,
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

    // Hardware info channel
    let (s, r) = async_channel::unbounded();

    let mut data = Data {
        first_run: true,
        sys: System::new_all(),
        network: Networks::new_with_refreshed_list(),
        hw_info: HardwareInfo::default(),
        nvml: Nvml::init(),
        nvml_available: true,
    };

    let app_state = Arc::new(AppState {
        hardware_info_receiver: r.clone(),
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
        .with_state(app_state);

    let listener = tokio::net::TcpListener::bind("127.0.0.1:5390")
        .await
        .unwrap();

    info!(
        "HTTP server listening on http://{}",
        listener.local_addr().unwrap()
    );

    // Refresh hardware info every 5 seconds
    let hw_task = tokio::spawn(async move {
        loop {
            data.sys.refresh_all();
            std::thread::sleep(hardwareinfo::MINIMUM_CPU_UPDATE_INTERVAL);
            data.sys.refresh_all();
            data.network.refresh();
            refresh_hardware_info(&mut data);

            s.send(data.hw_info.clone()).await.unwrap();

            tokio::time::sleep(std::time::Duration::from_secs(5)).await;
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
        }

        impl DataChannelHandler for MyDataChannelHandler {
            fn handle_data_channel_open(&self, dc: Arc<RTCDataChannel>) {
                warn!("Data channel opened!");

                let receiver = self.receiver.clone();

                tokio::spawn(async move {
                    loop {
                        if dc.ready_state() == RTCDataChannelState::Open {
                            let hw_message = receiver.recv().await.unwrap();
                            let network_data = NetworkData {
                                r#type: "data".to_string(),
                                data: hw_message.clone(),
                            };

                            dc.send_text(serde_json::to_string(&network_data).unwrap())
                                .await
                                .unwrap();
                        }

                        tokio::time::sleep(std::time::Duration::from_secs(2)).await;
                    }
                });
            }

            fn handle_data_channel_message(&self, message: String) {
                warn!("Data channel message received: {:?}", message);
            }
        }

        // Start the connection
        let _host = EzRTCHost::new(
            "wss://rtc-usw.levminer.com/one-to-many".to_string(),
            //"ws://localhost:5391".to_string(),
            "crs_4444".to_string(),
            ice_servers,
            Arc::new(Box::new(MyDataChannelHandler {
                receiver: r.clone(),
            })),
        )
        .await;

        info!("RTC started");

        loop {
            tokio::time::sleep(std::time::Duration::from_secs(100)).await;
        }
    });

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

    // Spawn a sender task to send data to the client
    let mut send_task = tokio::spawn(async move {
        let receiver = state.hardware_info_receiver.clone();

        loop {
            let hw_message = receiver.recv().await.unwrap();
            let network_data = NetworkData {
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

            tokio::time::sleep(std::time::Duration::from_secs(5)).await;
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
