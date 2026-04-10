using lib;
using Serilog;

namespace service;

public sealed class WindowsBackgroundService : BackgroundService {
	internal static HardwareInfo HardwareInfo = new(Program.Settings);
	internal static RTCServer RTCServer = new();
	internal static Analytics Analytics = new();
	internal static Server Server = new();

	private static Task StartSupervisedTask(string taskName, Func<CancellationToken, Task> taskFactory, CancellationToken stoppingToken) {
		return Task.Run(async () => {
			try {
				await taskFactory(stoppingToken);
			}
			catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested) {
				Log.Information("Background task '{TaskName}' cancelled", taskName);
			}
			catch (Exception ex) {
				var wrapped = new InvalidOperationException($"Background task '{taskName}' crashed: ", ex);
				Log.Error(wrapped, "{TaskName} failed", taskName);
				SentrySdk.CaptureException(wrapped);
				throw wrapped;
			}
		}, stoppingToken);
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
		Log.Information("Starting Cores service");
		HardwareInfo.GetInfo();
		Server.Start(HardwareInfo);

		var backgroundTasks = new List<Task>();

		// Send analytics
		backgroundTasks.Add(StartSupervisedTask("Analytics.SendEvent", async _ => {
			await Analytics.SendEvent(Program.Settings);
		}, stoppingToken));

		// Start remote connection
		if (Program.Settings.remoteConnections) {
			backgroundTasks.Add(StartSupervisedTask("RTCServer.Start", _ => {
				RTCServer.Start(HardwareInfo);
				return Task.CompletedTask;
			}, stoppingToken));
		}

		// Store last 60 minutes statistics
		// TODO: Should take the avg. of the last 60s
		backgroundTasks.Add(StartSupervisedTask("Database.InsertMinutesData", async token => {
			while (!token.IsCancellationRequested) {
				Program.Database.InsertMinutesData(HardwareInfo.API);

				await Task.Delay(TimeSpan.FromSeconds(60), token);
			}
		}, stoppingToken));

		// Cleanup old data
		backgroundTasks.Add(StartSupervisedTask("Database.Cleanup", async token => {
			while (!token.IsCancellationRequested) {
				Program.Database.Cleanup();
				Log.Information("Cleanup completed");
				await Task.Delay(TimeSpan.FromMinutes(60), token);
			}
		}, stoppingToken));

		try {
			while (!stoppingToken.IsCancellationRequested) {
				var faultedTask = backgroundTasks.FirstOrDefault(task => task.IsFaulted);
				if (faultedTask is not null) {
					throw faultedTask.Exception?.Flatten() ?? new Exception("Background task failed with unknown error");
				}

				HardwareInfo.Refresh();

				Program.Database.InsertSecondsData(HardwareInfo.API);

				// Wait for configured interval and account for processing time
				await Task.Delay(TimeSpan.FromMilliseconds((Program.Settings.interval * 1000) - 300), stoppingToken);
			}
		}
		catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested) {
			Log.Information("Service stopping");
		}
		catch (Exception ex) {
			Log.Error(ex, "Exception in main service loop");
			SentrySdk.CaptureException(ex);
			throw;
		}
		finally {
			try {
				await Task.WhenAll(backgroundTasks);
			}
			catch (Exception ex) {
				Log.Error(ex, "One or more supervised background tasks failed during shutdown");
			}

			RTCServer.Stop();
			Server.Stop();
			Program.Database.Close();
			HardwareInfo.Stop();
		}
	}
}
