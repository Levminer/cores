<div class="h-44">
	<Bar {data} {options} />
</div>

<script lang="ts">
	import { CategoryScale, Chart, registerables } from "chart.js"
	import ChartjsPluginStacked100 from "chartjs-plugin-stacked100"
	import type { ChartOptions, ChartDataset } from "chart.js"
	import { Bar } from "svelte-chartjs"
	import { hardwareInfo } from "../stores/hardwareInfo"

	export let load: Load[]
	export let pop: boolean

	Chart.register(...registerables, ChartjsPluginStacked100)

	if (pop) {
		load.pop()
	}

	let categories = load.map((load) => load.name.replaceAll("CPU", "").replaceAll("D3D", ""))

	if (categories.length < $hardwareInfo.cpu.load.length - 1) {
		categories = [...categories, ...Array($hardwareInfo.cpu.load.length - 1).fill("")]
	}

	$: loads = load.map((load) => Math.trunc(load.value))
	$: loadsPercentage = load.map((load) => Math.trunc(Math.abs(load.value - 100)))

	$: data = {
		labels: [...categories],
		datasets: [
			{ label: "Load", data: loads, backgroundColor: ["#00bbf9"] },
			{ label: "Load", data: loadsPercentage, backgroundColor: ["#262626"] },
		],
	}

	let options: ChartOptions<"bar"> = {
		responsive: true,
		maintainAspectRatio: false,
		indexAxis: "y",
		animation: {
			onComplete: (context) => {
				if (context.initial) {
					options.animation = false
				}
			},
		},
		plugins: {
			//@ts-ignore
			stacked100: {
				enable: true,
				replaceTooltipLabel: false,
			},
			legend: {
				display: false,
			},
			tooltip: {
				displayColors: false,
				callbacks: {
					label: (tooltipItem) => {
						const data = tooltipItem.chart.data
						const datasetIndex = tooltipItem.datasetIndex
						const index = tooltipItem.dataIndex
						const datasetLabel = data.datasets[datasetIndex].label || ""

						// @ts-ignore
						const originalValue = data.originalData[datasetIndex][index]

						if (datasetIndex === 0) {
							return `${datasetLabel}: ${originalValue}%`
						} else {
							return `${datasetLabel}: ${loads[index]}%`
						}
					},
					title: () => {
						return null
					},
				},
			},
		},
		scales: {
			x: {
				grid: { display: false },
				ticks: { display: false },
			},
			y: {
				grid: { display: false },
				ticks: { display: true, crossAlign: "far" },
			},
		},
	}
</script>
