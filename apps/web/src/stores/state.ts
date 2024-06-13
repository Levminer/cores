import { writable, get } from "svelte/store"

interface State {
	state: "loading" | "waiting" | "connected" | "disconnected" | "swapping"
	currentCode: string
}

const defaultState: State = {
	state: "waiting",
	currentCode: "",
}

// Create store
export const state = writable<State>(defaultState)

state.subscribe((state) => {
	console.log("State changed", state)
})
