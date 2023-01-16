using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;
using System.Runtime.InteropServices; // For DllImport
using WinRT; // required to support Window.As<ICompositionSupportsSystemBackdrop>()

namespace cores;

public class MicaBackrop {
	private WindowsSystemDispatcherQueueHelper m_wsdqHelper; // See below for implementation.
	private MicaController m_backdropController;
	private SystemBackdropConfiguration m_configurationSource;
	private readonly Window window;


	public MicaBackrop(Window _window) {
		window = _window;
	}

	public bool TrySetSystemBackdrop() {
		if (Microsoft.UI.Composition.SystemBackdrops.MicaController.IsSupported()) {
			m_wsdqHelper = new WindowsSystemDispatcherQueueHelper();
			m_wsdqHelper.EnsureWindowsSystemDispatcherQueueController();

			// Create the policy object.
			m_configurationSource = new SystemBackdropConfiguration();
			window.Activated += Window_Activated;
			window.Closed += Window_Closed;
			((FrameworkElement)window.Content).ActualThemeChanged += Window_ThemeChanged;

			// Initial configuration state.
			m_configurationSource.IsInputActive = true;
			SetConfigurationSourceTheme();

			m_backdropController = new Microsoft.UI.Composition.SystemBackdrops.MicaController();

			// Enable the system backdrop.
			// Note: Be sure to have "using WinRT;" to support the Window.As<...>() call.
			m_backdropController.AddSystemBackdropTarget(window.As<Microsoft.UI.Composition.ICompositionSupportsSystemBackdrop>());
			m_backdropController.SetSystemBackdropConfiguration(m_configurationSource);
			return true; // succeeded
		}

		return false; // Mica is not supported on this system
	}

	private void Window_Activated(object sender, WindowActivatedEventArgs args) {
		m_configurationSource.IsInputActive = args.WindowActivationState != WindowActivationState.Deactivated;
	}

	private void Window_Closed(object sender, WindowEventArgs args) {
		// Make sure any Mica/Acrylic controller is disposed
		// so it doesn't try to use this closed window.
		if (m_backdropController != null) {
			m_backdropController.Dispose();
			m_backdropController = null;
		}
		window.Activated -= Window_Activated;
		m_configurationSource = null;
	}

	private void Window_ThemeChanged(FrameworkElement sender, object args) {
		if (m_configurationSource != null) {
			SetConfigurationSourceTheme();
		}
	}

	private void SetConfigurationSourceTheme() {
		switch (((FrameworkElement)window.Content).ActualTheme) {
			case ElementTheme.Dark:
			m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Dark;
			break;
			case ElementTheme.Light:
			m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Light;
			break;
			case ElementTheme.Default:
			m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Default;
			break;
		}
	}
}

public class WindowsSystemDispatcherQueueHelper {
	[StructLayout(LayoutKind.Sequential)]
	struct DispatcherQueueOptions {
		internal int dwSize;
		internal int threadType;
		internal int apartmentType;
	}

	[DllImport("CoreMessaging.dll")]
	private static extern int CreateDispatcherQueueController([In] DispatcherQueueOptions options, [In, Out, MarshalAs(UnmanagedType.IUnknown)] ref object dispatcherQueueController);

	object m_dispatcherQueueController = null;
	public void EnsureWindowsSystemDispatcherQueueController() {
		if (Windows.System.DispatcherQueue.GetForCurrentThread() != null) {
			// one already exists, so we'll just use it.
			return;
		}

		if (m_dispatcherQueueController == null) {
			DispatcherQueueOptions options;
			options.dwSize = Marshal.SizeOf(typeof(DispatcherQueueOptions));
			options.threadType = 2;    // DQTYPE_THREAD_CURRENT
			options.apartmentType = 2; // DQTAT_COM_STA

			CreateDispatcherQueueController(options, ref m_dispatcherQueueController);
		}
	}
}
