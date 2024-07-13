use hardwareinfo::{refresh_hardware_info, Data, HardwareInfo};
use sysinfo::{Networks, System};

fn main() {
    let mut data = Data {
        first_run: true,
        sys: System::new_all(),
        network: Networks::new_with_refreshed_list(),
        hw_info: HardwareInfo::default(),
    };

    loop {
        data.sys.refresh_all();
        std::thread::sleep(sysinfo::MINIMUM_CPU_UPDATE_INTERVAL);
        data.sys.refresh_all();
        data.network.refresh();

        refresh_hardware_info(&mut data);
        println!("{:#?}", data.hw_info);

        std::thread::sleep(std::time::Duration::from_secs(5));
    }
}
