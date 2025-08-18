import { z } from "zod"
import { writable, get } from "svelte/store"

const localSettingsScheme = z.object({
	newsDate: z.string().default(""),
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
export const localSettings = writable<LibSettings>(
	localStorage.localSettings ? JSON.parse(localStorage.localSettings) : localSettingsScheme.parse({}),
)

// Listen for store events
localSettings.subscribe(async (data) => {
	console.log("Local settings changed: ", data)
	const validatedLocalSettings = localSettingsScheme.parse(data)

	localStorage.setItem("localSettings", JSON.stringify(validatedLocalSettings))
})

export const getLocalSettings = (): LibSettings => {
	return get(localSettings)
}

export const setLocalSettings = (newSettings: LibSettings) => {
	localSettings.set(newSettings)
}
