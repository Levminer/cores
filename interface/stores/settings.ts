import { writable, get } from "svelte/store"
import build from "../../build.json"

const defaultSettings: LibSettings = {
	interval: 2,
	minimizeToTray: true,
}

// Create store
export const settings = writable<LibSettings>(sessionStorage.settings ? JSON.parse(sessionStorage.settings) : defaultSettings)

// Listen for store events
settings.subscribe((data) => {
	console.log("Settings changed: ", data)

	sessionStorage.setItem("settings", JSON.stringify(data))
})

export const getSettings = (): LibSettings => {
	return get(settings)
}

export const setSettings = (newSettings: LibSettings) => {
	settings.set(newSettings)
}
