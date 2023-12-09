import { writable, get } from "svelte/store"

let defaultSensor = {
	name: "sample",
	value: 50,
	min: 40,
	max: 60,
}

let defaultLoad = {
	name: "sample",
	value: 10,
}

const defaultHardwareInfo: HardwareInfo = {
	cpu: {
		name: "CPUName",
		lastLoad: 15,
		temperature: [defaultSensor, defaultSensor],
		power: [defaultSensor, defaultSensor],
		load: [],
		info: [],
		clock: [defaultSensor, defaultSensor],
		voltage: [defaultSensor, defaultSensor],
	},

	gpu: {
		name: "GPUName",
		lastLoad: 15,
		temperature: [defaultSensor, defaultSensor],
		fan: [],
		memory: [defaultSensor, defaultSensor, defaultSensor, defaultSensor, defaultSensor, defaultSensor],
		power: [defaultSensor],
		load: [],
		info: "",
		clock: [defaultSensor, defaultSensor],
	},

	ram: {
		load: [defaultSensor, defaultSensor, defaultSensor, defaultSensor, defaultSensor, defaultSensor],
		info: [],
		layout: [],
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
					temperature: defaultSensor,
					freeSpace: 50,
					totalSpace: 100,
					health: "N/A",
					throughputRead: 100,
					throughputWrite: 100,
				},
			],
		},

		motherboard: {
			name: "MBName",
		},

		monitor: {
			monitors: [],
		},

		network: {
			interfaces: [],
		},

		bios: {
			version: "1.0",
			date: "2023. 01. 01",
			vendor: "Vendor",
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
