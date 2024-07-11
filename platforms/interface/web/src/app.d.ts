/// <reference types="vite-plugin-pwa/info" />
/// <reference types="vite-plugin-pwa/client" />
import "ui/index.d.ts"

declare global {
	namespace App {
		// interface Error {}
		// interface Locals {}
		// interface PageData {}
		// interface PageState {}
		// interface Platform {}
	}

	// fix view transition types
	interface ViewTransition {
		updateCallbackDone: Promise<void>
		ready: Promise<void>
		finished: Promise<void>
		skipTransition: () => void
	}

	interface Document {
		startViewTransition(updateCallback: () => Promise<void>): ViewTransition
	}
}

export {}
