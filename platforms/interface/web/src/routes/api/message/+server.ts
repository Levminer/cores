import { redirect, json } from "@sveltejs/kit"
import type { RequestHandler } from "./$types"

export const GET: RequestHandler = async ({ url }) => {
	return Response.json(
		{
			title: "Cores 0.36.0 Updates",
			message: "Improved support for Gigabyte motherboards, improved disk information and many more! Click show more to see all changes.",
			link: "https://github.com/Levminer/cores/releases/tag/0.35.0",
			date: "2025-11-22T15:41:14.481Z",
		},
		{ status: 200 },
	)
}
