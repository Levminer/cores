<div class="transparent-900 m-10 mx-auto w-11/12 rounded-xl sm:w-full">
	<div class="mx-10 flex flex-col gap-5 pb-10 pt-10 sm:mx-3 sm:flex-wrap">
		<!-- remote connections -->
		<div class="transparent-800 flex w-full flex-row flex-wrap items-center justify-between rounded-xl p-8 text-left sm:p-4">
			<div class="flex flex-col items-start gap-3">
				<div class="flex items-center gap-3">
					<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
						<Cable />
					</div>
					<h2>Remote connections</h2>
				</div>
				<h3>Get your connection code from the Cores desktop app.</h3>
			</div>

			<div class="flex flex-col items-start gap-3 sm:my-5">
				<Dialog
					open={dialogOpen}
					title={"Add Remote Connection"}
					description={"You can get your connection code from the Cores desktop app."}
					action={addConnectionCode}
				>
					<div class="flex flex-col flex-wrap gap-3">
						<div>
							<h5>Name <span class="text-red-500">*</span></h5>
							<input placeholder="My Home PC" class="input mt-1" type="text" id="name" />
						</div>

						<div>
							<h5>Connection code <span class="text-red-500">*</span></h5>
							<input placeholder="crs_abcde12345" class="input mt-1" type="text" id="code" />
						</div>

						<div>
							<h5>MAC address</h5>
							<input placeholder="AA:BB:CC:DD:EE:FF" class="input mt-1" type="text" id="mac" />
						</div>
					</div>
				</Dialog>
			</div>

			<div class="mt-5 flex w-full flex-col gap-5">
				{#each $settings.connectionCodes as item}
					<div class="flex w-full flex-row flex-wrap items-center justify-between gap-3">
						<div class="flex flex-row flex-wrap gap-3">
							<div>
								<h5>Name</h5>
								<input class="input mt-1" type="text" value={item.name} readonly />
							</div>

							<div>
								<h5>Connection code</h5>
								<input class="input mt-1" type="text" value={item.code} readonly />
							</div>
						</div>

						<div class="flex flex-row flex-wrap gap-3">
							<button class="button mt-6" on:click={() => deleteConnectionCode(item.code)}>
								<Trash2 />
								<span>Delete</span>
							</button>
						</div>
					</div>
				{/each}
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
	import Dialog from "ui/components/dialog.svelte"
	import { Trash2, Cable, Info } from "lucide-svelte"
	import { version, number, date } from "../../../../../../build.json"
	import { onMount } from "svelte"

	$: dialogOpen = false

	onMount(() => {
		// get connectionCode from url params
		const urlParams = new URLSearchParams(window.location.search)
		const connectionCode = urlParams.get("connectionCode")
		const name = urlParams.get("name") || "My Home PC"
		const mac = urlParams.get("mac") || ""

		console.log({ connectionCode, name, mac })

		if (connectionCode) {
			dialogOpen = true

			setTimeout(() => {
				const nameInput = document.getElementById("name") as HTMLInputElement
				const codeInput = document.getElementById("code") as HTMLInputElement
				const macInput = document.getElementById("mac") as HTMLInputElement

				console.log(nameInput, codeInput, macInput)

				nameInput.value = name
				codeInput.value = connectionCode
				macInput.value = mac
			}, 250)
		}
	})

	const addConnectionCode = () => {
		const name = document.getElementById("name") as HTMLInputElement
		const code = document.getElementById("code") as HTMLInputElement

		if (name.value === "") {
			return alert("Please enter a name for your connection")
		}

		if (!code.value.startsWith("crs_")) {
			return alert("Invalid connection code! The connection code must start with: crs_")
		}

		$settings.connectionCodes = [
			...$settings.connectionCodes,
			{
				name: name.value,
				code: code.value,
			},
		]

		name.value = ""
		code.value = ""
	}

	const deleteConnectionCode = (code: string) => {
		$settings.connectionCodes = $settings.connectionCodes.filter((item) => item.code !== code)
	}
</script>
