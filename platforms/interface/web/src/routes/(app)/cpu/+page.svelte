{#if $appState.state === "connected"}
	<Cpu />
{:else if $appState.state === "loading"}
	<Loading />
{:else}
	<Connect />
{/if}

<script>
	import { Cpu, Loading } from "ui"
	import { appState } from "../../../stores/state"
	import Connect from "../../../components/Connect.svelte"
</script>
