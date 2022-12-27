<div class="transparent-900 m-10 mx-auto w-4/5 rounded-xl sm:m-1 sm:w-full">
	<div class="mx-10 flex gap-5 pb-10 pt-10">
		<div class="flex w-3/5 flex-col justify-start gap-5">
			<!-- cpu info -->
			<div class="transparent-800 rounded-xl p-10 text-left">
				<div class="mb-5 flex items-baseline gap-3">
					<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" viewBox="0 0 16 16">
						<path d="M1 3a1 1 0 0 0-1 1v8a1 1 0 0 0 1 1h4.586a1 1 0 0 0 .707-.293l.353-.353a.5.5 0 0 1 .708 0l.353.353a1 1 0 0 0 .707.293H15a1 1 0 0 0 1-1V4a1 1 0 0 0-1-1H1Zm.5 1h3a.5.5 0 0 1 .5.5v4a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-4a.5.5 0 0 1 .5-.5Zm5 0h3a.5.5 0 0 1 .5.5v4a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-4a.5.5 0 0 1 .5-.5Zm4.5.5a.5.5 0 0 1 .5-.5h3a.5.5 0 0 1 .5.5v4a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-4ZM2 10v2H1v-2h1Zm2 0v2H3v-2h1Zm2 0v2H5v-2h1Zm3 0v2H8v-2h1Zm2 0v2h-1v-2h1Zm2 0v2h-1v-2h1Zm2 0v2h-1v-2h1Z" />
					</svg>
					<h2>RAM Info</h2>
				</div>
				{#each $hardwareInfo.ram.info as { manufacturer, speed, capacity, bankLabel, maxVoltage, minVoltage }}
					<div class="mt-5">
						<h3>Bank: {bankLabel}</h3>
						<h3>Vendor: {manufacturer}</h3>
						<h3>Speed: {speed} MHz</h3>
						<h3>Max/Min voltage: {maxVoltage} V/{minVoltage} V</h3>
						<h3>Capacity: {capacity / 1024 / 1024 / 1024} GB</h3>
					</div>
				{/each}
			</div>
		</div>
	</div>
</div>

<script lang="ts">
	import TemperatureChart from "../components/temperatureChart.svelte"
	import LineChart from "../components/lineChart.svelte"
	import { hardwareStatistics } from "../stores/hardwareStatistics"
	import { hardwareInfo } from "../stores/hardwareInfo"

	// get gpu driver date
	let driver = $hardwareInfo.gpu.info[0].driverDate.slice(0, 8)
	let driverDate = `${driver.slice(0, 4)}.${driver.slice(4, 6)}.${driver.slice(6, 8)}.`

	// get primary monitor
	let primaryMonitor = $hardwareInfo.system.monitor.monitors.find((monitor) => monitor.primary)
</script>
