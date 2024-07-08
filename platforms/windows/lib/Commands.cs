using Serilog;
using System.Diagnostics;
using System.Text.Json;

namespace lib;
public class Commands {
	public static string ExecuteCommand(string command) {
		var scriptArguments = $"-WindowStyle Hidden -Command \"{command}\"";
		var processStartInfo = new ProcessStartInfo("powershell.exe", scriptArguments) {
			CreateNoWindow = true,
			RedirectStandardOutput = true,
			RedirectStandardError = true
		};

		using var process = new Process();
		process.StartInfo = processStartInfo;
		process.Start();
		string output = process.StandardOutput.ReadToEnd();
		string error = process.StandardError.ReadToEnd();


		return output;
	}

	public class CmdOsInfo {
		public string Caption { get; set; }
		public string OSArchitecture { get; set; }
		public string Version { get; set; }
	}


	public static string GetOSInfo() {
		var command = ExecuteCommand("Get-WmiObject -class Win32_OperatingSystem | Select-Object Caption, OSArchitecture, Version | ConvertTo-Json");
		var osInfo = JsonSerializer.Deserialize<CmdOsInfo>(command);

		osInfo.Caption = osInfo.Caption.Replace("Microsoft", "");

		if (osInfo.OSArchitecture == "64-bit") {
			osInfo.OSArchitecture = "x64";
		}

		return $"{osInfo.Caption.Trim()} {osInfo.OSArchitecture} {osInfo.Version.Trim()}";
	}

	public class CmdGpuInfo {
		public string DriverDate { get; set; }
	}

	public static string GetGPUInfo() {
		var command = ExecuteCommand("Get-WmiObject -class Win32_VideoController | Select-Object DriverDate | ConvertTo-Json");
		var gpuInfo = JsonSerializer.Deserialize<CmdGpuInfo>(command);

		// format: 20240411000000.000000-000
		var driverDate = gpuInfo?.DriverDate ?? "00000000000000.000000-000";
		var unformattedDate = driverDate.Split(".")[0];
		var foramttedDate = $"{unformattedDate.Substring(0, 4)}. {unformattedDate.Substring(4, 2)}. {unformattedDate.Substring(6, 2)}.";

		return foramttedDate;
	}

	public class CycleCountInfo {
		public int? CycleCount { get; set; }
	}

	public static string GetCycleCount() {
		try {
			var command = ExecuteCommand("Get-WmiObject -Class MSBatteryClass -Namespace ROOT/WMI | Select-Object CycleCount | ConvertTo-Json");

			var cycles = JsonSerializer.Deserialize<List<CycleCountInfo>>(command);
			var c = 0;

			if (cycles?.Count > 0) {
				foreach (var cycle in cycles) {
					c += cycle.CycleCount ?? 0;
				}
			}

			if (c == 0) {
				return "N/A";
			} else {
				return c.ToString();
			}
		}
		catch (Exception) {
			Log.Error("Failed to get cycle count");
			return "N/A";
		}
	}
}
