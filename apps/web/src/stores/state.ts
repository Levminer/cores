import { writable, get } from "svelte/store"

interface State {
	state: "loading" | "waiting" | "connected"
	currentCode: string
}

const defaultState: State = {
	state: "waiting",
	currentCode: "",
}

// Create store
export const state = writable<State>(defaultState)
