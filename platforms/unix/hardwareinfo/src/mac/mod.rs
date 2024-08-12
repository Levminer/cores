use crate::{compare_sensor, CoresSensor, Data, Round};

#[cfg(target_os = "macos")]
pub mod metrics;
#[cfg(target_os = "macos")]
pub mod sources;

#[cfg(target_os = "macos")]
pub fn macos_hardware_info(data: &mut Data) {
    let mut sampler = metrics::Sampler::new().unwrap();
    let soc = sources::SocInfo::new().unwrap();

    let metrics = sampler.get_metrics(100).unwrap();

    if data.first_run {
        data.hw_info.cpu.info[0].manufacturer_name = "Apple".to_string();
        data.hw_info.cpu.info[0].socket_designation = soc.mac_model.clone();
        data.hw_info.gpu.name = soc.chip_name.clone();
        data.hw_info.system.motherboard.name = soc.mac_model.clone();

        data.hw_info.gpu.temperature.push(CoresSensor {
            name: "SOC".to_string(),
            value: (metrics.temp.gpu_temp_avg as f64).fmt_num(),
            min: (metrics.temp.gpu_temp_avg as f64).fmt_num(),
            max: (metrics.temp.gpu_temp_avg as f64).fmt_num(),
        });

        data.hw_info.gpu.power.push(CoresSensor {
            name: "SOC".to_string(),
            value: (metrics.gpu_power as f64).fmt_num(),
            min: (metrics.gpu_power as f64).fmt_num(),
            max: (metrics.gpu_power as f64).fmt_num(),
        });

        data.hw_info.gpu.clock.push(CoresSensor {
            name: "SOC".to_string(),
            value: (metrics.gpu_usage.0 as f64).fmt_num(),
            min: (metrics.gpu_usage.0 as f64).fmt_num(),
            max: (metrics.gpu_usage.0 as f64).fmt_num(),
        });

        data.hw_info.cpu.temperature.push(CoresSensor {
            name: "SOC".to_string(),
            value: (metrics.temp.cpu_temp_avg as f64).fmt_num(),
            min: (metrics.temp.cpu_temp_avg as f64).fmt_num(),
            max: (metrics.temp.cpu_temp_avg as f64).fmt_num(),
        });

        data.hw_info.cpu.power.push(CoresSensor {
            name: "SOC".to_string(),
            value: (metrics.cpu_power as f64).fmt_num(),
            min: (metrics.cpu_power as f64).fmt_num(),
            max: (metrics.cpu_power as f64).fmt_num(),
        });

        data.hw_info.gpu.load.push(CoresSensor {
            name: "Load".to_string(),
            value: (metrics.gpu_usage.1 as f64 * 100.0).fmt_num(),
            min: (metrics.gpu_usage.1 as f64 * 100.0).fmt_num(),
            max: (metrics.gpu_usage.1 as f64 * 100.0).fmt_num(),
        });

        data.hw_info.gpu.max_load = (data.hw_info.gpu.load[0].value as f64).fmt_num();
    } else {
        let prev_gpu_temp = data.hw_info.gpu.temperature[0].clone();
        let prev_gpu_power = data.hw_info.gpu.power[0].clone();
        let prev_cpu_temp = data.hw_info.cpu.temperature[0].clone();
        let prev_cpu_power = data.hw_info.cpu.power[0].clone();
        let prev_gpu_load = data.hw_info.gpu.load[0].clone();
        let prev_gpu_clock = data.hw_info.gpu.clock[0].clone();

        data.hw_info.gpu.temperature[0] =
            compare_sensor(&prev_gpu_temp, (metrics.temp.gpu_temp_avg as f64).fmt_num());
        data.hw_info.gpu.power[0] =
            compare_sensor(&prev_gpu_power, (metrics.gpu_power as f64).fmt_num());
        data.hw_info.cpu.temperature[0] =
            compare_sensor(&prev_cpu_temp, (metrics.temp.cpu_temp_avg as f64).fmt_num());
        data.hw_info.cpu.power[0] =
            compare_sensor(&prev_cpu_power, (metrics.cpu_power as f64).fmt_num());
        data.hw_info.gpu.load[0] = compare_sensor(
            &prev_gpu_load,
            (metrics.gpu_usage.1 as f64 * 100.0).fmt_num(),
        );
        data.hw_info.gpu.max_load = (data.hw_info.gpu.load[0].value as f64).fmt_num();
        data.hw_info.gpu.clock[0] =
            compare_sensor(&prev_gpu_clock, (metrics.gpu_usage.0 as f64).fmt_num());
    }
}
