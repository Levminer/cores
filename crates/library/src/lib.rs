use auto_launch::AutoLaunchBuilder;
use directories::BaseDirs;
use powershell_script;
use serde::{Deserialize, Serialize};
use std::ffi::CStr;
use std::ffi::CString;
use std::os::raw::c_char;
use uuid::Uuid;
use wfd::DialogParams;

#[no_mangle]
pub extern "C" fn dialog(c_buf: *const c_char) -> *const c_char {
    let c_str: &CStr = unsafe { CStr::from_ptr(c_buf) };
    let str_slice: &str = c_str.to_str().unwrap();

    let params = DialogParams {
        title: "Select file",
        file_types: vec![("Text file", "*.txt")],
        default_extension: "txt",
        file_name: str_slice,
        ..Default::default()
    };

    let dialog_result = wfd::save_dialog(params);

    let res = match dialog_result {
        Ok(path) => path
            .selected_file_path
            .into_os_string()
            .into_string()
            .unwrap(),
        Err(_) => String::from("error"),
    };

    return CString::new(res).unwrap().into_raw();
}

#[no_mangle]
pub extern "C" fn getGPUInfo() -> *const c_char {
    let driver = powershell_script::run(
        "Get-WmiObject -class Win32_VideoController | Select -Expand DriverDate",
    )
    .unwrap()
    .stdout()
    .unwrap();

    return CString::new(driver.trim()).unwrap().into_raw();
}

#[no_mangle]
pub extern "C" fn getOSInfo() -> *const c_char {
    let caption = powershell_script::run(
        "Get-WmiObject -class Win32_OperatingSystem | Select -Expand Caption",
    )
    .unwrap()
    .stdout()
    .unwrap();

    let version = powershell_script::run(
        "Get-WmiObject -class Win32_OperatingSystem | Select -Expand Version",
    )
    .unwrap()
    .stdout()
    .unwrap();

    let mut arch = std::env::consts::ARCH;

    arch = match arch {
        "x86_64" => "x64",
        "aarch64" => "arm64",
        _ => arch,
    };

    let returning = format!(
        "{} {} {}",
        caption.replace("Microsoft", "").trim(),
        arch,
        version.trim(),
    );

    return CString::new(returning).unwrap().into_raw();
}

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

#[no_mangle]
pub extern "C" fn getSettings() -> *const c_char {
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
        None => {
            return CString::new(serde_json::to_string(&sample_settings).unwrap())
                .unwrap()
                .into_raw();
        }
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

        return CString::new(serde_json::to_string(&sample_settings).unwrap())
            .unwrap()
            .into_raw();
    } else {
        let file = std::fs::read_to_string(appdata.join("Cores").join("settings.json")).unwrap();
        let settings: Result<Settings, _> = serde_json::from_str(&file);

        match settings {
            Ok(settings) => {
                return CString::new(serde_json::to_string(&settings).unwrap())
                    .unwrap()
                    .into_raw();
            }
            Err(_) => {
                std::fs::write(
                    appdata.join("Cores").join("settings.json"),
                    serde_json::to_string(&sample_settings).unwrap(),
                )
                .unwrap();

                return CString::new(serde_json::to_string(&sample_settings).unwrap())
                    .unwrap()
                    .into_raw();
            }
        };
    }
}

#[no_mangle]
pub extern "C" fn setSettings(settings: *const c_char) {
    let c_str: &CStr = unsafe { CStr::from_ptr(settings) };
    let str_slice: &str = c_str.to_str().unwrap();

    let dir = match BaseDirs::new() {
        Some(base_dirs) => base_dirs,
        None => return,
    };

    let appdata = dir.config_dir();

    // Replace file
    std::fs::write(appdata.join("Cores").join("settings.json"), str_slice).unwrap();
}

#[no_mangle]
pub extern "C" fn autoLaunch(c_buf: *const c_char) {
    let c_str: &CStr = unsafe { CStr::from_ptr(c_buf) };
    let str_slice: &str = c_str.to_str().unwrap();

    let auto = AutoLaunchBuilder::new()
        .set_app_name("Cores")
        .set_app_path(str_slice)
        .set_use_launch_agent(false)
        .set_args(&["--minimized"])
        .build()
        .unwrap();

    if auto.is_enabled().unwrap() {
        auto.disable().unwrap();
    } else {
        auto.enable().unwrap();
    }
}
