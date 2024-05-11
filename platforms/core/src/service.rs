#[tauri::command]
pub fn start_service() {
    let status = runas::Command::new("sc.exe")
        .args(&["start", "CoresService"])
        .status();

    if status.is_ok() {
        println!("Service started");
    } else {
        println!("Service failed to start");
    }
}

#[tauri::command]
pub fn stop_service() {
    let status = runas::Command::new("sc.exe")
        .args(&["stop", "CoresService"])
        .status();

    if status.is_ok() {
        println!("Service stopped");
    } else {
        println!("Service failed to stop");
    }
}

#[tauri::command]
pub fn restart_service() {
    stop_service();
    start_service();
}
