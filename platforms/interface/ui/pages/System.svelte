<div class="transparent-900 m-10 mx-auto w-11/12 rounded-xl sm:w-full">
	<div class="mx-10 flex gap-5 pt-10 sm:mx-3 sm:flex-wrap">
		<div class="flex w-full flex-row items-start justify-start gap-5 sm:flex-wrap">
			<!-- System info -->
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
					<h3>GPU: {$hardwareInfo.gpu.cards?.[0]?.name ?? "N/A"}</h3>
					<h3>MB: {$hardwareInfo.system.motherboard.name}</h3>
					<h3>OS: {$hardwareInfo.system.os.name}</h3>
				</div>
			</div>

			<!-- BIOS info -->
			{#if $hardwareInfo.system.bios.vendor !== "N/A"}
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
			{/if}
		</div>
	</div>

	<div class="mx-10 flex gap-5 pb-10 sm:mx-3 sm:flex-wrap">
		<div class="flex w-full flex-col justify-start gap-5 pt-5 sm:flex-wrap">
			<!-- fan read/write and temperature -->
			{#each $hardwareInfo.system.superIO.fan.filter((item) => item.value !== 0) as item, i}
				<div class="flex gap-5 sm:flex-wrap">
					<div class="w-1/2 sm:w-full">
						<ChartTile
							title="Speed"
							item={item?.name || "Fan"}
							bind:minutes
							props={{
								id: `Fan_Speed_${i}`,
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
								timestamp: minutes
									? $hardwareStatistics.minutes.map((value) => value?.timestamp ?? new Date().toISOString())
									: $hardwareStatistics.seconds.map((value) => value?.timestamp ?? new Date().toISOString()),
							}}
						>
							{#snippet icon()}
								<Fan />
							{/snippet}
						</ChartTile>
					</div>

					<div class="w-1/2 sm:w-full">
						<ChartTile
							title="Usage"
							item={item?.name || "Fan"}
							bind:minutes
							props={{
								id: `Fan_Usage_${i}`,
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
			{/each}
		</div>
	</div>
</div>

<script lang="ts">
	import { Fan, Gauge, CircuitBoard } from "lucide-svelte"
	import { PcDisplay } from "svelte-bootstrap-icons"
	import { hardwareInfo, hardwareStatistics } from "ui"
	import ChartTile from "../components/ChartTile.svelte"

	let minutes = $state(false)
</script>
