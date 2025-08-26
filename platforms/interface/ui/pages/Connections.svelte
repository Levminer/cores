<div class="transparent-900 m-10 mx-auto w-11/12 rounded-xl sm:w-full">
	{#if import.meta.env.VITE_CORES_MODE === "host"}
		<div class="mx-10 flex flex-col gap-5 pb-10 pt-10 sm:mx-3 sm:flex-wrap">
			<!-- remote connections -->
			<div class="transparent-800 flex w-full flex-row items-center justify-between rounded-xl p-8 text-left sm:p-4">
				<div class="flex flex-col items-start gap-3">
					<div class="flex items-center gap-3">
						<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
							<MonitorSmartphone />
						</div>
						<h2>Remote connections</h2>
					</div>
					<h3>You can access and control your computer from any device using the web dashboard.</h3>
				</div>

				<div class="flex flex-col items-start gap-3">
					<Toggle bind:checked={$settings.remoteConnections} onChange={remoteConnections} />
				</div>
			</div>

			<!-- web dashboard -->
			<div class="transparent-800 flex w-full flex-row items-center justify-between rounded-xl p-8 text-left sm:p-4">
				<div class="flex flex-col items-start gap-3">
					<div class="flex items-center gap-3">
						<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
							<Earth />
						</div>
						<h2>Web Dashboard</h2>
					</div>
					<h3>Your remote connections are automatically synced with the web dashboard.</h3>
				</div>

				<div class="flex flex-col items-start gap-3">
					<button
						class="button"
						onclick={() => {
							open("https://www.coresmonitor.com/login")
						}}
					>
						<ExternalLink />
						Open dashboard
					</button>
				</div>
			</div>

			<!-- copy connection code -->
			{#if $settings.remoteConnections}
				<div class="transparent-800 flex w-full flex-row items-center justify-between rounded-xl p-8 text-left sm:p-4">
					<div class="flex flex-col items-start gap-3">
						<div class="flex items-center gap-3">
							<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
								<KeyRound />
							</div>
							<h2>Connection code</h2>
						</div>
						<h3>
							You can also add this code manually on the dashboard <br /> to monitor and control you device, keep it private.
						</h3>
					</div>

					<div class="flex flex-col items-start gap-3">
						<div class="flex items-center justify-center space-x-3">
							<input class="input" readonly value={$settings.connectionCode} />
							<button class="button" onclick={copyConnectionCode}>
								<Clipboard />
								<span class="copy">Copy</span>
							</button>
						</div>
					</div>
				</div>
			{/if}

			<!-- connection server -->
			<div class="transparent-800 flex w-full flex-row items-center justify-between rounded-xl p-8 text-left sm:p-4">
				<div class="flex flex-col items-start gap-3">
					<div class="flex items-center gap-3">
						<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
							<Server />
						</div>
						<h2>Connection server</h2>
					</div>
					<h3>You can use the default connection server or host your own.</h3>
				</div>

				<div class="flex flex-col items-start gap-3">
					<ConnectionServer />
				</div>
			</div>
		</div>
	{/if}
</div>

<div class="transparent-900 m-10 mx-auto w-11/12 rounded-xl sm:w-full">
	{#if ($settings.remoteConnections && import.meta.env.VITE_CORES_MODE === "host") || import.meta.env.VITE_CORES_MODE === "client"}
		<div class="mx-10 flex flex-col gap-5 pb-10 pt-10 sm:mx-3 sm:flex-wrap">
			<!-- remote connections -->
			<div class="transparent-800 flex w-full flex-row flex-wrap items-center justify-between rounded-xl p-8 text-left sm:p-4">
				<div class="flex flex-col items-start gap-3">
					<div class="flex items-center gap-3">
						<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
							<Network />
						</div>
						<h2>Wake On LAN</h2>
					</div>
					<h3>You can wake up other devices on your network with Wake On LAN.</h3>
				</div>

				<div class="flex flex-col items-start gap-3 sm:my-5">
					<ModularDialog title={"Add device"} description={"You can get the MAC address from the Cores desktop app."}>
						{#snippet openButton()}
							<Dialog.Trigger class="button w-full">
								<Plus />
								Add device
							</Dialog.Trigger>
						{/snippet}
						{#snippet confirmButton()}
							<Dialog.Close on:click={() => addNetworkDevice()} class="smallButton">
								<Plus class="h-5 w-5" />
								Add device
							</Dialog.Close>
						{/snippet}
						<div class="flex flex-col flex-wrap gap-3">
							<div>
								<h5>Name <span class="text-red-500">*</span></h5>
								<input placeholder="My Home PC" class="input mt-1" type="text" id="name" />
							</div>

							<div>
								<h5>MAC address <span class="text-red-500">*</span></h5>
								<input placeholder="AA:BB:CC:DD:EE:FF" class="input mt-1" type="text" id="mac" />
							</div>
						</div>
					</ModularDialog>
				</div>

				<div class="mt-5 flex w-full flex-col gap-5">
					{#each $settings.networkDevices.filter((item) => item.mac !== undefined && item.mac !== "") as item, i}
						<div class="flex w-full flex-row flex-wrap items-center justify-between gap-3">
							<div class="flex flex-row flex-wrap gap-3">
								<div>
									<h5>Name</h5>
									<input class="input mt-1" type="text" value={item.name} readonly />
								</div>

								<div>
									<h5>MAC address</h5>
									<input class="input mt-1" type="text" value={item.mac} readonly />
								</div>
							</div>

							<div class="flex flex-row flex-wrap gap-3">
								<button
									class="button"
									onclick={() => {
										WOL(item)
									}}
								>
									<Power />
									Wake up
								</button>

								<button
									class="button"
									onclick={() => {
										deleteNetworkDevice(item.mac)
									}}
								>
									<Trash2 />
									Delete
								</button>
							</div>
						</div>
					{/each}
				</div>
			</div>
		</div>
	{/if}
</div>

<script lang="ts">
	import { ConnectionServer, getSettings, hardwareInfo, ModularDialog, setSettings, settings, Toggle } from "ui"
	import { Clipboard, ExternalLink, MonitorSmartphone, KeyRound, Network, Plus, Power, Trash2, Earth, Server } from "lucide-svelte"
	import { invoke } from "@tauri-apps/api/core"
	import { open } from "@tauri-apps/plugin-shell"
	import { Dialog } from "bits-ui"
	import { supabaseClient } from "../utils/supabase.ts"
	import { addNetworkDevice, deleteNetworkDevice } from "../utils/connection.ts"
	import { onMount } from "svelte"

	onMount(async () => {
		const { data: userData, error: userError } = await supabaseClient.auth.getUser()

		if (!userError && userData !== null) {
			const { data, error } = await supabaseClient.from("network_device").select("*").order("created_at", { ascending: true })

			// check if connection is already added
			if (data && data.length > 0) {
				for (let i = 0; i < data.length; i++) {
					const settings = getSettings()
					const item = settings.networkDevices.filter((item) => item.mac === data[i].mac)

					if (item.length === 0) {
						settings.networkDevices = [
							...settings.networkDevices,
							{
								name: data[i].name!,
								code: "",
								mac: data[i].mac!,
							},
						]

						setSettings(settings)
					}
				}
			}
		}
	})

	const remoteConnections = async () => {
		const { data: userData, error: userError } = await supabaseClient.auth.getUser()
		const { data: connectionData, error: connectionError } = await supabaseClient.from("remote_connection").select("*")

		if (connectionData && connectionData.length > 5) {
			$settings.remoteConnections = false
			return alert(
				"You can only have a maximum of 5 remote connections. Please remove one before adding a new one. \n\nIf you need more connections please reach out to support@coresmonitor.com for more information.",
			)
		}

		if (import.meta.env.PROD) {
			if ($settings.remoteConnections) {
				const { data, error } = await supabaseClient.from("remote_connection").insert({
					code: $settings.connectionCode,
					name: $hardwareInfo.system.os.hostname ?? "Cores Desktop",
					user_id: userData?.user?.id,
				})
			} else {
				const { data, error } = await supabaseClient.from("remote_connection").delete().eq("code", $settings.connectionCode)
			}

			invoke("restart_service")
		}
	}

	const copyConnectionCode = () => {
		navigator.clipboard.writeText($settings.connectionCode)
		document.querySelector(".copy")!.innerHTML = "Copied"

		setTimeout(() => {
			document.querySelector(".copy")!.innerHTML = "Copy"
		}, 1000)
	}

	let {
		WOL = async (item: { name?: string; code?: string; mac: string }) => {
			await fetch("http://localhost:5390/post", {
				method: "POST",
				body: JSON.stringify({
					type: "wol",
					data: {
						mac: item.mac?.replaceAll(":", ""),
					},
				}),
				headers: {
					"Content-Type": "application/json",
				},
			})
		},
	} = $props()
</script>
