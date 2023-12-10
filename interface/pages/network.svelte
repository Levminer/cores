<div class="transparent-900 m-10 mx-auto w-11/12 rounded-xl sm:m-3 sm:w-full">
	<div class="mx-10 flex flex-col gap-5 pb-10 pt-10">
		<div class="flex w-full flex-row items-start justify-start gap-5">
			<!-- interface info -->
			<div class="transparent-800 w-3/5 rounded-xl p-10 text-left">
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
						stroke-linejoin="round"
						><rect x="9" y="2" width="6" height="6" /><rect x="16" y="16" width="6" height="6" /><rect
							x="2"
							y="16"
							width="6"
							height="6"
						/><path d="M5 16v-4h14v4" /><path d="M12 12V8" /></svg
					>
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
			<div class="transparent-800 w-2/5 rounded-xl p-10">
				<div class="mb-5 flex items-baseline gap-3">
					<svg
						xmlns="http://www.w3.org/2000/svg"
						width="24"
						height="24"
						viewBox="0 0 24 24"
						fill="none"
						stroke="currentColor"
						stroke-width="2"
						stroke-linecap="round"
						stroke-linejoin="round"
						class="lucide lucide-pie-chart"><path d="M21.21 15.89A10 10 0 1 1 8 2.83" /><path d="M22 12A10 10 0 0 0 12 2v10z" /></svg
					>
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

		<div class="flex w-full flex-col justify-start gap-10 pt-10 sm:flex-wrap">
			<!-- network usage -->
			{#each $hardwareInfo.system.network.interfaces as item, i}
				<div class="flex gap-5">
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
									stroke-linejoin="round"
									class="lucide lucide-download"
									><path d="M21 15v4a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2v-4" /><polyline points="7 10 12 15 17 10" /><line
										x1="12"
										x2="12"
										y1="15"
										y2="3"
									/></svg
								>
								<h2>{item.name} Download speed</h2>
							</div>
							<div>
								<h5 class="cursor-pointer" on:click={() => (minutes = !minutes)}>
									{minutes ? "Last 60 minutes" : "Last 60 seconds"}
								</h5>
							</div>
						</div>
						<div>
							<LineChart
								statistics={minutes
									? $hardwareStatistics.minutes.map((value) => value.network[i].throughputDownload)
									: $hardwareStatistics.seconds.map((value) => value.network[i].throughputDownload)}
								type={"Download speed"}
								unit={" MB/s"}
								color={"#00bbf9"}
								time={minutes ? "m" : "s"}
								zero={true}
							/>
						</div>
					</div>

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
									stroke-linejoin="round"
									class="lucide lucide-upload"
									><path d="M21 15v4a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2v-4" /><polyline points="17 8 12 3 7 8" /><line
										x1="12"
										x2="12"
										y1="3"
										y2="15"
									/></svg
								>
								<h2>{item.name} Upload speed</h2>
							</div>
							<div>
								<h5 class="cursor-pointer" on:click={() => (minutes = !minutes)}>
									{minutes ? "Last 60 minutes" : "Last 60 seconds"}
								</h5>
							</div>
						</div>
						<div>
							<LineChart
								statistics={minutes
									? $hardwareStatistics.minutes.map((value) => value.network[i].throughputUpload)
									: $hardwareStatistics.seconds.map((value) => value.network[i].throughputUpload)}
								type={"Upload speed"}
								unit={" MB/s"}
								color={"#00bbf9"}
								time={minutes ? "m" : "s"}
								zero={true}
							/>
						</div>
					</div>
				</div>
			{/each}
		</div>
	</div>
</div>

<script lang="ts">
	import LineChart from "../components/lineChart.svelte"
	import { hardwareStatistics } from "../stores/hardwareStatistics"
	import { hardwareInfo } from "../stores/hardwareInfo"

	let minutes = false
</script>
