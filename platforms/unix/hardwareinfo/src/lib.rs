use indexmap::IndexMap;
use netdev::{get_default_interface, ip::Ipv4Net, mac::MacAddr, NetworkDevice};
use std::{
    env,
    net::{IpAddr, Ipv4Addr},
};

pub use sysinfo::{Components, Disks, Networks, System, MINIMUM_CPU_UPDATE_INTERVAL};

#[derive(Debug)]
pub struct Data {
    pub sys: System,
    pub network: Networks,
    pub hw_info: HardwareInfo,
    pub first_run: bool,
}

#[derive(Debug, Clone)]
pub struct CoresSensor {
    pub name: String,
    pub value: f64,
    pub min: f64,
    pub max: f64,
}

#[derive(Debug)]
pub struct CoresCPUInfo {
    pub manufacturer_name: String,
    pub socket_designation: String,
    pub max_speed: f64,
    pub core_count: u32,
    pub thread_count: u32,
}

#[derive(Debug)]
pub struct CoresCPU {
    pub name: String,
    pub info: CoresCPUInfo,
    pub max_load: f64,
    pub load: Vec<CoresSensor>,
    pub clock: Vec<CoresSensor>,
}

#[derive(Debug)]
pub struct CoresRAM {
    pub load: Vec<CoresSensor>,
}

#[derive(Debug)]
pub struct CoresOS {
    pub name: String,
}

#[derive(Debug)]
pub struct CoresDisks {
    pub name: String,
    pub total_space: u64,
    pub free_space: u64,
    pub throughput_read: f64,
    pub throughput_write: f64,
}

#[derive(Debug)]
pub struct CoresStorage {
    pub disks: Vec<CoresDisks>,
}

#[derive(Debug)]
pub struct CoresNetInterface {
    pub name: String,
    pub description: String,
    pub mac_address: String,
    pub ip_address: String,
    pub mask: String,
    pub gateway: String,
    pub dns: String,
    pub speed: String,
    pub upload_data: f64,
    pub download_data: f64,
    pub throughput_upload: f64,
    pub throughput_download: f64,
}

#[derive(Debug)]
pub struct CoresNetwork {
    pub interfaces: Vec<CoresNetInterface>,
}

#[derive(Debug)]
pub struct CoresSystem {
    pub network: CoresNetwork,
    pub storage: CoresStorage,
    pub os: CoresOS,
}

#[derive(Debug)]
pub struct HardwareInfo {
    pub cpu: CoresCPU,
    pub ram: CoresRAM,
    pub system: CoresSystem,
}

impl HardwareInfo {
    pub fn default() -> HardwareInfo {
        HardwareInfo {
            ram: CoresRAM { load: Vec::new() },
            cpu: CoresCPU {
                name: "N/A".to_string(),
                max_load: 0.0,
                load: Vec::new(),
                clock: Vec::new(),
                info: CoresCPUInfo {
                    manufacturer_name: "N/A".to_string(),
                    socket_designation: "N/A".to_string(),
                    max_speed: 0.0,
                    core_count: 0,
                    thread_count: 0,
                },
            },
            system: CoresSystem {
                storage: CoresStorage { disks: Vec::new() },
                os: CoresOS {
                    name: "N/A".to_string(),
                },
                network: CoresNetwork {
                    interfaces: Vec::new(),
                },
            },
        }
    }
}

