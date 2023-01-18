use powershell_script;
use std::ffi::CStr;
use std::ffi::CString;
use std::os::raw::c_char;
use std::process::Command;
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
