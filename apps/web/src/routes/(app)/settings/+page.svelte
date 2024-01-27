<div class="transparent-900 m-10 mx-auto w-11/12 rounded-xl p-8 sm:w-full sm:p-4">
	<h1 class="mb-10">Remote connections</h1>

	<div class="mx-auto flex flex-col items-center justify-center gap-5 rounded-2xl">
		<div class="transparent-800 flex w-full flex-col items-start justify-start rounded-xl p-8 text-left sm:flex-col sm:p-4">
			<div>
				<h2>Add connection code</h2>
				<h3>Get your connection code from the Cores desktop app.</h3>
			</div>
			<div class="mt-4 flex w-full gap-3">
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

		<div class="transparent-800 flex w-full flex-col items-start justify-start rounded-xl p-8 text-left sm:flex-col sm:p-4">
			<div>
				<h2>Connection codes</h2>
				<h3>List of your connection codes.</h3>
			</div>
			<div class="mt-4 flex w-full flex-col gap-3">
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
			</div>
		</div>
	</div>
</div>

<div class="transparent-900 m-10 mx-auto w-11/12 rounded-xl p-8 sm:w-full sm:p-4">
	<h1 class="mb-10">About</h1>

	<div class="transparent-800 flex w-full flex-row items-center justify-between rounded-xl p-10 text-left">
		<div>
			<h2>About Cores</h2>
			<h3>Information about your Cores build and your computer.</h3>
		</div>
		<div class="ml-20 flex gap-3">
			<button
				on:click={() => {
					alert(`Cores ${version} \n\nRelease date: ${date} \nBuild number: ${number} \n\nCreated by Lőrik Levente`)
				}}
				class="button"
			>
				<svg
					xmlns="http://www.w3.org/2000/svg"
					width="24"
					height="24"
					viewBox="0 0 24 24"
					fill="none"
					stroke="currentColor"
					stroke-width="2"
					stroke-linecap="round"
					stroke-linejoin="round"
					><circle cx="12" cy="12" r="10" /><line x1="12" y1="16" x2="12" y2="12" /><line x1="12" y1="8" x2="12.01" y2="8" /></svg
				>
				About Cores
			</button>
		</div>
	</div>
</div>

<script lang="ts">
	import { settings } from "ui/stores/settings.ts"
	import { Plus, RefreshCcw, Trash2 } from "lucide-svelte"
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
