<ErrorBoundary>
	{#if !$appState.plan}
		<Onboarding />
	{:else}
		<Connections />
	{/if}
</ErrorBoundary>

<script lang="ts">
	import { appState, Connections, ErrorBoundary, Onboarding } from "ui"
</script>
