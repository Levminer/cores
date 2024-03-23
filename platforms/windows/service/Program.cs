using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;
using System.Runtime.Versioning;
using System.Text.Json;

namespace service;

[SupportedOSPlatform("Windows")]
public class Program {
	internal static JsonSerializerOptions SerializerOptions = new() {
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		WriteIndented = true,
	};

	public static void Main(string[] args) {
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
