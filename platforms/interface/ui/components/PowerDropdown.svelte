<DropdownMenu.Root>
	<DropdownMenu.Trigger
		class="inline-flex items-center justify-center gap-2 rounded-2xl bg-gray-700 px-3 py-2 text-lg font-medium duration-200 ease-in hover:bg-white hover:text-black"
	>
		<Power />
		<p class="hidden md:block">Power</p>
	</DropdownMenu.Trigger>
	<DropdownMenu.Content
		class="bg-background w-full max-w-[230px] overflow-hidden rounded-xl bg-gray-600 shadow-xl"
		transition={flyAndScale}
		sideOffset={8}
	>
		<DropdownMenu.Item
			on:click={() => {
				action("disconnect")
			}}
			class="flex cursor-pointer select-none items-center gap-3 p-3 px-5 duration-200 ease-in-out data-[highlighted]:bg-gray-500"
		>
			<div class="flex items-center">
				<ScreenShareOff />
			</div>
			<div class="flex flex-col items-start">
				<h4>Disconnect</h4>
			</div>
		</DropdownMenu.Item>
		<DropdownMenu.Separator class="-ml-1 -mr-1 block h-px bg-gray-500" />
		<DropdownMenu.Item
			on:click={() => {
				action("sleep")
			}}
			class="flex cursor-pointer select-none items-center gap-3 p-3 px-5 duration-200 ease-in-out data-[highlighted]:bg-gray-500"
		>
			<div class="flex items-center">
				<Moon />
			</div>
			<div class="flex flex-col items-start">
				<h4>Sleep</h4>
			</div>
		</DropdownMenu.Item>
		<DropdownMenu.Separator class="-ml-1 -mr-1 block h-px bg-gray-500" />
		<DropdownMenu.Item
			on:click={() => {
				action("shutdown")
			}}
			class="flex cursor-pointer select-none items-center gap-3 p-3 px-5 duration-200 ease-in-out data-[highlighted]:bg-gray-500"
		>
			<div class="flex items-center">
				<Power />
			</div>
			<div class="flex flex-col items-start">
				<h4>Shut down</h4>
			</div>
		</DropdownMenu.Item>
		<DropdownMenu.Separator class="-ml-1 -mr-1 block h-px bg-gray-500" />
		<DropdownMenu.Item
			on:click={() => {
				action("restart")
			}}
			class="flex cursor-pointer select-none items-center gap-3 p-3 px-5 duration-200 ease-in-out data-[highlighted]:bg-gray-500"
		>
			<div class="flex items-center">
				<RotateCcw />
			</div>
			<div class="flex flex-col items-start">
				<h4>Restart</h4>
			</div>
		</DropdownMenu.Item>
	</DropdownMenu.Content>
</DropdownMenu.Root>

<script lang="ts">
	import { DropdownMenu } from "bits-ui"
	import { flyAndScale } from "../utils/transitions.ts"
	import { Moon, Power, RotateCcw, ScreenShareOff } from "lucide-svelte"

	export let action = (type: string) => {}
</script>
