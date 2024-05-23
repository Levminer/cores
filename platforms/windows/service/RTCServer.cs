using ezrtc;
using lib;
using SIPSorcery.Net;
using System.Text.Json;

namespace service;
public class RTCServer {
	internal static EzRTCHost EzRTCHost = new(new Uri("wss://rtc-usw.levminer.com/one-to-many"), Program.Settings.connectionCode, new List<RTCIceServer> { new RTCIceServer { urls = "stun:openrelay.metered.ca:80" }, new RTCIceServer { urls = "turn:standard.relay.metered.ca:443", credential = "8By67N7nOLDIagJk", username = "2ce7aaf275c1abdef74ec7e3", credentialType = RTCIceCredentialType.password } });
	internal static bool stop = false;

	public void Start(HardwareInfo hardwareInfo) {
		Task.Run(async () => {
			Task.Run(() => {
				EzRTCHost.Start();
			});

			EzRTCHost.onDataChannelOpen += (data) => {
				EzRTCHost.sendMessageToAll(JsonSerializer.Serialize(new NetworkMessage() { Type = "initialData", Data = hardwareInfo.API }, Program.CompressedSerializerOptions));
			};

			while (!stop) {
				EzRTCHost.sendMessageToAll(JsonSerializer.Serialize(new NetworkMessage() { Type = "data", Data = hardwareInfo.API }, Program.CompressedSerializerOptions));

				await Task.Delay(2000);
			}
		});
	}

	public void Stop() {
		stop = true;
	}
}
