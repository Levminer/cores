<div class="transparent-900 m-10 mx-auto w-11/12 rounded-xl sm:w-full">
	<!-- header -->
	{#if import.meta.env.VITE_CORES_MODE === "host"}
		<div class="mx-10 flex justify-evenly gap-5 pt-10 sm:mx-3 sm:flex-wrap">
			<div class="transparent-800 flex w-full flex-row items-center justify-between rounded-xl px-8 py-4 sm:p-4">
				<div class="flex flex-row items-center gap-2">
					<div class="flex items-center gap-3">
						<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
							<img alt="icon" src="https://www.coresmonitor.com/favicon.ico" height="24" width="24" />
						</div>
						<h2>Cores</h2>
					</div>

					{#if $state.plan !== "personal" && $state.plan !== "unix"}
						<h2 class="text-cores-alternative">Trial</h2>

						<a
							class="bg-cores-alternative border-cores-alternative mt-2 cursor-pointer rounded-xl border-2 px-2 py-0.5 text-xl font-medium text-white duration-200 ease-in hover:border-white"
							href="/onboarding">Upgrade</a
						>
					{/if}
				</div>
				<div class="flex flex-row gap-3">
					<a href="/connections" class="transparent-900 flex items-center justify-center gap-2 rounded-lg p-3 font-semibold sm:p-2">
						<MonitorSmartphone />
						{#if $settings.remoteConnections}
							Remote connections
							<div id="status" class="relative top-0.5 size-3 rounded-full bg-green-500" />
						{:else}
							Remote connections
							<div id="status" class="relative top-0.5 size-3 rounded-full bg-red-500" />
						{/if}
					</a>

					<button
						on:click={() => {
							open("https://link.levminer.com/crs-dc")
						}}
						class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2"
					>
						<svg class="h-6 w-6" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 127.14 96.36"
							><path
								fill="#fff"
								d="M107.7,8.07A105.15,105.15,0,0,0,81.47,0a72.06,72.06,0,0,0-3.36,6.83A97.68,97.68,0,0,0,49,6.83,72.37,72.37,0,0,0,45.64,0,105.89,105.89,0,0,0,19.39,8.09C2.79,32.65-1.71,56.6.54,80.21h0A105.73,105.73,0,0,0,32.71,96.36,77.7,77.7,0,0,0,39.6,85.25a68.42,68.42,0,0,1-10.85-5.18c.91-.66,1.8-1.34,2.66-2a75.57,75.57,0,0,0,64.32,0c.87.71,1.76,1.39,2.66,2a68.68,68.68,0,0,1-10.87,5.19,77,77,0,0,0,6.89,11.1A105.25,105.25,0,0,0,126.6,80.22h0C129.24,52.84,122.09,29.11,107.7,8.07ZM42.45,65.69C36.18,65.69,31,60,31,53s5-12.74,11.43-12.74S54,46,53.89,53,48.84,65.69,42.45,65.69Zm42.24,0C78.41,65.69,73.25,60,73.25,53s5-12.74,11.44-12.74S96.23,46,96.12,53,91.08,65.69,84.69,65.69Z"
							/></svg
						>
					</button>

					<a href="/settings" class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
						<Settings />
					</a>
				</div>
			</div>
		</div>
	{/if}

	<!-- Row 1 -->
	<div class="mx-10 flex justify-evenly gap-5 pt-10 sm:mx-3 sm:flex-wrap">
		<div class="transparent-800 flex w-1/3 flex-col rounded-xl p-8 sm:w-full sm:p-4">
			<div class="mb-5 flex items-center gap-3">
				<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
					<Cpu />
				</div>
				<h2>CPU</h2>
			</div>
			<h3 class="max-w-full truncate">{$hardwareInfo.cpu.name}</h3>
			<div class="flex flex-col items-start justify-start gap-5 pt-5 md:flex-row">
				<div class="mx-auto flex w-3/5 justify-start md:w-2/5">
					<GaugeChart load={$hardwareInfo.cpu.maxLoad} />
				</div>

				<div class="overlayScroll mx-auto w-full flex-col justify-start space-y-2 overflow-y-auto md:max-h-48 md:w-3/5">
					{#each $hardwareInfo.cpu.load as item, i}
						<div>
							<div class="flex w-[95%] flex-row justify-between">
								<p class="text-sm">{item.name?.replaceAll("CPU", "")}</p>
								<p class="text-sm text-[#969696]">{Math.round(item.value)}%</p>
							</div>
							<Progress value={item.value} />
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
			<h3 class="max-w-full truncate">{$hardwareInfo.ram.info[0]?.manufacturerName ?? "Generic Memory"}</h3>
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
						<Progress value={$hardwareInfo.ram.load[5]?.value ?? 0} />
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
			{#if $hardwareInfo.gpu.cards?.length > 0}
				<h3 class="max-w-full truncate">{$hardwareInfo.gpu.cards.map((card) => card.name).join(", ")}</h3>
			{:else}
				<h3 class="max-w-full truncate">{$hardwareInfo.gpu.cards?.[0]?.name ?? "N/A"}</h3>
			{/if}
			<div class="flex flex-col items-start justify-start gap-5 pt-5 md:flex-row">
				<div class="mx-auto flex w-3/5 justify-start md:w-2/5">
					<GaugeChart load={Math.round($hardwareInfo.gpu.cards?.[0]?.maxLoad ?? 0)} />
				</div>
				<div class="overlayScroll mx-auto w-full flex-col justify-start space-y-2 overflow-y-auto md:max-h-48 md:w-3/5">
					{#each $hardwareInfo.gpu.cards?.[0]?.load ?? [] as item, i}
						<div>
							<div class="flex w-[95%] flex-row justify-between">
								<p class="text-sm">{item.name?.replaceAll("D3D", "")}</p>
								<p class="text-sm text-[#969696]">{Math.round(item.value)}%</p>
							</div>
							<Progress value={item.value} />
						</div>
					{/each}
				</div>
			</div>
		</div>
	</div>

	<!-- Row 1.1 -->
	<div class="mx-10 flex justify-evenly gap-5 pt-5 sm:mx-3 sm:flex-wrap">
		<div class="flex w-1/3 flex-col gap-5 text-left sm:w-full">
			<div class="transparent-800 flex flex-1 flex-col rounded-xl p-8 sm:p-4">
				<div class="mb-5 flex items-center gap-3">
					<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
						<HardDrive />
					</div>
					<h2>Drives</h2>
				</div>
				<div class="flex flex-col items-start justify-start gap-5 md:flex-row">
					<div class="overlayScroll mx-auto w-full flex-col justify-start space-y-2 overflow-y-auto md:max-h-20">
						{#each $hardwareInfo.system.storage.disks as item, i}
							<div>
								<div class="flex w-[95%] flex-row justify-between">
									<p class="text-sm">{item.name} Read Speed</p>
									<p class="text-sm text-[#969696]">{parseFloat((item.throughputRead / 1_048_576).toFixed(2))} MB/s</p>
								</div>
								<Progress value={parseFloat((item.throughputRead / 1_048_576).toFixed(2))} />
							</div>
							<div>
								<div class="flex w-[95%] flex-row justify-between">
									<p class="text-sm">{item.name} Write Speed</p>
									<p class="text-sm text-[#969696]">{parseFloat((item.throughputWrite / 1_048_576).toFixed(2))} MB/s</p>
								</div>
								<Progress value={parseFloat((item.throughputWrite / 1_048_576).toFixed(2))} />
							</div>
						{/each}
					</div>
				</div>
			</div>
		</div>

		<div class="flex w-1/3 flex-col gap-5 text-left sm:w-full">
			{#if $hardwareInfo.system.superIO.fan.length > 0}
				<div class="transparent-800 flex flex-1 flex-col rounded-xl p-8 sm:p-4">
					<div class="mb-5 flex items-center gap-3">
						<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
							<Fan />
						</div>
						<h2>Fans</h2>
					</div>

					<div class="flex flex-col items-start justify-start gap-5 md:flex-row">
						<div class="overlayScroll mx-auto w-full flex-col justify-start space-y-2 overflow-y-auto md:max-h-20">
							{#each $hardwareInfo.system.superIO.fan as item, i}
								{#if item.value != 0}
									<div>
										<div class="flex w-[95%] flex-row justify-between">
											<p class="text-sm">{item.name}</p>
											<p class="text-sm text-[#969696]">{Math.round($hardwareInfo.system.superIO.fanControl[i].value)}%</p>
										</div>
										<Progress value={$hardwareInfo.system.superIO.fanControl[i].value} />
									</div>
								{/if}
							{/each}
						</div>
					</div>
				</div>
			{/if}
		</div>

		<div class="flex w-1/3 flex-col gap-5 text-left sm:w-full">
			<div class="transparent-800 flex flex-1 flex-col rounded-xl p-8 sm:p-4">
				<div class="mb-5 flex items-center gap-3">
					<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
						<Network />
					</div>
					<h2>Interfaces</h2>
				</div>
				<div class="flex flex-col items-start justify-start gap-5 md:flex-row">
					<div class="overlayScroll mx-auto w-full flex-col justify-start space-y-2 overflow-y-auto md:max-h-20">
						{#each $hardwareInfo.system.network.interfaces as item, i}
							<div>
								<div class="flex w-[95%] flex-row justify-between">
									<p class="text-sm">{item.name} Download Speed</p>
									<p class="text-sm text-[#969696]">{parseFloat((item.throughputDownload / 1_048_576).toFixed(2))} MB/s</p>
								</div>
								<Progress value={parseFloat((item.throughputDownload / 125_000).toFixed(2))} />
							</div>
							<div>
								<div class="flex w-[95%] flex-row justify-between">
									<p class="text-sm">{item.name} Upload Speed</p>
									<p class="text-sm text-[#969696]">{parseFloat((item.throughputUpload / 1_048_576).toFixed(2))} MB/s</p>
								</div>
								<Progress value={parseFloat((item.throughputUpload / 125_000).toFixed(2))} />
							</div>
						{/each}
					</div>
				</div>
			</div>
		</div>
	</div>

	<!-- Row 2 -->
	<div class="mx-10 flex justify-evenly gap-5 pb-10 pt-5 sm:mx-3 sm:flex-wrap">
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
						categories={$hardwareInfo.cpu.temperature.map((temp) => `${temp.name?.replaceAll("CPU", "")} (${temp.value} °C)`)}
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

			{#if $hardwareInfo.cpu.power.length > 0}
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
								.map((temp) => `${temp.name?.replaceAll("CPU", "")} (${temp.value} W)`)}
							type={{ name: "power usage", unit: "W" }}
						/>
					</div>
				</div>
			{/if}

			{#if $hardwareInfo.cpu.voltage.length > 0}
				<div class="transparent-800 rounded-xl p-8 sm:p-4">
					<div class="mb-5 flex items-center gap-3">
						<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
							<Zap />
						</div>
						<h2>CPU Voltage</h2>
					</div>
					<h3>
						Avg. voltage: {($hardwareInfo.cpu.voltage.reduce((a, b) => a + b.value, 0) / $hardwareInfo.cpu.voltage.length).toFixed(1)}
						V
					</h3>
					<div>
						<MeterChart
							readings={$hardwareInfo.cpu.voltage}
							categories={$hardwareInfo.cpu.voltage.map((temp, i) => `Core #${i + 1} (${temp.value} V)`)}
							type={{ name: "voltage", unit: "V" }}
						/>
					</div>
				</div>
			{/if}

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
					Memory: {`${$hardwareInfo.ram.load[0]?.value.toFixed(1) ?? 0}/${(
						($hardwareInfo.ram.load[0]?.value ?? 0) + ($hardwareInfo.ram.load[1]?.value ?? 0)
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
					Virtual memory: {`${$hardwareInfo.ram.load[3]?.value.toFixed(1) ?? 0}/${(
						($hardwareInfo.ram.load[3]?.value ?? 0) + ($hardwareInfo.ram.load[4]?.value ?? 0)
					).toFixed(1)} GB`}
				</h3>
				{#if $hardwareInfo.ram.load[3]?.value ?? 0 > 0}
					<div>
						<MeterChart
							readings={[$hardwareInfo.ram.load[3]]}
							categories={["Virtual RAM usage"]}
							type={{ name: "virtual memory usage", unit: "GB" }}
						/>
					</div>
				{/if}
			</div>

			<div class="transparent-800 rounded-xl p-8 sm:p-4">
				<div class="mb-5 flex items-center gap-3">
					<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
						<PcDisplay width={24} height={24} />
					</div>
					<h2>System</h2>
				</div>
				<div class="select-text">
					<h3>CPU: {$hardwareInfo.cpu.name}</h3>
					<h3>RAM: {Math.round(($hardwareInfo.ram.load[0]?.value ?? 0) + ($hardwareInfo.ram.load[1]?.value ?? 0))} GB</h3>
					<h3>GPU: {$hardwareInfo.gpu.cards?.[0]?.name ?? "N/A"}</h3>
					<h3>MB: {$hardwareInfo.system.motherboard.name}</h3>
					<h3>OS: {$hardwareInfo.system.os.name}</h3>
				</div>
			</div>

			{#if $hardwareInfo.system.battery?.capacity.length ?? 0 > 0}
				<div class="transparent-800 rounded-xl p-8 sm:p-4">
					<div class="mb-5 flex items-center gap-3">
						<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
							<Battery />
						</div>
						<h2>Battery</h2>
					</div>

					<div class="mt-5 select-text">
						<h3>Charge level: {Math.round($hardwareInfo.system.battery?.level[1].value ?? 0)}%</h3>
						<h3>Health: {Math.round(100 - ($hardwareInfo.system.battery?.level[0].value ?? 0))}%</h3>
						<h3>Cycle count: {$hardwareInfo.system.battery?.cycleCount}</h3>
						<h3>
							Capacity: {Math.round(($hardwareInfo.system.battery?.capacity[2].value ?? 0) / 1000)}/{Math.round(
								($hardwareInfo.system.battery?.capacity[1].value ?? 0) / 1000,
							)} Wh
						</h3>
					</div>
				</div>
			{/if}

			{#if $hardwareInfo.system.monitor?.monitors.length ?? 0 > 0}
				<div class="transparent-800 rounded-xl p-8 sm:p-4">
					<div class="mb-5 flex items-center gap-3">
						<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
							<Monitor />
						</div>
						<h2>Monitors</h2>
					</div>

					{#each $hardwareInfo.system.monitor?.monitors ?? [] as { name, refreshRate, resolution }}
						<div class="mt-5 select-text">
							<h3>Name: {name}</h3>
							<h3>Resolution: {resolution}</h3>
							<h3>Refresh rate: {refreshRate} Hz</h3>
						</div>
					{/each}
				</div>
			{/if}

			{#if $hardwareInfo.system.bios.vendor !== "N/A"}
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
			{/if}
		</div>

		<!-- GPU info -->
		<div class="flex w-1/3 flex-col gap-5 text-left sm:w-full">
			{#if $hardwareInfo.gpu.cards?.[0]?.temperature.length > 0}
				<div class="transparent-800 rounded-xl p-8 sm:p-4">
					<div class="mb-5 flex items-center gap-3">
						<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
							<Thermometer />
						</div>
						<h2>GPU Temperature</h2>
					</div>
					<h3>
						Avg. temperature: {Math.round(
							$hardwareInfo.gpu.cards[0].temperature.reduce((a, b) => a + b.value, 0) / $hardwareInfo.gpu.cards[0].temperature.length,
						)} °C
					</h3>
					<div>
						<MeterChart
							readings={$hardwareInfo.gpu.cards[0].temperature}
							categories={$hardwareInfo.gpu.cards[0].temperature.map(
								(temp) => `${temp.name?.replaceAll("GPU", "")} (${temp.value} °C)`,
							)}
							type={{ name: "temperature", unit: "°C" }}
						/>
					</div>
				</div>
			{/if}

			{#if $hardwareInfo.gpu.cards?.[0]?.fan.length > 0}
				<div class="transparent-800 rounded-xl p-8 sm:p-4">
					<div class="mb-5 flex items-center gap-3">
						<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
							<Fan />
						</div>
						<h2>GPU Fan Speed</h2>
					</div>
					<h3>
						Avg. fan speed: {Math.round(
							$hardwareInfo.gpu.cards[0].fan.reduce((a, b) => a + b.value, 0) / $hardwareInfo.gpu.cards[0].fan.length,
						)} RPM
					</h3>
					{#if $hardwareInfo.gpu.cards[0].fan[0].max > 0}
						<div>
							<MeterChart
								categories={$hardwareInfo.gpu.cards[0].fan.map((temp, i) => `Fan #${i} (${temp.value} RPM)`)}
								readings={$hardwareInfo.gpu.cards[0].fan}
								type={{ name: "fan speed", unit: "RPM" }}
							/>
						</div>
					{/if}
				</div>
			{/if}

			{#if $hardwareInfo.gpu.cards?.[0]?.memory.length > 2}
				<div class="transparent-800 rounded-xl p-8 sm:p-4">
					<div class="mb-5 flex items-center gap-3">
						<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
							<Memory width={24} height={24} />
						</div>
						<h2>GPU Memory Usage</h2>
					</div>
					<h3>GPU memory: {`${$hardwareInfo.gpu.cards[0].memory[0].value.toFixed(1)}/${$hardwareInfo.gpu.cards[0].memory[2].value}`} GB</h3>
					<div>
						<MeterChart
							readings={[$hardwareInfo.gpu.cards[0].memory[0]]}
							categories={["GPU memory usage"]}
							type={{ name: "GPU memory usage", unit: "GB" }}
						/>
					</div>
				</div>
			{/if}

			{#if $hardwareInfo.gpu.cards?.[0]?.clock.length > 0}
				<div class="transparent-800 rounded-xl p-8 sm:p-4">
					<div class="mb-5 flex items-center gap-3">
						<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
							<Clock />
						</div>
						<h2>GPU Clock Speed</h2>
					</div>
					<div>
						<MeterChart
							readings={$hardwareInfo.gpu.cards[0].clock}
							categories={$hardwareInfo.gpu.cards[0].clock.map(
								(temp) => `${temp.name?.replaceAll("GPU", "")} (${(temp.value / 1000).toFixed(1)} GHz)`,
							)}
							type={{ name: "clock speed", unit: "MHz" }}
						/>
					</div>
				</div>
			{/if}

			{#if $hardwareInfo.gpu.cards?.[0]?.power.length > 0}
				<div class="transparent-800 rounded-xl p-8 sm:p-4">
					<div class="mb-5 flex items-center gap-3">
						<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
							<Plug />
						</div>

						<h2>GPU Power Usage</h2>
					</div>
					<h3>Power usage: {$hardwareInfo.gpu.cards[0].power.reduce((a, b) => a + b.value, 0)} W</h3>
					<div>
						<MeterChart
							readings={$hardwareInfo.gpu.cards[0].power}
							categories={$hardwareInfo.gpu.cards[0].power.map((temp) => `${temp.name?.replaceAll("GPU", "")} (${temp.value} W)`)}
							type={{ name: "power usage", unit: "W" }}
						/>
					</div>
				</div>
			{/if}

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
						<h3>MAC address: {macAddress}</h3>
						<h3>Gateway: {gateway} ({dns})</h3>
						<h3>Speed: {speed} Mbit/s</h3>
					</div>
				{/each}
			</div>
		</div>
	</div>
</div>

<script lang="ts">
	import { GaugeChart, hardwareInfo, MeterChart, Progress, settings, state } from "ui"
	import {
		Gauge,
		CircuitBoard,
		Clock,
		Fan,
		HardDrive,
		Monitor,
		Network,
		Plug,
		Thermometer,
		Zap,
		Cpu,
		Battery,
		Megaphone,
		Settings,
		MonitorSmartphone,
	} from "lucide-svelte"
	import { GpuCard, Memory, PcDisplay } from "svelte-bootstrap-icons"
	import { open } from "@tauri-apps/plugin-shell"
</script>
