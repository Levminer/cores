<Line {data} {options} />

<script lang="ts">
	import { colors } from "@lib/utils"
	import { Chart, registerables } from "chart.js"
	import type { ChartOptions } from "chart.js"
	import { Line } from "svelte-chartjs"

	export let statistics: Sensor[]
	export let time: string
	export let unit: string = "°C"
	export let name: string = "Temperature"

	Chart.register(...registerables)

	$: min = statistics.map((value) => value.min)
	$: max = statistics.map((value) => value.max)
	$: value = statistics.map((value) => value.value)

	$: labels = value.map((_, i) => `${value.length - 1 - i}${time} ago`)

	$: data = {
		labels: labels,
		datasets: [
			{
				label: `Min ${name}`,
				data: min,
				backgroundColor: colors.min,
				borderColor: colors.min,
				tension: 0.2,
				pointHitRadius: 15,
				borderWidth: 4,
			},
			{
				label: `Current ${name}`,
				data: value,
				backgroundColor: colors.current,
				borderColor: colors.current,
				tension: 0.2,
				pointHitRadius: 15,
				borderWidth: 4,
			},
			{
				label: `Max ${name}`,
				data: max,
				backgroundColor: colors.max,
				borderColor: colors.max,
				tension: 0.2,
				pointHitRadius: 15,
				borderWidth: 4,
			},
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
				ticks: {
					color: "#969696",
					callback: (value) => {
						return `${value} ${unit}`
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

						return `${datasetLabel}: ${originalValue} ${unit}`
					},
				},
			},
			legend: {
				display: false,
			},
		},
	}
</script>
