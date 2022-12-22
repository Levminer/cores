using Cores;
using LibreHardwareMonitor.Hardware;
using System;
using System.Diagnostics;
using System.Linq;
using WindowsDisplayAPI;
using HI = Hardware.Info;

namespace cores;

public class HardwareInfo {
	public bool firstRun = true;
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
		API.CPU.Power.Clear();
		API.CPU.Clock.Clear();
		API.CPU.Voltage.Clear();

		API.GPU.Load.Clear();
		API.GPU.Temperature.Clear();
		API.GPU.Fans.Clear();
		API.GPU.Memory.Clear();
		API.GPU.Power.Clear();
		API.GPU.Clock.Clear();

		API.RAM.Load.Clear();

		var diskID = -1;

		for (int i = 0; i < computerHardware.Count; i++) {
			var sensor = computerHardware[i].Sensors;
			var diskLoad = false;

			// Get component names
			if (firstRun) {
				if (computerHardware[i].HardwareType.ToString() == "Cpu") {
					API.CPU.Name = computerHardware[i].Name;
				}

				if (computerHardware[i].HardwareType.ToString().Contains("Gpu")) {
					API.GPU.Name = computerHardware[i].Name;
				}

				if (computerHardware[i].HardwareType.ToString() == "Motherboard") {
					API.System.Motherboard.Name = computerHardware[i].Name;
				}
			}

			// Get disk names and IDs
			if (computerHardware[i].HardwareType.ToString() == "Storage") {
				var temp = new Disk {
					Name = computerHardware[i].Name,
				};

				if (firstRun) {
					API.System.Storage.Disks.Add(temp);
				}

				diskID++;
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
					API.CPU.Power.Add(new Sensor {
						Name = sensor[j].Name.ToString(),
						Value = (float)Math.Round((float)sensor[j].Value),
						Min = (float)Math.Round((float)sensor[j].Min),
						Max = (float)Math.Round((float)sensor[j].Max),
					});
				}

				// CPU load
				if (sensor[j].SensorType.ToString() == "Load" && computerHardware[i].HardwareType.ToString() == "Cpu") {
					API.CPU.Load.Add(new Load { Name = sensor[j].Name.ToString(), Value = float.Parse(sensor[j].Value.ToString()) });
				}

				// CPU clock
				if (sensor[j].SensorType.ToString() == "Clock" && computerHardware[i].HardwareType.ToString() == "Cpu" && !sensor[j].Name.Contains("Bus")) {
					API.CPU.Clock.Add(new Sensor {
						Name = sensor[j].Name.ToString(),
						Value = (float)Math.Round((float)sensor[j].Value),
						Min = (float)Math.Round((float)sensor[j].Min),
						Max = (float)Math.Round((float)sensor[j].Max),
					});
				}

				// CPU voltage
				if (sensor[j].SensorType.ToString() == "Voltage" && computerHardware[i].HardwareType.ToString() == "Cpu" && sensor[j].Name.Contains("#")) {
					API.CPU.Voltage.Add(new Sensor {
						Name = sensor[j].Name.ToString(),
						Value = (float)Math.Round((float)sensor[j].Value, 3),
						Min = (float)Math.Round((float)sensor[j].Min, 3),
						Max = (float)Math.Round((float)sensor[j].Max, 3),
					});
				}

				// GPU load
				if (sensor[j].SensorType.ToString() == "Load" && computerHardware[i].HardwareType.ToString().Contains("Gpu") && sensor[j].Name.Contains("D3D")) {
					API.GPU.Load.Add(new Load { Name = sensor[j].Name.ToString(), Value = float.Parse(sensor[j].Value.ToString()) });
				}

				// GPU fan
				if (sensor[j].SensorType.ToString() == "Fan" && computerHardware[i].HardwareType.ToString().Contains("Gpu")) {
					API.GPU.Fans.Add(new Load { Name = sensor[j].Name.ToString(), Value = float.Parse(sensor[j].Value.ToString()) });
				}

				// GPU Memory 
				if (sensor[j].SensorType.ToString() == "SmallData" && computerHardware[i].HardwareType.ToString().Contains("Gpu")) {
					API.GPU.Memory.Add(new Load { Name = sensor[j].Name.ToString(), Value = float.Parse(sensor[j].Value.ToString()) });
				}

				// GPU power
				if (sensor[j].SensorType.ToString() == "Power" && computerHardware[i].HardwareType.ToString().Contains("Gpu")) {
					API.GPU.Power.Add(new Sensor {
						Name = sensor[j].Name.ToString(),
						Value = (float)Math.Round((float)sensor[j].Value),
						Min = (float)Math.Round((float)sensor[j].Min),
						Max = (float)Math.Round((float)sensor[j].Max),
					});
				}

				// GPU clock
				if (sensor[j].SensorType.ToString() == "Clock" && computerHardware[i].HardwareType.ToString().Contains("Gpu")) {
					API.GPU.Clock.Add(new Sensor {
						Name = sensor[j].Name.ToString(),
						Value = (float)Math.Round((float)sensor[j].Value),
						Min = (float)Math.Round((float)sensor[j].Min),
						Max = (float)Math.Round((float)sensor[j].Max),
					});
				}

				// Memory load
				if (computerHardware[i].HardwareType.ToString() == "Memory") {
					API.RAM.Load.Add(new Load { Name = sensor[j].Name.ToString(), Value = float.Parse(sensor[j].Value.ToString()) });
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

		try {
			API.CPU.LastLoad = float.Parse(API.CPU.Load.Last().Value.ToString());
			API.CPU.Load.RemoveAt(API.CPU.Load.Count - 1);
			API.GPU.LastLoad = API.GPU.Load.Max(t => t.Value);
		}
		catch (Exception) {
			Debug.WriteLine("Error");
		}


		if (firstRun) {
			// HwInfo
			HwInfo.RefreshOperatingSystem();
			HwInfo.RefreshMemoryList();
			HwInfo.RefreshDriveList();
			HwInfo.RefreshVideoControllerList();
			HwInfo.RefreshCPUList(false);

			// CPU info
			API.CPU.Info = HwInfo.CpuList;

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
			var displays = Display.GetDisplays().ToArray();

			for (int i = 0; i < displays.Length; i++) {
				API.System.Monitor.Monitors.Add(new Monitor {
					Name = displays[i].ToPathDisplayTarget().FriendlyName,
					RefreshRate = Convert.ToString(displays[i].CurrentSetting.Frequency),
					Resolution = $"{displays[i].CurrentSetting.Resolution.Width}x{displays[i].CurrentSetting.Resolution.Height}",
				});
			}
		}

		firstRun = false;
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

