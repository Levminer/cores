use anyhow::{Context, Result};
use regex::Regex;
use std::{
    collections::HashMap,
    fmt::Display,
    path::{Path, PathBuf},
    sync::LazyLock,
};

const SYS_STATS: &str = r" *(?P<read_ios>[0-9]*) *(?P<read_merges>[0-9]*) *(?P<read_sectors>[0-9]*) *(?P<read_ticks>[0-9]*) *(?P<write_ios>[0-9]*) *(?P<write_merges>[0-9]*) *(?P<write_sectors>[0-9]*) *(?P<write_ticks>[0-9]*) *(?P<in_flight>[0-9]*) *(?P<io_ticks>[0-9]*) *(?P<time_in_queue>[0-9]*) *(?P<discard_ios>[0-9]*) *(?P<discard_merges>[0-9]*) *(?P<discard_sectors>[0-9]*) *(?P<discard_ticks>[0-9]*) *(?P<flush_ios>[0-9]*) *(?P<flush_ticks>[0-9]*)";

static RE_DRIVE: LazyLock<Regex> = LazyLock::new(|| Regex::new(SYS_STATS).unwrap());

#[derive(Debug)]
pub struct DriveData {
    pub inner: Drive,
    pub is_virtual: bool,
    pub writable: Result<bool>,
    pub removable: Result<bool>,
    pub disk_stats: HashMap<String, usize>,
    pub capacity: Result<u64>,
}

impl DriveData {
    pub fn new(path: &Path) -> Self {
        let inner = Drive::from_sysfs(path);
        let is_virtual = inner.is_virtual();
        let writable = inner.writable();
        let removable = inner.removable();
        let disk_stats = inner.sys_stats().unwrap_or_default();
        let capacity = inner.capacity();

        Self {
            inner,
            is_virtual,
            writable,
            removable,
            disk_stats,
            capacity,
        }
    }
}

#[derive(Debug, Clone, Copy, Default, PartialEq, Eq, Hash)]
pub enum DriveType {
    CdDvdBluray,
    Emmc,
    Flash,
    Floppy,
    Hdd,
    LoopDevice,
    MappedDevice,
    Nvme,
    Raid,
    RamDisk,
    Ssd,
    ZfsVolume,
    Zram,
    #[default]
    Unknown,
}

#[derive(Debug, Clone, Default, Eq)]
pub struct Drive {
    pub model: Option<String>,
    pub drive_type: DriveType,
    pub block_device: String,
    pub sysfs_path: PathBuf,
}

impl Display for DriveType {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        write!(
            f,
            "{}",
            match self {
                DriveType::CdDvdBluray => "CD/DVD/Blu-ray Drive",
                DriveType::Emmc => "eMMC Storage",
                DriveType::Flash => "Flash Storage",
                DriveType::Floppy => "Floppy Drive",
                DriveType::Hdd => "Hard Disk Drive",
                DriveType::LoopDevice => "Loop Device",
                DriveType::MappedDevice => "Mapped Device",
                DriveType::Nvme => "NVMe Drive",
                DriveType::Unknown => "N/A",
                DriveType::Raid => "Software Raid",
                DriveType::RamDisk => "RAM Disk",
                DriveType::Ssd => "Solid State Drive",
                DriveType::ZfsVolume => "ZFS Volume",
                DriveType::Zram => "Compressed RAM Disk (zram)",
            }
        )
    }
}

impl PartialEq for Drive {
    fn eq(&self, other: &Self) -> bool {
        self.block_device == other.block_device
    }
}

impl Drive {
    /// Creates a `Drive` using a SysFS Path
    ///
    /// # Errors
    ///
    /// Will return `Err` if the are errors during
    /// reading or parsing
    pub fn from_sysfs<P: AsRef<Path>>(sysfs_path: P) -> Drive {
        let path = sysfs_path.as_ref().to_path_buf();
        let block_device = path
            .file_name()
            .expect("sysfs path ends with \"..\"?")
            .to_string_lossy()
            .to_string();

        let mut drive = Self::default();
        drive.sysfs_path = path;
        drive.block_device = block_device;
        drive.model = drive.model().ok().map(|model| model.trim().to_string());
        drive.drive_type = drive.drive_type().unwrap_or_default();
        drive
    }

    /// Returns the SysFS Paths of possible drives
    ///
    /// # Errors
    ///
    /// Will return `Err` if the are errors during
    /// reading or parsing
    pub fn get_sysfs_paths() -> Result<Vec<PathBuf>> {
        let mut list = Vec::new();
        let entries = std::fs::read_dir("/sys/block")?;
        for entry in entries {
            let entry = entry?;
            let block_device = entry.file_name().to_string_lossy().to_string();
            if block_device.is_empty() {
                continue;
            }
            list.push(entry.path());
        }
        Ok(list)
    }

