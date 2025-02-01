<Select.Root
	items={options}
	selected={options.find((option) => option.value === $settings[setting])}
	onSelectedChange={(event) => {
		$settings[setting] = event.value
	}}
>
	<Select.Trigger class="select w-48" aria-label="Select a theme">
		<Select.Value class="text-xl" placeholder="Select a theme" />
	</Select.Trigger>

	<Select.Content class="w-full rounded-xl shadow-xl border-cyan-500 bg-white p-2 text-black outline-none" sideOffset={8} transition={flyAndScale}>
		{#each options as option}
			<Select.Item
				class="rounded-button flex h-10 w-full select-none items-center rounded-lg py-2 pl-3 pr-2 text-base outline-none transition-all duration-150 ease-linear aria-selected:font-bold data-[highlighted]:bg-black data-[highlighted]:text-white"
				value={option.value}
				label={option.label}
			>
				{option.label}
				<Select.ItemIndicator class="ml-auto" asChild={false}>
					<Check />
				</Select.ItemIndicator>
			</Select.Item>
		{/each}
	</Select.Content>
</Select.Root>

<script lang="ts">
	import { Select } from "bits-ui"
	import { Check } from "lucide-svelte"
	import { flyAndScale } from "../utils/transitions.ts"
	import { settings } from "ui/stores/settings.ts"

	export let options = [] as { value: number | string; label: string }[]
	export let setting: string
</script>
