<div class="flex h-screen flex-col">
	<div id="layout" class="scroll w-full overflow-hidden overflow-y-scroll">
		<div id="cores" class="top" />

		<div>
			<slot />
		</div>
	</div>
</div>

<svelte:head>
	<title>Cores</title>
	<meta name="description" content="Cores - Hardware monitor - Monitor CPU/RAM/GPU usage like clock speed, voltage, memory usage, temperature" />
	<meta property="og:title" content="Cores - Hardware monitor" />
	<meta property="og:image" content="https://www.levminer.com/og.png" />
	<meta property="og:type" content="website" />
	<meta
		property="og:description"
		content="Cores - Hardware monitor - Monitor CPU/RAM/GPU usage like clock speed, voltage, memory usage, temperature"
	/>
	<meta property="og:locale" content="en_US" />

	{@html webManifestLink}
</svelte:head>

<script lang="ts">
	import "ui/styles/index.css"
	import { onNavigate } from "$app/navigation"
	import { pwaInfo } from "virtual:pwa-info"
	import { onMount } from "svelte"

	$: webManifestLink = pwaInfo ? pwaInfo.webManifest.linkTag : ""

	onMount(async () => {
		if (pwaInfo) {
			const { registerSW } = await import("virtual:pwa-register")
			registerSW({
				immediate: true,
				onRegistered(r) {
					console.log(`SW Registered: ${r}`)
				},
				onRegisterError(error) {
					console.log("SW registration error", error)
				},
			})
		}
	})

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
