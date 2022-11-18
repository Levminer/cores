<div class="transparent-900 m-10 rounded-xl w-4/5 mx-auto">
	<p id="test" class="bg-red-500">{"{}"}</p>

	<div class="flex justify-evenly items-center gap-5 mx-5">
		<div class="mt-20 rounded-xl p-10 text-center transparent-800 w-1/3 flex flex-col items-center justify-center">
			<div>
				<GaugeChart
					class="!mx-auto flex justify-center items-center"
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
					<h3>mem</h3>
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

	<div class="flex justify-evenly items-center gap-5 mx-5 mt-10 pb-20">
		<div class="rounded-xl p-10 text-lef transparent-800 w-1/3 text-white">
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

		<div class="rounded-xl p-10 text-center transparent-800 w-1/3">
			<h1>yo</h1>
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

	$: CPUName = "CPUName"
	$: GPUName = "GPUName"
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
			CPUName = input.CPUName
			GPUName = input.GPUName
			CPUData[0].value = parseInt(input.CPULoadLast)
			RAMData[0].value = parseInt(input.RAM[2].value)

			CPUTemp = []

			for (let i = 0; i < input.CPUTemp.length; i++) {
				CPUTemp.push({
					value: parseInt(input.CPUTemp[i].value),
					min: parseInt(input.CPUTemp[i].min),
					max: parseInt(input.CPUTemp[i].max),
				})
			}
		}
	}

	onMount(() => {
		interval = setInterval(() => {
			init()
		}, 1000)
	})

	onDestroy(() => {
		clearInterval(interval)
	})
</script>
