﻿using System.Collections.Generic;
using HI = Hardware.Info;

namespace Cores;

public class Load {
	public string name {
		get; set;
	}

	public float value {
		get; set;
	}
}

public class Sensor {
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

	public int size {
		get; set;
	}
}

public class CPUAPI {
	public string name {
		get; set;
	}

	public List<Load> load {
		get; set;
	} = new();

	public float lastLoad {
		get; set;
	}

	public List<Sensor> temperature {
		get; set;
	} = new();

	public List<Load> power {
		get; set;
	} = new();
}

public class GPUAPI : CPUAPI {
	public List<Load> fans {
		get; set;
	} = new();

	public List<Load> memory {
		get; set;
	} = new();
}

public class RAMAPI {
	public List<Load> load {
		get; set;
	} = new();

	public List<HI.Memory> modules {
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

public class SystemAPI {
	public OSAPI OS {
		get; set;
	} = new();

	public STORAGEAPI Storage {
		get; set;
	} = new();

	public MBAPI Motherboard {
		get; set;
	} = new();
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

	public SystemAPI System {
		get; set;
	} = new();
}
