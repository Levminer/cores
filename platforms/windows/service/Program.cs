using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;
using System.Diagnostics;
using System.Runtime.Versioning;
using System.Text.Json;

namespace service;

[SupportedOSPlatform("Windows")]
public class Program {
	internal static JsonSerializerOptions SerializerOptions = new() {
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		WriteIndented = true,
	};
	private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
		Debug.WriteLine("App crashed");
		SentrySdk.CaptureException((Exception)e.ExceptionObject);
	}

	public static void Main(string[] args) {
		SentrySdk.Init(settings => {
			settings.Dsn = "https://d4ad9ffb8f4333d328052ee6551db833@o4506670275428352.ingest.us.sentry.io/4507197428596736";
			settings.AutoSessionTracking = true;
			settings.IsGlobalModeEnabled = true;
			settings.EnableTracing = true;
		});

		AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

		HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
		builder.Services.AddWindowsService(options => {
			options.ServiceName = "Cores Service";
		});

		LoggerProviderOptions.RegisterProviderOptions<
			EventLogSettings, EventLogLoggerProvider>(builder.Services);

		builder.Services.AddHostedService<WindowsBackgroundService>();

		IHost host = builder.Build();
		host.Run();
	}
}
