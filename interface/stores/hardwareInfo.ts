import { writable, get } from "svelte/store"

let defaultSensor = {
	value: 50,
	min: 40,
	max: 60,
}

const defaultHardwareInfo: HardwareInfo = {
	cpu: {
		name: "CPUName",
		lastLoad: 15,
		temperature: [defaultSensor, defaultSensor],
		power: [defaultSensor, defaultSensor],
		load: [],
		info: [],
	},

	gpu: {
		name: "GPUName",
		lastLoad: 15,
		temperature: [defaultSensor, defaultSensor],
		fans: [],
		memory: [],
		power: [defaultSensor],
		load: [],
	},

	ram: {
		load: [
			{
				name: "1",
				value: 10,
			},
			{
				name: "1",
				value: 10,
			},
			{
				name: "1",
				value: 10,
			},
		],
		modules: [],
	},

	system: {
		os: {
			name: "Windows 11",
			app: "app",
			webView: "webview",
			runtime: "runtime",
		},

		storage: {
			disks: [
				{
					name: "Disk",
					temperature: 20,
					usedSpace: 50,
					size: 250,
				},
			],
		},

		motherboard: {
			name: "MBName",
		},

		monitor: {
			monitors: [],
		},
	},
}

export const hardwareInfo = writable<HardwareInfo>(sessionStorage.hardwareInfo ? JSON.parse(sessionStorage.hardwareInfo) : defaultHardwareInfo)

hardwareInfo.subscribe((data) => {
	console.log("HardwareInfo changed: ", data)

	sessionStorage.setItem("hardwareInfo", JSON.stringify(data))
})

export const getHardwareInfo = (): HardwareInfo => {
	return get(hardwareInfo)
}

export const setHardwareInfo = (newState: HardwareInfo) => {
	hardwareInfo.set(newState)
}
