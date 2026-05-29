using lib;
using Microsoft.Data.Sqlite;
using Serilog;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace service;

public class Database {
	internal static SqliteConnection connection = null;
	public void Start() {
		try {
			var settingsFolder = Program.Settings.GetSettingsFolder();
			var dbPath = Path.Combine(settingsFolder, "stats.sqlite");
			connection = new SqliteConnection($"DataSource={dbPath}");
			connection.Open();
		}
		catch (Exception) {
			// Fall back to in-memory database if the file cannot be opened
			connection = new SqliteConnection("DataSource=:memory:");
			connection.Open();
		}
	}

	public void Close() {
		connection.Close();
	}

	public void Seed() {
		using var command = connection.CreateCommand();
		command.CommandText = "CREATE TABLE IF NOT EXISTS seconds_data (id INTEGER PRIMARY KEY AUTOINCREMENT, timestamp DATETIME DEFAULT CURRENT_TIMESTAMP, data TEXT);";
		command.ExecuteNonQuery();
		command.CommandText = "CREATE TABLE IF NOT EXISTS minutes_data (id INTEGER PRIMARY KEY AUTOINCREMENT, timestamp DATETIME DEFAULT CURRENT_TIMESTAMP, data TEXT);";
		command.ExecuteNonQuery();
		command.CommandText = "CREATE TABLE IF NOT EXISTS data (id INTEGER PRIMARY KEY AUTOINCREMENT, timestamp DATETIME DEFAULT CURRENT_TIMESTAMP, data TEXT);";
		command.ExecuteNonQuery();
	}

	public void Cleanup() {
		using var command = connection.CreateCommand();
		command.CommandText = "DELETE FROM seconds_data WHERE id NOT IN (SELECT id FROM seconds_data ORDER BY timestamp DESC LIMIT 60);";
		command.ExecuteNonQuery();
		command.CommandText = "DELETE FROM minutes_data WHERE id NOT IN (SELECT id FROM minutes_data ORDER BY timestamp DESC LIMIT 60);";
		command.ExecuteNonQuery();

		// Keep the most recent 120 entries, and also keep at least one entry per 15-minute interval for the last 24 hours, but delete entries older than 1 hour that are not needed for the 15-minute intervals
		command.CommandText = "DELETE FROM data WHERE id NOT IN (SELECT id FROM data ORDER BY id DESC LIMIT 120) AND id NOT IN (SELECT MIN(id) FROM data WHERE timestamp >= datetime('now', '-24 hours') GROUP BY strftime('%s', timestamp) / 900) AND timestamp < datetime('now', '-1 hour');";
		command.ExecuteNonQuery();
	}

	public void InsertData(API data) {
		try {
			using var insertCommand = connection.CreateCommand();
			insertCommand.CommandText = "INSERT INTO data (data) VALUES (@data);";
			var param = insertCommand.CreateParameter();
			param.ParameterName = "@data";
			param.Value = JsonSerializer.Serialize(data, Program.CompressedSerializerOptions);
			insertCommand.Parameters.Add(param);
			insertCommand.ExecuteNonQuery();
		}
		catch (Exception ex) {
			Log.Error(ex, "Error inserting data");
		}
	}

	public List<JsonNode> SelectSecondsData() {
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

	public List<JsonNode> SelectMinutesData() {
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
