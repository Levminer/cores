{#if $state.state === "connected"}
	<Connections
		WOL={async (item) => {
			console.log(item)

			$state.message = JSON.stringify({
				type: "wol",
				data: item.mac?.replaceAll(":", ""),
			})
		}}
	/>
{:else if $state.state === "loading"}
	<Loading />
{:else}
	<Connect />
{/if}

<script>
	import Connections from "ui/pages/connections.svelte"
	import { state } from "../../../stores/state"
	import Loading from "ui/navigation/loading.svelte"
	import Connect from "../../../components/connect.svelte"
</script>
