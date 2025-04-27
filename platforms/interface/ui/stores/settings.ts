import { z } from "zod"
import { writable, get } from "svelte/store"
import { invoke } from "@tauri-apps/api/core"

let initialized = false

const generateConnectionCode = () => {
	return `crs_${crypto.randomUUID().replaceAll("-", "")}`.slice(0, 16)
}

const generateUserId = () => {
	return `user_${crypto.randomUUID().replaceAll("-", "")}`.slice(0, 10)
}

const connectionCodesScheme = z.object({
	name: z.string(),
	code: z.string(),
})
const networkDevicesScheme = z.object({
	name: z.string(),
	code: z.string(),
	mac: z.string(),
})

const settingsScheme = z.object({
	interval: z.number().min(1).default(3),
	minimizeToTray: z.boolean().default(true),
	connectionCode: z.string().default(generateConnectionCode()),
	connectionCodes: connectionCodesScheme.array().default([]),
	connectionURL: z.string().default("rtc-usw.coresmonitor.com"),
	networkDevices: networkDevicesScheme.array().default([]),
	remoteConnections: z.boolean().default(false),
	licenseKey: z.string().default(""),
	licenseActivated: z.string().default(""),
	userId: z.string().default(generateUserId()),
	colors: z
		.object({
			min: z.string().default("#35cbfd"),
			current: z.string().default("#ff5380"),
			max: z.string().default("#9d0cfd"),
			yellow: z.string().default("#fee440"),
			orange: z.string().default("#fe884d"),
			categoricalPalette: z.array(z.string()).default(["#dc94ff", "#7d70fe", "#2a9d8f"]),
		})
		.default({}),
	defaultDevices: z
		.object({
			gpu: z.string().default(""),
			network: z.string().default(""),
			storage: z.string().default(""),
		})
		.default({}),
})

// Create store
export const settings = writable<LibSettings>(localStorage.settings ? JSON.parse(localStorage.settings) : settingsScheme.parse({}))

export const initializeSettings = async () => {
	const storedSettings = (await invoke("get_settings")) as LibSettings
	const validatedSettings = settingsScheme.parse(storedSettings)

	setSettings(validatedSettings)
	initialized = true
}

// Listen for store events
settings.subscribe(async (data) => {
	console.log("Settings changed: ", data)
	const validatedSettings = settingsScheme.parse(data)

	if (import.meta.env.VITE_CORES_MODE === "host" && initialized) {
		await invoke("set_settings", { settings: JSON.stringify(validatedSettings) })
		await fetch("http://localhost:5390/post", {
			method: "POST",
			body: JSON.stringify({
				type: "new_settings",
				data: {
					settings: JSON.stringify(validatedSettings),
				},
			}),
			headers: {
				"Content-Type": "application/json",
			},
		})
	}

	localStorage.setItem("settings", JSON.stringify(validatedSettings))
})

export const getSettings = (): LibSettings => {
	return get(settings)
}

export const setSettings = (newSettings: LibSettings) => {
	settings.set(newSettings)
}
