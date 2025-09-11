<div class="transparent-900 m-10 mx-auto w-11/12 rounded-xl sm:w-full">
	<div class="mx-10 flex gap-5 pt-10 sm:mx-3 sm:flex-wrap">
		<div class="flex w-full flex-row items-start justify-start gap-5 sm:flex-wrap">
			<!-- drive info -->
			<div class="transparent-800 w-3/5 rounded-xl p-8 sm:w-full sm:p-4">
				<div class="mb-5 flex items-center gap-3">
					<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
						<HardDrive />
					</div>
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
			<div class="transparent-800 w-3/5 rounded-xl p-8 sm:w-full sm:p-4">
				<div class="mb-5 flex items-center gap-3">
					<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
						<PieChart />
					</div>
					<h2>Drive usage</h2>
				</div>
				{#each $hardwareInfo.system.storage.disks as { name, dataRead, dataWritten }}
					{#if dataRead != 0 || dataWritten != 0}
						<div class="mt-5 select-text">
							<h3>Name: {name}</h3>
							<h3>Data read: {dataRead} GB</h3>
							<h3>Data written: {dataWritten} GB</h3>
						</div>
					{/if}
				{/each}
			</div>
		</div>
	</div>

	<div class="mx-10 flex gap-5 pb-10 sm:mx-3 sm:flex-wrap">
		<div class="flex w-full flex-col justify-start gap-5 pt-5 sm:flex-wrap">
			<!-- storage read/write and temperature -->
			{#each $hardwareInfo.system.storage.disks as item, i}
				<div class="flex gap-5 sm:flex-wrap">
					<div class="w-1/2 sm:w-full">
						<ChartTile
							title="Read/Write Speed"
							item={item.name}
							bind:minutes
							props={{
								id: `Storage_Read_Write_Speed_${i}`,
								statistics: [
									{
										label: `Read speed`,
										data: minutes
											? $hardwareStatistics.minutes.map((value) => value.storage[i].throughputRead)
											: $hardwareStatistics.seconds.map((value) => value.storage[i].throughputRead),
									},
									{
										label: `Write speed`,
										data: minutes
											? $hardwareStatistics.minutes.map((value) => value.storage[i].throughputWrite)
											: $hardwareStatistics.seconds.map((value) => value.storage[i].throughputWrite),
									},
								],
								unit: " MB/s",
								time: minutes ? "m" : "s",
								min: 0,
								timestamp: minutes
									? $hardwareStatistics.minutes.map((value) => value?.timestamp ?? new Date().toISOString())
									: $hardwareStatistics.seconds.map((value) => value?.timestamp ?? new Date().toISOString()),
							}}
						>
							{#snippet icon()}
								<Activity />
							{/snippet}
						</ChartTile>
					</div>

					{#if item.temperature.value > 0}
						<div class="w-1/2 sm:w-full">
							<ChartTile
								title="Temperature"
								item={item.name}
								bind:minutes
								props={{
									id: `Storage_Temperature_${i}`,
									statistics: [
										{
											label: `Max Temperature`,
											color: "max",
											data: minutes
												? $hardwareStatistics.minutes.map((value) => value.storage[i].temperature.max)
												: $hardwareStatistics.seconds.map((value) => value.storage[i].temperature.max),
										},
										{
											label: `Current Temperature`,
											color: "current",
											data: minutes
												? $hardwareStatistics.minutes.map((value) => value.storage[i].temperature.value)
												: $hardwareStatistics.seconds.map((value) => value.storage[i].temperature.value),
										},
										{
											label: `Min Temperature`,
											color: "min",
											data: minutes
												? $hardwareStatistics.minutes.map((value) => value.storage[i].temperature.min)
												: $hardwareStatistics.seconds.map((value) => value.storage[i].temperature.min),
										},
									],
									unit: " °C",
									time: minutes ? "m" : "s",
									timestamp: minutes
										? $hardwareStatistics.minutes.map((value) => value?.timestamp ?? new Date().toISOString())
										: $hardwareStatistics.seconds.map((value) => value?.timestamp ?? new Date().toISOString()),
								}}
							>
								{#snippet icon()}
									<Thermometer />
								{/snippet}
							</ChartTile>
						</div>
					{/if}
				</div>
			{/each}
		</div>
	</div>
</div>

<script lang="ts">
	import { Activity, HardDrive, PieChart, Thermometer } from "lucide-svelte"
	import { hardwareInfo, hardwareStatistics } from "ui"
	import ChartTile from "../components/ChartTile.svelte"

	let minutes = $state(false)
</script>
