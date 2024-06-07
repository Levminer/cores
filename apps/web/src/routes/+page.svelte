<Header />

<div class="mx-auto w-11/12 rounded-xl sm:w-full">
	<div class="mb-60 mt-10 flex flex-col items-center justify-center text-center">
		<h1 class="mt-10 text-4xl font-medium md:text-6xl">Modern hardware monitor for your</h1>

		{#key index}
			<h2
				class="bg-gradient-to-r from-purple-400 to-pink-600 bg-clip-text text-4xl font-medium italic text-transparent md:text-6xl"
				transition:slide
			>
				{greetings[index]}
			</h2>
		{/key}

		<h3 class="mt-5">
			Unleash the power of real-time PC monitoring from anywhere. <br /> Access detailed information about your computer anytime, anywhere.
		</h3>

		<div class="my-5 flex flex-row items-center justify-center gap-5">
			<a
				href="#download"
				class="bg-cores-current hover:text-cores-current transform rounded-2xl px-5 py-4 text-xl font-semibold duration-200 ease-in-out hover:translate-y-0.5 hover:bg-white hover:from-transparent hover:to-transparent"
				>Get Cores</a
			>
			<a class="flex text-lg font-semibold duration-200 ease-in-out hover:text-gray-200" href="#features">More info <ArrowRight /></a>
		</div>

		<div class="flex w-full flex-row flex-wrap items-center justify-center">
			<img width="850" height="474" src="https://cdn.levminer.com/cores/laptop.webp" alt="Cores on desktop computer" />
			<img width="250" height="483" src="https://cdn.levminer.com/cores/phone.webp" alt="Cores on on a phone" />
		</div>
	</div>

	<div class="mb-20 space-y-20">
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
				<div class="relative flex w-full items-center justify-center overflow-hidden rounded-xl bg-black p-3">
					<video id="video0" muted loop src="https://cdn.levminer.com/cores/home.webm" />
					<div class="absolute flex items-center justify-center">
						<Play id="play0" fill="white" class="h-20 w-20 rounded-full bg-black/80 p-5 backdrop-blur-sm" />
					</div>
				</div>
			</div>
		</div>

		<div id="features2" class="flex justify-center rounded-xl bg-gradient-to-r from-teal-200 to-teal-500 py-20">
			<div class="flex select-text flex-wrap items-center justify-between gap-10 px-20 sm:w-full sm:px-2 md:flex-nowrap">
				<div class="relative flex w-full items-center justify-center overflow-hidden rounded-xl bg-black p-3">
					<video id="video1" muted loop src="https://cdn.levminer.com/cores/charts.webm" />
					<div class="absolute flex items-center justify-center">
						<Play id="play1" fill="white" class="h-20 w-20 rounded-full bg-black/80 p-5 backdrop-blur-sm" />
					</div>
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
								<h2 class="bg-gradient-to-r bg-clip-text font-extrabold text-transparent">Detailed Information</h2>
							</div>
							<div class="select-text">
								<h3>Detailed information about each component in its own page.</h3>
							</div>
						</div>

						<!-- card 2 -->
						<div class="w-full rounded-xl bg-gray-900 p-8 sm:p-4">
							<div class="mb-5 flex items-center gap-3">
								<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg bg-gradient-to-r p-3 sm:p-2">
									<BarChart3 />
								</div>
								<h2 class="bg-gradient-to-r bg-clip-text font-extrabold text-transparent">Useful Charts</h2>
							</div>
							<div class="select-text">
								<h3>Track your components status with informative charts.</h3>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>

		<div id="features3" class="flex justify-center rounded-xl bg-gradient-to-r from-indigo-400 to-cyan-400 py-20">
			<div class="flex select-text flex-col flex-wrap items-center justify-between gap-10 px-20 sm:w-full sm:px-2 md:flex-nowrap">
				<div class="relative flex w-[800px] items-center justify-center overflow-hidden rounded-xl bg-black p-3 sm:w-full">
					<video id="video2" muted loop src="https://cdn.levminer.com/cores/remote.webm" />
					<div class="absolute flex items-center justify-center">
						<Play id="play2" fill="white" class="h-20 w-20 rounded-full bg-black/80 p-5 backdrop-blur-sm" />
					</div>
				</div>
				<div class="flex w-[800px] flex-col sm:w-full">
					<!-- card 0 -->
					<div class="mx-auto flex w-full flex-col items-start justify-center rounded-xl bg-gray-900 p-10 text-left">
						<h2 class="mb-5 bg-gradient-to-r bg-clip-text text-4xl font-extrabold text-transparent">Remote access</h2>
						<h3>Monitor your device easily from anywhere.</h3>
					</div>

					<div class="mt-5 flex flex-row justify-between gap-5 sm:flex-col">
						<!-- card 1 -->
						<div class="w-full rounded-xl bg-gray-900 p-8 sm:p-4">
							<div class="mb-5 flex items-center gap-3">
								<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg bg-gradient-to-r p-3 sm:p-2">
									<Radio />
								</div>
								<h2 class="bg-gradient-to-r bg-clip-text font-extrabold text-transparent">Monitor from anywhere</h2>
							</div>
							<div class="select-text">
								<h3>You can monitor your system from any device with a web browser.</h3>
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
								<h3>You can connect directly to multiple host systems. No cloud, no central server.</h3>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>

		<div id="features4" class="flex flex-col items-center justify-center rounded-xl py-20">
			<h1 class="mb-10 text-5xl font-bold sm:text-3xl">Packed with features</h1>
			<div class="flex max-w-2xl flex-wrap items-center justify-center gap-1">
				{#each features as item}
					<div class="transparent-900 rounded-xl border border-gray-500 p-3 text-lg shadow-xl backdrop-blur-xl">
						<p class="text-[#d3cfcf]">{item}</p>
					</div>
				{/each}
			</div>
		</div>

		<div id="download" class="flex justify-center rounded-xl bg-gradient-to-r from-red-200 to-yellow-200 py-20">
			<div class="flex select-text flex-wrap items-center justify-between gap-10 px-20 sm:w-full sm:px-2 md:flex-nowrap">
				<div class="flex w-full flex-col">
					<!-- card 0 -->
					<div class="mx-auto flex w-full flex-col items-start justify-center rounded-xl bg-gray-900 p-10 text-left">
						<h2 class="mb-5 bg-gradient-to-r bg-clip-text text-4xl font-extrabold text-transparent">Downloads</h2>
						<h3>Download the latest version of Cores now and gain real-time insights into your computer's performance.</h3>
					</div>

					<div class="mt-5 flex w-full flex-col justify-between gap-5 sm:flex-col">
						<!-- windows installer -->
						<div class="w-full space-y-8 rounded-xl bg-gray-900 p-8 sm:p-4">
							<div class="flex flex-col justify-between gap-10 md:flex-row">
								<div class="flex flex-col">
									<div class="flex flex-row items-center gap-3">
										<Microsoft width="24" height="24" />
										<h2>Windows Installer</h2>
									</div>
									<h3>Download the installer or get the app from the Microsoft Store.</h3>
								</div>
								<div class="flex flex-col gap-3 md:flex-row">
									<a href="https://link.levminer.com/cores-installer" target="_blank" class="smallButton">
										<Download width="24" height="24" />
										Download
									</a>
									<a href="https://link.levminer.com/cores-ms-store" target="_blank" class="smallButton">
										<Microsoft width="24" height="24" />
										Microsoft Store
									</a>
								</div>
							</div>
							<div class="border-t-2 pt-6">
								<h3 class="text-lg">System requirements: Windows 10 or 11, 64bit processor</h3>
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
										<p title="Coming soon...">Linux</p>
									</div>
									<div class="px-3 py-3">
										<p title="Coming soon...">macOS</p>
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
									<h2 class="text-[3rem] font-semibold">$14<sup>99</sup></h2>

									<div class="text-center">
										<button
											class="button hover:border-cores-current hover:text-cores-current mt-5 w-full hover:translate-y-0.5 hover:animate-pulse"
										>
											Coming soon
										</button>
										<div class="mt-3">
											<a rel="noreferrer" href="#download" class="underline duration-200 hover:text-gray-200"
												>Get started for free</a
											>
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
							<div class="mt-4 flex flex-wrap justify-center gap-2 text-center text-lg">
								<p>Want to use Cores as a business?</p>
								<a class="underline" href="mailto:cores@levminer.com">Contact</a>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>

	<div class="mb-44">
		<GetStarted />
	</div>
</div>

<Footer />

<script lang="ts">
	import { ArrowRight, BarChart3, Check, Cpu, Info, Network, Play, Radio, Download } from "lucide-svelte"
	import { onDestroy, onMount } from "svelte"
	import { GpuCard, Memory, PcDisplay, Microsoft } from "svelte-bootstrap-icons"
	import { slide } from "svelte/transition"
	import Footer from "../components/footer.svelte"
	import Header from "../components/header.svelte"
	import GetStarted from "../components/getStarted.svelte"

	// prettier-ignore
	const features=["CPU Temperature","CPU Clock Speed","CPU Load","CPU Voltage","CPU Power Usage","RAM Usage","RAM Speed","Virtual RAM Usage","GPU Load","GPU Temperature","GPU Power Usage","GPU Clock Speed","GPU Fan Speed","GPU Driver Date","Network Speed","Network Usage","Drive Temperature","Drive Health","Drive Usage","BIOS Date", "Fan Speed","Remote Access","Runs In The Background", "Minimal Resource Usage"]
	let greetings = ["Windows PC", "Windows PC", "Linux PC", "MacBook Pro", "MacBook Air", "Home Server", "Gamer PC", "Laptop"]
	let index = 0
	let roller: NodeJS.Timeout

	onMount(() => {
		// smooth scroll
		document.querySelector("#layout")?.classList.add("scroll-smooth")

		// hero text animation
		roller = setInterval(() => {
			if (index === greetings.length - 1) index = 0
			else index++
		}, 5000)

		if (index === greetings.length - 1) index = 0
		else index++

		// play videos on hover
		for (let i = 0; i < 3; i++) {
			const video = document.getElementById(`video${i}`) as HTMLVideoElement
			const play = document.getElementById(`play${i}`) as HTMLElement

			video.addEventListener("mouseenter", () => {
				video.play()
				play.style.display = "none"
			})

			video.addEventListener("mouseleave", () => {
				video.pause()
				play.style.display = "flex"
			})

			play.addEventListener("click", () => {
				video.play()
				play.style.display = "none"
			})
		}
	})

	onDestroy(() => {
		clearInterval(roller)

		document.querySelector("#layout")?.classList.remove("scroll-smooth")

		for (let i = 0; i < 3; i++) {
			const video = document.getElementById(`video${i}`) as HTMLVideoElement
			const play = document.getElementById(`play${i}`) as HTMLElement

			video.removeEventListener("mouseenter", () => {})
			video.removeEventListener("mouseleave", () => {})
			play.removeEventListener("click", () => {})
		}
	})
</script>
