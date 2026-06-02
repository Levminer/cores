<div class="transparent-900 m-10 mx-auto w-11/12 rounded-xl sm:w-full">
	<div class="mx-10 flex flex-col gap-5 pb-10 pt-10 sm:mx-3 sm:flex-wrap">
		<!-- minimize to tray -->
		<div class="transparent-800 flex w-full flex-row items-center justify-between rounded-xl p-8 text-left sm:p-4">
			<div class="flex flex-col items-start gap-3">
				<div class="flex items-center gap-3">
					<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
						<Minimize2 />
					</div>
					<h2>Minimize to tray</h2>
				</div>
				<h3>When closing the app Cores will not quit. You can open Cores from the system tray.</h3>
			</div>

			<div class="flex flex-col items-start gap-3">
				<Toggle bind:checked={$settings.minimizeToTray} />
			</div>
		</div>

		<!-- refresh interval -->
		<div class="transparent-800 flex w-full flex-row items-center justify-between rounded-xl p-8 text-left sm:p-4">
			<div class="flex flex-col items-start gap-3">
				<div class="flex items-center gap-3">
					<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
						<RefreshCcw />
					</div>
					<h2>Refresh interval</h2>
				</div>
				<h3>How often does Cores refreshes the sensors and displays the data.</h3>
			</div>
			<div class="flex flex-col items-start gap-3">
				<Select
					options={[
						{ value: "1", label: "1s" },
						{ value: "2", label: "2s" },
						{ value: "3", label: "3s" },
						{ value: "5", label: "5s" },
						{ value: "15", label: "15s" },
					]}
					setting={"interval"}
				/>
			</div>
		</div>

		<!-- change colors -->
		<div class="transparent-800 flex w-full flex-row items-center justify-between rounded-xl p-8 text-left sm:p-4">
			<div class="flex flex-col items-start gap-3">
				<div class="flex items-center gap-3">
					<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
						<Palette />
					</div>
					<h2>Colors</h2>
				</div>
				<h3>You can customize the colors used by Cores.</h3>
			</div>
			<div class="flex flex-col items-start gap-3">
				<ModularDialog title={"Colors"} description={"You can customize the colors used by Cores."}>
					{#snippet openButton()}
						<Dialog.Trigger class="button">
							<Palette />
							Customize
						</Dialog.Trigger>
					{/snippet}
					<div class="w-full space-y-5">
						<div class="flex items-center justify-center gap-1 rounded-xl border-2 border-white p-3">
							<input type="color" bind:value={$localSettings.colors.min} />
							<input type="color" bind:value={$localSettings.colors.current} />
							<input type="color" bind:value={$localSettings.colors.max} />
							<input type="color" bind:value={$localSettings.colors.yellow} />
							<input type="color" bind:value={$localSettings.colors.orange} />
							{#each $localSettings.colors.categoricalPalette as item, i}
								<input type="color" bind:value={$localSettings.colors.categoricalPalette[i]} />
							{/each}
						</div>

						<div>
							<button
								onclick={() => {
									location.reload()
								}}
								class="smallButton w-full">Confirm</button
							>
						</div>
					</div>
				</ModularDialog>
			</div>
		</div>

		<!-- default devices -->
		<div class="transparent-800 flex w-full flex-row items-center justify-between rounded-xl p-8 text-left sm:p-4">
			<div class="flex flex-col items-start gap-3">
				<div class="flex items-center gap-3">
					<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
						<PcCase />
					</div>
					<h2>Default devices</h2>
				</div>
				<h3>You can change the default devices that are displayed on the main page.</h3>
			</div>
			<div class="flex flex-col items-start gap-3">
				<ModularDialog title={"Default Devices"} description={"You can change the default devices that are displayed on the main page."}>
					{#snippet openButton()}
						<Dialog.Trigger class="button">
							<PcCase />
							Change
						</Dialog.Trigger>
					{/snippet}
					<div class="w-full space-y-5">
						<div class="flex flex-col gap-1 p-3">
							<div>
								<h5 class="mb-1">Default storage device</h5>
								<select
									class="form-select w-full rounded-xl border-transparent text-black focus:border-transparent focus:ring-0"
									bind:value={$settings.defaultDevices.storage}
								>
									<option disabled selected>Select your option</option>
									{#each $hardwareInfo.system.storage.disks as item}
										<option value={item.id}>{item.name}</option>
									{/each}
								</select>
							</div>

							<div>
								<h5 class="mb-1">Default network interface</h5>
								<select
									class="form-select w-full rounded-xl border-transparent text-black focus:border-transparent focus:ring-0"
									bind:value={$settings.defaultDevices.network}
								>
									<option disabled selected>Select your option</option>
									{#each $hardwareInfo.system.network.interfaces as item}
										<option value={item.id}>{item.name}</option>
									{/each}
								</select>
							</div>

							<div>
								<h5 class="mb-1">Default GPU</h5>
								<select
									class="form-select w-full rounded-xl border-transparent text-black focus:border-transparent focus:ring-0"
									bind:value={$settings.defaultDevices.gpu}
								>
									<option disabled selected>Select your option</option>
									{#each $hardwareInfo.gpu.cards as item}
										<option value={item.id}>{item.name}</option>
									{/each}
								</select>
							</div>
						</div>

						<div>
							<button
								onclick={async () => {
									await invoke("restart_service")
									location.reload()
								}}
								class="smallButton w-full">Confirm</button
							>
						</div>
					</div>
				</ModularDialog>
			</div>
		</div>

		<!-- account -->
		<div class="transparent-800 flex w-full flex-row flex-wrap items-center justify-between rounded-xl p-8 text-left sm:p-4">
			<div class="flex flex-col items-start gap-3">
				<div class="flex items-center gap-3">
					<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
						<User />
					</div>
					<h2>Account</h2>
				</div>
				<h3>{user?.email ?? "Not logged in"}</h3>
			</div>

			{#if !loading}
				<div class="flex flex-col items-start gap-3 sm:my-5">
					{#if user?.email}
						<button
							onclick={async () => {
								await supabaseClient.auth.signOut({ scope: "local" })
								location.href = "/onboarding"
							}}
							class="button"
						>
							<LogOut />
							Log out
						</button>
					{:else}
						<button
							onclick={async () => {
								await supabaseClient.auth.signOut({ scope: "local" })
								goto("/onboarding")
							}}
							class="button"
						>
							<User />
							Log in
						</button>
					{/if}
				</div>
			{/if}
		</div>
	</div>
</div>

<div class="transparent-900 m-10 mx-auto w-11/12 rounded-xl sm:w-full">
	<div class="mx-10 flex flex-col gap-5 pb-10 pt-10 sm:mx-3 sm:flex-wrap">
		<!-- feedback -->
		<div class="transparent-800 flex w-full flex-row items-center justify-between rounded-xl p-8 text-left sm:p-4">
			<div class="flex flex-col items-start gap-3">
				<div class="flex items-center gap-3">
					<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
						<Megaphone />
					</div>
					<h2>Feedback</h2>
				</div>
				<h3>Feedback is always welcome! Report issues or request features.</h3>
			</div>

			<div class="flex flex-col items-start gap-3">
				<ModularDialog title={"Feedback"} description={"Feedback is always welcome! Report issues or request features."}>
					{#snippet openButton()}
						<Dialog.Trigger class="button">
							<Megaphone />
							Feedback
						</Dialog.Trigger>
					{/snippet}
					<div class="w-full space-y-5">
						<div class="rounded-xl border-2 border-white p-3">
							<h5 class="mb-3">Join the community on Discord</h5>
							<button
								onclick={() => {
									open("https://link.levminer.com/crs-dc")
								}}
								class="smallButton w-full">Discord</button
							>
						</div>

						<div class="rounded-xl border-2 border-white p-3">
							<h5 class="mb-3">Report Issues or Request Features</h5>
							<button
								onclick={() => {
									open("https://github.com/levminer/cores/issues")
								}}
								class="smallButton w-full">Open GitHub</button
							>
						</div>

						<div class="rounded-xl border-2 border-white p-3">
							<h5 class="mb-3">Send an email to support@coresmonitor.com</h5>
							<button
								onclick={() => {
									open("mailto:feedback@coresmonitor.com")
								}}
								class="smallButton w-full">Send Email</button
							>
						</div>
					</div>
				</ModularDialog>
			</div>
		</div>

		<!-- debug report -->
		<div class="transparent-800 flex w-full flex-row items-center justify-between rounded-xl p-8 text-left sm:p-4">
			<div class="flex flex-col items-start gap-3">
				<div class="flex items-center gap-3">
					<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
						<Bug />
					</div>
					<h2>Debug report</h2>
				</div>
				<h3>Debug information about your computer. Include this report with your feedback.</h3>
			</div>

			<div class="flex flex-col items-start gap-3">
				<button class="button" onclick={debug}>
					<FileCog />
					Save
				</button>
			</div>
		</div>

		<!-- about -->
		<div class="transparent-800 flex w-full flex-row items-center justify-between rounded-xl p-8 text-left sm:p-4">
			<div class="flex flex-col items-start gap-3">
				<div class="flex items-center gap-3">
					<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
						<Info />
					</div>
					<h2>About Cores</h2>
				</div>
				<h3>Information about your Cores build and your computer.</h3>
			</div>

			<div class="flex flex-col items-start gap-3">
				<button onclick={about} class="button">
					<Info />
					About Cores
				</button>
			</div>
		</div>
	</div>
</div>

<script lang="ts">
	import build from "../../../../build.json"
	import type { User as SupabaseUser } from "@supabase/supabase-js"
	import { Minimize2, RefreshCcw, Bug, Megaphone, Info, FileCog, User, LogOut, Palette, PcCase } from "lucide-svelte"
	import { open } from "@tauri-apps/plugin-shell"
	import { message, save } from "@tauri-apps/plugin-dialog"
	import { invoke } from "@tauri-apps/api/core"
	import { supabaseClient } from "../utils/supabase.ts"
	import { onMount } from "svelte"
	import { hardwareInfo, localSettings, ModularDialog, Select, settings, Toggle } from "ui"
	import { Dialog } from "bits-ui"

	interface Props {
		goto: (path: string) => void
	}

	let { goto }: Props = $props()

	let user = $state<SupabaseUser | null>(null)
	let loading = $state(true)

	onMount(async () => {
		const { data, error } = await supabaseClient.auth.getUser()

		if (!error && data.user) {
			user = data.user
		}

		loading = false
	})

	const launchOnStartup = () => {
		// @ts-ignore
	}

	const about = async () => {
		interface userAgentData {
			fullVersionList?: { version: string }[]
		}

		let runtimeVersion = navigator.userAgent

		try {
			// @ts-ignore
			const ua: userAgentData = await navigator.userAgentData.getHighEntropyValues([
				"architecture",
				"model",
				"platform",
				"platformVersion",
				"fullVersionList",
			])

			if (ua.fullVersionList !== undefined && ua.fullVersionList.length > 0) {
				// @ts-ignore
				runtimeVersion = ua.fullVersionList.filter((item) => item.brand === "Chromium")[0]?.version || "N/A"
			}
		} catch (error) {
			console.log(error)
		}

		const systemInfo: SystemInfo = await invoke("system_info")

		let dialogMessage = `Cores: ${build.version} \n\nTauri: ${systemInfo.tauriVersion} \nRuntime: ${runtimeVersion}\n\nOS version: ${
			$hardwareInfo.system.os.name
		} \nHardware info: ${$hardwareInfo.cpu.name} ${Math.round(systemInfo.totalMem / 1024 / 1024 / 1024)} GB RAM\n\nRelease date: ${
			build.date
		} \nBuild number: ${build.number} \n\nCreated by: Lőrik Levente`

		message(dialogMessage)
	}

	const debug = async () => {
		await fetch("http://localhost:5390/post", {
			method: "POST",
			body: JSON.stringify({
				type: "debug_report",
				data: {
					systemInfo: `Cores: ${build.version} (${build.number})`,
				},
			}),
			headers: {
				"Content-Type": "application/json",
				"Authorization": `Bearer ${$settings.connectionCode}`,
			},
		})
	}
</script>
