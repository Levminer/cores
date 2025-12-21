import posthog from "posthog-js"
import { browser } from "$app/environment"
import { goto } from "$app/navigation"
import { env } from "$env/dynamic/public"

export const ssr = false
export const prerender = true

export const load = async ({ url }) => {
	if (browser) {
		if (url.pathname === "/" && !env.PUBLIC_LOGIN) {
			goto("/home")
		}

		if (env.PUBLIC_POSTHOG_KEY) {
			posthog.init(env.PUBLIC_POSTHOG_KEY, {
				api_host: "https://eu.i.posthog.com",
				capture_pageview: false,
				capture_pageleave: false,
				persistence: "localStorage",
			})
		}
	}
	return
}
