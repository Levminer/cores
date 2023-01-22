<div class="transparent-900 m-10 mx-auto w-4/5 rounded-xl sm:m-3 sm:w-full">
	<div class="mx-10 flex justify-evenly gap-5 pt-10 sm:flex-wrap">
		<div class="transparent-800 flex w-1/3 flex-col rounded-xl p-10 pt-0 text-center sm:w-full">
			<div class="mx-auto flex w-full justify-center sm:w-1/2">
				<GaugeChart load={$hardwareInfo.cpu.lastLoad} />
			</div>
			<div>
				<h2>CPU</h2>
				<div class="mt-5">
					<h3>{$hardwareInfo.cpu.name}</h3>
				</div>
				<div class="mt-3 flex justify-center">
					<svg class={`${loadGraphsShown ? "mb-3 rotate-180 transform" : ""} cursor-pointer text-white`} on:click={showLoadGraphs} on:keydown={showLoadGraphs} xmlns="http://www.w3.org/2000/svg" width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><polyline points="6 9 12 15 18 9" /></svg>
				</div>
				<div>
					{#if loadGraphs}
						{#await loadGraphs then { default: ProgressBar }}
							<ProgressBar load={$hardwareInfo.cpu.load} i={0} />
						{/await}
					{/if}
				</div>
			</div>
		</div>

		<div class="transparent-800 flex w-1/3 flex-col rounded-xl p-10 pt-0 text-center sm:w-full">
			<div class="mx-auto flex w-full justify-center sm:w-1/2">
				<GaugeChart load={$hardwareInfo.ram.load[2].value} />
			</div>
			<div>
				<h2>RAM</h2>
				<div class="mt-5">
					<h3>Generic Memory</h3>
				</div>
				<div class="mt-3 flex justify-center">
					<svg class={`${loadGraphsShown ? "mb-3 rotate-180 transform" : ""} cursor-pointer text-white`} on:click={showLoadGraphs} on:keydown={showLoadGraphs} xmlns="http://www.w3.org/2000/svg" width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><polyline points="6 9 12 15 18 9" /></svg>
				</div>
				<div>
					{#if loadGraphs}
						{#await loadGraphs then { default: ProgressBar }}
							<ProgressBar load={[$hardwareInfo.ram.load[5]]} i={1} />
						{/await}
					{/if}
				</div>
			</div>
		</div>

		<div class="transparent-800 flex w-1/3 flex-col rounded-xl p-10 pt-0 text-center sm:w-full">
			<div class="mx-auto flex w-full justify-center sm:w-1/2">
				<GaugeChart load={$hardwareInfo.gpu.lastLoad} />
			</div>
			<div>
				<h2>GPU</h2>
				<div class="mt-5">
					<h3>{$hardwareInfo.gpu.name}</h3>
				</div>
				<div class="mt-3 flex justify-center">
					<svg class={`${loadGraphsShown ? "mb-3 rotate-180 transform" : ""} cursor-pointer text-white`} on:click={showLoadGraphs} on:keydown={showLoadGraphs} xmlns="http://www.w3.org/2000/svg" width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><polyline points="6 9 12 15 18 9" /></svg>
				</div>
				<div>
					{#if loadGraphs}
						{#await loadGraphs then { default: ProgressBar }}
							<ProgressBar load={$hardwareInfo.gpu.load} i={2} />
						{/await}
					{/if}
				</div>
			</div>
		</div>
	</div>

	<div class="mx-10 mt-5 flex gap-5 pb-10 sm:flex-wrap">
		<!-- CPU info -->
		<div class="flex w-1/3 flex-col gap-5 text-left sm:w-full">
			<div class="transparent-800 rounded-xl p-10">
				<div class="mb-5 flex items-baseline gap-3">
					<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M14 4v10.54a4 4 0 1 1-4 0V4a2 2 0 0 1 4 0Z" /></svg>
					<h2>CPU Temperature</h2>
				</div>
				<h3>Avg. temperature: {Math.round($hardwareInfo.cpu.temperature.reduce((a, b) => a + b.value, 0) / $hardwareInfo.cpu.temperature.length)} °C</h3>
				<div>
					<MeterChart readings={$hardwareInfo.cpu.temperature} categories={$hardwareInfo.cpu.temperature.map((temp, i) => `Core #${i} (${temp.value} °C)`)} i={0} type={{ name: "temperature", unit: "°C" }} />
				</div>
			</div>

			<div class="transparent-800 rounded-xl p-10">
				<div class="mb-5 flex items-baseline gap-3">
					<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><circle cx="12" cy="12" r="10" /><polyline points="12 6 12 12 16 14" /></svg>
					<h2>CPU Clock speed</h2>
				</div>
				<h3>Avg. clock speed: {(Math.round($hardwareInfo.cpu.clock.reduce((a, b) => a + b.value, 0) / $hardwareInfo.cpu.clock.length) / 1000).toFixed(1)} GHz</h3>
				<div>
					<MeterChart readings={$hardwareInfo.cpu.clock} categories={$hardwareInfo.cpu.clock.map((temp, i) => `Core #${i} (${(temp.value / 1000).toFixed(1)} GHz)`)} i={2} type={{ name: "clock speed", unit: "MHz" }} />
				</div>
			</div>

			<div class="transparent-800 rounded-xl p-10">
				<div class="mb-5 flex items-baseline gap-3">
					<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M12 22v-5" /><path d="M9 7V2" /><path d="M15 7V2" /><path d="M6 13V8h12v5a4 4 0 0 1-4 4h-4a4 4 0 0 1-4-4Z" /></svg>
					<h2>CPU Power usage</h2>
				</div>
				<h3>Power usage: {$hardwareInfo.cpu.power.reduce((a, b) => a + b.value, 0)} W</h3>
				<div>
					<MeterChart readings={$hardwareInfo.cpu.power.filter((power) => power.value !== 0)} categories={$hardwareInfo.cpu.power.filter((power) => power.value !== 0).map((temp) => `${temp.name.replaceAll("CPU", "")} (${temp.value} W)`)} i={3} type={{ name: "power usage", unit: "W" }} />
				</div>
			</div>

			<div class="transparent-800 rounded-xl p-10">
				<div class="mb-5 flex items-baseline gap-3">
					<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><polygon points="13 2 3 14 12 14 11 22 21 10 12 10 13 2" /></svg>
					<h2>CPU Voltage</h2>
				</div>
				<h3>Avg. voltage: {($hardwareInfo.cpu.voltage.reduce((a, b) => a + b.value, 0) / $hardwareInfo.cpu.voltage.length).toFixed(1)} V</h3>
				<div>
					<MeterChart readings={$hardwareInfo.cpu.voltage} categories={$hardwareInfo.cpu.voltage.map((temp, i) => `Core #${i} (${temp.value} V)`)} i={5} type={{ name: "voltage", unit: "V" }} />
				</div>
			</div>
		</div>

		<!-- RAM info -->
		<div class="flex w-1/3 flex-col gap-5 text-left sm:w-full">
			<div class="transparent-800 rounded-xl p-10">
				<div class="mb-5 flex items-baseline gap-3">
					<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="bi bi-memory" viewBox="0 0 16 16">
						<path d="M1 3a1 1 0 0 0-1 1v8a1 1 0 0 0 1 1h4.586a1 1 0 0 0 .707-.293l.353-.353a.5.5 0 0 1 .708 0l.353.353a1 1 0 0 0 .707.293H15a1 1 0 0 0 1-1V4a1 1 0 0 0-1-1H1Zm.5 1h3a.5.5 0 0 1 .5.5v4a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-4a.5.5 0 0 1 .5-.5Zm5 0h3a.5.5 0 0 1 .5.5v4a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-4a.5.5 0 0 1 .5-.5Zm4.5.5a.5.5 0 0 1 .5-.5h3a.5.5 0 0 1 .5.5v4a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-4ZM2 10v2H1v-2h1Zm2 0v2H3v-2h1Zm2 0v2H5v-2h1Zm3 0v2H8v-2h1Zm2 0v2h-1v-2h1Zm2 0v2h-1v-2h1Zm2 0v2h-1v-2h1Z" />
					</svg>
					<h2>RAM Usage</h2>
				</div>
				<h3>Memory: {`${$hardwareInfo.ram.load[0].value.toFixed(1)}/${($hardwareInfo.ram.load[0].value + $hardwareInfo.ram.load[1].value).toFixed(1)} GB`}</h3>
				<div>
					<MeterChart readings={[$hardwareInfo.ram.load[0]]} categories={["RAM usage"]} i={8} type={{ name: "memory usage", unit: "GB" }} />
				</div>
			</div>

			<div class="transparent-800 rounded-xl p-10">
				<div class="mb-5 flex items-baseline gap-3">
					<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="bi bi-memory" viewBox="0 0 16 16">
						<path d="M1 3a1 1 0 0 0-1 1v8a1 1 0 0 0 1 1h4.586a1 1 0 0 0 .707-.293l.353-.353a.5.5 0 0 1 .708 0l.353.353a1 1 0 0 0 .707.293H15a1 1 0 0 0 1-1V4a1 1 0 0 0-1-1H1Zm.5 1h3a.5.5 0 0 1 .5.5v4a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-4a.5.5 0 0 1 .5-.5Zm5 0h3a.5.5 0 0 1 .5.5v4a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-4a.5.5 0 0 1 .5-.5Zm4.5.5a.5.5 0 0 1 .5-.5h3a.5.5 0 0 1 .5.5v4a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-4ZM2 10v2H1v-2h1Zm2 0v2H3v-2h1Zm2 0v2H5v-2h1Zm3 0v2H8v-2h1Zm2 0v2h-1v-2h1Zm2 0v2h-1v-2h1Zm2 0v2h-1v-2h1Z" />
					</svg>
					<h2>Virtual RAM Usage</h2>
				</div>
				<h3>Virtual memory: {`${$hardwareInfo.ram.load[3].value.toFixed(1)}/${($hardwareInfo.ram.load[3].value + $hardwareInfo.ram.load[4].value).toFixed(1)} GB`}</h3>
				<div>
					<MeterChart readings={[$hardwareInfo.ram.load[3]]} categories={["Virtual RAM usage"]} i={9} type={{ name: "virtual memory usage", unit: "GB" }} />
				</div>
			</div>
		</div>

		<!-- GPU info -->
		<div class="flex w-1/3 flex-col gap-5 text-left sm:w-full">
			<div class="transparent-800 rounded-xl p-10">
				<div class="mb-5 flex items-baseline gap-3">
					<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M14 4v10.54a4 4 0 1 1-4 0V4a2 2 0 0 1 4 0Z" /></svg>
					<h2>GPU Temperature</h2>
				</div>
				<h3>Avg. temperature: {Math.round($hardwareInfo.gpu.temperature.reduce((a, b) => a + b.value, 0) / $hardwareInfo.gpu.temperature.length)} °C</h3>
				<div>
					<MeterChart readings={$hardwareInfo.gpu.temperature} categories={$hardwareInfo.gpu.temperature.map((temp) => `${temp.name.replaceAll("GPU", "")} (${temp.value} °C)`)} i={1} type={{ name: "temperature", unit: "°C" }} />
				</div>
			</div>

			<div class="transparent-800 rounded-xl p-10">
				<div class="mb-5 flex items-baseline gap-3">
					<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M10.827 16.379a6.082 6.082 0 0 1-8.618-7.002l5.412 1.45a6.082 6.082 0 0 1 7.002-8.618l-1.45 5.412a6.082 6.082 0 0 1 8.618 7.002l-5.412-1.45a6.082 6.082 0 0 1-7.002 8.618l1.45-5.412Z" /><path d="M12 12v.01" /></svg>
					<h2>GPU Fan Speed</h2>
				</div>
				<h3>Avg. fan speed: {Math.round($hardwareInfo.gpu.fans.reduce((a, b) => a + b.value, 0) / $hardwareInfo.gpu.fans.length)} RPM</h3>
				<div>
					<MeterChart categories={$hardwareInfo.gpu.fans.map((temp, i) => `Fan #${i} (${temp.value} RPM)`)} readings={$hardwareInfo.gpu.fans} i={7} type={{ name: "fan speed", unit: "RPM" }} />
				</div>
			</div>

			<div class="transparent-800 rounded-xl p-10">
				<div class="mb-5 flex items-baseline gap-3">
					<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="bi bi-memory" viewBox="0 0 16 16">
						<path d="M1 3a1 1 0 0 0-1 1v8a1 1 0 0 0 1 1h4.586a1 1 0 0 0 .707-.293l.353-.353a.5.5 0 0 1 .708 0l.353.353a1 1 0 0 0 .707.293H15a1 1 0 0 0 1-1V4a1 1 0 0 0-1-1H1Zm.5 1h3a.5.5 0 0 1 .5.5v4a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-4a.5.5 0 0 1 .5-.5Zm5 0h3a.5.5 0 0 1 .5.5v4a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-4a.5.5 0 0 1 .5-.5Zm4.5.5a.5.5 0 0 1 .5-.5h3a.5.5 0 0 1 .5.5v4a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-4ZM2 10v2H1v-2h1Zm2 0v2H3v-2h1Zm2 0v2H5v-2h1Zm3 0v2H8v-2h1Zm2 0v2h-1v-2h1Zm2 0v2h-1v-2h1Zm2 0v2h-1v-2h1Z" />
					</svg>
					<h2>GPU Memory Usage</h2>
				</div>
				<h3>GPU memory: {`${($hardwareInfo.gpu.memory[0].value).toFixed(1)}/${$hardwareInfo.gpu.memory[2].value} GB`} GB</h3>
				<div>
					<MeterChart readings={[$hardwareInfo.gpu.memory[0]]} categories={["GPU memory usage"]} i={10} type={{ name: "GPU memory usage", unit: "GB" }} />
				</div>
			</div>

			<div class="transparent-800 rounded-xl p-10">
				<div class="mb-5 flex items-baseline gap-3">
					<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><circle cx="12" cy="12" r="10" /><polyline points="12 6 12 12 16 14" /></svg>
					<h2>GPU Clock speed</h2>
				</div>
				<h3>Avg. clock speed: {(Math.round($hardwareInfo.gpu.clock.reduce((a, b) => a + b.value, 0) / $hardwareInfo.gpu.clock.length) / 1000).toFixed(1)} GHz</h3>
				<div>
					<MeterChart readings={$hardwareInfo.gpu.clock} categories={$hardwareInfo.gpu.clock.map((temp) => `${temp.name.replaceAll("GPU", "")} (${(temp.value / 1000).toFixed(1)} GHz)`)} i={6} type={{ name: "clock speed", unit: "MHz" }} />
				</div>
			</div>

			<div class="transparent-800 rounded-xl p-10">
				<div class="mb-5 flex items-baseline gap-3">
					<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M12 22v-5" /><path d="M9 7V2" /><path d="M15 7V2" /><path d="M6 13V8h12v5a4 4 0 0 1-4 4h-4a4 4 0 0 1-4-4Z" /></svg>
					<h2>GPU Power usage</h2>
				</div>
				<h3>Power usage: {$hardwareInfo.gpu.power.reduce((a, b) => a + b.value, 0)} W</h3>
				<div>
					<MeterChart readings={$hardwareInfo.gpu.power} categories={$hardwareInfo.gpu.power.map((temp) => `${temp.name.replaceAll("GPU", "")} (${temp.value} W)`)} i={4} type={{ name: "power usage", unit: "W" }} />
				</div>
			</div>
		</div>
	</div>
</div>

<script lang="ts">
	import { onDestroy, onMount } from "svelte"
	import { hardwareInfo, setHardwareInfo } from "../stores/hardwareInfo"
	import { hardwareStatistics } from "../stores/hardwareStatistics"
	import GaugeChart from "../components/gaugeChart.svelte"
	import MeterChart from "../components/meterChart.svelte"

	let loadGraphs = null
	let loadGraphsShown = false

	// Show load graphs
	const showLoadGraphs = () => {
		if (!loadGraphsShown) {
			loadGraphs = import("../components/loadChart.svelte")

			loadGraphsShown = true
		} else {
			loadGraphs = null

			loadGraphsShown = false
		}
	}

	let observer: MutationObserver

	// Watch for changes in the DOM
	observer = new MutationObserver(() => {
		updateHardwareInfo()
	})

	// Watch for API changes
	observer.observe(document.querySelector("#api"), {
		attributes: true,
		childList: true,
		characterData: true,
	})

	// Update hardware info
	const updateHardwareInfo = () => {
		const input: HardwareInfo = JSON.parse(document.querySelector<HTMLInputElement>("#api").textContent)

		if (Object.keys(input).length !== 0) {
			setHardwareInfo(input)
		}
	}

	onDestroy(() => {
		observer.disconnect()
	})
</script>
