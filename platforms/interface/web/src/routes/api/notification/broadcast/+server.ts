import type { RequestHandler } from "./$types"
import { Expo, type ExpoPushMessage } from "expo-server-sdk"
import { createClient } from "@supabase/supabase-js"
import { env } from "$env/dynamic/public"
import { env as privateEnv } from "$env/dynamic/private"
import type { Database } from "ui"

export const POST: RequestHandler = async ({ url, request }) => {
	const headers = new Headers()
	headers.set("Access-Control-Allow-Origin", "*")

	// get Authorization header
	const apiKey = request.headers.get("Authorization")?.replace("Bearer ", "")

	// get title and body from request body
	const { title, body } = await request.json()

	if (!title || !body) {
		return Response.json({ message: "Title and body are required" }, { status: 400, headers })
	}

	let supabaseClient

	if (apiKey == privateEnv.API_KEY) {
		supabaseClient = createClient<Database>(env.PUBLIC_SUPABASE_URL!, privateEnv.SUPABASE_SECRET_KEY!)
	} else {
		return Response.json({ message: "Unauthorized" }, { status: 401, headers })
	}

	const { data, error } = await supabaseClient.from("push_token").select()
	const expo = new Expo()
	const messages: ExpoPushMessage[] = []

	if (error) {
		return Response.json({ error: error.message }, { status: 500, headers })
	}

	if (data && data.length > 0) {
		console.log("message count ", data.length)

		for (let i = 0; i < data.length; i++) {
			if (!Expo.isExpoPushToken(data[i].token)) {
				console.error(`Push token ${data[i].token} is not a valid Expo push token`)
				continue
			}

			messages.push({
				to: data[i].token,
				title: title,
				body: body,
				data: {
					user_id: data[i].user_id,
				},
			} as ExpoPushMessage)
		}

		// batch insert notifications for all users with valid push tokens
		const notificationsToInsert = messages
			.filter((message) => message.data?.user_id)
			.map((message) => ({
				user_id: message.data?.user_id as string,
				title: message.title || "",
				body: message.body || "",
			}))

		if (notificationsToInsert.length > 0) {
			const { error: insertError } = await supabaseClient.from("notification").upsert(notificationsToInsert).select()
			if (insertError) {
				console.error("Failed to insert notifications:", insertError)
			}
		}

		const chunks = expo.chunkPushNotifications(messages)

		await Promise.all(
			chunks.map(async (chunk) => {
				const receipt = await expo.sendPushNotificationsAsync(chunk)
				console.log(receipt)
			}),
		)
	}

	return Response.json(
		{
			message: "ok",
		},
		{ status: 200, headers },
	)
}
