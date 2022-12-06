<div class="transparent-900 m-10 rounded-xl w-4/5 mx-auto">
	<div class="flex justify-evenly gap-5 mx-10 pt-10">
		<div class="rounded-xl p-10 pt-0 text-center transparent-800 w-1/3 flex flex-col items-center justify-center">
			<div class="w-full">
				<GaugeChart load={$hardwareInfo.cpu.lastLoad} />
			</div>
			<div>
				<h2>CPU</h2>
				<div class="mt-5">
					<h3>{$hardwareInfo.cpu.name}</h3>
				</div>
			</div>
		</div>

		<div class="rounded-xl p-10 pt-0 text-center transparent-800 w-1/3 flex flex-col items-center justify-center">
			<div class="w-full">
				<GaugeChart load={$hardwareInfo.ram.load[2].value} />
			</div>
			<div>
				<h2>RAM</h2>
				<div class="mt-5">
					<h3>Generic Memory</h3>
				</div>
			</div>
		</div>

		<div class="rounded-xl p-10 pt-0 text-center transparent-800 w-1/3 flex flex-col items-center justify-center">
			<div class="w-full">
				<GaugeChart load={$hardwareInfo.gpu.lastLoad} />
			</div>
			<div>
				<h2>GPU</h2>
				<div class="mt-5">
					<h3>{$hardwareInfo.gpu.name}</h3>
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
				<h3>Avg. Temperature: {AvgCPUTemp} °C</h3>
				<div>
					<MeterChart temps={CPUTemps} categories={CPUCategories} />
				</div>
			</div>
		</div>

		<!-- RAM info -->
		<div class="text-left w-1/3">
			<div class="transparent-800 rounded-xl p-10 mb-10">
				<div class="flex items-baseline gap-3 mb-5">
					<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="bi bi-memory" viewBox="0 0 16 16">
						<path d="M1 3a1 1 0 0 0-1 1v8a1 1 0 0 0 1 1h4.586a1 1 0 0 0 .707-.293l.353-.353a.5.5 0 0 1 .708 0l.353.353a1 1 0 0 0 .707.293H15a1 1 0 0 0 1-1V4a1 1 0 0 0-1-1H1Zm.5 1h3a.5.5 0 0 1 .5.5v4a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-4a.5.5 0 0 1 .5-.5Zm5 0h3a.5.5 0 0 1 .5.5v4a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-4a.5.5 0 0 1 .5-.5Zm4.5.5a.5.5 0 0 1 .5-.5h3a.5.5 0 0 1 .5.5v4a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-4ZM2 10v2H1v-2h1Zm2 0v2H3v-2h1Zm2 0v2H5v-2h1Zm3 0v2H8v-2h1Zm2 0v2h-1v-2h1Zm2 0v2h-1v-2h1Zm2 0v2h-1v-2h1Z" />
					</svg>
					<h2>RAM Usage</h2>
				</div>
				<h3>Memory: {RAM}</h3>
				<h3>Virtual memory: {VRAM}</h3>
			</div>

			<div class="transparent-800 rounded-xl p-10">
				<div class="flex items-baseline gap-3">
					<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="bi bi-motherboard" viewBox="0 0 16 16">
						<path d="M11.5 2a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 .5-.5Zm2 0a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 .5-.5Zm-10 8a.5.5 0 0 0 0 1h6a.5.5 0 0 0 0-1h-6Zm0 2a.5.5 0 0 0 0 1h6a.5.5 0 0 0 0-1h-6ZM5 3a1 1 0 0 0-1 1h-.5a.5.5 0 0 0 0 1H4v1h-.5a.5.5 0 0 0 0 1H4a1 1 0 0 0 1 1v.5a.5.5 0 0 0 1 0V8h1v.5a.5.5 0 0 0 1 0V8a1 1 0 0 0 1-1h.5a.5.5 0 0 0 0-1H9V5h.5a.5.5 0 0 0 0-1H9a1 1 0 0 0-1-1v-.5a.5.5 0 0 0-1 0V3H6v-.5a.5.5 0 0 0-1 0V3Zm0 1h3v3H5V4Zm6.5 7a.5.5 0 0 0-.5.5v1a.5.5 0 0 0 .5.5h2a.5.5 0 0 0 .5-.5v-1a.5.5 0 0 0-.5-.5h-2Z" />
						<path d="M1 2a2 2 0 0 1 2-2h11a2 2 0 0 1 2 2v11a2 2 0 0 1-2 2H3a2 2 0 0 1-2-2v-2H.5a.5.5 0 0 1-.5-.5v-1A.5.5 0 0 1 .5 9H1V8H.5a.5.5 0 0 1-.5-.5v-1A.5.5 0 0 1 .5 6H1V5H.5a.5.5 0 0 1-.5-.5v-2A.5.5 0 0 1 .5 2H1Zm1 11a1 1 0 0 0 1 1h11a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1H3a1 1 0 0 0-1 1v11Z" />
					</svg>
					<h2>RAM Modules</h2>
				</div>
				{#each $hardwareInfo.ram.modules as { bankLabel, manufacturer, speed, partNumber, serialNumber, capacity }}
					<div class="mt-5">
						<h3>Bank: {bankLabel}</h3>
						<h3>Speed: {speed} MHz</h3>
						<h3>Capacity: {capacity / 1024 / 1024 / 1024} GB</h3>
					</div>
				{/each}
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
				{#each $hardwareInfo.gpu.fans as { value }, i}
					<h3>Fan #{i}: {value} RPM</h3>
				{/each}
			</div>
		</div>
	</div>
</div>

<script lang="ts">
	import { onDestroy, onMount } from "svelte"
	import { hardwareInfo, setHardwareInfo } from "../stores/hardwareInfo"
	import GaugeChart from "../components/gaugeChart.svelte"
	import MeterChart from "../components/meterChart.svelte"

	let observer: MutationObserver

	// Watch for changes in the DOM
	observer = new MutationObserver(() => {
		init()
	})

	// Start observing the target
	observer.observe(document.querySelector("#api"), {
		attributes: true,
		childList: true,
		characterData: true,
	})

	$: GPUTemp = 50
	$: RAM = "8GB/16 GB"
	$: VRAM = "10GB/20 GB"
	$: AvgCPUTemp = 50

	$: CPUTemps = [
		{
			data: $hardwareInfo.cpu.temperature.map((temp) => temp.min),
		},
		{
			data: $hardwareInfo.cpu.temperature.map((temp) => temp.value),
		},
		{
			data: $hardwareInfo.cpu.temperature.map((temp) => temp.max),
		},
	]

	$: CPUCategories = $hardwareInfo.cpu.temperature.map((temp, i) => `Core #${i} (${temp.value} °C)`)

	const init = () => {
		const input: HardwareInfo = JSON.parse(document.querySelector<HTMLInputElement>("#api").textContent)

		if (Object.keys(input).length !== 0) {
			setHardwareInfo(input)

			console.log(input)

			// RAM
			let usedRAM = input.ram.load[0].value
			let availableRAM = input.ram.load[1].value

			let usedVRAM = input.ram.load[3].value
			let availableVRAM = input.ram.load[4].value

			RAM = `${usedRAM.toFixed(1)}/${(usedRAM + availableRAM).toFixed(1)} GB`
			VRAM = `${usedVRAM.toFixed(1)}/${(usedVRAM + availableVRAM).toFixed(1)} GB`

			// CPU temperature
			let temp = 0

			for (let i = 0; i < input.cpu.temperature.length; i++) {
				temp += input.cpu.temperature[i].value
			}

			AvgCPUTemp = Math.trunc(temp / input.cpu.temperature.length)
			GPUTemp = input.gpu.temperature[0].value
		}
	}

	onDestroy(() => {
		observer.disconnect()
	})
</script>
