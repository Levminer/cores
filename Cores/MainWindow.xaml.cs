using cores;
using Microsoft.UI.Xaml;
using Microsoft.Web.WebView2.Core;
using System;
using System.Linq;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Cores;
/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window {
	private readonly DispatcherTimer dispatcherTimer;

	public MainWindow() {
		InitializeComponent();

		// Set Mica
		var mica = new Mica(this);
		// mica.TrySetSystemBackdrop();

		// Webview setup
		Init();

		// Refresh hardware info
		dispatcherTimer = new DispatcherTimer();
		dispatcherTimer.Tick += dispatcherTimer_Tick;
		dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
		dispatcherTimer.Start();
	}

	public void dispatcherTimer_Tick(object sender, object e) {
		System.Threading.Tasks.Task.Run(() => {
			App.GlobalHardwareInfo.Refresh();
		});
		Send();
	}

	private async void Init() {
		await webView.EnsureCoreWebView2Async();

		webView.CoreWebView2.SetVirtualHostNameToFolderMapping(
			"appassets", "assets", CoreWebView2HostResourceAccessKind.Allow);

		//webView.Source = new Uri("http://appassets/dist/index.html");
		webView.Source = new Uri("http://localhost:3000/");
		webView.CoreWebView2.OpenDevToolsWindow();
		webView.DefaultBackgroundColor = Windows.UI.Color.FromArgb(0, 1, 0, 0);

		webView.CoreWebView2.DOMContentLoaded += EventHandler;
	}

	public void EventHandler(object target, CoreWebView2DOMContentLoadedEventArgs arg) {
		Send();
	}

	public async void Send() {
		var CPUTemp = App.GlobalHardwareInfo.CPUTemp;

		var temp = "";

		for (int i = 0; i < CPUTemp.Count; i++) {
			// $"\n{hardver[i].Name}, value: {hardver[i].Value}, type {hardver[i].SensorType} - {hardver[i].Identifier} Min: {hardver[i].Min} Max: {hardver[i].Max}"
			if (CPUTemp[i].Name.StartsWith("CPU Core")) {
				temp += $"\nCore {i}: {CPUTemp[i].Value} Min: {CPUTemp[i].Min} Max: {CPUTemp[i].Max}";
			} else {
				break;
			}
		}

		temp += App.GlobalHardwareInfo.GPUName;

		text.Text = temp;

		await webView.CoreWebView2.ExecuteScriptAsync($"document.querySelector('#test').textContent = `{temp}`");
		await webView.CoreWebView2.ExecuteScriptAsync($"document.querySelector('#CPUName').textContent = `{App.GlobalHardwareInfo.CPUName}`");
		await webView.CoreWebView2.ExecuteScriptAsync($"document.querySelector('#GPUName').textContent = `{App.GlobalHardwareInfo.GPUName}`");
		await webView.CoreWebView2.ExecuteScriptAsync($"document.querySelector('#CPULoad').textContent = `{App.GlobalHardwareInfo.CPULoad.Last().Value}`");
	}
}
