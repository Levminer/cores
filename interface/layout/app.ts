import App from "./app.svelte"
import "../styles/index.css"
import "ui/index.d.ts"

const app = new App({
	target: document.body,
})

export default app
