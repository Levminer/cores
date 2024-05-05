using lib;
using System.Net;
using System.Text;
using System.Text.Json;

namespace service;
public class HTTPServer {
	static HttpListener listener;

	public void Start(HardwareInfo hardwareInfo) {
		string url = "http://localhost:5390/";

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

			// Process the request asynchronously
			await ProcessRequestAsync(context, hardwareInfo);
		}
	}

	static async Task ProcessRequestAsync(HttpListenerContext context, HardwareInfo hardwareInfo) {
		string path = context.Request.RawUrl;
		string method = context.Request.HttpMethod;

		if (method == "OPTIONS") {
			await HandleOptionsRequest(context);
			return;
		}

		switch (path) {
			case "/":
				await HandleRootRequest(context);
				break;
			case "/data":
				await HandleDataRequest(context, hardwareInfo);
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

	static async Task HandleDataRequest(HttpListenerContext context, HardwareInfo hardwareInfo) {
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
					Console.WriteLine(newSettings.ToString());
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

	public void Stop() {
		listener.Stop();
	}
}
