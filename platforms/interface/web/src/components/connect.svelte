<div class="m-20 mx-auto flex w-full max-w-2xl flex-row px-3">
	<div class="flex w-full flex-col gap-3 rounded-xl p-3">
		{#each $settings.connectionCodes as item}
			<ConnectItem {item} />
		{/each}
		{#if $settings.connectionCodes.length === 0}
			<div class="text-center">
				<h2>No remote connections</h2>
				<h3 class="mb-5 text-center">Add a new remote connection or login to sync your connections</h3>
			</div>
		{/if}
		{#if user === null}
			<a href="/login" class="button">Login</a>
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
	import { getSettings, setSettings, settings } from "ui/stores/settings.ts"
	import ModularDialog from "ui/components/modularDialog.svelte"
	import { Plus } from "lucide-svelte"
	import { Dialog } from "bits-ui"
	import { addConnectionCode } from "ui/utils/connection.ts"
	import ConnectItem from "./connectItem.svelte"
	import { onMount } from "svelte"
	import { supabaseClient } from "ui/utils/supabase"
	import type { User } from "@supabase/supabase-js"

	$: user = null as User | null

	onMount(async () => {
		const { data: userData, error: userError } = await supabaseClient.auth.getUser()
		const settings = getSettings()

		if (!userError && userData !== null) {
			user = userData.user
			const { data, error } = await supabaseClient.from("remote_connection").select("*")

			// check if connection is already added
			if (data && data.length > 0) {
				for (let i = 0; i < data.length; i++) {
					const item = $settings.connectionCodes.filter((item) => item.code === data[i].code)

					if (item.length === 0) {
						settings.connectionCodes = [
							...settings.connectionCodes,
							{
								name: data[i].name!,
								code: data[i].code!,
							},
						]

						setSettings(settings)
					}
				}
			}
		}
	})
</script>
