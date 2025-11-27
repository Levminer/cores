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

public class WindowState {
	public int x { get; set; } = 100;
	public int y { get; set; } = 100;
	public uint width { get; set; } = 1900;
	public uint height { get; set; } = 1000;
	public bool maximized { get; set; } = true;
	public uint monitorIndex { get; set; } = 0;
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
	public WindowState windowState { get; set; } = new();
}

public class Settings : DefaultSettings {
	internal static JsonSerializerOptions SerializerOptions = new() {
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		WriteIndented = true,
	};

	public string GetSettingsFolder() {
		return Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Cores");
	}

	public void CheckIfSettingsExists() {
		var defaultSettings = new DefaultSettings();
		var settingsFolder = GetSettingsFolder();

		// check if Cores folder exists
		if (!Directory.Exists(settingsFolder)) {
			Directory.CreateDirectory(settingsFolder);
		}

		// set folder permissions
		var folderInfo = new DirectoryInfo(settingsFolder);
		var folderSecurity = folderInfo.GetAccessControl();
		folderSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null),
		FileSystemRights.FullControl,
		InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
		PropagationFlags.None,
		AccessControlType.Allow));
		folderInfo.SetAccessControl(folderSecurity);

		// check if settings.json exists
		var settingsPath = Path.Join(settingsFolder, "settings.json");
		if (!File.Exists(settingsPath)) {
			// create settings.json
			File.WriteAllText(settingsPath, JsonSerializer.Serialize(defaultSettings));
		}
	}

	public Settings() {
		var defaultSettings = new DefaultSettings();
		var settingsFolder = GetSettingsFolder();

		CheckIfSettingsExists();

		// read settings.json
		try {
			var settingsPath = Path.Join(settingsFolder, "settings.json");
			using var stream = new FileStream(settingsPath, FileMode.Open, FileAccess.Read, FileShare.Read);
			var settings = JsonSerializer.Deserialize<DefaultSettings>(stream, SerializerOptions);

			interval = settings?.interval ?? defaultSettings.interval;
			minimizeToTray = settings?.minimizeToTray ?? defaultSettings.minimizeToTray;
			remoteConnections = settings?.remoteConnections ?? defaultSettings.remoteConnections;
			connectionCode = settings?.connectionCode ?? defaultSettings.connectionCode;
			connectionCodes = settings?.connectionCodes ?? defaultSettings.connectionCodes;
			connectionURL = settings?.connectionURL ?? defaultSettings.connectionURL;
			userId = settings?.userId ?? defaultSettings.userId;
			defaultDevices = settings?.defaultDevices ?? defaultSettings.defaultDevices;
			windowState = settings?.windowState ?? defaultSettings.windowState;
		}
		catch (Exception e) {
			SentrySdk.CaptureException(e);
		}
	}

	public void SetSettings() {
		var settingsFolder = GetSettingsFolder();

		CheckIfSettingsExists();

		// write settings.json
		var settingsPath = Path.Join(settingsFolder, "settings.json");
		File.WriteAllText(settingsPath, JsonSerializer.Serialize(this, SerializerOptions));
	}
}
