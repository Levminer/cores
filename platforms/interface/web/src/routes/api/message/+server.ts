import type { RequestHandler } from "./$types"

export const GET: RequestHandler = async ({ url, request }) => {
	const headers = new Headers()
	headers.set("Access-Control-Allow-Origin", "*")

	return Response.json(
		{
			title: "Cores September 2026 Update",
			message: "Various stability improvements across Windows and Linux. Click show more to see all changes.",
			link: "https://github.com/Levminer/cores/releases/tag/0.42.0",
			date: "2026-07-11T15:41:14.481Z",
		},
		{ status: 200, headers },
	)
}
