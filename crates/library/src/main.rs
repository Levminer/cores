use lib::*;
use std::ffi::CStr;

fn main() {
    let res0 = getGPUInfo();
    let res1 = getOSInfo();

    let c_str0: &CStr = unsafe { CStr::from_ptr(res0) };
    let str_slice0 = c_str0.to_str().unwrap().to_string();

    let c_str1: &CStr = unsafe { CStr::from_ptr(res1) };
    let str_slice1 = c_str1.to_str().unwrap().to_string();

    println!("{}", str_slice0);
    print!("{}", str_slice1);
}
