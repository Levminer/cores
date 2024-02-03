{#if showTips}
	<div class="mx-auto flex h-screen w-full flex-col items-center justify-center">
		<div class="flex flex-row flex-wrap items-baseline justify-center gap-4">
			<div class="h-10 w-10 animate-bounce rounded-full bg-[#35cbfd] [animation-delay:-0.3s]" />
			<div class="h-10 w-10 animate-bounce rounded-full bg-[#ff5380] [animation-delay:-0.15s]" />
			<div class="h-10 w-10 animate-bounce rounded-full bg-[#9d0cfd]" />
		</div>
		{#if showTips}
			<h1>Loading...</h1>
			<div class="transparent-900 m-3 my-10 rounded-xl p-8 text-left">
				<h2 class="mb-5">App not loading?</h2>
				<ul class="list-inside list-disc">
					<li>
						Try to <button class="underline" on:click={reload}>Refresh</button> this page
					</li>
					<li>Restart your Cores app</li>
					<li>
						Make sure you typed in the <button class="underline" on:click={connectionCode}>connection code</button> in the settings right
					</li>
					<li>
						Go back to the <a class="underline" href="/">home</a> page
					</li>
				</ul>
			</div>
		{/if}
	</div>
{/if}

<script lang="ts">
	import { onMount } from "svelte"

	export let showTips = false

	onMount(() => {
		setTimeout(() => {
			showTips = true
		}, 1000)
	})

	const reload = () => {
		location.reload()
	}

	const connectionCode = () => {
		location.href = "/settings"
	}
</script>
