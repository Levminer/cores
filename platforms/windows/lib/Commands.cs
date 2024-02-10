using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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

		return gpuInfo.DriverDate;
	}
}
