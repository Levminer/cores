{#if $hardwareInfo.cpu !== undefined}
	<CPU />
{/if}

<script>
	import { goto } from "$app/navigation"
	import { onMount } from "svelte"
	import CPU from "ui/pages/cpu.svelte"
	import { hardwareInfo } from "ui/stores/hardwareInfo.ts"

	onMount(() => {
		if ($hardwareInfo.cpu === undefined) {
			goto("/connect")
		}
	})
</script>
