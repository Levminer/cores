{#if $appState.state === "connected"}
	<Ram />
{:else if $appState.state === "loading"}
	<Loading />
{:else}
	<Connect />
{/if}

<script>
	import { Loading, Ram } from "ui"
	import { appState } from "../../../stores/state"
	import Connect from "../../../components/Connect.svelte"
</script>
