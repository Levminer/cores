using lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace service;
public class Server {
	static HttpListener listener;

	public void Start(HardwareInfo hardwareInfo) {
		string url = "http://localhost:8080/";

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
		// Prepare the response content (JSON)
		string responseJson = JsonSerializer.Serialize(hardwareInfo.API, Program.SerializerOptions);
		byte[] buffer = Encoding.UTF8.GetBytes(responseJson);

		// Set response headers, including CORS headers
		context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
		context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
		context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Accept, X-Requested-With");

		context.Response.ContentType = "application/json";
		context.Response.ContentLength64 = buffer.Length;

		// Write the response data to the output stream asynchronously
		using (Stream output = context.Response.OutputStream) {
			await output.WriteAsync(buffer, 0, buffer.Length);
		}

		// Close the response
		context.Response.Close();
	}

	public void Stop() {
		listener.Stop();
	}
}
