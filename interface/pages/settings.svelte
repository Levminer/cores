<div class="transparent-900 m-10 mx-auto w-4/5 rounded-xl p-10 sm:m-3 sm:w-full">
	<h1 class="mb-10">General</h1>

	<div class="mx-auto flex flex-col items-center justify-center gap-5 rounded-2xl">
		<div class="transparent-800 flex w-full flex-row items-center justify-between rounded-xl p-10 text-left">
			<div>
				<h2>Refresh interval</h2>
				<h3>How often does Cores refreshes the sensors and displays the data. Restart required.</h3>
			</div>
			<div class="ml-20 flex gap-3">
				<Select options={["1s", "2s", "3s", "5s", "15s"]} setting={"interval"} values={[1, 2, 3, 5, 15]} />
			</div>
		</div>
	</div>
</div>

<div class="transparent-900 m-10 mx-auto w-4/5 rounded-xl p-10 sm:m-3 sm:w-full">
	<h1 class="mb-10">About</h1>

	<div class="mx-auto flex flex-col items-center justify-center gap-5 rounded-2xl">
		<div class="transparent-800 flex w-full flex-row items-center justify-between rounded-xl p-10 text-left">
			<div>
				<h2>Feedback</h2>
				<h3>Thank you for providing feedback! Please report issues or feature requests on GitHub or by Email (cores@levminer.com).</h3>
			</div>
			<div class="ml-20 flex gap-3">
				<button
					class="button"
					on:click={() => {
						open("https://github.com/levminer/cores/issues")
					}}
				>
					<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M15 22v-4a4.8 4.8 0 0 0-1-3.5c3 0 6-2 6-5.5.08-1.25-.27-2.48-1-3.5.28-1.15.28-2.35 0-3.5 0 0-1 0-3 1.5-2.64-.5-5.36-.5-8 0C6 2 5 2 5 2c-.3 1.15-.3 2.35 0 3.5A5.403 5.403 0 0 0 4 9c0 3.5 3 5.5 6 5.5-.39.49-.68 1.05-.85 1.65-.17.6-.22 1.23-.15 1.85v4" /><path d="M9 18c-4.51 2-5-2-7-2" /></svg>
					GitHub
				</button>
			</div>
		</div>

		<div class="transparent-800 flex w-full flex-row items-center justify-between rounded-xl p-10 text-left">
			<div>
				<h2>About Cores</h2>
				<h3>Information about your Cores build and your computer.</h3>
			</div>
			<div class="ml-20 flex gap-3">
				<button on:click={about} class="button">
					<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><circle cx="12" cy="12" r="10" /><line x1="12" y1="16" x2="12" y2="12" /><line x1="12" y1="8" x2="12.01" y2="8" /></svg>
					About Cores
				</button>
			</div>
		</div>
	</div>
</div>

<script lang="ts">
	import { hardwareInfo } from "../stores/hardwareInfo"
	import build from "../../build.json"
	import Select from "../components/select.svelte"

	const about = () => {
		let usedRAM = $hardwareInfo.ram.load[0].value
		let availableRAM = $hardwareInfo.ram.load[1].value

		let RAM = `${Math.round(usedRAM + availableRAM)} GB`

		let message = `Cores: ${$hardwareInfo.system.os.app} \n\nRuntime: ${$hardwareInfo.system.os.runtime} \nChromium: ${$hardwareInfo.system.os.webView}\n\nOS version: ${$hardwareInfo.system.os.name} \nHardware info: ${$hardwareInfo.cpu.name} ${RAM} RAM\n\nRelease date: ${build.date} \nBuild number: ${build.number} \n\nCreated by: Lőrik Levente`

		// @ts-ignore
		window.chrome.webview.postMessage({ name: "about", content: message })
	}
</script>
