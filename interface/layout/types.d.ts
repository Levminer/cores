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

	interface HardwareInfo {
		CPU: {
			name: string
			temperature: Temp[]
			lastLoad: number
		}

		GPU: {
			name: string
			temperature: Temp[]
			lastLoad: number
		}

		RAM: {
			load: Load[]
		}
	}
}

export {}
