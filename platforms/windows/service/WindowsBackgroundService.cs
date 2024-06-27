using lib;
using Serilog;
using System.Text.Json;

namespace service;
public sealed class WindowsBackgroundService : BackgroundService {
	internal static HardwareInfo HardwareInfo = new();
	internal static HTTPServer HTTPServer = new();
	internal static WSServer WSServer = new();
	internal static RTCServer RTCServer = new();
	internal static Analytics Analytics = new();

	protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
		Log.Information("Starting Cores service");
		HardwareInfo.GetInfo();
		HTTPServer.Start(HardwareInfo);
		WSServer.Start(HardwareInfo);

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
				if (Program.HardwareStats.minutes.Count() < 60) {
					Program.HardwareStats.minutes.Add(JsonSerializer.Serialize(HardwareInfo.API, Program.CompressedSerializerOptions));
				} else {
					var api = new API();
					Program.HardwareStats.minutes.RemoveAt(0);
					Program.HardwareStats.minutes.Add(JsonSerializer.Serialize(HardwareInfo.API, Program.CompressedSerializerOptions));
				}

				await Task.Delay(TimeSpan.FromSeconds(60));
			}
		});

		while (!stoppingToken.IsCancellationRequested) {
			try {
				HardwareInfo.Refresh();

				if (Program.HardwareStats.seconds.Count() < 60) {
					Program.HardwareStats.seconds.Add(JsonSerializer.Serialize(HardwareInfo.API, Program.CompressedSerializerOptions));
				} else {
					var api = new API();
					Program.HardwareStats.seconds.RemoveAt(0);
					Program.HardwareStats.seconds.Add(JsonSerializer.Serialize(HardwareInfo.API, Program.CompressedSerializerOptions));
				}

				await Task.Delay(TimeSpan.FromSeconds(Program.Settings.interval), stoppingToken);
			}
			catch (OperationCanceledException) {
				RTCServer.Stop();
				HTTPServer.Stop();
				WSServer.Stop();
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
