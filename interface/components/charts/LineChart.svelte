<Line {data} {options} />

<script lang="ts">
	import { colors } from "@lib/utils"
	import { Chart, registerables } from "chart.js"
	import type { ChartOptions } from "chart.js"
	import { Line } from "svelte-chartjs"

	interface Props {
		statistics: {
			label?: string
			data?: number[]
		}[]
		unit: string
		color: string
		time: string
		zero: boolean
	}

	export let props: Props = {
		statistics: [{}],
		unit: "",
		color: "",
		time: "",
		zero: false,
	}

	Chart.register(...registerables)

	$: labels = props.statistics[0].data.map((_, i) => `${props.statistics[0].data.length - 1 - i}${props.time} ago`)

	$: data = {
		labels: labels,
		datasets: [
			...props.statistics.map((value, index) => {
				return {
					label: value.label,
					data: value.data,
					backgroundColor: colors.categoricalPalette[index % colors.categoricalPalette.length],
					borderColor: colors.categoricalPalette[index % colors.categoricalPalette.length],
					tension: 0.2,
					pointHitRadius: 15,
					borderWidth: 4,
				}
			}),
		],
	}

	let options: ChartOptions<"line"> = {
		elements: {
			point: {
				radius: 0,
			},
		},
		interaction: {
			intersect: false,
			mode: "index",
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
						return `${value}${props.unit}`
					},
					precision: 2,
					color: "#969696",
				},
				min: props.zero ? 0 : undefined,
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

						return `${datasetLabel}: ${originalValue}${props.unit}`
					},
				},
			},
			legend: {
				display: false, // TODO consider adding a legend
			},
		},
	}
</script>
