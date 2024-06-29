using System.Security.AccessControl;
using System.Security.Principal;
using System.Text.Json;

namespace lib;

public class DefaultValues {
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
	public string connectionCode { get; set; } = DefaultValues.Generate();
	public string licenseKey { get; set; } = "";
	public string licenseActivated { get; set; } = "";
	public string userId { get; set; } = DefaultValues.Generate();
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

		// set folder permissions
		var folderInfo = new DirectoryInfo(Path.Join(prorgamData, "Cores"));
		var folderSecurity = folderInfo.GetAccessControl();
		folderSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null),
		FileSystemRights.FullControl,
		InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
		PropagationFlags.None,
		AccessControlType.Allow));
		folderInfo.SetAccessControl(folderSecurity);

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
			licenseKey = settings?.licenseKey ?? defaultSettings.licenseKey;
			licenseActivated = settings?.licenseActivated ?? defaultSettings.licenseActivated;
			userId = settings?.userId ?? defaultSettings.userId;
			version = settings?.version ?? defaultSettings.version;
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
