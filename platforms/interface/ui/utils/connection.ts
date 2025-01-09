import { getSettings, setSettings } from "../stores/settings.ts"
import { supabaseClient } from "./supabase.ts"

export const deleteConnectionCode = async (code: string) => {
	const settings = getSettings()

	settings.connectionCodes = settings.connectionCodes.filter((item) => item.code !== code)

	setSettings(settings)

	// delete connection
	try {
		const { data: userData, error: userError } = await supabaseClient.auth.getUser()

		if (!userError && userData.user) {
			const res = confirm("Do you want to delete the connection from the cloud?")

			if (res) {
				const { data, error } = await supabaseClient.from("remote_connection").delete().eq("code", code)
			}
		}
	} catch (error) {
		console.log(error)
	}
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

export const addConnectionCode = async () => {
	const settings = getSettings()

	const nameInput = document.getElementById("name") as HTMLInputElement
	const codeInput = document.getElementById("code") as HTMLInputElement

	if (nameInput.value === "") {
		return alert("Please enter a name for your connection")
	}

	if (!codeInput.value.startsWith("crs_")) {
		return alert("Invalid connection code! The connection code must start with: crs_")
	}

	// check if connection code already exists
	const connectionCodeExists = settings.connectionCodes.find((item) => item.code === codeInput.value)
	if (connectionCodeExists) {
		return alert("Connection code already exists! Please enter a unique connection code.")
	}

	settings.connectionCodes = [
		...settings.connectionCodes,
		{
			name: nameInput.value,
			code: codeInput.value,
		},
	]

	setSettings(settings)

	// save connection
	try {
		const { data: userData, error: userError } = await supabaseClient.auth.getUser()

		if (!userError && userData.user) {
			const { data, error } = await supabaseClient.from("remote_connection").insert({
				code: codeInput.value,
				name: nameInput.value,
				user_id: userData.user.id,
			})
		}
	} catch (error) {
		console.log(error)
	}
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
