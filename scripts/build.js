import esbuild from "esbuild";
import esbuildSvelte from "esbuild-svelte";
import sveltePreprocess from "svelte-preprocess";
import postCssPlugin from "esbuild-style-plugin";
import { existsSync, copyFileSync, mkdirSync } from "fs";
import tw from "tailwindcss";
import ap from "autoprefixer";

if (existsSync("./dist/index.html") === false) {
	mkdirSync("./dist");
	copyFileSync("./interface/layout/index.html", "./dist/index.html");
}

esbuild.build({
	entryPoints: ["interface/layout/app.ts"],
	mainFields: ["svelte", "browser", "module", "main"],
	logLevel: "info",
	bundle: true,
	outdir: "./dist",
	format: "esm",
	minify: true,
	plugins: [
		postCssPlugin({
			postcss: {
				plugins: [tw, ap],
			},
		}),
		esbuildSvelte({
			preprocess: sveltePreprocess(),
		}),
	],
});
