import { writable, get } from "svelte/store"

const defaultSettings: LibSettings = {
	interval: 2,
	minimizeToTray: true,
	launchOnStartup: false,
	connectionCode: `crs_${crypto.randomUUID().replaceAll("-", "")}`,
}

// Create store
export const settings = writable<LibSettings>(localStorage.settings ? JSON.parse(localStorage.settings) : defaultSettings)
settings.update((settings) => {
	return {
		...settings,
		mode: import.meta.env.VITE_CORES_MODE,
	}
})

// Listen for store events
settings.subscribe((data) => {
	let prev: LibSettings = localStorage.settings ? JSON.parse(localStorage.settings) : defaultSettings
	console.log("Settings changed: ", data)

	data.mode = import.meta.env.VITE_CORES_MODE

	if (!data.connectionCode) {
		if (!prev.connectionCode) {
			data.connectionCode = `crs_${crypto.randomUUID().replaceAll("-", "")}`
		} else {
			data.connectionCode = prev.connectionCode
		}
	}

	localStorage.setItem("settings", JSON.stringify(data))
})

export const getSettings = (): LibSettings => {
	return get(settings)
}

export const setSettings = (newSettings: LibSettings) => {
	settings.set(newSettings)
}
