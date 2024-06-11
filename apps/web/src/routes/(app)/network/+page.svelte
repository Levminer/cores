{#if $state.state === "connected"}
	<Network />
{:else if $state.state === "loading"}
	<Loading />
{:else}
	<Connect />
{/if}

<script>
	import Network from "ui/pages/network.svelte"
	import { state } from "../../../stores/state"
	import Loading from "ui/navigation/loading.svelte"
	import Connect from "../../../components/connect.svelte"
</script>
