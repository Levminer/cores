{#if $hardwareInfo.cpu === undefined}
	<Loading showTips={true} />
{:else}
	<slot />
{/if}

<script lang="ts">
	import { setHardwareInfo, hardwareInfo } from "ui/stores/hardwareInfo"
	import Loading from "ui/navigation/loading.svelte"
</script>
