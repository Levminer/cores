<div class="mx-auto mb-20 flex min-h-screen w-full flex-col items-center justify-center">
	<h1 class="text-center">Select remote connection</h1>
	<div class="transparent-900 m-3 my-10 rounded-xl p-8 text-left">
		<h2 class="mb-5">Select remote connection</h2>
		<div class="flex flex-col gap-3">
			{#each $settings.connectionCodes as item}
				<div class="transparent-800 flex w-full flex-row items-center justify-between rounded-xl p-5">
					<div>
						<h4>{item.name}</h4>
						<h5>crs_********{item.code.slice(-2)}</h5>
					</div>
					<button
						on:click={() => {
							$settings.connectionCode = item.code
							connect()
							goto("/home")
						}}
					>
						<Plug />
					</button>
				</div>
			{/each}
			<a href="/settings" class="button">Add connection</a>
		</div>
	</div>
</div>

<script lang="ts">
	import { settings } from "ui/stores/settings.ts"
	import { Plug } from "lucide-svelte"
	import { goto } from "$app/navigation"

	export let connect = () => {}
</script>
