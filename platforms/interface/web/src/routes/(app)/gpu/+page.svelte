{#if $state.state === "connected"}
	<Gpu />
{:else if $state.state === "loading"}
	<Loading />
{:else}
	<Connect />
{/if}

<script>
	import { Gpu, Loading } from "ui"
	import { state } from "../../../stores/state"
	import Connect from "../../../components/Connect.svelte"
</script>
