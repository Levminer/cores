<div class="top"></div>

{#if $appState.state === "connected"}
	{#if hash === "#home"}
		<Home />
	{:else if hash === "#cpu"}
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
	{:else if hash === "#connections"}
		<Connections {WOL} {action} />
	{:else}
		<Home />
	{/if}
{:else}
	<Loading />
{/if}

<script lang="ts">
	import { Cpu, Loading, Ram, settings, Gpu, Storage, Network, System, Home, Connections, supabaseClient } from "ui"
	import { appState } from "../../../stores/state"
	import { onMount } from "svelte"

	let hash = $state(location.hash)

	const signIn = async (access: string, refresh: string) => {
		console.log("login")
		const { data, error } = await supabaseClient.auth.setSession({
			access_token: access,
			refresh_token: refresh,
		})
		console.log("login2")

		console.log(data, error)
	}

	onMount(() => {
		// get code query parameter
		const urlParams = new URLSearchParams(window.location.search)
		const code = urlParams.get("code")
		const access = urlParams.get("access")
		const refresh = urlParams.get("refresh")

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
			console.log("change")
			document.querySelector(".top")?.scrollIntoView({ behavior: "instant", block: "start" })
		})

		if (access && refresh) {
			console.log("login")
			signIn(access, refresh)
		}
	})

	const WOL = async (item: { mac: string }) => {
		$appState.message = JSON.stringify({
			type: "wol",
			data: item.mac?.replaceAll(":", ""),
		})

		alert(`Wake On LAN packet sent to ${item.mac} address.`)
	}

	const action = (type: string) => {
		if (type === "disconnect") {
			return ($appState.state = "disconnected")
		}

		$appState.message = JSON.stringify({
			type,
			data: type,
		})

		alert("Command sent to the connected device. Shutdown/Restart has a 1m delay.")
	}
</script>
