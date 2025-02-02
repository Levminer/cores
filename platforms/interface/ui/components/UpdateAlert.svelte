{#if $state.updateAvailable}
	<div class="updateAlert bg-popup-blue z-10 w-full">
		<div class="container mx-auto flex items-center justify-between px-6 py-4">
			<div class="flex items-center justify-center gap-3">
				<svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
					<path
						stroke-linecap="round"
						stroke-linejoin="round"
						stroke-width="2"
						d="M15 13l-3 3m0 0l-3-3m3 3V8m0 13a9 9 0 110-18 9 9 0 010 18z"
					/>
				</svg>

				<p class="updateText mx-1 text-lg font-bold">Downloading update... {progress}</p>

				<button type="button" class="smallButton" on:click={showReleaseNotes}>
					<svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
						<path
							stroke-linecap="round"
							stroke-linejoin="round"
							stroke-width="2"
							d="M19 20H5a2 2 0 01-2-2V6a2 2 0 012-2h10a2 2 0 012 2v1m2 13a2 2 0 01-2-2V7m2 13a2 2 0 002-2V9a2 2 0 00-2-2h-2m-4-3H9M7 16h6M7 8h6v4H7V8z"
						/>
					</svg>
					<span>Release notes</span>
				</button>
			</div>

			<button class="updateClose transform duration-200 hover:text-black" on:click={hidePopup}>
				<svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
					<path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
				</svg>
			</button>
		</div>
	</div>
{/if}

<script lang="ts">
	import { onMount } from "svelte"
	import { check } from "@tauri-apps/plugin-updater"
	import { state } from "../stores/state"
	import { open } from "@tauri-apps/plugin-shell"
	import { relaunch } from "@tauri-apps/plugin-process"
	import { ask } from "@tauri-apps/plugin-dialog"
	import build from "../../../../build.json"

	$: progress = ""

	onMount(async () => {
		if (!build.dev) {
			const update = await check()

			console.log(update)

			if (update?.available) {
				$state.updateAvailable = true

				const result = await ask("A new version of Cores is available. Do you want to update?", {
					title: "Cores update available",
				})

				let downloaded = 0
				let contentLength = 0

				if (result) {
					await update.downloadAndInstall((event) => {
						switch (event.event) {
							case "Started":
								contentLength = event.data.contentLength
								console.log(`started downloading ${event.data.contentLength} bytes`)
								break
							case "Progress":
								downloaded += event.data.chunkLength
								console.log(`downloaded ${downloaded} from ${contentLength}`)
								break
							case "Finished":
								console.log("download finished")
								break
						}

						progress = `${Math.round((downloaded / contentLength) * 100)}%`
					})

					await relaunch()
				}
			}
		}
	})

	const showReleaseNotes = () => {
		open("https://github.com/levminer/cores/releases/latest")
	}

	const hidePopup = () => {
		const popup = document.querySelector(".updateAlert") as HTMLDivElement
		popup.style.display = "none"
	}
</script>
