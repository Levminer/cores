<ModularDialog title={"Connection server"} description={"You can use the default connection server or host your own."}>
	<slot slot="openButton">
		<Dialog.Trigger class="button">
			<Server />
			Change
		</Dialog.Trigger>
	</slot>
	<div class="w-full space-y-5">
		<div class="flex flex-col gap-1 p-3">
			<div>
				<h5 class="mb-1">Connection server</h5>
				<input
					class="form-input w-full rounded-xl border-transparent text-black focus:border-transparent focus:ring-0 disabled:cursor-not-allowed"
					type="text"
					bind:value={conURL}
				/>
				<button
					on:click={async () => {
						$settings.connectionURL = conURL

						if (import.meta.env.VITE_CORES_MODE === "host") {
							await invoke("restart_service")
							location.reload()
						} else {
							location.href = "/home"
						}
					}}
					class="smallButton mt-3 w-full">Confirm</button
				>
			</div>
		</div>
	</div>
</ModularDialog>

<script lang="ts">
	import { hardwareInfo, ModularDialog, Select, settings, Toggle } from "ui"
	import { invoke } from "@tauri-apps/api/core"
	import { Dialog } from "bits-ui"
	import { Server } from "lucide-svelte"
	import { get } from "svelte/store"

	$: conURL = get(settings).connectionURL
</script>
