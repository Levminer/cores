<div class="transparent-900 m-10 mx-auto w-4/5 rounded-xl sm:m-3 sm:w-full">
	<div class="mx-10 flex justify-evenly gap-5 pt-10 pb-10">
		<div class="w-2/3 text-left">
			<!-- System -->
			<div class="transparent-800 mb-5 rounded-xl p-10">
				<div class="flex items-baseline gap-3">
					<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="bi bi-pc-display" viewBox="0 0 16 16">
						<path d="M8 1a1 1 0 0 1 1-1h6a1 1 0 0 1 1 1v14a1 1 0 0 1-1 1H9a1 1 0 0 1-1-1V1Zm1 13.5a.5.5 0 1 0 1 0 .5.5 0 0 0-1 0Zm2 0a.5.5 0 1 0 1 0 .5.5 0 0 0-1 0ZM9.5 1a.5.5 0 0 0 0 1h5a.5.5 0 0 0 0-1h-5ZM9 3.5a.5.5 0 0 0 .5.5h5a.5.5 0 0 0 0-1h-5a.5.5 0 0 0-.5.5ZM1.5 2A1.5 1.5 0 0 0 0 3.5v7A1.5 1.5 0 0 0 1.5 12H6v2h-.5a.5.5 0 0 0 0 1H7v-4H1.5a.5.5 0 0 1-.5-.5v-7a.5.5 0 0 1 .5-.5H7V2H1.5Z" />
					</svg>
					<h2 class="mb-5">System</h2>
				</div>
				<h3>CPU: {$hardwareInfo.cpu.name}</h3>
				<h3>RAM: {($hardwareInfo.ram.load[0].value + $hardwareInfo.ram.load[1].value).toFixed(1)} GB</h3>
				<h3>GPU: {$hardwareInfo.gpu.name}</h3>
				<h3>MB: {$hardwareInfo.system.motherboard.name}</h3>
				<h3>OS: {$hardwareInfo.system.os.name}</h3>
			</div>

			<div class="flex gap-5 sm:flex-wrap">
				<!-- Monitors -->
				<div class="transparent-800 w-1/2 rounded-xl p-10 sm:w-full">
					<div class="flex items-baseline gap-3">
						<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><rect x="2" y="3" width="20" height="14" rx="2" ry="2" /><line x1="8" y1="21" x2="16" y2="21" /><line x1="12" y1="17" x2="12" y2="21" /></svg>
						<h2>Monitors</h2>
					</div>
					{#each $hardwareInfo.system.monitor.monitors as { name, refreshRate, resolution }}
						<div class="mt-5">
							<h3>Name: {name}</h3>
							<h3>Resolution: {resolution}</h3>
							<h3>Refresh rate: {refreshRate} Hz</h3>
						</div>
					{/each}
				</div>

				<!-- Interfaces -->
				<div class="transparent-800 w-1/2 rounded-xl p-10 sm:w-full">
					<div class="flex items-baseline gap-3">
						<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><rect x="9" y="2" width="6" height="6" /><rect x="16" y="16" width="6" height="6" /><rect x="2" y="16" width="6" height="6" /><path d="M5 16v-4h14v4" /><path d="M12 12V8" /></svg>
						<h2>Interfaces</h2>
					</div>
					{#each $hardwareInfo.system.network.interfaces as { name, description, ipAddress, mask, gateway, dns, speed, macAddress }}
						<div class="mt-5">
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

		<div class="flex w-1/3 gap-5 text-left flex-col">
			<!-- Disk info -->
			<div class="transparent-800 rounded-xl p-10">
				<div class="flex items-baseline gap-3">
					<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><line x1="22" y1="12" x2="2" y2="12" /><path d="M5.45 5.11 2 12v6a2 2 0 0 0 2 2h16a2 2 0 0 0 2-2v-6l-3.45-6.89A2 2 0 0 0 16.76 4H7.24a2 2 0 0 0-1.79 1.11z" /><line x1="6" y1="16" x2="6.01" y2="16" /><line x1="10" y1="16" x2="10.01" y2="16" /></svg>
					<h2>Disks</h2>
				</div>
				{#each $hardwareInfo.system.storage.disks as { name, temperature, usedSpace, size, health }}
					<div class="mt-5">
						<h3>Name: {name}</h3>
						<h3>Temperature: {temperature} °C</h3>
						<h3>Health: {health}%</h3>
						<h3>Available space: {(size - size * (usedSpace / 100)).toFixed()}/{size} GB</h3>
					</div>
				{/each}
			</div>

			<!-- BIOS info -->
			<div class="transparent-800 rounded-xl p-10">
				<div class="flex items-baseline gap-3">
					<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><rect x="4" y="4" width="16" height="16" rx="2" ry="2" /><rect x="9" y="9" width="6" height="6" /><line x1="9" y1="2" x2="9" y2="4" /><line x1="15" y1="2" x2="15" y2="4" /><line x1="9" y1="21" x2="9" y2="22" /><line x1="15" y1="20" x2="15" y2="22" /><line x1="20" y1="9" x2="22" y2="9" /><line x1="20" y1="14" x2="22" y2="14" /><line x1="2" y1="9" x2="4" y2="9" /><line x1="2" y1="14" x2="4" y2="14" /></svg>
					<h2>BIOS</h2>
				</div>
				<div class="mt-5">
					<h3>Vendor: {$hardwareInfo.system.bios.vendor}</h3>
					<h3>Version: {$hardwareInfo.system.bios.version}</h3>
					<h3>Date: {$hardwareInfo.system.bios.date}</h3>
				</div>
			</div>
		</div>
	</div>
</div>

<script lang="ts">
	import { hardwareInfo } from "../stores/hardwareInfo"
</script>
