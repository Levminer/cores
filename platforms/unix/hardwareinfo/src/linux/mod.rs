use log::{debug, warn};
use std::{path::PathBuf, sync::LazyLock, time::SystemTime};

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
                if mem_device.size.unwrap_or(0) > 0 {
                    data.hw_info.ram.info.push(CoresRAMInfo {
                        manufacturer_name: mem_device.manufacturer.unwrap_or("Drive".to_string()),
                        configured_speed: mem_device.speed_mts.unwrap_or(0),
                        configured_voltage: mem_device.configured_voltage.unwrap_or(0.0) * 1000.0,
                        size: mem_device.size.unwrap_or(1) / 1024 / 1024,
                    });
                }
            }
        }

        // CPU
        let logical_cpus = data.sys.cpus().len();

        // lscpu is missing on minimal systems
        let max_speed = match cpu::cpu_info() {
            Ok(cpu_info) => cpu_info.max_speed,
            Err(e) => {
                warn!("Failed to get CPU info: {}", e);
                None
            }
        };

        data.hw_info.cpu.info[0].current_speed = max_speed.unwrap_or(1.0) / 1000.0 / 1000.0;

        data.hw_info.cpu.name.contains("Intel").then(|| {
            data.hw_info.cpu.info[0].manufacturer_name = "Intel".to_string();
        });

        data.hw_info.cpu.name.contains("AMD").then(|| {
            data.hw_info.cpu.info[0].manufacturer_name = "AMD".to_string();
        });

        let cpu_data = cpu::CpuData::new(logical_cpus);
        if let Ok(temp) = cpu_data.temperature {
            data.hw_info.cpu.temperature.push(CoresSensor {
                name: "Temperature".to_string(),
                value: temp as f64,
                min: temp as f64,
                max: temp as f64,
            });
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
        if data.hw_info.cpu.temperature.len() > 0 {
            let logical_cpus = data.sys.cpus().len();
            let prev_temp = data.hw_info.cpu.temperature[0].clone();

            let cpu_data = cpu::CpuData::new(logical_cpus);
            if let Ok(temp) = cpu_data.temperature {
                data.hw_info.cpu.temperature[0] = compare_sensor(&prev_temp, temp as f64);
            }
        }
    }

    // Drives
    // SMART data changes slowly, so only run smartctl once per minute
    let refresh_drive_info = SystemTime::now()
        .duration_since(data.elapsed_time)
        .map_or(true, |time_passed| time_passed.as_secs() >= 60);

    if refresh_drive_info {
        data.elapsed_time = SystemTime::now();
    }

    let disks = &mut data.hw_info.system.storage.disks;
    let mut present_drives = Vec::new();

    for path in &drive_paths {
        let d = drive::DriveData::new(path);

        if d.is_virtual {
            continue;
        }

        present_drives.push(d.inner.block_device.clone());

        // Drives are matched by name, they can be plugged in or removed while running
        let Some(disk) = disks
            .iter_mut()
            .find(|disk| disk.block_device == d.inner.block_device)
        else {
            disks.push(new_disk(path, &d));
            continue;
        };

        // The drive was removed while it was being read
        let (Some(&write_sectors), Some(&read_sectors)) = (
            d.disk_stats.get("write_sectors"),
            d.disk_stats.get("read_sectors"),
        ) else {
            continue;
        };

        let time_passed = SystemTime::now()
            .duration_since(disk.last_timestamp)
            .map_or(1.0f64, |timestamp| timestamp.as_secs_f64());

        let delta_write_sectors = write_sectors.saturating_sub(disk.write_sectors);
        let delta_read_sectors = read_sectors.saturating_sub(disk.read_sectors);
        let write_speed = (delta_write_sectors * SECTOR_SIZE) as f64 / time_passed;
        let read_speed = (delta_read_sectors * SECTOR_SIZE) as f64 / time_passed;
        let total_written = (write_sectors * SECTOR_SIZE) as f64 / 1_000_000_000.0;
        let total_read = (read_sectors * SECTOR_SIZE) as f64 / 1_000_000_000.0;

        disk.throughput_write = write_speed;
        disk.throughput_read = read_speed;
        disk.data_read = total_read.fmt_num();
        disk.data_written = total_written.fmt_num();

        disk.write_sectors = write_sectors;
        disk.read_sectors = read_sectors;
        disk.last_timestamp = SystemTime::now();

        if refresh_drive_info {
            let drive_info = drive::get_drive_info(path);

            disk.temperature = compare_sensor(&disk.temperature, drive_info.temperature.value);
            disk.health = drive_info.health;
        }
    }

    // Keep the list if the drives could not be listed at all
    if !drive_paths.is_empty() {
        disks.retain(|disk| present_drives.contains(&disk.block_device));
    }
}

fn new_disk(path: &PathBuf, d: &drive::DriveData) -> CoresDisk {
    let drive_space = drive::get_free_space(path);
    let drive_info = drive::get_drive_info(path);
    let drive_size = d.inner.capacity().unwrap_or(1) / 1000 / 1000 / 1000;

    CoresDisk {
        name: d
            .inner
            .model
            .clone()
            .unwrap_or(format!("{} GB drive", drive_size)),
        total_space: drive_size,
        free_space: drive_space,
        throughput_read: 0.0,
        throughput_write: 0.0,
        data_read: 0.0,
        data_written: 0.0,
        temperature: drive_info.temperature,
        health: drive_info.health,
        read_sectors: d.disk_stats.get("read_sectors").copied().unwrap_or(0),
        write_sectors: d.disk_stats.get("write_sectors").copied().unwrap_or(0),
        last_timestamp: SystemTime::now(),
        block_device: d.inner.block_device.clone(),
    }
}
