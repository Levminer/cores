import posthog from "posthog-js"
import { browser } from "$app/environment"
import { PUBLIC_CONNECTION_URL } from "$env/static/public"

export const ssr = false
export const prerender = true

if (!PUBLIC_CONNECTION_URL) {
	throw new Error("PUBLIC_CONNECTION_URL is not defined. Please set it in your environment variables.")
}

export const load = async () => {
	if (browser) {
		posthog.init("phc_9ZC5KfIjWVF5oRgeILwKwR0LKam8TlC0iGlNa6RYR9F", {
			api_host: "https://eu.i.posthog.com",
			capture_pageview: false,
			capture_pageleave: false,
			persistence: "localStorage",
		})
	}
	return
}
