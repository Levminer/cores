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
	const connection_code = request.headers.get("Authorization")?.replace("Bearer ", "")

	// get title and body from request body
	const { title, body } = await request.json()

	if (!title || !body || !connection_code) {
		return Response.json({ message: "Title, body, and connection code are required" }, { status: 400, headers })
	}

	const supabaseClient = createClient<Database>(env.PUBLIC_SUPABASE_URL!, privateEnv.SUPABASE_SECRET_KEY!)

	const { data: codeData, error: codeError } = await supabaseClient.from("remote_connection").select().eq("code", connection_code).limit(1).single()

	console.log(codeData, codeError)

	if (codeError || !codeData) {
		return Response.json({ message: "Invalid connection code" }, { status: 400, headers })
	}

	const { data, error } = await supabaseClient.from("push_token").select().eq("user_id", codeData.user_id!).single()

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
