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
					<h3>Base speed: {($hardwareInfo.cpu.info[0].currentSpeed / 1000).toFixed(1)} GHz</h3>
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
						<h2>Average Temperature</h2>
					</div>
					<div class="flex flex-row">
						<SaveDataButton
							props={{
								id: "CPU_Temperature",
								statistics: [
									{
										label: "Max Temperature",
										data: minutes
											? $hardwareStatistics.minutes.map((value) => value.cpu.temperature.max)
											: $hardwareStatistics.seconds.map((value) => value.cpu.temperature.max),
									},
									{
										label: "Current Temperature",
										data: minutes
											? $hardwareStatistics.minutes.map((value) => value.cpu.temperature.value)
											: $hardwareStatistics.seconds.map((value) => value.cpu.temperature.value),
									},
									{
										label: "Min Temperature",
										data: minutes
											? $hardwareStatistics.minutes.map((value) => value.cpu.temperature.min)
											: $hardwareStatistics.seconds.map((value) => value.cpu.temperature.min),
									},
								],
							}}
						/>
						<ToggleButton selected={minutes} on:click={() => (minutes = !minutes)} />
					</div>
				</div>

				<div>
					<LineChart
						props={{
							id: "CPU_Temperature",
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
							timestamp: minutes
								? $hardwareStatistics.minutes.map((value) => value?.timestamp ?? new Date().toISOString())
								: $hardwareStatistics.seconds.map((value) => value?.timestamp ?? new Date().toISOString()),
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
						<h2>Average Clock Speed</h2>
					</div>
					<div class="flex flex-row">
						<SaveDataButton
							props={{
								id: "CPU_Clock_Speed",
								statistics: [
									{
										label: "Max Clock Speed",
										data: minutes
											? $hardwareStatistics.minutes.map((value) => value.cpu.clock.max)
											: $hardwareStatistics.seconds.map((value) => value.cpu.clock.max),
									},
									{
										label: "Current Clock Speed",
										data: minutes
											? $hardwareStatistics.minutes.map((value) => value.cpu.clock.value)
											: $hardwareStatistics.seconds.map((value) => value.cpu.clock.value),
									},
									{
										label: "Min Clock Speed",
										data: minutes
											? $hardwareStatistics.minutes.map((value) => value.cpu.clock.min)
											: $hardwareStatistics.seconds.map((value) => value.cpu.clock.min),
									},
								],
							}}
						/>
						<ToggleButton selected={minutes} on:click={() => (minutes = !minutes)} />
					</div>
				</div>

				<div>
					<LineChart
						props={{
							id: "CPU_Clock_Speed",
							statistics: [
								{
									label: "Max Clock Speed",
									data: minutes
										? $hardwareStatistics.minutes.map((value) => value.cpu.clock.max)
										: $hardwareStatistics.seconds.map((value) => value.cpu.clock.max),
								},
								{
									label: "Current Clock Speed",
									data: minutes
										? $hardwareStatistics.minutes.map((value) => value.cpu.clock.value)
										: $hardwareStatistics.seconds.map((value) => value.cpu.clock.value),
								},
								{
									label: "Min Clock Speed",
									data: minutes
										? $hardwareStatistics.minutes.map((value) => value.cpu.clock.min)
										: $hardwareStatistics.seconds.map((value) => value.cpu.clock.min),
								},
							],
							time: minutes ? "m" : "s",
							unit: " Mhz",
							timestamp: minutes
								? $hardwareStatistics.minutes.map((value) => value?.timestamp ?? new Date().toISOString())
								: $hardwareStatistics.seconds.map((value) => value?.timestamp ?? new Date().toISOString()),
						}}
					/>
				</div>
			</div>
		</div>

		<div class="flex w-2/5 flex-col justify-start gap-5 sm:w-full">
			<!-- cpu load -->
			<div class="transparent-800 rounded-xl p-8 sm:p-4">
				<div class="flex items-baseline justify-between">
					<div class="mb-5 flex items-center gap-3">
						<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
							<Gauge />
						</div>
						<h2>Average Load</h2>
					</div>
					<div class="flex flex-row">
						<SaveDataButton
							props={{
								id: "CPU_Load",
								statistics: [
									{
										label: "Load",
										data: minutes
											? $hardwareStatistics.minutes.map((value) => value.cpu.load)
											: $hardwareStatistics.seconds.map((value) => value.cpu.load),
									},
								],
							}}
						/>
						<ToggleButton selected={minutes} on:click={() => (minutes = !minutes)} />
					</div>
				</div>

				<div>
					<LineChart
						props={{
							id: "CPU_Load",
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
							timestamp: minutes
								? $hardwareStatistics.minutes.map((value) => value?.timestamp ?? new Date().toISOString())
								: $hardwareStatistics.seconds.map((value) => value?.timestamp ?? new Date().toISOString()),
						}}
					/>
				</div>
			</div>

			<!-- cpu power usage -->
			{#if $hardwareInfo.cpu.power.length > 0}
				<div class="transparent-800 rounded-xl p-8 sm:p-4">
					<div class="flex items-baseline justify-between">
						<div class="mb-5 flex items-center gap-3">
							<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
								<Plug />
							</div>
							<h2>Power Usage</h2>
						</div>
						<div class="flex flex-row">
							<SaveDataButton
								props={{
									id: "CPU_Power_Usage",
									statistics: [
										{
											label: "Power Usage",
											data: minutes
												? $hardwareStatistics.minutes.map((value) => value.cpu.power)
												: $hardwareStatistics.seconds.map((value) => value.cpu.power),
										},
									],
								}}
							/>
							<ToggleButton selected={minutes} on:click={() => (minutes = !minutes)} />
						</div>
					</div>

					<div>
						<LineChart
							props={{
								id: "CPU_Power_Usage",
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
								timestamp: minutes
									? $hardwareStatistics.minutes.map((value) => value?.timestamp ?? new Date().toISOString())
									: $hardwareStatistics.seconds.map((value) => value?.timestamp ?? new Date().toISOString()),
							}}
						/>
					</div>
				</div>
			{/if}

			<!-- cpu voltage -->
			{#if $hardwareInfo.cpu.voltage.length > 0}
				<div class="transparent-800 rounded-xl p-8 sm:p-4">
					<div class="flex items-baseline justify-between">
						<div class="mb-5 flex items-center gap-3">
							<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
								<Zap />
							</div>
							<h2>Voltage</h2>
						</div>
						<div class="flex flex-row">
							<SaveDataButton
								props={{
									id: "CPU_Voltage",
									statistics: [
										{
											label: "Voltage",
											data: minutes
												? $hardwareStatistics.minutes.map((value) => value.cpu.voltage)
												: $hardwareStatistics.seconds.map((value) => value.cpu.voltage),
										},
									],
								}}
							/>
							<ToggleButton selected={minutes} on:click={() => (minutes = !minutes)} />
						</div>
					</div>

					<div>
						<LineChart
							props={{
								id: "CPU_Voltage",
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
								timestamp: minutes
									? $hardwareStatistics.minutes.map((value) => value?.timestamp ?? new Date().toISOString())
									: $hardwareStatistics.seconds.map((value) => value?.timestamp ?? new Date().toISOString()),
							}}
						/>
					</div>
				</div>
			{/if}
		</div>
	</div>
</div>

<script lang="ts">
	import { Clock, Cpu, Gauge, Plug, Thermometer, Zap } from "lucide-svelte"
	import { hardwareInfo, hardwareStatistics, LineChart, SaveDataButton, ToggleButton } from "ui"

	let minutes = $state(false)
</script>
