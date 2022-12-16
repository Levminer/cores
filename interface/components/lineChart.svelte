<Line {data} {options} />

<script lang="ts">
	import { Chart, registerables } from "chart.js"
	import type { ChartOptions } from "chart.js"
	import { Line } from "svelte-chartjs"

	export let statistics: number[]

	Chart.register(...registerables)

	let labels = []

	for (let i = 0; i < statistics.length; i++) {
		labels.push(`${i}`)
	}

	$: data = {
		labels: labels,
		datasets: [
			{ label: "Value", data: statistics, backgroundColor: ["#ffd60a"], borderColor: "#ffd60a" },
		],
	}

	let options: ChartOptions<"line"> = {
		animation: {
			onComplete: (context) => {
				if (context.initial) {
					options.animation = false
				}
			},
		},
	}
</script>
