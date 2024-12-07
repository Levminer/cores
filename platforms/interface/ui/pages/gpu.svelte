<div class="transparent-900 m-10 mx-auto w-11/12 rounded-xl sm:w-full">
	<div class="mx-10 flex gap-5 pb-10 pt-10 sm:mx-3 sm:flex-wrap">
		<div class="flex w-3/5 flex-col justify-start gap-5 sm:w-full">
			<!-- gpu info -->
			<div class="transparent-800 rounded-xl p-8 sm:p-4">
				<div class="mb-5 flex items-center gap-3">
					<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
						<GpuCard width={24} height={24} />
					</div>
					<h2>GPU Info</h2>
				</div>
				<div class="select-text">
					<h3>Vendor: {$hardwareInfo.gpu.name?.split(" ")[0] ?? "N/A"}</h3>
					<h3>Name: {$hardwareInfo.gpu.name ?? "N/A"}</h3>
					<h3>GPU memory: {$hardwareInfo.gpu.memory.length > 2 ? Math.round($hardwareInfo.gpu.memory[2]?.value ?? 0) : "N/A"} GB</h3>
					<h3>Driver: {$hardwareInfo.gpu.info}</h3>
					<!-- <h3>
						Primary monitor: {$hardwareInfo.system.monitor.monitors.find((monitor) => monitor.primary)?.resolution ?? "N/A"}
						{$hardwareInfo.system.monitor.monitors.find((monitor) => monitor.primary)?.refreshRate ?? "N/A"} Hz
					</h3> -->
				</div>
			</div>

			<!-- gpu temperature -->
			<div class="transparent-800 rounded-xl p-8 sm:p-4">
				<div class="flex items-baseline justify-between">
					<div class="mb-5 flex items-center gap-3">
						<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
							<Thermometer />
						</div>
						<h2>Average GPU Temperature</h2>
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
									label: "Max Temperature",
									color: "max",
									data: minutes
										? $hardwareStatistics.minutes.map((value) => value.gpu.temperature.max)
										: $hardwareStatistics.seconds.map((value) => value.gpu.temperature.max),
								},
								{
									label: "Current Temperature",
									color: "current",
									data: minutes
										? $hardwareStatistics.minutes.map((value) => value.gpu.temperature.value)
										: $hardwareStatistics.seconds.map((value) => value.gpu.temperature.value),
								},
								{
									label: "Min Temperature",
									color: "min",
									data: minutes
										? $hardwareStatistics.minutes.map((value) => value.gpu.temperature.min)
										: $hardwareStatistics.seconds.map((value) => value.gpu.temperature.min),
								},
							],
							time: minutes ? "m" : "s",
							unit: " °C",
						}}
					/>
				</div>
			</div>

			<!-- gpu clock speed -->
			<div class="transparent-800 rounded-xl p-8 sm:p-4">
				<div class="flex items-baseline justify-between">
					<div class="mb-5 flex items-center gap-3">
						<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
							<Clock />
						</div>
						<h2>Average GPU Core Clock</h2>
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
									label: "Max Clock Speed",
									color: "max",
									data: minutes
										? $hardwareStatistics.minutes.map((value) => value.gpu.clock.max)
										: $hardwareStatistics.seconds.map((value) => value.gpu.clock.max),
								},
								{
									label: "Current Clock Speed",
									color: "current",
									data: minutes
										? $hardwareStatistics.minutes.map((value) => value.gpu.clock.value)
										: $hardwareStatistics.seconds.map((value) => value.gpu.clock.value),
								},
								{
									label: "Min Clock Speed",
									color: "min",
									data: minutes
										? $hardwareStatistics.minutes.map((value) => value.gpu.clock.min)
										: $hardwareStatistics.seconds.map((value) => value.gpu.clock.min),
								},
							],
							time: minutes ? "m" : "s",
							unit: " Mhz",
						}}
					/>
				</div>
			</div>
		</div>

		<div class="flex w-2/5 flex-col justify-start gap-5 sm:w-full">
			<!-- gpu power usage -->
			<div class="transparent-800 rounded-xl p-8 sm:p-4">
				<div class="flex items-baseline justify-between">
					<div class="mb-5 flex items-center gap-3">
						<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
							<Zap />
						</div>
						<h2>GPU Power Usage</h2>
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
									label: "Power Usage",
									color: "yellow",
									data: minutes
										? $hardwareStatistics.minutes.map((value) => value.gpu.power)
										: $hardwareStatistics.seconds.map((value) => value.gpu.power),
								},
							],
							time: minutes ? "m" : "s",
							unit: " W",
						}}
					/>
				</div>
			</div>

			<!-- gpu load -->
			<div class="transparent-800 rounded-xl p-8 sm:p-4">
				<div class="flex items-baseline justify-between">
					<div class="mb-5 flex items-center gap-3">
						<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
							<Gauge />
						</div>
						<h2>Average GPU Load</h2>
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
									label: "Load",
									color: "min",
									fill: true,
									data: minutes
										? $hardwareStatistics.minutes.map((value) => value.gpu.load)
										: $hardwareStatistics.seconds.map((value) => value.gpu.load),
								},
							],
							time: minutes ? "m" : "s",
							unit: "%",
							min: 0,
							max: 100,
						}}
					/>
				</div>
			</div>

			<!-- gpu fan usage -->
			<div class="transparent-800 rounded-xl p-8 sm:p-4">
				<div class="flex items-baseline justify-between">
					<div class="mb-5 flex items-center gap-3">
						<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
							<Fan />
						</div>
						<h2>GPU Fan Speed</h2>
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
									label: "Fan Speed",
									color: "min",
									data: minutes
										? $hardwareStatistics.minutes.map((value) => value.gpu.fan)
										: $hardwareStatistics.seconds.map((value) => value.gpu.fan),
								},
							],
							time: minutes ? "m" : "s",
							unit: " RPM",
							min: 0,
						}}
					/>
				</div>
			</div>

			<!-- gpu memory usage -->
			<div class="transparent-800 rounded-xl p-8 sm:p-4">
				<div class="flex items-baseline justify-between">
					<div class="mb-5 flex items-center gap-3">
						<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
							<Memory height={24} width={24} />
						</div>
						<h2>GPU Memory Usage</h2>
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
									label: "Memory Usage",
									color: "min",
									data: minutes
										? $hardwareStatistics.minutes.map((value) => value.gpu.memory)
										: $hardwareStatistics.seconds.map((value) => value.gpu.memory),
								},
							],
							time: minutes ? "m" : "s",
							unit: " GB",
							min: 0,
						}}
					/>
				</div>
			</div>
		</div>
	</div>
</div>

<script lang="ts">
	import LineChart from "ui/charts/LineChart.svelte"
	import { hardwareStatistics } from "ui/stores/hardwareStatistics.ts"
	import { hardwareInfo } from "ui/stores/hardwareInfo.ts"
	import { GpuCard, Memory } from "svelte-bootstrap-icons"
	import { Clock, Fan, Gauge, Thermometer, Zap } from "lucide-svelte"
	import ToggleButton from "ui/components/toggleButton.svelte"

	let minutes = false
</script>
