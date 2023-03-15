using Cores;
using LibreHardwareMonitor.Hardware;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using WindowsDisplayAPI;

namespace cores;

public class HardwareInfo {
	public bool firstRun = true;
	public HardwareUpdater refresher = new();
	public Computer computer = new() {
		IsCpuEnabled = true,
		IsGpuEnabled = true,
		IsMemoryEnabled = true,
		IsMotherboardEnabled = true,
		IsControllerEnabled = true,
		IsStorageEnabled = true
	};

	[DllImport("lib.dll")]
	private static extern string getOSInfo();

	[DllImport("lib.dll")]
	private static extern string getGPUInfo();

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
		API.GPU.Fan.Clear();
		API.GPU.Memory.Clear();
		API.GPU.Power.Clear();
		API.GPU.Clock.Clear();

		API.RAM.Load.Clear();

		var diskID = -1;

		for (int i = 0; i < computerHardware.Count; i++) {
			var sensor = computerHardware[i].Sensors;

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

				diskID++;

				if (firstRun) {
					API.System.Storage.Disks.Add(temp);

					// Get disk size
					var report = computerHardware[i].GetReport().Split("\n");
					long total = 0;
					long free = 0;
					string health = "N/A";

					foreach (var line in report) {
						if (line.StartsWith("Total Size")) {
							total = Convert.ToInt64(line.Split(":")[1].Trim()) / 1024 / 1024 / 1024;
						}

						if (line.StartsWith("Total Free Space")) {
							free = Convert.ToInt64(line.Split(":")[1].Trim()) / 1024 / 1024 / 1024;
						}

						// Sandforce
						if (line.Trim().StartsWith("E7")) {
							health = line.Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries).Last();
						}

						// Intel
						if (line.Trim().StartsWith("E8")) {
							health = line.Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries).Last();
						}

						// Samsung
						if (line.Trim().StartsWith("B4")) {
							health = line.Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries).Last();
						}

						// Indilinx
						if (line.Trim().StartsWith("D1")) {
							health = line.Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries).Last();
						}

						// Micron
						if (line.Trim().StartsWith("CA")) {
							health = line.Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries).Last();
						}
					}

					API.System.Storage.Disks[diskID].TotalSpace = (int)total;
					API.System.Storage.Disks[diskID].Health = health;
					API.System.Storage.Disks[diskID].FreeSpace = (int)free;
				}
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
						Value = (float)Math.Round((float)sensor[j].Value, 2),
						Min = (float)Math.Round((float)sensor[j].Min, 2),
						Max = (float)Math.Round((float)sensor[j].Max, 2),
					});
				}

				// GPU load
				if (sensor[j].SensorType.ToString() == "Load" && computerHardware[i].HardwareType.ToString().Contains("Gpu") && sensor[j].Name.Contains("D3D")) {
					API.GPU.Load.Add(new Load { Name = sensor[j].Name.ToString(), Value = float.Parse(sensor[j].Value.ToString()) });
				}

				// GPU fan
				if (sensor[j].SensorType.ToString() == "Fan" && computerHardware[i].HardwareType.ToString().Contains("Gpu")) {
					API.GPU.Fan.Add(new Sensor {
						Name = sensor[j].Name.ToString(),
						Value = (float)Math.Round((float)sensor[j].Value, 1),
						Min = (float)Math.Round((float)sensor[j].Min, 1),
						Max = (float)Math.Round((float)sensor[j].Max, 1),
					});
				}

				// GPU Memory 
				if (sensor[j].SensorType.ToString() == "SmallData" && computerHardware[i].HardwareType.ToString().Contains("Gpu")) {
					API.GPU.Memory.Add(new Sensor {
						Name = sensor[j].Name.ToString(),
						Value = (float)Math.Round((float)sensor[j].Value / 1024, 1),
						Min = (float)Math.Round((float)sensor[j].Min / 1024, 1),
						Max = (float)Math.Round((float)sensor[j].Max / 1024, 1),
					});
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
					API.RAM.Load.Add(new Sensor {
						Name = sensor[j].Name.ToString(),
						Value = (float)Math.Round((float)sensor[j].Value, 1),
						Min = (float)Math.Round((float)sensor[j].Min, 1),
						Max = (float)Math.Round((float)sensor[j].Max, 1),
					});
				}

				// Drive temperature
				if (computerHardware[i].HardwareType.ToString() == "Storage" && sensor[j].SensorType.ToString() == "Temperature") {
					API.System.Storage.Disks[diskID].Temperature = float.Parse(sensor[j].Value.ToString());
				}
			}
		}

		// Last loads
		try {
			API.CPU.LastLoad = float.Parse(API.CPU.Load.Last().Value.ToString());
			API.CPU.Load.RemoveAt(API.CPU.Load.Count - 1);

			API.GPU.LastLoad = API.GPU.Load.Max(t => t.Value);
		}
		catch (Exception) {
			Debug.WriteLine("Error");
		}

		// HWInfo, monitors, network interfaces
		if (firstRun) {
			// CPU info
			for (int i = 0; i < computer.SMBios.Processors.Length; i++) {
				API.CPU.Info.Add(computer.SMBios.Processors[i]);
			}

			// GPU info
			API.GPU.Info = getGPUInfo();

			// OS info
			API.System.OS.Name = getOSInfo();

			// RAM modules
			for (int i = 0; i < computer.SMBios.MemoryDevices.Length; i++) {
				if (computer.SMBios.MemoryDevices[i].Speed != 0) {
					API.RAM.Info.Add(computer.SMBios.MemoryDevices[i]);
				}

				API.RAM.Layout.Add(computer.SMBios.MemoryDevices[i]);
			}

			// Monitors
			var displays = Display.GetDisplays().ToArray();

			for (int i = 0; i < displays.Length; i++) {
				var defaultName = "N/A";
				var name = displays[i].ToPathDisplayTarget().FriendlyName;

				if (name != "") {
					defaultName = name;
				}

				API.System.Monitor.Monitors.Add(new Monitor {
					Name = defaultName,
					RefreshRate = Convert.ToString(displays[i].CurrentSetting.Frequency),
					Resolution = $"{displays[i].CurrentSetting.Resolution.Width}x{displays[i].CurrentSetting.Resolution.Height}",
					Primary = displays[i].IsGDIPrimary
				});
			}

			// BIOS info
			API.System.BIOS = new BIOSInfo {
				Vendor = computer.SMBios.Bios.Vendor,
				Version = computer.SMBios.Bios.Version,
				Date = computer.SMBios.Bios.Date.Value.ToShortDateString(),
			};

			// Network interfaces
			foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces()) {

				if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet) {
					var temp = new NetInterface {
						Name = ni.Name,
						Description = ni.Description,
						MACAddress = ni.GetPhysicalAddress().ToString(),
						DNS = ni.GetIPProperties().DnsAddresses[0].ToString(),
						Speed = (ni.Speed / 1000 / 1000).ToString()
					};

					if (ni.GetIPProperties().GatewayAddresses.Count != 0) {
						temp.Gateway = ni.GetIPProperties().GatewayAddresses[0].Address.ToString();
					} else {
						temp.Gateway = "N/A";
					}

					foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses) {
						if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) {
							temp.IPAddress = ip.Address.ToString();
							temp.Mask = ip.IPv4Mask.ToString();
						}
					}

					API.System.Network.Interfaces.Add(temp);
				}

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
		foreach (IHardware subHardware in hardware.SubHardware) {
			subHardware.Accept(this);
		}
	}

	public void VisitSensor(ISensor sensor) {
	}

	public void VisitParameter(IParameter parameter) {
	}
}

