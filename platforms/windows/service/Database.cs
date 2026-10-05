using lib;
using Microsoft.Data.Sqlite;
using Serilog;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace service;

public class Database {
	internal static SqliteConnection connection = null;

	// SqliteConnection is not thread-safe. Serialize every access since the connection is shared
	// across the insert loop, cleanup task, and HTTP/WebRTC read handlers.
	private static readonly object gate = new();

	private static readonly TimeSpan RecoveryCooldown = TimeSpan.FromHours(1);
	private static DateTime lastRecovery = DateTime.MinValue;
	private static bool insertFailing;

	private static string DatabasePath => Path.Combine(Program.Settings.GetSettingsFolder(), "stats.sqlite");

	// CORRUPT, FULL, NOTADB: the file is unusable, but it only caches history so it is safe to discard.
	private static bool IsRecoverable(SqliteException ex) => ex.SqliteErrorCode is 11 or 13 or 26;

	public void Initialize() {
		lock (gate) {
			try {
				Open(DatabasePath);
				CheckIntegrity();
				SeedTables();
				CleanupTables();
			}
			catch (SqliteException ex) when (IsRecoverable(ex)) {
				Recover(ex, "startup");
			}
			catch (Exception ex) {
				Log.Error(ex, "Failed to open database file, using in-memory database instead");
				SentrySdk.CaptureException(ex);
				OpenInMemory();
			}
		}
	}

	public void Close() {
		lock (gate) {
			connection.Close();
		}
	}

	private static void Open(string dataSource) {
		connection?.Dispose();
		// Pooling would keep the file handle open after Dispose and block deleting the file during recovery.
		connection = new SqliteConnection(new SqliteConnectionStringBuilder { DataSource = dataSource, Pooling = false }.ToString());
		connection.Open();

		// WAL survives crashes better with fewer fsyncs; in-memory temp store avoids SQLITE_FULL from the system temp folder.
		using var command = connection.CreateCommand();
		command.CommandText = "PRAGMA journal_mode=WAL; PRAGMA synchronous=NORMAL; PRAGMA temp_store=MEMORY;";
		command.ExecuteNonQuery();
	}

	private static void OpenInMemory() {
		Open(":memory:");
		SeedTables();
	}

	// Open() does not read the file, so corruption would otherwise only surface on the first query.
	private static void CheckIntegrity() {
		using var command = connection.CreateCommand();
		command.CommandText = "PRAGMA quick_check;";
		var result = command.ExecuteScalar() as string;

		if (result != "ok") {
			throw new SqliteException($"Integrity check failed: {result}", 11);
		}
	}

	private static void Recover(SqliteException ex, string operation) {
		if (DateTime.UtcNow - lastRecovery < RecoveryCooldown) {
			return;
		}

		lastRecovery = DateTime.UtcNow;

		Log.Warning(ex, "Database error {ErrorCode} during {Operation}, recreating database", ex.SqliteErrorCode, operation);
		SentrySdk.CaptureException(ex, scope => {
			scope.Level = SentryLevel.Warning;
			scope.SetTag("db.operation", operation);
			scope.SetFingerprint(["sqlite-recovery", ex.SqliteErrorCode.ToString()]);
		});

		try {
			connection?.Dispose();

			foreach (var suffix in new[] { "", "-journal", "-wal", "-shm" }) {
				File.Delete(DatabasePath + suffix);
			}

			Open(DatabasePath);
			SeedTables();
			Log.Information("Database recreated");
		}
		catch (Exception recreateEx) {
			Log.Error(recreateEx, "Failed to recreate database file, using in-memory database instead");
			OpenInMemory();
		}
	}

	private static void SeedTables() {
		using var command = connection.CreateCommand();
		// Legacy tables, no longer written to.
		command.CommandText = "DROP TABLE IF EXISTS seconds_data; DROP TABLE IF EXISTS minutes_data;";
		command.ExecuteNonQuery();
		command.CommandText = "CREATE TABLE IF NOT EXISTS data (id INTEGER PRIMARY KEY AUTOINCREMENT, timestamp DATETIME DEFAULT CURRENT_TIMESTAMP, data TEXT);";
		command.ExecuteNonQuery();
	}

