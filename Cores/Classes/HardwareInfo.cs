using LibreHardwareMonitor.Hardware;
using System.Collections.Generic;

namespace cores;

public class HardwareInfo {
	public HardwareUpdater refresher = new();
	public List<ISensor> CPUTemp = new();
	public List<ISensor> CPULoad = new();
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

		for (int i = 0; i < computerHardware.Count; i++) {
			var sensor = computerHardware[i].Sensors;

			if (computerHardware[i].HardwareType.ToString() == "Cpu") {
				CPUName = computerHardware[i].Name;
			}

			if (computerHardware[i].HardwareType.ToString().Contains("Gpu")) {
				GPUName = computerHardware[i].Name;
			}

			for (int j = 0; j < sensor.Length; j++) {
				if (sensor[j].SensorType.ToString() == "Temperature" && computerHardware[i].HardwareType.ToString() == "Cpu") {
					CPUTemp.Add(sensor[j]);
				}

				if (sensor[j].SensorType.ToString() == "Load" && computerHardware[i].HardwareType.ToString() == "Cpu") {
					CPULoad.Add(sensor[j]);
				}
			}
		}
	}

	public void Refresh() {
		refresher.VisitComputer(computer);
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

