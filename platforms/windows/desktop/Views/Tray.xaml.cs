using H.NotifyIcon;
using H.NotifyIcon.EfficiencyMode;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using WinUIEx;

namespace Cores.Views;
public sealed partial class TrayIconView {
	public TrayIconView() {
		InitializeComponent();
	}

	public void ShowHideWindowCommand_ExecuteRequested(object _, ExecuteRequestedEventArgs args) {
		var window = App.MainWindow;

		if (window == null) {
			return;
		}

		if (window.Visible) {
			window.Hide();
			EfficiencyModeUtilities.SetEfficiencyMode(true);
			ShowHideText.Label = "Show Cores";
		} else {
			window.Show();
			EfficiencyModeUtilities.SetEfficiencyMode(false);
			ShowHideText.Label = "Hide Cores";
		}
	}

	public void ExitApplicationCommand_ExecuteRequested(object _, ExecuteRequestedEventArgs args) {
		TrayIcon.Dispose();
		App.exiting = true;
		Application.Current.Exit();
	}
}
