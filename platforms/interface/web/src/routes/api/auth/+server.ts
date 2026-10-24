import { redirect, json } from "@sveltejs/kit"
import type { RequestHandler } from "./$types"

export const GET: RequestHandler = async ({ url }) => {
	const link = url.searchParams.get("link")

	if (!link) {
		return Response.redirect("cores://oauth-login")
	}

	return redirect(302, link)
}
