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
				icons: [
					{
						src: "maskable_icon_x512",
						sizes: "512x512",
						type: "image/png",
						purpose: "any",
					},
					{
						src: "maskable_icon_x192",
						sizes: "192x192",
						type: "image/png",
						purpose: "any",
					},
				],
			},
		}),
	],
	server: {
		fs: {
			allow: ["../.."],
		},
	},
})
