import { sveltekit } from "@sveltejs/kit/vite"
import { defineConfig } from "vite"

export default defineConfig({
	plugins: [sveltekit()],
	server: {
		fs: {
			allow: ["../.."],
		},
	},
	ssr: {
		noExternal: ["expo-server-sdk"],
	},
})
