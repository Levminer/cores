using lib;
using System.Runtime.InteropServices;

namespace service;
public sealed class WindowsBackgroundService : BackgroundService {
	private readonly ILogger<WindowsBackgroundService> logger;
	internal static HardwareInfo HardwareInfo = new();
	internal static HTTPServer HTTPServer = new();
	internal static WSServer WSServer = new();
	internal static RTCServer RTCServer = new();

	public WindowsBackgroundService(ILogger<WindowsBackgroundService> logger) {
		this.logger = logger;
	}

	[DllImport("rtc.dll")]
	private static extern void start();

	[DllImport("rtc.dll")]
	private static extern void stop();

	protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
		logger.LogWarning("Starting Cores service");
		HardwareInfo.GetInfo();
		HTTPServer.Start(HardwareInfo);
		WSServer.Start(HardwareInfo);

		if (Program.Settings.remoteConnections) {
			Task.Run(() => {
				start();
			});
		}

		while (!stoppingToken.IsCancellationRequested) {
			try {
				HardwareInfo.Refresh();
				await Task.Delay(TimeSpan.FromSeconds(Program.Settings.interval), stoppingToken);
			}
			catch (OperationCanceledException) {
				stop();
				HTTPServer.Stop();
				WSServer.Stop();
				HardwareInfo.Stop();
			}
			catch (Exception ex) {
				logger.LogError(ex, "{Message}", ex.Message);
				SentrySdk.CaptureException(ex);

				Environment.Exit(1);
			}
		}
	}
}
