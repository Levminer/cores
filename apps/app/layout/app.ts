import App from "./app.svelte"
import "ui/styles/index.css"
import "./app.css"
import "ui/index.d.ts"

const app = new App({
	target: document.body,
})

export default app
