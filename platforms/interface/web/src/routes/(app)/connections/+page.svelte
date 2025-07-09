{#if $appState.state === "connected"}
	<Connections {WOL} />
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
	}

	import { Connections, Loading } from "ui"
	import { appState } from "../../../stores/state"
	import Connect from "../../../components/Connect.svelte"
</script>
