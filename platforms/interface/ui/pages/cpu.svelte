<div class="transparent-900 m-10 mx-auto w-11/12 rounded-xl sm:w-full">
	<div class="mx-10 flex gap-5 pb-10 pt-10 sm:mx-3 sm:flex-wrap">
		<div class="flex w-3/5 flex-col justify-start gap-5 sm:w-full">
			<!-- cpu info -->
			<div class="transparent-800 rounded-xl p-8 sm:p-4">
				<div class="mb-5 flex items-center gap-3">
					<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
						<Cpu />
					</div>
					<h2>CPU Info</h2>
				</div>
				<div class="select-text">
					<h3>Vendor: {$hardwareInfo.cpu.info[0].manufacturerName.replaceAll("(R)", "").replaceAll("Corporation", "")}</h3>
					<h3>Name: {$hardwareInfo.cpu.name}</h3>
					<h3>Socket: {$hardwareInfo.cpu.info[0].socketDesignation}</h3>
					<h3>Max speed: {($hardwareInfo.cpu.info[0].maxSpeed / 1000).toFixed(1)} GHz</h3>
					<h3>Cores/Threads: {$hardwareInfo.cpu.info[0].coreCount} C/{$hardwareInfo.cpu.info[0].threadCount} T</h3>
				</div>
			</div>

			<!-- cpu temp -->
			<div class="transparent-800 rounded-xl p-8 sm:p-4">
				<div class="flex items-baseline justify-between">
					<div class="mb-5 flex items-center gap-3">
						<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
							<Thermometer />
						</div>
						<h2>Average CPU Temperature</h2>
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
										? $hardwareStatistics.minutes.map((value) => value.cpu.temperature.max)
										: $hardwareStatistics.seconds.map((value) => value.cpu.temperature.max),
								},
								{
									label: "Current Temperature",
									color: "current",
									data: minutes
										? $hardwareStatistics.minutes.map((value) => value.cpu.temperature.value)
										: $hardwareStatistics.seconds.map((value) => value.cpu.temperature.value),
								},
								{
									label: "Min Temperature",
									color: "min",
									data: minutes
										? $hardwareStatistics.minutes.map((value) => value.cpu.temperature.min)
										: $hardwareStatistics.seconds.map((value) => value.cpu.temperature.min),
								},
							],
							time: minutes ? "m" : "s",
							unit: " °C",
						}}
					/>
				</div>
			</div>

			<!-- cpu clock speed -->
			<div class="transparent-800 rounded-xl p-8 sm:p-4">
				<div class="flex items-baseline justify-between">
					<div class="mb-5 flex items-center gap-3">
						<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
							<Clock />
						</div>
						<h2>Average Clock speed</h2>
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
										? $hardwareStatistics.minutes.map((value) => value.cpu.clock.max)
										: $hardwareStatistics.seconds.map((value) => value.cpu.clock.max),
								},
								{
									label: "Current Clock Speed",
									color: "current",
									data: minutes
										? $hardwareStatistics.minutes.map((value) => value.cpu.clock.value)
										: $hardwareStatistics.seconds.map((value) => value.cpu.clock.value),
								},
								{
									label: "Min Clock Speed",
									color: "min",
									data: minutes
										? $hardwareStatistics.minutes.map((value) => value.cpu.clock.min)
										: $hardwareStatistics.seconds.map((value) => value.cpu.clock.min),
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
			<!-- cpu power usage -->
			{#if $hardwareInfo.cpu.power.length > 0}
				<div class="transparent-800 rounded-xl p-8 sm:p-4">
					<div class="flex items-baseline justify-between">
						<div class="mb-5 flex items-center gap-3">
							<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
								<Plug />
							</div>
							<h2>CPU Power Usage</h2>
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
											? $hardwareStatistics.minutes.map((value) => value.cpu.power)
											: $hardwareStatistics.seconds.map((value) => value.cpu.power),
									},
								],
								time: minutes ? "m" : "s",
								unit: " W",
							}}
						/>
					</div>
				</div>
			{/if}

			<!-- cpu load -->
			<div class="transparent-800 rounded-xl p-8 sm:p-4">
				<div class="flex items-baseline justify-between">
					<div class="mb-5 flex items-center gap-3">
						<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
							<Gauge />
						</div>
						<h2>Average CPU Load</h2>
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
										? $hardwareStatistics.minutes.map((value) => value.cpu.load)
										: $hardwareStatistics.seconds.map((value) => value.cpu.load),
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

			<!-- cpu voltage -->
			{#if $hardwareInfo.cpu.voltage.length > 0}
				<div class="transparent-800 rounded-xl p-8 sm:p-4">
					<div class="flex items-baseline justify-between">
						<div class="mb-5 flex items-center gap-3">
							<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
								<Zap />
							</div>
							<h2>CPU Voltage</h2>
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
										label: "Voltage",
										color: "orange",
										data: minutes
											? $hardwareStatistics.minutes.map((value) => value.cpu.voltage)
											: $hardwareStatistics.seconds.map((value) => value.cpu.voltage),
									},
								],
								time: minutes ? "m" : "s",
								unit: " V",
							}}
						/>
					</div>
				</div>
			{/if}
		</div>
	</div>
</div>

<script lang="ts">
	import LineChart from "ui/charts/LineChart.svelte"
	import { hardwareStatistics } from "ui/stores/hardwareStatistics.ts"
	import { hardwareInfo } from "ui/stores/hardwareInfo.ts"
	import { Clock, Cpu, Gauge, Plug, Thermometer, Zap } from "lucide-svelte"
	import ToggleButton from "ui/components/toggleButton.svelte"

	let minutes = false
</script>
