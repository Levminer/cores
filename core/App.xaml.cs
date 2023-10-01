using cores;
using Microsoft.UI.Xaml;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json;
using WinUIEx;

namespace Cores;

public partial class App : Application {
	internal static Window MainWindow;
	internal static HardwareInfo GlobalHardwareInfo = new();
	internal static JsonSerializerOptions SerializerOptions = new() {
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		WriteIndented = true
	};
	internal static Settings GlobalSettings = new();
	internal static bool exiting = false;

	[DllImport("lib.dll")]
	private static extern Settings getSettings();

	public App() {
		InitializeComponent();

		// Force dark mode
		Current.RequestedTheme = ApplicationTheme.Dark;

		//Get settings
		GlobalSettings = getSettings();
	}

	protected override async void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args) {
		// Single instance
		var appArgs = Microsoft.Windows.AppLifecycle.AppInstance.GetCurrent().GetActivatedEventArgs();

		// Get or register the main instance
		var mainInstance = Microsoft.Windows.AppLifecycle.AppInstance.FindOrRegisterForKey("main");

		// If the main instance isn't this current instance
		if (!mainInstance.IsCurrent) {
			// Redirect activation to that instance
			await mainInstance.RedirectActivationToAsync(appArgs);

			// And exit our instance and stop
			Process.GetCurrentProcess().Kill();
			return;
		}

		// Otherwise, register for activation redirection
		Microsoft.Windows.AppLifecycle.AppInstance.GetCurrent().Activated += App_Activated;

		// Create the main window
		MainWindow = new MainWindow();
		MainWindow.Activate();
		MainWindow.Maximize();
		MainWindow.Title = "Cores";
		MainWindow.SetIcon("Assets/icon.ico");

		// Prevent window close
		MainWindow.Closed += (s, e) => {
			if (GlobalSettings.minimizeToTray && !exiting) {
				MainWindow.Hide();
				e.Handled = true;
			} else {
				Process.GetCurrentProcess().Kill();
			}
		};

		// Resize window
		var windowManager = WindowManager.Get(MainWindow);
		windowManager.MinWidth = 1000;
		windowManager.MinHeight = 600;
	}

	private void App_Activated(object sender, Microsoft.Windows.AppLifecycle.AppActivationArguments e) {
		MainWindow.Maximize();
		MainWindow.SetForegroundWindow();
	}
}
