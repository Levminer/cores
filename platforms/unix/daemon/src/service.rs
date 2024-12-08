use log::{error, info};

pub fn setup_service() {
    // copy current executable to /bin as sudo
    let current_exe = std::env::current_exe().expect("Failed to get current executable");
    let status = std::fs::copy(current_exe, "/bin/coresd");

    if let Err(e) = status {
        error!(
            "Failed to copy executable to /bin, please run as root! Error: {}",
            e
        );
        std::process::exit(1);
    }

    let file_contents = "[Unit]
Description=coresd
After=network.target

[Service]
User=root
ExecStart=/bin/coresd
Restart=always

[Install]
WantedBy=multi-user.target";

    // create service file
    let status = std::fs::write("/etc/systemd/system/coresd.service", file_contents);

    if let Err(e) = status {
        error!(
            "Failed to create coresd service file, please run as root! Error: {}",
            e
        );
        std::process::exit(1);
    }

    // enable service
    std::process::Command::new("sudo")
        .arg("systemctl")
        .arg("enable")
        .arg("coresd")
        .status()
        .expect("Failed to enable coresd service");

    // start service
    std::process::Command::new("sudo")
        .arg("systemctl")
        .arg("start")
        .arg("coresd")
        .status()
        .expect("Failed to start coresd service");

    info!("Service created successfully, please run `sudo systemctl status coresd` for more information");
    std::process::exit(0);
}
