<svelte:boundary onerror={console.error}>
	{@render children()}

	{#snippet failed(error)}
		<p>Failed to load, please try again later!</p>
		<p>{error}</p>
	{/snippet}
</svelte:boundary>

<script>
	let { children } = $props()
</script>
