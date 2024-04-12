{#if $hardwareInfo.cpu !== undefined}
	<RAM/>
{/if}

<script>
	import { goto } from "$app/navigation"
	import { onMount } from "svelte"
	import RAM from "ui/pages/ram.svelte"
	import { hardwareInfo } from "ui/stores/hardwareInfo.ts"

	onMount(() => {
		if ($hardwareInfo.cpu === undefined) {
			goto("/connect")
		}
	})
</script>
