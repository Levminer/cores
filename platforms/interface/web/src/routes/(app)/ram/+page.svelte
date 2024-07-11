{#if $state.state === "connected"}
	<RAM />
{:else if $state.state === "loading"}
	<Loading />
{:else}
	<Connect />
{/if}

<script>
	import RAM from "ui/pages/ram.svelte"
	import { state } from "../../../stores/state"
	import Loading from "ui/navigation/loading.svelte"
	import Connect from "../../../components/connect.svelte"
</script>
