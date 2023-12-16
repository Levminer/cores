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
					<h3>Vendor: {$hardwareInfo.gpu.name.split(" ")[0]}</h3>
					<h3>Name: {$hardwareInfo.gpu.name}</h3>
					<h3>GPU memory: {$hardwareInfo.gpu.memory.length > 2 ? Math.round($hardwareInfo.gpu.memory[2].value) : "N/A"} GB</h3>
					<h3>Driver date: {driverDate}</h3>
					<h3>Primary monitor: {primaryMonitor.resolution} {primaryMonitor.refreshRate} Hz</h3>
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
					<MultiLineChart
						statistics={minutes
							? $hardwareStatistics.minutes.map((value) => value.gpu.temperature)
							: $hardwareStatistics.seconds.map((value) => value.gpu.temperature)}
						time={minutes ? "m" : "s"}
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
					<MultiLineChart
						statistics={minutes
							? $hardwareStatistics.minutes.map((value) => value.gpu.clock)
							: $hardwareStatistics.seconds.map((value) => value.gpu.clock)}
						time={minutes ? "m" : "s"}
						unit="Mhz"
						name="Clock Speed"
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
						statistics={minutes
							? $hardwareStatistics.minutes.map((value) => value.gpu.power)
							: $hardwareStatistics.seconds.map((value) => value.gpu.power)}
						type={"Power usage"}
						unit={" W"}
						color={"#ffd60a"}
						time={minutes ? "m" : "s"}
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
					<UsageChart
						statistics={minutes
							? $hardwareStatistics.minutes.map((value) => value.gpu.load)
							: $hardwareStatistics.seconds.map((value) => value.gpu.load)}
						type={"Load"}
						unit={"%"}
						time={minutes ? "m" : "s"}
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
						statistics={minutes
							? $hardwareStatistics.minutes.map((value) => value.gpu.fan)
							: $hardwareStatistics.seconds.map((value) => value.gpu.fan)}
						type={"Fan speed"}
						unit={" RPM"}
						color={"#00bbf9"}
						time={minutes ? "m" : "s"}
						zero={true}
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
						statistics={minutes
							? $hardwareStatistics.minutes.map((value) => value.gpu.memory)
							: $hardwareStatistics.seconds.map((value) => value.gpu.memory)}
						type={"Memory usage"}
						unit={" GB"}
						color={"#00bbf9"}
						time={minutes ? "m" : "s"}
						zero={true}
					/>
				</div>
			</div>
		</div>
	</div>
</div>

<script lang="ts">
	import MultiLineChart from "../components/multiLineChart.svelte"
	import LineChart from "../components/lineChart.svelte"
	import { hardwareStatistics } from "../stores/hardwareStatistics"
	import { hardwareInfo } from "../stores/hardwareInfo"
	import UsageChart from "../components/usageChart.svelte"
	import { GpuCard, Memory } from "svelte-bootstrap-icons"
	import { Clock, Fan, Gauge, Thermometer, Zap } from "lucide-svelte"
	import ToggleButton from "@components/toggleButton.svelte"

	let minutes = false

	// get gpu driver date
	let driver = $hardwareInfo.gpu.info.slice(0, 8)
	let driverDate = `${driver.slice(0, 4)}.${driver.slice(4, 6)}.${driver.slice(6, 8)}.`

	// get primary monitor
	let primaryMonitor = $hardwareInfo.system.monitor.monitors.find((monitor) => monitor.primary)
</script>
