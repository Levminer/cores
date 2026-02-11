using LibreHardwareMonitor.Hardware;
using Serilog;
using System.Net.NetworkInformation;

namespace lib;

public static class ArrayExtensions {
	public static bool TrySetValue<T>(this List<T> list, int index, T value) {
		if (index >= 0 && index < list.Count) {
			list[index] = value;
			return true;
		}
		return false;
	}
}

public class HardwareInfo {
	private static bool firstRun = true;
	private static bool errorSent = false;
	private DateTime lastRun = DateTime.Now;
	public HardwareUpdater refresher = new();
	public Commands commands = new();
	public Settings settings = new();
	public Computer computer = new() {
		IsCpuEnabled = true,
		IsGpuEnabled = true,
		IsMemoryEnabled = true,
		IsMotherboardEnabled = true,
		IsStorageEnabled = true,
		IsNetworkEnabled = true,
		IsBatteryEnabled = true,
	};

	public API API {
		get; set;
	} = new();

	public HardwareInfo(Settings settings) {
		this.settings = settings;
		computer.Open();
		computer.Accept(refresher);

		GetInfo();
	}

	public void GetInfo() {
		try {
			var computerHardware = computer.Hardware;

			if (firstRun) {
				Log.Information("HW firstRun");
			}

			if (firstRun || DateTime.Now.Subtract(lastRun).TotalSeconds > 60) {
				// Network interfaces
				foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces()) {
					if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet) {
						var temp = new NetInterface {
							Name = ni.Name,
							Id = new Identifier("nic", ni.Id).ToString(),
							Description = ni.Description,
							Speed = (ni.Speed / 1000 / 1000).ToString(),
						};

						// Priority
						if (!temp.Description.Contains("Virtual") && temp.Name.Contains("Ethernet")) {
							temp.Priority = 0;
						} else if (!temp.Description.Contains("Virtual") && (temp.Name.Contains("WiFi") || temp.Name.Contains("Wi-Fi"))) {
							temp.Priority = 1;
						} else {
							temp.Priority = 2;
						}

						if (temp.Id.Contains(settings.defaultDevices.network)) {
							temp.Priority = -1;
						}

						// Mac address
						var mac = ni.GetPhysicalAddress().ToString();
						temp.MACAddress = string.Join(":", Enumerable.Range(0, mac.Length)
													.Where(x => x % 2 == 0)
													.Select(x => mac.Substring(x, 2))
													.ToArray());

						// DNS
						temp.DNS = "N/A";
						temp.DNSV6 = "N/A";

						if (ni.GetIPProperties().DnsAddresses.Count != 0) {
							for (int i = 0; i < ni.GetIPProperties().DnsAddresses.Count; i++) {
								if (temp.DNS == "N/A" && ni.GetIPProperties().DnsAddresses[i].AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) {
									temp.DNS = ni.GetIPProperties().DnsAddresses[i].ToString();
								} else if (temp.DNSV6 == "N/A" && ni.GetIPProperties().DnsAddresses[i].AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6) {
									temp.DNSV6 = ni.GetIPProperties().DnsAddresses[i].ToString();
								}
							}
						}

						// Gateway
						temp.Gateway = "N/A";
						temp.GatewayV6 = "N/A";

						if (ni.GetIPProperties().DnsAddresses.Count != 0) {
							for (int i = 0; i < ni.GetIPProperties().GatewayAddresses.Count; i++) {
								if (temp.Gateway == "N/A" && ni.GetIPProperties().GatewayAddresses[i].Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) {
									temp.Gateway = ni.GetIPProperties().GatewayAddresses[i].Address.ToString();
								} else if (temp.GatewayV6 == "N/A" && ni.GetIPProperties().GatewayAddresses[i].Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6) {
									temp.GatewayV6 = ni.GetIPProperties().GatewayAddresses[i].Address.ToString();
								}
							}
						}

						// Current IP
						temp.IPAddress = "N/A";
						temp.IPAddressV6 = "N/A";
						temp.Mask = "N/A";

						foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses) {
							if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) {
								temp.IPAddress = ip.Address.ToString();
								temp.Mask = ip.IPv4Mask.ToString();
							}

							if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6) {
								temp.IPAddressV6 = ip.Address.ToString();
							}
						}

