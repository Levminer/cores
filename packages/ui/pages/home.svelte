<div class="transparent-900 m-10 mx-auto w-11/12 rounded-xl sm:w-full">
	<!-- Row 1 -->
	<div class="mx-10 flex justify-evenly gap-5 pt-10 sm:mx-3 sm:flex-wrap">
		<div class="transparent-800 flex w-1/3 flex-col rounded-xl p-8 sm:w-full sm:p-4">
			<div class="mb-5 flex items-center gap-3">
				<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
					<Cpu />
				</div>
				<h2>CPU</h2>
			</div>
			<h3>{$hardwareInfo.cpu.name}</h3>
			<div class="flex flex-col items-start justify-start gap-5 pt-5 md:flex-row">
				<div class="mx-auto flex w-3/5 justify-start md:w-2/5">
					<GaugeChart load={$hardwareInfo.cpu.lastLoad} />
				</div>

				<div class="overlayScroll mx-auto w-full flex-col justify-start space-y-2 overflow-y-auto md:max-h-48 md:w-3/5">
					{#each $hardwareInfo.cpu.load as item, i}
						<div>
							<div class="flex w-[95%] flex-row justify-between">
								<p class="text-sm">{item.name?.replaceAll("CPU", "")}</p>
								<p class="text-sm text-[#969696]">{Math.round(item.value)}%</p>
							</div>
							<div class="progress">
								<div id={`progress${i}`} class="progressFill" />
							</div>
						</div>
					{/each}
				</div>
			</div>
		</div>

		<div class="transparent-800 flex w-1/3 flex-col rounded-xl p-8 sm:w-full sm:p-4">
			<div class="mb-5 flex items-center gap-3">
				<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
					<Memory height={24} width={24} />
				</div>
				<h2>RAM</h2>
			</div>
			<h3>{$hardwareInfo.ram.info[0]?.manufacturerName ?? "Generic Memory"}</h3>
			<div class="flex flex-col items-start justify-start gap-5 pt-5 md:flex-row">
				<div class="mx-auto flex w-3/5 justify-start md:w-2/5">
					<GaugeChart load={$hardwareInfo.ram.load[2]?.value ?? 0} />
				</div>
				<div class="overlayScroll mx-auto w-full flex-col justify-start space-y-2 overflow-y-auto md:max-h-48 md:w-3/5">
					<div>
						<div class="flex w-[95%] flex-row justify-between">
							<p class="text-sm">Virtual memory</p>
							<p class="text-sm text-[#969696]">{Math.round($hardwareInfo.ram.load[5]?.value ?? 0)}%</p>
						</div>
						<div class="progress">
							<div id="2progress" class="progressFill" />
						</div>
					</div>
				</div>
			</div>
		</div>

		<div class="transparent-800 flex w-1/3 flex-col rounded-xl p-8 sm:w-full sm:p-4">
			<div class="mb-5 flex items-center gap-3">
				<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
					<GpuCard height={24} width={24} />
				</div>
				<h2>GPU</h2>
			</div>
			<h3>{$hardwareInfo.gpu.name}</h3>
			<div class="flex flex-col items-start justify-start gap-5 pt-5 md:flex-row">
				<div class="mx-auto flex w-3/5 justify-start md:w-2/5">
					<GaugeChart load={Math.round($hardwareInfo.gpu.lastLoad)} />
				</div>
				<div class="overlayScroll mx-auto w-full flex-col justify-start space-y-2 overflow-y-auto md:max-h-48 md:w-3/5">
					{#each $hardwareInfo.gpu.load as item, i}
						<div>
							<div class="flex w-[95%] flex-row justify-between">
								<p class="text-sm">{item.name?.replaceAll("D3D", "")}</p>
								<p class="text-sm text-[#969696]">{Math.round(item.value)}%</p>
							</div>
							<div class="progress">
								<div id={`3progress${i}`} class="progressFill" />
							</div>
						</div>
					{/each}
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
						categories={$hardwareInfo.cpu.temperature.map((temp, i) => `Core #${i + 1} (${temp.value} °C)`)}
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
						categories={$hardwareInfo.cpu.clock.map((temp, i) => `Core #${i + 1} (${(temp.value / 1000).toFixed(1)} GHz)`)}
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
						categories={$hardwareInfo.cpu.voltage.map((temp, i) => `Core #${i + 1} (${temp.value} V)`)}
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
						<Gauge />
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
						<Gauge />
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
	import { hardwareInfo } from "ui/stores/hardwareInfo.ts"
	import GaugeChart from "ui/charts/gaugeChart.svelte"
	import MeterChart from "ui/charts/meterChart.svelte"
	import { Gauge, CircuitBoard, Clock, Fan, HardDrive, Monitor, Network, Plug, Thermometer, Zap, Cpu } from "lucide-svelte"
	import { GpuCard, Memory, PcDisplay } from "svelte-bootstrap-icons"
	import { onMount } from "svelte"

	let interval: NodeJS.Timeout

	onMount(() => {
		interval = setInterval(() => {
			$hardwareInfo.cpu.load.forEach((load, i) => {
				const progress = document.getElementById(`progress${i}`) as HTMLDivElement
				progress.style.width = `${Math.round(load.value)}%`
			})

			$hardwareInfo.gpu.load.forEach((load, i) => {
				const progress = document.getElementById(`3progress${i}`) as HTMLDivElement
				progress.style.width = `${Math.round(load.value)}%`
			})

			let ramLoad = document.getElementById("2progress") as HTMLDivElement
			ramLoad.style.width = `${Math.round($hardwareInfo.ram.load[5].value)}%`
		}, 1000)

		return () => {
			clearInterval(interval)
		}
	})
</script>
