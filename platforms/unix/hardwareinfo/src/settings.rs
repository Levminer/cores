use log::info;
use serde::{Deserialize, Serialize};
use uuid::Uuid;

const fn default_value() -> u32 {
    3
}

const fn default_false() -> bool {
    false
}

const fn default_true() -> bool {
    true
}

const fn default_connection_codes() -> Vec<ConnectionCode> {
    Vec::new()
}

fn default_connection_url() -> String {
    "rtc-usw.coresmonitor.com".to_string()
}

pub fn default_connection_code() -> String {
    let id: String = Uuid::new_v4()
        .to_string()
        .replace("-", "")
        .chars()
        .take(16)
        .collect();

    format!("crs_{}", id)
}

pub fn default_user_id() -> String {
    let id: String = Uuid::new_v4()
        .to_string()
        .replace("-", "")
        .chars()
        .take(10)
        .collect();

    format!("user_{}", id)
}

#[derive(Serialize, Deserialize, Debug, Clone)]
pub struct ConnectionCode {
    pub name: String,
    pub code: String,
    pub mac: String,
}

#[derive(Serialize, Deserialize, Debug, Clone)]
pub struct DefaultDevices {
    pub gpu: String,
    pub network: String,
    pub storage: String,
}

fn default_default_devices() -> DefaultDevices {
    DefaultDevices {
        gpu: "".to_string(),
        network: "".to_string(),
        storage: "".to_string(),
    }
}

#[derive(Serialize, Deserialize, Debug, Clone)]
pub struct Settings {
    #[serde(rename = "interval", default = "default_value")]
    pub interval: u32,
    #[serde(rename = "minimizeToTray", default = "default_true")]
    pub minimize_to_tray: bool,
    #[serde(rename = "remoteConnections", default = "default_false")]
    pub remote_connections: bool,
    #[serde(rename = "connectionCodes", default = "default_connection_codes")]
    pub connection_codes: Vec<ConnectionCode>,
    #[serde(rename = "connectionURL", default = "default_connection_url")]
    pub connection_url: String,
    #[serde(rename = "networkDevices", default = "default_connection_codes")]
    pub network_devices: Vec<ConnectionCode>,
    #[serde(rename = "connectionCode", default = "default_connection_code")]
    pub connection_code: String,
    #[serde(rename = "userId", default = "default_user_id")]
    pub user_id: String,
    #[serde(rename = "defaultDevices", default = "default_default_devices")]
    pub default_devices: DefaultDevices,
}

fn sample_settings() -> Settings {
    Settings {
        interval: 3,
        minimize_to_tray: true,
        remote_connections: false,
        connection_code: default_connection_code(),
        connection_codes: default_connection_codes(),
        connection_url: default_connection_url(),
        network_devices: default_connection_codes(),
        user_id: default_connection_code(),
        default_devices: default_default_devices(),
    }
}

#[cfg(target_os = "windows")]
pub fn get_settings_path() -> std::path::PathBuf {
    std::path::PathBuf::from("C:\\ProgramData")
}

#[cfg(not(target_os = "windows"))]
pub fn get_settings_path() -> std::path::PathBuf {
    use directories::BaseDirs;

    match BaseDirs::new() {
        Some(base_dirs) => {
            return base_dirs.config_dir().to_path_buf();
        }
        None => {
            return std::path::PathBuf::from("/");
        }
    };
}

fn check_if_settings_exits() {
    let sample_settings = sample_settings();

    let program_data = get_settings_path();

    // Check if folder exists
    if !program_data.join("Cores").exists() {
        std::fs::create_dir_all(program_data.join("Cores"))
            .expect("Failed to create settings folder");
    }

    // Check if file exists
    if !program_data.join("Cores").join("settings.json").exists() {
        std::fs::write(
            program_data.join("Cores").join("settings.json"),
            serde_json::to_string(&sample_settings).expect("Failed to convert to JSON"),
        )
        .expect("Failed to create settings file");
    }
}

pub fn get_settings() -> Settings {
    let sample_settings = sample_settings();

    info!("Getting settings");

    let program_data = get_settings_path();

    check_if_settings_exits();

    let file = std::fs::read_to_string(program_data.join("Cores").join("settings.json"))
        .expect("Failed to read settings file");
    let settings: Result<Settings, _> = serde_json::from_str(&file);

    match settings {
        Ok(settings) => {
            return settings;
        }
        Err(_) => {
            std::fs::write(
                program_data.join("Cores").join("settings.json"),
                serde_json::to_string(&sample_settings).expect("Failed to convert to JSON"),
            )
            .expect("Failed to create a missing settings file");

            return sample_settings;
        }
    };
}

pub fn set_settings(settings: String) {
    let program_data = get_settings_path();

    info!("Setting settings");

    check_if_settings_exits();

    let res = std::fs::write(program_data.join("Cores").join("settings.json"), settings);

    match res {
        Ok(_) => {
            info!("Settings saved successfully");
        }
        Err(e) => {
            info!("Failed to save settings: {}", e);
        }
    }
}
