using Serilog;
using System.Text;
using System.Text.Json;

namespace lib {
	public class AnalyticsPayload {
		public string api_key { get; set; }
		public string @event { get; set; }
		public string distinct_id { get; set; }
	}

	public class Analytics {
		public static async Task SendEvent(Settings settings) {
			string url = "https://eu.i.posthog.com/capture/";

			var payload = new AnalyticsPayload {
				api_key = "phc_2zbUPXXhnelCYP2VLXeWZvKy0hykzQA7edSOsFrYZaa",
				distinct_id = settings.userId,
				@event = "service"
			};

			try {
				using var httpClient = new HttpClient();
				var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

				var response = await httpClient.PostAsync(url, content);

				response.EnsureSuccessStatusCode();

				var responseContent = await response.Content.ReadAsStringAsync();
			}
			catch (Exception ex) {
				Log.Error("Failed to send analytics");
			}
		}
	}
}
