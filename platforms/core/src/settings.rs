use hardwareinfo::settings::{Settings, WindowState};

#[tauri::command]
pub fn get_settings() -> Settings {
    return hardwareinfo::settings::get_settings()
}

#[tauri::command]
pub fn set_settings(settings: String) {
    return hardwareinfo::settings::set_settings(settings)
}

#[tauri::command]
pub fn save_window_state(window_state: WindowState) {
    let mut settings = hardwareinfo::settings::get_settings();
    settings.window_state = window_state;
    hardwareinfo::settings::set_settings(
        serde_json::to_string(&settings).expect("Failed to convert settings to JSON"),
    );
}
