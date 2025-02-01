{#key $router.path}
	<div in:fade={{ duration: 200 }}>
		<slot />
	</div>
{/key}

<script lang="ts">
	import { router } from "@baileyherbert/tinro"
	import { fade } from "svelte/transition"
</script>
