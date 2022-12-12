using Cores;
using LibreHardwareMonitor.Hardware;
using System;
using System.Linq;
using WindowsDisplayAPI.DisplayConfig;
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

		API.CPU.Temperature.Clear();
		API.CPU.Load.Clear();
		API.GPU.Load.Clear();
		API.RAM.Load.Clear();
		API.GPU.Temperature.Clear();
		API.System.Storage.Disks.Clear();
		API.GPU.Fans.Clear();
		API.GPU.Memory.Clear();
		API.CPU.Power.Clear();
		API.GPU.Power.Clear();
		API.System.Monitor.Monitors.Clear();

		var diskID = -1;

		for (int i = 0; i < computerHardware.Count; i++) {
			var sensor = computerHardware[i].Sensors;
			var diskLoad = false;

			if (computerHardware[i].HardwareType.ToString() == "Cpu") {
				API.CPU.Name = computerHardware[i].Name;
			}

			if (computerHardware[i].HardwareType.ToString().Contains("Gpu")) {
				API.GPU.Name = computerHardware[i].Name;
			}

			if (computerHardware[i].HardwareType.ToString() == "Storage") {
				var temp = new Disk {
					Name = computerHardware[i].Name,
				};

				API.System.Storage.Disks.Add(temp);
				diskID++;
			}

			if (computerHardware[i].HardwareType.ToString() == "Motherboard") {
				API.System.Motherboard.Name = computerHardware[i].Name;
			}

			for (int j = 0; j < sensor.Length; j++) {
				// CPU temperature
				if (sensor[j].SensorType.ToString() == "Temperature" && computerHardware[i].HardwareType.ToString() == "Cpu" && sensor[j].Name.StartsWith("CPU Core") && !sensor[j].Name.Contains("Tj")) {
					var temp = new Sensor {
						Value = float.Parse(sensor[j].Value.ToString()),
						Min = float.Parse(sensor[j].Min.ToString()),
						Max = float.Parse(sensor[j].Max.ToString()),
						Name = sensor[j].Name.ToString(),
					};

					API.CPU.Temperature.Add(temp);
				}

				// GPU temperature
				if (sensor[j].SensorType.ToString() == "Temperature" && computerHardware[i].HardwareType.ToString().Contains("Gpu")) {
					var temp = new Sensor {
						Name = sensor[j].Name.ToString(),
						Value = (float)Math.Truncate(float.Parse(sensor[j].Value.ToString())),
						Min = (float)Math.Truncate(float.Parse(sensor[j].Min.ToString())),
						Max = (float)Math.Truncate(float.Parse(sensor[j].Max.ToString())),
					};

					API.GPU.Temperature.Add(temp);
				}

				// CPU power
				if (sensor[j].SensorType.ToString() == "Power" && computerHardware[i].HardwareType.ToString() == "Cpu") {
					API.CPU.Power.Add(new Load { Name = sensor[j].Name.ToString(), Value = float.Parse(sensor[j].Value.ToString()) });
				}

				// CPU load
				if (sensor[j].SensorType.ToString() == "Load" && computerHardware[i].HardwareType.ToString() == "Cpu") {
					API.CPU.Load.Add(new Load { Name = sensor[j].Name.ToString(), Value = float.Parse(sensor[j].Value.ToString()) });
				}

				// GPU load
				if (sensor[j].SensorType.ToString() == "Load" && computerHardware[i].HardwareType.ToString().Contains("Gpu") && sensor[j].Name.Contains("D3D")) {
					API.GPU.Load.Add(new Load { Name = sensor[j].Name.ToString(), Value = float.Parse(sensor[j].Value.ToString()) });
				}

				// GPU Fan
				if (sensor[j].SensorType.ToString() == "Fan" && computerHardware[i].HardwareType.ToString().Contains("Gpu")) {
					var temp = new Load {
						Name = sensor[j].Name.ToString(),
						Value = float.Parse(sensor[j].Value.ToString()),
					};

					API.GPU.Fans.Add(temp);
				}

				// GPU Memory 
				if (sensor[j].SensorType.ToString() == "SmallData" && computerHardware[i].HardwareType.ToString().Contains("Gpu")) {
					var temp = new Load {
						Name = sensor[j].Name.ToString(),
						Value = float.Parse(sensor[j].Value.ToString()),
					};

					API.GPU.Memory.Add(temp);
				}

				// CPU power
				if (sensor[j].SensorType.ToString() == "Power" && computerHardware[i].HardwareType.ToString().Contains("Gpu")) {
					API.GPU.Power.Add(new Load { Name = sensor[j].Name.ToString(), Value = float.Parse(sensor[j].Value.ToString()) });
				}

				// Memory load
				if (computerHardware[i].HardwareType.ToString() == "Memory") {
					var temp = new Load {
						Name = sensor[j].Name.ToString(),
						Value = float.Parse(sensor[j].Value.ToString()),
					};

					API.RAM.Load.Add(temp);
				}

				// Disk info
				if (computerHardware[i].HardwareType.ToString() == "Storage") {
					if (sensor[j].SensorType.ToString() == "Temperature") {
						API.System.Storage.Disks[diskID].Temperature = float.Parse(sensor[j].Value.ToString());
					} else if (sensor[j].SensorType.ToString() == "Load" && diskLoad == false) {
						API.System.Storage.Disks[diskID].UsedSpace = float.Parse(sensor[j].Value.ToString());
						diskLoad = true;
					}
				}
			}
		}

		API.CPU.LastLoad = float.Parse(API.CPU.Load.Last().Value.ToString());
		try {
			API.GPU.LastLoad = API.GPU.Load.Max(t => t.Value);
		}
		catch (Exception) {
			// error
		}

		// HwInfo
		HwInfo.RefreshOperatingSystem();
		HwInfo.RefreshMemoryList();
		HwInfo.RefreshDriveList();
		HwInfo.RefreshVideoControllerList();

		// Drive info
		for (int i = 0; i < HwInfo.DriveList.Count; i++) {
			API.System.Storage.Disks[i].Size = Convert.ToInt32(HwInfo.DriveList[i].Size / 1024 / 1024 / 1024);
		}

		// OS name
		var arch = System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.ToString().ToLower();
		API.System.OS.Name = $"{HwInfo.OperatingSystem.Name.Replace("Microsoft", "")} {arch} {HwInfo.OperatingSystem.Version}";

		// RAM modules
		API.RAM.Modules = HwInfo.MemoryList;

		// Monitors
		var displayNames = PathDisplayTarget.GetDisplayTargets().ToArray();

		for (int i = 0; i < displayNames.Length; i++) {
			var displaySettings = displayNames[i].ToDisplayDevice().GetPreferredSetting();

			API.System.Monitor.Monitors.Add(new Monitor {
				Name = displayNames[i].FriendlyName,
				RefreshRate = Convert.ToString(displaySettings.Frequency),
				Resolution = $"{displaySettings.Resolution.Width}x{displaySettings.Resolution.Height}",
			});
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

