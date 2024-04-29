use directories::BaseDirs;
use serde::{Deserialize, Serialize};
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

    // check if appdata exists, return sample settings if not
    let dir = match BaseDirs::new() {
        Some(base_dirs) => base_dirs,
        None => return sample_settings,
    };

    let appdata = dir.config_dir();

    // Check if folder exists
    if !appdata.join("Cores").exists() {
        std::fs::create_dir(appdata.join("Cores")).unwrap();
    }

    // Check if file exists, and return it
    if !appdata.join("Cores").join("settings.json").exists() {
        std::fs::write(
            appdata.join("Cores").join("settings.json"),
            serde_json::to_string(&sample_settings).unwrap(),
        )
        .unwrap();

        return sample_settings;
    } else {
        let file = std::fs::read_to_string(appdata.join("Cores").join("settings.json")).unwrap();
        let settings: Result<Settings, _> = serde_json::from_str(&file);

        match settings {
            Ok(settings) => {
                return settings;
            }
            Err(_) => {
                std::fs::write(
                    appdata.join("Cores").join("settings.json"),
                    serde_json::to_string(&sample_settings).unwrap(),
                )
                .unwrap();

                return sample_settings;
            }
        };
    }
}

#[tauri::command]
pub fn set_settings(settings: String) {
    let dir = match BaseDirs::new() {
        Some(base_dirs) => base_dirs,
        None => return,
    };

    let appdata = dir.config_dir();

    // replace file
    std::fs::write(appdata.join("Cores").join("settings.json"), settings).unwrap();
}
