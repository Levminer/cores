<div class="transparent-900 m-10 mx-auto w-4/5 rounded-xl sm:m-1 sm:w-full">
	<div class="mx-10 flex flex-col gap-5 pb-10 pt-10">
		<div class="flex w-full flex-row justify-start items-start gap-5">
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
				<div class="mb-5 flex items-baseline gap-3">
					<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" viewBox="0 0 16 16">
						<path d="M8 4a.5.5 0 0 1 .5.5V6a.5.5 0 0 1-1 0V4.5A.5.5 0 0 1 8 4zM3.732 5.732a.5.5 0 0 1 .707 0l.915.914a.5.5 0 1 1-.708.708l-.914-.915a.5.5 0 0 1 0-.707zM2 10a.5.5 0 0 1 .5-.5h1.586a.5.5 0 0 1 0 1H2.5A.5.5 0 0 1 2 10zm9.5 0a.5.5 0 0 1 .5-.5h1.5a.5.5 0 0 1 0 1H12a.5.5 0 0 1-.5-.5zm.754-4.246a.389.389 0 0 0-.527-.02L7.547 9.31a.91.91 0 1 0 1.302 1.258l3.434-4.297a.389.389 0 0 0-.029-.518z" />
						<path fill-rule="evenodd" d="M0 10a8 8 0 1 1 15.547 2.661c-.442 1.253-1.845 1.602-2.932 1.25C11.309 13.488 9.475 13 8 13c-1.474 0-3.31.488-4.615.911-1.087.352-2.49.003-2.932-1.25A7.988 7.988 0 0 1 0 10zm8-7a7 7 0 0 0-6.603 9.329c.203.575.923.876 1.68.63C4.397 12.533 6.358 12 8 12s3.604.532 4.923.96c.757.245 1.477-.056 1.68-.631A7 7 0 0 0 8 3z" />
					</svg>
					<h2>RAM usage</h2>
				</div>
				<div>
					<UsageChart statistics={$hardwareStatistics.ram.usage.physical} type={"Usage"} unit={"%"} color={"#00bbf9"} />
				</div>
			</div>

			<!-- virtual ram usage -->
			<div class="transparent-800 w-1/2 rounded-xl p-10 text-left sm:w-full">
				<div class="mb-5 flex items-baseline gap-3">
					<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" viewBox="0 0 16 16">
						<path d="M8 4a.5.5 0 0 1 .5.5V6a.5.5 0 0 1-1 0V4.5A.5.5 0 0 1 8 4zM3.732 5.732a.5.5 0 0 1 .707 0l.915.914a.5.5 0 1 1-.708.708l-.914-.915a.5.5 0 0 1 0-.707zM2 10a.5.5 0 0 1 .5-.5h1.586a.5.5 0 0 1 0 1H2.5A.5.5 0 0 1 2 10zm9.5 0a.5.5 0 0 1 .5-.5h1.5a.5.5 0 0 1 0 1H12a.5.5 0 0 1-.5-.5zm.754-4.246a.389.389 0 0 0-.527-.02L7.547 9.31a.91.91 0 1 0 1.302 1.258l3.434-4.297a.389.389 0 0 0-.029-.518z" />
						<path fill-rule="evenodd" d="M0 10a8 8 0 1 1 15.547 2.661c-.442 1.253-1.845 1.602-2.932 1.25C11.309 13.488 9.475 13 8 13c-1.474 0-3.31.488-4.615.911-1.087.352-2.49.003-2.932-1.25A7.988 7.988 0 0 1 0 10zm8-7a7 7 0 0 0-6.603 9.329c.203.575.923.876 1.68.63C4.397 12.533 6.358 12 8 12s3.604.532 4.923.96c.757.245 1.477-.056 1.68-.631A7 7 0 0 0 8 3z" />
					</svg>
					<h2>Virtual RAM usage</h2>
				</div>
				<div>
					<UsageChart statistics={$hardwareStatistics.ram.usage.virtual} type={"Usage"} unit={"%"} color={"#00bbf9"} />
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
</script>
