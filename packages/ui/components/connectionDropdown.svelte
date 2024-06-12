<DropdownMenu.Root>
	<DropdownMenu.Trigger
		class="hover:bg-cores-current inline-flex items-center justify-center gap-2 rounded-full bg-gray-700 px-3 py-2 text-xl font-semibold duration-200 ease-in-out"
	>
		<Plug size="30" />
		<p class="hidden md:block">Connection</p>
		<div id="status" class="relative top-0.5 size-3 rounded-full bg-red-800" />
	</DropdownMenu.Trigger>
	<DropdownMenu.Content
		class="bg-background shadow-popover w-full max-w-[229px] rounded-xl bg-gray-700 p-1"
		transition={flyAndScale}
		sideOffset={8}
	>
		{#if $settings.connectionCodes.length === 0}
			<DropdownMenu.Item
				class="flex cursor-pointer select-none items-center gap-3 rounded-xl p-2 duration-200 ease-in-out data-[highlighted]:bg-gray-600"
			>
				<div class="flex items-center">
					<Plug />
				</div>
				<div class="flex flex-col items-start">
					<h4>No connections</h4>
					<h5>Go to settings</h5>
				</div>
			</DropdownMenu.Item>
		{/if}
		{#each $settings.connectionCodes as item, i}
			{#if i > 0}
				<DropdownMenu.Separator class="my-1 -ml-1 -mr-1 block h-px bg-gray-600" />
			{/if}
			<DropdownMenu.Item
				on:click={() => {
					connect(item.code)
				}}
				class="flex cursor-pointer select-none items-center gap-3 rounded-xl p-2 duration-200 ease-in-out data-[highlighted]:bg-gray-600"
			>
				<div class="flex items-center">
					<Plug />
				</div>
				<div class="flex flex-col items-start">
					<h4>{item.name}</h4>
					<h5>crs_********{item.code.slice(-2)}</h5>
				</div>
			</DropdownMenu.Item>
		{/each}
	</DropdownMenu.Content>
</DropdownMenu.Root>

<script lang="ts">
	import { DropdownMenu } from "bits-ui"
	import { flyAndScale } from "../utils/transitions.ts"
	import { Plug } from "lucide-svelte"
	import { settings } from "ui/stores/settings.ts"

	export let connect = (item: string) => {}
</script>
