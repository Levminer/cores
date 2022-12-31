import { writable, get } from "svelte/store"

const defaultHardwareStatistics: HardwareStatistics = {
	cpu: {
		temperature: {
			value: [],
			min: [],
			max: [],
		},
		power: [],
		load: [],
	},

	ram: {
		usage: {
			physical: [],
			virtual: [],
		},
	},

	gpu: {
		temperature: {
			value: [],
			min: [],
			max: [],
		},
		power: []
	}
}

export const hardwareStatistics = writable<HardwareStatistics>(sessionStorage.hardwareStatistics ? JSON.parse(sessionStorage.hardwareStatistics) : defaultHardwareStatistics)

hardwareStatistics.subscribe((data) => {
	// console.log("HardwareStatistics changed: ", data)

	sessionStorage.setItem("hardwareStatistics", JSON.stringify(data))
})

export const getHardwareStatistics = (): HardwareStatistics => {
	return get(hardwareStatistics)
}

export const setHardwareStatistics = (newState: HardwareStatistics) => {
	hardwareStatistics.set(newState)
}
