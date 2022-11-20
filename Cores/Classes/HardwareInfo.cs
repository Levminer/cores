using LibreHardwareMonitor.Hardware;
using System.Collections.Generic;

namespace cores;

public class CPUTempI {
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

public class RAMI {
	public string name {
		get; set;
	}
	public float value {
		get; set;
	}
}

public class HardwareInfo {
	public HardwareUpdater refresher = new();
	public List<CPUTempI> CPUTemp = new();
	public List<ISensor> CPULoad = new();
	public List<RAMI> RAM = new();
	public string CPUName;
	public string GPUName;
	public Computer computer = new() {
		IsCpuEnabled = true,
		IsGpuEnabled = true,
		IsMemoryEnabled = true,
		IsMotherboardEnabled = true,
		IsControllerEnabled = true,
		IsStorageEnabled = true
	};

	public HardwareInfo() {

		computer.Open();
		computer.Accept(refresher);

		GetInfo();
	}

	public void GetInfo() {
		var computerHardware = computer.Hardware;

		CPUTemp.Clear();
		CPULoad.Clear();
		RAM.Clear();

		for (int i = 0; i < computerHardware.Count; i++) {
			var sensor = computerHardware[i].Sensors;

			if (computerHardware[i].HardwareType.ToString() == "Cpu") {
				CPUName = computerHardware[i].Name;
			}

			if (computerHardware[i].HardwareType.ToString().Contains("Gpu")) {
				GPUName = computerHardware[i].Name;
			}

			for (int j = 0; j < sensor.Length; j++) {
				if (sensor[j].SensorType.ToString() == "Temperature" && computerHardware[i].HardwareType.ToString() == "Cpu" && sensor[j].Name.StartsWith("CPU Core") && !sensor[j].Name.Contains("Tj")) {
					var temp = new CPUTempI {
						value = float.Parse(sensor[j].Value.ToString()),
						min = float.Parse(sensor[j].Min.ToString()),
						max = float.Parse(sensor[j].Max.ToString()),
						name = sensor[j].Name.ToString(),
					};

					CPUTemp.Add(temp);
				}

				if (sensor[j].SensorType.ToString() == "Load" && computerHardware[i].HardwareType.ToString() == "Cpu") {
					CPULoad.Add(sensor[j]);
				}

				if (computerHardware[i].HardwareType.ToString() == "Memory") {
					var temp = new RAMI {
						name = sensor[j].Name.ToString(),
						value = float.Parse(sensor[j].Value.ToString()),
					};

					RAM.Add(temp);
				}
			}
		}
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

