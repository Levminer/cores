using H.NotifyIcon.EfficiencyMode;
using lib;
using Serilog;
using System.Runtime.Versioning;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace service;

internal class HardwareStats {
	public List<string> seconds = new();
	public List<string> minutes = new();
}

[SupportedOSPlatform("Windows")]
public class Program {
	internal static JsonSerializerOptions CompressedSerializerOptions = new() {
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
	};
	internal static JsonSerializerOptions SerializerOptions = new() {
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
		WriteIndented = true,
	};
	internal static HardwareStats HardwareStats = new();
	internal static Settings Settings = new();
	internal static Database Database = new();

	private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
		Log.Error(e.ExceptionObject as Exception, "App crashed with an unhandled exception");
		SentrySdk.CaptureException((Exception)e.ExceptionObject);
	}

	private static void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e) {
		var flattened = e.Exception.Flatten();
		Log.Error("Failed task: {@flattened}", flattened);
		SentrySdk.CaptureException(flattened);
		e.SetObserved();
	}

	public static void Main(string[] args) {
		SentrySdk.Init(settings => {
			settings.Dsn = "https://4e746421f320352c8db806951ee076e1@o4506670275428352.ingest.us.sentry.io/4507197428596736";
			settings.AutoSessionTracking = true;
			settings.IsGlobalModeEnabled = true;
			settings.TracesSampleRate = 0.8;
		});

		Log.Logger = new LoggerConfiguration()
				.WriteTo.Console()
				.WriteTo.File(Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Cores", "service.log"), fileSizeLimitBytes: 1024 * 1024, rollOnFileSizeLimit: true, retainedFileCountLimit: 2)
				.CreateLogger();

		AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
		TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

		// Turn on Windows Efficiency mode for process
		try {
			EfficiencyModeUtilities.SetEfficiencyMode(true);
		}
		catch (Exception ex) {
			Log.Error(ex, "Failed to turn on efficiency mode");
			SentrySdk.CaptureException(ex);
		}

		// Check if the firewall rule exists
		var ruleExists = Commands.ExecuteCommand(@"netsh advfirewall firewall show rule name='CoresService'");

		if (ruleExists.Contains("No rules match the specified criteria.")) {
			var exe = Path.Join(AppContext.BaseDirectory, "CoresService.exe");
			var res = Commands.ExecuteCommand($"netsh advfirewall firewall add rule name='CoresService' dir=in action=allow program='{exe}' enable=yes profile=private,public");
		}

		// Load db
		Database.Start();
		Database.Seed();
		Database.Cleanup();

		// Create the service
		HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
		builder.Services.AddWindowsService(options => {
			options.ServiceName = "Cores Service";
		});

		builder.Services.AddHostedService<WindowsBackgroundService>();

		IHost host = builder.Build();
		host.Run();
	}
}
