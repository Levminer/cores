{#if $hardwareInfo.cpu !== undefined}
	<System />
{/if}

<script>
	import { goto } from "$app/navigation"
	import { onMount } from "svelte"
	import System from "ui/pages/system.svelte"
	import { hardwareInfo } from "ui/stores/hardwareInfo.ts"

	onMount(() => {
		if ($hardwareInfo.cpu === undefined) {
			goto("/connect")
		}
	})
</script>
