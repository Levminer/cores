// use cores::settings::get_settings;
use ezrtc::host::IceCandidate;
use futures_util::{SinkExt, StreamExt};
use log::{error, info};
use log::{warn, LevelFilter};
use once_cell::sync::Lazy;
use serde::{Deserialize, Serialize};
use simplelog::{ColorChoice, Config, TermLogger, TerminalMode};
use std::collections::HashMap;
use std::path::Path;
use std::sync::{Arc, Mutex, RwLock};
use std::time::Duration;
use tokio::sync::mpsc;
use tokio::task::JoinHandle;
use tokio_stream::wrappers::UnboundedReceiverStream;
use tokio_tungstenite::{connect_async, tungstenite::protocol::Message};
use tokio_util::sync::CancellationToken;
use wasm_peers_protocol::one_to_many::SignalMessage;
use wasm_peers_protocol::{SessionId, UserId};
use webrtc::api::interceptor_registry::register_default_interceptors;
use webrtc::api::media_engine::MediaEngine;
use webrtc::api::APIBuilder;
use webrtc::data_channel::data_channel_state::RTCDataChannelState;
use webrtc::ice_transport::ice_candidate::RTCIceCandidateInit;
use webrtc::ice_transport::ice_credential_type::RTCIceCredentialType;
use webrtc::ice_transport::ice_server::RTCIceServer;
use webrtc::interceptor::registry::Registry;
use webrtc::peer_connection::configuration::RTCConfiguration;
use webrtc::peer_connection::peer_connection_state::RTCPeerConnectionState;
use webrtc::peer_connection::sdp::session_description::RTCSessionDescription;
use webrtc::peer_connection::signaling_state::RTCSignalingState;

#[derive(Serialize, Deserialize, Debug)]
pub struct Settings {
    #[serde(rename = "connectionCode")]
    pub connection_code: String,
}

