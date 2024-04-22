using ezrtc;
using lib;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SIPSorcery.Net;
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
	internal static HTTPServer HTTPServer = new();
	internal static WSServer WSServer = new();
	internal static RTCServer RTCServer = new();

	public WindowsBackgroundService(ILogger<WindowsBackgroundService> logger) {
		this.logger = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
		logger.LogWarning("Starting Cores service");
		HardwareInfo.GetInfo();
		HTTPServer.Start(HardwareInfo);
		WSServer.Start(HardwareInfo);
		RTCServer.Start(HardwareInfo);

		while (!stoppingToken.IsCancellationRequested) {
			try {
				HardwareInfo.Refresh();
				await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
			}
			catch (OperationCanceledException) {
				HTTPServer.Stop();
				WSServer.Stop();
				RTCServer.Stop();
				HardwareInfo.Stop();
			}
			catch (Exception ex) {
				logger.LogError(ex, "{Message}", ex.Message);

				Environment.Exit(1);
			}
		}
	}
}
