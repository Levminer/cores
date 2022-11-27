<div class="transparent-900 m-10 rounded-xl w-4/5 mx-auto">
	<div class="flex justify-evenly gap-5 mx-10 pt-10">
		<div class="rounded-xl p-10 text-center transparent-800 w-1/3 flex flex-col items-center justify-center">
			<div>
				<GaugeChart data={CPUData} i={0} />
			</div>
			<div class="mt-10">
				<h2>CPU</h2>
				<div class="mt-5">
					<h3>{$hardwareInfo.CPU.name}</h3>
				</div>
			</div>
		</div>

		<div class="rounded-xl p-10 text-center transparent-800 w-1/3 flex flex-col items-center justify-center">
			<div>
				<GaugeChart data={RAMData} i={1} />
			</div>
			<div class="mt-10">
				<h2>RAM</h2>
				<div class="mt-5">
					<h3>Generic Memory</h3>
				</div>
			</div>
		</div>

		<div class="rounded-xl p-10 text-center transparent-800 w-1/3 flex flex-col items-center justify-center">
			<div>
				<GaugeChart data={GPUData} i={2} />
			</div>
			<div class="mt-10">
				<h2>GPU</h2>
				<div class="mt-5">
					<h3>{$hardwareInfo.GPU.name}</h3>
				</div>
			</div>
		</div>
	</div>

	<div class="flex gap-5 justify-evenly mx-10 mt-10 pb-10">
		<!-- CPU info -->
		<div class="text-left w-1/3">
			<div class="transparent-800 rounded-xl p-10">
				<div class="flex items-baseline gap-3 mb-5">
					<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M14 4v10.54a4 4 0 1 1-4 0V4a2 2 0 0 1 4 0Z" /></svg>
					<h2>CPU Temperature</h2>
				</div>
				<h3 class="mb-5">Avg. Temperature: {AvgCPUTemp} °C</h3>
				{#each $hardwareInfo.CPU.temperature as { value, min, max }, i}
					<h5>Core #{i}</h5>
					<MeterChart
						data={[
							{
								group: "Min",
								value: min,
							},
							{
								group: "Current",
								value: value,
							},
							{
								group: "Max",
								value: max,
							},
						]}
						{i}
					/>{/each}
			</div>
		</div>

		<!-- RAM info -->
		<div class="text-left w-1/3">
			<div class="transparent-800 rounded-xl p-10">
				<div class="flex items-baseline gap-3 mb-5">
					<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="bi bi-memory" viewBox="0 0 16 16">
						<path d="M1 3a1 1 0 0 0-1 1v8a1 1 0 0 0 1 1h4.586a1 1 0 0 0 .707-.293l.353-.353a.5.5 0 0 1 .708 0l.353.353a1 1 0 0 0 .707.293H15a1 1 0 0 0 1-1V4a1 1 0 0 0-1-1H1Zm.5 1h3a.5.5 0 0 1 .5.5v4a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-4a.5.5 0 0 1 .5-.5Zm5 0h3a.5.5 0 0 1 .5.5v4a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-4a.5.5 0 0 1 .5-.5Zm4.5.5a.5.5 0 0 1 .5-.5h3a.5.5 0 0 1 .5.5v4a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-4ZM2 10v2H1v-2h1Zm2 0v2H3v-2h1Zm2 0v2H5v-2h1Zm3 0v2H8v-2h1Zm2 0v2h-1v-2h1Zm2 0v2h-1v-2h1Zm2 0v2h-1v-2h1Z" />
					</svg>
					<h2>RAM Usage</h2>
				</div>
				<h3>Memory: {RAM}</h3>
				<h3>Virtual memory: {VRAM}</h3>
			</div>
		</div>

		<!-- GPU info -->
		<div class="text-left w-1/3">
			<div class="p-10 transparent-800 rounded-xl mb-10">
				<div class="flex items-baseline gap-3 mb-5">
					<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M14 4v10.54a4 4 0 1 1-4 0V4a2 2 0 0 1 4 0Z" /></svg>
					<h2>GPU Temperature</h2>
				</div>
				<h3>GPU Temperature: {GPUTemp} °C</h3>
			</div>

			<div class="p-10 transparent-800 rounded-xl">
				<div class="flex items-baseline gap-3 mb-5">
					<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M10.827 16.379a6.082 6.082 0 0 1-8.618-7.002l5.412 1.45a6.082 6.082 0 0 1 7.002-8.618l-1.45 5.412a6.082 6.082 0 0 1 8.618 7.002l-5.412-1.45a6.082 6.082 0 0 1-7.002 8.618l1.45-5.412Z" /><path d="M12 12v.01" /></svg>
					<h2>GPU Fans</h2>
				</div>
				{#each $hardwareInfo.GPU.fans as { value }, i}
					<h3>Fan #{i}: {value} RPM</h3>
				{/each}
			</div>
		</div>
	</div>
</div>

<script lang="ts">
	import { onDestroy, onMount } from "svelte"
	import { hardwareInfo, setHardwareInfo } from "../stores/hardwareInfo"
	let interval: NodeJS.Timer
	import GaugeChart from "../components/gaugeChart.svelte"
	import MeterChart from "../components/meterChart.svelte"

	$: hardwareInfo

	$: GPUTemp = 50
	$: RAM = "8GB/16 GB"
	$: VRAM = "10GB/20 GB"
	$: AvgCPUTemp = 50

	$: CPUData = [
		{
			group: "value",
			value: 10,
		},
		{
			group: "delta",
			value: 1,
		},
	]

	$: GPUData = [
		{
			group: "value",
			value: 10,
		},
		{
			group: "delta",
			value: 1,
		},
	]

	$: RAMData = [
		{
			group: "value",
			value: 10,
		},
		{
			group: "delta",
			value: 1,
		},
	]

	const init = () => {
		const input: HardwareInfo = JSON.parse(document.querySelector<HTMLInputElement>("#api").textContent)

		if (Object.keys(input).length !== 0) {
			setHardwareInfo(input)

			console.log(input)

			// Load graphs
			CPUData[0].value = Math.round(input.CPU.lastLoad)
			GPUData[0].value = Math.round(input.GPU.lastLoad)
			RAMData[0].value = Math.round(input.RAM.load[2].value)

			// RAM
			let usedRAM = input.RAM.load[0].value
			let availableRAM = input.RAM.load[1].value

			let usedVRAM = input.RAM.load[3].value
			let availableVRAM = input.RAM.load[4].value

			RAM = `${usedRAM.toFixed(1)}/${(usedRAM + availableRAM).toFixed(1)} GB`
			VRAM = `${usedVRAM.toFixed(1)}/${(usedVRAM + availableVRAM).toFixed(1)} GB`

			// CPU temperature
			let temp = 0

			for (let i = 0; i < input.CPU.temperature.length; i++) {
				temp += input.CPU.temperature[i].value
			}

			AvgCPUTemp = Math.trunc(temp / input.CPU.temperature.length)
			GPUTemp = input.GPU.temperature[0].value
		}
	}

	onMount(() => {
		init()

		interval = setInterval(() => {
			init()
		}, 1000)
	})

	onDestroy(() => {
		clearInterval(interval)
	})
</script>
