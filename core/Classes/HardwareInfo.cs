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
		IsStorageEnabled = true,
		IsNetworkEnabled = true,
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

		API.System.SuperIO.Voltage.Clear();
		API.System.SuperIO.Temperature.Clear();
		API.System.SuperIO.Fan.Clear();
		API.System.SuperIO.FanControl.Clear();

		if (firstRun) {
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

		for (int i = 0; i < computer.Hardware.Count; i++) {
			var hardware = computer.Hardware[i];

			// Get component names
			if (firstRun) {
				if (computerHardware[i].HardwareType == HardwareType.Cpu) {
					API.CPU.Name = computerHardware[i].Name;
				}

				if (computerHardware[i].HardwareType.ToString().Contains("Gpu")) {
					API.GPU.Name = computerHardware[i].Name;
				}

				if (computerHardware[i].HardwareType == HardwareType.Motherboard) {
					API.System.Motherboard.Name = computerHardware[i].Name;

					if (computerHardware[i].SubHardware.Length != 0) {
						API.System.SuperIO.Name = computerHardware[i].SubHardware[0].Name;
					}
				}
			}

			// CPU
			if (hardware.HardwareType == HardwareType.Cpu) {
				var sensor = hardware.Sensors;

				for (int j = 0; j < hardware.Sensors.Length; j++) {
					// CPU temperature
					if (sensor[j].SensorType == SensorType.Temperature && sensor[j].Name.StartsWith("CPU Core") && !sensor[j].Name.Contains("Tj")) {
						API.CPU.Temperature.Add(new Sensor {
							Name = sensor[j].Name,
							Value = sensor[j].Value ?? 0,
							Min = sensor[j].Min ?? 0,
							Max = sensor[j].Max ?? 0,
						});
					}

					// CPU power
					if (sensor[j].SensorType == SensorType.Power) {
						API.CPU.Power.Add(new Sensor {
							Name = sensor[j].Name,
							Value = (float)Math.Round(sensor[j].Value ?? 0),
							Min = (float)Math.Round(sensor[j].Min ?? 0),
							Max = (float)Math.Round(sensor[j].Max ?? 0),
						});
					}

					// CPU load
					if (sensor[j].SensorType == SensorType.Load) {
						API.CPU.Load.Add(new Sensor {
							Name = sensor[j].Name,
							Value = sensor[j].Value ?? 0,
							Min = sensor[j].Min ?? 0,
							Max = sensor[j].Max ?? 0,
						});
					}

					// CPU clock
					if (sensor[j].SensorType == SensorType.Clock && !sensor[j].Name.Contains("Bus")) {
						API.CPU.Clock.Add(new Sensor {
							Name = sensor[j].Name,
							Value = (float)Math.Round(sensor[j].Value ?? 0),
							Min = (float)Math.Round(sensor[j].Min ?? 0),
							Max = (float)Math.Round(sensor[j].Max ?? 0),
						});
					}

					// CPU voltage
					if (sensor[j].SensorType == SensorType.Voltage && sensor[j].Name.Contains("#")) {
						API.CPU.Voltage.Add(new Sensor {
							Name = sensor[j].Name.ToString(),
							Value = (float)Math.Round(sensor[j].Value ?? 0, 2),
							Min = (float)Math.Round(sensor[j].Min ?? 0, 2),
							Max = (float)Math.Round(sensor[j].Max ?? 0, 2),
						});
					}
				}
			}

			// GPU
			if (hardware.HardwareType.ToString().Contains("Gpu")) {
				var sensor = hardware.Sensors;

				for (int j = 0; j < hardware.Sensors.Length; j++) {
					// GPU temperature
					if (sensor[j].SensorType == SensorType.Temperature) {
						API.GPU.Temperature.Add(new Sensor {
							Name = sensor[j].Name,
							Value = (float)Math.Round(sensor[j].Value ?? 0),
							Min = (float)Math.Round(sensor[j].Min ?? 0),
							Max = (float)Math.Round(sensor[j].Max ?? 0),
						});
					}

					// GPU load
					if (sensor[j].SensorType == SensorType.Load && sensor[j].Name.Contains("D3D")) {
						API.GPU.Load.Add(new Sensor {
							Name = sensor[j].Name,
							Value = sensor[j].Value ?? 0,
							Min = sensor[j].Min ?? 0,
							Max = sensor[j].Max ?? 0,
						});
					}

					// GPU fan
					if (sensor[j].SensorType == SensorType.Fan) {
						API.GPU.Fan.Add(new Sensor {
							Name = sensor[j].Name,
							Value = (float)Math.Round(sensor[j].Value ?? 0),
							Min = (float)Math.Round(sensor[j].Min ?? 0),
							Max = (float)Math.Round(sensor[j].Max ?? 0),
						});
					}

					// GPU Memory 
					if (sensor[j].SensorType == SensorType.SmallData) {
						API.GPU.Memory.Add(new Sensor {
							Name = sensor[j].Name,
							Value = (float)Math.Round(sensor[j].Value ?? 0 / 1024, 1),
							Min = (float)Math.Round(sensor[j].Min ?? 0 / 1024, 1),
							Max = (float)Math.Round(sensor[j].Max ?? 0 / 1024, 1),
						});
					}

					// GPU power
					if (sensor[j].SensorType == SensorType.Power) {
						API.GPU.Power.Add(new Sensor {
							Name = sensor[j].Name,
							Value = (float)Math.Round(sensor[j].Value ?? 0),
							Min = (float)Math.Round(sensor[j].Min ?? 0),
							Max = (float)Math.Round(sensor[j].Max ?? 0),
						});
					}

					// GPU clock
					if (sensor[j].SensorType == SensorType.Clock) {
						API.GPU.Clock.Add(new Sensor {
							Name = sensor[j].Name,
							Value = (float)Math.Round(sensor[j].Value ?? 0),
							Min = (float)Math.Round(sensor[j].Min ?? 0),
							Max = (float)Math.Round(sensor[j].Max ?? 0),
						});
					}
				}
			}

			// RAM
			if (hardware.HardwareType == HardwareType.Memory) {
				var sensor = hardware.Sensors;

				for (int j = 0; j < hardware.Sensors.Length; j++) {
					// RAM load
					API.RAM.Load.Add(new Sensor {
						Name = sensor[j].Name,
						Value = (float)Math.Round(sensor[j].Value ?? 0, 1),
						Min = (float)Math.Round(sensor[j].Min ?? 0, 1),
						Max = (float)Math.Round(sensor[j].Max ?? 0, 1),
					});
				}
			}

			// Storage
			if (hardware.HardwareType == HardwareType.Storage) {
				var sensor = hardware.Sensors;

				if (firstRun) {
					var temp = new Disk {
						Name = computerHardware[i].Name,
						Id = computerHardware[i].Identifier,
					};

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

					temp.TotalSpace = (int)total;
					temp.Health = health;
					temp.FreeSpace = (int)free;

					API.System.Storage.Disks.Add(temp);
				}

				for (int j = 0; j < hardware.Sensors.Length; j++) {
					// Drive temperature
					if (sensor[j].SensorType == SensorType.Temperature) {
						// find disk by id and overwrite value
						for (int k = 0; k < API.System.Storage.Disks.Count; k++) {
							if (API.System.Storage.Disks[k].Id == computerHardware[i].Identifier) {
								var min = (float)Math.Round(sensor[j].Min ?? 0);

								// Some drives don't return min temp
								if (firstRun && min == 0) {
									min = (float)Math.Round(sensor[j].Value ?? 0);
								}

								// Replace min temp if current temp is lower
								if (min == 0) {
									if ((float)Math.Round(sensor[j].Value ?? 0) < API.System.Storage.Disks[k].Temperature.Min) {
										min = (float)Math.Round(sensor[j].Value ?? 0);
									} else {
										min = API.System.Storage.Disks[k].Temperature.Min;
									}
								}

								API.System.Storage.Disks[k].Temperature = new Sensor {
									Name = sensor[j].Name,
									Value = (float)Math.Round(sensor[j].Value ?? 0),
									Min = min,
									Max = (float)Math.Round(sensor[j].Max ?? 0),
								};
							}
						}
					}

					// Drive throughput
					if (sensor[j].SensorType == SensorType.Throughput) {
						// find disk by ide and overwrite value
						for (int k = 0; k < API.System.Storage.Disks.Count; k++) {
							if (API.System.Storage.Disks[k].Id == computerHardware[i].Identifier) {
								if (sensor[j].Name.Contains("Read")) {
									if (sensor[j].Value.ToString() == "0" || sensor[j].Value == null) {
										API.System.Storage.Disks[k].ThroughputRead = 0;
									} else {
										API.System.Storage.Disks[k].ThroughputRead = (float)Math.Round(sensor[j].Value ?? 0, 1);
									}
								}

								if (sensor[j].Name.Contains("Write")) {
									if (sensor[j].Value.ToString() == "0" || sensor[j].Value == null) {
										API.System.Storage.Disks[k].ThroughputWrite = 0;
									} else {
										API.System.Storage.Disks[k].ThroughputWrite = (float)Math.Round(sensor[j].Value ?? 0, 1);
									}
								}
							}
						}
					}

					// Drive data
					if (sensor[j].SensorType == SensorType.Data) {
						// find disk by ide and overwrite value
						for (int k = 0; k < API.System.Storage.Disks.Count; k++) {
							if (API.System.Storage.Disks[k].Id == computerHardware[i].Identifier) {
								if (sensor[j].Name.Contains("Read")) {
									API.System.Storage.Disks[k].DataRead = (float)Math.Round(sensor[j].Value ?? 0, 1);
								}

								if (sensor[j].Name.Contains("Written")) {
									API.System.Storage.Disks[k].DataWritten = (float)Math.Round(sensor[j].Value ?? 0, 1);
								}
							}
						}
					}
				}
			}

			// superIO
			if (hardware.HardwareType == HardwareType.Motherboard && computerHardware[i].SubHardware.Length != 0) {
				var sh = computerHardware[i].SubHardware[0];

				for (int j = 0; j < sh.Sensors.Length; j++) {
					if (sh.Sensors[j].SensorType == SensorType.Voltage) {
						API.System.SuperIO.Voltage.Add(new Sensor {
							Name = sh.Sensors[j].Name,
							Value = (float)Math.Round(sh.Sensors[j].Value ?? 0, 2),
							Min = (float)Math.Round(sh.Sensors[j].Min ?? 0, 2),
							Max = (float)Math.Round(sh.Sensors[j].Max ?? 0, 2),
						});
					}

					if (sh.Sensors[j].SensorType == SensorType.Temperature) {
						API.System.SuperIO.Temperature.Add(new Sensor {
							Name = sh.Sensors[j].Name,
							Value = (float)Math.Round(sh.Sensors[j].Value ?? 0),
							Min = (float)Math.Round(sh.Sensors[j].Min ?? 0),
							Max = (float)Math.Round(sh.Sensors[j].Max ?? 0),
						});
					}

					if (sh.Sensors[j].SensorType == SensorType.Fan) {
						API.System.SuperIO.Fan.Add(new Sensor {
							Name = sh.Sensors[j].Name,
							Value = (float)Math.Round(sh.Sensors[j].Value ?? 0),
							Min = (float)Math.Round(sh.Sensors[j].Min ?? 0),
							Max = (float)Math.Round(sh.Sensors[j].Max ?? 0),
						});
					}

					if (sh.Sensors[j].SensorType == SensorType.Control) {
						API.System.SuperIO.FanControl.Add(new Sensor {
							Name = sh.Sensors[j].Name,
							Value = (float)Math.Round(sh.Sensors[j].Value ?? 0),
							Min = (float)Math.Round(sh.Sensors[j].Min ?? 0),
							Max = (float)Math.Round(sh.Sensors[j].Max ?? 0),
						});
					}
				}
			}

			// Network
			if (hardware.HardwareType == HardwareType.Network) {
				var sensor = hardware.Sensors;

				for (int j = 0; j < hardware.Sensors.Length; j++) {
					// Throughput
					if (sensor[j].SensorType == SensorType.Throughput) {
						// find interface by name and overwrite value
						for (int k = 0; k < API.System.Network.Interfaces.Count; k++) {
							if (API.System.Network.Interfaces[k].Name == computerHardware[i].Name) {
								if (sensor[j].Name.Contains("Download")) {
									API.System.Network.Interfaces[k].ThroughputDownload = (float)Math.Round(sensor[j].Value ?? 0);
								}

								if (sensor[j].Name.Contains("Upload")) {
									API.System.Network.Interfaces[k].ThroughputUpload = (float)Math.Round(sensor[j].Value ?? 0);
								}
							}
						}

					}

					// Load
					if (sensor[j].SensorType == SensorType.Data) {
						// find interface by name and overwrite value
						for (int k = 0; k < API.System.Network.Interfaces.Count; k++) {
							if (API.System.Network.Interfaces[k].Name == computerHardware[i].Name) {
								if (sensor[j].Name.Contains("Download")) {
									API.System.Network.Interfaces[k].DownloadData = (float)Math.Round(sensor[j].Value ?? 0, 1);
								}

								if (sensor[j].Name.Contains("Upload")) {
									API.System.Network.Interfaces[k].UploadData = (float)Math.Round(sensor[j].Value ?? 0, 1);
								}
							}
						}
					}
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

			try {
				// GPU info
				API.GPU.Info = getGPUInfo();

				// OS info
				API.System.OS.Name = getOSInfo();
			}
			catch (Exception) {
				Debug.WriteLine("Failed to get GPU and OS name");
			}

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
			var biosDate = computer.SMBios.Bios.Date ?? new DateTime(1970, 01, 01);

			API.System.BIOS = new BIOSInfo {
				Vendor = computer.SMBios.Bios.Vendor,
				Version = computer.SMBios.Bios.Version,
				Date = biosDate.ToShortDateString(),
			};
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

