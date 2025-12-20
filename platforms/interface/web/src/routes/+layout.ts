import posthog from "posthog-js"
import { browser } from "$app/environment"
import { goto } from "$app/navigation"
import { env } from "$env/dynamic/public"
import { getSettings, setSettings } from "ui"

export const ssr = false
export const prerender = true

export const load = async ({ url }) => {
	if (browser) {
		if (url.pathname === "/" && !import.meta.env.VITE_LOGIN) {
			goto("/home")
		}

		if (import.meta.env.VITE_POSTHOG_KEY) {
			posthog.init(import.meta.env.VITE_POSTHOG_KEY, {
				api_host: "https://eu.i.posthog.com",
				capture_pageview: false,
				capture_pageleave: false,
				persistence: "localStorage",
			})
		}
	}
	return
}
