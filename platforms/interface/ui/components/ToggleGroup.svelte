<BitsToggleGroup.Root type="single" bind:value class="transparent-900 mt-1 inline-flex w-full items-center gap-2 rounded-xl p-2">
	{#each options as option}
		<BitsToggleGroup.Item
			value={option.value}
			class="flex-1 rounded-lg px-4 py-2 text-base font-medium text-zinc-300 transition-all duration-200 hover:bg-zinc-700/40 data-[state=on]:bg-white data-[state=on]:text-black"
		>
			{option.label}
		</BitsToggleGroup.Item>
	{/each}
</BitsToggleGroup.Root>

<script lang="ts">
	import { ToggleGroup as BitsToggleGroup } from "bits-ui"

	interface Option {
		value: string
		label: string
	}

	interface Props {
		value?: string
		options?: Option[]
	}

	let { value = $bindable(""), options = [] }: Props = $props()
</script>
