<div class="transparent-900 m-10 mx-auto w-11/12 rounded-xl sm:m-5 sm:w-full">
	<!-- Row 1 -->
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
							<ProgressBar load={$hardwareInfo.gpu.load} i={2} />
						{/await}
					{/if}
				</div>
			</div>
		</div>
	</div>

	<!-- Row 2 -->
	<div class="mx-10 mt-10 flex gap-5 sm:flex-wrap">
		<!-- CPU info -->
		<div class="flex w-1/3 flex-col gap-5 text-left sm:w-full">
			<div class="transparent-800 rounded-xl p-10">
				<div class="mb-5 flex items-baseline gap-3">
					<svg
						xmlns="http://www.w3.org/2000/svg"
						width="24"
						height="24"
						viewBox="0 0 24 24"
						fill="none"
						stroke="currentColor"
						stroke-width="2"
						stroke-linecap="round"
						stroke-linejoin="round"><path d="M14 4v10.54a4 4 0 1 1-4 0V4a2 2 0 0 1 4 0Z" /></svg
					>
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
						i={0}
						type={{ name: "temperature", unit: "°C" }}
					/>
				</div>
			</div>

			<div class="transparent-800 rounded-xl p-10">
				<div class="mb-5 flex items-baseline gap-3">
					<svg
						xmlns="http://www.w3.org/2000/svg"
						width="24"
						height="24"
						viewBox="0 0 24 24"
						fill="none"
						stroke="currentColor"
						stroke-width="2"
						stroke-linecap="round"
						stroke-linejoin="round"><circle cx="12" cy="12" r="10" /><polyline points="12 6 12 12 16 14" /></svg
					>
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
						i={2}
						type={{ name: "clock speed", unit: "MHz" }}
					/>
				</div>
			</div>

			<div class="transparent-800 rounded-xl p-10">
				<div class="mb-5 flex items-baseline gap-3">
					<svg
						xmlns="http://www.w3.org/2000/svg"
						width="24"
						height="24"
						viewBox="0 0 24 24"
						fill="none"
						stroke="currentColor"
						stroke-width="2"
						stroke-linecap="round"
						stroke-linejoin="round"
						><path d="M12 22v-5" /><path d="M9 7V2" /><path d="M15 7V2" /><path d="M6 13V8h12v5a4 4 0 0 1-4 4h-4a4 4 0 0 1-4-4Z" /></svg
					>
					<h2>CPU Power Usage</h2>
				</div>
				<h3>Power usage: {$hardwareInfo.cpu.power.reduce((a, b) => a + b.value, 0)} W</h3>
				<div>
					<MeterChart
						readings={$hardwareInfo.cpu.power.filter((power) => power.value !== 0)}
						categories={$hardwareInfo.cpu.power
							.filter((power) => power.value !== 0)
							.map((temp) => `${temp.name.replaceAll("CPU", "")} (${temp.value} W)`)}
						i={3}
						type={{ name: "power usage", unit: "W" }}
					/>
				</div>
			</div>

			<div class="transparent-800 rounded-xl p-10">
				<div class="mb-5 flex items-baseline gap-3">
					<svg
						xmlns="http://www.w3.org/2000/svg"
						width="24"
						height="24"
						viewBox="0 0 24 24"
						fill="none"
						stroke="currentColor"
						stroke-width="2"
						stroke-linecap="round"
						stroke-linejoin="round"><polygon points="13 2 3 14 12 14 11 22 21 10 12 10 13 2" /></svg
					>
					<h2>CPU Voltage</h2>
				</div>
				<h3>Avg. voltage: {($hardwareInfo.cpu.voltage.reduce((a, b) => a + b.value, 0) / $hardwareInfo.cpu.voltage.length).toFixed(1)} V</h3>
				<div>
					<MeterChart
						readings={$hardwareInfo.cpu.voltage}
						categories={$hardwareInfo.cpu.voltage.map((temp, i) => `Core #${i} (${temp.value} V)`)}
						i={5}
						type={{ name: "voltage", unit: "V" }}
					/>
				</div>
			</div>
		</div>

		<!-- RAM info -->
		<div class="flex w-1/3 flex-col gap-5 text-left sm:w-full">
			<div class="transparent-800 rounded-xl p-10">
				<div class="mb-5 flex items-baseline gap-3">
					<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="bi bi-memory" viewBox="0 0 16 16">
						<path
							d="M1 3a1 1 0 0 0-1 1v8a1 1 0 0 0 1 1h4.586a1 1 0 0 0 .707-.293l.353-.353a.5.5 0 0 1 .708 0l.353.353a1 1 0 0 0 .707.293H15a1 1 0 0 0 1-1V4a1 1 0 0 0-1-1H1Zm.5 1h3a.5.5 0 0 1 .5.5v4a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-4a.5.5 0 0 1 .5-.5Zm5 0h3a.5.5 0 0 1 .5.5v4a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-4a.5.5 0 0 1 .5-.5Zm4.5.5a.5.5 0 0 1 .5-.5h3a.5.5 0 0 1 .5.5v4a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-4ZM2 10v2H1v-2h1Zm2 0v2H3v-2h1Zm2 0v2H5v-2h1Zm3 0v2H8v-2h1Zm2 0v2h-1v-2h1Zm2 0v2h-1v-2h1Zm2 0v2h-1v-2h1Z"
						/>
					</svg>
					<h2>RAM Usage</h2>
				</div>
				<h3>
					Memory: {`${$hardwareInfo.ram.load[0].value.toFixed(1)}/${(
						$hardwareInfo.ram.load[0].value + $hardwareInfo.ram.load[1].value
					).toFixed(1)} GB`}
				</h3>
				<div>
					<MeterChart readings={[$hardwareInfo.ram.load[0]]} categories={["RAM usage"]} i={8} type={{ name: "memory usage", unit: "GB" }} />
				</div>
			</div>

			<div class="transparent-800 rounded-xl p-10">
				<div class="mb-5 flex items-baseline gap-3">
					<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="bi bi-memory" viewBox="0 0 16 16">
						<path
							d="M1 3a1 1 0 0 0-1 1v8a1 1 0 0 0 1 1h4.586a1 1 0 0 0 .707-.293l.353-.353a.5.5 0 0 1 .708 0l.353.353a1 1 0 0 0 .707.293H15a1 1 0 0 0 1-1V4a1 1 0 0 0-1-1H1Zm.5 1h3a.5.5 0 0 1 .5.5v4a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-4a.5.5 0 0 1 .5-.5Zm5 0h3a.5.5 0 0 1 .5.5v4a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-4a.5.5 0 0 1 .5-.5Zm4.5.5a.5.5 0 0 1 .5-.5h3a.5.5 0 0 1 .5.5v4a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-4ZM2 10v2H1v-2h1Zm2 0v2H3v-2h1Zm2 0v2H5v-2h1Zm3 0v2H8v-2h1Zm2 0v2h-1v-2h1Zm2 0v2h-1v-2h1Zm2 0v2h-1v-2h1Z"
						/>
					</svg>
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
						i={9}
						type={{ name: "virtual memory usage", unit: "GB" }}
					/>
				</div>
			</div>
		</div>

		<!-- GPU info -->
		<div class="flex w-1/3 flex-col gap-5 text-left sm:w-full">
			{#if $hardwareInfo.gpu.temperature.length > 0}
				<div class="transparent-800 rounded-xl p-10">
					<div class="mb-5 flex items-baseline gap-3">
						<svg
							xmlns="http://www.w3.org/2000/svg"
							width="24"
							height="24"
							viewBox="0 0 24 24"
							fill="none"
							stroke="currentColor"
							stroke-width="2"
							stroke-linecap="round"
							stroke-linejoin="round"><path d="M14 4v10.54a4 4 0 1 1-4 0V4a2 2 0 0 1 4 0Z" /></svg
						>
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
							i={1}
							type={{ name: "temperature", unit: "°C" }}
						/>
					</div>
				</div>
			{/if}

			{#if $hardwareInfo.gpu.fan.length > 0}
				<div class="transparent-800 rounded-xl p-10">
					<div class="mb-5 flex items-baseline gap-3">
						<svg
							xmlns="http://www.w3.org/2000/svg"
							width="24"
							height="24"
							viewBox="0 0 24 24"
							fill="none"
							stroke="currentColor"
							stroke-width="2"
							stroke-linecap="round"
							stroke-linejoin="round"
							><path
								d="M10.827 16.379a6.082 6.082 0 0 1-8.618-7.002l5.412 1.45a6.082 6.082 0 0 1 7.002-8.618l-1.45 5.412a6.082 6.082 0 0 1 8.618 7.002l-5.412-1.45a6.082 6.082 0 0 1-7.002 8.618l1.45-5.412Z"
							/><path d="M12 12v.01" /></svg
						>
						<h2>GPU Fan Speed</h2>
					</div>
					<h3>Avg. fan speed: {Math.round($hardwareInfo.gpu.fan.reduce((a, b) => a + b.value, 0) / $hardwareInfo.gpu.fan.length)} RPM</h3>
					{#if $hardwareInfo.gpu.fan[0].max > 0}
						<div>
							<MeterChart
								categories={$hardwareInfo.gpu.fan.map((temp, i) => `Fan #${i} (${temp.value} RPM)`)}
								readings={$hardwareInfo.gpu.fan}
								i={7}
								type={{ name: "fan speed", unit: "RPM" }}
							/>
						</div>
					{/if}
				</div>
			{/if}

			{#if $hardwareInfo.gpu.memory.length > 2}
				<div class="transparent-800 rounded-xl p-10">
					<div class="mb-5 flex items-baseline gap-3">
						<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="bi bi-memory" viewBox="0 0 16 16">
							<path
								d="M1 3a1 1 0 0 0-1 1v8a1 1 0 0 0 1 1h4.586a1 1 0 0 0 .707-.293l.353-.353a.5.5 0 0 1 .708 0l.353.353a1 1 0 0 0 .707.293H15a1 1 0 0 0 1-1V4a1 1 0 0 0-1-1H1Zm.5 1h3a.5.5 0 0 1 .5.5v4a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-4a.5.5 0 0 1 .5-.5Zm5 0h3a.5.5 0 0 1 .5.5v4a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-4a.5.5 0 0 1 .5-.5Zm4.5.5a.5.5 0 0 1 .5-.5h3a.5.5 0 0 1 .5.5v4a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-4ZM2 10v2H1v-2h1Zm2 0v2H3v-2h1Zm2 0v2H5v-2h1Zm3 0v2H8v-2h1Zm2 0v2h-1v-2h1Zm2 0v2h-1v-2h1Zm2 0v2h-1v-2h1Z"
							/>
						</svg>
						<h2>GPU Memory Usage</h2>
					</div>
					<h3>GPU memory: {`${$hardwareInfo.gpu.memory[0].value.toFixed(1)}/${$hardwareInfo.gpu.memory[2].value} GB`} GB</h3>
					<div>
						<MeterChart
							readings={[$hardwareInfo.gpu.memory[0]]}
							categories={["GPU memory usage"]}
							i={10}
							type={{ name: "GPU memory usage", unit: "GB" }}
						/>
					</div>
				</div>
			{/if}

			{#if $hardwareInfo.gpu.clock.length > 0}
				<div class="transparent-800 rounded-xl p-10">
					<div class="mb-5 flex items-baseline gap-3">
						<svg
							xmlns="http://www.w3.org/2000/svg"
							width="24"
							height="24"
							viewBox="0 0 24 24"
							fill="none"
							stroke="currentColor"
							stroke-width="2"
							stroke-linecap="round"
							stroke-linejoin="round"><circle cx="12" cy="12" r="10" /><polyline points="12 6 12 12 16 14" /></svg
						>
						<h2>GPU Clock Speed</h2>
					</div>
					<div>
						<MeterChart
							readings={$hardwareInfo.gpu.clock}
							categories={$hardwareInfo.gpu.clock.map(
								(temp) => `${temp.name.replaceAll("GPU", "")} (${(temp.value / 1000).toFixed(1)} GHz)`,
							)}
							i={6}
							type={{ name: "clock speed", unit: "MHz" }}
						/>
					</div>
				</div>
			{/if}

			{#if $hardwareInfo.gpu.power.length > 0}
				<div class="transparent-800 rounded-xl p-10">
					<div class="mb-5 flex items-baseline gap-3">
						<svg
							xmlns="http://www.w3.org/2000/svg"
							width="24"
							height="24"
							viewBox="0 0 24 24"
							fill="none"
							stroke="currentColor"
							stroke-width="2"
							stroke-linecap="round"
							stroke-linejoin="round"
							><path d="M12 22v-5" /><path d="M9 7V2" /><path d="M15 7V2" /><path
								d="M6 13V8h12v5a4 4 0 0 1-4 4h-4a4 4 0 0 1-4-4Z"
							/></svg
						>
						<h2>GPU Power Usage</h2>
					</div>
					<h3>Power usage: {$hardwareInfo.gpu.power.reduce((a, b) => a + b.value, 0)} W</h3>
					<div>
						<MeterChart
							readings={$hardwareInfo.gpu.power}
							categories={$hardwareInfo.gpu.power.map((temp) => `${temp.name.replaceAll("GPU", "")} (${temp.value} W)`)}
							i={4}
							type={{ name: "power usage", unit: "W" }}
						/>
					</div>
				</div>
			{/if}
		</div>
	</div>

	<!-- Row 3 -->
	<div class="mx-10 mt-10 flex gap-5 pb-10 sm:flex-wrap">
		<!-- Drives -->
		<div class="flex w-1/3 flex-col gap-5 text-left sm:w-full">
			<div class="transparent-800 rounded-xl p-10">
				<div class="flex items-baseline gap-3">
					<svg
						xmlns="http://www.w3.org/2000/svg"
						width="24"
						height="24"
						viewBox="0 0 24 24"
						fill="none"
						stroke="currentColor"
						stroke-width="2"
						stroke-linecap="round"
						stroke-linejoin="round"
						><line x1="22" y1="12" x2="2" y2="12" /><path
							d="M5.45 5.11 2 12v6a2 2 0 0 0 2 2h16a2 2 0 0 0 2-2v-6l-3.45-6.89A2 2 0 0 0 16.76 4H7.24a2 2 0 0 0-1.79 1.11z"
						/><line x1="6" y1="16" x2="6.01" y2="16" /><line x1="10" y1="16" x2="10.01" y2="16" /></svg
					>
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

			<div class="transparent-800 rounded-xl p-10">
				<div class="mb-5 flex items-baseline gap-3">
					<svg
						xmlns="http://www.w3.org/2000/svg"
						width="24"
						height="24"
						viewBox="0 0 24 24"
						fill="none"
						stroke="currentColor"
						stroke-width="2"
						stroke-linecap="round"
						stroke-linejoin="round"><path d="M14 4v10.54a4 4 0 1 1-4 0V4a2 2 0 0 1 4 0Z" /></svg
					>
					<h2>Drive Temperatures</h2>
				</div>
				<div>
					<MeterChart
						readings={$hardwareInfo.system.storage.disks.map((disk) => disk.temperature)}
						categories={$hardwareInfo.system.storage.disks.map((temp, i) => `${temp.name} (${temp.temperature.value} °C)`)}
						i={100}
						type={{ name: "temperature", unit: "°C" }}
					/>
				</div>
			</div>
		</div>

		<!-- System -->
		<div class="flex w-1/3 flex-col gap-5 text-left sm:w-full">
			<div class="transparent-800 rounded-xl p-10">
				<div class="flex items-baseline gap-3">
					<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="bi bi-pc-display" viewBox="0 0 16 16">
						<path
							d="M8 1a1 1 0 0 1 1-1h6a1 1 0 0 1 1 1v14a1 1 0 0 1-1 1H9a1 1 0 0 1-1-1V1Zm1 13.5a.5.5 0 1 0 1 0 .5.5 0 0 0-1 0Zm2 0a.5.5 0 1 0 1 0 .5.5 0 0 0-1 0ZM9.5 1a.5.5 0 0 0 0 1h5a.5.5 0 0 0 0-1h-5ZM9 3.5a.5.5 0 0 0 .5.5h5a.5.5 0 0 0 0-1h-5a.5.5 0 0 0-.5.5ZM1.5 2A1.5 1.5 0 0 0 0 3.5v7A1.5 1.5 0 0 0 1.5 12H6v2h-.5a.5.5 0 0 0 0 1H7v-4H1.5a.5.5 0 0 1-.5-.5v-7a.5.5 0 0 1 .5-.5H7V2H1.5Z"
						/>
					</svg>
					<h2 class="mb-5">System</h2>
				</div>
				<div class="select-text">
					<h3>CPU: {$hardwareInfo.cpu.name}</h3>
					<h3>RAM: {Math.round($hardwareInfo.ram.load[0].value + $hardwareInfo.ram.load[1].value)} GB</h3>
					<h3>GPU: {$hardwareInfo.gpu.name}</h3>
					<h3>MB: {$hardwareInfo.system.motherboard.name}</h3>
					<h3>OS: {$hardwareInfo.system.os.name}</h3>
				</div>
			</div>

			<div class="transparent-800 rounded-xl p-10">
				<div class="flex items-baseline gap-3">
					<svg
						xmlns="http://www.w3.org/2000/svg"
						width="24"
						height="24"
						viewBox="0 0 24 24"
						fill="none"
						stroke="currentColor"
						stroke-width="2"
						stroke-linecap="round"
						stroke-linejoin="round"
						><rect x="2" y="3" width="20" height="14" rx="2" ry="2" /><line x1="8" y1="21" x2="16" y2="21" /><line
							x1="12"
							y1="17"
							x2="12"
							y2="21"
						/></svg
					>
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

			<div class="transparent-800 rounded-xl p-10">
				<div class="flex items-baseline gap-3">
					<svg
						xmlns="http://www.w3.org/2000/svg"
						width="24"
						height="24"
						viewBox="0 0 24 24"
						fill="none"
						stroke="currentColor"
						stroke-width="2"
						stroke-linecap="round"
						stroke-linejoin="round"
						><rect x="4" y="4" width="16" height="16" rx="2" ry="2" /><rect x="9" y="9" width="6" height="6" /><line
							x1="9"
							y1="2"
							x2="9"
							y2="4"
						/><line x1="15" y1="2" x2="15" y2="4" /><line x1="9" y1="21" x2="9" y2="22" /><line x1="15" y1="20" x2="15" y2="22" /><line
							x1="20"
							y1="9"
							x2="22"
							y2="9"
						/><line x1="20" y1="14" x2="22" y2="14" /><line x1="2" y1="9" x2="4" y2="9" /><line x1="2" y1="14" x2="4" y2="14" /></svg
					>
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
			<div class="transparent-800 rounded-xl p-10">
				<div class="flex items-baseline gap-3">
					<svg
						xmlns="http://www.w3.org/2000/svg"
						width="24"
						height="24"
						viewBox="0 0 24 24"
						fill="none"
						stroke="currentColor"
						stroke-width="2"
						stroke-linecap="round"
						stroke-linejoin="round"
						><rect x="9" y="2" width="6" height="6" /><rect x="16" y="16" width="6" height="6" /><rect
							x="2"
							y="16"
							width="6"
							height="6"
						/><path d="M5 16v-4h14v4" /><path d="M12 12V8" /></svg
					>
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
