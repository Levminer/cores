import { writable, get } from "svelte/store"
import { invoke } from "@tauri-apps/api/core"

let initialized = false

const generateConnectionCode = () => {
	return `crs_${crypto.randomUUID().replaceAll("-", "")}`.slice(0, 14)
}

const defaultSettings: LibSettings = {
	interval: 2,
	minimizeToTray: true,
	launchOnStartup: false,
	connectionCode: import.meta.env.VITE_CORES_MODE === "host" ? generateConnectionCode() : "",
	connectionCodes: [],
	version: 1,
	remoteConnections: false,
	optionalAnalytics: true,
	licenseKey: "",
	licenseActivated: "",
	userId: import.meta.env.VITE_CORES_MODE === "host" ? generateConnectionCode() : "",
}

// Create store
export const settings = writable<LibSettings>(localStorage.settings ? JSON.parse(localStorage.settings) : defaultSettings)

export const initializeSettings = async () => {
	const storeSettings = (await invoke("get_settings")) as LibSettings
	setSettings(storeSettings)
	initialized = true
}

// Listen for store events
settings.subscribe(async (data) => {
	console.log("Settings changed: ", data)

	if (import.meta.env.VITE_CORES_MODE === "host" && initialized) {
		await invoke("set_settings", { settings: JSON.stringify(data) })

		await fetch("http://localhost:5390/post", {
			method: "POST",
			body: JSON.stringify({
				type: "new_settings",
				data: {
					settings: JSON.stringify(data),
				},
			}),
			headers: {
				"Content-Type": "application/json",
			},
		})
	}

	localStorage.setItem("settings", JSON.stringify(data))
})

export const getSettings = (): LibSettings => {
	return get(settings)
}

export const setSettings = (newSettings: LibSettings) => {
	settings.set(newSettings)
}
