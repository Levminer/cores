<div class="transparent-900 m-10 mx-auto w-11/12 rounded-xl p-8 sm:w-full sm:p-4">
	<h1 class="mb-10">Remote connections</h1>

	<div class="mx-auto flex flex-col items-center justify-center gap-5 rounded-2xl">
		<div class="transparent-800 flex w-full flex-row items-center justify-between rounded-xl p-8 text-left sm:flex-col sm:p-4">
			<div>
				<h2>Connection code</h2>
				<h3>Use this code on the website (https://cores.levminer.com/settings).</h3>
			</div>
			<div class="ml-20 flex gap-3 sm:ml-0 sm:mt-4">
				<div class="flex flex-wrap items-center justify-center space-x-3 sm:items-start sm:justify-start">
					<input class="input" bind:value={$settings.connectionCode} />
					<button
						class="button"
						on:click={() => {
							sessionStorage.clear()
							location.href = "/home"
						}}
					>
						<RefreshCcw />
						<span class="copy">Connect</span>
					</button>
				</div>
			</div>
		</div>
	</div>
</div>

<script lang="ts">
	import { settings } from "ui/stores/settings.ts"
	import { RefreshCcw } from "lucide-svelte"
</script>
