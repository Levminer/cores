import { writable, get } from "svelte/store"

export const hardwareInfo = writable<HardwareInfo>(sessionStorage.hardwareInfo ? JSON.parse(sessionStorage.hardwareInfo) : {})

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
