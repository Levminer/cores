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
			fill?: boolean
			color?: "min" | "max" | "current" | "yellow" | "orange"
		}[]
		unit: string
		time: string
		min?: number
		max?: number
		step?: number
	}

	export let props: Props = {
		statistics: [{}],
		unit: "",
		time: "",
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
					backgroundColor: value.color ? colors[value.color] : colors.categoricalPalette[index % colors.categoricalPalette.length],
					borderColor: value.color ? colors[value.color] : colors.categoricalPalette[index % colors.categoricalPalette.length],
					tension: 0.2,
					pointHitRadius: 15,
					borderWidth: 4,
					fill: value.fill,
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
				max: props.max ? props.max : undefined,
				min: props.min >= 0 ? props.min : undefined,
				ticks: {
					callback: (value) => {
						return `${value}${props.unit}`
					},
					precision: 2,
					color: "#969696",
					stepSize: props.step ? props.step : undefined,
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
