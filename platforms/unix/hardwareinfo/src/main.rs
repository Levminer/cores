use hardwareinfo::{refresh_hardware_info, Data, HardwareInfo};
use nvml_wrapper::Nvml;
use sysinfo::{Networks, System};

fn main() {
    let mut data = Data {
        first_run: true,
        sys: System::new_all(),
        network: Networks::new_with_refreshed_list(),
        hw_info: HardwareInfo::default(),
        nvml: Nvml::init(),
        nvml_available: true,
        interval: 5.0,
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
