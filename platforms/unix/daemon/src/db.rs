use duckdb::{params, Connection};
use hardwareinfo::HardwareInfo;

#[derive(Debug)]
struct Row {
    data: String,
}

pub fn seed(conn: &Connection) {
    let sql1 = "CREATE TABLE IF NOT EXISTS seconds_data (id UUID DEFAULT uuid(), timestamp TIMESTAMP DEFAULT now(), data JSON);";
    let sql2 = "CREATE TABLE IF NOT EXISTS minutes_data (id UUID DEFAULT uuid(), timestamp TIMESTAMP DEFAULT now(), data JSON);";

    conn.execute_batch(sql1).expect("Failed to create table");
    conn.execute_batch(sql2).expect("Failed to create table");
}

pub fn insert_seconds_data(conn: &Connection, data: &str) {
    let sql = "INSERT INTO seconds_data (data) VALUES (?);";

    conn.execute(sql, params![data])
        .expect("Failed to insert seconds data");
}

pub fn insert_minutes_data(conn: &Connection, data: &str) {
    let sql = "INSERT INTO minutes_data (data) VALUES (?);";

    conn.execute(sql, params![data])
        .expect("Failed to insert minutes data");
}

pub fn select_seconds_data(conn: &Connection) -> Vec<HardwareInfo> {
    let sql = "SELECT data FROM seconds_data ORDER BY timestamp DESC LIMIT 60;";
    let mut stmt = conn.prepare(sql).expect("Failed to prepare statement");

    let rows = stmt
        .query_map([], |row| {
            Ok(Row {
                data: row.get(0).expect("Failed to get data"),
            })
        })
        .expect("Failed to query seconds data");

    let mut result: Vec<HardwareInfo> = rows
        .map(|row| {
            let row = row.expect("Failed to get row");
            serde_json::from_str::<HardwareInfo>(&row.data)
                .expect("Failed to deserialize HardwareInfo")
        })
        .collect();

    result.reverse();
    return result;
}

pub fn select_minutes_data(conn: &Connection) -> Vec<HardwareInfo> {
    let sql = "SELECT data FROM minutes_data ORDER BY timestamp DESC LIMIT 60;";
    let mut stmt = conn.prepare(sql).expect("Failed to prepare statement");

    let rows = stmt
        .query_map([], |row| {
            Ok(Row {
                data: row.get(0).expect("Failed to get data"),
            })
        })
        .expect("Failed to query seconds data");

    let mut result: Vec<HardwareInfo> = rows
        .map(|row| {
            let row = row.expect("Failed to get row");
            serde_json::from_str::<HardwareInfo>(&row.data)
                .expect("Failed to deserialize HardwareInfo")
        })
        .collect();

    result.reverse();
    return result;
}
