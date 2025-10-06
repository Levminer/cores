{#if $appState.state === "connected"}
	<Connections {WOL} {action} />
{:else if $appState.state === "loading"}
	<Loading />
{:else}
	<Connect />
{/if}

<script lang="ts">
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
	}

	import { Connections, Loading } from "ui"
	import { appState } from "../../../stores/state"
	import Connect from "../../../components/Connect.svelte"
</script>
