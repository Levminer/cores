/// <reference types="svelte" />
/// <reference types="vite/client" />

declare global {
	interface NetworkMessage {
		type: "data" | "initialData" | "secondsData" | "minutesData" | "initialMinutesData"
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
		priority: number
		id: string
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
		id: string
		priority: number
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
			info: string
			cards: {
				name: string
				temperature: Sensor[]
				fan: Sensor[]
				memory: Sensor[]
				power: Sensor[]
				clock: Sensor[]
				load: Sensor[]
				maxLoad: number
				id: string
				priority: number
			}[]
		}

		ram: {
			load: Sensor[]
			info: RAM[]
			layout: RAM[]
			temperature?: Sensor[]
		}

		system: {
			os: {
				name: string
				app: string
				webView: string
				runtime: string
				hostname?: string
			}

			storage: {
				disks: Disk[]
			}

			motherboard: {
				name: string
			}

			battery?: {
				capacity: Sensor[]
				level: Sensor[]
				remainingTime?: Sensor
				cycleCount: string
			}

			monitor?: {
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

		timestamp?: string
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
			cards: {
				temperature: Sensor
				clock: Sensor
				fan: number
				load: number
				power: number
				memory: number
			}[]
		}

		ram: {
			physicalUsage: number
			virtualUsage: number
			temperature?: Sensor
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

		timestamp?: string
	}

	interface HardwareStatistics {
		seconds: Stats[]
		minutes: Stats[]
	}

	interface LibSettings {
		interval: number
		minimizeToTray: boolean
		connectionCode: string
		connectionCodes: {
			name: string
			code: string
		}[]
		connectionURL?: string
		networkDevices: {
			name: string
			code: string
			mac: string
		}[]
		remoteConnections: boolean
		userId: string
		defaultDevices: {
			gpu: string
			network: string
			storage: string
		}
	}

	interface LibState {
		showMenu: boolean
		updateAvailable: boolean
		plan: string | null
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
