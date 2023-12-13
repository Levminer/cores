<Line {data} {options} />

<script lang="ts">
	import { Chart, registerables } from "chart.js"
	import type { ChartOptions } from "chart.js"
	import { Line } from "svelte-chartjs"

	export let statistics: number[]
	export let type: string
	export let unit: string
	export let color: string
	export let time: string
	export let zero = false

	Chart.register(...registerables)

	$: labels = statistics.map((_, i) => `${statistics.length - 1 - i}${time} ago`)

	$: data = {
		labels: labels,
		datasets: [{ label: type, data: statistics, backgroundColor: [color], borderColor: color, tension: 0.2, pointHitRadius: 15, borderWidth: 4 }],
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
				ticks: {
					callback: (value) => {
						return `${value}${unit}`
					},
					precision: 2,
					color: "#969696",
				},
				min: zero ? 0 : undefined,
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
				display: false,
			},
		},
	}
</script>
