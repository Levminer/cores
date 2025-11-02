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
						<h2>Connect remotely</h2>
					</div>
					<h3>Your remote connections are automatically synced and accessible on the web dashboard or in the iOS app.</h3>
				</div>

				<div class="ml-10 flex flex-col items-start gap-3 md:flex-row">
					<button
						class="button"
						onclick={() => {
							open("https://www.coresmonitor.com/login")
						}}
					>
						<ExternalLink />
						Dashboard
					</button>

					<button
						class="button"
						onclick={() => {
							open("https://link.levminer.com/cores-ios")
						}}
					>
						<svg class="h-6 w-6" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
							<path
								fill="currentColor"
								d="M8.8086 14.9194l6.1107-11.0368c.0837-.1513.1682-.302.2437-.4584.0685-.142.1267-.2854.1646-.4403.0803-.3259.0588-.6656-.066-.9767-.1238-.3095-.3417-.5678-.6201-.7355a1.4175 1.4175 0 0 0-.921-.1924c-.3207.043-.6135.1935-.8443.4288-.1094.1118-.1996.2361-.2832.369-.092.1463-.175.2979-.259.4492l-.3864.6979-.3865-.6979c-.0837-.1515-.1667-.303-.2587-.4492-.0837-.1329-.1739-.2572-.2835-.369-.2305-.2353-.5233-.3857-.844-.429a1.4181 1.4181 0 0 0-.921.1926c-.2784.1677-.4964.426-.6203.7355-.1246.311-.1461.6508-.066.9767.038.155.0962.2984.1648.4403.0753.1564.1598.307.2437.4584l1.248 2.2543-4.8625 8.7825H2.0295c-.1676 0-.3351-.0007-.5026.0092-.1522.009-.3004.0284-.448.0714-.3108.0906-.5822.2798-.7783.548-.195.2665-.3006.5929-.3006.9279 0 .3352.1057.6612.3006.9277.196.2683.4675.4575.7782.548.1477.043.296.0623.4481.0715.1675.01.335.009.5026.009h13.0974c.0171-.0357.059-.1294.1-.2697.415-1.4151-.6156-2.843-2.0347-2.843zM3.113 18.5418l-.7922 1.5008c-.0818.1553-.1644.31-.2384.4705-.067.1458-.124.293-.1611.452-.0785.3346-.0576.6834.0645 1.0029.1212.3175.3346.583.607.7549.2727.172.5891.2416.9013.1975.3139-.044.6005-.1986.8263-.4402.1072-.1148.1954-.2424.2772-.3787.0902-.1503.1714-.3059.2535-.4612L6 19.4636c-.0896-.149-.9473-1.4704-2.887-.9218m20.5861-3.0056a1.4707 1.4707 0 0 0-.779-.5407c-.1476-.0425-.2961-.0616-.4483-.0705-.1678-.0099-.3352-.0091-.503-.0091H18.648l-4.3891-7.817c-.6655.7005-.9632 1.485-1.0773 2.1976-.1655 1.0333.0367 2.0934.546 3.0004l5.2741 9.3933c.084.1494.167.299.2591.4435.0837.131.1739.2537.2836.364.231.2323.5238.3809.8449.4232.3192.0424.643-.0244.9217-.1899.2784-.1653.4968-.4204.621-.7257.1246-.3072.146-.6425.0658-.9641-.0381-.1529-.0962-.2945-.165-.4346-.0753-.1543-.1598-.303-.2438-.4524l-1.216-2.1662h1.596c.1677 0 .3351.0009.5029-.009.1522-.009.3007-.028.4483-.0705a1.4707 1.4707 0 0 0 .779-.5407A1.5386 1.5386 0 0 0 24 16.452a1.539 1.539 0 0 0-.3009-.9158Z"
							/>
						</svg>
						App Store
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

					<div class="ml-10 flex flex-col items-start gap-3 md:flex-row">
						<input class="input" readonly value={$settings.connectionCode} />
						<button class="button" onclick={copyConnectionCode}>
							<Clipboard />
							<span class="copy">Copy</span>
						</button>
					</div>
				</div>
			{/if}
		</div>
	{/if}
</div>

<div class="transparent-900 m-10 mx-auto w-11/12 rounded-xl sm:w-full">
	{#if ($settings.remoteConnections && import.meta.env.VITE_CORES_MODE === "host") || import.meta.env.VITE_CORES_MODE === "client"}
		<div class="mx-10 flex flex-col gap-5 pb-10 pt-10 sm:mx-3 sm:flex-wrap">
			<!-- power options -->
			{#if import.meta.env.VITE_CORES_MODE === "client"}
				<div class="transparent-800 flex w-full flex-row flex-wrap items-center justify-between rounded-xl p-8 text-left sm:p-4">
					<div class="flex flex-col items-start gap-3">
						<div class="flex items-center gap-3">
							<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
								<Power />
							</div>
							<h2>Power</h2>
						</div>
						<h3>Left your computer on? You can turn it off remotely.</h3>
					</div>

					<div class="flex flex-col items-start gap-3 sm:my-5">
						<PowerButton {action} />
					</div>
				</div>
			{/if}

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
	import { ConnectionServer, getSettings, hardwareInfo, ModularDialog, setSettings, settings, Toggle, PowerButton } from "ui"
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
			const res = await fetch("http://localhost:5390/post", {
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

			if (res.status !== 200) {
				alert("Failed to send Wake On LAN packet. Please make sure the Cores service is running.")
			} else {
				alert(`Wake On LAN packet sent to ${item.mac} address.`)
			}
		},
		action = (type: string) => {},
	} = $props()
</script>
