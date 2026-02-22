<button {onclick} class="mx-auto flex flex-1">
	<span
		class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2"
	>
		{#if selected}
			<TimerReset />
		{:else}
			<Timer />
		{/if}
	</span>
</button>

<script lang="ts">
	import { Timer, TimerReset } from "lucide-svelte"

	let { selected = false, onclick }: { selected: boolean; onclick: () => void } = $props()
</script>
