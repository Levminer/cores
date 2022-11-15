using cores;
using Microsoft.UI.Xaml;
using Microsoft.Web.WebView2.Core;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Cores;
/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window {
	public MainWindow() {
		InitializeComponent();

		// Set Mica
		var mica = new Mica(this);
		mica.TrySetSystemBackdrop();

		Init();
	}

	private async void Init() {
		await webView.EnsureCoreWebView2Async();

		webView.CoreWebView2.SetVirtualHostNameToFolderMapping(
			"appassets", "assets", CoreWebView2HostResourceAccessKind.Allow);

		webView.Source = new Uri("http://appassets/dist/index.html");
		webView.CoreWebView2.OpenDevToolsWindow();
	}
}
