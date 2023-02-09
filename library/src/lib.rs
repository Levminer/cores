use directories::BaseDirs;
use powershell_script;
use serde::{Deserialize, Serialize};
use std::ffi::CStr;
use std::ffi::CString;
use std::os::raw::c_char;
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

#[derive(Serialize, Deserialize, Debug)]
#[repr(C)]
pub struct Settings {
    pub interval: u32,
    #[serde(rename = "minimizeToTray")]
    pub minimize_to_tray: Option<bool>,
}

#[no_mangle]
pub extern "C" fn getSettings() -> Settings {
    let sample_settings = Settings {
        interval: 2,
        minimize_to_tray: Some(true),
    };

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
        let settings: Settings = serde_json::from_str(&file).unwrap();

        return settings;
    }
}

#[no_mangle]
pub extern "C" fn setSettings(settings: Settings) {
    let dir = match BaseDirs::new() {
        Some(base_dirs) => base_dirs,
        None => return,
    };

    let appdata = dir.config_dir();

    // Replace file
    std::fs::write(
        appdata.join("Cores").join("settings.json"),
        serde_json::to_string(&settings).unwrap(),
    )
    .unwrap();
}
