use serde::{Deserialize, Serialize};
use std::path::Path;
use uuid::Uuid;

const fn default_value() -> u32 {
    2
}

const fn default_false() -> bool {
    false
}

const fn default_true() -> bool {
    true
}
pub fn default_connection_code() -> String {
    let id: String = Uuid::new_v4()
        .to_string()
        .replace("-", "")
        .chars()
        .take(10)
        .collect();

    format!("crs_{}", id)
}

#[derive(Serialize, Deserialize, Debug)]
pub struct Settings {
    #[serde(rename = "interval", default = "default_value")]
    pub interval: u32,
    #[serde(rename = "minimizeToTray", default = "default_true")]
    pub minimize_to_tray: bool,
    #[serde(rename = "launchOnStartup", default = "default_false")]
    pub launch_on_startup: bool,
    #[serde(rename = "remoteConnections", default = "default_false")]
    pub remote_connections: bool,
    #[serde(rename = "connectionCode", default = "default_connection_code")]
    pub connection_code: String,
    pub version: Option<u8>,
}

fn check_if_settings_exits() {
    let sample_settings = Settings {
        version: Some(1),
        interval: 2,
        minimize_to_tray: true,
        launch_on_startup: false,
        remote_connections: false,
        connection_code: default_connection_code(),
    };

    let program_data = Path::new("C:\\ProgramData");

    // Check if folder exists
    if !program_data.join("Cores").exists() {
        std::fs::create_dir(program_data.join("Cores")).unwrap();
    }

    // Check if file exists
    if !program_data.join("Cores").join("settings.json").exists() {
        std::fs::write(
            program_data.join("Cores").join("settings.json"),
            serde_json::to_string(&sample_settings).unwrap(),
        )
        .unwrap();
    }
}

#[tauri::command]
pub fn get_settings() -> Settings {
    let sample_settings = Settings {
        version: Some(1),
        interval: 2,
        minimize_to_tray: true,
        launch_on_startup: false,
        remote_connections: false,
        connection_code: default_connection_code(),
    };

    println!("Getting settings");

    let program_data = Path::new("C:\\ProgramData");

    check_if_settings_exits();

    let file = std::fs::read_to_string(program_data.join("Cores").join("settings.json")).unwrap();
    let settings: Result<Settings, _> = serde_json::from_str(&file);

    match settings {
        Ok(settings) => {
            return settings;
        }
        Err(_) => {
            std::fs::write(
                program_data.join("Cores").join("settings.json"),
                serde_json::to_string(&sample_settings).unwrap(),
            )
            .unwrap();

            return sample_settings;
        }
    };
}

#[tauri::command]
pub fn set_settings(settings: String) {
    let program_data = Path::new("C:\\ProgramData");

    println!("Setting settings");

    check_if_settings_exits();

    std::fs::write(program_data.join("Cores").join("settings.json"), settings).unwrap();
}
