<Select.Root
	type="single"
	items={options}
	bind:value
	onValueChange={(newValue) => {
		// @ts-ignore
		$settings[setting] = parseInt(newValue)
	}}
>
	<Select.Trigger class="select w-48" aria-label="Select a value">
		<p class="text-xl">{selectedLabel}</p>
	</Select.Trigger>

	<Select.Portal>
		<Select.Content forceMount class="w-48 rounded-xl bg-white p-2 text-black shadow-xl outline-none" sideOffset={8}>
			{#snippet child({ wrapperProps, props, open })}
				{#if open}
					<div {...wrapperProps}>
						<div {...props} transition:flyAndScale>
							{#each options as option, i}
								<Select.Item
									class="flex h-10 w-full select-none items-center rounded-lg py-2 pl-3 pr-2 text-base outline-none transition-all duration-150 ease-linear aria-selected:font-bold data-[highlighted]:bg-black data-[highlighted]:text-white"
									value={option.value}
									label={option.label}
								>
									{#snippet children({ selected })}
										{option.label}
										{#if selected}
											<div class="ml-auto">
												<Check aria-label="check" />
											</div>
										{/if}
									{/snippet}
								</Select.Item>
							{/each}
						</div>
					</div>
				{/if}
			{/snippet}
		</Select.Content>
	</Select.Portal>
</Select.Root>

<script lang="ts">
	import { Select } from "bits-ui"
	import { Check } from "lucide-svelte"
	import { flyAndScale } from "../utils/transitions.ts"
	import { settings } from "ui"

	interface Props {
		options?: { value: string; label: string }[]
		setting: keyof LibSettings
	}

	let { options = [], setting }: Props = $props()

	let value = $state<string>(options.find((option) => parseInt(option.value) === $settings[setting])?.value || "")
	const selectedLabel = $derived(value ? options.find((option) => option.value === value)?.label : "Select a value")
	$inspect(value)
	$inspect(selectedLabel)
</script>
