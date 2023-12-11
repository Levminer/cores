import { defineConfig } from "vite"
import { svelte } from "@sveltejs/vite-plugin-svelte"
import sveltePreprocess from "svelte-preprocess"
import tsconfigPaths from "vite-tsconfig-paths"

export default defineConfig({
	plugins: [
		svelte({
			preprocess: sveltePreprocess(),
			onwarn: (warning, handler) => {
				if (warning.code.startsWith("a11y-")) return
				handler(warning)
			},
		}),
		tsconfigPaths(),
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
		outDir: "../core/Assets/assets",
	},
})
