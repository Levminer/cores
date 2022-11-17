using cores;
using Microsoft.UI.Xaml;

namespace Cores;

public partial class App : Application {
	private Window m_window;
	internal static HardwareInfo GlobalHardwareInfo;

	public App() {
		InitializeComponent();

		GlobalHardwareInfo = new HardwareInfo();
	}

	protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args) {
		m_window = new MainWindow();
		m_window.Activate();
	}
}
