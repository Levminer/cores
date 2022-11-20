/* eslint-disable no-unused-vars */
/// <reference types="svelte" />

declare global {
	interface RAM {
		name: string
		value: number
	}

	interface CPUTemp {
		value: number
		min: number
		max: number
	}

	interface HardwareInfo {
		CPUName: string
		CPULoadLast: number
		GPUName: string
		CPUTemp: CPUTemp[]
		RAM: RAM[]
	}
}

export {}
