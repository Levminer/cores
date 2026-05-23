use crate::{compare_sensor, CoresDisk, CoresSensor, Data, Round};

#[cfg(target_os = "macos")]
pub fn macos_hardware_info(data: &mut Data) {
    use core::str;
    use macmon::{metrics, sources};
    use mtop::metrics::Sampler;
    use std::{process::Command, time::SystemTime};
    use sysinfo::Disks;

    use crate::{CoresGPUCard, SmartctlDiskInfo};

    let mut sampler = metrics::Sampler::new().unwrap();
    let metrics = sampler.get_metrics(100).unwrap();
    let soc = sources::SocInfo::new().unwrap();

    let mut sampler2 = Sampler::new().unwrap();
    let metrics2 = sampler2.sample(100).unwrap();

    if data.first_run {
        data.hw_info.cpu.info[0].manufacturer_name = "Apple".to_string();
        data.hw_info.cpu.info[0].socket_designation = soc.mac_model.clone();
        data.hw_info.gpu.cards.push(CoresGPUCard::default());
        data.hw_info.gpu.cards[0].name = soc.chip_name.clone();
        data.hw_info.system.motherboard.name = soc.mac_model.clone();

        data.hw_info.gpu.cards[0].temperature.push(CoresSensor::new(
            "SOC".to_string(),
            (metrics.temp.gpu_temp_avg as f64).fmt_num(),
        ));

        data.hw_info.gpu.cards[0].power.push(CoresSensor::new(
            "SOC".to_string(),
            (metrics.gpu_power as f64).fmt_num(),
        ));

        data.hw_info.gpu.cards[0].clock.push(CoresSensor::new(
            "SOC".to_string(),
            (metrics.gpu_usage.0 as f64).fmt_num(),
        ));

        data.hw_info.cpu.temperature.push(CoresSensor::new(
            "SOC".to_string(),
            (metrics.temp.cpu_temp_avg as f64).fmt_num(),
        ));

        data.hw_info.cpu.power.push(CoresSensor::new(
            "SOC".to_string(),
            (metrics.cpu_power as f64).fmt_num(),
        ));

        data.hw_info.cpu.clock.push(CoresSensor::new(
            "P Cores".to_string(),
            (metrics.pcpu_usage.0 as f64).fmt_num(),
        ));

        data.hw_info.cpu.clock.push(CoresSensor::new(
            "E Cores".to_string(),
            (metrics.ecpu_usage.0 as f64).fmt_num(),
        ));

        data.hw_info.gpu.cards[0].load.push(CoresSensor::new(
            "Load".to_string(),
            (metrics.gpu_usage.1 as f64 * 100.0).fmt_num(),
        ));

        data.hw_info.gpu.cards[0].max_load =
            (data.hw_info.gpu.cards[0].load[0].value as f64).fmt_num();

        // Disks
        let gb = 1024_f64.powi(3);
        let disks = Disks::new_with_refreshed_list();
        for disk in disks.list() {
            let free_space = disk.available_space() as f64 / gb;
            let total_space = disk.total_space() as f64 / gb;
            let name = disk.name().to_str().unwrap().to_string();

            let read_bytes = metrics2.disk.read_bytes_sec;
            let write_bytes = metrics2.disk.write_bytes_sec;
            let ssd_temp = metrics2.temperature.ssd_avg_c;

            if !disk.is_removable() && disk.mount_point().to_str() == Some("/") {
                let mut primary_disk = CoresDisk {
                    name: name.clone(),
                    total_space: total_space as u64,
                    free_space: free_space as u64,
                    throughput_read: read_bytes as f64,
                    throughput_write: write_bytes as f64,
                    temperature: CoresSensor::new(
                        "SSD Temp".to_string(),
                        (ssd_temp as f64).fmt_num(),
                    ),
                    health: "N/A".to_string(),
                    data_read: 0.0,
                    data_written: 0.0,
                    read_sectors: 0,
                    write_sectors: 0,
                    last_timestamp: SystemTime::now(),
                };

                let command = format!("smartctl -a disk0 -j");
                let output = Command::new("sh").arg("-c").arg(&command).output();

                if let Ok(output) = output {
                    if let Ok(result) = str::from_utf8(&output.stdout) {
                        let json = serde_json::from_str::<SmartctlDiskInfo>(result);

                        if let Ok(json) = json {
                            if json.device.r#type == "nvme" {
                                primary_disk.health = (100
                                    - json
                                        .nvme_smart_health_information_log
                                        .unwrap()
                                        .percentage_used
                                        as u64)
                                    .to_string();

                                primary_disk.data_read = (json
                                    .nvme_smart_health_information_log
                                    .unwrap()
                                    .data_units_read
                                    .unwrap_or(0.0)
                                    * 512000.0
                                    / 1_000_000_000.0)
                                    .fmt_num();

                                primary_disk.data_written = (json
                                    .nvme_smart_health_information_log
                                    .unwrap()
                                    .data_units_written
                                    .unwrap_or(0.0)
                                    * 512000.0
                                    / 1_000_000_000.0)
                                    .fmt_num();
                            }
                        }
                    }
                }

                data.hw_info.system.storage.disks.push(primary_disk);
            }
        }
    } else {
        let prev_gpu_temp = data.hw_info.gpu.cards[0].temperature[0].clone();
        let prev_gpu_power = data.hw_info.gpu.cards[0].power[0].clone();
        let prev_cpu_temp = data.hw_info.cpu.temperature[0].clone();
        let prev_cpu_power = data.hw_info.cpu.power[0].clone();
        let prev_gpu_load = data.hw_info.gpu.cards[0].load[0].clone();
        let prev_gpu_clock = data.hw_info.gpu.cards[0].clock[0].clone();
        let prev_cpu_p_clock = data.hw_info.cpu.clock[0].clone();
        let prev_cpu_e_clock = data.hw_info.cpu.clock[1].clone();

        data.hw_info.gpu.cards[0].temperature[0] =
            compare_sensor(&prev_gpu_temp, (metrics.temp.gpu_temp_avg as f64).fmt_num());
        data.hw_info.gpu.cards[0].power[0] =
            compare_sensor(&prev_gpu_power, (metrics.gpu_power as f64).fmt_num());
        data.hw_info.cpu.temperature[0] =
            compare_sensor(&prev_cpu_temp, (metrics.temp.cpu_temp_avg as f64).fmt_num());
        data.hw_info.cpu.power[0] =
            compare_sensor(&prev_cpu_power, (metrics.cpu_power as f64).fmt_num());
        data.hw_info.gpu.cards[0].load[0] = compare_sensor(
            &prev_gpu_load,
            (metrics.gpu_usage.1 as f64 * 100.0).fmt_num(),
        );
        data.hw_info.gpu.cards[0].max_load =
            (data.hw_info.gpu.cards[0].load[0].value as f64).fmt_num();
        data.hw_info.gpu.cards[0].clock[0] =
            compare_sensor(&prev_gpu_clock, (metrics.gpu_usage.0 as f64).fmt_num());
        data.hw_info.cpu.clock[0] =
            compare_sensor(&prev_cpu_p_clock, (metrics.pcpu_usage.0 as f64).fmt_num());
        data.hw_info.cpu.clock[1] =
            compare_sensor(&prev_cpu_e_clock, (metrics.ecpu_usage.0 as f64).fmt_num());

        // disks
        let disk = &mut data.hw_info.system.storage.disks[0];
        disk.throughput_read = metrics2.disk.read_bytes_sec as f64;
        disk.throughput_write = metrics2.disk.write_bytes_sec as f64;

        let temp = (metrics2.temperature.ssd_avg_c as f64).fmt_num();
        disk.temperature = compare_sensor(&disk.temperature, temp);
    }
}
