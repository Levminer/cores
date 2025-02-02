<nav class="start-0 top-0 z-20 w-full border-b border-gray-600 bg-gray-900 px-8 sm:px-0">
	<div class="mx-auto flex flex-wrap items-center justify-between p-3">
		<a href="/" class="flex items-center space-x-3">
			<img class="h-8 w-8" src={"/favicon.ico"} alt="Cores logo" />
			<span class="self-center whitespace-nowrap text-xl font-semibold text-white">Cores</span>
		</a>
		<div class="flex space-x-2 md:order-2">
			<a
				href="/account"
				class="inline-flex items-center justify-center gap-2 rounded-2xl bg-gray-700 px-3 py-2 text-lg font-medium duration-200 ease-in hover:bg-white hover:text-black"
			>
				<User />
				<p class="hidden md:block">Account</p>
			</a>
			{#if $state.state === "connected"}
				<PowerDropdown {action} />
			{/if}
			<ConnectionDropdown {connect} />
		</div>
	</div>
</nav>

<script lang="ts">
	import { state } from "../stores/state"
	import { onMount } from "svelte"
	import { User } from "lucide-svelte"
	import { settings } from "ui"
	import PowerDropdown from "./PowerDropdown.svelte"
	import ConnectionDropdown from "./ConnectionDropdown.svelte"

	const action = (type: string) => {
		if (type === "disconnect") {
			return ($state.state = "disconnected")
		}

		$state.message = JSON.stringify({
			type,
			data: type,
		})
	}

	const connect = (code: string) => {
		$settings.connectionCode = code
		$state.currentCode = code
		$state.state = "swapping"
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
