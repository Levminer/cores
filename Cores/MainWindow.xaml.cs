using cores;
using Microsoft.UI.Xaml;
using Microsoft.Web.WebView2.Core;
using System;
using System.Diagnostics;
using System.Text.Json;

namespace Cores;

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
			"appassets", "assets", CoreWebView2HostResourceAccessKind.Allow);

		if (Debugger.IsAttached) {
			webView.Source = new Uri("http://localhost:3000");
		}

		webView.CoreWebView2.DOMContentLoaded += EventHandler;
	}

	public void EventHandler(object target, CoreWebView2DOMContentLoadedEventArgs arg) {
		Send();
	}

	public async void Send() {
		var serializeOptions = new JsonSerializerOptions {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			WriteIndented = true
		};

		var test = JsonSerializer.Serialize(App.GlobalHardwareInfo.API, serializeOptions);

		// text.Text = test;

		await webView.CoreWebView2.ExecuteScriptAsync($"document.querySelector('#api').textContent = `{test}`");
	}
}
