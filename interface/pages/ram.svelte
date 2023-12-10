<div class="transparent-900 m-10 mx-auto w-11/12 rounded-xl sm:m-3 sm:w-full">
	<div class="mx-10 flex flex-col gap-5 pb-10 pt-10">
		<div class="flex w-full flex-row items-start justify-start gap-5">
			<!-- ram info -->
			<div class="transparent-800 w-3/5 rounded-xl p-10 text-left">
				<div class="mb-5 flex items-baseline gap-3">
					<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" viewBox="0 0 16 16">
						<path
							d="M1 3a1 1 0 0 0-1 1v8a1 1 0 0 0 1 1h4.586a1 1 0 0 0 .707-.293l.353-.353a.5.5 0 0 1 .708 0l.353.353a1 1 0 0 0 .707.293H15a1 1 0 0 0 1-1V4a1 1 0 0 0-1-1H1Zm.5 1h3a.5.5 0 0 1 .5.5v4a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-4a.5.5 0 0 1 .5-.5Zm5 0h3a.5.5 0 0 1 .5.5v4a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-4a.5.5 0 0 1 .5-.5Zm4.5.5a.5.5 0 0 1 .5-.5h3a.5.5 0 0 1 .5.5v4a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-4ZM2 10v2H1v-2h1Zm2 0v2H3v-2h1Zm2 0v2H5v-2h1Zm3 0v2H8v-2h1Zm2 0v2h-1v-2h1Zm2 0v2h-1v-2h1Zm2 0v2h-1v-2h1Z"
						/>
					</svg>
					<h2>RAM Modules</h2>
				</div>
				{#each $hardwareInfo.ram.info as { manufacturerName, configuredSpeed, configuredVoltage, size, bankLocator }}
					<div class="mt-5 select-text">
						<h3>Vendor: {manufacturerName}</h3>
						<h3>Speed: {configuredSpeed} MHz</h3>
						<h3>Voltage: {configuredVoltage / 1000} V</h3>
						<h3>Capacity: {size / 1024} GB</h3>
					</div>
				{/each}
			</div>

			<!-- ram usage -->
			<div class="transparent-800 w-2/5 rounded-xl p-10">
				<div class="mb-5 flex items-baseline gap-3">
					<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="bi bi-motherboard" viewBox="0 0 16 16">
						<path
							d="M11.5 2a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 .5-.5Zm2 0a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 .5-.5Zm-10 8a.5.5 0 0 0 0 1h6a.5.5 0 0 0 0-1h-6Zm0 2a.5.5 0 0 0 0 1h6a.5.5 0 0 0 0-1h-6ZM5 3a1 1 0 0 0-1 1h-.5a.5.5 0 0 0 0 1H4v1h-.5a.5.5 0 0 0 0 1H4a1 1 0 0 0 1 1v.5a.5.5 0 0 0 1 0V8h1v.5a.5.5 0 0 0 1 0V8a1 1 0 0 0 1-1h.5a.5.5 0 0 0 0-1H9V5h.5a.5.5 0 0 0 0-1H9a1 1 0 0 0-1-1v-.5a.5.5 0 0 0-1 0V3H6v-.5a.5.5 0 0 0-1 0V3Zm0 1h3v3H5V4Zm6.5 7a.5.5 0 0 0-.5.5v1a.5.5 0 0 0 .5.5h2a.5.5 0 0 0 .5-.5v-1a.5.5 0 0 0-.5-.5h-2Z"
						/>
						<path
							d="M1 2a2 2 0 0 1 2-2h11a2 2 0 0 1 2 2v11a2 2 0 0 1-2 2H3a2 2 0 0 1-2-2v-2H.5a.5.5 0 0 1-.5-.5v-1A.5.5 0 0 1 .5 9H1V8H.5a.5.5 0 0 1-.5-.5v-1A.5.5 0 0 1 .5 6H1V5H.5a.5.5 0 0 1-.5-.5v-2A.5.5 0 0 1 .5 2H1Zm1 11a1 1 0 0 0 1 1h11a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1H3a1 1 0 0 0-1 1v11Z"
						/>
					</svg>
					<h2>RAM Layout</h2>
				</div>
				{#each $hardwareInfo.ram.layout as { manufacturerName, partNumber, size, deviceLocator, bankLocator }}
					{#if size == 0}
						<div class="mt-5 select-text">
							<h3>Module: Not detected</h3>
							<h3>Controller: {deviceLocator}</h3>
							<h3>Bank: {bankLocator}</h3>
						</div>
					{:else}
						<div class="mt-5 select-text">
							<h3>Module: {manufacturerName} ({partNumber})</h3>
							<h3>Controller: {deviceLocator}</h3>
							<h3>Bank: {bankLocator}</h3>
						</div>
					{/if}
				{/each}
			</div>
		</div>

		<div class="flex w-full flex-row justify-start gap-5 sm:flex-wrap">
			<!-- ram usage -->
			<div class="transparent-800 w-1/2 rounded-xl p-10 text-left sm:w-full">
				<div class="mb-5 flex items-baseline justify-between">
					<div class="flex items-baseline gap-3">
						<svg
							xmlns="http://www.w3.org/2000/svg"
							width="24"
							height="24"
							viewBox="0 0 24 24"
							fill="none"
							stroke="currentColor"
							stroke-width="2"
							stroke-linecap="round"
							stroke-linejoin="round"><path d="M3 3v18h18" /><path d="m19 9-5 5-4-4-3 3" /></svg
						>
						<h2>RAM usage</h2>
					</div>
					<div>
						<h5 class="cursor-pointer" on:click={() => (minutes = !minutes)}>{minutes ? "Last 60 minutes" : "Last 60 seconds"}</h5>
					</div>
				</div>
				<div>
					<UsageChart
						statistics={minutes
							? $hardwareStatistics.minutes.map((value) => value.ram.physicalUsage)
							: $hardwareStatistics.seconds.map((value) => value.ram.physicalUsage)}
						type={"Usage"}
						unit={"%"}
						time={minutes ? "m" : "s"}
					/>
				</div>
			</div>

			<!-- virtual ram usage -->
			<div class="transparent-800 w-1/2 rounded-xl p-10 text-left sm:w-full">
				<div class="mb-5 flex items-baseline justify-between">
					<div class="flex items-baseline gap-3">
						<svg
							xmlns="http://www.w3.org/2000/svg"
							width="24"
							height="24"
							viewBox="0 0 24 24"
							fill="none"
							stroke="currentColor"
							stroke-width="2"
							stroke-linecap="round"
							stroke-linejoin="round"><path d="M3 3v18h18" /><path d="m19 9-5 5-4-4-3 3" /></svg
						>
						<h2>Virtual RAM usage</h2>
					</div>
					<div>
						<h5 class="cursor-pointer" on:click={() => (minutes = !minutes)}>{minutes ? "Last 60 minutes" : "Last 60 seconds"}</h5>
					</div>
				</div>
				<div>
					<UsageChart
						statistics={minutes
							? $hardwareStatistics.minutes.map((value) => value.ram.virtualUsage)
							: $hardwareStatistics.seconds.map((value) => value.ram.virtualUsage)}
						type={"Usage"}
						unit={"%"}
						time={minutes ? "m" : "s"}
					/>
				</div>
			</div>
		</div>
	</div>
</div>

<script lang="ts">
	import TemperatureChart from "../components/multiLineChart.svelte"
	import LineChart from "../components/lineChart.svelte"
	import UsageChart from "../components/usageChart.svelte"
	import { hardwareStatistics } from "../stores/hardwareStatistics"
	import { hardwareInfo } from "../stores/hardwareInfo"

	let minutes = false
</script>
