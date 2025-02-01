{#if $state.state === "connected"}
	<System />
{:else if $state.state === "loading"}
	<Loading />
{:else}
	<Connect />
{/if}

<script>
	import { Loading, System } from "ui"
	import { state } from "../../../stores/state"
	import Connect from "../../../components/Connect.svelte"
</script>
