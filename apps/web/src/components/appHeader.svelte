<nav class="start-0 top-0 z-20 w-full border-b border-gray-600 bg-gray-900 px-8 sm:px-0">
	<div class="mx-auto flex flex-wrap items-center justify-between p-3">
		<a href="/home" class="flex items-center space-x-3">
			<img class="h-8 w-8" src={"/favicon.ico"} alt="Cores logo" />
			<span class="self-center whitespace-nowrap text-2xl font-semibold text-white">Cores</span>
		</a>
		<div class="flex space-x-2 md:order-2">
			{#if $state.state === "connected"}
				<PowerDropdown />
			{/if}
			<ConnectionDropdown {connect} />
		</div>
	</div>
</nav>

<script lang="ts">
	import { goto } from "$app/navigation"
	import ConnectionDropdown from "ui/components/connectionDropdown.svelte"
	import PowerDropdown from "ui/components/powerDropdown.svelte"
	import { settings } from "ui/stores/settings"
	import { state } from "../stores/state"
	import { onMount } from "svelte"
	import { hardwareInfo } from "ui/stores/hardwareInfo"

	const connect = (code: string) => {
		$settings.connectionCode = code
		$state.currentCode = code
		$state.state = "loading"

		if ($hardwareInfo.cpu !== undefined) {
			location.href = "/home"
		} else {
			goto("home")
		}
	}

	onMount(() => {
		state.subscribe((data) => {
			const status = document.querySelector("#status") as HTMLDivElement

			if (data.state === "waiting") {
				status.style.background = "red"
			} else if (data.state === "connected") {
				status.style.background = "green"
			} else {
				status.style.background = "orange"
			}
		})
	})
</script>
