using LibreHardwareMonitor.Hardware;
using System.Collections.Generic;
using System.Linq;

namespace cores;

public class CostumSensor {
	public string name {
		get; set;
	}

	public float value {
		get; set;
	}

	public float min {
		get; set;
	}

	public float max {
		get; set;
	}
}

public class CPUAPI {
	public string name {
		get; set;
	}

	public List<ISensor> Load = new();

	public float lastLoad {
		get; set;
	}

	public List<CostumSensor> temperature {
		get; set;
	} = new();
}

public class RAMAPI {
	public List<CostumSensor> load {
		get; set;
	} = new();
}

public class API {
	public CPUAPI CPU {
		get; set;
	} = new();

	public CPUAPI GPU {
		get; set;
	} = new();

	public RAMAPI RAM {
		get; set;
	} = new();
}

public class HardwareInfo {
	public HardwareUpdater refresher = new();
	public Computer computer = new() {
		IsCpuEnabled = true,
		IsGpuEnabled = true,
		IsMemoryEnabled = true,
		IsMotherboardEnabled = true,
		IsControllerEnabled = true,
		IsStorageEnabled = true
	};
	public API API {
		get; set;
	} = new();

	public HardwareInfo() {

		computer.Open();
		computer.Accept(refresher);

		GetInfo();
	}

	public void GetInfo() {
		var computerHardware = computer.Hardware;

		API.CPU.temperature.Clear();
		API.CPU.Load.Clear();
		API.RAM.load.Clear();
		API.GPU.temperature.Clear();


		for (int i = 0; i < computerHardware.Count; i++) {
			var sensor = computerHardware[i].Sensors;

			if (computerHardware[i].HardwareType.ToString() == "Cpu") {
				API.CPU.name = computerHardware[i].Name;
			}

			if (computerHardware[i].HardwareType.ToString().Contains("Gpu")) {
				API.GPU.name = computerHardware[i].Name;
			}

			for (int j = 0; j < sensor.Length; j++) {
				if (sensor[j].SensorType.ToString() == "Temperature" && computerHardware[i].HardwareType.ToString() == "Cpu" && sensor[j].Name.StartsWith("CPU Core") && !sensor[j].Name.Contains("Tj")) {
					var temp = new CostumSensor {
						value = float.Parse(sensor[j].Value.ToString()),
						min = float.Parse(sensor[j].Min.ToString()),
						max = float.Parse(sensor[j].Max.ToString()),
						name = sensor[j].Name.ToString(),
					};

					API.CPU.temperature.Add(temp);
				}

				if (sensor[j].SensorType.ToString() == "Temperature" && computerHardware[i].HardwareType.ToString().Contains("Gpu")) {
					var temp = new CostumSensor {
						name = sensor[j].Name.ToString(),
						value = float.Parse(sensor[j].Value.ToString()),
					};

					API.GPU.temperature.Add(temp);
				}

				if (sensor[j].SensorType.ToString() == "Load" && computerHardware[i].HardwareType.ToString() == "Cpu") {
					API.CPU.Load.Add(sensor[j]);
				}

				if (computerHardware[i].HardwareType.ToString() == "Memory") {
					var temp = new CostumSensor {
						name = sensor[j].Name.ToString(),
						value = float.Parse(sensor[j].Value.ToString()),
					};

					API.RAM.load.Add(temp);
				}
			}
		}

		API.CPU.lastLoad = float.Parse(API.CPU.Load.Last().Value.ToString());
	}

	public void Refresh() {
		refresher.VisitComputer(computer);
		GetInfo();
	}

	public void Stop() {
		computer.Close();
	}
}

public class HardwareUpdater : IVisitor {
	public void VisitComputer(IComputer computer) {
		computer.Traverse(this);
	}

	public void VisitHardware(IHardware hardware) {
		hardware.Update();
		foreach (IHardware subHardware in hardware.SubHardware)
			subHardware.Accept(this);
	}

	public void VisitSensor(ISensor sensor) {
	}

	public void VisitParameter(IParameter parameter) {
	}
}

