import { writable, get } from "svelte/store"

interface State {
	state: "loading" | "waiting" | "connected" | "disconnected" | "swapping"
	currentCode: string
	message: string
}

const defaultState: State = {
	state: "waiting",
	currentCode: "",
	message: "",
}

// Create store
export const state = writable<State>(defaultState)

state.subscribe((state) => {
	console.log("State changed", state)
})
