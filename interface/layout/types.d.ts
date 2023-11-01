/* eslint-disable no-unused-vars */
/// <reference types="svelte" />

import type { settings } from "stores/settings"

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
		freeSpace: number
		totalSpace: number
		health: string
	}

	interface RAMModule {
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

	interface CPUInfo {
		characteristics: ProcessorCharacteristics
		coreCount: number
		coreEnabled: number
		currentSpeed: number
		externalClock: number
		family: ProcessorFamily
		handle: number
		id: number
		l1CacheHandle: number
		l2CacheHandle: number
		l3CacheHandle: number
		manufacturerName: string
		maxSpeed: number
		processorType: ProcessorType
		serial: string
		socket: ProcessorSocket
		socketDesignation: string
		threadCount: number
		version: string
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
			fan: Sensor[]
			memory: Sensor[]
			power: Sensor[]
			load: Load[]
			info: string
			clock: Sensor[]
		}

		ram: {
			load: Sensor[]
			info: RAMModule[]
			layout: RAMModule[]
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
	}

	interface Message {
		name: string
		content: string
	}
}

export {}
