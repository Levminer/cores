import type { RequestHandler } from "./$types"
import { Expo, type ExpoPushMessage } from "expo-server-sdk"
import { createClient } from "@supabase/supabase-js"
import { env } from "$env/dynamic/public"
import type { Database } from "ui"

export const POST: RequestHandler = async ({ url, request }) => {
	const headers = new Headers()
	headers.set("Access-Control-Allow-Origin", "*")

	// get Authorization header
	const jwt = request.headers.get("Authorization")?.replace("Bearer ", "")

	// get title and body from request body
	const { title, body } = await request.json()

	if (!title || !body) {
		return Response.json({ message: "Title and body are required" }, { status: 400, headers })
	}

	let supabaseClient

	if (jwt) {
		supabaseClient = createClient<Database>(env.PUBLIC_SUPABASE_URL!, env.PUBLIC_SUPABASE_ANON_KEY!, {
			accessToken: async () => {
				return jwt
			},
		})
	} else {
		return Response.json({ message: "Unauthorized" }, { status: 401, headers })
	}

	const { data, error } = await supabaseClient.from("push_token").select().single()
	const expo = new Expo()
	const messages: ExpoPushMessage[] = []

	if (error) {
		return Response.json({ error: error.message }, { status: 500, headers })
	}

	if (data) {
		if (!data.user_id) {
			return Response.json({ error: "Missing user id for push token" }, { status: 500, headers })
		}

		if (!data.token || !Expo.isExpoPushToken(data.token)) {
			return Response.json({ error: "Invalid push token" }, { status: 500, headers })
		}

		messages.push({
			to: data.token,
			title: title,
			body: body,
		} as ExpoPushMessage)

		const { data: sendData, error: sendError } = await supabaseClient.from("notification").insert({
			body,
			title,
			user_id: data.user_id,
		})

		console.log(sendData, sendError)

		const receipt = await expo.sendPushNotificationsAsync(messages)
		console.log(receipt)
	}

	return Response.json(
		{
			message: "ok",
		},
		{ status: 200, headers },
	)
}
