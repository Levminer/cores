use hardwareinfo::HardwareInfo;
use rusqlite::{params, Connection};

#[derive(Debug)]
struct Row {
    data: String,
}

pub fn seed(conn: &Connection) {
    let sql1 = "CREATE TABLE IF NOT EXISTS seconds_data (id INTEGER PRIMARY KEY AUTOINCREMENT, timestamp DATETIME DEFAULT CURRENT_TIMESTAMP, data TEXT);";
    let sql2 = "CREATE TABLE IF NOT EXISTS minutes_data (id INTEGER PRIMARY KEY AUTOINCREMENT, timestamp DATETIME DEFAULT CURRENT_TIMESTAMP, data TEXT);";
    let sql3 = "CREATE TABLE IF NOT EXISTS data (id INTEGER PRIMARY KEY AUTOINCREMENT, timestamp DATETIME DEFAULT CURRENT_TIMESTAMP, data TEXT);";

    conn.execute_batch(sql1).expect("Failed to create table");
    conn.execute_batch(sql2).expect("Failed to create table");
    conn.execute_batch(sql3).expect("Failed to create table");
}

pub fn cleanup(conn: &Connection) {
    let sql1 = "DELETE FROM seconds_data WHERE id NOT IN (SELECT id FROM seconds_data ORDER BY timestamp DESC LIMIT 60);";
    let sql2 = "DELETE FROM minutes_data WHERE id NOT IN (SELECT id FROM minutes_data ORDER BY timestamp DESC LIMIT 60);";
    // Keep the most recent 120 entries, and also keep at least one entry per 15-minute interval for the last 24 hours, but delete entries older than 1 hour that are not needed for the 15-minute intervals
    let sql3 = "DELETE FROM data WHERE id NOT IN (SELECT id FROM data ORDER BY id DESC LIMIT 120) AND id NOT IN (SELECT MIN(id) FROM data WHERE timestamp >= datetime('now', '-24 hours') GROUP BY strftime('%s', timestamp) / 900) AND timestamp < datetime('now', '-1 hour');";

    conn.execute_batch(sql1).expect("Failed to cleanup table");
    conn.execute_batch(sql2).expect("Failed to cleanup table");
    conn.execute_batch(sql3).expect("Failed to cleanup table");
}

pub fn insert_data(conn: &Connection, data: &str) {
    let sql = "INSERT INTO data (data) VALUES (?);";

    conn.execute(sql, params![data])
        .expect("Failed to insert data");
}

pub fn select_minutes_data(conn: &Connection) -> Vec<HardwareInfo> {
    let sql = "
        WITH config(window_seconds) AS (SELECT ?1)
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

    let mut stmt = conn.prepare(sql).expect("Failed to prepare statement");

    // 900 seconds = 15 minutes, so we get 96 data points for the last 24 hours
    let rows = stmt
        .query_map([900], |row| {
            Ok(Row {
                data: row.get(0).expect("Failed to get data"),
            })
        })
        .expect("Failed to query minutes data");

    return rows
        .map(|row| {
            let row = row.expect("Failed to get row");
            serde_json::from_str::<HardwareInfo>(&row.data)
                .expect("Failed to deserialize HardwareInfo")
        })
        .collect();
}

pub fn select_seconds_data(conn: &Connection) -> Vec<HardwareInfo> {
    let sql = "
        SELECT data FROM data
        ORDER BY timestamp DESC
        LIMIT 60;
    ";

    let mut stmt = conn.prepare(sql).expect("Failed to prepare statement");

    let mut result: Vec<HardwareInfo> = stmt
        .query_map([], |row| {
            Ok(Row {
                data: row.get(0).expect("Failed to get data"),
            })
        })
        .expect("Failed to query seconds data")
        .map(|row| {
            let row = row.expect("Failed to get row");
            serde_json::from_str::<HardwareInfo>(&row.data)
                .expect("Failed to deserialize HardwareInfo")
        })
        .collect();

    result.reverse();
    return result;
}
