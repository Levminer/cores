import type { RequestHandler } from "./$types"
import { createClient } from "@supabase/supabase-js"
import { env } from "$env/dynamic/public"
import { env as privateEnv } from "$env/dynamic/private"
import type { Database } from "ui"

export const OPTIONS: RequestHandler = async () => {
	return new Response(null, {
		status: 204,
		headers: {
			"Access-Control-Allow-Origin": "*",
			"Access-Control-Allow-Methods": "GET, OPTIONS",
			"Access-Control-Allow-Headers": "Authorization",
		},
	})
}

export const GET: RequestHandler = async ({ url, request, fetch }) => {
	const headers = new Headers()
	headers.set("Access-Control-Allow-Origin", "*")

	const key = url.searchParams.get("license_key")
	const token = request.headers.get("Authorization")?.replace("Bearer ", "")

	if (!key) {
		return Response.json({ error: "Missing license key" }, { status: 400, headers })
	}

	if (!token) {
		return Response.json({ error: "Missing access token" }, { status: 401, headers })
	}

	try {
		const supabaseClient = createClient<Database>(env.PUBLIC_SUPABASE_URL!, privateEnv.SUPABASE_SECRET_KEY!)

		const { data: userData, error: userError } = await supabaseClient.auth.getUser(token)

		if (userError || !userData.user) {
			return Response.json({ error: "Invalid access token" }, { status: 401, headers })
		}

		const userId = userData.user.id

		const response = await fetch("https://api.polar.sh/v1/customer-portal/license-keys/activate", {
			method: "POST",
			headers: { "content-type": "application/json" },
			body: JSON.stringify({ key, label: "Cores-ONLINE", organization_id: privateEnv.POLAR_ORGANIZATION_ID }),
		})
		const data = await response.json()

		if (response.ok) {
			const { error } = await supabaseClient.from("user").update({ plan: "personal" }).eq("user_id", userId)

			if (error) {
				console.error(error)
				return Response.json({ error: "Failed to update plan" }, { status: 500, headers })
			}

			return Response.json({ activated: true, meta: { store_id: 62942 } }, { headers })
		}

		return Response.json(data, { status: response.status, headers })
	} catch (err) {
		console.error(err)
		return Response.json({ error: "Internal server error" }, { status: 500, headers })
	}
}
