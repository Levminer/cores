<div class="m-20 mx-auto flex w-full max-w-2xl flex-row px-3">
	<div class="flex w-full flex-col gap-3">
		{#each $settings.connectionCodes as item}
			<div class="transparent-900 flex w-full flex-col space-y-1 rounded-xl p-5">
				<div class="flex flex-row items-center justify-between">
					<div class="flex flex-row items-center gap-2">
						<div class="transparent-800 rounded-full p-3">
							<Power id="power" class="h-5 w-5 text-white" />
						</div>
						<h4>{item.name}</h4>
					</div>
					<div>
						<ModularDialog title={"Edit Remote Connection"} description={"You can get your connection code from the Cores desktop app."}>
							<slot slot="openButton">
								<Dialog.Trigger class="rounded-full bg-white p-3 text-black duration-200 ease-in-out hover:bg-gray-300">
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
							class="rounded-full bg-white p-3 text-black duration-200 ease-in-out hover:bg-gray-300"
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
				<div class="flex flex-col justify-center space-y-1">
					<div class="flex flex-row items-center gap-1">
						<KeyRound class="h-5 w-5" color={"#d3cfcf"} />
						<h5>crs_********{item.code.slice(-2)}</h5>
					</div>
					<div class="flex flex-row items-center gap-1">
						<Network class="h-5 w-5" color={"#d3cfcf"} />
						<h5>{item.mac || "N/A"}</h5>
					</div>
				</div>
			</div>
		{/each}
		{#if $settings.connectionCodes.length === 0}
			<h2 class="mb-5 text-center">Add a new remote connection</h2>
		{/if}
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

<script lang="ts">
	import { settings } from "ui/stores/settings.ts"
	import { Plug, Trash2, Pencil, Network, KeyRound, Power } from "lucide-svelte"
	import { goto } from "$app/navigation"
	import { state } from "../stores/state.ts"
	import ModularDialog from "ui/components/modularDialog.svelte"
	import { Plus } from "lucide-svelte"
	import { Dialog } from "bits-ui"
	import { addConnectionCode, deleteConnectionCode, editConnectionCode } from "ui/utils/connection.ts"
</script>
