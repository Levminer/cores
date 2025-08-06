<canvas bind:this={canvas} id={props.id}></canvas>

<script lang="ts">
	import { Chart, registerables } from "chart.js"
	import type { ChartOptions } from "chart.js"
	import { onMount } from "svelte"
	import { colors } from "../utils/colors.ts"

	interface Props {
		props: {
			id?: string
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
			timestamp?: string[]
		}
	}

	let { props }: Props = $props()

	let canvas: HTMLCanvasElement
	let chart: Chart<"line">

	Chart.register(...registerables)

	// Initialize chart when component mounts
	onMount(() => {
		if (canvas) {
			chart = new Chart(canvas, {
				type: "line",
				data: data,
				options: options,
			})
		}

		return () => {
			if (chart) {
				chart.destroy()
			}
		}
	})

	// Update chart when data changes
	$effect(() => {
		if (chart && data) {
			chart.data = data
			chart.update()
		}
	})

	const labels = $derived(
		props.timestamp
			? props.timestamp.map((timestamp: string) => {
					const date = new Date(timestamp)
					return `${date.toLocaleTimeString()}`
				})
			: (props.statistics[0].data?.map((_, i) => `${props.statistics[0].data!.length - 1 - i}${props.time} ago`) ?? []),
	)

	const data = $derived({
		labels: labels,
		datasets: [
			...props.statistics.map((value, index) => {
				return {
					label: value.label,
					data: value.data ?? [],
					backgroundColor: value.color ? colors[value.color] : colors.categoricalPalette[index % colors.categoricalPalette.length],
					borderColor: value.color ? colors[value.color] : colors.categoricalPalette[index % colors.categoricalPalette.length],
					tension: 0.2,
					pointHitRadius: 15,
					borderWidth: 4,
					fill: value.fill,
				}
			}),
		],
	})

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
				min: props.min || 0 >= 0 ? props.min : undefined,
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
					callback: function (value, index, ticks) {
						if (index === 0 || index === ticks.length - 1) {
							// @ts-ignore show full label for first and last tick
							return this.getLabelForValue(value)
						}
						return ""
					},
					maxRotation: 0,
					minRotation: 0,
					color: "#969696",
					autoSkip: false,
				},
				// TODO: adjust grid lines
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
