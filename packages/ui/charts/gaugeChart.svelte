<Doughnut {data} {options} plugins={pluginTest} />

<script lang="ts">
	import { colors } from "ui/utils/colors.ts"
	import { Chart, registerables } from "chart.js"
	import type { ChartOptions } from "chart.js"
	import { Doughnut } from "svelte-chartjs"

	export let load

	Chart.register(...registerables)

	let options: ChartOptions<"doughnut"> = {
		responsive: true,
		maintainAspectRatio: true,
		rotation: 270,
		circumference: 180,
		cutout: "90%",
		hover: {
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

	let pluginTest = [
		{
			id: "text",
			beforeDraw: function (chart, a, b) {
				var width = chart.width,
					height = chart.height,
					ctx = chart.ctx

				ctx.restore()
				var fontSize = (height / 114).toFixed(2)
				ctx.font = fontSize + "em sans-serif"
				ctx.textBaseline = "middle"
				ctx.fillStyle = "white"

				var text = percentage.toString() + "%",
					textX = Math.round((width - ctx.measureText(text).width) / 2),
					textY = (height + 30) / 2

				ctx.fillText(text, textX, textY)
				ctx.save()
			},
		},
	]

	$: percentage = parseInt(load)
	$: total = percentage - 100

	$: data = {
		datasets: [
			{
				data: [percentage, total],
				backgroundColor: [colors.min, "hsla(0, 0%, 100%, 3.26%)"],
			},
		],
	}
</script>
