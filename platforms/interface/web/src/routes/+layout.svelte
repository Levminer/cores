<div class="flex h-screen flex-col">
	<div id="layout" class="scroll w-full overflow-hidden overflow-y-scroll">
		<div id="cores" class="top" />

		<div>
			<slot />
		</div>
	</div>
</div>

<script lang="ts">
	import "ui/styles/index.css"
	import { onNavigate } from "$app/navigation"
	import posthog from "posthog-js"
	import { browser } from "$app/environment"
	import { beforeNavigate, afterNavigate } from "$app/navigation"

	if (browser) {
		beforeNavigate(() => posthog.capture("$pageleave"))
		afterNavigate(() => posthog.capture("$pageview"))
	}

	onNavigate((navigation) => {
		if (!document.startViewTransition) return

		return new Promise((resolve) => {
			document.startViewTransition(async () => {
				resolve()
				await navigation.complete
				document.querySelector(".top")!.scrollIntoView()
			})
		})
	})
</script>
