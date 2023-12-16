<div class="transparent-900 m-10 mx-auto w-11/12 rounded-xl sm:w-full">
	<!-- Row 1 -->
	<div class="mx-10 flex justify-evenly gap-5 pt-10 sm:mx-3 sm:flex-wrap">
		<div class="transparent-800 flex w-1/3 flex-col rounded-xl p-10 pt-0 text-center sm:w-full">
			<div class="mx-auto flex w-4/5 justify-center sm:w-1/2">
				<GaugeChart load={$hardwareInfo.cpu.lastLoad} />
			</div>
			<div>
				<h2>CPU</h2>
				<div class="mt-5">
					<h3>{$hardwareInfo.cpu.name}</h3>
				</div>
				<div class="mt-3 flex justify-center">
					<svg
						class={`${loadGraphsShown ? "mb-3 rotate-180 transform" : ""} cursor-pointer text-white`}
						on:click={showLoadGraphs}
						on:keydown={showLoadGraphs}
						xmlns="http://www.w3.org/2000/svg"
						width="32"
						height="32"
						viewBox="0 0 24 24"
						fill="none"
						stroke="currentColor"
						stroke-width="2"
						stroke-linecap="round"
						stroke-linejoin="round"><polyline points="6 9 12 15 18 9" /></svg
					>
				</div>
				<div>
					{#if loadGraphs}
						{#await loadGraphs then { default: ProgressBar }}
							<ProgressBar load={$hardwareInfo.cpu.load} />
						{/await}
					{/if}
				</div>
			</div>
		</div>

		<div class="transparent-800 flex w-1/3 flex-col rounded-xl p-10 pt-0 text-center sm:w-full">
			<div class="mx-auto flex w-4/5 justify-center sm:w-1/2">
				<GaugeChart load={$hardwareInfo.ram.load[2].value} />
			</div>
			<div>
				<h2>RAM</h2>
				<div class="mt-5">
					<h3>Generic Memory</h3>
				</div>
				<div class="mt-3 flex justify-center">
					<svg
						class={`${loadGraphsShown ? "mb-3 rotate-180 transform" : ""} cursor-pointer text-white`}
						on:click={showLoadGraphs}
						on:keydown={showLoadGraphs}
						xmlns="http://www.w3.org/2000/svg"
						width="32"
						height="32"
						viewBox="0 0 24 24"
						fill="none"
						stroke="currentColor"
						stroke-width="2"
						stroke-linecap="round"
						stroke-linejoin="round"><polyline points="6 9 12 15 18 9" /></svg
					>
				</div>
				<div>
					{#if loadGraphs}
						{#await loadGraphs then { default: ProgressBar }}
							<ProgressBar load={[$hardwareInfo.ram.load[5]]} />
						{/await}
					{/if}
				</div>
			</div>
		</div>

		<div class="transparent-800 flex w-1/3 flex-col rounded-xl p-10 pt-0 text-center sm:w-full">
			<div class="mx-auto flex w-4/5 justify-center sm:w-1/2">
				<GaugeChart load={$hardwareInfo.gpu.lastLoad} />
			</div>
			<div>
				<h2>GPU</h2>
				<div class="mt-5">
					<h3>{$hardwareInfo.gpu.name}</h3>
				</div>
				<div class="mt-3 flex justify-center">
					<svg
						class={`${loadGraphsShown ? "mb-3 rotate-180 transform" : ""} cursor-pointer text-white`}
						on:click={showLoadGraphs}
						on:keydown={showLoadGraphs}
						xmlns="http://www.w3.org/2000/svg"
						width="32"
						height="32"
						viewBox="0 0 24 24"
						fill="none"
						stroke="currentColor"
						stroke-width="2"
						stroke-linecap="round"
						stroke-linejoin="round"><polyline points="6 9 12 15 18 9" /></svg
					>
				</div>
				<div>
					{#if loadGraphs}
						{#await loadGraphs then { default: ProgressBar }}
							<ProgressBar load={$hardwareInfo.gpu.load} />
						{/await}
					{/if}
				</div>
			</div>
		</div>
	</div>

	<!-- Row 2 -->
	<div class="mx-10 mt-10 flex justify-evenly gap-5 sm:mx-3 sm:flex-wrap">
		<!-- CPU info -->
		<div class="flex w-1/3 flex-col gap-5 text-left sm:w-full">
			<div class="transparent-800 rounded-xl p-8 sm:p-4">
				<div class="mb-5 flex items-center gap-3">
					<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
						<Thermometer />
					</div>
					<h2>CPU Temperature</h2>
				</div>
				<h3>
					Avg. temperature: {Math.round(
						$hardwareInfo.cpu.temperature.reduce((a, b) => a + b.value, 0) / $hardwareInfo.cpu.temperature.length,
					)} °C
				</h3>
				<div>
					<MeterChart
						readings={$hardwareInfo.cpu.temperature}
						categories={$hardwareInfo.cpu.temperature.map((temp, i) => `Core #${i} (${temp.value} °C)`)}
						type={{ name: "temperature", unit: "°C" }}
					/>
				</div>
			</div>

			<div class="transparent-800 rounded-xl p-8 sm:p-4">
				<div class="mb-5 flex items-center gap-3">
					<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
						<Clock />
					</div>

					<h2>CPU Clock Speed</h2>
				</div>
				<h3>
					Avg. clock speed: {(
						Math.round($hardwareInfo.cpu.clock.reduce((a, b) => a + b.value, 0) / $hardwareInfo.cpu.clock.length) / 1000
					).toFixed(1)} GHz
				</h3>
				<div>
					<MeterChart
						readings={$hardwareInfo.cpu.clock}
						categories={$hardwareInfo.cpu.clock.map((temp, i) => `Core #${i} (${(temp.value / 1000).toFixed(1)} GHz)`)}
						type={{ name: "clock speed", unit: "MHz" }}
					/>
				</div>
			</div>

			<div class="transparent-800 rounded-xl p-8 sm:p-4">
				<div class="mb-5 flex items-center gap-3">
					<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
						<Plug />
					</div>
					<h2>CPU Power Usage</h2>
				</div>
				<h3>Power usage: {$hardwareInfo.cpu.power.reduce((a, b) => a + b.value, 0)} W</h3>
				<div>
					<MeterChart
						readings={$hardwareInfo.cpu.power.filter((power) => power.value !== 0)}
						categories={$hardwareInfo.cpu.power
							.filter((power) => power.value !== 0)
							.map((temp) => `${temp.name.replaceAll("CPU", "")} (${temp.value} W)`)}
						type={{ name: "power usage", unit: "W" }}
					/>
				</div>
			</div>

			<div class="transparent-800 rounded-xl p-8 sm:p-4">
				<div class="mb-5 flex items-center gap-3">
					<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
						<Zap />
					</div>
					<h2>CPU Voltage</h2>
				</div>
				<h3>Avg. voltage: {($hardwareInfo.cpu.voltage.reduce((a, b) => a + b.value, 0) / $hardwareInfo.cpu.voltage.length).toFixed(1)} V</h3>
				<div>
					<MeterChart
						readings={$hardwareInfo.cpu.voltage}
						categories={$hardwareInfo.cpu.voltage.map((temp, i) => `Core #${i} (${temp.value} V)`)}
						type={{ name: "voltage", unit: "V" }}
					/>
				</div>
			</div>
		</div>

		<!-- RAM info -->
		<div class="flex w-1/3 flex-col gap-5 text-left sm:w-full">
			<div class="transparent-800 rounded-xl p-8 sm:p-4">
				<div class="mb-5 flex items-center gap-3">
					<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
						<Memory width={24} height={24} />
					</div>

					<h2>RAM Usage</h2>
				</div>
				<h3>
					Memory: {`${$hardwareInfo.ram.load[0].value.toFixed(1)}/${(
						$hardwareInfo.ram.load[0].value + $hardwareInfo.ram.load[1].value
					).toFixed(1)} GB`}
				</h3>
				<div>
					<MeterChart readings={[$hardwareInfo.ram.load[0]]} categories={["RAM usage"]} type={{ name: "memory usage", unit: "GB" }} />
				</div>
			</div>

			<div class="transparent-800 rounded-xl p-8 sm:p-4">
				<div class="mb-5 flex items-center gap-3">
					<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
						<Memory width={24} height={24} />
					</div>
					<h2>Virtual RAM Usage</h2>
				</div>
				<h3>
					Virtual memory: {`${$hardwareInfo.ram.load[3].value.toFixed(1)}/${(
						$hardwareInfo.ram.load[3].value + $hardwareInfo.ram.load[4].value
					).toFixed(1)} GB`}
				</h3>
				<div>
					<MeterChart
						readings={[$hardwareInfo.ram.load[3]]}
						categories={["Virtual RAM usage"]}
						type={{ name: "virtual memory usage", unit: "GB" }}
					/>
				</div>
			</div>
		</div>

		<!-- GPU info -->
		<div class="flex w-1/3 flex-col gap-5 text-left sm:w-full">
			{#if $hardwareInfo.gpu.temperature.length > 0}
				<div class="transparent-800 rounded-xl p-8 sm:p-4">
					<div class="mb-5 flex items-center gap-3">
						<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
							<Thermometer />
						</div>
						<h2>GPU Temperature</h2>
					</div>
					<h3>
						Avg. temperature: {Math.round(
							$hardwareInfo.gpu.temperature.reduce((a, b) => a + b.value, 0) / $hardwareInfo.gpu.temperature.length,
						)} °C
					</h3>
					<div>
						<MeterChart
							readings={$hardwareInfo.gpu.temperature}
							categories={$hardwareInfo.gpu.temperature.map((temp) => `${temp.name.replaceAll("GPU", "")} (${temp.value} °C)`)}
							type={{ name: "temperature", unit: "°C" }}
						/>
					</div>
				</div>
			{/if}

			{#if $hardwareInfo.gpu.fan.length > 0}
				<div class="transparent-800 rounded-xl p-8 sm:p-4">
					<div class="mb-5 flex items-center gap-3">
						<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
							<Fan />
						</div>
						<h2>GPU Fan Speed</h2>
					</div>
					<h3>Avg. fan speed: {Math.round($hardwareInfo.gpu.fan.reduce((a, b) => a + b.value, 0) / $hardwareInfo.gpu.fan.length)} RPM</h3>
					{#if $hardwareInfo.gpu.fan[0].max > 0}
						<div>
							<MeterChart
								categories={$hardwareInfo.gpu.fan.map((temp, i) => `Fan #${i} (${temp.value} RPM)`)}
								readings={$hardwareInfo.gpu.fan}
								type={{ name: "fan speed", unit: "RPM" }}
							/>
						</div>
					{/if}
				</div>
			{/if}

			{#if $hardwareInfo.gpu.memory.length > 2}
				<div class="transparent-800 rounded-xl p-8 sm:p-4">
					<div class="mb-5 flex items-center gap-3">
						<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
							<Memory width={24} height={24} />
						</div>
						<h2>GPU Memory Usage</h2>
					</div>
					<h3>GPU memory: {`${$hardwareInfo.gpu.memory[0].value.toFixed(1)}/${$hardwareInfo.gpu.memory[2].value} GB`} GB</h3>
					<div>
						<MeterChart
							readings={[$hardwareInfo.gpu.memory[0]]}
							categories={["GPU memory usage"]}
							type={{ name: "GPU memory usage", unit: "GB" }}
						/>
					</div>
				</div>
			{/if}

			{#if $hardwareInfo.gpu.clock.length > 0}
				<div class="transparent-800 rounded-xl p-8 sm:p-4">
					<div class="mb-5 flex items-center gap-3">
						<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
							<Clock />
						</div>
						<h2>GPU Clock Speed</h2>
					</div>
					<div>
						<MeterChart
							readings={$hardwareInfo.gpu.clock}
							categories={$hardwareInfo.gpu.clock.map(
								(temp) => `${temp.name.replaceAll("GPU", "")} (${(temp.value / 1000).toFixed(1)} GHz)`,
							)}
							type={{ name: "clock speed", unit: "MHz" }}
						/>
					</div>
				</div>
			{/if}

			{#if $hardwareInfo.gpu.power.length > 0}
				<div class="transparent-800 rounded-xl p-8 sm:p-4">
					<div class="mb-5 flex items-center gap-3">
						<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
							<Plug />
						</div>

						<h2>GPU Power Usage</h2>
					</div>
					<h3>Power usage: {$hardwareInfo.gpu.power.reduce((a, b) => a + b.value, 0)} W</h3>
					<div>
						<MeterChart
							readings={$hardwareInfo.gpu.power}
							categories={$hardwareInfo.gpu.power.map((temp) => `${temp.name.replaceAll("GPU", "")} (${temp.value} W)`)}
							type={{ name: "power usage", unit: "W" }}
						/>
					</div>
				</div>
			{/if}
		</div>
	</div>

	<!-- Row 3 -->
	<div class="mx-10 mt-10 flex justify-evenly gap-5 pb-10 sm:mx-3 sm:flex-wrap">
		<!-- Drives -->
		<div class="flex w-1/3 flex-col gap-5 text-left sm:w-full">
			<div class="transparent-800 rounded-xl p-8 sm:p-4">
				<div class="mb-5 flex items-center gap-3">
					<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
						<HardDrive />
					</div>
					<h2>Drives</h2>
				</div>
				{#each $hardwareInfo.system.storage.disks as { name, freeSpace, totalSpace, health }}
					<div class="mt-5 select-text">
						<h3>Name: {name}</h3>
						<h3>Health: {health}%</h3>
						<h3>Available space: {freeSpace}/{totalSpace} GB</h3>
					</div>
				{/each}
			</div>

			<div class="transparent-800 rounded-xl p-8 sm:p-4">
				<div class="mb-5 flex items-center gap-3">
					<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
						<Thermometer />
					</div>
					<h2>Drive Temperatures</h2>
				</div>
				<div>
					<MeterChart
						readings={$hardwareInfo.system.storage.disks.map((disk) => disk.temperature)}
						categories={$hardwareInfo.system.storage.disks.map((temp, i) => `${temp.name} (${temp.temperature.value} °C)`)}
						type={{ name: "temperature", unit: "°C" }}
					/>
				</div>
			</div>
		</div>

		<!-- System -->
		<div class="flex w-1/3 flex-col gap-5 text-left sm:w-full">
			<div class="transparent-800 rounded-xl p-8 sm:p-4">
				<div class="mb-5 flex items-center gap-3">
					<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
						<PcDisplay width={24} height={24} />
					</div>
					<h2>System</h2>
				</div>
				<div class="select-text">
					<h3>CPU: {$hardwareInfo.cpu.name}</h3>
					<h3>RAM: {Math.round($hardwareInfo.ram.load[0].value + $hardwareInfo.ram.load[1].value)} GB</h3>
					<h3>GPU: {$hardwareInfo.gpu.name}</h3>
					<h3>MB: {$hardwareInfo.system.motherboard.name}</h3>
					<h3>OS: {$hardwareInfo.system.os.name}</h3>
				</div>
			</div>

			<div class="transparent-800 rounded-xl p-8 sm:p-4">
				<div class="mb-5 flex items-center gap-3">
					<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
						<Monitor />
					</div>
					<h2>Monitors</h2>
				</div>
				{#each $hardwareInfo.system.monitor.monitors as { name, refreshRate, resolution }}
					<div class="mt-5 select-text">
						<h3>Name: {name}</h3>
						<h3>Resolution: {resolution}</h3>
						<h3>Refresh rate: {refreshRate} Hz</h3>
					</div>
				{/each}
			</div>

			<div class="transparent-800 rounded-xl p-8 sm:p-4">
				<div class="mb-5 flex items-center gap-3">
					<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
						<CircuitBoard />
					</div>
					<h2>BIOS</h2>
				</div>
				<div class="mt-5 select-text">
					<h3>Vendor: {$hardwareInfo.system.bios.vendor}</h3>
					<h3>Version: {$hardwareInfo.system.bios.version}</h3>
					<h3>Date: {$hardwareInfo.system.bios.date}</h3>
				</div>
			</div>
		</div>

		<!-- Network -->
		<div class="flex w-1/3 flex-col gap-5 text-left sm:w-full">
			<div class="transparent-800 rounded-xl p-8 sm:p-4">
				<div class="mb-5 flex items-center gap-3">
					<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
						<Network />
					</div>
					<h2>Interfaces</h2>
				</div>
				{#each $hardwareInfo.system.network.interfaces as { name, description, ipAddress, mask, gateway, dns, speed, macAddress }}
					<div class="mt-5 select-text">
						<h3>Name: {name}</h3>
						<h3>Description: {description}</h3>
						<h3>Address: {ipAddress} ({mask})</h3>
						<h3>Gateway: {gateway} ({dns})</h3>
						<h3>Speed: {speed} Mbit/s</h3>
					</div>
				{/each}
			</div>
		</div>
	</div>
</div>

<script lang="ts">
	import { hardwareInfo } from "../stores/hardwareInfo"
	import GaugeChart from "../components/gaugeChart.svelte"
	import MeterChart from "../components/meterChart.svelte"
	import { CircuitBoard, Clock, Fan, HardDrive, Monitor, Network, Plug, Thermometer, Zap } from "lucide-svelte"
	import { Memory, PcDisplay } from "svelte-bootstrap-icons"

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
</script>
