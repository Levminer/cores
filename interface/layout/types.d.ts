/* eslint-disable no-unused-vars */
/// <reference types="svelte" />

declare global {
	interface Load {
		name: string
		value: number
	}

	interface Monitors {
		name: string
		resolution: string
		refreshRate: string
		primary: boolean
	}

	interface Sensor {
		name?: string
		value: number
		min: number
		max: number
	}

	interface Disk {
		name: string
		temperature: number
		usedSpace: number
		size: number
		health: string
	}

	interface RAMModule {
		bankLabel: string
		capacity: number
		formFactor: number
		manufacturer: string
		maxVoltage: number
		minVoltage: number
		partNumber: string
		serialNumber: string
		speed: number
	}

	interface CPUInfo {
		caption: string
		currentClockSpeed: number
		description: string
		l2CacheSize: number
		l3CacheSize: number
		manufacturer: string
		maxClockSpeed: number
		name: string
		numberOfCores: number
		numberOfLogicalProcessors: number
		processorId: string
		secondLevelAddressTranslationExtensions: boolean
		socketDesignation: string
		virtualizationFirmwareEnabled: boolean
		vMMonitorModeExtensions: boolean
		percentProcessorTime: number
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
	}

	interface HardwareInfo {
		cpu: {
			name: string
			temperature: Sensor[]
			lastLoad: number
			power: Sensor[]
			load: Load[]
			info: CPUInfo[]
			clock: Sensor[]
			voltage: Sensor[]
		}

		gpu: {
			name: string
			temperature: Sensor[]
			lastLoad: number
			fans: Load[]
			memory: Load[]
			power: Sensor[]
			load: Load[]
			info: any[]
			clock: Sensor[]
		}

		ram: {
			load: Load[]
			info: RAMModule[]
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
				monitors: Monitors[]
			}

			network: {
				interfaces: NetworkInterface[]
			}

			bios: {
				vendor: string
				version: string
				date: string
			}
		}
	}

	interface HardwareStatistics {
		cpu: {
			temperature: {
				value: number[]
				min: number[]
				max: number[]
			}
			power: number[]
			load: number[]
		}

		ram: {
			usage: {
				physical: number[]
				virtual: number[]
			}
		}

		gpu: {
			temperature: {
				value: number[]
				min: number[]
				max: number[]
			}
			power: number[]
		}
	}
}

export {}
