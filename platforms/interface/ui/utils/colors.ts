import { getSettings } from "../stores/settings"
const settings = getSettings()

export const colors = {
	min: settings.colors?.min ?? "#35cbfd",
	current: settings.colors?.current ?? "#ff5380",
	max: settings.colors?.max ?? "#9d0cfd",
	yellow: settings.colors?.yellow ?? "#fee440",
	orange: settings.colors?.orange ?? "#fe884d",
	categoricalPalette: settings.colors?.categoricalPalette ?? ["#dc94ff", "#7d70fe", "#2a9d8f"],
	divergentPalette: [
		"#35cbfd",
		"#78ceff",
		"#a4d2ff",
		"#c6d6ff",
		"#e0dcff",
		"#f3e3ff",
		"#ffecff",
		"#f8d1ed",
		"#f3b5d7",
		"#ef98bb",
		"#ea7a9b",
		"#e15c77",
		"#d43d51",
	],
} as const
