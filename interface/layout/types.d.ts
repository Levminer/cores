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

	interface HardwareInfo {
		cpu: {
			name: string
			temperature: Sensor[]
			lastLoad: number
			power: Sensor[]
			load: Load[]
			info: CPUInfo[]
		}

		gpu: {
			name: string
			temperature: Sensor[]
			lastLoad: number
			fans: Load[]
			memory: Load[]
			power: Sensor[]
			load: Load[]
		}

		ram: {
			load: Load[]
			modules: RAMModule[]
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
		}
	}
}

export {}
