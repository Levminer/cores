<div class="transparent-900 m-10 rounded-xl w-4/5 mx-auto">
	<div class="flex justify-evenly gap-5 mx-5">
		<div class="mt-20 rounded-xl p-10 text-center transparent-800 w-1/3 flex flex-col items-center justify-center">
			<div>
				<GaugeChart
					data={CPUData}
					options={{
						height: "150px",
						toolbar: {
							enabled: false,
						},
						color: {
							scale: {
								value: "#00a2ed",
							},
						},
						theme: ChartTheme.G100,
					}}
				/>
			</div>
			<div class="mt-10">
				<h2>CPU</h2>
				<div class="mt-5">
					<h3>{$hardwareInfo.CPU.name}</h3>
				</div>
			</div>
		</div>

		<div class="mt-20 rounded-xl p-10 text-center transparent-800 w-1/3 flex flex-col items-center justify-center">
			<div>
				<GaugeChart
					data={RAMData}
					options={{
						height: "150px",
						width: "100%",
						toolbar: {
							enabled: false,
						},
						color: {
							scale: {
								value: "#00a2ed",
							},
						},
						theme: ChartTheme.G100,
					}}
				/>
			</div>
			<div class="mt-10">
				<h2>RAM</h2>
				<div class="mt-5">
					<h3>Generic Memory</h3>
				</div>
			</div>
		</div>

		<div class="mt-20 rounded-xl p-10 text-center transparent-800 w-1/3 flex flex-col items-center justify-center">
			<div>
				<GaugeChart
					data={GPUData}
					options={{
						height: "150px",
						width: "100%",
						toolbar: {
							enabled: false,
						},
						color: {
							scale: {
								value: "#00a2ed",
							},
						},
						theme: ChartTheme.G100,
					}}
				/>
			</div>
			<div class="mt-10">
				<h2>GPU</h2>
				<div class="mt-5">
					<h3>{$hardwareInfo.GPU.name}</h3>
				</div>
			</div>
		</div>
	</div>

	<div class="flex justify-evenly gap-5 mx-5 mt-10 pb-20">
		<div class="rounded-xl p-10 text-left transparent-800 w-1/3">
			<h3 class="mb-5">CPU Temperature: {AvgCPUTemp} °C</h3>
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
					options={{
						height: "50px",
						toolbar: {
							enabled: false,
						},
						legend: {
							enabled: false,
						},
						meter: {
							proportional: {
								unit: "C",
								total: 300,
							},
							showLabels: false,
						},
						color: {
							pairing: {
								option: 2,
							},
						},
						theme: ChartTheme.G100,
					}}
				/>{/each}
		</div>

		<div class="rounded-xl p-10 text-left transparent-800 w-1/3">
			<h3 class="mb-5">Used memory: {RAM}</h3>
			<h3 class="mb-5">Used virtual memory: {VRAM}</h3>
		</div>

		<div class="rounded-xl p-10 text-left transparent-800 w-1/3">
			<h3 class="mb-5">GPU Temperature: {GPUTemp} °C</h3>
		</div>
	</div>
</div>

<script lang="ts">
	import { GaugeChart, MeterChart } from "@carbon/charts-svelte"
	import { ChartTheme } from "@carbon/charts/interfaces"
	import { onDestroy, onMount } from "svelte"
	import { hardwareInfo, setHardwareInfo } from "../stores/hardwareInfo"
	let interval: NodeJS.Timer

	$: hardwareInfo

	$: GPUTemp = 50
	$: RAM = "8GB/16GB"
	$: VRAM = "10GB/20GB"
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

			RAM = `${usedRAM.toFixed(1)}/${(usedRAM + availableRAM).toFixed(1)}GB`
			VRAM = `${usedVRAM.toFixed(1)}/${(usedVRAM + availableVRAM).toFixed(1)}GB`

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
		}, 2500)
	})

	onDestroy(() => {
		clearInterval(interval)
	})
</script>
