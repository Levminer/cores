import App from "./app.svelte"
import "../../ui/styles/index.css"
import "./app.css"
import "../../ui/types.d.ts"
import { mount } from "svelte"

const app = mount(App, {
	target: document.body,
})

export default app
