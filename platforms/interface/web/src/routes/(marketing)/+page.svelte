<svelte:head>
	<link rel="canonical" href="https://coresmonitor.com/" />
	{@html structuredData}
</svelte:head>

<Header />

<main class="w-full">
	<section class="heroSection relative overflow-hidden pb-24 pt-16 md:pt-28">
		<div class="relative mx-auto grid max-w-7xl items-center gap-12 px-6 lg:grid-cols-[0.9fr_1.1fr] lg:gap-8">
			<div class="select-text">
				<h1 class="text-4xl font-bold leading-[1.05] tracking-tight text-white md:text-6xl">
					A modern hardware monitor for Windows, Linux, and macOS
				</h1>
				<p class="mt-8 max-w-xl text-xl leading-relaxed text-gray-200">
					Monitor your devices in real-time. Access detailed hardware statistics from any browser, anywhere.
				</p>
				<div class="mt-10 flex flex-row flex-wrap items-center gap-6">
					<a
						href="#downloads"
						class="bg-cores-alternative hover:text-cores-alternative flex transform flex-row items-center gap-2 rounded-2xl px-6 py-4 text-xl font-semibold text-white duration-200 ease-in-out hover:translate-y-0.5 hover:bg-white"
					>
						<ArrowDownToLine class="hidden md:block" />
						Download Cores
					</a>
					<a
						href="#features"
						class="text-lg font-semibold text-gray-200 underline decoration-gray-500 underline-offset-8 duration-200 ease-in-out hover:text-white"
					>
						See what it tracks
					</a>
				</div>
				<p class="text-html-gray mt-6 text-base">Free 7-day trial. Buy once, no subscription required.</p>
			</div>

			<div class="deviceStage" aria-label="Cores running on a laptop and phone">
				<img
					class="deviceLaptop"
					width="850"
					height="474"
					src="https://cdn.levminer.com/cores/laptop2.png"
					alt="Cores dashboard with CPU, GPU and memory statistics on a laptop"
					fetchpriority="high"
				/>
				<img
					class="devicePhone"
					width="280"
					height="440"
					src="https://cdn.levminer.com/cores/phone2.webp"
					alt="Cores mobile app with live hardware readings on a phone"
				/>
			</div>
		</div>

		<svg class="h-52 w-full md:h-72" viewBox="0 0 1000 200" preserveAspectRatio="none" aria-hidden="true">
			<defs>
				<linearGradient id="traceStroke" x1="0" y1="0" x2="1" y2="0">
					<stop offset="0%" stop-color="#35cbfd" />
					<stop offset="55%" stop-color="#ff5380" />
					<stop offset="100%" stop-color="#9d0cfd" />
				</linearGradient>
				<linearGradient id="traceFill" x1="0" y1="0" x2="0" y2="1">
					<stop offset="0%" stop-color="#ff5380" stop-opacity="0.3" />
					<stop offset="100%" stop-color="#ff5380" stop-opacity="0" />
				</linearGradient>
			</defs>
			<path d={traceAreaPath} fill="url(#traceFill)" />
			<path d={tracePath} fill="none" stroke="url(#traceStroke)" stroke-width="2" stroke-linecap="round" vector-effect="non-scaling-stroke" />
		</svg>

		<div class="relative mx-auto -mt-14 max-w-6xl px-6">
			<dl class="flex flex-row flex-wrap gap-x-14 gap-y-6">
				<!-- {#each sensors as sensor}
					<div>
						<dt class="text-sm text-gray-200">{sensor.name}</dt>
						<dd class="mono mt-1 text-3xl font-semibold text-white md:text-4xl">
							{fmt(sensor.value, sensor.decimals)}<span class="text-html-gray text-base font-normal"> {sensor.unit}</span>
						</dd>
					</div>
				{/each} -->
			</dl>
		</div>
	</section>

	<section id="features" class="mx-auto max-w-6xl select-text px-6 py-24">
		<h2 class="max-w-2xl text-4xl font-bold tracking-tight text-white md:text-5xl">Monitor any system with ease</h2>
		<p class="mt-4 max-w-2xl text-xl text-gray-200">All your system's hardware readings, in one place.</p>

		<div class="mt-20 space-y-24">
			<div class="grid items-center gap-10 lg:grid-cols-2">
				<div class="max-w-md">
					<div class="bg-cores-min/10 text-cores-min mb-4 flex h-11 w-11 items-center justify-center rounded-lg">
						<House />
					</div>
					<h3 class="text-2xl font-bold text-white">Build your own home screen</h3>
					<p class="mt-3 text-lg text-gray-200">Arrange tiles to show exactly the readings you care about.</p>
				</div>
				<img
					class="w-full rounded-xl border border-gray-500"
					width="1000"
					src="https://cdn.levminer.com/cores/1.png"
					alt="Customizable Cores home screen with hardware tiles"
					loading="lazy"
				/>
			</div>

			<div class="grid items-center gap-10 lg:grid-cols-2">
				<div class="max-w-md lg:order-last">
					<div class="bg-cores-current/10 text-cores-current mb-4 flex h-11 w-11 items-center justify-center rounded-lg">
						<ChartLine />
					</div>
					<h3 class="text-2xl font-bold text-white">Every sensor, charted</h3>
					<p class="mt-3 text-lg text-gray-200">
						Minimum, current, and maximum values for every hardware component, with detailed charts you can export as .csv or image.
					</p>
				</div>
				<img
					class="w-full rounded-xl border border-gray-500"
					width="1000"
					src="https://cdn.levminer.com/cores/2.png"
					alt="Detailed hardware statistics with minimum, current and maximum charts"
					loading="lazy"
				/>
			</div>

			<div class="grid items-center gap-10 lg:grid-cols-2">
				<div class="max-w-md">
					<div class="bg-cores-max/10 text-cores-max mb-4 flex h-11 w-11 items-center justify-center rounded-lg">
						<Radio />
					</div>
					<h3 class="text-2xl font-bold text-white">Check any machine from any browser</h3>
					<p class="mt-3 text-lg text-gray-200">
						Remote monitoring runs over WebRTC with end-to-end encryption. Enter a connection code in the dashboard and watch live
						readings from anywhere. No port forwarding, no subscription.
					</p>
				</div>
				<img
					class="w-full rounded-xl border border-gray-500"
					width="1000"
					src="https://cdn.levminer.com/cores/3.png"
					alt="Cores web dashboard monitoring a remote computer"
					loading="lazy"
				/>
			</div>

			<div class="grid items-center gap-10 lg:grid-cols-2">
				<div class="max-w-md lg:order-last">
					<div class="bg-cores-alternative/10 text-cores-alternative mb-4 flex h-11 w-11 items-center justify-center rounded-lg">
						<Bell />
					</div>
					<h3 class="text-2xl font-bold text-white">Easy alerts</h3>
					<p class="mt-3 text-lg text-gray-200">
						Set thresholds for multiple sensors, and Cores notifies you the moment a machine crosses them.
					</p>
				</div>
				<img
					class="w-full rounded-xl border border-gray-500"
					width="1000"
					src="https://cdn.levminer.com/cores/4.png"
					alt="Hardware notification settings for load and temperature alerts"
					loading="lazy"
				/>
			</div>
		</div>

		<div class="mt-24 grid gap-10 border-t border-gray-600 pt-16 md:grid-cols-3">
			<div>
				<div class="bg-cores-min/10 text-cores-min mb-4 flex h-11 w-11 items-center justify-center rounded-lg">
					<Feather />
				</div>
				<h3 class="text-xl font-bold text-white">Lightweight by design</h3>
				<p class="mt-2 text-gray-200">
					Cores runs as a background service with minimal resource usage, so it never competes with your games or workloads.
				</p>
			</div>
			<div>
				<div class="bg-cores-current/10 text-cores-current mb-4 flex h-11 w-11 items-center justify-center rounded-lg">
					<Moon />
				</div>
				<h3 class="text-xl font-bold text-white">Shut down from anywhere</h3>
				<p class="mt-2 text-gray-200">Left in a hurry? Shut down or put any of your computers to sleep remotely.</p>
			</div>
			<div>
				<div class="bg-cores-max/10 text-cores-max mb-4 flex h-11 w-11 items-center justify-center rounded-lg">
					<Power />
				</div>
				<h3 class="text-xl font-bold text-white">Wake on LAN</h3>
				<p class="mt-2 text-gray-200">Start other devices on your network remotely, straight from the dashboard.</p>
			</div>
		</div>
	</section>

	<section id="sensors" class="mx-auto max-w-6xl select-text px-6 py-24">
		<h2 class="max-w-2xl text-4xl font-bold tracking-tight text-white md:text-5xl">Every reading Cores collects</h2>
		<p class="mt-4 max-w-2xl text-xl text-gray-200">A spec sheet's worth of sensors, sampled live from each machine.</p>

		<div class="mt-14 grid grid-cols-1 gap-x-12 gap-y-12 md:grid-cols-3">
			{#each sensorSpecs as group}
				<div>
					<h3 class="border-b border-gray-500 pb-2 text-lg font-bold text-white">{group.name}</h3>
					<ul class="mt-3 space-y-2">
						{#each group.items as [name, sample]}
							<li class="flex items-baseline justify-between gap-3 text-gray-200">
								<span>{name}</span>
								<span class="mono text-html-gray text-sm">{sample}</span>
							</li>
						{/each}
					</ul>
				</div>
			{/each}
		</div>
	</section>

	<section class="mx-auto max-w-6xl select-text px-6 py-24">
		<h2 class="max-w-2xl text-4xl font-bold tracking-tight text-white md:text-5xl">Who runs Cores</h2>
		<div class="mt-14 grid gap-10 md:grid-cols-3">
			<div>
				<div class="bg-cores-min/10 text-cores-min mb-4 flex h-11 w-11 items-center justify-center rounded-lg">
					<Gamepad2 />
				</div>
				<h3 class="text-xl font-bold text-white">Gamers and power users</h3>
				<p class="mt-2 text-gray-200">Keep an eye on temperatures and clocks while you play, and check on your rig from the couch.</p>
			</div>
			<div>
				<div class="bg-cores-current/10 text-cores-current mb-4 flex h-11 w-11 items-center justify-center rounded-lg">
					<Server />
				</div>
				<h3 class="text-xl font-bold text-white">Homelabs</h3>
				<p class="mt-2 text-gray-200">Monitor a Raspberry Pi or an enterprise-grade server at home, all from one dashboard.</p>
			</div>
			<div>
				<div class="bg-cores-max/10 text-cores-max mb-4 flex h-11 w-11 items-center justify-center rounded-lg">
					<MonitorSmartphone />
				</div>
				<h3 class="text-xl font-bold text-white">Office fleets</h3>
				<p class="mt-2 text-gray-200">Watch a whole department of machines remotely, without walking desk to desk.</p>
			</div>
		</div>
	</section>

	<section id="downloads" class="mx-auto max-w-6xl select-text px-6 py-24">
		<h2 class="max-w-2xl text-4xl font-bold tracking-tight text-white md:text-5xl">Download Cores</h2>
		<p class="mt-4 max-w-2xl text-xl text-gray-200">Get the latest version for your platform and start the free 7-day trial.</p>

		<div class="mt-12 flex flex-row flex-wrap gap-5">
			<button
				onclick={() => {
					changeDownload("windows")
				}}
				class={selectedDownload === "windows" ? "selectedDownloadButton" : "downloadButton"}
			>
				<Microsoft class="h-12 w-12 text-current" />
				<span class="text-2xl font-bold text-current">Windows</span>
			</button>

			<button
				onclick={() => {
					changeDownload("linux")
				}}
				class={selectedDownload === "linux" ? "selectedDownloadButton" : "downloadButton"}
			>
				<Ubuntu class="h-12 w-12 text-current" />
				<span class="text-2xl font-bold text-current">Linux</span>
			</button>

			<button
				onclick={() => {
					changeDownload("macos")
				}}
				class={selectedDownload === "macos" ? "selectedDownloadButton" : "downloadButton"}
			>
				<Apple class="h-12 w-12 text-current" />
				<span class="text-2xl font-bold text-current">macOS</span>
			</button>

			<button
				onclick={() => {
					changeDownload("mobile")
				}}
				class={selectedDownload === "mobile" ? "selectedDownloadButton" : "downloadButton"}
			>
				<TabletSmartphone class="h-12 w-12 text-current" />
				<span class="text-2xl font-bold text-current">Mobile</span>
			</button>
		</div>

		<div class="transparent-900 mt-10 max-w-3xl space-y-6 rounded-xl border border-gray-500 p-8 sm:p-4">
			{#if selectedDownload === "windows"}
				<p class="text-lg text-gray-200">Download the installer or get the app from the Microsoft Store.</p>
				<div class="flex flex-row flex-wrap items-center gap-3">
					<a
						href="https://link.levminer.com/cores-installer"
						target="_blank"
						class="smallButton bg-cores-alternative hover:text-cores-alternative border-cores-alternative gap-2 font-bold text-white"
					>
						<Download width="24" height="24" />
						Download for Windows
					</a>
					<a href="https://link.levminer.com/cores-ms-store" target="_blank" class="smallButton">
						<Microsoft width="24" height="24" />
						Microsoft Store
					</a>
				</div>
				<p class="text-html-gray border-t border-gray-500 pt-5">System requirements: Windows 10 or 11, 64-bit processor</p>
			{:else if selectedDownload === "linux"}
				<p class="text-lg text-gray-200">Download the package for your distro or get the daemon.</p>
				<div class="flex flex-row flex-wrap items-center gap-3">
					<a
						href="https://github.com/levminer/cores/releases/latest"
						target="_blank"
						class="smallButton bg-cores-alternative hover:text-cores-alternative border-cores-alternative gap-2 font-bold text-white"
					>
						<Download width="24" height="24" />
						Download for Linux
					</a>
				</div>
				<p class="text-html-gray border-t border-gray-500 pt-5">System requirements: Linux, 64-bit or arm64 processor</p>
			{:else if selectedDownload === "mobile"}
				<p class="text-lg text-gray-200">Download the mobile app from the App Store or the Play Store.</p>
				<div class="flex flex-row flex-wrap items-center gap-3">
					<a
						href="https://link.levminer.com/cores-ios"
						target="_blank"
						class="smallButton bg-cores-alternative hover:text-cores-alternative border-cores-alternative gap-2 font-bold text-white"
					>
						<Download width="24" height="24" />
						App Store
					</a>
					<a
						href="https://link.levminer.com/crs-android"
						target="_blank"
						class="smallButton bg-cores-alternative hover:text-cores-alternative border-cores-alternative gap-2 font-bold text-white"
					>
						<Download width="24" height="24" />
						Play Store
					</a>
				</div>
				<p class="text-html-gray border-t border-gray-500 pt-5">System requirements: iOS 17 or later, Android 8.0 or later</p>
			{:else}
				<p class="text-lg text-gray-200">Download and install the app with the installer.</p>
				<div class="flex flex-row flex-wrap items-center gap-3">
					<a
						href="https://github.com/levminer/cores/releases/latest"
						target="_blank"
						class="smallButton bg-cores-alternative hover:text-cores-alternative border-cores-alternative gap-2 font-bold text-white"
					>
						<Download width="24" height="24" />
						Download for macOS
					</a>
				</div>
				<p class="text-html-gray border-t border-gray-500 pt-5">System requirements: macOS Big Sur, arm64 (Apple Silicon) processor</p>
			{/if}
		</div>
	</section>

	<section id="pricing" class="mx-auto max-w-6xl select-text px-6 py-24">
		<h2 class="max-w-2xl text-4xl font-bold tracking-tight text-white md:text-5xl">Pricing</h2>
		<p class="mt-4 max-w-2xl text-xl text-gray-200">Hate subscriptions too? Buy Cores once and keep it for life.</p>

		<div class="mt-12 grid gap-8 lg:grid-cols-2">
			<div class="transparent-900 border-cores-alternative flex flex-col rounded-xl border-2 p-8 sm:p-4">
				<h3 class="text-2xl font-bold text-white">Cores Pro</h3>
				<p class="mt-1 text-gray-200">For personal use</p>
				<p class="mono mt-6 text-4xl font-semibold text-white">
					$9.99 <span class="text-html-gray text-sm font-normal">one-time purchase</span>
				</p>
				<ul class="mt-6 flex-1 space-y-3">
					<li class="flex items-start gap-2 text-gray-200">
						<Check class="mt-0.5 h-5 w-5 shrink-0 text-green-500" />
						One-time purchase, no subscription
					</li>
					<li class="flex items-start gap-2 text-gray-200">
						<Check class="mt-0.5 h-5 w-5 shrink-0 text-green-500" />
						Monitor up to 5 devices
					</li>
					<li class="flex items-start gap-2 text-gray-200">
						<Check class="mt-0.5 h-5 w-5 shrink-0 text-green-500" />
						Access any device remotely
					</li>
					<li class="flex items-start gap-2 text-gray-200">
						<Check class="mt-0.5 h-5 w-5 shrink-0 text-green-500" />
						Export data as .csv and image
					</li>
				</ul>
				<a
					href="https://link.levminer.com/buy-cores-app?utm_source=web"
					class="button bg-cores-alternative border-cores-alternative hover:text-cores-alternative mt-8 gap-2 font-bold text-white"
				>
					<ShoppingCart />
					Buy Cores Pro
				</a>
			</div>

			<div class="transparent-900 flex flex-col rounded-xl border border-gray-500 p-8 sm:p-4">
				<h3 class="text-2xl font-bold text-white">Cores Advanced</h3>
				<p class="mt-1 text-gray-200">For personal and business use</p>
				<p class="mono mt-6 text-4xl font-semibold text-white">
					$2.99 <span class="text-html-gray text-sm font-normal">per month, per device</span>
				</p>
				<ul class="mt-6 flex-1 space-y-3">
					<li class="flex items-start gap-2 text-gray-200">
						<Check class="mt-0.5 h-5 w-5 shrink-0 text-green-500" />
						Monthly subscription, cancel anytime
					</li>
					<li class="flex items-start gap-2 text-gray-200">
						<Check class="mt-0.5 h-5 w-5 shrink-0 text-green-500" />
						Choose how many devices you monitor
					</li>
					<li class="flex items-start gap-2 text-gray-200">
						<Check class="mt-0.5 h-5 w-5 shrink-0 text-green-500" />
						Access any device remotely
					</li>
					<li class="flex items-start gap-2 text-gray-200">
						<Check class="mt-0.5 h-5 w-5 shrink-0 text-green-500" />
						Export data as .csv and image
					</li>
				</ul>
				<a href="https://link.levminer.com/buy-cores-app?utm_source=web" class="button mt-8 gap-2 font-bold">
					<ShoppingCart />
					Buy Cores Advanced
				</a>
			</div>
		</div>
	</section>

	<section id="faq" class="mx-auto max-w-6xl select-text px-6 py-24">
		<h2 class="max-w-2xl text-4xl font-bold tracking-tight text-white md:text-5xl">Frequently asked questions</h2>
		<div class="mt-10 max-w-3xl">
			{#each faqs as faq}
				<details class="group border-b border-gray-600 py-5">
					<summary
						class="flex cursor-pointer list-none items-center justify-between gap-4 text-lg font-semibold text-white [&::-webkit-details-marker]:hidden"
					>
						{faq.question}
						<span class="text-html-gray text-2xl font-normal duration-200 group-open:rotate-45">+</span>
					</summary>
					<p class="mt-3 text-gray-200">{faq.answer}</p>
				</details>
			{/each}
			<p class="mt-8 text-gray-200">
				Something else on your mind? Email <a class="underline" href="mailto:support@coresmonitor.com">support@coresmonitor.com</a> and we will
				help.
			</p>
		</div>
	</section>

	<section class="mx-auto mb-24 max-w-6xl px-6">
		<GetStarted />
	</section>
</main>

<Footer />

<script lang="ts">
	import {
		ArrowDownToLine,
		Bell,
		ChartLine,
		Check,
		Download,
		Feather,
		Gamepad2,
		House,
		MonitorSmartphone,
		Moon,
		Power,
		Radio,
		Server,
		ShoppingCart,
		TabletSmartphone,
	} from "lucide-svelte"
	import { onDestroy, onMount } from "svelte"
	import { Microsoft, Ubuntu, Apple } from "svelte-bootstrap-icons"
	import Footer from "../../components/Footer.svelte"
	import Header from "../../components/Header.svelte"
	import GetStarted from "../../components/GetStarted.svelte"
	import { browser } from "$app/environment"

	let selectedDownload = $state("windows")

	const changeDownload = (value: string) => {
		selectedDownload = value
	}

	// simulated hero telemetry
	type LiveSensor = {
		name: string
		unit: string
		decimals: number
		base: number
		amp: number
		floor: number
		ceil: number
		value: number
		history: number[]
	}

	// deterministic so the server render matches hydration
	const makeHistory = (base: number, amp: number) =>
		Array.from({ length: 72 }, (_, i) => base + Math.sin(i / 4.5) * amp + Math.sin(i / 1.7) * amp * 0.3)

	let sensors = $state<LiveSensor[]>([
		{ name: "CPU temperature", unit: "°C", decimals: 0, base: 49, amp: 4, floor: 40, ceil: 86, value: 49, history: makeHistory(49, 4) },
		{ name: "CPU load", unit: "%", decimals: 0, base: 34, amp: 12, floor: 2, ceil: 100, value: 34, history: makeHistory(34, 12) },
		{ name: "GPU temperature", unit: "°C", decimals: 0, base: 57, amp: 3, floor: 38, ceil: 90, value: 57, history: makeHistory(57, 3) },
		{ name: "Memory used", unit: "GB", decimals: 1, base: 11.6, amp: 0.7, floor: 6, ceil: 32, value: 11.6, history: makeHistory(11.6, 0.7) },
	])

	const fmt = (value: number, decimals: number) => value.toFixed(decimals)

	const smoothPath = (history: number[]) => {
		const min = Math.min(...history)
		const max = Math.max(...history)
		const range = max - min || 1
		const coordinates = history.map((value, index) => ({
			x: (index / (history.length - 1)) * 1000,
			y: 125 - ((value - min) / range) * 70,
		}))
		let path = `M ${coordinates[0].x},${coordinates[0].y}`

		for (let index = 0; index < coordinates.length - 1; index++) {
			const previous = coordinates[Math.max(0, index - 1)]
			const current = coordinates[index]
			const next = coordinates[index + 1]
			const following = coordinates[Math.min(coordinates.length - 1, index + 2)]
			const controlOne = { x: current.x + (next.x - previous.x) / 6, y: current.y + (next.y - previous.y) / 6 }
			const controlTwo = { x: next.x - (following.x - current.x) / 6, y: next.y - (following.y - current.y) / 6 }

			path += ` C ${controlOne.x},${controlOne.y} ${controlTwo.x},${controlTwo.y} ${next.x},${next.y}`
		}

		return path
	}

	// CPU temperature drives the hero trace
	const tracePath = $derived(smoothPath(sensors[0].history))
	const traceAreaPath = $derived(`${tracePath} L 1000,200 L 0,200 Z`)

	const tick = () => {
		for (const sensor of sensors) {
			const drift = (Math.random() - 0.5) * sensor.amp * 0.6 + (sensor.base - sensor.value) * 0.05
			sensor.value = Math.min(sensor.ceil, Math.max(sensor.floor, sensor.value + drift))
			sensor.history = [...sensor.history.slice(1), sensor.value]
		}
	}

	let interval: ReturnType<typeof setInterval> | undefined

	const sensorSpecs = [
		{
			name: "Processor",
			items: [
				["Load", "31 %"],
				["Temperature", "48 °C"],
				["Clock speed", "4.7 GHz"],
				["Voltage", "1.21 V"],
				["Power draw", "64 W"],
			],
		},
		{
			name: "Graphics",
			items: [
				["Load", "72 %"],
				["Temperature", "57 °C"],
				["Clock speed", "2.6 GHz"],
				["Fan speed", "1420 RPM"],
				["Power draw", "180 W"],
				["VRAM usage", "8 GB"],
			],
		},
		{
			name: "Memory",
			items: [
				["RAM Usage", "11.4 GB"],
				["Virtual RAM Usage", "3.2 GB"],
				["Speed", "6000 MT/s"],
				["Voltage", "1.35 V"],
				["Temperature", "42 °C"],
			],
		},
		{
			name: "Network",
			items: [
				["Download speed", "250 mbps"],
				["Upload speed", "150 mbps"],
				["Data usage", "412 GB"],
				["Interface speed", "2.5 Gb/s"],
			],
		},
		{
			name: "Storage",
			items: [
				["Read speed", "3.4 GB/s"],
				["Write speed", "2.9 GB/s"],
				["Temperature", "39 °C"],
				["Health", "98 %"],
			],
		},
		{
			name: "System",
			items: [
				["BIOS date", "2026-04-12"],
				["Fan speed", "800 RPM"],
			],
		},
	]

	const faqs = [
		{
			question: "Do you offer a money-back guarantee?",
			answer: "Yes. If you are not happy with Cores, reach out within the first 7 days and you get your money back, no questions asked.",
		},
		{
			question: "How do I activate my license key?",
			answer: "Download the app, log in with your Google or Apple account, then enter your license key on the next screen.",
		},
		{
			question: "Does my hardware support Cores?",
			answer: "Most hardware released in the last 10 years is supported. The free 7-day trial lets you check before you buy.",
		},
		{
			question: "How does remote access work?",
			answer: "Remote monitoring runs over WebRTC and is end-to-end encrypted. You access each computer with its unique connection code.",
		},
	]

	const structuredData = `<script type="application/ld+json">${JSON.stringify([
		{
			"@context": "https://schema.org",
			"@type": "SoftwareApplication",
			name: "Cores",
			operatingSystem: "Windows, Linux, macOS, iOS, Android",
			applicationCategory: "UtilitiesApplication",
			url: "https://coresmonitor.com",
			description:
				"Cores is a modern hardware monitor for Windows, Linux, and macOS. Track CPU, GPU, RAM, storage, and network sensors in real time, with encrypted remote access from any browser.",
			offers: [
				{ "@type": "Offer", name: "Cores Pro", price: "9.99", priceCurrency: "USD" },
				{ "@type": "Offer", name: "Cores Advanced", price: "2.99", priceCurrency: "USD" },
			],
		},
		{
			"@context": "https://schema.org",
			"@type": "FAQPage",
			mainEntity: faqs.map((faq) => ({
				"@type": "Question",
				name: faq.question,
				acceptedAnswer: { "@type": "Answer", text: faq.answer },
			})),
		},
	])}<\/script>`

	onMount(() => {
		// smooth scroll
		document.querySelector("#layout")?.classList.add("scroll-smooth")

		if (!window.matchMedia("(prefers-reduced-motion: reduce)").matches) {
			interval = setInterval(tick, 1200)
		}
	})

	onDestroy(() => {
		if (browser) {
			document.querySelector("#layout")?.classList.remove("scroll-smooth")
			clearInterval(interval)
		}
	})
</script>

<style>
	.heroSection {
		background: radial-gradient(85% 50% at 50% 88%, rgba(157, 12, 253, 0.14), transparent 62%), #0a0a0a;
	}

	.deviceStage {
		position: relative;
		min-height: clamp(19rem, 44vw, 35rem);
		display: flex;
		align-items: center;
		justify-content: center;
	}

	.deviceLaptop {
		width: min(100%, 52rem);
		height: auto;
	}

	.devicePhone {
		position: absolute;
		right: 1%;
		bottom: 2%;
		width: clamp(6.5rem, 23%, 11.5rem);
		height: auto;
		filter: drop-shadow(0 20px 24px rgba(0, 0, 0, 0.4));
	}

	.mono {
		font-family: ui-monospace, "Cascadia Mono", "SF Mono", Menlo, Consolas, monospace;
		font-variant-numeric: tabular-nums;
	}

	@media (max-width: 1023px) {
		.deviceStage {
			min-height: clamp(18rem, 63vw, 31rem);
		}

		.deviceLaptop {
			width: min(88%, 46rem);
		}

		.devicePhone {
			right: 5%;
		}
	}
</style>
