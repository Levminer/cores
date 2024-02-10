using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace lib;

public class ConnectionCode {
	public static string Generate() {
		// get first 10 characters of guid
		var id = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);

		return $"crs_{id}";
	}
}
public class Settings {
	public static readonly ConnectionCode ConnectionCode = new();

	public int interval { get; set; } = 2;
	public bool minimizeToTray { get; set; } = true;
	public bool launchOnStartup { get; set; } = false;
	public bool remoteConnections { get; set; } = false;
	public string connectionCode { get; set; } = ConnectionCode.Generate();
	public int version { get; set; } = 2;

	public void GetSettings() {
		var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

		// check if Cores folder exists
		if (!File.Exists(Path.Join(appData, "Cores"))) {
			Directory.CreateDirectory(Path.Join(appData, "Cores"));
		}

		// check if settings.json exists
		if (!File.Exists(Path.Join(appData, "Cores", "settings.json"))) {
			// create settings.json
			File.WriteAllText(Path.Join(appData, "Cores", "settings.json"), JsonSerializer.Serialize(this));
		}

		// read settings.json
		var settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText(Path.Join(appData, "Cores", "settings.json")));

		interval = settings.interval;
		minimizeToTray = settings.minimizeToTray;
		launchOnStartup = settings.launchOnStartup;
		remoteConnections = settings.remoteConnections;
		connectionCode = settings.connectionCode;
		version = settings.version;
	}

	public void SetSettings() {
		var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

		// write settings.json
		File.WriteAllText(Path.Join(appData, "Cores", "settings.json"), JsonSerializer.Serialize(this));
	}
}
