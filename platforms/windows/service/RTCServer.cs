using ezrtc;
using lib;
using SIPSorcery.Net;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace service;
public class RTCServer {
	internal static EzRTCHost EzRTCHost = new(new Uri($"wss://{Program.Settings.connectionURL ?? "rtc-usw.coresmonitor.com"}/one-to-many"), Program.Settings.connectionCode, new List<RTCIceServer> { new RTCIceServer { urls = "stun:stun.cloudflare.com:3478" } });
	internal static bool stop = false;

	public void Start(HardwareInfo hardwareInfo) {
		Task.Run(async () => {
			Task.Run(() => {
				EzRTCHost.Start();
			});

			EzRTCHost.dataChannelOpen += (RTCDataChannel data) => {
				EzRTCHost.sendMessageToAll(JsonSerializer.Serialize(new GenericMessage<API>() { Type = "initialData", Data = hardwareInfo.API }, Program.CompressedSerializerOptions));

				if (data.readyState == RTCDataChannelState.open) {
					var secondsList = Program.HardwareStats.seconds.Where((x, i) => (i + 1) % 3 == 0).ToList();

					for (int i = 0; i < secondsList.Count; i++) {
						data.send(JsonSerializer.Serialize(new GenericMessage<JsonNode>() { Type = "secondsData", Data = JsonNode.Parse(secondsList[i]) }, Program.CompressedSerializerOptions));
					}

					var minutesList = Program.HardwareStats.minutes.Where((x, i) => (i + 1) % 3 == 0).ToList();
					if (minutesList.Count > 0) {
						data.send(JsonSerializer.Serialize(new GenericMessage<JsonNode>() { Type = "initialMinutesData", Data = JsonNode.Parse(minutesList[0]) }, Program.CompressedSerializerOptions));
					}

					for (int i = 0; i < minutesList.Count; i++) {
						data.send(JsonSerializer.Serialize(new GenericMessage<JsonNode>() { Type = "minutesData", Data = JsonNode.Parse(minutesList[i]) }, Program.CompressedSerializerOptions));
					}
				}
			};

			EzRTCHost.dataChannelMessage += (data) => {
				Commands.HandleRemoteMessage(data);
			};

			EzRTCHost.keepAliveMessage += (websocketClient, text) => {
				var keepAlive = SignalMessage.KeepAlive.Decode(text);

				var status = new Status { is_host = true, session_id = EzRTCHost.sessionId, version = "0.6.0", metadata = new Dictionary<string, object>() };
				status.metadata.Add("cpu", hardwareInfo.API.CPU.MaxLoad);
				status.metadata.Add("ram", hardwareInfo.API.RAM.Load[2]?.Value ?? 0);
				if (hardwareInfo.API.GPU.Cards.Count > 0) {
					status.metadata.Add("gpu", hardwareInfo.API.GPU.Cards[0].MaxLoad);
				} else {
					status.metadata.Add("gpu", 0);
				}

				var message = SignalMessage.KeepAlive.Encode(keepAlive.userId, status);

				websocketClient.Send(message);
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
