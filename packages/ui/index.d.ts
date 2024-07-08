/// <reference types="svelte" />
/// <reference types="vite/client" />

declare global {
	interface NetworkMessage {
		type: "data" | "initialData" | "secondsData" | "minutesData"
		data: HardwareInfo
	}

	interface Sensor {
		name?: string
		value: number
		min: number
		max: number
	}

	interface Disk {
		name: string
		temperature: Sensor
		freeSpace: number
		totalSpace: number
		health: string
		throughputRead: number
		throughputWrite: number
		dataRead: number
		dataWritten: number
		systemDrive: boolean
	}

	interface Monitor {
		name: string
		resolution: string
		refreshRate: string
		primary: boolean
	}

	interface NetworkInterface {
		name: string
		description: string
		macAddress: string
		ipAddress: string
		mask: string
		gateway: string
		dns: string
		speed: string
		throughputDownload: number
		throughputUpload: number
		downloadData: number
		uploadData: number
	}

	interface RAM {
		bankLocator: string
		deviceLocator: string
		manufacturerName: string
		partNumber: string
		serialNumber: string
		size: number
		speed: number
		configuredSpeed: number
		configuredVoltage: number
		type: number
	}

	interface CPU {
		coreCount: number
		coreEnabled: number
		currentSpeed: number
		externalClock: number
		handle: number
		id: number
		l1CacheHandle: number
		l2CacheHandle: number
		l3CacheHandle: number
		manufacturerName: string
		maxSpeed: number
		serial: string
		socketDesignation: string
		threadCount: number
		version: string
	}

	interface HardwareInfo {
		cpu: {
			name: string
			temperature: Sensor[]
			maxLoad: number
			power: Sensor[]
			load: Sensor[]
			info: CPU[]
			clock: Sensor[]
			voltage: Sensor[]
		}

		gpu: {
			name: string
			temperature: Sensor[]
			maxLoad: number
			fan: Sensor[]
			memory: Sensor[]
			power: Sensor[]
			load: Sensor[]
			info: string
			clock: Sensor[]
		}

		ram: {
			load: Sensor[]
			info: RAM[]
			layout: RAM[]
		}

		system: {
			os: {
				name: string
				app: string
				webView: string
				runtime: string
			}

			storage: {
				disks: Disk[]
			}

			motherboard: {
				name: string
			}

			monitor: {
				monitors: Monitor[]
			}

			network: {
				interfaces: NetworkInterface[]
			}

			bios: {
				vendor: string
				version: string
				date: string
			}

			superIO: {
				name: string
				fan: Sensor[]
				fanControl: Sensor[]
				voltage: Sensor[]
				temperature: Sensor[]
			}
		}
	}

	interface Stats {
		cpu: {
			temperature: Sensor
			clock: Sensor
			load: number
			power: number
			voltage: number
		}

		gpu: {
			temperature: Sensor
			clock: Sensor
			fan: number
			load: number
			power: number
			memory: number
		}

		ram: {
			physicalUsage: number
			virtualUsage: number
		}

		network: {
			throughputDownload: number
			throughputUpload: number
			downloadedData: number
			uploadedData: number
		}[]

		storage: {
			throughputRead: number
			throughputWrite: number
			temperature: Sensor
		}[]

		fan: {
			speed: Sensor
			control: Sensor
		}[]
	}

	interface HardwareStatistics {
		seconds: Stats[]
		minutes: Stats[]
	}

	interface LibSettings {
		interval: number
		minimizeToTray: boolean
		launchOnStartup: boolean
		connectionCode: string
		connectionCodes: {
			name: string
			code: string
		}[]
		version: number
		remoteConnections: boolean
		optionalAnalytics: boolean
		licenseKey: string
		licenseActivated: string
		userId: string
	}

	interface SystemInfo {
		tauriVersion: string
		osName: string
		osVersion: string
		osArch: string
		cpuName: string
		totalMem: number
		gpuName: string
	}
}

export {}
