use indexmap::IndexMap;
use log::{error, info};
use netdev::{MacAddr, NetworkDevice, get_default_interface};
use nvml_wrapper::enum_wrappers::device::{Clock, TemperatureSensor};
use nvml_wrapper::struct_wrappers::device::{MemoryInfo, Utilization};
use serde::{Deserialize, Serialize};
use starship_battery::units::energy::milliwatt_hour;
use starship_battery::units::ratio::percent;
use std::net::Ipv6Addr;
use std::time::SystemTime;
use std::{
    env,
    net::{IpAddr, Ipv4Addr},
};

pub use nvml_wrapper::Nvml;
pub use sysinfo::{Components, Disks, Networks, System, MINIMUM_CPU_UPDATE_INTERVAL};

#[cfg(target_os = "linux")]
pub mod linux;
#[cfg(target_os = "macos")]
pub mod mac;
pub mod settings;

trait Round {
    fn fmt_num(self) -> f64;
    fn fmt_num2(self) -> f64;
}

impl Round for f64 {
    fn fmt_num(self) -> f64 {
        (self * 10.0).round() / 10.0
    }

    fn fmt_num2(self) -> f64 {
        (self * 100.0).round() / 100.0
    }
}

#[derive(Debug)]
pub struct Data {
    pub sys: System,
    pub network: Networks,
    pub hw_info: HardwareInfo,
    pub first_run: bool,
    pub nvml: Result<Nvml, nvml_wrapper::error::NvmlError>,
    pub nvml_available: bool,
    pub interval: f64,
}

fn deserialize_f64_or_zero<'de, D>(deserializer: D) -> Result<f64, D::Error>
where
    D: serde::Deserializer<'de>,
{
    let opt = Option::<f64>::deserialize(deserializer)?;
    Ok(opt.unwrap_or(0.0))
}

#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct CoresSensor {
    pub name: String,
    #[serde(deserialize_with = "deserialize_f64_or_zero")]
    pub value: f64,
    #[serde(deserialize_with = "deserialize_f64_or_zero")]
    pub min: f64,
    #[serde(deserialize_with = "deserialize_f64_or_zero")]
    pub max: f64,
}

#[derive(Debug, Serialize, Deserialize, Clone)]
#[serde(rename_all = "camelCase")]
pub struct CoresCPUInfo {
    pub manufacturer_name: String,
    pub socket_designation: String,
    pub current_speed: f64,
    pub core_count: u32,
    pub thread_count: u32,
}

#[derive(Debug, Serialize, Deserialize, Clone)]
#[serde(rename_all = "camelCase")]
pub struct CoresCPU {
    pub name: String,
    pub info: Vec<CoresCPUInfo>,
    pub max_load: f64,
    pub load: Vec<CoresSensor>,
    pub clock: Vec<CoresSensor>,
    pub temperature: Vec<CoresSensor>,
    pub voltage: Vec<CoresSensor>,
    pub power: Vec<CoresSensor>,
}

#[derive(Debug, Serialize, Deserialize, Clone)]
#[serde(rename_all = "camelCase")]
pub struct CoresGPUCard {
    pub name: String,
    pub temperature: Vec<CoresSensor>,
    pub memory: Vec<CoresSensor>,
    pub max_load: f64,
    pub load: Vec<CoresSensor>,
    pub clock: Vec<CoresSensor>,
    pub power: Vec<CoresSensor>,
    pub fan: Vec<CoresSensor>,
}

#[derive(Debug, Serialize, Deserialize, Clone)]
#[serde(rename_all = "camelCase")]
pub struct CoresGPU {
    pub info: String,
    pub cards: Vec<CoresGPUCard>,
}

#[derive(Debug, Serialize, Deserialize, Clone)]
#[serde(rename_all = "camelCase")]
pub struct CoresRAMInfo {
    pub manufacturer_name: String,
    pub configured_speed: u32,
    pub configured_voltage: f32,
    pub size: u64,
}

