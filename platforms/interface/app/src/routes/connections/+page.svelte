<ErrorBoundary>
	{#if !$appState.plan}
		<Onboarding goto={goto} />
	{:else}
		<Connections />
	{/if}
</ErrorBoundary>

<script lang="ts">
	import { appState, Connections, ErrorBoundary, Onboarding } from "ui"
	import { goto } from "$app/navigation"
</script>
