{#if $hardwareInfo.cpu !== undefined}
	<Network />
{/if}

<script>
	import { goto } from "$app/navigation"
	import { onMount } from "svelte"
	import Network from "ui/pages/network.svelte"
	import { hardwareInfo } from "ui/stores/hardwareInfo.ts"

	onMount(() => {
		if ($hardwareInfo.cpu === undefined) {
			goto("/connect")
		}
	})
</script>
