<div class="loadChart{id}">
	<Bar {data} {options} />
</div>

<script lang="ts">
	import { Chart, registerables } from "chart.js"
	import ChartjsPluginStacked100 from "chartjs-plugin-stacked100"
	import type { ChartOptions } from "chart.js"
	import { Bar } from "svelte-chartjs"
	import { hardwareInfo } from "../stores/hardwareInfo"
	import { onMount } from "svelte"
	import { colors } from "@lib/utils"

	export let load: Load[]
	let id = crypto.randomUUID()

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
			{ label: "Load", data: loads, backgroundColor: colors.min },
			{ label: "Load", data: loadsPercentage, backgroundColor: ["hsla(0, 0%, 100%, 3.26%)"] },
		],
	}

	// Resize chart to fit all data
	onMount(() => {
		document.querySelector<HTMLDivElement>(`.loadChart${id}`).style.height = $hardwareInfo.cpu.load.length * 25 + "px"
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
