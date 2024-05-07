<div class="mx-auto flex h-screen w-full flex-col items-center justify-center">
	<div class="flex flex-row flex-wrap items-baseline justify-center gap-4">
		<div class="h-10 w-10 animate-bounce rounded-full bg-[#35cbfd] [animation-delay:-0.3s]" />
		<div class="h-10 w-10 animate-bounce rounded-full bg-[#ff5380] [animation-delay:-0.15s]" />
		<div class="h-10 w-10 animate-bounce rounded-full bg-[#9d0cfd]" />
	</div>

	<h1>Loading...</h1>
	<div class="error transparent-900 m-3 my-10 hidden rounded-xl border-2 border-red-700 p-8 text-left">
		<h2 class="mb-5">Cores service is not running</h2>
		<ul class="list-inside list-disc">
			<li>
				Make sure the Cores Service is running: <button class="underline" on:click={startService}>Launch service</button>
			</li>
		</ul>
	</div>
</div>

<script lang="ts">
	import { onMount } from "svelte"
	import { invoke } from "@tauri-apps/api/core"

	const startService = async () => {
		await invoke("start_service")
	}

	onMount(async () => {
		try {
			// Check if service is running
			await fetch("http://localhost:5390/status")
		} catch (error) {
			document.querySelector(".error").classList.remove("hidden")
		}
	})
</script>
