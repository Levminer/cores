<div class="transparent-900 flex w-full flex-col space-y-1 rounded-xl p-5">
	<div class="flex flex-row items-center justify-between">
		<div class="flex flex-row items-center gap-2">
			<div class="transparent-800 flex items-center justify-center rounded-full p-3">
				{#if status === "online"}
					<img src="/favicon.ico" alt="Cores logo" class="h-5 w-5" />
				{:else}
					<img src="/grayscale.png" alt="Cores logo" class="h-5 w-5" />
				{/if}
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
	<div class="flex flex-row flex-wrap items-center justify-start gap-1 pt-1">
		<div class="transparent-800 flex flex-row items-center gap-1 rounded-xl p-1 px-3">
			<Globe class="h-5 w-5" color={"#d3cfcf"} />
			<h5>{status === "online" ? "Online" : "Unknown"}</h5>
		</div>
		<div class="transparent-800 flex flex-row items-center gap-1 rounded-xl p-1 px-3">
			<KeyRound class="h-5 w-5" color={"#d3cfcf"} />
			<h5>crs_********{item.code.slice(-2)}</h5>
		</div>
	</div>
</div>

<script lang="ts">
	import { Plug, Trash2, Pencil, Network, KeyRound, Globe } from "lucide-svelte"
	import { goto } from "$app/navigation"
	import ModularDialog from "ui/components/modularDialog.svelte"
	import { Dialog } from "bits-ui"
	import { deleteConnectionCode, editConnectionCode } from "ui/utils/connection.ts"
	import { settings } from "ui/stores/settings.ts"
	import { state } from "../stores/state.ts"
	import { onMount } from "svelte"

	export let item: LibSettings["connectionCodes"][0]

	type Status = "unknown" | "online" | "offline"

	let status = "unknown" as Status

	onMount(async () => {
		const res = await fetch(`https://rtc-usw.levminer.com/status/${item.code}`)

		const json = await res.json()

		if (json.online) {
			status = "online"
		}
	})
</script>
