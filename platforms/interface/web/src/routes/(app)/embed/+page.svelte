{#if $appState.state === "connected"}
	{#if hash === "#cpu"}
		<Cpu />
	{:else if hash === "#ram"}
		<Ram />
	{:else if hash === "#gpu"}
		<Gpu />
	{:else if hash === "#storage"}
		<Storage />
	{:else if hash === "#network"}
		<Network />
	{:else if hash === "#system"}
		<System />
	{:else}
		<Cpu />
	{/if}
{:else}
	<Loading />
{/if}

<script>
	import { Cpu, Loading, Ram, settings, Gpu, Storage, Network, System } from "ui"
	import { appState } from "../../../stores/state"
	import { onMount } from "svelte"

	let hash = $state(location.hash)

	onMount(() => {
		// get code query parameter
		const urlParams = new URLSearchParams(window.location.search)
		const code = urlParams.get("code")

		console.log(code)

		if (code) {
			$settings.connectionCode = code
			$appState.currentCode = code
		}

		if (hash === "") {
			hash = "#cpu"
		}

		// watch hash change
		window.addEventListener("hashchange", () => {
			hash = location.hash

			console.log(hash)
		})

		console.log(hash)
	})
</script>
