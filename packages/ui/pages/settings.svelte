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
						{ value: 1, label: "1s" },
						{ value: 2, label: "2s" },
						{ value: 3, label: "3s" },
						{ value: 5, label: "5s" },
						{ value: 15, label: "15s" },
					]}
					setting={"interval"}
				/>
			</div>
		</div>
	</div>
</div>

<div class="transparent-900 m-10 mx-auto w-11/12 rounded-xl sm:w-full">
	<div class="mx-10 flex flex-col gap-5 pb-10 pt-10 sm:mx-3 sm:flex-wrap">
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
				<button class="button" on:click={debug}>
					<FileCog />
					Save
				</button>
			</div>
		</div>

		<!-- feedback -->
		<div class="transparent-800 flex w-full flex-row items-center justify-between rounded-xl p-8 text-left sm:p-4">
			<div class="flex flex-col items-start gap-3">
				<div class="flex items-center gap-3">
					<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
						<Megaphone />
					</div>
					<h2>Feedback</h2>
				</div>
				<h3>Thank you for providing feedback! Please report issues or feature requests on GitHub or by Email (cores@levminer.com).</h3>
			</div>

			<div class="flex flex-col items-start gap-3">
				<button
					class="button"
					on:click={() => {
						open("https://github.com/levminer/cores/issues")
					}}
				>
					<Github />
					GitHub
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
				<button on:click={about} class="button">
					<Info />
					About Cores
				</button>
			</div>
		</div>
	</div>
</div>

<script lang="ts">
	import { hardwareInfo } from "ui/stores/hardwareInfo.ts"
	import { settings } from "ui/stores/settings.ts"
	import build from "../../../build.json"
	import Select from "ui/components/select.svelte"
	import Toggle from "ui/components/toggle.svelte"
	import { Minimize2, RefreshCcw, Bug, Megaphone, Info, Cable, Github, FileCog } from "lucide-svelte"
	import { open } from "@tauri-apps/plugin-shell"
	import { message, save } from "@tauri-apps/plugin-dialog"
	import { invoke } from "@tauri-apps/api/core"

	const launchOnStartup = () => {
		// @ts-ignore
	}

	const about = async () => {
		interface userAgentData {
			fullVersionList?: { version: string }[]
		}

		let chromium_version = "N/A"

		// @ts-ignore
		const ua: userAgentData = await navigator.userAgentData.getHighEntropyValues([
			"architecture",
			"model",
			"platform",
			"platformVersion",
			"fullVersionList",
		])

		if (ua.fullVersionList !== undefined && ua.fullVersionList.length > 0) {
			chromium_version = ua.fullVersionList?.[0]?.version || "N/A"
		}

		const systemInfo: SystemInfo = await invoke("system_info")

		let dialogMessage = `Cores: ${build.version} \n\nTauri: ${systemInfo.tauriVersion} \nChromium: ${chromium_version}\n\nOS version: ${
			$hardwareInfo.system.os.name
		} \nHardware info: ${$hardwareInfo.cpu.name} ${Math.round(systemInfo.totalMem / 1024 / 1024 / 1024)} GB RAM\n\nRelease date: ${
			build.date
		} \nBuild number: ${build.number} \n\nCreated by: Lőrik Levente`

		message(dialogMessage)
	}

	const debug = async () => {
		const filePath = await save({
			filters: [
				{
					name: "Text file",
					extensions: ["txt"],
				},
			],
		})

		if (filePath) {
			await fetch("http://localhost:5390/post", {
				method: "POST",
				body: JSON.stringify({
					type: "debug_report",
					data: {
						filePath: filePath,
						systemInfo: `Cores: ${build.version}`,
					},
				}),
				headers: {
					"Content-Type": "application/json",
				},
			})

			// let name = `cores-debug-${new Date().toISOString().replace("T", "-").replaceAll(":", "-").substring(0, 19)}.txt`
		}
	}
</script>
