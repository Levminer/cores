use ezrtc::host::EzRTCHost;
use ezrtc::socket::DataChannelHandler;
use futures_util::{SinkExt, StreamExt};
use log::{error, warn, LevelFilter};
use once_cell::sync::Lazy;
use serde::{Deserialize, Serialize};
use simplelog::{ColorChoice, CombinedLogger, Config, TermLogger, TerminalMode, WriteLogger};
use std::fs::File;
use std::path::Path;
use std::sync::{Arc, RwLock};
use std::time::Duration;
use tokio::sync::mpsc::{self, UnboundedSender};
use tokio::task::JoinHandle;
use tokio_stream::wrappers::UnboundedReceiverStream;
use tokio_tungstenite::{connect_async, tungstenite::protocol::Message};
use tokio_util::sync::CancellationToken;
use webrtc::data_channel::data_channel_state::RTCDataChannelState;
use webrtc::data_channel::RTCDataChannel;
use webrtc::ice_transport::ice_credential_type::RTCIceCredentialType;
use webrtc::ice_transport::ice_server::RTCIceServer;

#[derive(Serialize, Deserialize, Debug)]
pub struct Settings {
    #[serde(rename = "connectionCode")]
    pub connection_code: String,
}

pub async fn host() {
    // Load settings
    let program_data = Path::new("C:\\ProgramData");
    let file = std::fs::read_to_string(program_data.join("Cores").join("settings.json")).unwrap();
    let settings: Result<Settings, _> = serde_json::from_str(&file);

    // Logger
    CombinedLogger::init(vec![
        TermLogger::new(
            LevelFilter::Info,
            Config::default(),
            TerminalMode::Mixed,
            ColorChoice::Auto,
        ),
        WriteLogger::new(
            LevelFilter::Info,
            Config::default(),
            File::create(program_data.join("Cores").join("rtc.log")).unwrap(),
        ),
    ])
    .unwrap();

    let session_id = String::from(settings.unwrap().connection_code);
    let local_ws_url = url::Url::parse("ws://localhost:5391").unwrap();
    let data = Arc::new(RwLock::new(String::new()));
    let data_clone = Arc::clone(&data);

    // Connect to the local websocket server
    let (local_ws_stream, _response) = connect_async(local_ws_url)
        .await
        .expect("Failed to connect");
    println!("WebSocket handshake has been successfully completed");

    // Create a channel to send and receive WS messages from the local websocket server
    let (mut local_ws_write, mut local_ws_read) = local_ws_stream.split();
    let (local_write, local_read) = mpsc::unbounded_channel();
    let mut local_read = UnboundedReceiverStream::new(local_read);

    // Send message to the local websocket server when the channel is receiving messages
    tokio::task::spawn(async move {
        while let Some(message) = local_read.next().await {
            local_ws_write.send(message).await.unwrap();
        }
    });

    let local_writer = Arc::new(local_write.clone());

    // Read messages from the local websocket server
    tokio::task::spawn(async move {
        while let Some(message) = local_ws_read.next().await {
            // convert message to string
            let msg_ok = match message {
                Ok(msg) => msg,
                Err(e) => {
                    error!("Error converting message to string: {:?}", e);
                    continue;
                }
            };

            let text = match msg_ok.to_text() {
                Ok(text) => text,
                Err(e) => {
                    error!("Error converting message to text: {:?}", e);
                    continue;
                }
            }
            .to_string();

            let mut global_data = data_clone.write().unwrap();
            *global_data = text.clone();
        }
    });

    // STUN and TURN servers
    let ice_servers = vec![
        RTCIceServer {
            urls: vec!["stun:stun.relay.metered.ca:80".to_owned()],
            ..Default::default()
        },
        RTCIceServer {
            urls: vec!["turn:standard.relay.metered.ca:80".to_owned()],
            username: "34a987bde7c718428704bde7".to_owned(),
            credential: "hZA1e3RHAhw70JoP".to_owned(),
            credential_type: RTCIceCredentialType::Password,
        },
        RTCIceServer {
            urls: vec!["turn:standard.relay.metered.ca:443".to_owned()],
            username: "34a987bde7c718428704bde7".to_owned(),
            credential: "hZA1e3RHAhw70JoP".to_owned(),
            credential_type: RTCIceCredentialType::Password,
        },
    ];

    struct MyDataChannelHandler {
        local_writer: Arc<UnboundedSender<tokio_tungstenite::tungstenite::Message>>,
        data: Arc<RwLock<String>>,
    }

    impl DataChannelHandler for MyDataChannelHandler {
        fn handle_data_channel_open(&self, dc: Arc<RTCDataChannel>) {
            warn!("Data channel opened!");

            let data_clone = self.data.clone();
            tokio::spawn(async move {
                loop {
                    if dc.ready_state() == RTCDataChannelState::Open {
                        let text = data_clone.read().unwrap().to_string();

                        dc.send_text(text).await.unwrap();
                    }

                    tokio::time::sleep(Duration::from_secs(2)).await;
                }
            });
        }

        fn handle_data_channel_message(&self, message: String) {
            self.local_writer.send(Message::Text(message)).unwrap();
        }
    }

    // Start the connection
    let host = EzRTCHost::new(
        "wss://rtc-usw.levminer.com/one-to-many".to_string(),
        session_id,
        ice_servers,
        Arc::new(Box::new(MyDataChannelHandler {
            local_writer: local_writer.clone(),
            data: data.clone(),
        })),
    )
    .await;

    loop {
        tokio::time::sleep(Duration::from_secs(100)).await;
    }
}

static CANCELLATION_TOKEN: Lazy<CancellationToken> = Lazy::new(|| CancellationToken::new());

#[no_mangle]
pub extern "C" fn start() {
    let token_clone = CANCELLATION_TOKEN.clone();

    tokio::runtime::Builder::new_multi_thread()
        .enable_all()
        .build()
        .unwrap()
        .block_on(async {
            let mut task_handle: JoinHandle<()> = tokio::spawn(async move {
                host().await;
            });

            tokio::select! {
                _ = &mut task_handle => {}
                _ = token_clone.cancelled() => {
                    task_handle.abort();
                    warn!("Host task cancelled");
                }
            };
        })
}

#[no_mangle]
pub extern "C" fn stop() {
    let token_clone = CANCELLATION_TOKEN.clone();

    token_clone.cancel();
}
