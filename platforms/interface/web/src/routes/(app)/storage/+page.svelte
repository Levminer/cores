{#if $appState.state === "connected"}
	<Storage />
{:else if $appState.state === "loading"}
	<Loading />
{:else}
	<Connect />
{/if}

<script>
	import { Loading, Storage } from "ui"
	import { appState } from "../../../stores/state"
	import Connect from "../../../components/Connect.svelte"
</script>
