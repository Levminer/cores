<canvas bind:this={canvasElement}></canvas>

<script lang="ts">
	import { onMount } from "svelte"
	import { Chart, registerables } from "chart.js"
	import type { ChartOptions } from "chart.js"
	import { colors } from "../utils/colors.ts"

	interface Props {
		load: number
	}

	let { load }: Props = $props()

	let canvasElement: HTMLCanvasElement | undefined
	let chart: Chart<"doughnut">

	Chart.register(...registerables)

	// Initialize chart when component mounts
	onMount(() => {
		if (canvasElement) {
			chart = new Chart(canvasElement, {
				type: "doughnut",
				data: data,
				options: options,
				plugins: pluginTest,
			})

			return () => {
				if (chart) {
					chart.destroy()
				}
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

	const options: ChartOptions<"doughnut"> = {
		rotation: 0,
		circumference: 360,
		cutout: "85%",
		hover: {
			// @ts-ignore
			mode: null,
		},
		plugins: {
			legend: {
				display: false,
			},
			tooltip: {
				enabled: false,
			},
		},
		elements: {
			arc: {
				borderWidth: 0,
			},
		},
		animation: {
			onProgress: (context) => {
				if (context.initial) {
					options.animation = false
				}
			},
		},
	}

	const pluginTest = [
		{
			id: "text",
			beforeDraw: function (chart: any, a: any, b: any) {
				var width = chart.width,
					height = chart.height,
					ctx = chart.ctx

				ctx.restore()
				var fontSize = (height / 85).toFixed(2)
				ctx.font = fontSize + "em sans-serif"
				ctx.textBaseline = "middle"
				ctx.fillStyle = "white"

				var text = percentage.toString() + "%",
					textX = Math.round((width - ctx.measureText(text).width) / 2),
					textY = height / 2

				ctx.fillText(text, textX, textY)
				ctx.save()
			},
		},
	]

	const percentage = $derived(Math.trunc(load))
	const total = $derived(percentage - 100)

	const data = $derived({
		datasets: [
			{
				data: [percentage, total],
				backgroundColor: [colors.min, "hsla(0, 0%, 100%, 3.26%)"],
			},
		],
	})
</script>
