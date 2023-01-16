use std::ffi::CStr;
use std::ffi::CString;
use std::os::raw::c_char;
use std::path::PathBuf;
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
