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
				<Select options={["1s", "2s", "3s", "5s", "15s"]} setting={"interval"} values={[1, 2, 3, 5, 15]} />
			</div>
		</div>
	</div>
</div>

<div class="transparent-900 m-10 mx-auto w-11/12 rounded-xl sm:w-full">
	<div class="mx-10 flex flex-col gap-5 pb-10 pt-10 sm:mx-3 sm:flex-wrap">
		<!-- connection code -->
		<div class="transparent-800 flex w-full flex-row items-center justify-between rounded-xl p-8 text-left sm:p-4">
			<div class="flex flex-col items-start gap-3">
				<div class="flex items-center gap-3">
					<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
						<Cable />
					</div>
					<h2>Connection code</h2>
				</div>
				<h3>Use this code on the website (https://cores.levminer.com/settings).</h3>
			</div>

			<div class="flex flex-col items-start gap-3">
				<div class="flex items-center justify-center space-x-3">
					<input class="input" readonly value={$settings.connectionCode} />
					<button class="button" on:click={copyConnectionCode}>
						<Clipboard />
						<span class="copy">Copy</span>
					</button>
				</div>
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
	import { Clipboard, Minimize2, RefreshCcw, Bug, Megaphone, Info, Cable, Github, FileCog } from "lucide-svelte"
	import { open } from "@tauri-apps/plugin-shell"
	import { message } from "@tauri-apps/plugin-dialog"
	import { invoke } from "@tauri-apps/api/core"

	const launchOnStartup = () => {
		// @ts-ignore
	}

	const about = async () => {
		// @ts-ignore
		const ua = await navigator.userAgentData.getHighEntropyValues(["architecture", "model", "platform", "platformVersion", "fullVersionList"])

		console.log(ua)

		const systemInfo = (await invoke("system_info")) as {
			tauriVersion: string
			osName: string
			osVersion: string
			osArch: string
			cpuName: string
			totalMem: number
		}

		console.log(systemInfo)

		let dialogMessage = `Cores: ${build.version} \n\nTauri: ${systemInfo.tauriVersion} \nChromium: ${
			ua.fullVersionList[0].version
		}\n\nOS version: ${$hardwareInfo.system.os.name} \nHardware info: ${$hardwareInfo.cpu.name} ${Math.round(
			$hardwareInfo.ram.load[0].value + $hardwareInfo.ram.load[1].value,
		)} GB RAM\n\nRelease date: ${build.date} \nBuild number: ${build.number} \n\nCreated by: Lőrik Levente`

		message(dialogMessage)
	}

	const debug = async () => {
		// let name = `cores-debug-${new Date().toISOString().replace("T", "-").replaceAll(":", "-").substring(0, 19)}.txt`

		// @ts-ignore
		alert("Debug report saved to the desktop")
	}

	const copyConnectionCode = () => {
		navigator.clipboard.writeText($settings.connectionCode)
		document.querySelector(".copy").innerHTML = "Copied"

		setTimeout(() => {
			document.querySelector(".copy").innerHTML = "Copy"
		}, 1000)
	}
</script>
