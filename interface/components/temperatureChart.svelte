<Line {data} {options} />

<script lang="ts">
	import { Chart, registerables } from "chart.js"
	import type { ChartOptions } from "chart.js"
	import { Line } from "svelte-chartjs"

	export let statistics: { min: number[]; value: number[]; max: number[] }
	export let time: string

	Chart.register(...registerables)

	$: labels = statistics.value.map((_, i) => `${statistics.value.length - 1 - i}${time} ago`)

	$: data = {
		labels: labels,
		datasets: [
			{ label: "Min temperature", data: statistics.min, backgroundColor: ["#00bbf9"], borderColor: "#00bbf9", tension: 0.3, pointHitRadius: 15  },
			{ label: "Current temperature", data: statistics.value, backgroundColor: ["#f15bb5"], borderColor: "#f15bb5", tension: 0.3, pointHitRadius: 15 },
			{ label: "Max temperature", data: statistics.max, backgroundColor: ["#9b5de5"], borderColor: "#9b5de5", tension: 0.3, pointHitRadius: 15  },
		],
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
						return `${value} °C`
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

						return `${datasetLabel}: ${originalValue} °C`
					},
				},
			},
			legend: {
				position: "bottom",
			},
		},
	}
</script>
