<Header />

<div class="mx-auto w-11/12 rounded-xl sm:w-full">
	<div class="mb-60 mt-10 flex flex-col items-center justify-center text-center">
		<h1 class="bg-gradient-to-r from-purple-400 to-pink-600 bg-clip-text text-8xl font-extrabold text-transparent">Cores</h1>
		<h2 class="mt-10 font-medium">Modern hardware monitor for your</h2>

		{#key index}
			<h1 class="bg-gradient-to-r from-purple-400 to-pink-600 bg-clip-text font-medium italic text-transparent" transition:slide>
				{greetings[index]}
			</h1>
		{/key}

		<div class="">
			<!-- @ts-ignore -->
			<img
				width="800"
				height="450"
				src="https://pub-59a3ca658f4c4ece9990b6c50534538e.r2.dev/cores/landing.webp"
				alt="Cores on desktop computer and on a phone"
			/>
		</div>
	</div>

	<div class="space-y-20 mb-20">
		<div id="features" class="flex justify-center rounded-xl bg-gradient-to-r from-red-500 to-orange-500 py-20">
			<div class="flex select-text flex-wrap items-center justify-between gap-10 px-20 sm:w-full sm:px-2 md:flex-nowrap">
				<div class="flex w-full flex-col">
					<!-- card 0 -->
					<div class="mx-auto flex w-full flex-col items-start justify-center rounded-xl bg-gray-900 p-10 text-left">
						<h2 class="mb-5 bg-gradient-to-r bg-clip-text text-4xl font-extrabold text-transparent">Informative home screen</h2>
						<h3>Track your CPU, GPU and RAM stats on the home screen.</h3>
					</div>

					<div class="mt-5 flex flex-row justify-between gap-5 sm:flex-col">
						<!-- card 1 -->
						<div class="w-full rounded-xl bg-gray-900 p-8 sm:p-4">
							<div class="mb-5 flex items-center gap-3">
								<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg bg-gradient-to-r p-3 sm:p-2">
									<Cpu />
								</div>
								<h2 class="bg-gradient-to-r bg-clip-text font-extrabold text-transparent">CPU</h2>
							</div>
							<div class="select-text">
								<h3>Temperature</h3>
								<h3>Clock speed</h3>
								<h3>Power usage</h3>
								<h3>Voltage</h3>
							</div>
						</div>

						<!-- card 2 -->
						<div class="w-full rounded-xl bg-gray-900 p-8 sm:p-4">
							<div class="mb-5 flex items-center gap-3">
								<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg bg-gradient-to-r p-3 sm:p-2">
									<GpuCard height="24" width="24" />
								</div>
								<h2 class="bg-gradient-to-r bg-clip-text font-extrabold text-transparent">GPU</h2>
							</div>
							<div class="select-text">
								<h3>Temperature</h3>
								<h3>Fan speed</h3>
								<h3>Memory usage</h3>
								<h3>Clock speed</h3>
								<h3>Power usage</h3>
							</div>
						</div>
					</div>

					<div class="mt-5 flex flex-row justify-between gap-5 sm:flex-col">
						<!-- card 3 -->
						<div class="w-full rounded-xl bg-gray-900 p-8 sm:p-4">
							<div class="mb-5 flex items-center gap-3">
								<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg bg-gradient-to-r p-3 sm:p-2">
									<Memory height="24" width="24" />
								</div>
								<h2 class="bg-gradient-to-r bg-clip-text font-extrabold text-transparent">RAM</h2>
							</div>
							<div class="select-text">
								<h3>RAM usage</h3>
								<h3>Virtual RAM usage</h3>
							</div>
						</div>

						<!-- card 4 -->
						<div class="w-full rounded-xl bg-gray-900 p-8 sm:p-4">
							<div class="mb-5 flex items-center gap-3">
								<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg bg-gradient-to-r p-3 sm:p-2">
									<PcDisplay height="24" width="24" />
								</div>
								<h2 class="bg-gradient-to-r bg-clip-text font-extrabold text-transparent">System</h2>
							</div>
							<div class="select-text">
								<h3>Drives</h3>
								<h3>Monitors</h3>
								<h3>Network interfaces</h3>
								<h3>BIOS</h3>
							</div>
						</div>
					</div>
				</div>
				<div class="flex w-full items-center justify-center overflow-hidden rounded-xl bg-black p-3">
					<video autoplay muted src="https://pub-59a3ca658f4c4ece9990b6c50534538e.r2.dev/cores/home.webm" loop />
				</div>
			</div>
		</div>

		<div id="features2" class="flex justify-center rounded-xl bg-gradient-to-r from-teal-200 to-teal-500 py-20">
			<div class="flex select-text flex-wrap items-center justify-between gap-10 px-20 sm:w-full sm:px-2 md:flex-nowrap">
				<div class="flex w-full items-center justify-center overflow-hidden rounded-xl bg-black p-3">
					<video autoplay muted src="https://pub-59a3ca658f4c4ece9990b6c50534538e.r2.dev/cores/charts.webm" loop />
				</div>
				<div class="flex w-full flex-col">
					<!-- card 0 -->
					<div class="mx-auto flex w-full flex-col items-start justify-center rounded-xl bg-gray-900 p-10 text-left">
						<h2 class="mb-5 bg-gradient-to-r bg-clip-text text-4xl font-extrabold text-transparent">Detailed statistics</h2>
						<h3>Detailed charts for CPU, GPU and RAM stats.</h3>
					</div>

					<div class="mt-5 flex flex-row justify-between gap-5 sm:flex-col">
						<!-- card 1 -->
						<div class="w-full rounded-xl bg-gray-900 p-8 sm:p-4">
							<div class="mb-5 flex items-center gap-3">
								<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg bg-gradient-to-r p-3 sm:p-2">
									<Info />
								</div>
								<h2 class="bg-gradient-to-r bg-clip-text font-extrabold text-transparent">Info</h2>
							</div>
							<div class="select-text">
								<h3>Detailed information about each component</h3>
							</div>
						</div>

						<!-- card 2 -->
						<div class="w-full rounded-xl bg-gray-900 p-8 sm:p-4">
							<div class="mb-5 flex items-center gap-3">
								<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg bg-gradient-to-r p-3 sm:p-2">
									<BarChart3 />
								</div>
								<h2 class="bg-gradient-to-r bg-clip-text font-extrabold text-transparent">Charts</h2>
							</div>
							<div class="select-text">
								<h3>Track your components status with charts</h3>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>

		<div id="features3" class="flex justify-center rounded-xl bg-gradient-to-r from-indigo-400 to-cyan-400 py-20">
			<div class="flex select-text flex-col flex-wrap items-center justify-between gap-10 px-20 sm:w-full sm:px-2 md:flex-nowrap">
				<div class="flex w-[800px] items-center justify-center overflow-hidden rounded-xl bg-black p-3 sm:w-full">
					<video autoplay muted src="https://pub-59a3ca658f4c4ece9990b6c50534538e.r2.dev/cores/remote.webm" loop />
				</div>
				<div class="flex w-[800px] flex-col sm:w-full">
					<!-- card 0 -->
					<div class="mx-auto flex w-full flex-col items-start justify-center rounded-xl bg-gray-900 p-10 text-left">
						<h2 class="mb-5 bg-gradient-to-r bg-clip-text text-4xl font-extrabold text-transparent">Remote access</h2>
						<h3>Monitor your device from anywhere.</h3>
					</div>

					<div class="mt-5 flex flex-row justify-between gap-5 sm:flex-col">
						<!-- card 1 -->
						<div class="w-full rounded-xl bg-gray-900 p-8 sm:p-4">
							<div class="mb-5 flex items-center gap-3">
								<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg bg-gradient-to-r p-3 sm:p-2">
									<Radio />
								</div>
								<h2 class="bg-gradient-to-r bg-clip-text font-extrabold text-transparent">Remote connection</h2>
							</div>
							<div class="select-text">
								<h3>You can monitor your device from any device with a browser.</h3>
							</div>
						</div>

						<!-- card 2 -->
						<div class="w-full rounded-xl bg-gray-900 p-8 sm:p-4">
							<div class="mb-5 flex items-center gap-3">
								<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg bg-gradient-to-r p-3 sm:p-2">
									<Network />
								</div>
								<h2 class="bg-gradient-to-r bg-clip-text font-extrabold text-transparent">P2P connection</h2>
							</div>
							<div class="select-text">
								<h3>You can connect directly to the host computer, no server needed</h3>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>

		<div id="pricing" class="flex justify-center rounded-xl bg-gradient-to-r from-fuchsia-600 to-pink-600 py-20">
			<div class="flex select-text flex-wrap items-center justify-between gap-10 px-20 sm:w-full sm:px-2 md:flex-nowrap">
				<div class="flex w-full flex-col">
					<!-- card 0 -->
					<div class="mx-auto flex w-full flex-col items-start justify-center rounded-xl bg-gray-900 p-10 text-left">
						<h2 class="mb-5 bg-gradient-to-r bg-clip-text text-4xl font-extrabold text-transparent">Pricing</h2>
						<h3>Do you hate subscriptions too? Buy Cores now and get life time access.</h3>
					</div>

					<div class="mt-5 flex flex-row justify-between gap-5 sm:flex-col">
						<!-- card 1 -->
						<div class="w-full rounded-xl bg-gray-900 p-8 sm:p-4">
							<div class="flex items-center justify-center">
								<div
									class="my-5 inline-flex gap-3 rounded-xl bg-gradient-to-r from-purple-400 to-pink-600 p-3 text-lg font-medium backdrop-blur-sm"
								>
									<button class="rounded-xl bg-white/30 px-3 py-3 backdrop-blur-xl">
										<p>Windows</p>
									</button>
									<div class="px-3 py-3">
										<p>Linux</p>
									</div>
									<div class="px-3 py-3">
										<p>macOS</p>
									</div>
								</div>
							</div>

							<div class="mx-auto mt-5 flex max-w-lg flex-col gap-5 rounded-xl border-2 border-purple-400 p-5 sm:flex-col">
								<h1
									class="bg-gradient-to-r from-purple-400 to-pink-600 bg-clip-text px-5 text-center text-4xl font-extrabold text-transparent"
								>
									Cores for Windows
								</h1>
								<div class="px-5 text-center sm:w-full">
									<h3 class="text-[3rem]">$14<sup>99</sup></h3>

									<div class="text-center">
										<button
											class="button mt-5 w-full hover:translate-y-0.5 hover:animate-pulse hover:border-pink-600 hover:text-pink-400"
										>
											Coming soon
										</button>
										<div class="mt-3">
											<a href="" class="underline">Download the trial for free</a>
										</div>
									</div>
								</div>
								<div class="flex flex-col justify-center space-y-2 border-t-2 border-gray-600 px-5 pt-3 text-left text-lg sm:w-full">
									<div class="flex items-center space-x-2">
										<Check class="h-5 w-5 text-green-500" />
										<p>For personal use</p>
									</div>
									<div class="flex items-center space-x-2">
										<Check class="h-5 w-5 text-green-500" />
										<p>One-time purchase, no subscriptions</p>
									</div>
									<div class="flex items-center space-x-2">
										<Check class="h-5 w-5 text-green-500" />
										<p>Open source</p>
									</div>
									<div class="flex items-center space-x-2">
										<Check class="h-5 w-5 text-green-500" />
										<p>Use on up to 10 devices</p>
									</div>

									<!-- <p>Access to free updates for a year</p> -->
								</div>
							</div>
							<div class="mt-4 flex justify-center gap-2 text-center flex-wrap text-lg">
								<p>Want to use Cores as a business?</p>
								<a class="underline" href="mailto:cores@levminer.com">Contact</a>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>

	<!-- <div class="flex justify-center bg-gradient-to-r from-red-500 to-orange-500 py-96">
		<div class="flex items-center justify-center gap-10">
			<div
				class="isolate aspect-video w-96 transform rounded-xl bg-white/20 p-8 shadow-lg ring-1 ring-black/5 duration-200 ease-in-out hover:rotate-1 hover:scale-105 sm:p-4"
			>
				<div class="mb-5 flex items-center gap-3">
					<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
						<Cpu />
					</div>
					<h2>Easy monitoring</h2>
				</div>
				<div class="select-text text-left">
					<p class="text-base leading-relaxed">Monitor CPU/RAM/GPU usage like clock speed, voltage, memory usage</p>
				</div>
			</div>

			<div
				class="relative -top-5 isolate aspect-video w-96 transform rounded-xl bg-white/20 p-8 shadow-lg ring-1 ring-black/5 duration-200 ease-in-out hover:rotate-1 hover:scale-105 sm:p-4"
			>
				<div class="mb-5 flex items-center gap-3">
					<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
						<Cpu />
					</div>
					<h2>Easy monitoring</h2>
				</div>
				<div class="select-text text-left">
					<p class="text-base leading-relaxed">Monitor CPU/RAM/GPU usage like clock speed, voltage, memory usage</p>
				</div>
			</div>

			<div
				class="isolate aspect-video w-96 transform rounded-xl bg-white/20 p-8 shadow-lg ring-1 ring-black/5 duration-200 ease-in-out hover:rotate-1 hover:scale-105 sm:p-4"
			>
				<div class="mb-5 flex items-center gap-3">
					<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
						<Cpu />
					</div>
					<h2>Easy monitoring</h2>
				</div>
				<div class="select-text text-left">
					<p class="text-base leading-relaxed">Monitor CPU/RAM/GPU usage like clock speed, voltage, memory usage</p>
				</div>
			</div>
		</div>

		<div class="flex items-center justify-center gap-10">
			<div
				class="relative -top-5 isolate aspect-video w-96 transform rounded-xl bg-white/20 p-8 shadow-lg ring-1 ring-black/5 duration-200 ease-in-out hover:rotate-1 hover:scale-105 sm:p-4"
			>
				<div class="mb-5 flex items-center gap-3">
					<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
						<Cpu />
					</div>
					<h2>Easy monitoring</h2>
				</div>
				<div class="select-text text-left">
					<p class="text-base leading-relaxed">Monitor CPU/RAM/GPU usage like clock speed, voltage, memory usage</p>
				</div>
			</div>

			<div
				class="relative -top-5 isolate aspect-video w-96 transform rounded-xl bg-white/20 p-8 shadow-lg ring-1 ring-black/5 duration-200 ease-in-out hover:rotate-1 hover:scale-105 sm:p-4"
			>
				<div class="mb-5 flex items-center gap-3">
					<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
						<Cpu />
					</div>
					<h2>Easy monitoring</h2>
				</div>
				<div class="select-text text-left">
					<p class="text-base leading-relaxed">Monitor CPU/RAM/GPU usage like clock speed, voltage, memory usage</p>
				</div>
			</div>

			<div
				class="relative -top-5 isolate aspect-video w-96 transform rounded-xl bg-white/20 p-8 shadow-lg ring-1 ring-black/5 duration-200 ease-in-out hover:rotate-1 hover:scale-105 sm:p-4"
			>
				<div class="mb-5 flex items-center gap-3">
					<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
						<Cpu />
					</div>
					<h2>Easy monitoring</h2>
				</div>
				<div class="select-text text-left">
					<p class="text-base leading-relaxed">Monitor CPU/RAM/GPU usage like clock speed, voltage, memory usage</p>
				</div>
			</div>
		</div>
	</div> -->
</div>

<Footer />

<script lang="ts">
	import { BarChart3, Check, Cpu, Info, Network, Play, Radio } from "lucide-svelte"
	import { BrowserChrome, GpuCard, Memory, Microsoft, PcDisplay, Ubuntu, Apple } from "svelte-bootstrap-icons"
	import Header from "../components/header.svelte"
	import Footer from "../components/footer.svelte"
	import { onMount, onDestroy } from "svelte"
	import { slide } from "svelte/transition"

	let greetings = ["Windows PC", "Windows PC", "Linux PC", "MacBook Pro", "MacBook Air", "Home Server", "Gamer PC", "Laptop"]
	let index = 0
	let roller: NodeJS.Timer

	onMount(() => {
		roller = setInterval(() => {
			if (index === greetings.length - 1) index = 0
			else index++
		}, 5000)

		if (index === greetings.length - 1) index = 0
		else index++
	})

	onDestroy(() => {
		clearInterval(roller)
	})
</script>
