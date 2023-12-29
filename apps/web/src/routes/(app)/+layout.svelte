{#if $hardwareInfo.cpu === undefined}
	<Loading showTips={true} />
{:else}
	<BuildNumber />
	<Navigation />
	<slot />
{/if}

<script lang="ts">
	import { setHardwareInfo, hardwareInfo } from "ui/stores/hardwareInfo"
	import Loading from "ui/navigation/loading.svelte"
	import BuildNumber from "ui/navigation/buildNumber.svelte"
	import Navigation from "ui/navigation/navigation.svelte"
</script>
