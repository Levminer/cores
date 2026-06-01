using lib;
using Serilog;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace service;

internal sealed class Notification {
	private static readonly TimeSpan CooldownPeriod = TimeSpan.FromMinutes(15);
	private static readonly HttpClient _httpClient = new() { Timeout = TimeSpan.FromSeconds(10) };
	private readonly Dictionary<string, DateTimeOffset> startTimes = new();
	private readonly Dictionary<string, DateTimeOffset> lastFired = new();

	private static string GetKey(Notifications notification) {
		return $"{notification.json}|{notification.condition}|{notification.value}|{notification.seconds}";
	}

	private static bool TryGetJsonValue(JsonNode? root, string path, out double value) {
		value = default;

		if (root is null || string.IsNullOrWhiteSpace(path)) {
			return false;
		}

		JsonNode? current = root;

		foreach (var segment in path.Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)) {
			var remaining = segment;

			while (!string.IsNullOrEmpty(remaining)) {
				var bracketIndex = remaining.IndexOf('[');

				if (bracketIndex < 0) {
					if (current is not JsonObject jsonObject || !jsonObject.TryGetPropertyValue(remaining, out current)) {
						return false;
					}

					remaining = string.Empty;
					continue;
				}

				var propertyName = remaining[..bracketIndex];
				if (!string.IsNullOrWhiteSpace(propertyName)) {
					if (current is not JsonObject jsonObject || !jsonObject.TryGetPropertyValue(propertyName, out current)) {
						return false;
					}
				}

				var closingBracketIndex = remaining.IndexOf(']', bracketIndex + 1);
				if (closingBracketIndex < 0 || current is not JsonArray jsonArray) {
					return false;
				}

				var indexText = remaining[(bracketIndex + 1)..closingBracketIndex];
				if (!int.TryParse(indexText, out var index) || index < 0 || index >= jsonArray.Count) {
					return false;
				}

				current = jsonArray[index];
				remaining = remaining[(closingBracketIndex + 1)..];
			}
		}

		if (current is null) {
			return false;
		}

		try {
			value = current.GetValue<double>();
			return true;
		}
		catch {
			return false;
		}
	}

	public void Prune(IEnumerable<Notifications> activeNotifications) {
		var activeKeys = activeNotifications.Select(GetKey).ToHashSet();
		foreach (var key in startTimes.Keys.Except(activeKeys).ToList()) startTimes.Remove(key);
		foreach (var key in lastFired.Keys.Except(activeKeys).ToList()) lastFired.Remove(key);
	}

	public bool ShouldTrigger(JsonNode? hardwareJson, Notifications notification) {
		if (!TryGetJsonValue(hardwareJson, notification.json, out var currentValue)) {
			startTimes.Remove(GetKey(notification));
			return false;
		}

		var conditionMet = notification.condition switch {
			"higher" => currentValue > notification.value,
			"lower" => currentValue < notification.value,
			_ => false,
		};

		var key = GetKey(notification);

		if (!conditionMet) {
			startTimes.Remove(key);
			return false;
		}

		var now = DateTimeOffset.UtcNow;

		if (lastFired.TryGetValue(key, out var firedAt) && (now - firedAt) < CooldownPeriod) {
			return false;
		}

		if (!startTimes.TryGetValue(key, out var startTime)) {
			startTimes[key] = now;
			return false;
		}

		if ((now - startTime).TotalSeconds < notification.seconds) {
			return false;
		}

		startTimes.Remove(key);
		lastFired[key] = now;
		return true;
	}

	public async Task SendAsync(Notifications notification, string title, string body) {
		try {
			var request = new HttpRequestMessage {
				Method = HttpMethod.Post,
				RequestUri = new Uri("https://www.coresmonitor.com/api/notification"),
				Headers = {
					{ "Authorization", $"Bearer {Program.Settings.connectionCode}" },
				},
				Content = new StringContent(JsonSerializer.Serialize(new { title, body })) {
					Headers = {
						ContentType = new MediaTypeHeaderValue("application/json")
					}
				}
			};

			using var response = await _httpClient.SendAsync(request);
			response.EnsureSuccessStatusCode();
			Log.Information("Notification sent to API: {json}", notification.json);
		}
		catch (Exception ex) {
			Log.Error(ex, "Failed to send notification");
			SentrySdk.CaptureException(ex);
		}
	}
}
