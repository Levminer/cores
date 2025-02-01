{#if $state.state === "connected"}
	<Ram />
{:else if $state.state === "loading"}
	<Loading />
{:else}
	<Connect />
{/if}

<script>
	import { Loading, Ram } from "ui"
	import { state } from "../../../stores/state"
	import Connect from "../../../components/Connect.svelte"
</script>
