<svelte:head>
	<title>Cores</title>
	<meta name="description" content="Cores - Hardware monitor - Monitor CPU/RAM/GPU usage like clock speed, voltage, memory usage, temperature" />
	<meta property="og:title" content="Cores - Hardware monitor" />
	<meta property="og:image" content="https://cdn.levminer.com/cores/og.webp" />
	<meta property="og:type" content="website" />
	<meta
		property="og:description"
		content="Cores - Hardware monitor - Monitor CPU/RAM/GPU usage like clock speed, voltage, memory usage, temperature"
	/>
	<meta property="og:locale" content="en_US" />
</svelte:head>

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
