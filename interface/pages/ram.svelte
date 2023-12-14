<div class="transparent-900 m-10 mx-auto w-11/12 rounded-xl sm:w-full">
	<div class="mx-10 flex gap-5 pt-10 sm:mx-3 sm:flex-wrap">
		<div class="flex w-full flex-row items-start justify-start gap-5 sm:flex-wrap">
			<!-- ram modules -->
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
						<h3>Speed: {configuredSpeed} MHz</h3>
						<h3>Voltage: {configuredVoltage / 1000} V</h3>
						<h3>Capacity: {size / 1024} GB</h3>
					</div>
				{/each}
			</div>

			<!-- ram layout -->
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
		</div>
	</div>

	<div class="mx-10 flex justify-evenly gap-5 pb-10 pt-5 sm:mx-3 sm:flex-wrap">
		<div class="flex w-full flex-row justify-start gap-5 sm:flex-wrap">
			<!-- ram usage -->
			<div class="transparent-800 w-1/2 rounded-xl p-8 sm:w-full sm:p-4">
				<div class="flex items-baseline justify-between">
					<div class="mb-5 flex items-center gap-3">
						<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
							<Memory width={24} height={24} />
						</div>
						<h2>RAM Usage</h2>
					</div>
					<div>
						<ToggleButton selected={minutes} on:click={() => (minutes = !minutes)} />
					</div>
				</div>
				<div>
					<UsageChart
						statistics={minutes
							? $hardwareStatistics.minutes.map((value) => value.ram.physicalUsage)
							: $hardwareStatistics.seconds.map((value) => value.ram.physicalUsage)}
						type={"Usage"}
						unit={"%"}
						time={minutes ? "m" : "s"}
					/>
				</div>
			</div>

			<!-- virtual ram usage -->
			<div class="transparent-800 w-1/2 rounded-xl p-8 sm:w-full sm:p-4">
				<div class="flex items-baseline justify-between">
					<div class="mb-5 flex items-center gap-3">
						<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
							<HardDrive />
						</div>
						<h2>Virtual RAM Usage</h2>
					</div>
					<div>
						<ToggleButton selected={minutes} on:click={() => (minutes = !minutes)} />
					</div>
				</div>
				<div>
					<UsageChart
						statistics={minutes
							? $hardwareStatistics.minutes.map((value) => value.ram.virtualUsage)
							: $hardwareStatistics.seconds.map((value) => value.ram.virtualUsage)}
						type={"Usage"}
						unit={"%"}
						time={minutes ? "m" : "s"}
					/>
				</div>
			</div>
		</div>
	</div>
</div>

<script lang="ts">
	import UsageChart from "../components/usageChart.svelte"
	import { hardwareStatistics } from "../stores/hardwareStatistics"
	import { hardwareInfo } from "../stores/hardwareInfo"
	import ToggleButton from "@components/toggleButton.svelte"
	import { Memory, Motherboard } from "svelte-bootstrap-icons"
	import { HardDrive, LineChart } from "lucide-svelte"

	let minutes = false
</script>
