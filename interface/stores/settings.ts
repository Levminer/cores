import { writable, get } from "svelte/store"

const defaultSettings: LibSettings = {
	interval: 2,
	minimizeToTray: true,
	launchOnStartup: false,
}

// Create store
export const settings = writable<LibSettings>(sessionStorage.settings ? JSON.parse(sessionStorage.settings) : defaultSettings)
settings.update((settings) => {
	return {
		...settings,
		mode: import.meta.env.VITE_CORES_MODE,
	}
})

// Listen for store events
settings.subscribe((data) => {
	console.log("Settings changed: ", data)

	data.mode = import.meta.env.VITE_CORES_MODE

	sessionStorage.setItem("settings", JSON.stringify(data))
})

export const getSettings = (): LibSettings => {
	return get(settings)
}

export const setSettings = (newSettings: LibSettings) => {
	settings.set(newSettings)
}
