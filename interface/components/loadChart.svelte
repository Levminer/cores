<div class="loadChart{i}">
	<Bar {data} {options} />
</div>

<script lang="ts">
	import { CategoryScale, Chart, registerables } from "chart.js"
	import ChartjsPluginStacked100 from "chartjs-plugin-stacked100"
	import type { ChartOptions, ChartDataset } from "chart.js"
	import { Bar } from "svelte-chartjs"
	import { hardwareInfo } from "../stores/hardwareInfo"
	import { onMount } from "svelte"

	export let load: Load[]
	export let i: number

	Chart.register(...registerables, ChartjsPluginStacked100)

	// Clean up the labels
	let categories = load.map((load) => load.name.replaceAll("CPU", "").replaceAll("D3D", ""))

	// Add empty labels to match the number of cores
	if (categories.length < $hardwareInfo.cpu.load.length) {
		categories = [...categories, ...Array($hardwareInfo.cpu.load.length - categories.length).fill("")]
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

	// Resize chart to fit all data
	onMount(() => {
		document.querySelector<HTMLDivElement>(`.loadChart${i}`).style.height = $hardwareInfo.cpu.load.length * 20 + "px"
	})

	// Chart options
	let options: ChartOptions<"bar"> = {
		responsive: true,
		maintainAspectRatio: false,
		indexAxis: "y",
		animation: {
			onProgress: (context) => {
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
