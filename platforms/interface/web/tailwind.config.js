/** @type {import('tailwindcss').Config} */
export default {
	content: ["./src/**/*.{html,js,svelte,ts}", "./layout/**/*.{html,js,svelte,ts}", , "../ui/**/*.{html,js,svelte,ts}"],
	theme: {
		extend: {
			colors: {
				gray: {
					100: "#ffffff",
					200: "#d3cfcf",
					500: "#282828",
					600: "#1E1E1E",
					700: "#141414",
					800: "#0a0a0a",
					900: "#000000",
				},
				popup: {
					red: "#CC001B",
					green: "#28A443",
					blue: "#16A3DF",
					yellow: "#F5AB00",
					magenta: "#9B5094",
				},
				cores: {
					min: "#35cbfd",
					current: "#ff5380",
					max: "#9d0cfd",
					alternative: "#d800ef",
				},
				html: {
					gray: "#808080",
				},
			},

			borderRadius: {
				xl: "15px",
				"2xl": "30px",
			},

			screens: {
				sm: { max: "1200px" },
			},
		},
	},
	plugins: [
		require("@tailwindcss/forms")({
			strategy: "class",
		}),
	],
}