pub fn refresh_hardware_info(data: &mut Data) {
    let gb = 1024_f64.powi(3);
    let mb = 1024_f64.powi(2);

    // OS Info
    if data.first_run {
        let mut os_name = System::name().unwrap_or(String::from("N/A"));
        let os_version = System::os_version().unwrap_or(String::from("N/A"));
        let mut os_arch = env::consts::ARCH.to_string();

        os_name = match os_name.as_str() {
            "Darwin" => "macOS".to_string(),
            _ => os_name,
        };

        os_arch = match os_arch.as_str() {
            "x86_64" => "x64".to_string(),
            "aarch64" => "arm64".to_string(),
            _ => os_arch,
        };

        data.hw_info.system.os.name = format!("{} {} {}", os_name, os_arch, os_version);
    }

    // RAM Info
    let total_memory = data.sys.total_memory() as f64 / gb;
    let used_memory = data.sys.used_memory() as f64 / gb;
    let total_swap = data.sys.total_swap() as f64 / gb;
    let used_swap = data.sys.used_swap() as f64 / gb;
    let ram_used = (used_memory / total_memory) * 100.0;
    let swap_used = (used_swap / total_swap) * 100.0;
    let memory_available = total_memory - used_memory;
    let virtual_memory_available = total_swap - used_swap;

    let mut map = IndexMap::<String, f64>::new();
    map.insert("Memory Used".to_string(), used_memory);
    map.insert("Memory Available".to_string(), memory_available);
    map.insert("Memory".to_string(), ram_used);
    map.insert("Virtual Memory Used".to_string(), used_swap);
    map.insert(
        "Virtual Memory Available".to_string(),
        virtual_memory_available,
    );
    map.insert("Virtual Memory".to_string(), swap_used);

    if data.hw_info.ram.load.len() == 0 {
        for (name, value) in map {
            data.hw_info.ram.load.push(CoresSensor {
                name,
                value,
                min: value,
                max: value,
            });
        }
    } else {
        let mut i = 0;

        for (name, value) in map {
            let prev = &data.hw_info.ram.load[i];

            data.hw_info.ram.load[i] = CoresSensor {
                name,
                value,
                min: if value < prev.min { value } else { prev.min },
                max: if value > prev.max { value } else { prev.max },
            };

            i += 1;
        }
    }

    // CPU Info
    let cpu_info = data.sys.global_cpu_info();
    data.hw_info.cpu.max_load = cpu_info.cpu_usage() as f64;

    if data.first_run {
        data.hw_info.cpu.info.core_count = data.sys.physical_core_count().unwrap() as u32;
        data.hw_info.cpu.info.thread_count = data.sys.cpus().len() as u32;
    }

    let mut cpu_count = 0;
    for cpu in data.sys.cpus() {
        data.hw_info.cpu.name = cpu.brand().to_string();
        let brand = cpu.brand().to_string();
        let name = cpu.name().to_string();

        if data.first_run {
            data.hw_info.cpu.load.push(CoresSensor {
                name: format!("{} #{}", brand, name),
                value: cpu.cpu_usage() as f64,
                min: cpu.cpu_usage() as f64,
                max: cpu.cpu_usage() as f64,
            });

            data.hw_info.cpu.clock.push(CoresSensor {
                name: format!("{} #{}", brand, name),
                value: cpu.frequency() as f64,
                min: cpu.frequency() as f64,
                max: cpu.frequency() as f64,
            });
        } else {
            let prev_load = &data.hw_info.cpu.load[cpu_count];
            let prev_clock = &data.hw_info.cpu.clock[cpu_count];

            data.hw_info.cpu.load[cpu_count] = CoresSensor {
                name: format!("{} #{}", brand, name),
                value: cpu.cpu_usage() as f64,
                min: if (cpu.cpu_usage() as f64) < prev_load.min {
                    cpu.cpu_usage() as f64
                } else {
                    prev_load.min
                },
                max: if (cpu.cpu_usage() as f64) > prev_load.max {
                    cpu.cpu_usage() as f64
                } else {
                    prev_load.max
                },
            };

            data.hw_info.cpu.clock[cpu_count] = CoresSensor {
                name: format!("{} #{}", brand, name),
                value: cpu.frequency() as f64,
                min: if (cpu.frequency() as f64) < prev_clock.min {
                    cpu.frequency() as f64
                } else {
                    prev_clock.min
                },
                max: if (cpu.frequency() as f64) > prev_clock.max {
                    cpu.frequency() as f64
                } else {
                    prev_clock.max
                },
            };

            cpu_count += 1;
        }
    }

    //Display processes ID, name na disk usage:
    // for (_pid, process) in data.sys.processes() {}

    // Disks
    if data.first_run {
        let disks = Disks::new_with_refreshed_list();
        for disk in disks.list() {
            let free_space = disk.available_space() as f64 / gb;
            let total_space = disk.total_space() as f64 / gb;
            let name = disk.name().to_str().unwrap().to_string();

            data.hw_info.system.storage.disks.push(CoresDisks {
                name: name.clone(),
                total_space: total_space as u64,
                free_space: free_space as u64,
                throughput_read: 0.0,
                throughput_write: 0.0,
            });
        }
    }

    // Network info
    match get_default_interface() {
        Ok(int) => {
            if data.first_run {
                data.hw_info.system.network.interfaces.push(CoresNetInterface {
                    name: int.friendly_name.unwrap_or(int.name),
                    description: int.description.unwrap_or("N/A".to_string()),
                    mac_address: int.mac_addr.unwrap_or(MacAddr::default()).address(),
                    ip_address: int
                        .ipv4
                        .get(0)
                        .unwrap_or(&Ipv4Net::new(Ipv4Addr::new(0, 0, 0, 0), 24))
                        .addr
                        .to_string(),
                    mask: int
                        .ipv4
                        .get(0)
                        .unwrap_or(&Ipv4Net::new(Ipv4Addr::new(0, 0, 0, 0), 24))
                        .netmask()
                        .to_string(),
                    gateway: int
                        .gateway
                        .unwrap_or(NetworkDevice::new())
                        .ipv4
                        .get(0)
                        .unwrap_or(&Ipv4Addr::new(0, 0, 0, 0))
                        .to_string(),
                    dns: int
                        .dns_servers
                        .get(0)
                        .unwrap_or(&IpAddr::V4(Ipv4Addr::new(0, 0, 0, 0)))
                        .to_string(),
                    speed: "N/A".to_string(),
                    upload_data: 0.0,
                    download_data: 0.0,
                    throughput_upload: 0.0,
                    throughput_download: 0.0,
                });
            }

            for (_interface_name, net_data) in data.network.iter() {
                if net_data.mac_address().to_string()
                    == data.hw_info.system.network.interfaces[0].mac_address
                {
                    let download_data = net_data.total_received() as f64 / gb;
                    let upload_data = net_data.total_transmitted() as f64 / gb;

                    let throughput_download = net_data.received() as f64 / mb;
                    let throughput_upload = net_data.transmitted() as f64 / mb;

                    data.hw_info.system.network.interfaces[0].download_data = download_data;
                    data.hw_info.system.network.interfaces[0].upload_data = upload_data;
                    data.hw_info.system.network.interfaces[0].throughput_download =
                        throughput_download;
                    data.hw_info.system.network.interfaces[0].throughput_upload = throughput_upload;
                }
            }

            data.first_run = false;
        }
        Err(err) => {
            println!("Error getting default interface: {:#?}", err)
        }
    };

    // Components temperature:
    // let components = Components::new_with_refreshed_list();
    // println!("=> components:");
    // for component in &components {
    //     println!("{component:?}");
    // }
}
