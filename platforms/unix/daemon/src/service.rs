use hardwareinfo::settings::{get_settings, set_settings};
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

    // prompt for connection code
    let mut current_settings = get_settings();
    let mut connection_code = current_settings.connection_code.clone();

    println!(
        "Please enter a connection code (press enter to keep the current \"{connection_code}\"): "
    );

    std::io::stdin()
        .read_line(&mut connection_code)
        .expect("Failed to read input");

    current_settings.connection_code = connection_code.trim().to_string();
    set_settings(
        serde_json::to_string(&current_settings).expect("Failed to convert settings to JSON"),
    );

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

    // enable service auto-start
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
    info!("NOTE: Your current settings file was copied to `/root/.config/Cores/settings.json` edit it there to change settings.");
    std::process::exit(0);
}
