using lib;
using Serilog;

namespace service;
public sealed class WindowsBackgroundService : BackgroundService {
	internal static HardwareInfo HardwareInfo = new(Program.Settings);
	internal static RTCServer RTCServer = new();
	internal static Analytics Analytics = new();
	internal static Server Server = new();

	protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
		Log.Information("Starting Cores service");
		HardwareInfo.GetInfo();
		Server.Start(HardwareInfo);

		// Send analytics
		_ = Task.Run(async () => {
			await Analytics.SendEvent(Program.Settings);
		});

		// Start remote connection
		if (Program.Settings.remoteConnections) {
			_ = Task.Run(() => {
				RTCServer.Start(HardwareInfo);
			});
		}

		// Store last 60 minutes statistics
		// TODO: Should take the avg. of the last 60s
		_ = Task.Run(async () => {
			while (!stoppingToken.IsCancellationRequested) {
				Program.Database.InsertMinutesData(HardwareInfo.API);

				await Task.Delay(TimeSpan.FromSeconds(60));
			}
		});

		// Cleanup old data
		_ = Task.Run(async () => {
			while (!stoppingToken.IsCancellationRequested) {
				Log.Information("Cleanup completed");
				Program.Database.Cleanup();
				await Task.Delay(TimeSpan.FromMinutes(60));
			}
		});

		while (!stoppingToken.IsCancellationRequested) {
			try {
				HardwareInfo.Refresh();

				Program.Database.InsertSecondsData(HardwareInfo.API);

				// Wait for configured interval and account for processing time
				await Task.Delay(TimeSpan.FromMilliseconds((Program.Settings.interval * 1000) - 300), stoppingToken);
			}
			catch (OperationCanceledException) {
				RTCServer.Stop();
				Server.Stop();
				Program.Database.Close();
				HardwareInfo.Stop();
			}
			catch (Exception ex) {
				Log.Error("Exception in main service loop: {@error}", ex.Message);
				SentrySdk.CaptureException(ex);

				Environment.Exit(1);
			}
		}
	}
}