						if (!temp.Name.Contains("Local Area Connection*")) {
							if (firstRun) {
								API.System.Network.Interfaces.Add(temp);
							} else {
								var nicId = API.System.Network.Interfaces.FindIndex(x => x.Id == temp.Id);
								if (nicId != -1) {
									API.System.Network.Interfaces[nicId] = temp;
								} else {
									API.System.Network.Interfaces.Add(temp);
								}
							}
						}
					}

					API.System.Network.Interfaces = API.System.Network.Interfaces.OrderBy(item => item.Priority).ToList();
				}
			}

			// RAM
			var ramSensors = computer.Hardware.Where(h => h.Identifier.ToString().Contains("/ram")).SelectMany(h => h.Sensors);
			var virtualRamSensors = computer.Hardware.Where(h => h.Identifier.ToString().Contains("/vram")).SelectMany(h => h.Sensors);
			var allMemorySensors = ramSensors.Concat(virtualRamSensors).ToArray();

			// RAM load
			for (int j = 0; j < allMemorySensors.Length; j++) {
				var sensor = allMemorySensors;

				if (sensor[j].SensorType == SensorType.Load || sensor[j].SensorType == SensorType.Data) {
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
						API.RAM.Load.TrySetValue(j, data);
					}
				}
			}

			// RAM temperature 
			if (firstRun || DateTime.Now.Subtract(lastRun).TotalSeconds > 60) {
				var ramTemperatureSensors = computer.Hardware
					.Where(h => h.Identifier.ToString().StartsWith("/memory"))
					.SelectMany(h => h.Sensors)
					.Where(x => x.SensorType == SensorType.Temperature && x.Name.StartsWith("DIMM"))
					.ToArray();

				for (int j = 0; j < ramTemperatureSensors.Length; j++) {
					var data = new Sensor {
						Name = ramTemperatureSensors[j].Name,
						Value = (float)Math.Round(ramTemperatureSensors[j].Value ?? 0),
						Min = (float)Math.Round(ramTemperatureSensors[j].Min ?? 0),
						Max = (float)Math.Round(ramTemperatureSensors[j].Max ?? 0),
					};

					// RAM temperature
					if (firstRun) {
						API.RAM.Temperature.Add(data);
					} else {
						API.RAM.Temperature.TrySetValue(j, data);
					}
				}
			}

			// Loop through remaining hardware
			for (int i = 0; i < computer.Hardware.Count; i++) {
				var hardware = computer.Hardware[i];

				// Get component names
				if (firstRun) {
					if (computerHardware[i].HardwareType == HardwareType.Cpu) {
						API.CPU.Name = computerHardware[i].Name;
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
					var temperatureSensors = hardware.Sensors.Where(x => x.SensorType == SensorType.Temperature && !x.Name.Contains("Tj") && (x.Name.StartsWith("P-Core") || x.Name.StartsWith("E-Core") || x.Name.StartsWith("CPU Core") || hardware.Identifier.ToString().Contains("amd"))).ToArray();
					var loadSensors = hardware.Sensors.Where(x => x.SensorType == SensorType.Load).ToArray();
					var powerSensors = hardware.Sensors.Where(x => x.SensorType == SensorType.Power).ToArray();
					var clockSensors = hardware.Sensors.Where(x => x.SensorType == SensorType.Clock && !x.Name.Contains("Bus")).ToArray();
					var voltageSensors = hardware.Sensors.Where(x => x.SensorType == SensorType.Voltage && x.Name.Contains('#')).ToArray();

					// CPU Temperature
					for (int j = 0; j < temperatureSensors.Length; j++) {
						var data = new Sensor {
							Name = temperatureSensors[j].Name,
							Value = (float)Math.Round(temperatureSensors[j].Value ?? 0),
							Min = (float)Math.Round(temperatureSensors[j].Min ?? 0),
							Max = (float)Math.Round(temperatureSensors[j].Max ?? 0),
						};

						if (firstRun) {
							API.CPU.Temperature.Add(data);
						} else {
							API.CPU.Temperature.TrySetValue(j, data);
						}
					}

					// CPU Power
					for (int j = 0; j < powerSensors.Length; j++) {
						// CPU power is buggy on wake from sleep
						if (powerSensors[j].Max < 2000 && powerSensors[j].Value < 2000) {
							var data = new Sensor {
								Name = powerSensors[j].Name,
								Value = (float)Math.Round(powerSensors[j].Value ?? 0),
								Min = (float)Math.Round(powerSensors[j].Min ?? 0),
								Max = (float)Math.Round(powerSensors[j].Max ?? 0),
							};

							if (firstRun) {
								API.CPU.Power.Add(data);
							} else {
								API.CPU.Power.TrySetValue(j, data);
							}
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
								API.CPU.Load.TrySetValue(j, data);
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
							API.CPU.Clock.TrySetValue(j, data);
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
							API.CPU.Voltage.TrySetValue(j, data);
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
					var loadSensors = hardware.Sensors.Where(x => x.SensorType == SensorType.Load && x.Name.StartsWith("D3D")).ToArray();

					// Intel ARC GPUs use different load sensors
					if (hardware.Identifier.ToString().Contains("gpu-intel")) {
						loadSensors = hardware.Sensors.Where(x => x.SensorType == SensorType.Load && !x.Name.Contains("Memory")).ToArray();
					}

					if (firstRun) {
						var data = new GPU {
							Name = computerHardware[i].Name,
							Id = computerHardware[i].Identifier.ToString(),
							Priority = 1,
						};

						if (hardware.HardwareType.ToString().Contains("Nvidia")) {
							data.Priority = 0;
						}

						if (data.Id.Contains(settings.defaultDevices.gpu)) {
							data.Priority = -1;
						}

						// init memory sensors
						data.Memory = new List<Sensor>
						{
							new Sensor { Name = "D3D Dedicated Memory Used", Value = 0, Min = 0, Max = 0 },
							new Sensor { Name = "D3D Shared Memory Used", Value = 0, Min = 0, Max = 0 },
							new Sensor { Name = "GPU Memory Total", Value = 0, Min = 0, Max = 0 },
							new Sensor { Name = "GPU Memory Free", Value = 0, Min = 0, Max = 0 },
							new Sensor { Name = "GPU Memory Used", Value = 0, Min = 0, Max = 0 }
						};

						API.GPU.Cards.Add(data);

						// sort by priority
						API.GPU.Cards = API.GPU.Cards.OrderBy(item => item.Priority).ToList();
					}

					var cardIndex = API.GPU.Cards.FindIndex(x => x.Id == computerHardware[i].Identifier.ToString());

					if (cardIndex == -1 || cardIndex >= API.GPU.Cards.Count) {
						// Skip this GPU if no matching card found or invalid index
						continue;
					}

					// GPU Temperature
					for (int j = 0; j < temperatureSensors.Length; j++) {
						var data = new Sensor {
							Name = temperatureSensors[j].Name,
							Value = (float)Math.Round(temperatureSensors[j].Value ?? 0),
							Min = (float)Math.Round(temperatureSensors[j].Min ?? 0),
							Max = (float)Math.Round(temperatureSensors[j].Max ?? 0),
						};

						if (firstRun) {
							API.GPU.Cards[cardIndex].Temperature.Add(data);
						} else {
							API.GPU.Cards[cardIndex].Temperature.TrySetValue(j, data);
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
							API.GPU.Cards[cardIndex].Fan.Add(data);
						} else {
							API.GPU.Cards[cardIndex].Fan.TrySetValue(j, data);
						}
					}

					// GPU Memory
					for (int j = 0; j < memorySensors.Length; j++) {
						for (int k = 0; k < API.GPU.Cards[cardIndex].Memory.Count; k++) {
							if (memorySensors[j].Name == API.GPU.Cards[cardIndex].Memory[k].Name) {
								var data = new Sensor {
									Name = memorySensors[j].Name,
									Value = (float)Math.Round(memorySensors[j].Value / 1024 ?? 0, 1),
									Min = (float)Math.Round(memorySensors[j].Min / 1024 ?? 0, 1),
									Max = (float)Math.Round(memorySensors[j].Max / 1024 ?? 0, 1),
								};

								API.GPU.Cards[cardIndex].Memory.TrySetValue(k, data);
							}
						}
					}

					// Intel ARC
					if (hardware.Identifier.ToString().Contains("gpu-intel")) {
						try {
							API.GPU.Cards[cardIndex].Memory[0] = API.GPU.Cards[cardIndex].Memory[4];
						}
						catch (Exception) {
							Log.Error("Failed to set GPU memory on Intel ARC");
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
							API.GPU.Cards[cardIndex].Power.Add(data);
						} else {
							API.GPU.Cards[cardIndex].Power.TrySetValue(j, data);
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
							API.GPU.Cards[cardIndex].Clock.Add(data);
						} else {
							API.GPU.Cards[cardIndex].Clock.TrySetValue(j, data);
						}
					}

					// GPU Load
					for (int j = 0; j < loadSensors.Length; j++) {
						var data = new Sensor {
							Name = loadSensors[j].Name,
							Value = (float)Math.Round(loadSensors[j].Value ?? 0),
							Min = (float)Math.Round(loadSensors[j].Min ?? 0),
							Max = (float)Math.Round(loadSensors[j].Max ?? 0),
						};
						if (firstRun) {
							API.GPU.Cards[cardIndex].Load.Add(data);
						} else {
							API.GPU.Cards[cardIndex].Load.TrySetValue(j, data);
						}
					}

					// GPU Max Load
					if (API.GPU.Cards[cardIndex].Load.Count > 0) {
						API.GPU.Cards[cardIndex].MaxLoad = API.GPU.Cards[cardIndex].Load.Max(x => x.Value);
					}
				}

				// Storage
				if (hardware.HardwareType == HardwareType.Storage) {
					var sensor = hardware.Sensors;

					if (firstRun || DateTime.Now.Subtract(lastRun).TotalSeconds > 60) {
						var data = new Disk {
							Name = computerHardware[i].Name,
							Id = computerHardware[i].Identifier.ToString(),
							Priority = 1,
							Health = "N/A",
						};

						// Get disk size
						var report = computerHardware[i].GetReport().Split("\n");
						long total = 0;
						long free = 0;

						foreach (var line in report) {
							if (line.StartsWith("Total Size")) {
								var parts = line.Split(":");
								if (parts.Length > 1) {
									if (Int64.TryParse(parts[1].Trim(), out long totalBytes)) {
										total = totalBytes / 1024 / 1024 / 1024;
									} else {
										Log.Warning($"Failed to parse Total Size: {parts[1].Trim()}");
									}
								}
							}

							if (line.StartsWith("Total Free Size")) {
								var parts = line.Split(":");
								if (parts.Length > 1) {
									if (Int64.TryParse(parts[1].Trim(), out long freeBytes)) {
										free = freeBytes / 1024 / 1024 / 1024;
									} else {
										Log.Warning($"Failed to parse Total Free Size: {parts[1].Trim()}");
									}
								}
							}

							if (line.StartsWith("Logical Drive Name: C")) {
								data.Priority = 0;
							}

							if (data.Id.Contains(settings.defaultDevices.storage)) {
								data.Priority = -1;
							}
						}

						data.TotalSpace = (int)total;
						data.FreeSpace = (int)free;

						if (firstRun) {
							API.System.Storage.Disks.Add(data);
						} else {
							var diskId = API.System.Storage.Disks.FindIndex(x => x.Id == computerHardware[i].Identifier.ToString());

							if (diskId != -1) {
								// Preserve existing temperature data when updating disk info
								var existingTemperature = API.System.Storage.Disks[diskId].Temperature;
								var existingPowerOnCount = API.System.Storage.Disks[diskId].PowerOnCount;
								var existingPowerOnHours = API.System.Storage.Disks[diskId].PowerOnHours;
								API.System.Storage.Disks[diskId] = data;
								API.System.Storage.Disks[diskId].Temperature = existingTemperature;
								API.System.Storage.Disks[diskId].PowerOnCount = existingPowerOnCount;
								API.System.Storage.Disks[diskId].PowerOnHours = existingPowerOnHours;
							}
						}

						API.System.Storage.Disks = API.System.Storage.Disks.OrderBy(item => item.Priority).ToList();
					}

					for (int j = 0; j < hardware.Sensors.Length; j++) {
						// Drive temperature
						if (sensor[j].SensorType == SensorType.Temperature && !sensor[j].Name.ToLower().Contains("warning") && !sensor[j].Name.ToLower().Contains("critical")) {
							// find disk by id and overwrite value
							for (int k = 0; k < API.System.Storage.Disks.Count; k++) {
								if (API.System.Storage.Disks[k].Id == computerHardware[i].Identifier.ToString()) {
									var sensorValue = sensor[j].Value ?? 0;
									var sensorMin = sensor[j].Min ?? 0;

									if (firstRun && sensorMin == 0) {
										sensorMin = sensorValue;
									} else if (sensorMin == 0) {
										if (sensorValue < API.System.Storage.Disks[k].Temperature.Min) {
											sensorMin = sensorValue;
										} else {
											sensorMin = API.System.Storage.Disks[k].Temperature.Min;
										}
									}

									API.System.Storage.Disks[k].Temperature = new Sensor {
										Name = sensor[j].Name,
										Value = sensorValue,
										Min = sensorMin,
										Max = (float)Math.Round(sensor[j].Max ?? 0),
									};
								}
							}
						}

						// Drive throughput
						if (sensor[j].SensorType == SensorType.Throughput) {
							// find disk by id and overwrite value
							for (int k = 0; k < API.System.Storage.Disks.Count; k++) {
								if (API.System.Storage.Disks[k].Id == computerHardware[i].Identifier.ToString()) {
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
								if (API.System.Storage.Disks[k].Id == computerHardware[i].Identifier.ToString()) {
									if (sensor[j].Name.Contains("Read")) {
										API.System.Storage.Disks[k].DataRead = (float)Math.Round(sensor[j].Value ?? 0, 1);
									}

									if (sensor[j].Name.Contains("Written")) {
										API.System.Storage.Disks[k].DataWritten = (float)Math.Round(sensor[j].Value ?? 0, 1);
									}
								}
							}
						}

						// SSD Health
						if (sensor[j].SensorType == SensorType.Level && (firstRun || DateTime.Now.Subtract(lastRun).TotalSeconds > 60)) {
							// find disk by id and overwrite value
							for (int k = 0; k < API.System.Storage.Disks.Count; k++) {
								if (API.System.Storage.Disks[k].Id == computerHardware[i].Identifier.ToString()) {
									if (sensor[j].Name.Contains("Life")) {
										API.System.Storage.Disks[k].Health = (sensor[j].Value ?? 0).ToString();
									}
								}
							}
						}

						if (sensor[j].SensorType == SensorType.Factor && firstRun) {
							for (int k = 0; k < API.System.Storage.Disks.Count; k++) {
								if (API.System.Storage.Disks[k].Id == computerHardware[i].Identifier.ToString()) {
									if (sensor[j].Name.Contains("Count")) {
										API.System.Storage.Disks[k].PowerOnCount = (sensor[j].Value ?? 0);
									}

									if (sensor[j].Name.Contains("Hours")) {
										API.System.Storage.Disks[k].PowerOnHours = (sensor[j].Value ?? 0);
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
							API.System.SuperIO.Voltage.TrySetValue(j, data);
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
							API.System.SuperIO.Temperature.TrySetValue(j, data);
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
							API.System.SuperIO.FanControl.TrySetValue(j, data);
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
							API.System.SuperIO.Fan.TrySetValue(j, data);
						}

						try {
							if (fanSensors[j].Value != 0 && fanControlSensors[j].Value == null) {
								var maxRPM = 2000;

								if (j == 0) {
									maxRPM = 1700;
								} else if (j == 1) {
									maxRPM = 2500;
								}

								var data2 = new Sensor {
									Name = fanSensors[j].Name,
									Value = (float)Math.Round(fanSensors[j].Value / maxRPM * 100 ?? 0),
									Min = (float)Math.Round(fanSensors[j].Min / maxRPM * 100 ?? 0),
									Max = (float)Math.Round(fanSensors[j].Max / maxRPM * 100 ?? 0),
								};

								if (firstRun) {
									API.System.SuperIO.FanControl.Add(data2);
								} else {
									API.System.SuperIO.FanControl.TrySetValue(j, data2);
								}
							}
						}
						catch (Exception) {
							Log.Error("Failed to calculate fan speed");
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
								if (API.System.Network.Interfaces[k].Id == computerHardware[i].Identifier.ToString()) {
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
								if (API.System.Network.Interfaces[k].Id == computerHardware[i].Identifier.ToString()) {
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

				// Battery
				if (hardware.HardwareType == HardwareType.Battery) {
					var energySensors = hardware.Sensors.Where(x => x.SensorType == SensorType.Energy).ToArray();
					var levelSensors = hardware.Sensors.Where(x => x.SensorType == SensorType.Level).ToArray();
					var timeSpanSensors = hardware.Sensors.Where(x => x.SensorType == SensorType.TimeSpan).ToArray();

					// Energy capacity
					for (int j = 0; j < energySensors.Length; j++) {
						var data = new Sensor {
							Name = energySensors[j].Name,
							Value = energySensors[j].Value ?? 0,
							Min = energySensors[j].Min ?? 0,
							Max = energySensors[j].Max ?? 0,
						};

						if (firstRun) {
							API.System.Battery.Capacity.Add(data);
						} else {
							API.System.Battery.Capacity[j] = data;
						}
					}

					// Charge level
					for (int j = 0; j < levelSensors.Length; j++) {
						var data = new Sensor {
							Name = levelSensors[j].Name,
							Value = levelSensors[j].Value ?? 0,
							Min = levelSensors[j].Min ?? 0,
							Max = levelSensors[j].Max ?? 0,
						};

						if (firstRun) {
							API.System.Battery.Level.Add(data);
						} else {
							API.System.Battery.Level[j] = data;
						}
					}

					// Cycle count
					if (firstRun) {
						var cycles = Commands.GetCycleCount();

						API.System.Battery.CycleCount = cycles;
					}
				}
			}

			// HWInfo, monitors, network interfaces
			if (firstRun) {
				// CPU info
				for (int i = 0; i < computer.SMBios.Processors.Length; i++) {
					API.CPU.Info.Add(computer.SMBios.Processors[i]);
				}

				// GPU info
				try {
					// GPU info
					API.GPU.Info = Commands.GetGPUInfo();
				}
				catch (Exception) {
					Log.Error("Failed to get GPU info");
				}

				try {
					// OS info
					API.System.OS.Name = Commands.GetOSInfo();

					// hostname
					API.System.OS.Hostname = System.Net.Dns.GetHostName();
				}
				catch (Exception) {
					Log.Error("Failed to get OS and hostname");
				}

				// RAM modules
				for (int i = 0; i < computer.SMBios.MemoryDevices.Length; i++) {
					if (computer.SMBios.MemoryDevices[i].Speed != 0) {
						API.RAM.Info.Add(computer.SMBios.MemoryDevices[i]);
					}

					API.RAM.Layout.Add(computer.SMBios.MemoryDevices[i]);
				}

				// BIOS info
				var biosDate = computer.SMBios.Bios.Date ?? new DateTime(1970, 01, 01);

				API.System.BIOS = new BIOSInfo {
					Vendor = computer.SMBios.Bios.Vendor,
					Version = computer.SMBios.Bios.Version,
					Date = biosDate.ToShortDateString(),
				};
			}

			// Refresh network and disk info every 60 seconds
			if (DateTime.Now.Subtract(lastRun).TotalSeconds > 60) {
				lastRun = DateTime.Now;
			}

			firstRun = false;
			// utc time
			API.Timestamp = DateTime.UtcNow;
		}
		catch (Exception ex) {
			if (errorSent == false) {
				SentrySdk.CaptureException(ex);
				Log.Information("HW Info Error: {@errorSent}", ex);
				errorSent = true;
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
		foreach (IHardware subHardware in hardware.SubHardware) {
			subHardware.Accept(this);
		}
	}

	public void VisitSensor(ISensor sensor) {
	}

	public void VisitParameter(IParameter parameter) {
	}
}

