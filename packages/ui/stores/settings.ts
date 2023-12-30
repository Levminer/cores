import { writable, get } from "svelte/store"

const generateConnectionCode = () => {
	return `crs_${crypto.randomUUID().replaceAll("-", "")}`.slice(0, 14)
}

const defaultSettings: LibSettings = {
	interval: 2,
	minimizeToTray: true,
	launchOnStartup: false,
	connectionCode: import.meta.env.VITE_CORES_MODE === "host" ? generateConnectionCode() : "",
}

// Create store
export const settings = writable<LibSettings>(localStorage.settings ? JSON.parse(localStorage.settings) : defaultSettings)

// Listen for store events
settings.subscribe((data) => {
	let prev: LibSettings = localStorage.settings ? JSON.parse(localStorage.settings) : defaultSettings
	console.log("Settings changed: ", data)

	// Load default connection code from local storage
	if (!data.connectionCode && import.meta.env.VITE_CORES_MODE === "host") {
		if (!prev.connectionCode && import.meta.env.VITE_CORES_MODE === "host") {
			data.connectionCode = generateConnectionCode()
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
