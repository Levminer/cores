import { defineConfig } from "vite"
import { svelte } from "@sveltejs/vite-plugin-svelte"
import sveltePreprocess from "svelte-preprocess"

export default defineConfig({
	plugins: [svelte({ preprocess: sveltePreprocess() })],
	optimizeDeps: { include: ["@carbon/charts"] },
	server: {
		port: 3000,
	},
})