#[derive(Debug, Serialize, Deserialize, Clone)]
pub struct CoresRAM {
    pub load: Vec<CoresSensor>,
    pub info: Vec<CoresRAMInfo>,
    pub layout: Vec<CoresRAMInfo>,
}

#[derive(Debug, Serialize, Deserialize, Clone)]
pub struct CoresOS {
    pub name: String,
}

#[derive(Debug, Serialize, Deserialize, Clone)]
#[serde(rename_all = "camelCase")]
pub struct CoresDisk {
    pub name: String,
    pub total_space: u64,
    pub free_space: u64,
    pub throughput_read: f64,
    pub throughput_write: f64,
    pub data_read: f64,
    pub data_written: f64,
    pub temperature: CoresSensor,
    pub health: String,
    pub read_sectors: usize,
    pub write_sectors: usize,
    pub last_timestamp: SystemTime,
}

#[derive(Debug, Serialize, Deserialize, Clone)]
pub struct SmartDevice {
    r#type: String,
}

#[derive(Debug, Serialize, Deserialize, Clone, Copy)]
pub struct SmartInfo {
    temperature: u64,
    percentage_used: u64,
    data_units_read: Option<f64>,
    data_units_written: Option<f64>,
}

#[derive(Debug, Serialize, Deserialize, Clone)]
pub struct SmartAttributeArray {
    name: String,
    value: u64,
}

#[derive(Debug, Serialize, Deserialize, Clone)]
pub struct SmartAttribute {
    table: Vec<SmartAttributeArray>,
}

#[derive(Debug, Serialize, Deserialize, Clone)]
pub struct SmartctlDiskInfo {
    device: SmartDevice,
    nvme_smart_health_information_log: Option<SmartInfo>,
    ata_smart_attributes: Option<SmartAttribute>,
}

#[derive(Debug, Serialize, Deserialize, Clone)]
#[serde(rename_all = "camelCase")]
pub struct CoresDiskInfo {
    pub health: String,
    pub temperature: CoresSensor,
}

#[derive(Debug, Serialize, Deserialize, Clone)]
pub struct CoresStorage {
    pub disks: Vec<CoresDisk>,
}

#[derive(Debug, Serialize, Deserialize, Clone)]
#[serde(rename_all = "camelCase")]
pub struct CoresNetInterface {
    pub name: String,
    pub description: String,
    pub mac_address: String,
    pub ip_address: String,
    #[serde(
        rename(deserialize = "ipAddressV6", serialize = "ipAddressV6"),
        default
    )]
    pub ip_address_v6: String,
    pub mask: String,
    pub gateway: String,
    #[serde(rename(deserialize = "gatewayV6", serialize = "gatewayV6"), default)]
    pub gateway_v6: String,
    pub dns: String,
    #[serde(rename(deserialize = "dnsV6", serialize = "dnsV6"), default)]
    pub dns_v6: String,
    pub speed: String,
    pub upload_data: f64,
    pub download_data: f64,
    pub throughput_upload: f64,
    pub throughput_download: f64,
}

#[derive(Debug, Serialize, Deserialize, Clone)]
pub struct CoresNetwork {
    pub interfaces: Vec<CoresNetInterface>,
}

#[derive(Debug, Serialize, Deserialize, Clone)]
#[serde(rename_all = "camelCase")]
pub struct CoresMonitorDevice {
    pub name: String,
    pub resolution: String,
    pub refresh_rate: String,
    pub primary: bool,
}

#[derive(Debug, Serialize, Deserialize, Clone)]
pub struct CoresMonitor {
    pub monitors: Vec<CoresMonitorDevice>,
}

#[derive(Debug, Serialize, Deserialize, Clone)]
pub struct CoresMotherboard {
    pub name: String,
}

