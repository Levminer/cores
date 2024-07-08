<div class="transparent-900 m-10 mx-auto w-11/12 rounded-xl sm:w-full">
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
				<h3>You can access and control your computer from any device with a web browser.</h3>
			</div>

			<div class="flex flex-col items-start gap-3">
				<Toggle bind:checked={$settings.remoteConnections} onChange={remoteConnections} />
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
						You can use this code on the website (https://cores.levminer.com) <br /> to monitor and control you device, keep it private.
					</h3>
				</div>

				<div class="flex flex-col items-start gap-3">
					<div class="flex items-center justify-center space-x-3">
						<input class="input" readonly value={$settings.connectionCode} />
						<button class="button" on:click={copyConnectionCode}>
							<Clipboard />
							<span class="copy">Copy</span>
						</button>
						<button
							class="button"
							on:click={() => {
								open(`https://cores.levminer.com/settings?connectionCode=${$settings.connectionCode}`)
							}}
						>
							<ExternalLink />
							<span class="copy">Open</span>
						</button>
					</div>
				</div>
			</div>
		{/if}
	</div>
</div>

<script lang="ts">
	import { Clipboard, ExternalLink, MonitorSmartphone, KeyRound } from "lucide-svelte"
	import Toggle from "ui/components/toggle.svelte"
	import { settings } from "ui/stores/settings.ts"
	import { invoke } from "@tauri-apps/api/core"
	import { open } from "@tauri-apps/plugin-shell"

	const remoteConnections = () => {
		invoke("restart_service")
	}

	const copyConnectionCode = () => {
		navigator.clipboard.writeText($settings.connectionCode)
		document.querySelector(".copy").innerHTML = "Copied"

		setTimeout(() => {
			document.querySelector(".copy").innerHTML = "Copy"
		}, 1000)
	}
</script>
