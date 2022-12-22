using System.Collections.Generic;
using HI = Hardware.Info;

namespace Cores;

public class Load {
	public string Name {
		get; set;
	}

	public float Value {
		get; set;
	}
}

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

public class Disk {
	public string Name {
		get; set;
	}

	public float Temperature {
		get; set;
	}

	public float UsedSpace {
		get; set;
	}

	public int Size {
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
}

public class CPUInfo {
	public string Name {
		get; set;
	}

	public List<Load> Load {
		get; set;
	} = new();

	public float LastLoad {
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

	public List<HI.CPU> Info {
		get; set;
	} = new();
}

public class GPUInfo : CPUInfo {
	public List<Load> Fans {
		get; set;
	} = new();

	public List<Load> Memory {
		get; set;
	} = new();

	new public List<HI.VideoController> Info {
		get; set;
	} = new();
}

public class RAMInfo {
	public List<Load> Load {
		get; set;
	} = new();

	public List<HI.Memory> Info {
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
