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
		usedSpace: number
		size: number
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
				seconds: {
					value: number[]
					min: number[]
					max: number[]
				}

				minutes: {
					value: number[]
					min: number[]
					max: number[]
				}
			}
			power: {
				seconds: number[]
				minutes: number[]
			}
			load: {
				seconds: number[]
				minutes: number[]
			}
			voltage: {
				seconds: number[]
				minutes: number[]
			}
			clock: {
				seconds: {
					value: number[]
					min: number[]
					max: number[]
				}

				minutes: {
					value: number[]
					min: number[]
					max: number[]
				}
			}
		}

		ram: {
			physicalUsage: {
				seconds: number[]
				minutes: number[]
			}

			virtualUsage: {
				seconds: number[]
				minutes: number[]
			}
		}

		gpu: {
			temperature: {
				seconds: {
					value: number[]
					min: number[]
					max: number[]
				}

				minutes: {
					value: number[]
					min: number[]
					max: number[]
				}
			}
			power: {
				seconds: number[]
				minutes: number[]
			}
		}
	}

	interface LibSettings {
		interval: number
	}
}

export {}
