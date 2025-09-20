<div class="transparent-900 m-10 mx-auto w-11/12 rounded-xl sm:w-full">
	<div class="mx-10 flex gap-5 pt-10 sm:mx-3 sm:flex-wrap">
		<div class="flex w-full flex-row items-start justify-start gap-5 sm:flex-wrap">
			<!-- ram modules -->
			{#if $hardwareInfo.ram.info.length > 0}
				<div class="transparent-800 w-3/5 rounded-xl p-8 sm:w-full sm:p-4">
					<div class="mb-5 flex items-center gap-3">
						<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
							<Memory width={24} height={24} />
						</div>
						<h2>RAM Modules</h2>
					</div>
					{#each $hardwareInfo.ram.info as { manufacturerName, configuredSpeed, configuredVoltage, size, bankLocator }}
						<div class="mt-5 select-text">
							<h3>Vendor: {manufacturerName}</h3>
							<h3>Speed: {configuredSpeed} MT/s</h3>
							<h3>Voltage: {configuredVoltage / 1000} V</h3>
							<h3>Capacity: {size / 1024} GB</h3>
						</div>
					{/each}
				</div>
			{/if}

			<!-- ram layout -->
			{#if $hardwareInfo.ram.layout.length > 0}
				<div class="transparent-800 w-2/5 rounded-xl p-8 sm:w-full sm:p-4">
					<div class="mb-5 flex items-center gap-3">
						<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
							<Motherboard width={24} height={24} />
						</div>
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
			{/if}
		</div>
	</div>

	<div class="mx-10 flex gap-5 pb-10 pt-10 sm:mx-3 sm:flex-wrap">
		<div class="flex w-full flex-row items-start justify-start gap-5 sm:flex-wrap">
			<!-- col 1 -->
			<div class="flex w-3/5 flex-col gap-5 sm:w-full">
				<!-- ram usage -->
				<div class="w-full">
					<ChartTile
						title="RAM Usage"
						item=""
						bind:minutes
						props={{
							id: "RAM_Usage",
							statistics: [
								{
									label: "Usage",
									fill: true,
									color: "min",
									data: minutes
										? $hardwareStatistics.minutes.map((value) => value.ram.physicalUsage)
										: $hardwareStatistics.seconds.map((value) => value.ram.physicalUsage),
								},
							],
							unit: " %",
							time: minutes ? "m" : "s",
							min: 0,
							max: 100,
							step: 10,
							timestamp: minutes
								? $hardwareStatistics.minutes.map((value) => value?.timestamp ?? new Date().toISOString())
								: $hardwareStatistics.seconds.map((value) => value?.timestamp ?? new Date().toISOString()),
						}}
					>
						{#snippet icon()}
							<Gauge />
						{/snippet}
					</ChartTile>
				</div>

				<!-- ram temperature -->
				{#if $hardwareInfo.ram.temperature?.length ?? 0 > 0}
					<div class="w-full">
						<ChartTile
							title="Average RAM Temperature"
							item=""
							bind:minutes
							props={{
								id: "RAM_Temperature",
								statistics: [
									{
										label: "Max Temperature",
										color: "max",
										data: minutes
											? $hardwareStatistics.minutes.map((value) => value.ram.temperature?.max ?? 0)
											: $hardwareStatistics.seconds.map((value) => value.ram.temperature?.max ?? 0),
									},
									{
										label: "Current Temperature",
										color: "current",
										data: minutes
											? $hardwareStatistics.minutes.map((value) => value.ram.temperature?.value ?? 0)
											: $hardwareStatistics.seconds.map((value) => value.ram.temperature?.value ?? 0),
									},
									{
										label: "Min Temperature",
										color: "min",
										data: minutes
											? $hardwareStatistics.minutes.map((value) => value.ram.temperature?.min ?? 0)
											: $hardwareStatistics.seconds.map((value) => value.ram.temperature?.min ?? 0),
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

			<!-- col 2 -->
			<div class="flex w-3/5 flex-col gap-5 sm:w-full">
				<!-- virtual ram usage -->
				<div class="w-full">
					<ChartTile
						title="Virtual RAM Usage"
						item=""
						bind:minutes
						props={{
							id: "Virtual_RAM_Usage",
							statistics: [
								{
									label: "Usage",
									fill: true,
									color: "min",
									data: minutes
										? $hardwareStatistics.minutes.map((value) => value.ram.virtualUsage)
										: $hardwareStatistics.seconds.map((value) => value.ram.virtualUsage),
								},
							],
							unit: " %",
							time: minutes ? "m" : "s",
							min: 0,
							max: 100,
							step: 10,
							timestamp: minutes
								? $hardwareStatistics.minutes.map((value) => value?.timestamp ?? new Date().toISOString())
								: $hardwareStatistics.seconds.map((value) => value?.timestamp ?? new Date().toISOString()),
						}}
					>
						{#snippet icon()}
							<Gauge />
						{/snippet}
					</ChartTile>
				</div>
			</div>
		</div>
	</div>
</div>

<script lang="ts">
	import { Memory, Motherboard } from "svelte-bootstrap-icons"
	import { Gauge, Thermometer } from "lucide-svelte"
	import { hardwareInfo, hardwareStatistics } from "ui"
	import ChartTile from "../components/ChartTile.svelte"

	let minutes = $state(false)
</script>
