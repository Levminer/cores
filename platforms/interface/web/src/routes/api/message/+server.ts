import { redirect, json } from "@sveltejs/kit"
import type { RequestHandler } from "./$types"

export const GET: RequestHandler = async ({ url }) => {
	return Response.json(
		{
			title: "Cores January 2026 Update",
			message: "Modular home screen layout, Android app and more! Click show more to see all changes.",
			link: "https://github.com/Levminer/cores/releases/tag/0.37.0",
			date: "2026-01-06T15:41:14.481Z",
		},
		{ status: 200 },
	)
}
