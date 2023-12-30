use std::cell::RefCell;
use std::rc::Rc;
use wasm_bindgen::prelude::*;
use wasm_peers::one_to_many::{MiniClient, MiniServer};
use wasm_peers::ConnectionType;
use wasm_peers_protocol::SessionId;
use web_sys::console;

static SIGNALING_SERVER_URL: &str = "wss://ss.levminer.com/one-to-many";
static STUN_SERVER_URL: &str = "stun:openrelay.metered.ca:80";
static TURN_SERVER_URL: &str = "turn:standard.relay.metered.ca:443";
static TURN_SERVER_USERNAME: &str = "2ce7aaf275c1abdef74ec7e3";
static TURN_SERVER_CREDENTIAL: &str = "8By67N7nOLDIagJk";

fn log(message: String) {
    console::log_1(&message.into());
}

#[wasm_bindgen]
pub struct WebRtcHost {
    server: MiniServer,
    open_connections: Rc<RefCell<i32>>,
}

#[wasm_bindgen]
impl WebRtcHost {
    #[wasm_bindgen(constructor)]
    pub fn new(session_id: String) -> Result<WebRtcHost, JsValue> {
        let mut server = MiniServer::new(
            SIGNALING_SERVER_URL,
            SessionId::new(session_id),
            ConnectionType::StunAndTurn {
                stun_urls: STUN_SERVER_URL.to_string(),
                turn_urls: TURN_SERVER_URL.to_string(),
                username: TURN_SERVER_USERNAME.to_string(),
                credential: TURN_SERVER_CREDENTIAL.to_string(),
            },
        )?;
        let open_connections = Rc::new(RefCell::new(0));

        let server_clone = server.clone();
        let server_on_open = {
            let server_open_connections_count = open_connections.clone();
            move |user_id| {
                log(format!("connection to user established: {:?}", user_id));
                *server_open_connections_count.borrow_mut() += 1;
                if *server_open_connections_count.borrow() == 2 {
                    server_clone.send_message_to_all("ping!");
                }
            }
        };
        let server_on_message = {
            move |user_id, message| {
                log(format!(
                    "server received message from client {:?}: {}",
                    user_id, message
                ));
            }
        };
        server.start(server_on_open, server_on_message);

        Ok(WebRtcHost {
            server,
            open_connections,
        })
    }

    pub fn send_message_to_clients(&self, message: &str) {
        self.server.send_message_to_all(message);
    }

    pub fn get_open_connections(&self) -> i32 {
        self.open_connections.borrow().clone()
    }
}

#[wasm_bindgen]
pub struct WebRtcClient {
    client: MiniClient,
    message: Rc<RefCell<String>>,
}

#[wasm_bindgen]
impl WebRtcClient {
    #[wasm_bindgen(constructor)]
    pub fn new(session_id: String) -> Result<WebRtcClient, JsValue> {
        let data = Rc::new(RefCell::new(String::new()));
        let data_clone = data.clone();

        let mut client = MiniClient::new(
            SIGNALING_SERVER_URL,
            SessionId::new(session_id.to_string()),
            ConnectionType::StunAndTurn {
                stun_urls: STUN_SERVER_URL.to_string(),
                turn_urls: TURN_SERVER_URL.to_string(),
                username: TURN_SERVER_USERNAME.to_string(),
                credential: TURN_SERVER_CREDENTIAL.to_string(),
            },
        )?;

        let client_on_open = || {
            log(format!("client connected"));
        };

        let on_message_callback = move |message: String| {
            // log(format!("client received message: {}", message));

            *data.borrow_mut() = message;
        };

        client.start(client_on_open, on_message_callback);

        Ok(WebRtcClient {
            client,
            message: data_clone,
        })
    }

    pub fn get_message(&self) -> String {
        self.message.borrow().clone()
    }

    pub fn send_message_to_host(&self, message: &str) {
        self.client.clone().send_message_to_host(message).unwrap();
    }
}
