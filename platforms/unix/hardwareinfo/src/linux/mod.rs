use std::{sync::LazyLock, time::SystemTime};

use drive::DriveData;
use log::{debug, info};

use crate::{compare_sensor, CoresDisk, CoresRAMInfo, CoresSensor, Data, Round};

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

        let cpu_data = cpu::CpuData::new(logical_cpus);
        if let Ok(temp) = cpu_data.temperature {
            data.hw_info.cpu.temperature.push(CoresSensor {
                name: "CPU".to_string(),
                value: temp as f64,
                min: temp as f64,
                max: temp as f64,
            });
        }

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
                    data_read: 0.0,
                    data_written: 0.0,
                    temperature: CoresSensor::default(),
                    health: "N/A".to_string(),
                    read_sectors: d.disk_stats.get("read_sectors").unwrap_or(&0).clone(),
                    write_sectors: d.disk_stats.get("write_sectors").unwrap_or(&0).clone(),
                    last_timestamp: SystemTime::now(),
                });
            }
        }

        // System
        let board_name = std::fs::read_to_string("/sys/devices/virtual/dmi/id/board_name");
        let bios_date = std::fs::read_to_string("/sys/devices/virtual/dmi/id/bios_date");
        let bios_version = std::fs::read_to_string("/sys/devices/virtual/dmi/id/bios_version");
        let bios_vendor = std::fs::read_to_string("/sys/devices/virtual/dmi/id/bios_vendor");

        if let Ok(model) = board_name {
            data.hw_info.system.motherboard.name = model.trim().to_string();
        }

        if let Ok(date) = bios_date {
            data.hw_info.system.bios.date = date.trim().to_string();
        }

        if let Ok(version) = bios_version {
            data.hw_info.system.bios.version = version.trim().to_string();
        }

        if let Ok(vendor) = bios_vendor {
            data.hw_info.system.bios.vendor = vendor.trim().to_string();
        }
    } else {
        // CPU
        let logical_cpus = data.sys.cpus().len();
        let prev_temp = data.hw_info.cpu.temperature[0].clone();

        let cpu_data = cpu::CpuData::new(logical_cpus);
        if let Ok(temp) = cpu_data.temperature {
            data.hw_info.cpu.temperature[0] = compare_sensor(&prev_temp, temp as f64);
        }

        // Drives
        let mut i = 0;

        for path in &drive_paths {
            let d = drive::DriveData::new(path);

            if !d.is_virtual {
                let inner = &d.inner;
                let prev = &data.hw_info.system.storage.disks[i].clone();

                let time_passed = SystemTime::now()
                    .duration_since(prev.last_timestamp)
                    .map_or(1.0f64, |timestamp| timestamp.as_secs_f64());

                let old_inner = inner.clone().sys_stats().unwrap();
                let write_sectors = old_inner.get("write_sectors").unwrap_or(&0);
                let read_sectors = old_inner.get("read_sectors").unwrap_or(&0);
                let delta_write_sectors = write_sectors.saturating_sub(prev.write_sectors);
                let delta_read_sectors = read_sectors.saturating_sub(prev.read_sectors);
                let write_speed = (delta_write_sectors * SECTOR_SIZE) as f64 / time_passed;
                let read_speed = (delta_read_sectors * SECTOR_SIZE) as f64 / time_passed;
                let total_written = (write_sectors * SECTOR_SIZE) as f64 / 1_000_000_000.0;
                let total_read = (read_sectors * SECTOR_SIZE) as f64 / 1_000_000_000.0;

                data.hw_info.system.storage.disks[i].throughput_write = write_speed;
                data.hw_info.system.storage.disks[i].throughput_read = read_speed;
                data.hw_info.system.storage.disks[i].data_read = total_read.fmt_num();
                data.hw_info.system.storage.disks[i].data_written = total_written.fmt_num();

                data.hw_info.system.storage.disks[i].write_sectors =
                    d.disk_stats.get("write_sectors").unwrap_or(&0).clone();
                data.hw_info.system.storage.disks[i].read_sectors =
                    d.disk_stats.get("read_sectors").unwrap_or(&0).clone();
                data.hw_info.system.storage.disks[i].last_timestamp = SystemTime::now();

                i += 1;
            }
        }
    }

    // let disks = &data.hw_info.system.storage.disks;
    // for disk in disks {
    //     let inner = &disk.inner;
    //     let time_passed = 5.0;
    //     let delta_write_sectors = inner.clone().sys_stats().unwrap().get("write_sectors").unwrap_or(&0).saturating_sub(rhs);
    // }
}
