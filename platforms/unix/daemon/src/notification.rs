use hardwareinfo::settings::{Notifications, Settings};
use hardwareinfo::HardwareInfo;
use log::{error, info};
use serde_json::Value;
use std::collections::HashMap;
use std::time::{Duration, Instant};

const COOLDOWN_PERIOD: Duration = Duration::from_secs(15 * 60);
const NOTIFICATION_URL: &str = "https://www.coresmonitor.com/api/notification";

pub struct Notifier {
    client: Option<reqwest::Client>,
    start_times: HashMap<String, Instant>,
    last_fired: HashMap<String, Instant>,
}

fn get_key(notification: &Notifications) -> String {
    format!(
        "{}|{}|{}|{}",
        notification.json, notification.condition, notification.value, notification.seconds
    )
}

// Resolve a path like `gpu.cards[0].temperature[0].value` to a number
fn resolve_path(root: &Value, path: &str) -> Option<f64> {
    let mut current = root;

    for segment in path.split('.').map(str::trim).filter(|s| !s.is_empty()) {
        let mut remaining = segment;

        while !remaining.is_empty() {
            let Some(bracket_index) = remaining.find('[') else {
                current = current.as_object()?.get(remaining)?;
                break;
            };

            let property_name = remaining[..bracket_index].trim();
            if !property_name.is_empty() {
                current = current.as_object()?.get(property_name)?;
            }

            let closing_bracket_index = bracket_index + remaining[bracket_index..].find(']')?;
            let index: usize = remaining[bracket_index + 1..closing_bracket_index]
                .parse()
                .ok()?;

            current = current.as_array()?.get(index)?;
            remaining = &remaining[closing_bracket_index + 1..];
        }
    }

    current.as_f64()
}

impl Notifier {
    pub fn new() -> Self {
        let client = match reqwest::Client::builder()
            .timeout(Duration::from_secs(10))
            .build()
        {
            Ok(client) => Some(client),
            Err(e) => {
                error!(
                    "Failed to create HTTP client, notifications disabled: {}",
                    e
                );
                None
            }
        };

        Self {
            client,
            start_times: HashMap::new(),
            last_fired: HashMap::new(),
        }
    }

    // Check every notification rule against the latest hardware info and send the ones that fire
    pub fn check(&mut self, hw_info: &HardwareInfo, settings: &Settings) {
        let Some(client) = self.client.clone() else {
            return;
        };

        if settings.notifications.is_empty() {
            return;
        }

        let hardware_json = match serde_json::to_value(hw_info) {
            Ok(hardware_json) => hardware_json,
            Err(e) => {
                error!("Failed to serialize hardware info: {}", e);
                return;
            }
        };

        for notification in &settings.notifications {
            if !self.should_trigger(&hardware_json, notification) {
                continue;
            }

            info!("Notification fired: {:?}", notification);

            let title = "Hardware notification".to_string();
            let body = format!(
                "{} was {} than {} for {} seconds.",
                notification.json, notification.condition, notification.value, notification.seconds
            );

            tokio::spawn(send(
                client.clone(),
                settings.connection_code.clone(),
                notification.json.clone(),
                title,
                body,
            ));
        }
    }

    fn should_trigger(&mut self, hardware_json: &Value, notification: &Notifications) -> bool {
        let key = get_key(notification);

        let Some(current_value) = resolve_path(hardware_json, &notification.json) else {
            self.start_times.remove(&key);
            return false;
        };

        let condition_met = match notification.condition.as_str() {
            "higher" => current_value > notification.value as f64,
            "lower" => current_value < notification.value as f64,
            _ => false,
        };

        if !condition_met {
            self.start_times.remove(&key);
            return false;
        }

        let now = Instant::now();

        if let Some(fired_at) = self.last_fired.get(&key) {
            if now.duration_since(*fired_at) < COOLDOWN_PERIOD {
                return false;
            }
        }

        let Some(start_time) = self.start_times.get(&key) else {
            self.start_times.insert(key, now);
            return false;
        };

        if now.duration_since(*start_time) < Duration::from_secs(notification.seconds as u64) {
            return false;
        }

        self.start_times.remove(&key);
        self.last_fired.insert(key, now);
        true
    }
}

async fn send(
    client: reqwest::Client,
    connection_code: String,
    json: String,
    title: String,
    body: String,
) {
    let response = client
        .post(NOTIFICATION_URL)
        .bearer_auth(connection_code)
        .json(&serde_json::json!({ "title": title, "body": body }))
        .send()
        .await
        .and_then(|response| response.error_for_status());

    match response {
        Ok(_) => info!("Notification sent to API: {}", json),
        Err(e) => error!("Failed to send notification: {}", e),
    }
}
