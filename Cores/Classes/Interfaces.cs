using LibreHardwareMonitor.Hardware;
using System.Collections.Generic;

namespace Cores;

public class NameValue {
	public string name {
		get; set;
	}

	public float value {
		get; set;
	}
}

public class CostumSensor {
	public string name {
		get; set;
	}

	public float value {
		get; set;
	}

	public float min {
		get; set;
	}

	public float max {
		get; set;
	}
}

public class Disk {
	public string name {
		get; set;
	}

	public float temperature {
		get; set;
	}

	public float usedSpace {
		get; set;
	}
}

public class CPUAPI {
	public string name {
		get; set;
	}

	public List<ISensor> load = new();

	public float lastLoad {
		get; set;
	}

	public List<CostumSensor> temperature {
		get; set;
	} = new();
}

public class GPUAPI : CPUAPI {
	public List<NameValue> fans {
		get; set;
	} = new();
}

public class RAMAPI {
	public List<NameValue> load {
		get; set;
	} = new();
}

public class OSAPI {
	public string name {
		get; set;
	}
}

public class STORAGEAPI {
	public List<Disk> disks {
		get; set;
	} = new();
}

public class MBAPI {
	public string name {
		get; set;
	}
}

public class API {
	public CPUAPI CPU {
		get; set;
	} = new();

	public GPUAPI GPU {
		get; set;
	} = new();

	public RAMAPI RAM {
		get; set;
	} = new();

	public OSAPI OS {
		get; set;
	} = new();

	public STORAGEAPI STORAGE {
		get; set;
	} = new();

	public MBAPI MB {
		get; set;
	} = new();
}
