import App from "./app.svelte"
import "../../ui/styles/index.css"
import "./app.css"
import "../../ui/types.d.ts"

const app = new App({
	target: document.body,
})

export default app
