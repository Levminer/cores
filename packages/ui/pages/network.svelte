<div class="transparent-900 m-10 mx-auto w-11/12 rounded-xl sm:w-full">
	<div class="mx-10 flex gap-5 pt-10 sm:mx-3 sm:flex-wrap">
		<div class="flex w-full flex-row items-start justify-start gap-5 sm:flex-wrap">
			<!-- interface info -->
			<div class="transparent-800 w-3/5 rounded-xl p-8 sm:w-full sm:p-4">
				<div class="mb-5 flex items-center gap-3">
					<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
						<Network />
					</div>
					<h2>Interfaces</h2>
				</div>
				{#each $hardwareInfo.system.network.interfaces as { name, description, ipAddress, mask, gateway, dns, speed, macAddress }}
					<div class="mt-5 select-text">
						<h3>Name: {name}</h3>
						<h3>Description: {description}</h3>
						<h3>Address: {ipAddress} ({mask})</h3>
						<h3>Gateway: {gateway} ({dns})</h3>
						<h3>Speed: {speed} Mbit/s</h3>
					</div>
				{/each}
			</div>

			<!-- network usage -->
			<div class="transparent-800 w-3/5 rounded-xl p-8 sm:w-full sm:p-4">
				<div class="mb-5 flex items-center gap-3">
					<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
						<PieChart />
					</div>
					<h2>Data usage</h2>
				</div>
				{#each $hardwareInfo.system.network.interfaces as { name, downloadData, uploadData }}
					<div class="mt-5 select-text">
						<h3>Name: {name}</h3>
						<h3>Downloaded data: {downloadData} GB</h3>
						<h3>Uploaded data: {uploadData} GB</h3>
					</div>
				{/each}
			</div>
		</div>
	</div>

	<div class="mx-10 flex gap-5 pb-10 sm:mx-3 sm:flex-wrap">
		<div class="flex w-full flex-col justify-start gap-10 pt-10 sm:flex-wrap">
			<!-- network usage -->
			{#each $hardwareInfo.system.network.interfaces as item, i}
				<div class="flex gap-5 sm:flex-wrap">
					<div class="transparent-800 w-1/2 rounded-xl p-8 sm:w-full sm:p-4">
						<div class="flex items-baseline justify-between">
							<div class="mb-5 flex items-center gap-3">
								<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
									<ArrowDownUp />
								</div>
								<h2><span class="line-clamp-1">{item.name}</span> Download/Upload Speed</h2>
							</div>
							<div>
								<ToggleButton selected={minutes} on:click={() => (minutes = !minutes)} />
							</div>
						</div>
						<div>
							<LineChart
								props={{
									statistics: [
										{
											label: `Download Speed`,
											data: minutes
												? $hardwareStatistics.minutes.map((value) => value.network[i].throughputDownload)
												: $hardwareStatistics.seconds.map((value) => value.network[i].throughputDownload),
										},
										{
											label: `Upload Speed`,
											data: minutes
												? $hardwareStatistics.minutes.map((value) => value.network[i].throughputUpload)
												: $hardwareStatistics.seconds.map((value) => value.network[i].throughputUpload),
										},
									],
									unit: " MB/s",
									time: minutes ? "m" : "s",
									min: 0,
								}}
							/>
						</div>
					</div>

					<div class="transparent-800 w-1/2 rounded-xl p-8 sm:w-full sm:p-4">
						<div class="flex items-baseline justify-between">
							<div class="mb-5 flex items-center gap-3">
								<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
									<PieChart />
								</div>
								<h2><span class="line-clamp-1">{item.name}</span> Data Usage</h2>
							</div>
							<div>
								<ToggleButton selected={minutes} on:click={() => (minutes = !minutes)} />
							</div>
						</div>
						<div>
							<LineChart
								props={{
									statistics: [
										{
											label: `Downloaded data`,
											data: minutes
												? $hardwareStatistics.minutes.map((value) => value.network[i].downloadedData)
												: $hardwareStatistics.seconds.map((value) => value.network[i].downloadedData),
										},
										{
											label: `Uploaded data`,
											data: minutes
												? $hardwareStatistics.minutes.map((value) => value.network[i].uploadedData)
												: $hardwareStatistics.seconds.map((value) => value.network[i].uploadedData),
										},
									],
									unit: " GB",
									time: minutes ? "m" : "s",
									min: 0,
								}}
							/>
						</div>
					</div>
				</div>
			{/each}
		</div>
	</div>
</div>

<script lang="ts">
	import LineChart from "ui/charts/LineChart.svelte"
	import { hardwareStatistics } from "../stores/hardwareStatistics.ts"
	import { hardwareInfo } from "ui/stores/hardwareInfo.ts"
	import { ArrowDownUp, Network, PieChart } from "lucide-svelte"
	import ToggleButton from "ui/components/toggleButton.svelte"

	let minutes = false
</script>
