<Bar {data} {options} />

<script lang="ts">
	import { Chart, registerables } from "chart.js"
	import ChartjsPluginStacked100 from "chartjs-plugin-stacked100"
	import type { ChartOptions } from "chart.js"
	import { Bar } from "svelte-chartjs"

	export let readings: Sensor[]
	export let categories

	Chart.register(...registerables, ChartjsPluginStacked100)

	$: temps = [{ data: readings.map((temp) => temp.min) }, { data: readings.map((temp) => temp.value) }, { data: readings.map((temp) => temp.max) }]

	$: data = {
		labels: categories,
		datasets: [
			{ label: "Min", data: temps[0].data, backgroundColor: ["#00bbf9"] },
			{ label: "Current", data: temps[1].data, backgroundColor: ["#f15bb5"] },
			{ label: "Max", data: temps[2].data, backgroundColor: ["#9b5de5"] },
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

						return `${datasetLabel}: ${originalValue} °C`
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
