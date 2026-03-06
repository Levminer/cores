import { redirect, json } from "@sveltejs/kit"
import type { RequestHandler } from "./$types"

export const GET: RequestHandler = async ({ url, request }) => {
	const headers = new Headers()
	headers.set("Access-Control-Allow-Origin", "*")

	return Response.json(
		{
			title: "Cores March 2026 Update",
			message: "Disk usage improvements and better Anti-cheat compatibility. Click show more to see all changes.",
			link: "https://github.com/Levminer/cores/releases/tag/0.39.0",
			date: "2026-01-06T15:41:14.481Z",
		},
		{ status: 200, headers },
	)
}
