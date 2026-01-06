<div class="meterChart{id}">
	<canvas bind:this={canvasElement}></canvas>
</div>

<script lang="ts">
	import { Chart, registerables } from "chart.js"
	import ChartjsPluginStacked100 from "chartjs-plugin-stacked100"
	import type { ChartOptions } from "chart.js"
	import { colors } from "../utils/colors.ts"
	import { onMount } from "svelte"

	interface Props {
		readings: Sensor[]
		categories: string[]
		type: { name: string; unit: string }
	}

	let { readings, categories, type }: Props = $props()

	const id = crypto.randomUUID()
	let canvasElement: HTMLCanvasElement
	let chart: Chart<"bar"> | null = null
	let lastCategories: string[] = $state(categories)

	Chart.register(...registerables, ChartjsPluginStacked100)

	const temps = $derived([
		{ data: readings.map((temp) => temp.min) },
		{ data: readings.map((temp) => temp.value) },
		{ data: readings.map((temp) => temp.max) },
	])

	const data = $derived({
		labels: categories,
		datasets: [
			{ label: `Min ${type.name}`, data: temps[0].data, backgroundColor: colors.min },
			{ label: `Current ${type.name}`, data: temps[1].data, backgroundColor: colors.current },
			{ label: `Max ${type.name}`, data: temps[2].data, backgroundColor: colors.max },
		],
	})

	// Initialize chart when component mounts
	onMount(() => {
		if (canvasElement) {
			chart = new Chart(canvasElement, {
				type: "bar",
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

	// Resize chart to fit all data
	$effect(() => {
		if (categories.length > lastCategories.length) {
			document.querySelector<HTMLDivElement>(`.meterChart${id}`)!.style.height = readings.length * 40 + "px"

			lastCategories = categories
		} else {
			// potential bottleneck
			if (readings.length < 2) {
				document.querySelector<HTMLDivElement>(`.meterChart${id}`)!.style.height = readings.length * 55 + "px"
			} else if (readings.length < 3) {
				document.querySelector<HTMLDivElement>(`.meterChart${id}`)!.style.height = readings.length * 45 + "px"
			} else {
				document.querySelector<HTMLDivElement>(`.meterChart${id}`)!.style.height = readings.length * 40 + "px"
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
				ticks: {
					display: true,
					crossAlign: "far",
					color: "#969696",
					// @ts-ignore
					callback: function (value: number) {
						const label = this.getLabelForValue(value)

						if (label.length > 20) {
							return label.slice(0, 17) + "..."
						} else {
							return label
						}
					},
				},
			},
		},
	}
</script>
