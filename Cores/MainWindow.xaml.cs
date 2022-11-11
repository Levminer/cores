using cores;
using Microsoft.UI.Xaml;
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

		// Set titlebar
		SetTitleBar(AppTitleBar);
		ExtendsContentIntoTitleBar = true;

		// Set Mica
		var mica = new Mica(this);
		mica.TrySetSystemBackdrop();
	}

	private void mainNavigation_SelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args) {
		if (args.IsSettingsSelected) {
			// contentFrame5.Navigate(typeof(SampleSettingsPage));
		} else {
			var selectedItem = (Microsoft.UI.Xaml.Controls.NavigationViewItem)args.SelectedItem;
			var selectedItemTag = ((string)selectedItem.Tag);
			sender.Header = selectedItem.Content;

			var pageName = $"Cores.Views.{selectedItemTag}";
			Type pageType = Type.GetType(pageName);
			mainContent.Navigate(pageType);
		}
	}

	private void mainNavigation_Loaded(object sender, RoutedEventArgs e) {
		var pageName = "Cores.Views.CPUView";
		Type pageType = Type.GetType(pageName);
		mainContent.Navigate(pageType);
	}
}
