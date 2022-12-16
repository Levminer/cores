using cores;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.Web.WebView2.Core;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using Windows.ApplicationModel.DataTransfer;

namespace Cores;

public sealed partial class MainWindow : Window {
	private readonly DispatcherTimer dispatcherTimer;

	public MainWindow() {
		InitializeComponent();

		// Set titlebar
		if (AppWindowTitleBar.IsCustomizationSupported()) {
			ExtendsContentIntoTitleBar = true;
			SetTitleBar(AppTitleBar);

		} else {
			AppTitleBar.Visibility = Visibility.Collapsed;
		}

		// Set Mica
		var mica = new MicaBackrop(this);
		mica.TrySetSystemBackdrop();

		// Webview setup
		Init();

		// Refresh hardware info
		dispatcherTimer = new DispatcherTimer();
		dispatcherTimer.Tick += dispatcherTimer_Tick;
		dispatcherTimer.Interval = new TimeSpan(0, 0, 2);
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
			"app.example", "assets", CoreWebView2HostResourceAccessKind.Allow);

		if (!Debugger.IsAttached) {
			webView.Source = new Uri("http://app.example/assets/index.html");
		}

		webView.CoreWebView2.DOMContentLoaded += EventHandler;
		webView.WebMessageReceived += WebView_WebMessageReceived;
		webView.CoreWebView2.NewWindowRequested += WebView_NewWindowRequested;
	}

	public void EventHandler(object target, CoreWebView2DOMContentLoadedEventArgs arg) {
		Send();
	}

	// Open links in browser
	public void WebView_NewWindowRequested(object sender, CoreWebView2NewWindowRequestedEventArgs args) {
		Process.Start(new ProcessStartInfo(args.Uri) { UseShellExecute = true });
		args.Handled = true;
	}

	// About dialog
	public async void WebView_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs args) {
		var res = args.TryGetWebMessageAsString();
		aboutDialogText.Text = res;

		var dialogResult = await aboutDialog.ShowAsync();

		if (dialogResult.ToString() == "Primary") {
			var content = new DataPackage();
			content.SetText(res);

			Clipboard.SetContent(content);
		}
	}

	// Send API info to the interface
	public async void Send() {
		var appVersion = Assembly.GetExecutingAssembly().GetName().Version;

		App.GlobalHardwareInfo.API.System.OS.WebView = webView.CoreWebView2.Environment.BrowserVersionString;
		App.GlobalHardwareInfo.API.System.OS.App = $"{appVersion.Major}.{appVersion.Minor}.{appVersion.Build}";
		App.GlobalHardwareInfo.API.System.OS.Runtime = "1.2.221209.1";

		var serializeOptions = new JsonSerializerOptions {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			WriteIndented = true
		};

		var JSON = JsonSerializer.Serialize(App.GlobalHardwareInfo.API, serializeOptions);

		await webView.CoreWebView2.ExecuteScriptAsync($"document.querySelector('#api').textContent = `{JSON}`");
	}
}
