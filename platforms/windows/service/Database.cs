using DuckDB.NET.Data;
using lib;
using Serilog;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace service;
public class Database {
	internal static DuckDBConnection connection = null;
	public void Start() {
		try {
			connection = new DuckDBConnection("DataSource=stats.duckdb");
			connection.Open();
		}
		catch (Exception) {
			// Fall back to in-memory database if the file cannot be opened
			connection = new DuckDBConnection("DataSource=:memory:");
			connection.Open();
		}
	}

	public void Close() {
		connection.Close();
	}

	public void Seed() {
		using var command = connection.CreateCommand();
		command.CommandText = "CREATE TABLE IF NOT EXISTS seconds_data (id UUID DEFAULT uuid(), timestamp TIMESTAMP DEFAULT now(), data JSON);";
		command.ExecuteNonQuery();
		command.CommandText = "CREATE TABLE IF NOT EXISTS minutes_data (id UUID DEFAULT uuid(), timestamp TIMESTAMP DEFAULT now(), data JSON);";
		command.ExecuteNonQuery();
	}

	public void InsertSecondsData(API data) {
		using var insertCommand = connection.CreateCommand();
		insertCommand.CommandText = "INSERT INTO seconds_data (data) VALUES (?);";
		var param = insertCommand.CreateParameter();
		param.Value = JsonSerializer.Serialize(data, Program.CompressedSerializerOptions);
		insertCommand.Parameters.Add(param);
		insertCommand.ExecuteNonQuery();
	}

	public void InsertMinutesData(API data) {
		using var insertCommand = connection.CreateCommand();
		insertCommand.CommandText = "INSERT INTO minutes_data (data) VALUES (?);";
		var param = insertCommand.CreateParameter();
		param.Value = JsonSerializer.Serialize(data, Program.CompressedSerializerOptions);
		insertCommand.Parameters.Add(param);
		insertCommand.ExecuteNonQuery();
	}

	public List<JsonNode> SelectSecondsData() {
		using var selectCommand = connection.CreateCommand();
		selectCommand.CommandText = "SELECT data, timestamp FROM seconds_data ORDER BY timestamp DESC LIMIT 60;";
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
		selectCommand.CommandText = "SELECT data, timestamp FROM minutes_data ORDER BY timestamp DESC LIMIT 60;";
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
}
