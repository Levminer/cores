import { getSettings, setSettings } from "../stores/settings.ts"

export const deleteConnectionCode = (code: string) => {
	const settings = getSettings()

	settings.connectionCodes = settings.connectionCodes.filter((item) => item.code !== code)

	setSettings(settings)
}

export const editConnectionCode = (code: string) => {
	const settings = getSettings()

	const nameInput = document.getElementById("name") as HTMLInputElement
	const codeInput = document.getElementById("code") as HTMLInputElement

	if (nameInput.value === "") {
		return alert("Please enter a name for your connection")
	}

	if (!codeInput.value.startsWith("crs_")) {
		return alert("Invalid connection code! The connection code must start with: crs_")
	}

	let id = settings.connectionCodes.findIndex((item) => item.code === code)
	settings.connectionCodes[id] = {
		name: nameInput.value,
		code: codeInput.value,
	}

	setSettings(settings)
}

export const addConnectionCode = () => {
	const settings = getSettings()

	const nameInput = document.getElementById("name") as HTMLInputElement
	const codeInput = document.getElementById("code") as HTMLInputElement

	if (nameInput.value === "") {
		return alert("Please enter a name for your connection")
	}

	if (!codeInput.value.startsWith("crs_")) {
		return alert("Invalid connection code! The connection code must start with: crs_")
	}

	settings.connectionCodes = [
		...settings.connectionCodes,
		{
			name: nameInput.value,
			code: codeInput.value,
		},
	]

	setSettings(settings)
}

export const addDevice = () => {
	const settings = getSettings()

	const nameInput = document.getElementById("name") as HTMLInputElement
	const macInput = document.getElementById("mac") as HTMLInputElement

	if (nameInput.value === "") {
		return alert("Please enter a name for your connection")
	}

	if (!macInput.value.includes(":")) {
		return alert("Invalid MAC address! The MAC address must be in the format: AA:BB:CC:DD:EE:FF")
	}

	settings.networkDevices = [
		...settings.networkDevices,
		{
			name: nameInput.value,
			code: settings.connectionCode,
			mac: macInput.value,
		},
	]

	setSettings(settings)
}

export const deleteDevice = (mac: string) => {
	const settings = getSettings()

	settings.networkDevices = settings.networkDevices.filter((item) => item.mac !== mac)

	setSettings(settings)
}
