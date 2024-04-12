{#if $hardwareInfo.cpu !== undefined}
	<Storage />
{/if}

<script>
	import { goto } from "$app/navigation"
	import { onMount } from "svelte"
	import Storage from "ui/pages/storage.svelte"
	import { hardwareInfo } from "ui/stores/hardwareInfo.ts"

	onMount(() => {
		if ($hardwareInfo.cpu === undefined) {
			goto("/connect")
		}
	})
</script>
