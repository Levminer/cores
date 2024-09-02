<div class="mx-auto mb-20 flex min-h-screen w-full flex-col items-center justify-center">
	<div class="transparent-900 m-3 my-10 rounded-xl p-8 text-left">
		<h2 class="mb-5 text-center">Select remote connection</h2>
		<div class="flex flex-col gap-3">
			{#each $settings.connectionCodes as item}
				<div class="transparent-800 flex w-full flex-row items-center justify-between rounded-xl p-5">
					<div>
						<h4>{item.name}</h4>
						<h5>crs_********{item.code.slice(-2)}</h5>
					</div>
					<div class="flex flex-row gap-2">
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

								<div>
									<h5>MAC address</h5>
									<input value={item.mac ?? ""} placeholder="AA:BB:CC:DD:EE:FF" class="input mt-1" type="text" id="mac" />
								</div>
							</div>
						</ModularDialog>
						<button
							class="rounded-full bg-white px-3 py-2 text-black duration-200 ease-in-out hover:bg-gray-300"
							on:click={() => {
								$settings.connectionCode = item.code
								$state.currentCode = item.code
								goto("/home")
							}}
						>
							<Plug class="h-5 w-5" />
						</button>
					</div>
				</div>
			{/each}
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

					<div>
						<h5>MAC address</h5>
						<input placeholder="AA:BB:CC:DD:EE:FF" class="input mt-1" type="text" id="mac" />
					</div>
				</div>
			</ModularDialog>
		</div>
	</div>
</div>

<script lang="ts">
	import { settings } from "ui/stores/settings.ts"
	import { Plug, Trash2, Pencil } from "lucide-svelte"
	import { goto } from "$app/navigation"
	import { state } from "../stores/state.ts"
	import ModularDialog from "ui/components/modularDialog.svelte"
	import { Plus } from "lucide-svelte"
	import { Dialog } from "bits-ui"
	import { addConnectionCode, deleteConnectionCode, editConnectionCode } from "ui/utils/connection.ts"

</script>
