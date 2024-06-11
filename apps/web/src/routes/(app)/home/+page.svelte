{#if $state.state === "connected"}
	<Home />
{:else if $state.state === "loading"}
	<Loading />
{:else}
	<Connect />
{/if}

<script>
	import Home from "ui/pages/home.svelte"
	import { state } from "../../../stores/state"
	import Loading from "ui/navigation/loading.svelte"
	import Connect from "../../../components/connect.svelte"
</script>
