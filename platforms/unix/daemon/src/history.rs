// Hardware history used by the dashboard charts. The last 60 samples are kept in memory only,
// one sample per 15 minutes is kept for 24 hours and persisted to a JSON file so it survives restarts.
// The data is only a cache, so a missing or unreadable file just means starting with an empty history.

use hardwareinfo::HardwareInfo;
use log::warn;
use serde::{Deserialize, Serialize};
use std::collections::VecDeque;
use std::fs::File;
use std::io::{BufWriter, ErrorKind, Write};
use std::path::{Path, PathBuf};
use std::sync::Mutex;
use std::time::{SystemTime, UNIX_EPOCH};

const SECONDS_CAPACITY: usize = 60;
const MINUTES_WINDOW: u64 = 15 * 60;
const MINUTES_RETENTION: u64 = 24 * 60 * 60;

#[derive(Serialize, Deserialize)]
struct Entry {
    timestamp: u64,
    // Stored untyped so a history written by another version never fails to load as a whole.
    data: serde_json::Value,
}

struct State {
    seconds: VecDeque<HardwareInfo>,
    minutes: Vec<Entry>,
    last_window: Option<u64>,
}

pub struct History {
    path: PathBuf,
    state: Mutex<State>,
}

impl History {
    pub fn load(folder: &Path) -> History {
        delete_legacy_database(folder);

        let path = folder.join("history.json");
        let mut minutes = match std::fs::read_to_string(&path) {
            Ok(text) => serde_json::from_str::<Vec<Entry>>(&text).unwrap_or_else(|e| {
                warn!(
                    "Failed to parse history, starting with an empty history: {}",
                    e
                );
                Vec::new()
            }),
            Err(e) if e.kind() == ErrorKind::NotFound => Vec::new(),
            Err(e) => {
                warn!(
                    "Failed to read history, starting with an empty history: {}",
                    e
                );
                Vec::new()
            }
        };

        let cutoff = now().saturating_sub(MINUTES_RETENTION);
        minutes.retain(|e| e.timestamp >= cutoff);
        let last_window = minutes.last().map(|e| e.timestamp / MINUTES_WINDOW);

        History {
            path,
            state: Mutex::new(State {
                seconds: VecDeque::with_capacity(SECONDS_CAPACITY + 1),
                minutes,
                last_window,
            }),
        }
    }

    pub fn insert(&self, data: &HardwareInfo) {
        let now = now();
        let window = now / MINUTES_WINDOW;
        let mut state = self.state.lock().unwrap();

        state.seconds.push_back(data.clone());

        if state.seconds.len() > SECONDS_CAPACITY {
            state.seconds.pop_front();
        }

        // Keep the first sample of each window. Compare with != so a clock set backwards still records.
        if state.last_window != Some(window) {
            let value = match serde_json::to_value(data) {
                Ok(value) => value,
                Err(e) => {
                    warn!("Failed to serialize history entry: {}", e);
                    return;
                }
            };

            state.last_window = Some(window);
            state.minutes.push(Entry {
                timestamp: now,
                data: value,
            });
            let cutoff = now.saturating_sub(MINUTES_RETENTION);
            state.minutes.retain(|e| e.timestamp >= cutoff);

            if let Err(e) = self.save(&state.minutes) {
                warn!("Failed to save history: {}", e);
            }
        }
    }

    pub fn seconds(&self) -> Vec<HardwareInfo> {
        self.state.lock().unwrap().seconds.iter().cloned().collect()
    }

    pub fn minutes(&self) -> Vec<HardwareInfo> {
        let cutoff = now().saturating_sub(MINUTES_RETENTION);
        let mut entries: Vec<(u64, serde_json::Value)> = self
            .state
            .lock()
            .unwrap()
            .minutes
            .iter()
            .filter(|e| e.timestamp >= cutoff)
            .map(|e| (e.timestamp, e.data.clone()))
            .collect();

        entries.sort_by_key(|(timestamp, _)| *timestamp);
        entries
            .into_iter()
            .filter_map(|(_, data)| serde_json::from_value(data).ok())
            .collect()
    }

    pub fn latest(&self) -> Option<HardwareInfo> {
        self.state.lock().unwrap().seconds.back().cloned()
    }

    // Write to a temp file and swap it in, so a crash mid-write never leaves a half written history.
    fn save(&self, minutes: &[Entry]) -> std::io::Result<()> {
        let temp_path = self.path.with_extension("json.tmp");

        let mut writer = BufWriter::new(File::create(&temp_path)?);
        serde_json::to_writer(&mut writer, minutes)?;
        writer.flush()?;
        writer.get_ref().sync_all()?;

        std::fs::rename(&temp_path, &self.path)
    }
}

fn now() -> u64 {
    SystemTime::now()
        .duration_since(UNIX_EPOCH)
        .map(|d| d.as_secs())
        .unwrap_or(0)
}

// Older versions stored history in SQLite.
fn delete_legacy_database(folder: &Path) {
    for name in [
        "stats.sqlite",
        "stats.sqlite-journal",
        "stats.sqlite-wal",
        "stats.sqlite-shm",
    ] {
        match std::fs::remove_file(folder.join(name)) {
            Err(e) if e.kind() != ErrorKind::NotFound => {
                warn!("Failed to delete legacy database file {}: {}", name, e);
            }
            _ => {}
        }
    }
}
