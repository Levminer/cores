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
	private readonly DispatcherTimer APIRefresher;

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

		APIRefresher = new DispatcherTimer();
		APIRefresher.Tick += RefreshAPI;
		APIRefresher.Interval = new TimeSpan(0, 0, App.GlobalSettings.settings.Interval);
		APIRefresher.Start();
	}

	public void RefreshAPI(object sender, object e) {
		System.Threading.Tasks.Task.Run(() => {
			App.GlobalHardwareInfo.Refresh();
		});

		SendAPI();
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
		SendAPI();

		var message = new Message() {
			Name = "settings",
			Content = JsonSerializer.Serialize(App.GlobalSettings.settings, App.SerializerOptions)
		};

		webView.CoreWebView2.PostWebMessageAsJson(JsonSerializer.Serialize(message, App.SerializerOptions));
	}

	// Open links in browser
	public void WebView_NewWindowRequested(object sender, CoreWebView2NewWindowRequestedEventArgs args) {
		Process.Start(new ProcessStartInfo(args.Uri) { UseShellExecute = true });
		args.Handled = true;
	}

	// About dialog
	public async void WebView_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs args) {
		var content = JsonSerializer.Deserialize<Message>(args.WebMessageAsJson, App.SerializerOptions);
		Debug.WriteLine($"Message: {content.Name}");

		switch (content.Name) {
			case "about":
				aboutDialogText.Text = content.Content;

				var dialogResult = await aboutDialog.ShowAsync();

				if (dialogResult.ToString() == "Primary") {
					var data = new DataPackage();
					data.SetText(content.Content);

					Clipboard.SetContent(data);
				}
				break;

			case "newSettings":
				App.GlobalSettings.SetSettings(content.Content);
				break;

			case "debug":

				var message = new Message() {
					Name = "debug",
					Content = $"{content.Content}\n{App.GlobalHardwareInfo.computer.GetReport()}"
				};

				webView.CoreWebView2.PostWebMessageAsJson(JsonSerializer.Serialize(message, App.SerializerOptions));
				break;
		}
	}

	// Send API info to the interface
	public async void SendAPI() {
		var appVersion = Assembly.GetExecutingAssembly().GetName().Version;

		App.GlobalHardwareInfo.API.System.OS.WebView = webView.CoreWebView2.Environment.BrowserVersionString;
		App.GlobalHardwareInfo.API.System.OS.App = $"{appVersion.Major}.{appVersion.Minor}.{appVersion.Build}";
		App.GlobalHardwareInfo.API.System.OS.Runtime = "1.2.221209.1";

		var JSON = JsonSerializer.Serialize(App.GlobalHardwareInfo.API, App.SerializerOptions);

		await webView.CoreWebView2.ExecuteScriptAsync($"document.querySelector('#api').textContent = `{JSON}`");
	}
}
