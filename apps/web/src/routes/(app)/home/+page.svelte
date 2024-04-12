{#if $hardwareInfo.cpu !== undefined}
	<Home />
{/if}

<script>
	import { goto } from "$app/navigation"
	import { onMount } from "svelte"
	import Home from "ui/pages/home.svelte"
	import { hardwareInfo } from "ui/stores/hardwareInfo.ts"

	onMount(() => {
		if ($hardwareInfo.cpu === undefined) {
			goto("/connect")
		}
	})
</script>
