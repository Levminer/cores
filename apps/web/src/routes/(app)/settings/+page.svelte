<div class="transparent-900 m-10 mx-auto w-11/12 rounded-xl sm:w-full">
	<div class="mx-10 flex flex-col gap-5 pb-10 pt-10 sm:mx-3 sm:flex-wrap">
		<!-- remote connections -->
		<div class="transparent-800 flex w-full flex-row flex-wrap items-center justify-between rounded-xl p-8 text-left sm:p-4">
			<div class="flex items-center gap-3">
				<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
					<Cable />
				</div>
				<h2>Remote connections</h2>
			</div>
			<div class="mt-5 flex w-full flex-col gap-5">
				<h3>Get your connection code from the Cores desktop app.</h3>
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

							<button
								class="button mt-6"
								on:click={() => {
									$settings.connectionCode = item.code
									sessionStorage.clear()
									location.href = "/home"
								}}
							>
								<RefreshCcw />
								<span>Connect</span>
							</button>
						</div>
					</div>
				{/each}
				<div class="flex w-full flex-row flex-wrap items-center justify-between gap-3">
					<div class="flex flex-row flex-wrap gap-3">
						<div>
							<h5>Name</h5>
							<input class="input mt-1" type="text" id="name" />
						</div>

						<div>
							<h5>Connection code</h5>
							<input class="input mt-1" type="text" id="code" />
						</div>
					</div>

					<button class="button mt-6" on:click={addConnectionCode}>
						<Plus />
						<span>Add</span>
					</button>
				</div>
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
	import { Plus, RefreshCcw, Trash2, Cable, Info } from "lucide-svelte"
	import { version, number, date } from "../../../../../../build.json"

	const addConnectionCode = () => {
		const name = document.getElementById("name") as HTMLInputElement
		const code = document.getElementById("code") as HTMLInputElement

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
