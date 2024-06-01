using LibreHardwareMonitor.Hardware;
using System.Diagnostics;
using System.Net.NetworkInformation;
using WindowsDisplayAPI;

namespace lib;

public class HardwareInfo {
	public bool firstRun = true;
	public HardwareUpdater refresher = new();
	public Commands commands = new();
	public Computer computer = new() {
		IsCpuEnabled = true,
		IsGpuEnabled = true,
		IsMemoryEnabled = true,
		IsMotherboardEnabled = true,
		IsStorageEnabled = true,
		IsNetworkEnabled = true,
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
		try {
			var computerHardware = computer.Hardware;

			if (firstRun) {
				// Network interfaces
				foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces()) {
					if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet) {
						var temp = new NetInterface {
							Name = ni.Name,
							Id = new Identifier("nic", ni.Id),
							Description = ni.Description,
							Speed = (ni.Speed / 1000 / 1000).ToString()
						};

						// Mac address
						var mac = ni.GetPhysicalAddress().ToString();
						temp.MACAddress = string.Join(":", Enumerable.Range(0, mac.Length)
													.Where(x => x % 2 == 0)
													.Select(x => mac.Substring(x, 2))
													.ToArray());

						// DNS
						if (ni.GetIPProperties().DnsAddresses.Count != 0) {
							temp.DNS = ni.GetIPProperties().DnsAddresses[0].ToString();
						} else {
							temp.DNS = "N/A";
						}

						// Gateway
						if (ni.GetIPProperties().GatewayAddresses.Count != 0) {
							temp.Gateway = ni.GetIPProperties().GatewayAddresses[0].Address.ToString();
						} else {
							temp.Gateway = "N/A";
						}

						// Current IP
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
					var temperatureSensors = hardware.Sensors.Where(x => x.SensorType == SensorType.Temperature && x.Name.StartsWith("CPU Core") && !x.Name.Contains("Tj")).ToArray();
					var loadSensors = hardware.Sensors.Where(x => x.SensorType == SensorType.Load).ToArray();
					var powerSensors = hardware.Sensors.Where(x => x.SensorType == SensorType.Power).ToArray();
					var clockSensors = hardware.Sensors.Where(x => x.SensorType == SensorType.Clock && !x.Name.Contains("Bus")).ToArray();
					var voltageSensors = hardware.Sensors.Where(x => x.SensorType == SensorType.Voltage && x.Name.Contains('#')).ToArray();

					// CPU Temperature
					for (int j = 0; j < temperatureSensors.Length; j++) {
						var data = new Sensor {
							Name = temperatureSensors[j].Name,
							Value = temperatureSensors[j].Value ?? 0,
							Min = temperatureSensors[j].Min ?? 0,
							Max = temperatureSensors[j].Max ?? 0,
						};

						if (firstRun) {
							API.CPU.Temperature.Add(data);
						} else {
							API.CPU.Temperature[j] = data;
						}
					}

					// CPU Power
					for (int j = 0; j < powerSensors.Length; j++) {
						var data2 = new Sensor {
							Name = powerSensors[j].Name,
							Value = (float)Math.Round(powerSensors[j].Value ?? 0),
							Min = (float)Math.Round(powerSensors[j].Min ?? 0),
							Max = (float)Math.Round(powerSensors[j].Max ?? 0),
						};

						if (firstRun) {
							API.CPU.Power.Add(data2);
						} else {
							API.CPU.Power[j] = data2;
						}
					}

					// CPU Load
					for (int j = 0; j < loadSensors.Length; j++) {
						var data = new Sensor {
							Name = loadSensors[j].Name,
							Value = loadSensors[j].Value ?? 0,
							Min = loadSensors[j].Min ?? 0,
							Max = loadSensors[j].Max ?? 0,
						};

						if (!loadSensors[j].Name.Contains("Total") && !loadSensors[j].Name.Contains("Max")) {
							if (firstRun) {
								API.CPU.Load.Add(data);
							} else {
								API.CPU.Load[j] = data;
							}
						}

						// This is the last in the array, might be a problem if its not last
						if (loadSensors[j].Name.Contains("Total")) {
							API.CPU.MaxLoad = loadSensors[j].Value ?? 0;
						}
					}

					// CPU Clock
					for (int j = 0; j < clockSensors.Length; j++) {
						var data = new Sensor {
							Name = clockSensors[j].Name,
							Value = (float)Math.Round(clockSensors[j].Value ?? 0),
							Min = (float)Math.Round(clockSensors[j].Min ?? 0),
							Max = (float)Math.Round(clockSensors[j].Max ?? 0),
						};


						if (firstRun) {
							API.CPU.Clock.Add(data);
						} else {
							API.CPU.Clock[j] = data;
						}
					}

					// CPU Voltage
					for (int j = 0; j < voltageSensors.Length; j++) {
						var data = new Sensor {
							Name = voltageSensors[j].Name.ToString(),
							Value = (float)Math.Round(voltageSensors[j].Value ?? 0, 2),
							Min = (float)Math.Round(voltageSensors[j].Min ?? 0, 2),
							Max = (float)Math.Round(voltageSensors[j].Max ?? 0, 2),
						};


						if (firstRun) {
							API.CPU.Voltage.Add(data);
						} else {
							API.CPU.Voltage[j] = data;
						}
					}
				}

				// GPU
				if (hardware.HardwareType.ToString().Contains("Gpu")) {
					var temperatureSensors = hardware.Sensors.Where(x => x.SensorType == SensorType.Temperature).ToArray();
					var fanSensors = hardware.Sensors.Where(x => x.SensorType == SensorType.Fan).ToArray();
					var memorySensors = hardware.Sensors.Where(x => x.SensorType == SensorType.SmallData).ToArray();
					var powerSensors = hardware.Sensors.Where(x => x.SensorType == SensorType.Power).ToArray();
					var clockSensors = hardware.Sensors.Where(x => x.SensorType == SensorType.Clock).ToArray();

					// GPU Temperature
					for (int j = 0; j < temperatureSensors.Length; j++) {
						var data = new Sensor {
							Name = temperatureSensors[j].Name,
							Value = (float)Math.Round(temperatureSensors[j].Value ?? 0),
							Min = (float)Math.Round(temperatureSensors[j].Min ?? 0),
							Max = (float)Math.Round(temperatureSensors[j].Max ?? 0),
						};

						if (firstRun) {
							API.GPU.Temperature.Add(data);
						} else {
							API.GPU.Temperature[j] = data;
						}
					}

					// GPU Fan
					for (int j = 0; j < fanSensors.Length; j++) {
						var data = new Sensor {
							Name = fanSensors[j].Name,
							Value = (float)Math.Round(fanSensors[j].Value ?? 0),
							Min = (float)Math.Round(fanSensors[j].Min ?? 0),
							Max = (float)Math.Round(fanSensors[j].Max ?? 0),
						};

						if (firstRun) {
							API.GPU.Fan.Add(data);
						} else {
							API.GPU.Fan[j] = data;
						}
					}

					// GPU Memory
					for (int j = 0; j < memorySensors.Length; j++) {
						var data = new Sensor {
							Name = memorySensors[j].Name,
							Value = (float)Math.Round(memorySensors[j].Value / 1024 ?? 0, 1),
							Min = (float)Math.Round(memorySensors[j].Min / 1024 ?? 0, 1),
							Max = (float)Math.Round(memorySensors[j].Max / 1024 ?? 0, 1),
						};

						if (firstRun) {
							API.GPU.Memory.Add(data);
						} else {
							API.GPU.Memory[j] = data;
						}
					}

					// GPU Power
					for (int j = 0; j < powerSensors.Length; j++) {
						var data = new Sensor {
							Name = powerSensors[j].Name,
							Value = (float)Math.Round(powerSensors[j].Value ?? 0),
							Min = (float)Math.Round(powerSensors[j].Min ?? 0),
							Max = (float)Math.Round(powerSensors[j].Max ?? 0),
						};

						if (firstRun) {
							API.GPU.Power.Add(data);
						} else {
							API.GPU.Power[j] = data;
						}
					}

					// GPU Clock
					for (int j = 0; j < clockSensors.Length; j++) {
						var data = new Sensor {
							Name = clockSensors[j].Name,
							Value = (float)Math.Round(clockSensors[j].Value ?? 0),
							Min = (float)Math.Round(clockSensors[j].Min ?? 0),
							Max = (float)Math.Round(clockSensors[j].Max ?? 0),
						};

						if (firstRun) {
							API.GPU.Clock.Add(data);
						} else {
							API.GPU.Clock[j] = data;
						}
					}

					// Get initial GPU Load
					if (firstRun) {
						API.GPU.Load = new List<Sensor> { new() { Name = "3D" }, new() { Name = "Copy" }, new() { Name = "Video Encode" }, new() { Name = "Video Decode" } };
					}

					// Get GPU Load
					Task.Run(async () => {
						try {
							var GPULoad = new GPULoad();
							await GPULoad.GetInfo();

							API.GPU.Load = GPULoad.Load;
							API.GPU.MaxLoad = GPULoad.MaxLoad;
						}
						catch (Exception) {
							Debug.WriteLine("Error in GPULoad");
						}
					});
				}

				// RAM
				if (hardware.HardwareType == HardwareType.Memory) {
					var sensor = hardware.Sensors;

					for (int j = 0; j < hardware.Sensors.Length; j++) {
						var data = new Sensor {
							Name = sensor[j].Name,
							Value = (float)Math.Round(sensor[j].Value ?? 0, 1),
							Min = (float)Math.Round(sensor[j].Min ?? 0, 1),
							Max = (float)Math.Round(sensor[j].Max ?? 0, 1),
						};

						// RAM load
						if (firstRun) {
							API.RAM.Load.Add(data);
						} else {
							API.RAM.Load[j] = data;
						}
					}
				}

				// Storage
				if (hardware.HardwareType == HardwareType.Storage) {
					var sensor = hardware.Sensors;

					if (firstRun) {
						var data = new Disk {
							Name = computerHardware[i].Name,
							Id = computerHardware[i].Identifier,
						};

						// Get disk size
						var report = computerHardware[i].GetReport().Split("\n");
						long total = 0;
						long free = 0;
						string health = "N/A";
						bool systemDrive = false;

						foreach (var line in report) {
							if (line.StartsWith("Total Size")) {
								total = Convert.ToInt64(line.Split(":")[1].Trim()) / 1024 / 1024 / 1024;
							}

							if (line.StartsWith("Total Free Space")) {
								free = Convert.ToInt64(line.Split(":")[1].Trim()) / 1024 / 1024 / 1024;
							}

							if (line.StartsWith("Logical Drive Name: C")) {
								systemDrive = true;
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

						data.TotalSpace = (int)total;
						data.Health = health;
						data.FreeSpace = (int)free;
						data.SystemDrive = systemDrive;

						API.System.Storage.Disks.Add(data);
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
							// find disk by id and overwrite value
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

						// M.2 SSD Health
						if (sensor[j].SensorType == SensorType.Level && firstRun) {
							// find disk by id and overwrite value
							for (int k = 0; k < API.System.Storage.Disks.Count; k++) {
								if (API.System.Storage.Disks[k].Id == computerHardware[i].Identifier) {
									if (sensor[j].Name.Contains("Percentage Used")) {
										API.System.Storage.Disks[k].Health = (100 - sensor[j].Value ?? 0).ToString();
									}
								}
							}
						}
					}
				}

				// superIO
				if (hardware.HardwareType == HardwareType.Motherboard && computerHardware[i].SubHardware.Length != 0) {
					var sh = computerHardware[i].SubHardware[0];

					var voltageSensors = sh.Sensors.Where(x => x.SensorType == SensorType.Voltage).ToArray();
					var temperatureSensors = sh.Sensors.Where(x => x.SensorType == SensorType.Temperature).ToArray();
					var fanSensors = sh.Sensors.Where(x => x.SensorType == SensorType.Fan).ToArray();
					var fanControlSensors = sh.Sensors.Where(x => x.SensorType == SensorType.Control).ToArray();


					for (int j = 0; j < voltageSensors.Length; j++) {
						var data = new Sensor {
							Name = voltageSensors[j].Name,
							Value = (float)Math.Round(voltageSensors[j].Value ?? 0, 2),
							Min = (float)Math.Round(voltageSensors[j].Min ?? 0, 2),
							Max = (float)Math.Round(voltageSensors[j].Max ?? 0, 2),
						};

						if (firstRun) {
							API.System.SuperIO.Voltage.Add(data);
						} else {
							API.System.SuperIO.Voltage[j] = data;
						}

					}

					for (int j = 0; j < temperatureSensors.Length; j++) {
						var data = new Sensor {
							Name = temperatureSensors[j].Name,
							Value = (float)Math.Round(temperatureSensors[j].Value ?? 0),
							Min = (float)Math.Round(temperatureSensors[j].Min ?? 0),
							Max = (float)Math.Round(temperatureSensors[j].Max ?? 0),
						};

						if (firstRun) {
							API.System.SuperIO.Temperature.Add(data);
						} else {
							API.System.SuperIO.Temperature[j] = data;
						}
					}

					for (int j = 0; j < fanSensors.Length; j++) {
						var data = new Sensor {
							Name = fanSensors[j].Name,
							Value = (float)Math.Round(fanSensors[j].Value ?? 0),
							Min = (float)Math.Round(fanSensors[j].Min ?? 0),
							Max = (float)Math.Round(fanSensors[j].Max ?? 0),
						};

						if (firstRun) {
							API.System.SuperIO.Fan.Add(data);
						} else {
							API.System.SuperIO.Fan[j] = data;
						}
					}

					for (int j = 0; j < fanControlSensors.Length; j++) {
						var data = new Sensor {
							Name = fanControlSensors[j].Name,
							Value = (float)Math.Round(fanControlSensors[j].Value ?? 0),
							Min = (float)Math.Round(fanControlSensors[j].Min ?? 0),
							Max = (float)Math.Round(fanControlSensors[j].Max ?? 0),
						};

						if (firstRun) {
							API.System.SuperIO.FanControl.Add(data);
						} else {
							API.System.SuperIO.FanControl[j] = data;
						}
					}
				}

				// Network
				if (hardware.HardwareType == HardwareType.Network) {
					var sensor = hardware.Sensors;

					for (int j = 0; j < hardware.Sensors.Length; j++) {
						// Throughput
						if (sensor[j].SensorType == SensorType.Throughput) {
							// find interface by id and overwrite value
							for (int k = 0; k < API.System.Network.Interfaces.Count; k++) {
								if (API.System.Network.Interfaces[k].Id == computerHardware[i].Identifier) {
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
							// find interface by id and overwrite value
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

			// HWInfo, monitors, network interfaces
			if (firstRun) {
				// CPU info
				for (int i = 0; i < computer.SMBios.Processors.Length; i++) {
					API.CPU.Info.Add(computer.SMBios.Processors[i]);
				}

				try {
					// GPU info
					API.GPU.Info = Commands.GetGPUInfo();

					// OS info
					API.System.OS.Name = Commands.GetOSInfo();
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
		catch (Exception e) {
			SentrySdk.CaptureException(e);
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
		foreach (IHardware subHardware in hardware.SubHardware) {
			subHardware.Accept(this);
		}
	}

	public void VisitSensor(ISensor sensor) {
	}

	public void VisitParameter(IParameter parameter) {
	}
}

