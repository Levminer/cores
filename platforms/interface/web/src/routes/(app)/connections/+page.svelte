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
	import {Connections, Loading} from "ui"
	import { state } from "../../../stores/state"
	import Connect from "../../../components/Connect.svelte"
</script>
