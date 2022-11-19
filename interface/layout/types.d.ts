/* eslint-disable no-unused-vars */
/// <reference types="svelte" />

declare global {
	interface RAM {
		name: string
		value: string
	}

	interface CPUTemp {
		value: string
		min: string
		max: string
	}

	interface HardwareInfo {
		CPUName: string
		CPULoadLast: string
		GPUName: string
		CPUTemp: CPUTemp[]
		RAM: RAM[]
	}
}

export {}
