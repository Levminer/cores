import type { RequestHandler } from "./$types"
import { env } from "$env/dynamic/private"

export const GET: RequestHandler = async ({ request }) => {
	const response = await fetch(`https://rtc.live.cloudflare.com/v1/turn/keys/${env.TURN_KEY}/credentials/generate`, {
		method: "POST",
		headers: {
			Authorization: `Bearer ${env.TURN_TOKEN}`,
			"Content-Type": "application/json",
		},
		body: JSON.stringify({ ttl: 86400 }),
	})

	if (!response.ok) {
		console.log(response)
		return Response.json({ error: "Failed to fetch TURN servers" }, { status: 500 })
	}

	const data = await response.json()
	console.log(data)

	const turnServers = []

	for (let i = 0; i < data.iceServers.urls.length; i++) {
		turnServers.push({
			urls: [data.iceServers.urls[i]],
			username: data.iceServers.username,
			credential: data.iceServers.credential,
		})
	}

	turnServers.shift()

	return Response.json(turnServers)
}
