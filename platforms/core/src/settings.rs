use hardwareinfo::settings::Settings;

#[tauri::command]
pub fn get_settings() -> Settings {
    return hardwareinfo::settings::get_settings()
}

#[tauri::command]
pub fn set_settings(settings: String) {
    return hardwareinfo::settings::set_settings(settings)
}