	public void Cleanup() {
		lock (gate) {
			try {
				CleanupTables();
			}
			catch (SqliteException ex) when (IsRecoverable(ex)) {
				Recover(ex, "cleanup");
			}
		}
	}

	private static void CleanupTables() {
		using var command = connection.CreateCommand();
		// Keep the most recent 120 entries, and also keep at least one entry per 15-minute interval for the last 24 hours, but delete entries older than 1 hour that are not needed for the 15-minute intervals
		command.CommandText = "DELETE FROM data WHERE id NOT IN (SELECT id FROM data ORDER BY id DESC LIMIT 120) AND id NOT IN (SELECT MIN(id) FROM data WHERE timestamp >= datetime('now', '-24 hours') GROUP BY strftime('%s', timestamp) / 900) AND timestamp < datetime('now', '-1 hour');";
		command.ExecuteNonQuery();
	}

	public void InsertData(API data) {
		lock (gate) {
			try {
				using var insertCommand = connection.CreateCommand();
				insertCommand.CommandText = "INSERT INTO data (data) VALUES (@data);";
				var param = insertCommand.CreateParameter();
				param.ParameterName = "@data";
				param.Value = JsonSerializer.Serialize(data, Program.CompressedSerializerOptions);
				insertCommand.Parameters.Add(param);
				insertCommand.ExecuteNonQuery();
				insertFailing = false;
			}
			catch (SqliteException ex) when (IsRecoverable(ex)) {
				Recover(ex, "insert");
			}
			catch (Exception ex) {
				// Runs every interval, so only log the first failure in a row.
				if (!insertFailing) {
					Log.Error(ex, "Error inserting data");
				}

				insertFailing = true;
			}
		}
	}

	public List<JsonNode> SelectSecondsData() {
		lock (gate) {
			try {
				return ReadSecondsData();
			}
			catch (SqliteException ex) when (IsRecoverable(ex)) {
				Recover(ex, "select");
				return [];
			}
		}
	}

	public List<JsonNode> SelectMinutesData() {
		lock (gate) {
			try {
				return ReadMinutesData();
			}
			catch (SqliteException ex) when (IsRecoverable(ex)) {
				Recover(ex, "select");
				return [];
			}
		}
	}

	private static List<JsonNode> ReadSecondsData() {
		using var selectCommand = connection.CreateCommand();
		selectCommand.CommandText = "SELECT data FROM data ORDER BY timestamp DESC LIMIT 60;";
		using var reader = selectCommand.ExecuteReader();

		var jsonList = new List<JsonNode>();

		while (reader.Read()) {
			var jsonString = reader.GetString(0);
			var node = JsonNode.Parse(jsonString);

			if (node != null) {
				jsonList.Add(node);
			}
		}

		jsonList.Reverse();
		return jsonList;
	}

	private static List<JsonNode> ReadMinutesData() {
		using var selectCommand = connection.CreateCommand();
		selectCommand.CommandText = @"
			WITH config(window_seconds) AS (SELECT @window_seconds)
			SELECT data
			FROM data, config
			WHERE id IN (
				SELECT MIN(id)
				FROM data, config
				WHERE timestamp >= datetime('now', '-24 hours')
				GROUP BY strftime('%s', timestamp) / window_seconds
			)
			ORDER BY timestamp ASC;
		";
		selectCommand.Parameters.AddWithValue("@window_seconds", 900);
		using var reader = selectCommand.ExecuteReader();

		var jsonList = new List<JsonNode>();

		while (reader.Read()) {
			var jsonString = reader.GetString(0);
			var node = JsonNode.Parse(jsonString);

			if (node != null) {
				jsonList.Add(node);
			}
		}

		return jsonList;
	}
}
