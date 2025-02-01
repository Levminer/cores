{#if $state.state === "connected"}
	<Cpu />
{:else if $state.state === "loading"}
	<Loading />
{:else}
	<Connect />
{/if}

<script>
	import { Cpu, Loading } from "ui"
	import { state } from "../../../stores/state"
	import Connect from "../../../components/Connect.svelte"
</script>
