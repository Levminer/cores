<div class="flex items-center justify-center">
	<div class="my-5 inline-flex gap-3 rounded-xl bg-gradient-to-r from-purple-400 to-pink-600 p-3 text-lg font-medium backdrop-blur-sm">
		<button id="windows" class="px-3 py-3" on:click={() => select("windows")}>
			<p>Windows</p>
		</button>
		<button id="linux" class="px-3 py-3" on:click={() => select("linux")}>
			<p>Linux</p>
		</button>
		<button id="macos" class="px-3 py-3" on:click={() => select("macos")}>
			<p>macOS</p>
		</button>
	</div>
</div>

<script lang="ts">
	import { onMount } from "svelte"

	export let selected = "windows"

	const select = (name: string) => {
		console.log(name)
		document.querySelector("#windows")!.classList.remove("rounded-xl", "bg-white/30", "backdrop-blur-xl")
		document.querySelector("#linux")!.classList.remove("rounded-xl", "bg-white/30", "backdrop-blur-xl")
		document.querySelector("#macos")!.classList.remove("rounded-xl", "bg-white/30", "backdrop-blur-xl")

		document.querySelector(`#${name}`)!.classList.add("rounded-xl", "bg-white/30", "backdrop-blur-xl")

		selected = name
	}

	onMount(() => {
		select("windows")
	})
</script>
