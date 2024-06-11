{#if $state.state === "connected"}
	<CPU />
{:else if $state.state === "loading"}
	<Loading />
{:else}
	<Connect />
{/if}

<script>
	import CPU from "ui/pages/cpu.svelte"
	import { state } from "../../../stores/state"
	import Loading from "ui/navigation/loading.svelte"
	import Connect from "../../../components/connect.svelte"
</script>
