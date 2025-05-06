using System.Security.AccessControl;
using System.Security.Principal;
using System.Text.Json;

namespace lib;

public class DefaultValues {
	public static string GenerateConnectionCode() {
		var id = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 16);

		return $"crs_{id}";
	}

	public static string GenerateUserId() {
		var id = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);

		return $"user_{id}";
	}

	public static string DefaultConnectionURL() {
		return "rtc-usw.coresmonitor.com";
	}
}

public class ConnectionCode {
	public string name;
	public string code;
	public string mac;
}

public class DefaultDevices {
	public string gpu { get; set; } = "";
	public string network { get; set; } = "";
	public string storage { get; set; } = "";
}


public class DefaultSettings {
	public int interval { get; set; } = 3;
	public bool minimizeToTray { get; set; } = true;
	public bool remoteConnections { get; set; } = false;
	public List<ConnectionCode> connectionCodes = new();
	public string connectionCode { get; set; } = DefaultValues.GenerateConnectionCode();
	public string connectionURL { get; set; } = DefaultValues.DefaultConnectionURL();
	public string userId { get; set; } = DefaultValues.GenerateUserId();
	public DefaultDevices defaultDevices { get; set; } = new();
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
			using var stream = new FileStream(Path.Join(appData, "Cores", "settings.json"), FileMode.Open, FileAccess.Read, FileShare.Read);
			var settings = JsonSerializer.Deserialize<DefaultSettings>(stream, SerializerOptions);

			interval = settings?.interval ?? defaultSettings.interval;
			minimizeToTray = settings?.minimizeToTray ?? defaultSettings.minimizeToTray;
			remoteConnections = settings?.remoteConnections ?? defaultSettings.remoteConnections;
			connectionCode = settings?.connectionCode ?? defaultSettings.connectionCode;
			connectionCodes = settings?.connectionCodes ?? defaultSettings.connectionCodes;
			connectionURL = settings?.connectionURL ?? defaultSettings.connectionURL;
			userId = settings?.userId ?? defaultSettings.userId;
			defaultDevices = settings?.defaultDevices ?? defaultSettings.defaultDevices;
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
