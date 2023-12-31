import { sveltekit } from "@sveltejs/kit/vite"
import { defineConfig } from "vite"
import { SvelteKitPWA } from "@vite-pwa/sveltekit"

export default defineConfig({
	plugins: [
		sveltekit(),
		SvelteKitPWA({
			manifest: {
				name: "Cores - Hardware Monitor",
				short_name: "Cores",
				description: "Modern hardware monitor for Windows.",
				background_color: "#000000",
				theme_color: "#000000",
				start_url: "/home",
				display: "standalone",
				orientation: "portrait-primary",
				lang: "en-US",
				categories: ["utilities"],
			},
		}),
	],
	server: {
		fs: {
			allow: ["../.."],
		},
	},
})
