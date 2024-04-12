{#if $hardwareInfo.cpu !== undefined}
	<GPU />
{/if}

<script>
	import { goto } from "$app/navigation"
	import { onMount } from "svelte"
	import GPU from "ui/pages/gpu.svelte"
	import { hardwareInfo } from "ui/stores/hardwareInfo.ts"

	onMount(() => {
		if ($hardwareInfo.cpu === undefined) {
			goto("/connect")
		}
	})
</script>
