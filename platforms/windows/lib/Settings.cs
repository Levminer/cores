using System.Text.Json;

namespace lib;

public class ConnectionCode {
	public static string Generate() {
		// get first 10 characters of guid
		var id = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);

		return $"crs_{id}";
	}
}

public class DefaultSettings {
	public int interval { get; set; } = 2;
	public bool minimizeToTray { get; set; } = true;
	public bool launchOnStartup { get; set; } = false;
	public bool remoteConnections { get; set; } = false;
	public bool optionalAnalytics { get; set; } = true;
	public string connectionCode { get; set; } = ConnectionCode.Generate();
	public int version { get; set; } = 2;
}

public class Settings : DefaultSettings {
	internal static JsonSerializerOptions SerializerOptions = new() {
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		WriteIndented = true,
	};

	public void CheckIfSettingsExists() {
		var defaultSettings = new DefaultSettings();
		var prorgamData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

		// check if Cores folder exists
		if (!File.Exists(Path.Join(prorgamData, "Cores"))) {
			Directory.CreateDirectory(Path.Join(prorgamData, "Cores"));
		}

		// check if settings.json exists
		if (!File.Exists(Path.Join(prorgamData, "Cores", "settings.json"))) {
			// create settings.json
			File.WriteAllText(Path.Join(prorgamData, "Cores", "settings.json"), JsonSerializer.Serialize(defaultSettings));
		}
	}

	public Settings() {
		var defaultSettings = new DefaultSettings();
		var appData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

		CheckIfSettingsExists();

		// read settings.json
		try {
			var settings = JsonSerializer.Deserialize<DefaultSettings>(File.ReadAllText(Path.Join(appData, "Cores", "settings.json")), SerializerOptions);

			interval = settings?.interval ?? defaultSettings.interval;
			minimizeToTray = settings?.minimizeToTray ?? defaultSettings.minimizeToTray;
			launchOnStartup = settings?.launchOnStartup ?? defaultSettings.launchOnStartup;
			remoteConnections = settings?.remoteConnections ?? defaultSettings.remoteConnections;
			optionalAnalytics = settings?.optionalAnalytics ?? defaultSettings.optionalAnalytics;
			connectionCode = settings?.connectionCode ?? defaultSettings.connectionCode;
			version = settings?.version ?? defaultSettings.version;
		}
		catch (Exception e) {
			SentrySdk.CaptureException(e);
		}
	}

	[Obsolete("Deprecated, remove when desktop is removed")]
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
		try {
			var settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText(Path.Join(appData, "Cores", "settings.json")));

			interval = settings.interval;
			minimizeToTray = settings.minimizeToTray;
			launchOnStartup = settings.launchOnStartup;
			remoteConnections = settings.remoteConnections;
			optionalAnalytics = settings.optionalAnalytics;
			connectionCode = settings.connectionCode;
			version = settings.version;
		}
		catch (Exception e) {
			SentrySdk.CaptureException(e);
		}
	}

	public void SetSettings() {
		var programData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

		CheckIfSettingsExists();

		// write settings.json
		File.WriteAllText(Path.Join(programData, "Cores", "settings.json"), JsonSerializer.Serialize(this, SerializerOptions));
	}
}
