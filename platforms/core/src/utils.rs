use std::env;

use serde::{Deserialize, Serialize};
use sysinfo::System;

#[derive(Serialize, Deserialize)]
#[serde(rename_all = "camelCase")]
pub struct SystemInfo {
    pub os_name: String,
    pub os_version: String,
    pub os_arch: String,
    pub cpu_name: String,
    pub total_mem: u64,
    pub tauri_version: String,
}

#[tauri::command]
pub fn system_info() -> SystemInfo {
    let mut sys = System::new_all();
    sys.refresh_all();

    let mut os_name = System::name().unwrap();
    let os_version = tauri_plugin_os::version().to_string();
    let tauri_version = tauri::VERSION.to_string();
    let mut os_arch = env::consts::ARCH.to_string();
    let cpu_name = sys.cpus()[0].brand().to_string();
    let total_mem = sys.total_memory();

    os_name = match os_name.as_str() {
        "Darwin" => "macOS".to_string(),
        _ => os_name,
    };

    os_arch = match os_arch.as_str() {
        "x86_64" => "x64".to_string(),
        "aarch64" => "arm64".to_string(),
        _ => os_arch,
    };

    let res = SystemInfo {
        os_name,
        os_version,
        cpu_name,
        total_mem,
        os_arch,
        tauri_version,
    };

    res.into()
}