#[derive(Debug, Serialize, Deserialize, Clone)]
#[serde(rename_all = "camelCase")]
pub struct CoresBattery {
    pub cycle_count: String,
    pub level: Vec<CoresSensor>,
    pub capacity: Vec<CoresSensor>,
}

#[derive(Debug, Serialize, Deserialize, Clone)]
pub struct CoresBIOS {
    pub vendor: String,
    pub version: String,
    pub date: String,
}

#[derive(Debug, Serialize, Deserialize, Clone)]
#[serde(rename_all = "camelCase")]
pub struct CoresSuperIO {
    pub name: String,
    pub fan: Vec<CoresSensor>,
    pub fan_control: Vec<CoresSensor>,
}

#[derive(Debug, Serialize, Deserialize, Clone)]
#[allow(non_snake_case)]
pub struct CoresSystem {
    pub network: CoresNetwork,
    pub storage: CoresStorage,
    pub os: CoresOS,
    pub motherboard: CoresMotherboard,
    pub bios: CoresBIOS,
    pub superIO: CoresSuperIO,
    pub monitor: CoresMonitor,
    pub battery: CoresBattery,
}

#[derive(Debug, Serialize, Deserialize, Clone)]
pub struct HardwareInfo {
    pub cpu: CoresCPU,
    pub ram: CoresRAM,
    pub gpu: CoresGPU,
    pub system: CoresSystem,
    pub timestamp: String,
}

impl HardwareInfo {
    pub fn default() -> HardwareInfo {
        HardwareInfo {
            ram: CoresRAM {
                load: Vec::new(),
                info: Vec::new(),
                layout: Vec::new(),
            },
            cpu: CoresCPU {
                name: "N/A".to_string(),
                max_load: 0.0,
                load: Vec::new(),
                clock: Vec::new(),
                temperature: Vec::new(),
                voltage: Vec::new(),
                power: Vec::new(),
                info: vec![CoresCPUInfo {
                    manufacturer_name: "N/A".to_string(),
                    socket_designation: "N/A".to_string(),
                    current_speed: 0.0,
                    core_count: 0,
                    thread_count: 0,
                }],
            },
            gpu: CoresGPU {
                info: "N/A".to_string(),
                cards: Vec::new(),
            },
            system: CoresSystem {
                storage: CoresStorage { disks: Vec::new() },
                os: CoresOS {
                    name: "N/A".to_string(),
                },
                network: CoresNetwork {
                    interfaces: Vec::new(),
                },
                motherboard: CoresMotherboard {
                    name: "N/A".to_string(),
                },
                bios: CoresBIOS {
                    vendor: "N/A".to_string(),
                    version: "N/A".to_string(),
                    date: "N/A".to_string(),
                },
                superIO: CoresSuperIO {
                    name: "N/A".to_string(),
                    fan: Vec::new(),
                    fan_control: Vec::new(),
                },
                monitor: CoresMonitor {
                    monitors: Vec::new(),
                },
                battery: CoresBattery {
                    cycle_count: "N/A".to_string(),
                    level: Vec::new(),
                    capacity: Vec::new(),
                },
            },
            timestamp: chrono::Utc::now().to_rfc3339(),
        }
    }
}

impl CoresSensor {
    pub fn default() -> CoresSensor {
        CoresSensor {
            name: "N/A".to_string(),
            value: 0.0,
            min: 0.0,
            max: 0.0,
        }
    }

    pub fn new(name: String, value: f64) -> CoresSensor {
        CoresSensor {
            name,
            value,
            min: value,
            max: value,
        }
    }
}

impl CoresGPUCard {
    pub fn default() -> CoresGPUCard {
        CoresGPUCard {
            name: "N/A".to_string(),
            clock: Vec::new(),
            temperature: Vec::new(),
            fan: Vec::new(),
            load: Vec::new(),
            memory: Vec::new(),
            power: Vec::new(),
            max_load: 0.0,
        }
    }
}

