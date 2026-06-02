import type { RequestHandler } from "./$types"

export const GET: RequestHandler = async ({ url, request }) => {
	const headers = new Headers()
	headers.set("Access-Control-Allow-Origin", "*")

	return Response.json(
		{
			title: "Cores June 2026 Update",
			message: "Hide tiles on the home screen, notifications about hardware events and more. Click show more to see all changes.",
			link: "https://github.com/Levminer/cores/releases/tag/0.41.0",
			date: "2026-04-11T15:41:14.481Z",
		},
		{ status: 200, headers },
	)
}
