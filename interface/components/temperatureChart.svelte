<Line {data} {options} />

<script lang="ts">
	import { Chart, registerables } from "chart.js"
	import type { ChartOptions } from "chart.js"
	import { Line } from "svelte-chartjs"

	export let statistics: { min: number[]; value: number[]; max: number[] }

	Chart.register(...registerables)

	let labels = []

	for (let i = 0; i < statistics.min.length; i++) {
		labels.push(`${i}`)
	}

	$: data = {
		labels: labels,
		datasets: [
			{ label: "Min", data: statistics.min, backgroundColor: ["#00bbf9"], borderColor: "#00bbf9" },
			{ label: "Current", data: statistics.value, backgroundColor: ["#f15bb5"], borderColor: "#f15bb5" },
			{ label: "Max", data: statistics.max, backgroundColor: ["#9b5de5"], borderColor: "#9b5de5" },
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
