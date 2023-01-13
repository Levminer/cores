<div class="meterChart{i}">
	<Bar {data} {options} />
</div>

<script lang="ts">
	import { Chart, registerables } from "chart.js"
	import ChartjsPluginStacked100 from "chartjs-plugin-stacked100"
	import type { ChartOptions } from "chart.js"
	import { Bar } from "svelte-chartjs"
	import { afterUpdate, onMount } from "svelte"

	export let readings: Sensor[]
	export let categories: string[]
	export let i: number
	export let type: { name: string; unit: string }

	let lastCategories: string[] = categories

	Chart.register(...registerables, ChartjsPluginStacked100)

	$: temps = [{ data: readings.map((temp) => temp.min) }, { data: readings.map((temp) => temp.value) }, { data: readings.map((temp) => temp.max) }]

	$: data = {
		labels: categories,
		datasets: [
			{ label: `Min ${type.name}`, data: temps[0].data, backgroundColor: ["#00bbf9"] },
			{ label: `Current ${type.name}`, data: temps[1].data, backgroundColor: ["#f15bb5"] },
			{ label: `Max ${type.name}`, data: temps[2].data, backgroundColor: ["#9b5de5"] },
		],
	}

	// Resize chart to fit all data
	afterUpdate(() => {
		if (categories.length > lastCategories.length) {
			document.querySelector<HTMLDivElement>(`.meterChart${i}`).style.height = readings.length * 40 + "px"

			lastCategories = categories
		} else {
			// potential bottleneck
			if (readings.length < 2) {
				document.querySelector<HTMLDivElement>(`.meterChart${i}`).style.height = readings.length * 55 + "px"
			} else if (readings.length < 3) {
				document.querySelector<HTMLDivElement>(`.meterChart${i}`).style.height = readings.length * 45 + "px"
			} else {
				document.querySelector<HTMLDivElement>(`.meterChart${i}`).style.height = readings.length * 40 + "px"
			}
		}
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
			// @ts-ignore
			stacked100: {
				enable: true,
				replaceTooltipLabel: false,
			},
			legend: {
				display: false,
			},
			tooltip: {
				callbacks: {
					label: (tooltipItem) => {
						const data = tooltipItem.chart.data
						const datasetIndex = tooltipItem.datasetIndex
						const index = tooltipItem.dataIndex
						const datasetLabel = data.datasets[datasetIndex].label || ""

						// @ts-ignore
						const originalValue = data.originalData[datasetIndex][index]

						return `${datasetLabel}: ${originalValue} ${type.unit}`
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
