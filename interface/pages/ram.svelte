<div class="transparent-900 m-10 mx-auto w-4/5 rounded-xl sm:m-3 sm:w-full">
	<div class="mx-10 flex flex-col gap-5 pb-10 pt-10">
		<div class="flex w-full flex-row items-start justify-start gap-5">
			<!-- ram info -->
			<div class="transparent-800 w-3/5 rounded-xl p-10 text-left">
				<div class="mb-5 flex items-baseline gap-3">
					<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" viewBox="0 0 16 16">
						<path d="M1 3a1 1 0 0 0-1 1v8a1 1 0 0 0 1 1h4.586a1 1 0 0 0 .707-.293l.353-.353a.5.5 0 0 1 .708 0l.353.353a1 1 0 0 0 .707.293H15a1 1 0 0 0 1-1V4a1 1 0 0 0-1-1H1Zm.5 1h3a.5.5 0 0 1 .5.5v4a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-4a.5.5 0 0 1 .5-.5Zm5 0h3a.5.5 0 0 1 .5.5v4a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-4a.5.5 0 0 1 .5-.5Zm4.5.5a.5.5 0 0 1 .5-.5h3a.5.5 0 0 1 .5.5v4a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-4ZM2 10v2H1v-2h1Zm2 0v2H3v-2h1Zm2 0v2H5v-2h1Zm3 0v2H8v-2h1Zm2 0v2h-1v-2h1Zm2 0v2h-1v-2h1Zm2 0v2h-1v-2h1Z" />
					</svg>
					<h2>RAM Info</h2>
				</div>
				{#each $hardwareInfo.ram.info as { manufacturer, speed, capacity, bankLabel, maxVoltage, minVoltage }}
					<div class="mt-5">
						<h3>Vendor: {manufacturer}</h3>
						<h3>Bank: {bankLabel}</h3>
						<h3>Speed: {speed} MHz</h3>
						<h3>Max/Min voltage: {maxVoltage} V/{minVoltage} V</h3>
						<h3>Capacity: {capacity / 1024 / 1024 / 1024} GB</h3>
					</div>
				{/each}
			</div>

			<!-- ram usage -->
			<div class="transparent-800 w-2/5 rounded-xl p-10">
				<div class="mb-5 flex items-baseline gap-3">
					<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="bi bi-memory" viewBox="0 0 16 16">
						<path d="M1 3a1 1 0 0 0-1 1v8a1 1 0 0 0 1 1h4.586a1 1 0 0 0 .707-.293l.353-.353a.5.5 0 0 1 .708 0l.353.353a1 1 0 0 0 .707.293H15a1 1 0 0 0 1-1V4a1 1 0 0 0-1-1H1Zm.5 1h3a.5.5 0 0 1 .5.5v4a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-4a.5.5 0 0 1 .5-.5Zm5 0h3a.5.5 0 0 1 .5.5v4a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-4a.5.5 0 0 1 .5-.5Zm4.5.5a.5.5 0 0 1 .5-.5h3a.5.5 0 0 1 .5.5v4a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-4ZM2 10v2H1v-2h1Zm2 0v2H3v-2h1Zm2 0v2H5v-2h1Zm3 0v2H8v-2h1Zm2 0v2h-1v-2h1Zm2 0v2h-1v-2h1Zm2 0v2h-1v-2h1Z" />
					</svg>
					<h2>RAM Usage</h2>
				</div>
				<h3>Memory: {`${$hardwareInfo.ram.load[0].value.toFixed(1)}/${($hardwareInfo.ram.load[0].value + $hardwareInfo.ram.load[1].value).toFixed(1)} GB`}</h3>
				<h3>Virtual memory: {`${$hardwareInfo.ram.load[3].value.toFixed(1)}/${($hardwareInfo.ram.load[3].value + $hardwareInfo.ram.load[4].value).toFixed(1)} GB`}</h3>
			</div>
		</div>

		<div class="flex w-full flex-row justify-start gap-5 sm:flex-wrap">
			<!-- ram usage -->
			<div class="transparent-800 w-1/2 rounded-xl p-10 text-left sm:w-full">
				<div class="mb-5 flex items-baseline justify-between">
					<div class="flex items-baseline gap-3">
						<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M3 3v18h18" /><path d="m19 9-5 5-4-4-3 3" /></svg>
						<h2>RAM usage</h2>
					</div>
					<div>
						<h5 class="cursor-pointer" on:click={() => (minutes = !minutes)}>{minutes ? "Last 60 minutes" : "Last 60 seconds"}</h5>
					</div>
				</div>
				<div>
					<UsageChart statistics={minutes ? $hardwareStatistics.ram.physicalUsage.minutes : $hardwareStatistics.ram.physicalUsage.seconds} type={"Usage"} unit={"%"} color={"#00bbf9"} time={minutes ? "m" : "s"} />
				</div>
			</div>

			<!-- virtual ram usage -->
			<div class="transparent-800 w-1/2 rounded-xl p-10 text-left sm:w-full">
				<div class="mb-5 flex items-baseline justify-between">
					<div class="flex items-baseline gap-3">
						<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M3 3v18h18" /><path d="m19 9-5 5-4-4-3 3" /></svg>
						<h2>Virtual RAM usage</h2>
					</div>
					<div>
						<h5 class="cursor-pointer" on:click={() => (minutes = !minutes)}>{minutes ? "Last 60 minutes" : "Last 60 seconds"}</h5>
					</div>
				</div>
				<div>
					<UsageChart statistics={minutes ? $hardwareStatistics.ram.virtualUsage.minutes : $hardwareStatistics.ram.virtualUsage.seconds} type={"Usage"} unit={"%"} color={"#00bbf9"} time={minutes ? "m" : "s"} />
				</div>
			</div>
		</div>
	</div>
</div>

<script lang="ts">
	import TemperatureChart from "../components/temperatureChart.svelte"
	import LineChart from "../components/lineChart.svelte"
	import UsageChart from "../components/usageChart.svelte"
	import { hardwareStatistics } from "../stores/hardwareStatistics"
	import { hardwareInfo } from "../stores/hardwareInfo"

	let minutes = false
</script>
