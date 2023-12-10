<div class="transparent-900 m-10 mx-auto w-11/12 rounded-xl sm:m-3 sm:w-full">
	<div class="mx-10 flex flex-col gap-5 pb-10 pt-10">
		<div class="flex w-full flex-row items-start justify-start gap-5">
			<!-- drive info -->
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
						><line x1="22" y1="12" x2="2" y2="12" /><path
							d="M5.45 5.11 2 12v6a2 2 0 0 0 2 2h16a2 2 0 0 0 2-2v-6l-3.45-6.89A2 2 0 0 0 16.76 4H7.24a2 2 0 0 0-1.79 1.11z"
						/><line x1="6" y1="16" x2="6.01" y2="16" /><line x1="10" y1="16" x2="10.01" y2="16" /></svg
					>
					<h2>Drives</h2>
				</div>
				{#each $hardwareInfo.system.storage.disks as { name, temperature, freeSpace, totalSpace, health }}
					<div class="mt-5 select-text">
						<h3>Name: {name}</h3>
						<h3>Temperature: {temperature.value} °C</h3>
						<h3>Health: {health}%</h3>
						<h3>Available space: {freeSpace}/{totalSpace} GB</h3>
					</div>
				{/each}
			</div>

			<!-- drive usage -->
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

		<div class="flex w-full flex-col justify-start gap-5 sm:flex-wrap">
			<!-- network usage -->
			{#each $hardwareInfo.system.storage.disks as item, i}
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
									class="lucide lucide-hard-drive-upload"
									><path d="m16 6-4-4-4 4" /><path d="M12 2v8" /><rect width="20" height="8" x="2" y="14" rx="2" /><path
										d="M6 18h.01"
									/><path d="M10 18h.01" /></svg
								>
								<h2><span class="line-clamp-1">{item.name}</span> Read speed</h2>
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
									? $hardwareStatistics.minutes.map((value) => value.storage[i].throughputRead)
									: $hardwareStatistics.seconds.map((value) => value.storage[i].throughputRead)}
								type={"Read speed"}
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
									class="lucide lucide-hard-drive-download"
									><path d="M12 2v8" /><path d="m16 6-4 4-4-4" /><rect width="20" height="8" x="2" y="14" rx="2" /><path
										d="M6 18h.01"
									/><path d="M10 18h.01" /></svg
								>
								<h2><span class="line-clamp-1">{item.name}</span> Write speed</h2>
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
									? $hardwareStatistics.minutes.map((value) => value.storage[i].throughputWrite)
									: $hardwareStatistics.seconds.map((value) => value.storage[i].throughputWrite)}
								type={"Write speed"}
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
