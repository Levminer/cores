{#if $state.state === "connected"}
	<Storage />
{:else if $state.state === "loading"}
	<Loading />
{:else}
	<Connect />
{/if}

<script>
	import { Loading, Storage } from "ui"
	import { state } from "../../../stores/state"
	import Connect from "../../../components/Connect.svelte"
</script>
