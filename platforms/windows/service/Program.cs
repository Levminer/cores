using H.NotifyIcon.EfficiencyMode;
using lib;
using Serilog;
using System.Runtime.Versioning;
using System.Text.Json;

namespace service;

internal class HardwareStats {
	public List<string> seconds = new();
	public List<string> minutes = new();
}

[SupportedOSPlatform("Windows")]
public class Program {
	internal static JsonSerializerOptions CompressedSerializerOptions = new() {
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
	};
	internal static JsonSerializerOptions SerializerOptions = new() {
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		WriteIndented = true,
	};
	internal static HardwareStats HardwareStats = new();
	internal static Settings Settings = new();

	private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
		Log.Error("App crashed with an unhandled exception");
		SentrySdk.CaptureException((Exception)e.ExceptionObject);
	}

	public static void Main(string[] args) {
		SentrySdk.Init(settings => {
			settings.Dsn = "https://d4ad9ffb8f4333d328052ee6551db833@o4506670275428352.ingest.us.sentry.io/4507197428596736";
			settings.AutoSessionTracking = true;
			settings.IsGlobalModeEnabled = true;
			settings.EnableTracing = true;
		});

		Log.Logger = new LoggerConfiguration()
				.WriteTo.Console()
				.WriteTo.File("./cores-service.log")
				.CreateLogger();

		AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

		// Turn on Windows Efficiency mode for process
		try {
			EfficiencyModeUtilities.SetEfficiencyMode(true);
		}
		catch (Exception e) {
			Log.Error("Failed to turn on efficiency mode");
			SentrySdk.CaptureException(e);
		}

		// Check if the firewall rule exists
		var ruleExists = Commands.ExecuteCommand(@"netsh advfirewall firewall show rule name='CoresService'");

		if (ruleExists.Contains("No rules match the specified criteria.")) {
			var exe = Path.Join(AppContext.BaseDirectory, "CoresService.exe");
			var res = Commands.ExecuteCommand($"netsh advfirewall firewall add rule name='CoresService' dir=in action=allow program='{exe}' enable=yes profile=private,public");
		}

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
