using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;
using System.Runtime.Versioning;

namespace service;

[SupportedOSPlatform("Windows")]
public class Program {
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
