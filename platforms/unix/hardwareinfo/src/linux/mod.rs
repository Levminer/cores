use std::sync::LazyLock;

use log::{debug, info};

use crate::{compare_sensor, CoresDisk, CoresRAMInfo, CoresSensor, Data};

pub mod cpu;
pub mod drive;
pub mod memory;

const FLATPAK_SPAWN: &str = "/usr/bin/flatpak-spawn";
static FLATPAK_APP_PATH: LazyLock<String> = LazyLock::new(|| String::new());
pub static IS_FLATPAK: LazyLock<bool> = LazyLock::new(|| {
    let is_flatpak = std::path::Path::new("/.flatpak-info").exists();

    if is_flatpak {
        debug!("Running as Flatpak");
    } else {
        debug!("Not running as Flatpak");
    }

    is_flatpak
});

pub fn linux_hardware_info(data: &mut Data) {
    let drive_paths = drive::Drive::get_sysfs_paths().unwrap_or_default();
    const SECTOR_SIZE: usize = 512;

    if data.first_run {
        // Memory
        let mem = memory::get_memory_devices();

        if let Ok(mem) = mem {
            for mem_device in mem {
                data.hw_info.ram.info.push(CoresRAMInfo {
                    manufacturer_name: mem_device.manufacturer.unwrap_or("Drive".to_string()),
                    configured_speed: mem_device.speed_mts.unwrap_or(0),
                    configured_voltage: mem_device.configured_voltage.unwrap_or(0.0) * 1000.0,
                    size: mem_device.size.unwrap_or(1) / 1024 / 1024,
                });
            }
        }

        // CPU
        let logical_cpus = data.sys.cpus().len();
        let cpu_info = cpu::cpu_info().unwrap();

        data.hw_info.cpu.info[0].max_speed = cpu_info.max_speed.unwrap_or(1.0) / 1000.0 / 1000.0;

        info!("{cpu_info:?}");

        let cpu_data = cpu::CpuData::new(logical_cpus);
        if let Ok(temp) = cpu_data.temperature {
            data.hw_info.cpu.temperature.push(CoresSensor {
                name: "CPU".to_string(),
                value: temp as f64,
                min: temp as f64,
                max: temp as f64,
            });
        }

        let mut drive_data = Vec::with_capacity(drive_paths.len());
        for path in &drive_paths {
            let d = drive::DriveData::new(path);

            if !d.is_virtual {
                let inner = &d.inner;

                data.hw_info.system.storage.disks.push(CoresDisk {
                    name: inner.clone().model.unwrap_or("N/A".to_string()),
                    total_space: inner.clone().capacity().unwrap_or(1) / 1000 / 1000 / 1000,
                    free_space: 0,
                    throughput_read: 0.0,
                    throughput_write: 0.0,
                    temperature: CoresSensor::default(),
                    health: "N/A".to_string(),
                });
                drive_data.push(drive::DriveData::new(path));
            }
        }
    } else {
        // CPU
        let logical_cpus = data.sys.cpus().len();
        let prev_temp = data.hw_info.cpu.temperature[0].clone();

        let cpu_data = cpu::CpuData::new(logical_cpus);
        if let Ok(temp) = cpu_data.temperature {
            data.hw_info.cpu.temperature[0] = compare_sensor(&prev_temp, temp as f64);
        }
    }

    // let disks = &data.hw_info.system.storage.disks;
    // for disk in disks {
    //     let inner = &disk.inner;
    //     let time_passed = 5.0;
    //     let delta_write_sectors = inner.clone().sys_stats().unwrap().get("write_sectors").unwrap_or(&0).saturating_sub(rhs);
    // }
}
