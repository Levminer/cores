import { redirect, json } from "@sveltejs/kit"
import type { RequestHandler } from "./$types"

export const GET: RequestHandler = async ({ url }) => {
	const link = url.searchParams.get("link")

	if (!link) {
		return json({ error: "Failed to get authorization link, please try again later." }, { status: 500 })
	}

	return redirect(302, link)
}
