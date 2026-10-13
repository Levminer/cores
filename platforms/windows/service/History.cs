using lib;
using Serilog;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace service;

// Hardware history used by the dashboard charts. The last 60 samples are kept in memory only,
// one sample per 15 minutes is kept for 24 hours and persisted to a JSON file so it survives restarts.
// The data is only a cache, so a missing or unreadable file just means starting with an empty history.
public class History {
	private const int SecondsCapacity = 60;
	private static readonly TimeSpan MinutesWindow = TimeSpan.FromMinutes(15);
	private static readonly TimeSpan MinutesRetention = TimeSpan.FromHours(24);

	private record Entry(DateTime Timestamp, JsonElement Data);

	// Accessed from the insert loop and the HTTP/WebRTC read handlers.
	private readonly object gate = new();
	private readonly Queue<JsonElement> seconds = new(SecondsCapacity + 1);
	private List<Entry> minutes = [];
	private long lastWindow = -1;

	private static string HistoryPath => Path.Combine(Program.Settings.GetSettingsFolder(), "history.json");

	public void Load() {
		DeleteLegacyDatabase();

		try {
			var entries = JsonSerializer.Deserialize<List<Entry>>(File.ReadAllText(HistoryPath)) ?? [];
			var cutoff = DateTime.UtcNow - MinutesRetention;

			// Skip entries that are not a snapshot object, e.g. from a hand edited file.
			entries = entries.Where(e => e.Timestamp >= cutoff && e.Data.ValueKind == JsonValueKind.Object).ToList();

			lock (gate) {
				minutes = entries;
				lastWindow = minutes.Count > 0 ? GetWindow(minutes[^1].Timestamp) : -1;
			}
		}
		catch (FileNotFoundException) { }
		catch (Exception ex) {
			Log.Warning(ex, "Failed to load history, starting with an empty history");
		}
	}

	public void InsertData(API data) {
		try {
			var json = JsonSerializer.SerializeToElement(data, Program.CompressedSerializerOptions);
			var now = DateTime.UtcNow;
			var window = GetWindow(now);

			lock (gate) {
				seconds.Enqueue(json);

				if (seconds.Count > SecondsCapacity) {
					seconds.Dequeue();
				}

				// Keep the first sample of each window. Compare with != so a clock set backwards still records.
				if (window != lastWindow) {
					lastWindow = window;
					minutes.Add(new Entry(now, json));
					minutes.RemoveAll(e => e.Timestamp < now - MinutesRetention);
					Save();
				}
			}
		}
		catch (Exception ex) {
			Log.Error(ex, "Error inserting history");
		}
	}

	public List<JsonNode> SelectSecondsData() {
		JsonElement[] snapshot;

		lock (gate) {
			snapshot = seconds.ToArray();
		}

		return ToNodes(snapshot);
	}

	public List<JsonNode> SelectMinutesData() {
		var cutoff = DateTime.UtcNow - MinutesRetention;
		JsonElement[] snapshot;

		lock (gate) {
			snapshot = minutes.Where(e => e.Timestamp >= cutoff).OrderBy(e => e.Timestamp).Select(e => e.Data).ToArray();
		}

		return ToNodes(snapshot);
	}

	private static long GetWindow(DateTime timestamp) => timestamp.Ticks / MinutesWindow.Ticks;

	// Each caller gets its own nodes, the shared elements are read-only.
	private static List<JsonNode> ToNodes(JsonElement[] items) {
		return items.Select(item => JsonObject.Create(item)).OfType<JsonNode>().ToList();
	}

	private void Save() {
		var tempPath = HistoryPath + ".tmp";

		try {
			// Write to a temp file and swap it in, so a crash mid-write never leaves a half written history.
			using (var stream = File.Create(tempPath)) {
				JsonSerializer.Serialize(stream, minutes);
				stream.Flush(true);
			}

			File.Move(tempPath, HistoryPath, true);
		}
		catch (Exception ex) {
			Log.Warning(ex, "Failed to save history");
		}
	}

	// Older versions stored history in SQLite.
	private static void DeleteLegacyDatabase() {
		var dbPath = Path.Combine(Program.Settings.GetSettingsFolder(), "stats.sqlite");

		foreach (var path in new[] { dbPath, $"{dbPath}-journal", $"{dbPath}-wal", $"{dbPath}-shm" }) {
			try {
				File.Delete(path);
			}
			catch (Exception ex) {
				Log.Warning(ex, "Failed to delete legacy database file {Path}", path);
			}
		}
	}
}
