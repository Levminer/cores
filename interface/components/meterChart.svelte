<div id="meterChart{i}" style="width: 100%; height: 100%;" />

<script lang="ts">
	import { ChartTheme } from "@carbon/charts/interfaces"
	import { MeterChart } from "@carbon/charts"
	import { onMount } from "svelte"

	export let data
	export let i

	onMount(() => {
		const options = {
			height: "50px",
			toolbar: {
				enabled: false,
			},
			legend: {
				enabled: false,
			},
			meter: {
				proportional: {
					unit: "C",
					total: 300,
				},
				showLabels: false,
			},
			color: {
				pairing: {
					option: 2,
				},
			},
			theme: ChartTheme.G100,
		}

		var chart = new MeterChart(document.getElementById(`meterChart${i}`), {
			data,
			options,
		})

		setInterval(() => {
			chart.model.setData(data)
		}, 2500)
	})
</script>
