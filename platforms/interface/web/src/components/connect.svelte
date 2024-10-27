<div class="m-20 mx-auto flex w-full max-w-2xl flex-row px-3">
	<div class="flex w-full flex-col gap-3">
		{#each $settings.connectionCodes as item}
			<ConnectItem {item} />
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
			</div>
		</ModularDialog>
	</div>
</div>

<script lang="ts">
	import { settings } from "ui/stores/settings.ts"
	import ModularDialog from "ui/components/modularDialog.svelte"
	import { Plus } from "lucide-svelte"
	import { Dialog } from "bits-ui"
	import { addConnectionCode } from "ui/utils/connection.ts"
	import ConnectItem from "./connectItem.svelte"
</script>
