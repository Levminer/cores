<button {onclick} class="transparent-900 absolute right-3 top-3 z-10 flex items-center gap-1 rounded-md px-2 py-1 text-xs">
	{#if hidden}
		<Eye size={14} />
		Show
	{:else}
		<EyeOff size={14} />
		Hide
	{/if}
</button>

<script lang="ts">
	import { Eye, EyeOff } from "lucide-svelte"

	interface Props {
		hidden: boolean
		onclick: () => void
	}

	let { hidden, onclick }: Props = $props()
</script>