fn compare_sensor(prev_sensor: &CoresSensor, value: f64) -> CoresSensor {
    return CoresSensor {
        name: prev_sensor.name.clone(),
        value,
        min: if value < prev_sensor.min {
            value
        } else {
            prev_sensor.min
        },
        max: if value > prev_sensor.max {
            value
        } else {
            prev_sensor.max
        },
    };
}

pub fn refresh_hardware_info(data: &mut Data) {
    let gb = 1024_f64.powi(3);
    let _mb = 1024_f64.powi(2);

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
    let total_memory = (data.sys.total_memory() as f64 / gb).fmt_num2();
    let used_memory = (data.sys.used_memory() as f64 / gb).fmt_num2();
    let total_swap = (data.sys.total_swap() as f64 / gb).fmt_num2();
    let used_swap = (data.sys.used_swap() as f64 / gb).fmt_num2();
    let ram_used = ((used_memory / total_memory) * 100.0).fmt_num2();
    let swap_used = ((used_swap / total_swap) * 100.0).fmt_num2();
    let memory_available = (total_memory - used_memory).fmt_num2();
    let virtual_memory_available = (total_swap - used_swap).fmt_num2();

    let mut mem_map = IndexMap::<String, f64>::new();
    mem_map.insert("Memory Used".to_string(), used_memory);
    mem_map.insert("Memory Available".to_string(), memory_available);
    mem_map.insert("Memory".to_string(), ram_used);
    mem_map.insert("Virtual Memory Used".to_string(), used_swap);
    mem_map.insert(
        "Virtual Memory Available".to_string(),
        virtual_memory_available,
    );
    mem_map.insert("Virtual Memory".to_string(), swap_used);

    if data.hw_info.ram.load.len() == 0 {
        for (name, value) in mem_map {
            data.hw_info.ram.load.push(CoresSensor {
                name,
                value,
                min: value,
                max: value,
            });
        }
    } else {
        let mut i = 0;
        for (_name, value) in mem_map {
            let prev = &data.hw_info.ram.load[i].clone();

            data.hw_info.ram.load[i] = compare_sensor(prev, value);

            i += 1;
        }
    }

    // CPU Info
    data.hw_info.cpu.max_load = data.sys.global_cpu_usage() as f64;

    if data.first_run {
        data.hw_info.cpu.info[0].core_count =
            sysinfo::System::physical_core_count().unwrap_or(0) as u32;
        data.hw_info.cpu.info[0].thread_count = data.sys.cpus().len() as u32;
    }

    let mut cpu_count = 0;
    for cpu in data.sys.cpus() {
        data.hw_info.cpu.name = cpu.brand().to_string();

        if data.first_run {
            data.hw_info.cpu.load.push(CoresSensor::new(
                format!("Core #{}", cpu_count),
                cpu.cpu_usage() as f64,
            ));

            if cfg!(target_os = "linux") {
                data.hw_info.cpu.clock.push(CoresSensor::new(
                    format!("Core #{}", cpu_count),
                    cpu.frequency() as f64,
                ));
            }

            cpu_count += 1;
        } else {
            let prev_load = &data.hw_info.cpu.load[cpu_count];
            let load = cpu.cpu_usage() as f64;
            data.hw_info.cpu.load[cpu_count] = compare_sensor(prev_load, load);

            if cfg!(target_os = "linux") {
                let prev_clock = &data.hw_info.cpu.clock[cpu_count];
                let clock = cpu.frequency() as f64;
                data.hw_info.cpu.clock[cpu_count] = compare_sensor(prev_clock, clock);
            }

            cpu_count += 1;
        }
    }

    //GPU
    if data.nvml_available {
        match &data.nvml {
            Ok(nvml) => {
                let device = nvml.device_by_index(0);
                let device_index = 0;

                match device {
                    Ok(device) => {
                        let power = device.power_usage().unwrap_or(1000) / 1000;
                        let temperature = device.temperature(TemperatureSensor::Gpu).unwrap_or(0);
                        let memory = device.memory_info().unwrap_or(MemoryInfo {
                            free: 0,
                            total: 0,
                            used: 0,
                        });
                        let gpu_clock = device.clock_info(Clock::Graphics).unwrap_or(0);
                        let mem_clock = device.clock_info(Clock::Memory).unwrap_or(0);
                        let gpu_usage = device
                            .utilization_rates()
                            .unwrap_or(Utilization { gpu: 0, memory: 0 });

                        let mut gpu_mem_map = IndexMap::<String, f64>::new();

                        gpu_mem_map.insert(
                            "GPU Memory Used".to_string(),
                            (memory.used as f64 / gb).fmt_num(),
                        );
                        gpu_mem_map.insert("N/A".to_string(), 0.0);
                        gpu_mem_map.insert(
                            "GPU Memory Total".to_string(),
                            (memory.total as f64 / gb).fmt_num(),
                        );
                        gpu_mem_map.insert(
                            "GPU Memory Free".to_string(),
                            (memory.free as f64 / gb).fmt_num(),
                        );
                        gpu_mem_map.insert(
                            "GPU Memory Used".to_string(),
                            (memory.used as f64 / gb).fmt_num(),
                        );

                        if data.first_run {
                            data.hw_info.gpu.cards.push(CoresGPUCard::default());

                            data.hw_info.gpu.cards[device_index].name =
                                device.name().expect("failed to get device name");
                            data.hw_info.gpu.info =
                                nvml.sys_driver_version().unwrap_or("N/A".to_string());

                            data.hw_info.gpu.cards[device_index].max_load = gpu_usage.gpu as f64;

                            data.hw_info.gpu.cards[device_index].load.push(CoresSensor {
                                name: "Load".to_string(),
                                value: gpu_usage.gpu as f64,
                                min: gpu_usage.gpu as f64,
                                max: gpu_usage.gpu as f64,
                            });

                            data.hw_info.gpu.cards[device_index]
                                .power
                                .push(CoresSensor {
                                    name: "Power Usage".to_string(),
                                    value: power as f64,
                                    min: power as f64,
                                    max: power as f64,
                                });

                            data.hw_info.gpu.cards[device_index]
                                .temperature
                                .push(CoresSensor {
                                    name: "Temperature".to_string(),
                                    value: temperature as f64,
                                    min: temperature as f64,
                                    max: temperature as f64,
                                });

                            for (name, value) in gpu_mem_map {
                                data.hw_info.gpu.cards[device_index]
                                    .memory
                                    .push(CoresSensor {
                                        name,
                                        value,
                                        min: value,
                                        max: value,
                                    });
                            }

                            data.hw_info.gpu.cards[device_index]
                                .clock
                                .push(CoresSensor {
                                    name: "GPU Core".to_string(),
                                    value: gpu_clock as f64,
                                    min: gpu_clock as f64,
                                    max: gpu_clock as f64,
                                });

                            data.hw_info.gpu.cards[device_index]
                                .clock
                                .push(CoresSensor {
                                    name: "GPU Memory".to_string(),
                                    value: mem_clock as f64,
                                    min: mem_clock as f64,
                                    max: mem_clock as f64,
                                });
                        } else {
                            data.hw_info.gpu.cards[device_index].max_load = gpu_usage.gpu as f64;

                            data.hw_info.gpu.cards[device_index].load[0] = compare_sensor(
                                &data.hw_info.gpu.cards[device_index].load[0],
                                gpu_usage.gpu as f64,
                            );

                            data.hw_info.gpu.cards[device_index].power[0] = compare_sensor(
                                &data.hw_info.gpu.cards[device_index].power[0],
                                power as f64,
                            );

                            data.hw_info.gpu.cards[device_index].temperature[0] = compare_sensor(
                                &data.hw_info.gpu.cards[device_index].temperature[0],
                                temperature as f64,
                            );

                            let mut i = 0;
                            for (_name, value) in gpu_mem_map {
                                let prev = &data.hw_info.gpu.cards[device_index].memory[i];

                                data.hw_info.gpu.cards[device_index].memory[i] =
                                    compare_sensor(prev, value);

                                i += 1;
                            }

                            data.hw_info.gpu.cards[device_index].clock[0] = compare_sensor(
                                &data.hw_info.gpu.cards[device_index].clock[0],
                                gpu_clock as f64,
                            );

                            data.hw_info.gpu.cards[device_index].clock[1] = compare_sensor(
                                &data.hw_info.gpu.cards[device_index].clock[1],
                                mem_clock as f64,
                            );
                        }
                    }
                    Err(err) => {
                        error!("Error getting GPU device: {:#?}", err);
                    }
                }
            }
            Err(_err) => {
                data.nvml_available = false;
                error!("Error initializing nvidia");
            }
        }
    }

    // Network info
    match get_default_interface() {
        Ok(int) => {
            if data.first_run {
                data.hw_info
                    .system
                    .network
                    .interfaces
                    .push(CoresNetInterface {
                        name: int.friendly_name.unwrap_or(int.name),
                        description: int.description.unwrap_or("N/A".to_string()),
                        mac_address: int.mac_addr.unwrap_or(MacAddr::default()).address(),
                        ip_address: int
                            .ipv4
                            .get(0)
                            .map(|net| net.addr().to_string())
                            .unwrap_or_else(|| Ipv4Addr::new(0, 0, 0, 0).to_string()),
                        ip_address_v6: int
                            .ipv6
                            .get(0)
                            .map(|net| net.addr().to_string())
                            .unwrap_or_else(|| Ipv6Addr::UNSPECIFIED.to_string()),
                        mask: int
                            .ipv4
                            .get(0)
                            .map(|net| net.netmask().to_string())
                            .unwrap_or_else(|| Ipv4Addr::new(0, 0, 0, 0).to_string()),
                        gateway: int
                            .gateway
                            .as_ref()
                            .unwrap_or(&NetworkDevice::new())
                            .ipv4
                            .get(0)
                            .unwrap_or(&Ipv4Addr::new(0, 0, 0, 0))
                            .to_string(),
                        gateway_v6: int
                            .gateway
                            .as_ref()
                            .unwrap_or(&NetworkDevice::new())
                            .ipv6
                            .get(0)
                            .unwrap_or(&Ipv6Addr::UNSPECIFIED)
                            .to_string(),
                        dns: int
                            .dns_servers
                            .iter()
                            .find(|ip| ip.is_ipv4())
                            .unwrap_or(&IpAddr::V4(Ipv4Addr::new(0, 0, 0, 0)))
                            .to_string(),
                        dns_v6: int
                            .dns_servers
                            .iter()
                            .find(|ip| ip.is_ipv6())
                            .unwrap_or(&IpAddr::V6(Ipv6Addr::UNSPECIFIED))
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
                    let download_data = (net_data.total_received() as f64 / gb).fmt_num();
                    let upload_data = (net_data.total_transmitted() as f64 / gb).fmt_num();

                    let throughput_download = net_data.received() as f64 / data.interval;
                    let throughput_upload = net_data.transmitted() as f64 / data.interval;

                    data.hw_info.system.network.interfaces[0].download_data = download_data;
                    data.hw_info.system.network.interfaces[0].upload_data = upload_data;
                    data.hw_info.system.network.interfaces[0].throughput_download =
                        throughput_download;
                    data.hw_info.system.network.interfaces[0].throughput_upload = throughput_upload;
                }
            }
        }
        Err(err) => {
            error!("Error getting default interface: {:#?}", err)
        }
    };

    // macOS
    #[cfg(target_os = "macos")]
    mac::macos_hardware_info(data);

    // Linux
    #[cfg(target_os = "linux")]
    linux::linux_hardware_info(data);

    // Raspberry Pi
    let model_file = std::fs::read_to_string("/proc/device-tree/model");

    if let Ok(model) = model_file {
        if model.contains("Raspberry Pi") {
            data.hw_info.system.motherboard.name = model.trim().to_string();

            // Components temperature:
            let components = Components::new_with_refreshed_list();

            if data.first_run {
                for component in &components {
                    let temp = if let Some(t) = component.temperature() {
                        (t as f64).fmt_num()
                    } else {
                        0.0
                    };

                    data.hw_info.cpu.temperature.push(CoresSensor {
                        name: component.label().to_string(),
                        value: temp,
                        min: temp,
                        max: temp,
                    });
                }
            } else {
                let mut i = 0;

                for component in &components {
                    let prev = &data.hw_info.cpu.temperature[i];

                    let temp = if let Some(t) = component.temperature() {
                        (t as f64).fmt_num()
                    } else {
                        0.0
                    };

                    data.hw_info.cpu.temperature[i] = compare_sensor(prev, temp);

                    i += 1;
                }
            }
        }
    }

    // Battery
    if data.first_run {
        let manager = starship_battery::Manager::new();

        match manager {
            Ok(manager) => {
                if let Ok(batteries) = manager.batteries() {
                    for battery in batteries {
                        if let Ok(battery) = battery {
                            let cycle_count = battery.cycle_count().unwrap_or(0);
                            let charge_level = battery.state_of_charge().get::<percent>();
                            let design_capacity =
                                battery.energy_full_design().get::<milliwatt_hour>();
                            let full_charge_capacity =
                                battery.energy_full().get::<milliwatt_hour>();
                            let remaining_capacity = battery.energy().get::<milliwatt_hour>();
                            let health = battery.state_of_health().get::<percent>();

                            // Skip batteries that return NaN values (desktop Macs without a real battery)
                            if charge_level.is_nan() || health.is_nan() {
                                continue;
                            }

                            data.hw_info.system.battery = CoresBattery {
                                cycle_count: cycle_count.to_string(),
                                level: Vec::new(),
                                capacity: Vec::new(),
                            };

                            data.hw_info.system.battery.level.push(CoresSensor {
                                name: "Health".to_string(),
                                value: 100.0 - health as f64,
                                min: 100.0 - health as f64,
                                max: 100.0 - health as f64,
                            });

                            data.hw_info.system.battery.level.push(CoresSensor {
                                name: "Charge level".to_string(),
                                value: charge_level as f64,
                                min: charge_level as f64,
                                max: charge_level as f64,
                            });

                            data.hw_info.system.battery.capacity.push(CoresSensor {
                                name: "Design capacity".to_string(),
                                value: design_capacity as f64,
                                min: design_capacity as f64,
                                max: design_capacity as f64,
                            });

                            data.hw_info.system.battery.capacity.push(CoresSensor {
                                name: "Full charge capacity".to_string(),
                                value: full_charge_capacity as f64,
                                min: full_charge_capacity as f64,
                                max: full_charge_capacity as f64,
                            });

                            data.hw_info.system.battery.capacity.push(CoresSensor {
                                name: "Remaining capacity".to_string(),
                                value: remaining_capacity as f64,
                                min: remaining_capacity as f64,
                                max: remaining_capacity as f64,
                            });
                        } else {
                            error!("Error getting specific battery info");
                        }
                    }
                }
            }
            Err(_err) => {
                error!("Error getting battery info");
            }
        };
    }

    // END

    if data.first_run {
        info!("Hardware info initialized");
        info!(
            "hw info: {}",
            serde_json::to_string(&data.hw_info).unwrap_or_default()
        );
    }

    data.first_run = false;
    data.hw_info.timestamp = chrono::Utc::now().to_rfc3339();
}
