{#if $appState.state === "connected"}
	<System />
{:else if $appState.state === "loading"}
	<Loading />
{:else}
	<Connect />
{/if}

<script>
	import { Loading, System } from "ui"
	import { appState } from "../../../stores/state"
	import Connect from "../../../components/Connect.svelte"
</script>
