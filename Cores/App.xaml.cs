using cores;
using Microsoft.UI.Xaml;
using System.Text.Json;
using WinUIEx;

namespace Cores;

public partial class App : Application {
	private Window MainWindow;
	internal static HardwareInfo GlobalHardwareInfo = new();
	internal static JsonSerializerOptions SerializerOptions = new() {
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		WriteIndented = true
	};
	internal static Settings GlobalSettings = new();

	public App() {
		InitializeComponent();

		// Setup settings
		GlobalSettings.SetupSettings();
		GlobalSettings.GetSettings();
	}

	protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args) {
		MainWindow = new MainWindow();
		MainWindow.Activate();
		MainWindow.Maximize();
		MainWindow.Title = "Cores";
		MainWindow.SetIcon("Assets/icon.ico");

		var windowManager = WinUIEx.WindowManager.Get(MainWindow);
		windowManager.MinWidth = 1000;
		windowManager.MinHeight = 600;
	}
}
