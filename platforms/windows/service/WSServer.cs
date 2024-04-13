using lib;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace service;
public class WSServer {
	static HttpListener listener;
	static ConcurrentDictionary<WebSocket, Task> connectedClients = new ConcurrentDictionary<WebSocket, Task>();

	public void Start(HardwareInfo hardwareInfo) {
		string url = "http://localhost:5391/";

		// Create an HttpListener
		listener = new HttpListener();
		listener.Prefixes.Add(url);

		// Start the listener
		listener.Start();

		Task.Run(async () => await HandleRequests(hardwareInfo));
	}

	static async Task HandleRequests(HardwareInfo hardwareInfo) {
		while (true) {
			// Wait for a request to be received asynchronously
			HttpListenerContext context = await listener.GetContextAsync();

			if (context.Request.IsWebSocketRequest) {
				WebSocketContext wsContext = await context.AcceptWebSocketAsync(null);
				WebSocket socket = wsContext.WebSocket;
				Task clientTask = ProcessWebSocketRequestAsync(socket, hardwareInfo);
				connectedClients.TryAdd(socket, clientTask);
			} else {
				byte[] responseBytes = Encoding.UTF8.GetBytes("Connection header did not include 'upgrade'");
				context.Response.ContentLength64 = responseBytes.Length;
				context.Response.OutputStream.Write(responseBytes, 0, responseBytes.Length);
				context.Response.OutputStream.Close();
			}
		}
	}

	static async Task ProcessWebSocketRequestAsync(WebSocket socket, HardwareInfo hardwareInfo) {
		byte[] initialBuffer = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(hardwareInfo.API, Program.SerializerOptions));
		await socket.SendAsync(new ArraySegment<byte>(initialBuffer, 0, initialBuffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);

		var receiveBuffer = new ArraySegment<byte>(new byte[1024 * 4]);
		WebSocketReceiveResult result;

		Task sendTask = Task.Run(async () => {
			while (socket.State == WebSocketState.Open) {
				byte[] buffer = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(hardwareInfo.API, Program.SerializerOptions));
				await socket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);
				await Task.Delay(2000);
			}
		});

		while (socket.State == WebSocketState.Open) {
			result = await socket.ReceiveAsync(receiveBuffer, CancellationToken.None);

			if (result.MessageType == WebSocketMessageType.Close) {
				await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
				connectedClients.TryRemove(socket, out _);
				break;
			}
		}

		// Wait for the send task to complete
		await sendTask;
	}

	public void Stop() {
		listener.Stop();
	}
}
