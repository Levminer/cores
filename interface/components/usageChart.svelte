<Line {data} {options} />

<script lang="ts">
	import { colors } from "@lib/utils"
	import { Chart, registerables } from "chart.js"
	import type { ChartOptions } from "chart.js"
	import { Line } from "svelte-chartjs"

	export let statistics: number[]
	export let type: string
	export let unit: string
	export let time: string

	Chart.register(...registerables)

	$: labels = statistics.map((_, i) => `${statistics.length - 1 - i}${time} ago`)

	$: data = {
		labels: labels,
		datasets: [
			{ label: type, data: statistics, backgroundColor: colors.min, borderColor: colors.min, tension: 0.2, fill: true, pointHitRadius: 15 },
		],
	}

	let options: ChartOptions<"line"> = {
		elements: {
			point: {
				radius: 0,
			},
		},
		animation: {
			onProgress: (context) => {
				if (context.initial) {
					options.animation = false
				}
			},
			duration: 0,
		},

		scales: {
			y: {
				beginAtZero: true,
				suggestedMax: 100,
				ticks: {
					callback: (value) => {
						return `${value}${unit}`
					},
					stepSize: 10,
				},
			},
			x: {
				ticks: {
					display: false,
				},
			},
		},
		plugins: {
			tooltip: {
				callbacks: {
					label: (tooltipItem) => {
						const data = tooltipItem.chart.data
						const datasetIndex = tooltipItem.datasetIndex
						const index = tooltipItem.dataIndex
						const datasetLabel = data.datasets[datasetIndex].label || ""

						const originalValue = data.datasets[datasetIndex].data[index]

						return `${datasetLabel}: ${originalValue}${unit}`
					},
				},
			},
			legend: {
				position: "bottom",
			},
		},
	}
</script>
