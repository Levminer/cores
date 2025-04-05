import { writable, get } from "svelte/store"
import { invoke } from "@tauri-apps/api/core"

let initialized = false

const generateConnectionCode = () => {
	return `crs_${crypto.randomUUID().replaceAll("-", "")}`.slice(0, 16)
}

const generateUserId = () => {
	return `user_${crypto.randomUUID().replaceAll("-", "")}`.slice(0, 10)
}

const defaultSettings: LibSettings = {
	interval: 2,
	minimizeToTray: true,
	launchOnStartup: false,
	connectionCode: import.meta.env.VITE_CORES_MODE === "host" ? generateConnectionCode() : "",
	connectionCodes: [],
	connectionURL: "rtc-usw.coresmonitor.com",
	networkDevices: [],
	version: 1,
	remoteConnections: false,
	optionalAnalytics: true,
	licenseKey: "",
	licenseActivated: "",
	userId: import.meta.env.VITE_CORES_MODE === "host" ? generateUserId() : "",
	colors: {
		min: "#35cbfd",
		current: "#ff5380",
		max: "#9d0cfd",
		yellow: "#fee440",
		orange: "#fe884d",
		categoricalPalette: ["#dc94ff", "#7d70fe", "#2a9d8f"],
	},
}

// Create store
export const settings = writable<LibSettings>(localStorage.settings ? JSON.parse(localStorage.settings) : defaultSettings)

export const initializeSettings = async () => {
	const storedSettings = (await invoke("get_settings")) as LibSettings

	if (get(settings).colors !== undefined) {
		storedSettings.colors = get(settings).colors
	} else {
		storedSettings.colors = {
			min: "#35cbfd",
			current: "#ff5380",
			max: "#9d0cfd",
			yellow: "#fee440",
			orange: "#fe884d",
			categoricalPalette: ["#dc94ff", "#7d70fe", "#2a9d8f"],
		}
	}

	setSettings(storedSettings)
	initialized = true
}

// Listen for store events
settings.subscribe(async (data) => {
	console.log("Settings changed: ", data)

	if (data.networkDevices === undefined) {
		data.networkDevices = []
	}

	if (data.connectionURL === undefined) {
		data.connectionURL = "rtc-usw.coresmonitor.com"
	}

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
