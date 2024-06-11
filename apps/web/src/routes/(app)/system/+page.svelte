{#if $state.state === "connected"}
	<System />
{:else if $state.state === "loading"}
	<Loading />
{:else}
	<Connect />
{/if}

<script>
	import System from "ui/pages/system.svelte"
	import { state } from "../../../stores/state"
	import Loading from "ui/navigation/loading.svelte"
	import Connect from "../../../components/connect.svelte"
</script>
