using lib;
using Serilog;
using System.Collections.Concurrent;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace service;
public class Server {
	static HttpListener listener;
	static ConcurrentDictionary<WebSocket, Task> connectedClients = new ConcurrentDictionary<WebSocket, Task>();

	// Start the server
	public void Start(HardwareInfo hardwareInfo) {
		string url = "http://localhost:5390/";

		// Create an HttpListener
		listener = new HttpListener();
		listener.Prefixes.Add(url);

		// Start the listener
		listener.Start();

		Task.Run(async () => await HandleRequests(hardwareInfo));
	}

	// Wait for a request
	static async Task HandleRequests(HardwareInfo hardwareInfo) {
		while (true) {
			// Wait for a request to be received asynchronously
			HttpListenerContext context = await listener.GetContextAsync();

			// Process the request asynchronously
			await ProcessRequestAsync(context, hardwareInfo);
		}
	}

	static async Task ProcessRequestAsync(HttpListenerContext context, HardwareInfo hardwareInfo) {
		if (context.Request.HttpMethod == "OPTIONS") {
			await HandleOptionsRequest(context);
			return;
		}

		switch (context.Request.RawUrl) {
			case "/":
				await HandleRootRequest(context);
				break;
			case "/ws":
				if (context.Request.IsWebSocketRequest) {
					WebSocketContext wsContext = await context.AcceptWebSocketAsync(null);
					WebSocket socket = wsContext.WebSocket;
					Task clientTask = HandleWSRequest(socket, hardwareInfo);
					connectedClients.TryAdd(socket, clientTask);
				} else {
					byte[] responseBytes = Encoding.UTF8.GetBytes("Connection header did not include 'upgrade'");
					context.Response.ContentLength64 = responseBytes.Length;
					context.Response.OutputStream.Write(responseBytes, 0, responseBytes.Length);
					context.Response.OutputStream.Close();
				}
				break;
			case "/rest":
				await HandleRESTRequest(context, hardwareInfo);
				break;
			case "/post":
				await HandlePostRequest(context, hardwareInfo);
				break;
			default:
				await HandleNotFoundRequest(context);
				break;
		}
	}

	static async Task HandleOptionsRequest(HttpListenerContext context) {
		context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
		context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
		context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Accept, X-Requested-With");
		context.Response.Headers.Add("Access-Control-Max-Age", "86400"); // Cache the preflight response for 24 hours

		context.Response.StatusCode = 200; // Set the status code to OK
		context.Response.Close();
	}

	static async Task HandleRootRequest(HttpListenerContext context) {
		byte[] buffer = Encoding.UTF8.GetBytes("OK");
		await SendResponse(context, buffer, "text/plain");
	}

	static async Task HandleRESTRequest(HttpListenerContext context, HardwareInfo hardwareInfo) {
		string responseJson = JsonSerializer.Serialize(hardwareInfo.API, Program.SerializerOptions);
		byte[] buffer = Encoding.UTF8.GetBytes(responseJson);
		await SendResponse(context, buffer, "application/json");
	}

	static async Task HandlePostRequest(HttpListenerContext context, HardwareInfo hardwareInfo) {
		// Read the request body
		using (var reader = new StreamReader(context.Request.InputStream)) {
			string requestBody = await reader.ReadToEndAsync();

			try {
				var message = JsonSerializer.Deserialize<ProtocolMessage>(requestBody, Program.SerializerOptions);

				if (message.Type == "new_settings") {
					var newSettings = JsonSerializer.Deserialize<Settings>(message.Data.Settings);
					Log.Information("New settings: {@Settings}", newSettings);
					Program.Settings = newSettings;
				}

				if (message.Type == "debug_report") {
					var contents = $"{message.Data.SystemInfo}\n{hardwareInfo.computer.GetReport()}";

					File.WriteAllText(message.Data.FilePath, contents);
				}

				// Send a response
				byte[] buffer = Encoding.UTF8.GetBytes(requestBody);
				await SendResponse(context, buffer, "application/json");
			}
			catch (Exception e) {
				SentrySdk.CaptureException(e);
				byte[] buffer = Encoding.UTF8.GetBytes("ERROR");
				await SendResponse(context, buffer, "text/plain", 400);
			}


		}
	}

	static async Task HandleNotFoundRequest(HttpListenerContext context) {
		byte[] buffer = Encoding.UTF8.GetBytes("404 Not Found");
		await SendResponse(context, buffer, "text/plain", 404);
	}

	static async Task SendResponse(HttpListenerContext context, byte[] buffer, string contentType, int statusCode = 200) {
		context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
		context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
		context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Accept, X-Requested-With");
		context.Response.ContentType = contentType;
		context.Response.ContentLength64 = buffer.Length;
		context.Response.StatusCode = statusCode;

		using (Stream output = context.Response.OutputStream) {
			await output.WriteAsync(buffer, 0, buffer.Length);
		}

		context.Response.Close();
	}

	static async Task HandleWSRequest(WebSocket socket, HardwareInfo hardwareInfo) {
		// Send the initial data
		byte[] initialBuffer = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new GenericMessage<API>() { Type = "initialData", Data = hardwareInfo.API }, Program.CompressedSerializerOptions));
		await socket.SendAsync(new ArraySegment<byte>(initialBuffer, 0, initialBuffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);

		var receiveBuffer = new ArraySegment<byte>(new byte[1024 * 4]);
		WebSocketReceiveResult result;

		// Send last 60s and last 60 minutes data
		await Task.Run(async () => {
			var secondsList = Program.HardwareStats.seconds.Where((x, i) => (i + 1) % 3 == 0).ToList();

			for (int i = 0; i < secondsList.Count; i++) {
				byte[] buffer = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new GenericMessage<JsonNode>() { Type = "secondsData", Data = JsonNode.Parse(secondsList[i]) }, Program.CompressedSerializerOptions));
				await socket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);
			}

			var minutesList = Program.HardwareStats.minutes.Where((x, i) => (i + 1) % 3 == 0).ToList();

			for (int i = 0; i < minutesList.Count; i++) {
				byte[] buffer = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new GenericMessage<JsonNode>() { Type = "minutesData", Data = JsonNode.Parse(minutesList[i]) }, Program.CompressedSerializerOptions));
				await socket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);
			}
		});

		// Send updated data every 2s
		Task sendTask = Task.Run(async () => {
			while (socket.State == WebSocketState.Open) {
				byte[] buffer = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new GenericMessage<API>() { Type = "data", Data = hardwareInfo.API }, Program.CompressedSerializerOptions));
				await socket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);
				await Task.Delay(2000);
			}
		});

		try {
			while (socket.State == WebSocketState.Open) {
				result = await socket.ReceiveAsync(receiveBuffer, CancellationToken.None);

				if (result.MessageType == WebSocketMessageType.Close) {
					await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
					connectedClients.TryRemove(socket, out _);
					break;
				} else if (result.MessageType == WebSocketMessageType.Text) {
					string receivedText = Encoding.UTF8.GetString(receiveBuffer.Array, receiveBuffer.Offset, result.Count);
					HandleWSMessage(receivedText);
				}
			}
		}
		catch (Exception) {
			Log.Error("Failed to close WS connection");
			connectedClients.TryRemove(socket, out _);
		}

		// Wait for the send task to complete
		await sendTask;
	}

	static void HandleWSMessage(string message) {
		Commands.HandleRemoteMessage(message);
	}

	public void Stop() {
		listener.Stop();
	}
}
