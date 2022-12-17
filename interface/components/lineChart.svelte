<Line {data} {options} />

<script lang="ts">
	import { Chart, registerables } from "chart.js"
	import type { ChartOptions } from "chart.js"
	import { Line } from "svelte-chartjs"

	export let statistics: number[]
	export let type: string

	Chart.register(...registerables)

	let labels = []

	for (let i = 0; i < 59; i++) {
		labels.push(`${59 - i}s ago`)
	}

	$: data = {
		labels: labels,
		datasets: [{ label: type, data: statistics, backgroundColor: ["#ffd60a"], borderColor: "#ffd60a" }],
	}

	let options: ChartOptions<"line"> = {
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
				ticks: {
					callback: (value) => {
						return `${value} W`
					},
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

						return `${datasetLabel}: ${originalValue} W`
					},
				},
			},
			legend: {
				position: "bottom",
			},
		},
	}
</script>
