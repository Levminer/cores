using LibreHardwareMonitor.Hardware;

namespace lib;

public class Sensor {
	public string Name {
		get; set;
	}

	public float Value {
		get; set;
	}

	public float Min {
		get; set;
	}

	public float Max {
		get; set;
	}
}

public class NetworkMessage {
	public string Type {
		get; set;
	}
	public API Data {
		get; set;
	}
}

public class Message {
	public string Name {
		get; set;
	}

	public string Content {
		get; set;
	}
}

public class ProtocolData {
	public string FilePath { get; set; }
	public string SystemInfo { get; set; }
	public string Settings { get; set; }
}

public class ProtocolMessage {
	public string Type { get; set; }
	public ProtocolData Data { get; set; }
}

public class Disk {
	public string Name {
		get; set;
	}

	public Identifier Id {
		get; set;
	}

	public Sensor Temperature {
		get; set;
	} = new();

	public int TotalSpace {
		get; set;
	}

	public int FreeSpace {
		get; set;
	}

	public string Health {
		get; set;
	}

	public float ThroughputRead {
		get; set;
	}

	public float ThroughputWrite {
		get; set;
	}

	public float DataRead {
		get; set;
	}

	public float DataWritten {
		get; set;
	}

	public bool SystemDrive {
		get; set;
	}
}

public class Monitor {
	public string Name {
		get; set;
	}

	public string Resolution {
		get; set;
	}

	public string RefreshRate {
		get; set;
	}

	public bool Primary {
		get; set;
	}
}

public class NetInterface {
	public string Name {
		get; set;
	}
	public string Description {
		get; set;
	}
	public string MACAddress {
		get; set;
	}
	public string IPAddress {
		get; set;
	}
	public string Mask {
		get; set;
	}
	public string Gateway {
		get; set;
	}
	public string DNS {
		get; set;
	}
	public string Speed {
		get; set;
	}

	public float UploadData {
		get; set;
	} = new();

	public float DownloadData {
		get; set;
	} = new();

	public float ThroughputUpload {
		get; set;
	} = new();

	public float ThroughputDownload {
		get; set;
	} = new();
}

public class BIOSInfo {
	public string Vendor {
		get; set;
	}

	public string Version {
		get; set;
	}

	public string Date {
		get; set;
	}
}

public class CPUInfo {
	public string Name {
		get; set;
	}

	public List<Sensor> Load {
		get; set;
	} = new();

	public float MaxLoad {
		get; set;
	}

	public List<Sensor> Temperature {
		get; set;
	} = new();

	public List<Sensor> Power {
		get; set;
	} = new();

	public List<Sensor> Clock {
		get; set;
	} = new();

	public List<Sensor> Voltage {
		get; set;
	} = new();

	public List<ProcessorInformation> Info {
		get; set;
	} = new();
}

public class GPUInfo : CPUInfo {
	public List<Sensor> Fan {
		get; set;
	} = new();

	public List<Sensor> Memory {
		get; set;
	} = new();

	new public string Info {
		get; set;
	}
}

public class RAMInfo {
	public List<Sensor> Load {
		get; set;
	} = new();

	public List<MemoryDevice> Info {
		get; set;
	} = new();

	public List<MemoryDevice> Layout {
		get; set;
	} = new();
}

public class OSInfo {
	public string Name {
		get; set;
	}

	public string WebView {
		get; set;
	}

	public string App {
		get; set;
	}

	public string Runtime {
		get; set;
	}
}

public class StorageInfo {
	public List<Disk> Disks {
		get; set;
	} = new();
}

public class SuperIOInfo {
	public string Name {
		get; set;
	}

	public List<Sensor> Voltage {
		get; set;
	} = new();

	public List<Sensor> Temperature {
		get; set;
	} = new();

	public List<Sensor> Fan {
		get; set;
	} = new();

	public List<Sensor> FanControl {
		get; set;
	} = new();
}

public class MotherboardInfo {
	public string Name {
		get; set;
	}
}

public class MonitorInfo {
	public List<Monitor> Monitors {
		get; set;
	} = new();
}

public class NetworkInfo {
	public List<NetInterface> Interfaces {
		get; set;
	} = new();
}

public class SystemAPI {
	public OSInfo OS {
		get; set;
	} = new();

	public StorageInfo Storage {
		get; set;
	} = new();

	public MotherboardInfo Motherboard {
		get; set;
	} = new();

	public MonitorInfo Monitor {
		get; set;
	} = new();

	public NetworkInfo Network {
		get; set;
	} = new();

	public BIOSInfo BIOS {
		get; set;
	} = new();

	public SuperIOInfo SuperIO {
		get; set;
	} = new();
}

public class API {
	public CPUInfo CPU {
		get; set;
	} = new();

	public GPUInfo GPU {
		get; set;
	} = new();

	public RAMInfo RAM {
		get; set;
	} = new();

	public SystemAPI System {
		get; set;
	} = new();
}
