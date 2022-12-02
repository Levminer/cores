<Bar {data} {options} />

<script lang="ts">
	import { Chart, registerables } from "chart.js"
	import ChartjsPluginStacked100 from "chartjs-plugin-stacked100"
	import type { ChartOptions } from "chart.js"
	import { Bar } from "svelte-chartjs"

	export let temps
	export let categories

	console.log(temps)

	Chart.register(...registerables, ChartjsPluginStacked100)

	$: data = {
		labels: categories,
		datasets: [
			{ label: "Min", data: temps[0].data, backgroundColor: ["#00bbf9"] },
			{ label: "Current", data: temps[1].data, backgroundColor: ["#f15bb5"] },
			{ label: "Max", data: temps[2].data, backgroundColor: ["#9b5de5"] },
		],
	}

	let options: ChartOptions<"bar"> = {
		responsive: true,
		maintainAspectRatio: false,
		indexAxis: "y",

		plugins: {
			stacked100: {
				enable: true,
			},
			legend: {
				display: false,
			},
		},
		scales: {
			x: {
				grid: { display: false },
				ticks: { display: false },
			},
			y: {
				grid: { display: false },
				ticks: { display: true },
			},
		},
	}
</script>
