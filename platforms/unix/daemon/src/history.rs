// Hardware history used by the dashboard charts. The last 60 samples are kept in memory only,
// one sample per 15 minutes is kept for 24 hours and persisted so it survives restarts.
// The file has one JSON entry per line, new entries are appended. Old entries are only removed by
// rewriting the file on startup and once it holds about twice the retention.
// Entries with a timestamp in the future are kept but not sent. The clock may be behind (e.g. right after boot)
// rather than the entries wrong, so they show up once the clock catches up.
// The data is only a cache, so a missing or unreadable file just means starting with an empty history.

use hardwareinfo::HardwareInfo;
use log::warn;
use serde::{Deserialize, Serialize};
use serde_json::value::RawValue;
use std::collections::VecDeque;
use std::fs::{File, OpenOptions};
use std::io::{BufWriter, ErrorKind, Write};
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
    // Kept as raw JSON and sent to clients as is. Values serde can't read back (NaN is stored as null)
    // never drop an entry, and a history written by another version never fails to load as a whole.
    data: Box<RawValue>,
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
        let now = now();
        let mut minutes = Vec::new();
        let mut lines = 0;

        match std::fs::read(&path) {
            // Split the raw bytes, a line cut off by a crash may not even be valid UTF-8
            Ok(bytes) => {
                for line in bytes.split(|b| *b == b'\n') {
                    if line.trim_ascii().is_empty() {
                        continue;
                    }

                    lines += 1;

                    // Skip entries that are not a snapshot object, e.g. from a hand edited file.
                    if let Ok(entry) = serde_json::from_slice::<Entry>(line) {
                        if !is_expired(entry.timestamp, now) && entry.data.get().starts_with('{') {
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

        // Keep the first sample of each window, at most one entry per window even if the clock moves back and forth.
        if state.last_window != Some(window)
            && !state
                .minutes
                .iter()
                .any(|e| e.timestamp / MINUTES_WINDOW == window)
        {
            let value = match serde_json::value::to_raw_value(data) {
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
            state.minutes.retain(|e| !is_expired(e.timestamp, now));

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

    pub fn minutes(&self) -> Vec<Box<RawValue>> {
        let now = now();
        let mut entries: Vec<(u64, Box<RawValue>)> = self
            .state
            .lock()
            .unwrap()
            .minutes
            .iter()
            .filter(|e| !is_expired(e.timestamp, now) && e.timestamp <= now)
            .map(|e| (e.timestamp, e.data.clone()))
            .collect();

        entries.sort_by_key(|(timestamp, _)| *timestamp);
        entries.into_iter().map(|(_, data)| data).collect()
    }

    // Identifies the newest minutes entry, read it before minutes() so a new entry is never missed.
    pub fn minutes_window(&self) -> Option<u64> {
        self.state.lock().unwrap().last_window
    }

    // Returns the newest minutes entry if one was added after the given window.
    pub fn minutes_since(&self, window: Option<u64>) -> Option<(Option<u64>, Box<RawValue>)> {
        let state = self.state.lock().unwrap();

        if state.last_window == window {
            return None;
        }

        Some((state.last_window, state.minutes.last()?.data.clone()))
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

fn is_expired(timestamp: u64, now: u64) -> bool {
    timestamp < now.saturating_sub(MINUTES_RETENTION)
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
