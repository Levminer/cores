using cores;
using lib;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
using Sentry;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;
using Windows.ApplicationModel.DataTransfer;

namespace Cores;

public sealed partial class MainWindow : Window {
	private readonly DispatcherTimer APIRefresher;
	private bool firstRun = true;

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
		APIRefresher.Interval = new TimeSpan(0, 0, App.GlobalSettings.interval);
		APIRefresher.Start();
	}

	// Refresh API
	public void RefreshAPI(object sender, object e) {
		System.Threading.Tasks.Task.Run(() => {
			App.GlobalHardwareInfo.Refresh();
		});

		SendAPI();
	}

	// Start webview
	private async void Init() {
		await webView.EnsureCoreWebView2Async();
		webView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;

		if (!Debugger.IsAttached) {
			webView.CoreWebView2.Settings.AreDevToolsEnabled = true;
		}

		webView.CoreWebView2.SetVirtualHostNameToFolderMapping(
			"app.example", "assets", CoreWebView2HostResourceAccessKind.Allow);

		if (!Debugger.IsAttached) {
			webView.Source = new Uri("https://app.example/assets/index.html");
		}

		webView.CoreWebView2.DOMContentLoaded += EventHandler;
		webView.WebMessageReceived += WebView_WebMessageReceived;
		webView.CoreWebView2.NewWindowRequested += WebView_NewWindowRequested;
	}

	// Navigation view navigation
	private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args) {
		webView.CoreWebView2.PostWebMessageAsJson(JsonSerializer.Serialize(new Message() {
			Name = "navigation",
			Content = args.InvokedItem.ToString().ToLower()
		}, App.SerializerOptions));
	}

	// Send settings to the interface
	public void EventHandler(object target, CoreWebView2DOMContentLoadedEventArgs arg) {
		SendAPI();

		var message = new Message() {
			Name = "settings",
			Content = JsonSerializer.Serialize(App.GlobalSettings, App.SerializerOptions)
		};

		webView.CoreWebView2.PostWebMessageAsJson(JsonSerializer.Serialize(message, App.SerializerOptions));
	}

	// Open links in browser
	public void WebView_NewWindowRequested(object sender, CoreWebView2NewWindowRequestedEventArgs args) {
		Process.Start(new ProcessStartInfo(args.Uri) { UseShellExecute = true });
		args.Handled = true;
	}

	// WebView events
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
				App.GlobalSettings = JsonSerializer.Deserialize<Settings>(content.Content, App.SerializerOptions);
				App.GlobalSettings.SetSettings();

				APIRefresher.Interval = new TimeSpan(0, 0, App.GlobalSettings.interval);

				break;

			case "debug":
				var path = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "cores-debug.txt");
				var contents = $"{content.Content}\n{App.GlobalHardwareInfo.computer.GetReport()}";

				// write to file
				File.WriteAllText(path, contents);
				break;
		}
	}

	// Send API info to the interface
	public void SendAPI() {
		if (firstRun) {
			try {
				var appVersion = Assembly.GetExecutingAssembly().GetName().Version;

				App.GlobalHardwareInfo.API.System.OS.WebView = webView.CoreWebView2.Environment.BrowserVersionString;
				App.GlobalHardwareInfo.API.System.OS.App = $"{appVersion.Major}.{appVersion.Minor}.{appVersion.Build}";
				App.GlobalHardwareInfo.API.System.OS.Runtime = "1.3.230724000";
			}
			catch (Exception e) {
				SentrySdk.CaptureException(e);
				SentrySdk.CaptureMessage(e.ToString());
			}

			firstRun = false;
		}

		var message = new Message() {
			Name = "api",
			Content = JsonSerializer.Serialize(App.GlobalHardwareInfo.API, App.SerializerOptions)
		};

		webView.CoreWebView2.PostWebMessageAsJson(JsonSerializer.Serialize(message, App.SerializerOptions));
	}
}
