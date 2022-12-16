using cores;
using Microsoft.UI.Xaml;
using WinUIEx;

namespace Cores;

public partial class App : Application {
	private Window MainWindow;
	internal static HardwareInfo GlobalHardwareInfo;

	public App() {
		InitializeComponent();

		GlobalHardwareInfo = new HardwareInfo();
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
