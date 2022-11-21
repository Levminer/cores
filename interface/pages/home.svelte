<div class="transparent-900 m-10 rounded-xl w-4/5 mx-auto">
	<p id="test" class="hidden">{"{}"}</p>

	<div class="flex justify-evenly items-center gap-5 mx-5">
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
					<h3>{CPUName}</h3>
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
					data={CPUData}
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
					<h3>{GPUName}</h3>
				</div>
			</div>
		</div>
	</div>

	<div class="flex justify-evenly gap-5 mx-5 mt-10 pb-20">
		<div class="rounded-xl p-10 text-left transparent-800 w-1/3 text-white">
			<h3 class="mb-5">CPU Temperature: {AvgCPUTemp} °C</h3>
			{#each CPUTemp as { value, min, max }, i}
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

		<div class="rounded-xl p-10 text-center transparent-800 w-1/3">
			<h1>yo</h1>
		</div>
	</div>
</div>

<script lang="ts">
	import { GaugeChart, MeterChart } from "@carbon/charts-svelte"
	import { ChartTheme } from "@carbon/charts/interfaces"
	import { onDestroy, onMount } from "svelte"
	let interval: NodeJS.Timer
	let first = false

	$: CPUName = "CPUName"
	$: GPUName = "GPUName"
	$: RAM = "8GB/16GB"
	$: VRAM = "10GB/20GB"
	$: AvgCPUTemp = 50
	$: CPUTemp = [
		{
			value: 50,
			min: 40,
			max: 60,
		},
		{
			value: 55,
			min: 45,
			max: 65,
		},
	]

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
		const input: HardwareInfo = JSON.parse(document.querySelector("#test").textContent)

		console.log(input)

		if (Object.keys(input).length !== 0) {
			if (first == false) {
				CPUName = input.CPU.name
				GPUName = input.GPU.name

				first = true
			}

			CPUData[0].value = Math.round(input.CPU.lastLoad)
			RAMData[0].value = Math.round(input.RAM.load[2].value)

			let usedRAM = input.RAM.load[0].value
			let availableRAM = input.RAM.load[1].value

			let usedVRAM = input.RAM.load[3].value
			let availableVRAM = input.RAM.load[4].value

			RAM = `${usedRAM.toFixed(1)}/${(usedRAM + availableRAM).toFixed(1)}GB`
			VRAM = `${usedVRAM.toFixed(1)}/${(usedVRAM + availableVRAM).toFixed(1)}GB`

			let temp = 0

			for (let i = 0; i < input.CPU.temperature.length; i++) {
				temp += input.CPU.temperature[i].value

				CPUTemp[i] = {
					value: input.CPU.temperature[i].value,
					min: input.CPU.temperature[i].min,
					max: input.CPU.temperature[i].max,
				}
			}

			AvgCPUTemp = Math.trunc(temp / input.CPU.temperature.length)
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
