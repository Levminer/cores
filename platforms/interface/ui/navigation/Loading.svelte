<div class="mx-auto flex h-screen w-full flex-col items-center justify-center">
	<div class="flex flex-row flex-wrap items-baseline justify-center gap-4">
		<div class="h-10 w-10 animate-bounce rounded-full bg-[#35cbfd] [animation-delay:-0.3s]" />
		<div class="h-10 w-10 animate-bounce rounded-full bg-[#ff5380] [animation-delay:-0.15s]" />
		<div class="h-10 w-10 animate-bounce rounded-full bg-[#9d0cfd]" />
	</div>

	<h1>Loading...</h1>
	<div class="webError transparent-900 m-3 my-10 hidden rounded-xl border-2 border-red-700 p-8 text-left">
		<h2 class="mb-5">App not loading?</h2>
		<ul class="list-inside list-disc">
			<li>The connection might be slow, try waiting a few seconds</li>
			<li>
				Try to <button class="underline" on:click={reload}>Refresh</button> this page
			</li>
			<li>Restart your Cores app on your computer</li>
			<li>
				Make sure the <button class="underline" on:click={connectionCode}>connection code</button> is correct
			</li>
			<li>
				Go back to the <a class="underline" href="/">home</a> page
			</li>
		</ul>
	</div>

	<div class="desktopError transparent-900 m-3 my-10 hidden rounded-xl border-2 border-red-700 p-8 text-left">
		<h2 class="mb-5">Cores service is not running</h2>
		<ul class="list-inside list-disc">
			<li>
				Make sure the Cores Service is running: <button class="underline" on:click={startService}>Launch service</button>
			</li>
			<li>
				Restart Cores: <button class="underline" on:click={reload}>Restart</button>
			</li>
		</ul>
	</div>
</div>

<script lang="ts">
	import { onMount } from "svelte"
	import { invoke } from "@tauri-apps/api/core"

	export let mode = "web" as "web" | "desktop"

	const reload = () => {
		sessionStorage.clear()
		location.reload()
	}

	const connectionCode = () => {
		location.href = "/home"
	}

	const startService = async () => {
		await invoke("start_service")
	}

	onMount(() => {
		let interval: NodeJS.Timeout

		if (mode === "desktop") {
			fetch("http://localhost:5390").catch(() => {
				document.querySelector(".desktopError").classList.remove("hidden")
			})
		} else {
			interval = setInterval(() => {
				document.querySelector(".webError").classList.remove("hidden")
			}, 5000)
		}

		return () => {
			clearInterval(interval)
		}
	})
</script>
