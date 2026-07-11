import type { RequestHandler } from "./$types"

export const GET: RequestHandler = async ({ url, request }) => {
	const headers = new Headers()
	headers.set("Access-Control-Allow-Origin", "*")

	return Response.json(
		{
			title: "Cores July 2026 Update",
			message: "Improvements on macOS and better disk usage on Windows. Click show more to see all changes.",
			link: "https://github.com/Levminer/cores/releases/tag/0.42.0",
			date: "2026-07-11T15:41:14.481Z",
		},
		{ status: 200, headers },
	)
}
