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

	<div class="mx-10 flex gap-5 pb-10 pt-5 sm:mx-3 sm:flex-wrap">
		<div class="flex w-full flex-row justify-start gap-5 sm:flex-wrap">
			<!-- ram usage -->
			<div class="transparent-800 w-1/2 rounded-xl p-8 sm:w-full sm:p-4">
				<div class="flex items-baseline justify-between">
					<div class="mb-5 flex items-center gap-3">
						<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
							<Gauge />
						</div>
						<h2>RAM Usage</h2>
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
						}}
					/>
				</div>
			</div>

			<!-- virtual ram usage -->
			<div class="transparent-800 w-1/2 rounded-xl p-8 sm:w-full sm:p-4">
				<div class="flex items-baseline justify-between">
					<div class="mb-5 flex items-center gap-3">
						<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
							<Gauge />
						</div>
						<h2>Virtual RAM Usage</h2>
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
						}}
					/>
				</div>
			</div>
		</div>
	</div>
</div>

<script lang="ts">
	import { hardwareStatistics } from "ui/stores/hardwareStatistics.ts"
	import { hardwareInfo } from "ui/stores/hardwareInfo.ts"
	import ToggleButton from "ui/components/toggleButton.svelte"
	import { Memory, Motherboard } from "svelte-bootstrap-icons"
	import { Gauge } from "lucide-svelte"
	import LineChart from "ui/charts/LineChart.svelte"

	let minutes = false
</script>
