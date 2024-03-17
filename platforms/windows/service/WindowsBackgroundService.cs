using lib;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO.Pipes;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace service;
public sealed class WindowsBackgroundService : BackgroundService {
	private readonly ILogger<WindowsBackgroundService> logger;
	internal static HardwareInfo HardwareInfo = new();
	internal static Server Server = new();

	public WindowsBackgroundService(ILogger<WindowsBackgroundService> logger) {
		this.logger = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
		logger.LogWarning("Starting Cores service");
		HardwareInfo.GetInfo();
		Server.Start(HardwareInfo);

		while (!stoppingToken.IsCancellationRequested) {
			try {
				HardwareInfo.Refresh();
				await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
			}
			catch (OperationCanceledException) {
				Server.Stop();
				HardwareInfo.Stop();
			}
			catch (Exception ex) {
				logger.LogError(ex, "{Message}", ex.Message);

				Environment.Exit(1);
			}
		}
	}
}
