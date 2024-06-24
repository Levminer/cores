use serde::{Deserialize, Serialize};
use std::env;
use sysinfo::System;

#[derive(Serialize, Deserialize, Debug)]
#[serde(rename_all = "camelCase")]
pub struct SystemInfo {
    pub os_name: String,
    pub os_version: String,
    pub os_arch: String,
    pub cpu_name: String,
    pub total_mem: u64,
    pub tauri_version: String,
    pub gpu_name: String,
}

#[derive(Serialize, Deserialize, Debug)]
#[serde(rename_all = "camelCase")]
pub struct PowershellGPUOutput {
    #[serde(rename = "Name")]
    pub name: String,
}

#[tauri::command]
pub fn system_info() -> SystemInfo {
    let mut sys = System::new_all();
    sys.refresh_all();

    let mut os_name = System::name().unwrap_or(String::from("N/A"));
    let os_version = tauri_plugin_os::version().to_string();
    let tauri_version = tauri::VERSION.to_string();
    let mut os_arch = env::consts::ARCH.to_string();
    let cpu_name = sys.cpus()[0].brand().to_string();
    let total_mem = sys.total_memory();

    let gpu_name_json = if let Ok(gpu_name) = powershell_script::run(
        "Get-CimInstance -ClassName Win32_VideoController | Select-Object Name | ConvertTo-Json",
    ) {
        gpu_name.stdout().unwrap_or(String::from("N/A"))
    } else {
        String::from("N/A")
    };

    let gpu_name = match serde_json::from_str::<PowershellGPUOutput>(&gpu_name_json) {
        Ok(parsed) => parsed.name,
        Err(_) => "N/A".to_string(),
    };

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
        gpu_name,
    };

    res.into()
}
