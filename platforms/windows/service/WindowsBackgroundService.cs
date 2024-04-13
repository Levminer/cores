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
	internal static HTTPServer Server = new();
	internal static WSServer WSServer = new();
	internal static EzRTCHost EzRTCHost = new(new Uri("wss://rtc-usw.levminer.com/one-to-many"), "crs_1d7de676b4", new List<RTCIceServer> { new RTCIceServer { urls = "stun:openrelay.metered.ca:80" }, new RTCIceServer { urls = "turn:standard.relay.metered.ca:443", credential = "8By67N7nOLDIagJk", username = "2ce7aaf275c1abdef74ec7e3", credentialType = RTCIceCredentialType.password } });

	public WindowsBackgroundService(ILogger<WindowsBackgroundService> logger) {
		this.logger = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
		logger.LogWarning("Starting Cores service");
		HardwareInfo.GetInfo();
		Server.Start(HardwareInfo);
		WSServer.Start(HardwareInfo);

		Task.Run(() => {
			EzRTCHost.Start();
		});

		while (!stoppingToken.IsCancellationRequested) {
			try {
				HardwareInfo.Refresh();
				EzRTCHost.sendMessageToAll(JsonSerializer.Serialize(HardwareInfo.API, Program.SerializerOptions));
				await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
			}
			catch (OperationCanceledException) {
				Server.Stop();
				HardwareInfo.Stop();
				WSServer.Stop();
			}
			catch (Exception ex) {
				logger.LogError(ex, "{Message}", ex.Message);

				Environment.Exit(1);
			}
		}
	}
}
