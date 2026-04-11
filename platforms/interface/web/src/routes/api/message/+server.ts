import { redirect, json } from "@sveltejs/kit"
import type { RequestHandler } from "./$types"

export const GET: RequestHandler = async ({ url, request }) => {
	const headers = new Headers()
	headers.set("Access-Control-Allow-Origin", "*")

	return Response.json(
		{
			title: "Cores April 2026 Update",
			message: "Edit home page layout on web/mobile and various bug fixes. Click show more to see all changes.",
			link: "https://github.com/Levminer/cores/releases/tag/0.40.0",
			date: "2026-04-11T15:41:14.481Z",
		},
		{ status: 200, headers },
	)
}
