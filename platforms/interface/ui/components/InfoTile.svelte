<div class="transparent-800 rounded-xl p-8 sm:p-4 {className}">
	<div class="mb-5 flex items-center gap-3">
		<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
			{@render icon?.()}
		</div>
		<h2 class="max-w-full truncate">{title}</h2>
	</div>
	<h3>
		{@render description?.()}
	</h3>
	<div>
		{@render children?.()}
	</div>
</div>

<script lang="ts">
	import type { Snippet } from "svelte"

	interface Props {
		title: string
		description?: Snippet
		icon?: Snippet
		children?: Snippet
		class?: string
	}

	let { title = "", description, icon, children, class: className = "" }: Props = $props()
</script>
