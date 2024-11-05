<div class="transparent-900 m-10 mx-auto w-11/12 rounded-xl sm:w-full">
	<div class="mx-10 flex flex-col gap-5 pb-10 pt-10 sm:mx-3 sm:flex-wrap">
		<!-- remote connections link -->
		<div class="hidden">
			<ModularDialog
				open={dialogOpen}
				title={"Add Remote Connection"}
				description={"You can get your connection code from the Cores desktop app."}
			>
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

		<!-- feedback -->
		<div class="transparent-800 flex w-full flex-row flex-wrap items-center justify-between rounded-xl p-8 text-left sm:p-4">
			<div class="flex flex-col items-start gap-3">
				<div class="flex items-center gap-3">
					<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
						<Megaphone />
					</div>
					<h2>Feedback</h2>
				</div>
				<h3>Thank you for providing feedback! Please report issues or feature requests on GitHub or by Email (cores@levminer.com).</h3>
			</div>

			<div class="flex flex-col items-start gap-3 sm:my-5">
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
		<div class="transparent-800 flex w-full flex-row flex-wrap items-center justify-between rounded-xl p-8 text-left sm:p-4">
			<div class="flex flex-col items-start gap-3">
				<div class="flex items-center gap-3">
					<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
						<Info />
					</div>
					<h2>About Cores</h2>
				</div>
				<h3>Information about your Cores build and your computer.</h3>
			</div>

			<div class="flex flex-col items-start gap-3 sm:my-5">
				<button
					on:click={() => {
						alert(`Cores ${version} \n\nRelease date: ${date} \nBuild number: ${number} \n\nCreated by Lőrik Levente`)
					}}
					class="button"
				>
					<Info />
					About Cores
				</button>
			</div>
		</div>
	</div>
</div>

<script lang="ts">
	import { settings } from "ui/stores/settings.ts"
	import { Info, Plus, Megaphone, Github } from "lucide-svelte"
	import { version, number, date } from "../../../../../../../build.json"
	import { onMount } from "svelte"
	import ModularDialog from "ui/components/modularDialog.svelte"
	import { Dialog } from "bits-ui"
	import { addConnectionCode } from "ui/utils/connection.ts"

	$: dialogOpen = false

	onMount(() => {
		// get connectionCode from url params
		const urlParams = new URLSearchParams(window.location.search)
		let connectionCode = urlParams.get("connectionCode")
		const name = urlParams.get("name") || "My Home PC"

		console.log({ connectionCode, name })

		if (connectionCode) {
			dialogOpen = true

			setTimeout(() => {
				const nameInput = document.getElementById("name") as HTMLInputElement
				const codeInput = document.getElementById("code") as HTMLInputElement

				console.log(nameInput, codeInput)

				nameInput.value = name
				codeInput.value = connectionCode
			}, 250)
		}
	})
</script>
