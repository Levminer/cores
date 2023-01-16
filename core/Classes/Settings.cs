using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace Cores;

public class SettingsStructure {
	public int Interval { get; set; }
}

public class Settings {
	public string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Cores\\settings.json";
	public SettingsStructure settings;

	public void SetupSettings() {
		// Create folder if it doesn't exist
		if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Cores")) {
			Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Cores");
		}

		// Default settings
		var defaultSettings = new SettingsStructure() {
			Interval = 2
		};


		// Create file if it doesn't exist
		if (!File.Exists(path)) {
			File.WriteAllText(path, JsonSerializer.Serialize(defaultSettings, App.SerializerOptions));
		}
	}

	public void GetSettings() {
		// Get settings from file
		settings = JsonSerializer.Deserialize<SettingsStructure>(File.ReadAllText(path), App.SerializerOptions);
	}

	public void SetSettings(string newSettings) {
		Debug.WriteLine(newSettings);
		// Update settings
		settings = JsonSerializer.Deserialize<SettingsStructure>(newSettings, App.SerializerOptions);

		// Write settings to file
		File.WriteAllText(path, JsonSerializer.Serialize(settings, App.SerializerOptions));
	}
}
