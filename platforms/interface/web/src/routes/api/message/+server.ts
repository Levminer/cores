import { redirect, json } from "@sveltejs/kit"
import type { RequestHandler } from "./$types"

export const GET: RequestHandler = async ({ url, request }) => {
	const origin = request.headers.get("origin")
	const allowedOrigins = ["http://localhost:3000", "https://tauri.localhost", "http://tauri.localhost"]

	const headers: Record<string, string> = {}

	if (origin && allowedOrigins.includes(origin)) {
		headers["Access-Control-Allow-Origin"] = origin
		headers["Access-Control-Allow-Methods"] = "GET, OPTIONS"
		headers["Access-Control-Allow-Headers"] = "Content-Type"
	}

	return Response.json(
		{
			title: "Cores January 2026 Update",
			message: "Modular home screen layout, Android app and more! Click show more to see all changes.",
			link: "https://github.com/Levminer/cores/releases/tag/0.37.0",
			date: "2026-01-06T15:41:14.481Z",
		},
		{ status: 200, headers },
	)
}
