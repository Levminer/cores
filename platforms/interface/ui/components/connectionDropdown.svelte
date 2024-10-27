<Popover.Root>
	{#if mode === "header"}
		<Popover.Trigger
			class="hover:bg-cores-current inline-flex items-center justify-center gap-2 rounded-full bg-gray-700 px-3 py-2 text-xl font-semibold duration-200 ease-in-out"
		>
			<Plug size="30" />
			<p class="hidden md:block">Connection</p>
			<div id="status" class="relative top-0.5 size-3 rounded-full bg-red-800" />
		</Popover.Trigger>
	{:else}
		<Popover.Trigger class="button">
			<Plug size="30" />
			<p>Connections</p>
		</Popover.Trigger>
	{/if}
	<Popover.Content
		class="bg-background w-full max-w-[300px] overflow-hidden rounded-xl bg-gray-600 shadow-xl"
		transition={flyAndScale}
		sideOffset={8}
	>
		{#each $settings.connectionCodes as item, i}
			{#if i > 0}
				<Separator.Root class="-ml-1 -mr-1 block h-px bg-gray-500" />
			{/if}
			<div class="flex select-none items-center justify-between gap-3 p-3 px-5 duration-200 ease-in-out">
				<div class="flex flex-col items-start">
					<h4>{item.name}</h4>
					<h5>crs_********{item.code.slice(-2)}</h5>
				</div>
				<div class="flex items-center gap-1">
					<ModularDialog title={"Edit Remote Connection"} description={"You can get your connection code from the Cores desktop app."}>
						<slot slot="openButton">
							<Dialog.Trigger class="rounded-full bg-white px-3 py-2 text-black duration-200 ease-in-out hover:bg-gray-300">
								<Pencil class="h-5 w-5" />
							</Dialog.Trigger>
						</slot>
						<slot slot="confirmButton">
							<Dialog.Close
								on:click={() => {
									editConnectionCode(item.code)
								}}
								class="smallButton"
							>
								<Pencil />
								Edit
							</Dialog.Close>
						</slot>
						<slot slot="deleteButton">
							<button
								class="smallButton border-red-600 bg-red-600 text-white hover:text-red-600"
								on:click={() => deleteConnectionCode(item.code)}
							>
								<Trash2 />
								Delete
							</button>
						</slot>
						<div class="flex flex-col flex-wrap gap-3">
							<div>
								<h5>Name <span class="text-red-500">*</span></h5>
								<input value={item.name} placeholder="My Home PC" class="input mt-1" type="text" id="name" />
							</div>

							<div>
								<h5>Connection code <span class="text-red-500">*</span></h5>
								<input value={item.code} placeholder="crs_abcde12345" class="input mt-1" type="text" id="code" />
							</div>
						</div>
					</ModularDialog>
					<button
						on:click={() => {
							connect(item.code)
						}}
						class="rounded-full bg-white px-3 py-2 text-black duration-200 ease-in-out hover:bg-gray-300"
					>
						<Plug class="h-5 w-5" />
					</button>
				</div>
			</div>
		{/each}
		<div class="w-full p-3 px-5">
			<ModularDialog title={"Add Remote Connection"} description={"You can get your connection code from the Cores desktop app."}>
				<slot slot="openButton">
					<Dialog.Trigger class="smallButton w-full">Add connection</Dialog.Trigger>
				</slot>
				<slot slot="confirmButton">
					<Dialog.Close on:click={() => addConnectionCode()} class="smallButton">
						<Plus class="h-5 w-5" />
						Add
					</Dialog.Close>
				</slot>
				<div class="flex flex-col flex-wrap gap-3">
					<div>
						<h5>Name <span class="text-red-500">*</span></h5>
						<input placeholder="My Home PC" class="input mt-1" type="text" id="name" />
					</div>

					<div>
						<h5>Connection code <span class="text-red-500">*</span></h5>
						<input placeholder="crs_abcde12345" class="input mt-1" type="text" id="code" />
					</div>
				</div>
			</ModularDialog>
		</div>
	</Popover.Content>
</Popover.Root>

<script lang="ts">
	import { Dialog, Popover, Separator } from "bits-ui"
	import { flyAndScale } from "../utils/transitions.ts"
	import { Pencil, Plug, Plus, Trash2 } from "lucide-svelte"
	import { settings } from "ui/stores/settings.ts"
	import ModularDialog from "./modularDialog.svelte"
	import { addConnectionCode, editConnectionCode, deleteConnectionCode } from "ui/utils/connection.ts"

	export let connect = (item: string) => {}
	export let mode = "header" as "menu" | "header"
</script>
