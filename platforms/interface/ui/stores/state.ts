import { writable, get } from "svelte/store"

const defaultState: LibState = {
	showMenu: false,
	updateAvailable: false,
	plan: null,
}

export const state = writable<LibState>(sessionStorage.state ? JSON.parse(sessionStorage.state) : defaultState)
export const appState = state

state.subscribe((data) => {
	console.log("State changed: ", data)

	sessionStorage.setItem("state", JSON.stringify(data))
})

export const getState = (): LibState => {
	return get(state)
}

export const setState = (newState: LibState) => {
	state.set(newState)
}
