<div id="gaugeChart{i}" style="width: 100%; height: 100%;" />

<script lang="ts">
	import { ChartTheme } from "@carbon/charts/interfaces"
	import { GaugeChart } from "@carbon/charts"
	import { onMount } from "svelte"

	export let data
    export let i

	onMount(() => {
		const options = {
			height: "150px",
			toolbar: {
				enabled: false,
			},
			color: {
				scale: {
					value: "#00a2ed",
				},
			},
			theme: ChartTheme.G100,
		}

		var chart = new GaugeChart(document.getElementById(`gaugeChart${i}`), {
			data,
			options,
		})

		setInterval(() => {
			chart.model.setData(data)
		}, 1000)
	})
</script>
