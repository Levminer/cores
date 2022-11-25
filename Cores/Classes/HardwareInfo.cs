using Cores;
using LibreHardwareMonitor.Hardware;
using System.Linq;
using HI = Hardware.Info;

namespace cores;

public class HardwareInfo {
	public HardwareUpdater refresher = new();
	public HI.IHardwareInfo HwInfo = new HI.HardwareInfo();
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
		API.CPU.load.Clear();
		API.GPU.load.Clear();
		API.RAM.load.Clear();
		API.GPU.temperature.Clear();
		API.STORAGE.disks.Clear();

		var diskID = -1;

		for (int i = 0; i < computerHardware.Count; i++) {
			var sensor = computerHardware[i].Sensors;
			var diskLoad = false;

			if (computerHardware[i].HardwareType.ToString() == "Cpu") {
				API.CPU.name = computerHardware[i].Name;
			}

			if (computerHardware[i].HardwareType.ToString().Contains("Gpu")) {
				API.GPU.name = computerHardware[i].Name;
			}

			if (computerHardware[i].HardwareType.ToString() == "Storage") {
				var temp = new Disk {
					name = computerHardware[i].Name,
				};

				API.STORAGE.disks.Add(temp);
				diskID++;
			}

			if (computerHardware[i].HardwareType.ToString() == "Motherboard") {
				API.MB.name = computerHardware[i].Name;
			}

			for (int j = 0; j < sensor.Length; j++) {
				// CPU temperature
				if (sensor[j].SensorType.ToString() == "Temperature" && computerHardware[i].HardwareType.ToString() == "Cpu" && sensor[j].Name.StartsWith("CPU Core") && !sensor[j].Name.Contains("Tj")) {
					var temp = new CostumSensor {
						value = float.Parse(sensor[j].Value.ToString()),
						min = float.Parse(sensor[j].Min.ToString()),
						max = float.Parse(sensor[j].Max.ToString()),
						name = sensor[j].Name.ToString(),
					};

					API.CPU.temperature.Add(temp);
				}

				// GPU temperature
				if (sensor[j].SensorType.ToString() == "Temperature" && computerHardware[i].HardwareType.ToString().Contains("Gpu")) {
					var temp = new CostumSensor {
						name = sensor[j].Name.ToString(),
						value = float.Parse(sensor[j].Value.ToString()),
					};

					API.GPU.temperature.Add(temp);
				}

				// CPU load
				if (sensor[j].SensorType.ToString() == "Load" && computerHardware[i].HardwareType.ToString() == "Cpu") {
					API.CPU.load.Add(sensor[j]);
				}

				// GPU load
				if (sensor[j].SensorType.ToString() == "Load" &&
					computerHardware[i].HardwareType.ToString().Contains("Gpu") &&
					!sensor[j].Name.Contains("Power") &&
					!sensor[j].Name.Contains("Core") &&
					!sensor[j].Name.Contains("Controller")) {
					API.GPU.load.Add(sensor[j]);
				}

				// Memory load
				if (computerHardware[i].HardwareType.ToString() == "Memory") {
					var temp = new NameValue {
						name = sensor[j].Name.ToString(),
						value = float.Parse(sensor[j].Value.ToString()),
					};

					API.RAM.load.Add(temp);
				}

				// Disk info
				if (computerHardware[i].HardwareType.ToString() == "Storage") {
					if (sensor[j].SensorType.ToString() == "Temperature") {
						API.STORAGE.disks[diskID].temperature = float.Parse(sensor[j].Value.ToString());
					} else if (sensor[j].SensorType.ToString() == "Load" && diskLoad == false) {
						API.STORAGE.disks[diskID].usedSpace = float.Parse(sensor[j].Value.ToString());
						diskLoad = true;
					}
				}
			}
		}

		API.CPU.lastLoad = float.Parse(API.CPU.load.Last().Value.ToString());
		API.GPU.lastLoad = float.Parse(API.GPU.load.Max(t => t.Value).Value.ToString());

		// HwInfo
		HwInfo.RefreshOperatingSystem();

		API.OS.name = $"{HwInfo.OperatingSystem.Name} {HwInfo.OperatingSystem.Version}";
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

