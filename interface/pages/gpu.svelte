<div class="transparent-900 m-10 mx-auto w-4/5 rounded-xl sm:m-3 sm:w-full">
	<div class="mx-10 flex gap-5 pb-10 pt-10">
		<div class="flex w-3/5 flex-col justify-start gap-5">
			<!-- cpu info -->
			<div class="transparent-800 rounded-xl p-10 text-left">
				<div class="mb-5 flex items-baseline gap-3">
					<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" viewBox="0 0 16 16">
						<path d="M4 8a1.5 1.5 0 1 1 3 0 1.5 1.5 0 0 1-3 0Zm7.5-1.5a1.5 1.5 0 1 0 0 3 1.5 1.5 0 0 0 0-3Z" />
						<path d="M0 1.5A.5.5 0 0 1 .5 1h1a.5.5 0 0 1 .5.5V4h13.5a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-.5.5H2v2.5a.5.5 0 0 1-1 0V2H.5a.5.5 0 0 1-.5-.5Zm5.5 4a2.5 2.5 0 1 0 0 5 2.5 2.5 0 0 0 0-5ZM9 8a2.5 2.5 0 1 0 5 0 2.5 2.5 0 0 0-5 0Z" />
						<path d="M3 12.5h3.5v1a.5.5 0 0 1-.5.5H3.5a.5.5 0 0 1-.5-.5v-1Zm4 1v-1h4v1a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5Z" />
					</svg>
					<h2>GPU Info</h2>
				</div>
				<h3>Vendor: {$hardwareInfo.gpu.info[0].manufacturer}</h3>
				<h3>Name: {$hardwareInfo.gpu.name}</h3>
				<h3>GPU memory: {Math.round($hardwareInfo.gpu.memory[2].value / 1024)} GB</h3>
				<h3>Driver date: {driverDate}</h3>
				<h3>Primary monitor: {primaryMonitor.resolution} {primaryMonitor.refreshRate} Hz</h3>
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
