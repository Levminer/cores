import { defineConfig } from "vite"
import { svelte } from "@sveltejs/vite-plugin-svelte"
import { vitePreprocess } from "@sveltejs/vite-plugin-svelte"

export default defineConfig({
	plugins: [
		svelte({
			preprocess: vitePreprocess(),
		}),
	],
	base: "",
	build: {
		rollupOptions: {
			output: {
				entryFileNames: `[name].js`,
				chunkFileNames: `[name].js`,
				assetFileNames: `[name].[ext]`,
			},
		},
		emptyOutDir: true,
		outDir: "../../../dist",
	},
})
