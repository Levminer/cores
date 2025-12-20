import { env } from "$env/dynamic/public"
import { getSettings, setSettings } from "ui"

if (env.PUBLIC_CONNECTION_SERVER_URL) {
	const settings = getSettings()
	settings.connectionURL = env.PUBLIC_CONNECTION_SERVER_URL
	setSettings(settings)
}
