using lib;
using Serilog;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace service;

// Hardware history used by the dashboard charts. The last 60 samples are kept in memory only,
// one sample per 15 minutes is kept for 24 hours and persisted so it survives restarts.
// The file has one JSON entry per line, new entries are appended. Old entries are only removed by
// rewriting the file on startup and once it holds about twice the retention.
// The data is only a cache, so a missing or unreadable file just means starting with an empty history.
public class History {
	private const int SecondsCapacity = 60;
	private static readonly TimeSpan MinutesWindow = TimeSpan.FromMinutes(15);
	private static readonly TimeSpan MinutesRetention = TimeSpan.FromHours(24);
	private static readonly int MaxFileLines = (int)(MinutesRetention / MinutesWindow) * 2;

	private record Entry(DateTime Timestamp, JsonElement Data);

	// Accessed from the insert loop and the HTTP/WebRTC read handlers.
	private readonly object gate = new();
	private readonly Queue<JsonElement> seconds = new(SecondsCapacity + 1);
	private List<Entry> minutes = [];
	private long lastWindow = -1;
	private int fileLines;

	private static string HistoryPath => Path.Combine(Program.Settings.GetSettingsFolder(), "history.jsonl");

	public void Load() {
		DeleteLegacyDatabase();

		List<Entry> entries = [];
		var lines = 0;

		try {
			var cutoff = DateTime.UtcNow - MinutesRetention;

			foreach (var line in File.ReadLines(HistoryPath)) {
				if (string.IsNullOrWhiteSpace(line)) {
					continue;
				}

				lines++;

				var entry = ParseEntry(line);

				if (entry != null && entry.Timestamp >= cutoff) {
					entries.Add(entry);
				}
			}
		}
		catch (FileNotFoundException) { }
		catch (Exception ex) {
			Log.Warning(ex, "Failed to load history, starting with an empty history");
		}

		lock (gate) {
			minutes = entries;
			lastWindow = minutes.Count > 0 ? GetWindow(minutes[^1].Timestamp) : -1;
			fileLines = lines;

			// Drop old and unreadable lines, e.g. a half written line from a crash.
			if (entries.Count != lines) {
				Compact();
			}
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
					var entry = new Entry(now, json);

					lastWindow = window;
					minutes.Add(entry);
					minutes.RemoveAll(e => e.Timestamp < now - MinutesRetention);

					if (fileLines >= MaxFileLines) {
						Compact();
					}
					else {
						Append(entry);
					}
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

	// Identifies the newest minutes entry, read it before SelectMinutesData so a new entry is never missed.
	public long SelectMinutesWindow() {
		lock (gate) {
			return lastWindow;
		}
	}

	// Returns the newest minutes entry if one was added after the given window.
	public (long Window, JsonNode Data)? SelectMinutesDataSince(long window) {
		JsonElement data;
		long latest;

		lock (gate) {
			if (lastWindow == window || minutes.Count == 0) {
				return null;
			}

			latest = lastWindow;
			data = minutes[^1].Data;
		}

		return (latest, JsonObject.Create(data)!);
	}

	private static long GetWindow(DateTime timestamp) => timestamp.Ticks / MinutesWindow.Ticks;

	// Each caller gets its own nodes, the shared elements are read-only.
	private static List<JsonNode> ToNodes(JsonElement[] items) {
		return items.Select(item => JsonObject.Create(item)).OfType<JsonNode>().ToList();
	}

	private static Entry? ParseEntry(string line) {
		try {
			var entry = JsonSerializer.Deserialize<Entry>(line);

			// Skip entries that are not a snapshot object, e.g. from a hand edited file.
			return entry?.Data.ValueKind == JsonValueKind.Object ? entry : null;
		}
		catch (JsonException) {
			return null;
		}
	}

	private static byte[] ToLine(Entry entry) {
		return [.. JsonSerializer.SerializeToUtf8Bytes(entry), (byte)'\n'];
	}

	private void Append(Entry entry) {
		try {
			using var stream = new FileStream(HistoryPath, FileMode.Append, FileAccess.Write);
			stream.Write(ToLine(entry));
			stream.Flush(true);
			fileLines++;
		}
		catch (Exception ex) {
			Log.Warning(ex, "Failed to save history");

			// A failed append can leave a partial line behind, rewrite the whole file next time.
			fileLines = MaxFileLines;
		}
	}

	private void Compact() {
		var tempPath = HistoryPath + ".tmp";

		try {
			// Write to a temp file and swap it in, so a crash mid-write never leaves a half written history.
			using (var stream = File.Create(tempPath)) {
				foreach (var entry in minutes) {
					stream.Write(ToLine(entry));
				}

				stream.Flush(true);
			}

			File.Move(tempPath, HistoryPath, true);
			fileLines = minutes.Count;
		}
		catch (Exception ex) {
			Log.Warning(ex, "Failed to save history");

			// Retry on the next insert instead of appending to a file that may end with a partial line.
			fileLines = MaxFileLines;
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
