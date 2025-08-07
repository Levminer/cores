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
				{#if $hardwareInfo.gpu.cards?.length > 0}
					{#each $hardwareInfo.gpu.cards as card}
						<div class="mt-5 select-text">
							<h3>Vendor: {card.name?.split(" ")[0] ?? "N/A"}</h3>
							<h3>Name: {card.name ?? "N/A"}</h3>
							<h3>GPU memory: {card.memory.length > 2 ? Math.round(card.memory[2]?.value ?? 0) : "N/A"} GB</h3>
							<h3>Driver: {$hardwareInfo.gpu.info}</h3>
						</div>
					{/each}
				{:else}
					<h3>No GPU information available.</h3>
				{/if}
			</div>

			{#if $hardwareInfo.gpu.cards?.length > 0}
				{#each $hardwareInfo.gpu.cards as item, i}
					<!-- gpu temperature -->
					<div class="transparent-800 rounded-xl p-8 sm:p-4">
						<div class="flex items-baseline justify-between">
							<div class="mb-5 flex items-center gap-3">
								<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
									<Thermometer />
								</div>
								<div>
									<h2><span class="line-clamp-1">{item.name}</span> Average Temperature</h2>
								</div>
							</div>
							<div class="flex flex-row">
								<SaveDataButton
									props={{
										id: "GPU_Temperature",
										statistics: [
											{
												label: "Max Temperature",
												data: minutes
													? $hardwareStatistics.minutes.map((value) => value.gpu.cards[i].temperature.max)
													: $hardwareStatistics.seconds.map((value) => value.gpu.cards[i].temperature.max),
											},
											{
												label: "Current Temperature",
												data: minutes
													? $hardwareStatistics.minutes.map((value) => value.gpu.cards[i].temperature.value)
													: $hardwareStatistics.seconds.map((value) => value.gpu.cards[i].temperature.value),
											},
											{
												label: "Min Temperature",
												data: minutes
													? $hardwareStatistics.minutes.map((value) => value.gpu.cards[i].temperature.min)
													: $hardwareStatistics.seconds.map((value) => value.gpu.cards[i].temperature.min),
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
									id: "GPU_Temperature",
									statistics: [
										{
											label: "Max Temperature",
											color: "max",
											data: minutes
												? $hardwareStatistics.minutes.map((value) => value.gpu.cards[i].temperature.max)
												: $hardwareStatistics.seconds.map((value) => value.gpu.cards[i].temperature.max),
										},
										{
											label: "Current Temperature",
											color: "current",
											data: minutes
												? $hardwareStatistics.minutes.map((value) => value.gpu.cards[i].temperature.value)
												: $hardwareStatistics.seconds.map((value) => value.gpu.cards[i].temperature.value),
										},
										{
											label: "Min Temperature",
											color: "min",
											data: minutes
												? $hardwareStatistics.minutes.map((value) => value.gpu.cards[i].temperature.min)
												: $hardwareStatistics.seconds.map((value) => value.gpu.cards[i].temperature.min),
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

					<!-- gpu clock speed -->
					<div class="transparent-800 rounded-xl p-8 sm:p-4">
						<div class="flex items-baseline justify-between">
							<div class="mb-5 flex items-center gap-3">
								<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
									<Clock />
								</div>
								<h2><span class="line-clamp-1">{item.name}</span> Core Clock Speed</h2>
							</div>
							<div class="flex flex-row">
								<SaveDataButton
									props={{
										id: "GPU_Clock_Speed",
										statistics: [
											{
												label: "Max Clock Speed",
												data: minutes
													? $hardwareStatistics.minutes.map((value) => value.gpu.cards[i].clock.max)
													: $hardwareStatistics.seconds.map((value) => value.gpu.cards[i].clock.max),
											},
											{
												label: "Current Clock Speed",
												data: minutes
													? $hardwareStatistics.minutes.map((value) => value.gpu.cards[i].clock.value)
													: $hardwareStatistics.seconds.map((value) => value.gpu.cards[i].clock.value),
											},
											{
												label: "Min Clock Speed",
												data: minutes
													? $hardwareStatistics.minutes.map((value) => value.gpu.cards[i].clock.min)
													: $hardwareStatistics.seconds.map((value) => value.gpu.cards[i].clock.min),
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
									id: "GPU_Clock_Speed",
									statistics: [
										{
											label: "Max Clock Speed",
											data: minutes
												? $hardwareStatistics.minutes.map((value) => value.gpu.cards[i].clock.max)
												: $hardwareStatistics.seconds.map((value) => value.gpu.cards[i].clock.max),
										},
										{
											label: "Current Clock Speed",
											data: minutes
												? $hardwareStatistics.minutes.map((value) => value.gpu.cards[i].clock.value)
												: $hardwareStatistics.seconds.map((value) => value.gpu.cards[i].clock.value),
										},
										{
											label: "Min Clock Speed",
											data: minutes
												? $hardwareStatistics.minutes.map((value) => value.gpu.cards[i].clock.min)
												: $hardwareStatistics.seconds.map((value) => value.gpu.cards[i].clock.min),
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
				{/each}
			{/if}
		</div>

		<div class="flex w-2/5 flex-col justify-start gap-5 sm:w-full">
			{#if $hardwareInfo.gpu.cards?.length > 0}
				{#each $hardwareInfo.gpu.cards as item, i}
					<div class="transparent-800 rounded-xl p-8 sm:p-4">
						<div class="flex items-baseline justify-between">
							<div class="mb-5 flex items-center gap-3">
								<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
									<Gauge />
								</div>
								<h2><span class="line-clamp-1">{item.name}</span> Average Load</h2>
							</div>
							<div class="flex flex-row">
								<SaveDataButton
									props={{
										id: "GPU_Load",
										statistics: [
											{
												label: "Load",
												data: minutes
													? $hardwareStatistics.minutes.map((value) => value.gpu.cards[i].load)
													: $hardwareStatistics.seconds.map((value) => value.gpu.cards[i].load),
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
									id: "GPU_Load",
									statistics: [
										{
											label: "Load",
											color: "min",
											fill: true,
											data: minutes
												? $hardwareStatistics.minutes.map((value) => value.gpu.cards[i].load)
												: $hardwareStatistics.seconds.map((value) => value.gpu.cards[i].load),
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

					<!-- gpu power usage -->
					<div class="transparent-800 rounded-xl p-8 sm:p-4">
						<div class="flex items-baseline justify-between">
							<div class="mb-5 flex items-center gap-3">
								<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
									<Zap />
								</div>
								<h2><span class="line-clamp-1">{item.name}</span> Power Usage</h2>
							</div>
							<div class="flex flex-row">
								<SaveDataButton
									props={{
										id: "GPU_Power_Usage",
										statistics: [
											{
												label: "Power Usage",
												data: minutes
													? $hardwareStatistics.minutes.map((value) => value.gpu.cards[i].power)
													: $hardwareStatistics.seconds.map((value) => value.gpu.cards[i].power),
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
									id: "GPU_Power_Usage",
									statistics: [
										{
											label: "Power Usage",
											color: "yellow",
											data: minutes
												? $hardwareStatistics.minutes.map((value) => value.gpu.cards[i].power)
												: $hardwareStatistics.seconds.map((value) => value.gpu.cards[i].power),
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

					<!-- gpu fan usage -->
					<div class="transparent-800 rounded-xl p-8 sm:p-4">
						<div class="flex items-baseline justify-between">
							<div class="mb-5 flex items-center gap-3">
								<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
									<Fan />
								</div>
								<h2><span class="line-clamp-1">{item.name}</span> Fan Speed</h2>
							</div>
							<div class="flex flex-row">
								<SaveDataButton
									props={{
										id: "GPU_Load",
										statistics: [
											{
												label: "Load",
												data: minutes
													? $hardwareStatistics.minutes.map((value) => value.gpu.cards[i].fan)
													: $hardwareStatistics.seconds.map((value) => value.gpu.cards[i].fan),
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
									id: "GPU_Fan_Speed",
									statistics: [
										{
											label: "Fan Speed",
											color: "min",
											data: minutes
												? $hardwareStatistics.minutes.map((value) => value.gpu.cards[i].fan)
												: $hardwareStatistics.seconds.map((value) => value.gpu.cards[i].fan),
										},
									],
									time: minutes ? "m" : "s",
									unit: " RPM",
									min: 0,
									timestamp: minutes
										? $hardwareStatistics.minutes.map((value) => value?.timestamp ?? new Date().toISOString())
										: $hardwareStatistics.seconds.map((value) => value?.timestamp ?? new Date().toISOString()),
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
								<h2><span class="line-clamp-1">{item.name}</span> Memory Usage</h2>
							</div>
							<div class="flex flex-row">
								<SaveDataButton
									props={{
										id: "GPU_Memory_Usage",
										statistics: [
											{
												label: "Memory Usage",
												data: minutes
													? $hardwareStatistics.minutes.map((value) => value.gpu.cards[i].memory)
													: $hardwareStatistics.seconds.map((value) => value.gpu.cards[i].memory),
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
									id: "GPU_Memory_Usage",
									statistics: [
										{
											label: "Memory Usage",
											color: "min",
											data: minutes
												? $hardwareStatistics.minutes.map((value) => value.gpu.cards[i].memory)
												: $hardwareStatistics.seconds.map((value) => value.gpu.cards[i].memory),
										},
									],
									time: minutes ? "m" : "s",
									unit: " GB",
									min: 0,
									timestamp: minutes
										? $hardwareStatistics.minutes.map((value) => value?.timestamp ?? new Date().toISOString())
										: $hardwareStatistics.seconds.map((value) => value?.timestamp ?? new Date().toISOString()),
								}}
							/>
						</div>
					</div>
				{/each}
			{/if}
		</div>
	</div>
</div>

<script lang="ts">
	import { GpuCard, Memory } from "svelte-bootstrap-icons"
	import { Clock, Fan, Gauge, Thermometer, Zap } from "lucide-svelte"
	import { hardwareInfo, hardwareStatistics, LineChart, ToggleButton, SaveDataButton } from "ui"

	let minutes = $state(false)
</script>
