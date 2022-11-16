<script lang="ts">
	import { GaugeChart } from "@carbon/charts-svelte";
	import { onMount } from "svelte";

	let cpuData = [
		{
			group: "value",
			value: 10,
		},
		{
			group: "delta",
			value: 1,
		},
	];

	onMount(() => {
		setInterval(() => {
			cpuData[0].value += 5;
		}, 1000);
	});
</script>

<div class="transparent-900 m-10 rounded-xl">
	<p id="test" class="bg-red-500">helloka</p>

	<div class="flex justify-evenly items-center">
		<div class="my-20 rounded-xl p-10 text-center bg-black text-white">
			<GaugeChart
				id="cpuChart"
				data={cpuData}
				options={{
					resizable: true,
					height: "150px",
					width: "100%",
					toolbar: {
						enabled: false,
					},
				}}
			/>
			<div class="mt-10">
				<h2>CPU</h2>
				<div class="mt-5">
					<h3>Intel Core i5-4570</h3>
				</div>
			</div>
		</div>

		<div class="my-20 rounded-xl p-10 text-center bg-black text-white">
			<GaugeChart
				id="cpuChart"
				data={cpuData}
				options={{
					resizable: true,
					height: "150px",
					width: "100%",
					toolbar: {
						enabled: false,
					},
				}}
			/>
			<h2 class="mt-10">GPU</h2>
		</div>
	</div>
</div>
