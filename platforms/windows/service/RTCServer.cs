using ezrtc;
using lib;
using SIPSorcery.Net;
using System.Text.Json;

namespace service;
public class RTCServer {
	internal static EzRTCHost EzRTCHost = new(new Uri("wss://rtc-usw.levminer.com/one-to-many"), Program.Settings.connectionCode, new List<RTCIceServer> { new RTCIceServer { urls = "stun:openrelay.metered.ca:80" }, new RTCIceServer { urls = "turn:standard.relay.metered.ca:443", credential = "hZA1e3RHAhw70JoP", username = "34a987bde7c718428704bde7", credentialType = RTCIceCredentialType.password }, new RTCIceServer { urls = "turn:standard.relay.metered.ca:80", credential = "hZA1e3RHAhw70JoP", username = "34a987bde7c718428704bde7", credentialType = RTCIceCredentialType.password } });
	internal static bool stop = false;

	public void Start(HardwareInfo hardwareInfo) {
		Task.Run(async () => {
			Task.Run(() => {
				EzRTCHost.Start();
			});

			EzRTCHost.dataChannelOpen += (data) => {
				EzRTCHost.sendMessageToAll(JsonSerializer.Serialize(new GenericMessage<API>() { Type = "initialData", Data = hardwareInfo.API }, Program.CompressedSerializerOptions));
			};

			EzRTCHost.dataChannelMessage += (data) => {
				var netMessage = JsonSerializer.Deserialize<GenericMessage<string>>(data, Program.SerializerOptions);

				switch (netMessage?.Data) {
					case "shutdown":
						Commands.ExecuteCommand(@"shutdown /s /t 30");
						break;

					case "sleep":
						Commands.ExecuteCommand("Start-Process rundll32.exe -ArgumentList 'powrprof.dll,SetSuspendState 0,1,0'");
						break;

					case "restart":
						Commands.ExecuteCommand("shutdown /r /t 30");
						break;
				}
			};

			while (!stop) {
				EzRTCHost.sendMessageToAll(JsonSerializer.Serialize(new GenericMessage<API>() { Type = "data", Data = hardwareInfo.API }, Program.CompressedSerializerOptions));

				await Task.Delay(2000);
			}
		});
	}

	public void Stop() {
		stop = true;
	}
}
