// Hardware history used by the dashboard charts. The last 60 samples are kept in memory only,
// one sample per 15 minutes is kept for 24 hours and persisted so it survives restarts.
// The file has one JSON entry per line, new entries are appended. Old entries are only removed by
// rewriting the file on startup and once it holds about twice the retention.
// The data is only a cache, so a missing or unreadable file just means starting with an empty history.

use hardwareinfo::HardwareInfo;
use log::warn;
use serde::{Deserialize, Serialize};
use std::collections::VecDeque;
use std::fs::{File, OpenOptions};
use std::io::{BufRead, BufReader, BufWriter, ErrorKind, Write};
use std::path::{Path, PathBuf};
use std::sync::Mutex;
use std::time::{SystemTime, UNIX_EPOCH};

const SECONDS_CAPACITY: usize = 60;
const MINUTES_WINDOW: u64 = 15 * 60;
const MINUTES_RETENTION: u64 = 24 * 60 * 60;
const MAX_FILE_LINES: usize = (MINUTES_RETENTION / MINUTES_WINDOW) as usize * 2;

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
    file_lines: usize,
}

pub struct History {
    path: PathBuf,
    state: Mutex<State>,
}

impl History {
    pub fn load(folder: &Path) -> History {
        delete_legacy_database(folder);

        let path = folder.join("history.jsonl");
        let cutoff = now().saturating_sub(MINUTES_RETENTION);
        let mut minutes = Vec::new();
        let mut lines = 0;

        match File::open(&path) {
            Ok(file) => {
                for line in BufReader::new(file).lines() {
                    let line = match line {
                        Ok(line) => line,
                        Err(e) => {
                            warn!("Failed to read history: {}", e);
                            break;
                        }
                    };

                    if line.trim().is_empty() {
                        continue;
                    }

                    lines += 1;

                    if let Ok(entry) = serde_json::from_str::<Entry>(&line) {
                        if entry.timestamp >= cutoff {
                            minutes.push(entry);
                        }
                    }
                }
            }
            Err(e) if e.kind() == ErrorKind::NotFound => {}
            Err(e) => warn!(
                "Failed to read history, starting with an empty history: {}",
                e
            ),
        }

        let last_window = minutes.last().map(|e| e.timestamp / MINUTES_WINDOW);
        let mut state = State {
            seconds: VecDeque::with_capacity(SECONDS_CAPACITY + 1),
            minutes,
            last_window,
            file_lines: lines,
        };

        // Drop old and unreadable lines, e.g. a half written line from a crash.
        if state.minutes.len() != lines {
            compact(&path, &mut state);
        }

        History {
            path,
            state: Mutex::new(state),
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

            if state.file_lines >= MAX_FILE_LINES {
                compact(&self.path, &mut state);
            } else {
                append(&self.path, &mut state);
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

    // Identifies the newest minutes entry, read it before minutes() so a new entry is never missed.
    pub fn minutes_window(&self) -> Option<u64> {
        self.state.lock().unwrap().last_window
    }

    // Returns the newest minutes entry if one was added after the given window.
    pub fn minutes_since(&self, window: Option<u64>) -> Option<(Option<u64>, HardwareInfo)> {
        let (latest, data) = {
            let state = self.state.lock().unwrap();

            if state.last_window == window {
                return None;
            }

            (state.last_window, state.minutes.last()?.data.clone())
        };

        serde_json::from_value(data).ok().map(|data| (latest, data))
    }

    pub fn latest(&self) -> Option<HardwareInfo> {
        self.state.lock().unwrap().seconds.back().cloned()
    }
}

fn append(path: &Path, state: &mut State) {
    let result = (|| -> std::io::Result<()> {
        let mut file = OpenOptions::new().create(true).append(true).open(path)?;
        file.write_all(&to_line(state.minutes.last().unwrap())?)?;
        file.sync_all()
    })();

    match result {
        Ok(()) => state.file_lines += 1,
        Err(e) => {
            warn!("Failed to save history: {}", e);

            // A failed append can leave a partial line behind, rewrite the whole file next time.
            state.file_lines = MAX_FILE_LINES;
        }
    }
}

// Write to a temp file and swap it in, so a crash mid-write never leaves a half written history.
fn compact(path: &Path, state: &mut State) {
    let temp_path = path.with_extension("jsonl.tmp");

    let result = (|| -> std::io::Result<()> {
        let mut writer = BufWriter::new(File::create(&temp_path)?);

        for entry in &state.minutes {
            writer.write_all(&to_line(entry)?)?;
        }

        writer.flush()?;
        writer.get_ref().sync_all()?;

        std::fs::rename(&temp_path, path)
    })();

    match result {
        Ok(()) => state.file_lines = state.minutes.len(),
        Err(e) => {
            warn!("Failed to save history: {}", e);

            // Retry on the next insert instead of appending to a file that may end with a partial line.
            state.file_lines = MAX_FILE_LINES;
        }
    }
}

fn to_line(entry: &Entry) -> std::io::Result<Vec<u8>> {
    let mut line = serde_json::to_vec(entry)?;
    line.push(b'\n');
    Ok(line)
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