pub async fn host() {
    TermLogger::init(
        LevelFilter::Info,
        Config::default(),
        TerminalMode::Mixed,
        ColorChoice::Auto,
    )
    .unwrap();

    // Load settings
    let program_data = Path::new("C:\\ProgramData");
    let file = std::fs::read_to_string(program_data.join("Cores").join("settings.json")).unwrap();
    let settings: Result<Settings, _> = serde_json::from_str(&file);

    let session_id: SessionId = SessionId::new(String::from(settings.unwrap().connection_code));
    let signaling_url = url::Url::parse("wss://rtc-usw.levminer.com/one-to-many").unwrap();
    let local_ws_url = url::Url::parse("ws://localhost:5391").unwrap();
    let data = Arc::new(RwLock::new(String::new()));
    let data_clone = Arc::clone(&data);

    // Connect to the signaling server
    let (ws_stream, _response) = connect_async(signaling_url)
        .await
        .expect("Failed to connect");
    println!("WebSocket handshake has been successfully completed");

    // Create a channel to send and receive WS messages from the signaling server
    let (mut user_ws_write, user_ws_read) = ws_stream.split();
    let (write, read) = mpsc::unbounded_channel();
    let mut read = UnboundedReceiverStream::new(read);

    // Send message to the signaling server when the channel is receiving messages
    tokio::task::spawn(async move {
        while let Some(message) = read.next().await {
            user_ws_write.send(message).await.unwrap();
        }
    });

    let writer = Arc::new(write.clone());

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

    // Send join message
    let join_message = SignalMessage::SessionJoin(session_id, true);
    writer
        .send(Message::Text(serde_json::to_string(&join_message).unwrap()))
        .unwrap();

    // Store peer connections in a map
    let global_peer_connections = Arc::new(Mutex::new(HashMap::new()));
    let global_data_channels = Arc::new(Mutex::new(HashMap::new()));

    // Receive messages from the signaling server
    let read_future = user_ws_read.for_each(|message| async {
        info!("Message received from signaling server: {:?}", message);

        match message {
            Ok(message) => {
                if let Ok(msg) = message.to_text() {
                    match serde_json::from_str::<SignalMessage>(msg) {
                        Ok(request) => match request {
                            SignalMessage::SessionReady(session_id, user_id) => {
                                // Setup WebRTC
                                let mut m = MediaEngine::default();
                                m.register_default_codecs().unwrap();

                                let mut registry = Registry::new();
                                registry = register_default_interceptors(registry, &mut m).unwrap();

                                let api = APIBuilder::new()
                                    .with_media_engine(m)
                                    .with_interceptor_registry(registry)
                                    .build();

                                let config = RTCConfiguration {
                                    ice_servers: vec![RTCIceServer {
                                        urls: vec![
                                            "stun:openrelay.metered.ca:80".to_owned(),
                                            "turn:standard.relay.metered.ca:443".to_owned(),
                                        ],
                                        credential: "8By67N7nOLDIagJk".to_owned(),
                                        username: "2ce7aaf275c1abdef74ec7e3".to_owned(),
                                        credential_type: RTCIceCredentialType::Password,
                                    }],
                                    ..Default::default()
                                };

                                let peer_connection =
                                    Arc::new(api.new_peer_connection(config).await.unwrap());
                                let data_channel = peer_connection
                                    .create_data_channel("testing", None)
                                    .await
                                    .unwrap();

                                let offer = peer_connection.create_offer(None).await.unwrap();

                                peer_connection
                                    .set_local_description(offer.clone())
                                    .await
                                    .unwrap();

                                global_peer_connections
                                    .lock()
                                    .unwrap()
                                    .insert(user_id, peer_connection);
                                global_data_channels
                                    .lock()
                                    .unwrap()
                                    .insert(user_id, data_channel);

                                writer
                                    .send(Message::Text(
                                        serde_json::to_string(&SignalMessage::SdpOffer(
                                            session_id,
                                            user_id,
                                            offer.clone().sdp,
                                        ))
                                        .unwrap(),
                                    ))
                                    .unwrap();
                            }
                            SignalMessage::SdpAnswer(_session_id, user_id, sdp_answer) => {
                                let peer_connection = {
                                    let peer_connections = global_peer_connections.lock().unwrap();
                                    peer_connections.get(&user_id).unwrap().clone()
                                };
                                let data_channel = {
                                    let data_channels = global_data_channels.lock().unwrap();
                                    data_channels.get(&user_id).unwrap().clone()
                                };

                                let answer =
                                    RTCSessionDescription::answer(sdp_answer.clone()).unwrap();

                                if peer_connection.signaling_state()
                                    == RTCSignalingState::HaveLocalOffer
                                {
                                    peer_connection
                                        .set_remote_description(answer)
                                        .await
                                        .unwrap();

                                    warn!("Remote description set");
                                } else {
                                    return;
                                }

                                let dc = Arc::clone(&data_channel);
                                let pc = Arc::clone(&peer_connection);
                                let dcs = Arc::clone(&global_data_channels);
                                let pcs = Arc::clone(&global_peer_connections);
                                peer_connection.on_peer_connection_state_change(Box::new(move |state| {
                                    warn!("State changed => {:?}", state);

                                    let dc2 = Arc::clone(&dc);
                                    let pc2 = Arc::clone(&pc);
                                    let dcs = Arc::clone(&dcs);
                                    let pcs = Arc::clone(&pcs);
                                    match state {
                                        RTCPeerConnectionState::Disconnected => {
                                            tokio::spawn(async move {
                                                pc2.close().await.unwrap();
                                                dc2.close().await.unwrap();

                                                let mut data_channels = dcs.lock().unwrap();
                                                let mut peer_connections = pcs.lock().unwrap();

                                                // Collect keys to remove
                                                let keys_to_remove: Vec<UserId> =
                                                    data_channels.iter().filter(|(_, v)| v.ready_state() == RTCDataChannelState::Closed).map(|(k, _)| k.clone()).collect();

                                                // Remove data channels
                                                for k in keys_to_remove {
                                                    data_channels.remove(&k);
                                                    peer_connections.remove(&k);
                                                }
                                            });
                                        }
                                        _ => {}
                                    }

                                    Box::pin(async move {})
                                }));

                                let dc = Arc::clone(&data_channel);
                                let dt = Arc::clone(&data);
                                data_channel.on_open(Box::new(move || {
                                    info!("Data channel opened");

                                    let dc2 = Arc::clone(&dc);
                                    let dt2 = Arc::clone(&dt);
                                    Box::pin(async move {
                                        loop {
                                            if dc2.ready_state() == RTCDataChannelState::Open {
                                                let text = dt2.read().unwrap().to_string();

                                                dc2.send_text(text).await.unwrap();
                                            }

                                            tokio::time::sleep(Duration::from_secs(2)).await;
                                        }
                                    })
                                }));

                                let lw = Arc::clone(&local_writer);
                                data_channel.on_message(Box::new(move |msg| {
                                    info!("Message received: {:?}", msg);

                                    let msg_text = msg.data.to_vec();

                                    // Forward message to the local ws server
                                    lw.send(Message::Text(String::from_utf8(msg_text).unwrap()))
                                        .unwrap();

                                    Box::pin(async move {})
                                }));
                            }
                            // SignalMessage::SdpOffer(session_id, user_id, sdp_offer) => {}
                            SignalMessage::IceCandidate(session_id, user_id, ice_candidate) => {
                                let peer_connection = {
                                    let peer_connections = global_peer_connections.lock().unwrap();
                                    peer_connections.get(&user_id).unwrap().clone()
                                };

                                info!(
                                    "Ice candidate received: {:?}, {session_id}, {user_id}",
                                    ice_candidate
                                );

                                let candidate =
                                    serde_json::from_str::<IceCandidate>(ice_candidate.as_str())
                                        .unwrap();

                                let candidate_init = RTCIceCandidateInit {
                                    candidate: candidate.candidate,
                                    sdp_mid: candidate.sdp_mid,
                                    sdp_mline_index: candidate.sdp_mline_index,
                                    username_fragment: candidate.username_fragment,
                                };

                                peer_connection
                                    .add_ice_candidate(candidate_init)
                                    .await
                                    .unwrap();
                            }
                            _ => {}
                        },
                        Err(error) => {
                            error!("Error parsing message from server: {:?}", error);
                        }
                    }
                }
            }
            Err(error) => {
                error!("Error receiving message from server: {:?}", error);
            }
        }
    });

    read_future.await;
}

static CANCELLATION_TOKEN: Lazy<CancellationToken> =
    Lazy::new(|| CancellationToken::new());

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

