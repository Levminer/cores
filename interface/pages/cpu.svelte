<div class="transparent-900 m-10 mx-auto w-4/5 rounded-xl sm:m-3 sm:w-full">
	<div class="mx-10 flex gap-5 pb-10 pt-10 sm:flex-wrap">
		<div class="flex w-3/5 flex-col justify-start gap-5 sm:w-full">
			<!-- cpu info -->
			<div class="transparent-800 rounded-xl p-10 text-left">
				<div class="mb-5 flex items-baseline gap-3">
					<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="bi bi-cpu" viewBox="0 0 16 16">
						<path d="M5 0a.5.5 0 0 1 .5.5V2h1V.5a.5.5 0 0 1 1 0V2h1V.5a.5.5 0 0 1 1 0V2h1V.5a.5.5 0 0 1 1 0V2A2.5 2.5 0 0 1 14 4.5h1.5a.5.5 0 0 1 0 1H14v1h1.5a.5.5 0 0 1 0 1H14v1h1.5a.5.5 0 0 1 0 1H14v1h1.5a.5.5 0 0 1 0 1H14a2.5 2.5 0 0 1-2.5 2.5v1.5a.5.5 0 0 1-1 0V14h-1v1.5a.5.5 0 0 1-1 0V14h-1v1.5a.5.5 0 0 1-1 0V14h-1v1.5a.5.5 0 0 1-1 0V14A2.5 2.5 0 0 1 2 11.5H.5a.5.5 0 0 1 0-1H2v-1H.5a.5.5 0 0 1 0-1H2v-1H.5a.5.5 0 0 1 0-1H2v-1H.5a.5.5 0 0 1 0-1H2A2.5 2.5 0 0 1 4.5 2V.5A.5.5 0 0 1 5 0zm-.5 3A1.5 1.5 0 0 0 3 4.5v7A1.5 1.5 0 0 0 4.5 13h7a1.5 1.5 0 0 0 1.5-1.5v-7A1.5 1.5 0 0 0 11.5 3h-7zM5 6.5A1.5 1.5 0 0 1 6.5 5h3A1.5 1.5 0 0 1 11 6.5v3A1.5 1.5 0 0 1 9.5 11h-3A1.5 1.5 0 0 1 5 9.5v-3zM6.5 6a.5.5 0 0 0-.5.5v3a.5.5 0 0 0 .5.5h3a.5.5 0 0 0 .5-.5v-3a.5.5 0 0 0-.5-.5h-3z" />
					</svg>
					<h2>CPU Info</h2>
				</div>
				<h3>Vendor: {$hardwareInfo.cpu.info[0].manufacturer}</h3>
				<h3>Family: {$hardwareInfo.cpu.info[0].caption}</h3>
				<h3>Name: {$hardwareInfo.cpu.name}</h3>
				<h3>Socket: {$hardwareInfo.cpu.info[0].socketDesignation}</h3>
				<h3>Base speed: {($hardwareInfo.cpu.info[0].currentClockSpeed / 1000).toFixed(1)} GHz</h3>
				<h3>Cores/Threads: {$hardwareInfo.cpu.info[0].numberOfCores} C/{$hardwareInfo.cpu.info[0].numberOfLogicalProcessors} T</h3>
			</div>

			<!-- cpu temp -->
			<div class="transparent-800 rounded-xl p-10 text-left">
				<div class="mb-5 flex items-baseline justify-between">
					<div class="flex items-baseline gap-3">
						<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M14 4v10.54a4 4 0 1 1-4 0V4a2 2 0 0 1 4 0Z" /></svg>
						<h2>Average CPU Temperature</h2>
					</div>
					<div>
						<h5 class="cursor-pointer" on:click={() => (minutes = !minutes)}>{minutes ? "Last 60 minutes" : "Last 60 seconds"}</h5>
					</div>
				</div>

				<div>
					<TemperatureChart statistics={minutes ? $hardwareStatistics.cpu.temperature.minutes : $hardwareStatistics.cpu.temperature.seconds} time={minutes ? "m" : "s"} />
				</div>
			</div>
		</div>

		<div class="flex w-2/5 flex-col justify-start gap-5 sm:w-full">
			<!-- cpu power usage -->
			<div class="transparent-800 rounded-xl p-10 text-left">
				<div class="mb-5 flex items-baseline justify-between">
					<div class="flex items-baseline gap-3">
						<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M12 22v-5" /><path d="M9 7V2" /><path d="M15 7V2" /><path d="M6 13V8h12v5a4 4 0 0 1-4 4h-4a4 4 0 0 1-4-4Z" /></svg>
						<h2>CPU Power Usage</h2>
					</div>
					<div>
						<h5 class="cursor-pointer" on:click={() => (minutes = !minutes)}>{minutes ? "Last 60m" : "Last 60s"}</h5>
					</div>
				</div>

				<div>
					<LineChart statistics={minutes ? $hardwareStatistics.cpu.power.minutes : $hardwareStatistics.cpu.power.seconds} type={"Power usage"} unit={" W"} color={"#ffd60a"} time={minutes ? "m" : "s"} />
				</div>
			</div>

			<!-- cpu load -->
			<div class="transparent-800 rounded-xl p-10 text-left">
				<div class="mb-5 flex items-baseline justify-between">
					<div class="flex items-baseline gap-3">
						<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" viewBox="0 0 16 16">
							<path d="M8 4a.5.5 0 0 1 .5.5V6a.5.5 0 0 1-1 0V4.5A.5.5 0 0 1 8 4zM3.732 5.732a.5.5 0 0 1 .707 0l.915.914a.5.5 0 1 1-.708.708l-.914-.915a.5.5 0 0 1 0-.707zM2 10a.5.5 0 0 1 .5-.5h1.586a.5.5 0 0 1 0 1H2.5A.5.5 0 0 1 2 10zm9.5 0a.5.5 0 0 1 .5-.5h1.5a.5.5 0 0 1 0 1H12a.5.5 0 0 1-.5-.5zm.754-4.246a.389.389 0 0 0-.527-.02L7.547 9.31a.91.91 0 1 0 1.302 1.258l3.434-4.297a.389.389 0 0 0-.029-.518z" />
							<path fill-rule="evenodd" d="M0 10a8 8 0 1 1 15.547 2.661c-.442 1.253-1.845 1.602-2.932 1.25C11.309 13.488 9.475 13 8 13c-1.474 0-3.31.488-4.615.911-1.087.352-2.49.003-2.932-1.25A7.988 7.988 0 0 1 0 10zm8-7a7 7 0 0 0-6.603 9.329c.203.575.923.876 1.68.63C4.397 12.533 6.358 12 8 12s3.604.532 4.923.96c.757.245 1.477-.056 1.68-.631A7 7 0 0 0 8 3z" />
						</svg>
						<h2>Average CPU Load</h2>
					</div>
					<div>
						<h5 class="cursor-pointer" on:click={() => (minutes = !minutes)}>{minutes ? "Last 60m" : "Last 60s"}</h5>
					</div>
				</div>

				<div>
					<UsageChart statistics={minutes ? $hardwareStatistics.cpu.load.minutes : $hardwareStatistics.cpu.load.seconds} type={"Load"} unit={"%"} color={"#00bbf9"} time={minutes ? "m" : "s"} />
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