    pub fn display_name(&self) -> String {
        return String::from("N/A");
    }

    /// Returns the current SysFS stats for the drive
    ///
    /// # Errors
    ///
    /// Will return `Err` if the are errors during
    /// reading or parsing
    pub fn sys_stats(&self) -> Result<HashMap<String, usize>> {
        let stat = std::fs::read_to_string(self.sysfs_path.join("stat"))
            .with_context(|| format!("unable to read /sys/block/{}/stat", self.block_device))?;

        let captures = RE_DRIVE
            .captures(&stat)
            .with_context(|| format!("unable to parse /sys/block/{}/stat", self.block_device))?;

        Ok(RE_DRIVE
            .capture_names()
            .flatten()
            .filter_map(|named_capture| {
                Some((
                    named_capture.to_string(),
                    captures.name(named_capture)?.as_str().parse().ok()?,
                ))
            })
            .collect())
    }

    fn drive_type(&self) -> Result<DriveType> {
        if self.block_device.starts_with("nvme") {
            Ok(DriveType::Nvme)
        } else if self.block_device.starts_with("mmc") {
            Ok(DriveType::Emmc)
        } else if self.block_device.starts_with("fd") {
            Ok(DriveType::Floppy)
        } else if self.block_device.starts_with("sr") {
            Ok(DriveType::CdDvdBluray)
        } else if self.block_device.starts_with("zram") {
            Ok(DriveType::Zram)
        } else if self.block_device.starts_with("md") {
            Ok(DriveType::Raid)
        } else if self.block_device.starts_with("loop") {
            Ok(DriveType::LoopDevice)
        } else if self.block_device.starts_with("dm") {
            Ok(DriveType::MappedDevice)
        } else if self.block_device.starts_with("ram") {
            Ok(DriveType::RamDisk)
        } else if self.block_device.starts_with("zd") {
            Ok(DriveType::ZfsVolume)
        } else if let Ok(rotational) =
            std::fs::read_to_string(self.sysfs_path.join("queue/rotational"))
        {
            // turn rot into a boolean
            let rotational = rotational
                .replace('\n', "")
                .parse::<u8>()
                .map(|rot| rot != 0)?;
            if rotational {
                Ok(DriveType::Hdd)
            } else if self.removable()? {
                Ok(DriveType::Flash)
            } else {
                Ok(DriveType::Ssd)
            }
        } else {
            Ok(DriveType::Unknown)
        }
    }

    /// Returns, whether the drive is removable
    ///
    /// # Errors
    ///
    /// Will return `Err` if the are errors during
    /// reading or parsing
    pub fn removable(&self) -> Result<bool> {
        std::fs::read_to_string(self.sysfs_path.join("removable"))?
            .replace('\n', "")
            .parse::<u8>()
            .map(|rem| rem != 0)
            .context("unable to parse removable sysfs file")
    }

    /// Returns, whether the drive is writable
    ///
    /// # Errors
    ///
    /// Will return `Err` if the are errors during
    /// reading or parsing
    pub fn writable(&self) -> Result<bool> {
        std::fs::read_to_string(self.sysfs_path.join("ro"))?
            .replace('\n', "")
            .parse::<u8>()
            .map(|ro| ro == 0)
            .context("unable to parse ro sysfs file")
    }

    /// Returns the capacity of the drive **in bytes**
    ///
    /// # Errors
    ///
    /// Will return `Err` if the are errors during
    /// reading or parsing
    pub fn capacity(&self) -> Result<u64> {
        std::fs::read_to_string(self.sysfs_path.join("size"))?
            .replace('\n', "")
            .parse::<u64>()
            .map(|sectors| sectors * 512)
            .context("unable to parse size sysfs file")
    }

    /// Returns the model information of the drive
    ///
    /// # Errors
    ///
    /// Will return `Err` if the are errors during
    /// reading or parsing
    pub fn model(&self) -> Result<String> {
        std::fs::read_to_string(self.sysfs_path.join("device/model"))
            .context("unable to parse model sysfs file")
    }

    /// Returns the World-Wide Identification of the drive
    ///
    /// # Errors
    ///
    /// Will return `Err` if the are errors during
    /// reading or parsing
    pub fn wwid(&self) -> Result<String> {
        std::fs::read_to_string(self.sysfs_path.join("device/wwid"))
            .context("unable to parse wwid sysfs file")
    }

    pub fn is_virtual(&self) -> bool {
        match self.drive_type {
            DriveType::LoopDevice
            | DriveType::MappedDevice
            | DriveType::Raid
            | DriveType::RamDisk
            | DriveType::ZfsVolume
            | DriveType::Zram => true,
            _ => self.capacity().unwrap_or(0) == 0,
        }
    }
}
