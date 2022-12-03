/* eslint-disable no-unused-vars */
/// <reference types="svelte" />

declare global {
	interface Load {
		name: string
		value: number
	}

	interface Temp {
		name?: string
		value: number
		min: number
		max: number
	}

	interface Disk {
		name: string
		temperature: number
		usedSpace: number
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

	interface HardwareInfo {
		cpu: {
			name: string
			temperature: Temp[]
			lastLoad: number
		}

		gpu: {
			name: string
			temperature: Temp[]
			lastLoad: number
			fans: Load[]
		}

		ram: {
			load: Load[]
			modules: RAMModule[]
		}

		os: {
			name: string
		}

		storage: {
			disks: Disk[]
		}

		mb: {
			name: string
		}
	}
}

export {}
