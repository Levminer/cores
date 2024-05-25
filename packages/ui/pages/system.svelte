<div class="transparent-900 m-10 mx-auto w-11/12 rounded-xl sm:w-full">
	<div class="mx-10 flex gap-5 pt-10 sm:mx-3 sm:flex-wrap">
		<div class="flex w-full flex-row items-start justify-start gap-5 sm:flex-wrap">
			<!-- drive info -->
			<div class="transparent-800 w-3/5 rounded-xl p-8 sm:w-full sm:p-4">
				<div class="mb-5 flex items-center gap-3">
					<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
						<PcDisplay width={24} height={24} />
					</div>
					<h2>System</h2>
				</div>
				<div class="select-text">
					<h3>CPU: {$hardwareInfo.cpu.name}</h3>
					<h3>RAM: {Math.round(($hardwareInfo.ram.load[0]?.value ?? 0) + ($hardwareInfo.ram.load[1]?.value ?? 0))} GB</h3>
					<h3>GPU: {$hardwareInfo.gpu.name}</h3>
					<h3>MB: {$hardwareInfo.system.motherboard.name}</h3>
					<h3>OS: {$hardwareInfo.system.os.name}</h3>
				</div>
			</div>

			<!-- drive usage -->
			<div class="transparent-800 w-3/5 rounded-xl p-8 sm:w-full sm:p-4">
				<div class="mb-5 flex items-center gap-3">
					<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
						<CircuitBoard />
					</div>
					<h2>BIOS</h2>
				</div>
				<div class="mt-5 select-text">
					<h3>Vendor: {$hardwareInfo.system.bios.vendor}</h3>
					<h3>Version: {$hardwareInfo.system.bios.version}</h3>
					<h3>Date: {$hardwareInfo.system.bios.date}</h3>
				</div>
			</div>
		</div>
	</div>

	<div class="mx-10 flex gap-5 pb-10 sm:mx-3 sm:flex-wrap">
		<div class="flex w-full flex-col justify-start gap-10 pt-10 sm:flex-wrap">
			<!-- storage read/write and temperature -->
			{#each $hardwareInfo.system.superIO.fan.filter((item) => item.value !== 0) as item, i}
				<div class="flex gap-5 sm:flex-wrap">
					<div class="transparent-800 w-1/2 rounded-xl p-8 sm:w-full sm:p-4">
						<div class="flex items-baseline justify-between">
							<div class="mb-5 flex items-center gap-3">
								<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
									<Fan />
								</div>
								<h2>{item.name} Speed</h2>
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
											label: `Max Speed`,
											data: minutes
												? $hardwareStatistics.minutes.map((value) => value.fan[i].speed.max)
												: $hardwareStatistics.seconds.map((value) => value.fan[i].speed.max),
										},
										{
											label: `Current Speed`,
											data: minutes
												? $hardwareStatistics.minutes.map((value) => value.fan[i].speed.value)
												: $hardwareStatistics.seconds.map((value) => value.fan[i].speed.value),
										},
										{
											label: `Min Speed`,
											data: minutes
												? $hardwareStatistics.minutes.map((value) => value.fan[i].speed.min)
												: $hardwareStatistics.seconds.map((value) => value.fan[i].speed.min),
										},
									],
									unit: " RPM",
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
									<Gauge />
								</div>
								<h2>{item.name} Usage</h2>
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
											label: `Fan Usage`,
											color: "min",
											fill: true,
											data: minutes
												? $hardwareStatistics.minutes.map((value) => value.fan[i].control.value)
												: $hardwareStatistics.seconds.map((value) => value.fan[i].control.value),
										},
									],
									unit: "%",
									time: minutes ? "m" : "s",
									min: 0,
									max: 100,
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
	import { hardwareStatistics } from "ui/stores/hardwareStatistics.ts"
	import { hardwareInfo } from "ui/stores/hardwareInfo.ts"
	import LineChart from "ui/charts/LineChart.svelte"
	import { Fan, PieChart, Gauge, CircuitBoard } from "lucide-svelte"
	import ToggleButton from "ui/components/toggleButton.svelte"
	import { PcDisplay } from "svelte-bootstrap-icons"

	let minutes = false
</script>
