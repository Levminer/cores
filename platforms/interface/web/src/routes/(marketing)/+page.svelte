<Header />

<div class="mx-auto w-full rounded-xl">
	<div class="headerBg mb-60 flex flex-col items-center justify-center py-20 text-center">
		<h1 class="mb-3 mt-10 text-4xl font-medium md:text-6xl">Modern hardware monitor for your</h1>

		<div class="flex flex-row flex-wrap items-center justify-center gap-1">
			<div class="rounded-xl bg-white px-3 py-2">
				<h2 class="bg-gradient-to-r from-purple-400 to-pink-600 bg-clip-text text-2xl font-medium italic text-transparent md:text-4xl">
					Windows PC
				</h2>
			</div>
			<div class="rounded-xl bg-white px-3 py-2">
				<h2 class="bg-gradient-to-r from-purple-400 to-pink-600 bg-clip-text text-2xl font-medium italic text-transparent md:text-4xl">
					Linux Server
				</h2>
			</div>
			<div class="rounded-xl bg-white px-3 py-2">
				<h2 class="bg-gradient-to-r from-purple-400 to-pink-600 bg-clip-text text-2xl font-medium italic text-transparent md:text-4xl">
					Mac Mini
				</h2>
			</div>
		</div>

		<h3 class="mt-5">
			Monitor your devices in real-time. <br /> Access detailed information about your computers anytime, anywhere.
		</h3>

		<div class="mb-10 mt-5 flex flex-row items-center justify-center gap-5">
			<a
				href="#downloads"
				class="bg-cores-alternative hover:text-cores-alternative flex transform flex-row items-center gap-1 rounded-2xl px-5 py-4 text-xl font-semibold duration-200 ease-in-out hover:translate-y-0.5 hover:bg-white hover:from-transparent hover:to-transparent"
			>
				<ArrowDownToLine class="hidden md:block" />
				Download Cores
			</a>
			<a class="flex gap-1 text-lg font-semibold duration-200 ease-in-out hover:text-gray-200" href="#features"
				>Features <ArrowRight class="relative top-1" /></a
			>
		</div>

		<div class="flex w-full flex-row flex-wrap items-center justify-center">
			<img width="850" height="474" src="https://cdn.levminer.com/cores/laptop2.png" alt="Cores on desktop computer" />
			<img width="320" height="480" src="https://cdn.levminer.com/cores/phone2.webp" alt="Cores on on a phone" />
		</div>
	</div>

	<div class="mx-auto mb-20 w-11/12 space-y-20 sm:w-full">
		<div id="features" class="flex justify-center rounded-xl bg-gradient-to-r from-indigo-500 to-cyan-500 py-20">
			<div class="flex select-text flex-col flex-wrap items-center justify-between gap-10 px-20 sm:w-full sm:px-2 md:flex-nowrap">
				<div class="flex flex-col sm:w-full">
					<div class="mb-10 text-center">
						<h2 class="mb-5 text-balance bg-gradient-to-r bg-clip-text text-center text-4xl font-extrabold text-white md:text-6xl">
							Modern Hardware Monitoring
						</h2>
						<h3 class="text-2xl italic text-gray-100">Monitor any system with ease.</h3>
					</div>

					<div class="flex flex-col items-center justify-center gap-5 md:flex-row">
						<div>
							<div class="mt-5 flex flex-row justify-between gap-3 sm:flex-col">
								<!-- cards 1 -->
								<div class="w-full rounded-xl bg-gray-900 p-8 sm:p-4">
									<div class="mb-5 flex items-center gap-3">
										<div
											class="transparent-900 flex aspect-square items-center justify-center rounded-lg bg-gradient-to-r p-3 sm:p-2"
										>
											<Feather />
										</div>
										<h4 class="bg-gradient-to-r bg-clip-text font-extrabold text-transparent">Lightweight</h4>
									</div>
									<div class="select-text">
										<h5>Cores runs as a background service with minimal resource usage.</h5>
									</div>
								</div>
								<div class="w-full rounded-xl bg-gray-900 p-8 sm:p-4">
									<div class="mb-5 flex items-center gap-3">
										<div
											class="transparent-900 flex aspect-square items-center justify-center rounded-lg bg-gradient-to-r p-3 sm:p-2"
										>
											<MonitorSmartphone />
										</div>
										<h4 class="bg-gradient-to-r bg-clip-text font-extrabold text-transparent">Cross-Platform</h4>
									</div>
									<div class="select-text">
										<h5>You can monitor your system on Windows, Linux and MacOS.</h5>
									</div>
								</div>
							</div>
							<div class="mt-5 flex flex-row justify-between gap-3 sm:flex-col">
								<!-- cards 2 -->
								<div class="w-full rounded-xl bg-gray-900 p-8 sm:p-4">
									<div class="mb-5 flex items-center gap-3">
										<div
											class="transparent-900 flex aspect-square items-center justify-center rounded-lg bg-gradient-to-r p-3 sm:p-2"
										>
											<Radio />
										</div>
										<h4 class="bg-gradient-to-r bg-clip-text font-extrabold text-transparent">Easy Remote Monitoring</h4>
									</div>
									<div class="select-text">
										<h5>You can monitor your system from any browser. No cloud, no central server powered by WebRTC.</h5>
									</div>
								</div>

								<div class="w-full rounded-xl bg-gray-900 p-8 sm:p-4">
									<div class="mb-5 flex items-center gap-3">
										<div
											class="transparent-900 flex aspect-square items-center justify-center rounded-lg bg-gradient-to-r p-3 sm:p-2"
										>
											<Webhook />
										</div>
										<h4 class="bg-gradient-to-r bg-clip-text font-extrabold text-transparent">API Access</h4>
									</div>
									<div class="select-text">
										<h5>You can access the REST or Websocket API to build your own integration.</h5>
									</div>
								</div>
							</div>
						</div>
						<div class="rounded-xl bg-black p-3">
							<img class="rounded-md" src="https://cdn.levminer.com/cores/dashboard.webp" alt="web dashboard" />
						</div>
					</div>
				</div>
			</div>
		</div>

		<div id="features2" class="flex min-h-screen justify-center rounded-xl bg-gradient-to-r from-red-500 to-orange-500 py-20">
			<div class="flex select-text flex-col flex-wrap items-center justify-between gap-10 px-20 sm:w-full sm:px-2 md:flex-nowrap">
				<div class="flex w-full flex-col items-center justify-center">
					<!-- card 0 -->
					<div class="mb-10 text-center">
						<h2 class="mb-5 text-balance bg-gradient-to-r bg-clip-text text-center text-4xl font-extrabold text-white md:text-6xl">
							Easy Remote Access
						</h2>
						<h3 class="text-2xl italic text-gray-100">Monitor your devices from anywhere.</h3>
					</div>

					<div class="relative flex w-[900px] items-center justify-center overflow-hidden rounded-xl bg-black p-3">
						<video id="video0" preload="metadata" muted loop src="https://cdn.levminer.com/cores/remote2.mp4" />
						<div class="absolute flex items-center justify-center">
							<Play id="play0" fill="white" class="m-10 h-20 w-20 rounded-full bg-black/80 p-5 backdrop-blur-sm" />
						</div>
					</div>
				</div>

				<div class="flex w-[1000px] flex-col sm:w-full">
					<div class="mt-5 flex flex-row justify-between gap-5 sm:flex-col">
						<!-- card 1 -->
						<div class="w-full rounded-xl bg-gray-900 p-8 sm:p-4">
							<div class="mb-5 flex items-center gap-3">
								<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg bg-gradient-to-r p-3 sm:p-2">
									<Server />
								</div>
								<h4 class="bg-gradient-to-r bg-clip-text font-extrabold text-transparent">Sync With Your Devices</h4>
							</div>
							<div class="select-text">
								<h5>You can monitor all your devices easily.</h5>
							</div>
						</div>

						<div class="w-full rounded-xl bg-gray-900 p-8 sm:p-4">
							<div class="mb-5 flex items-center gap-3">
								<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg bg-gradient-to-r p-3 sm:p-2">
									<Moon />
								</div>
								<h4 class="bg-gradient-to-r bg-clip-text font-extrabold text-transparent">Left In A Hurry?</h4>
							</div>
							<div class="select-text">
								<h5>You can shut down or put your computer to sleep remotely.</h5>
							</div>
						</div>

						<div class="w-full rounded-xl bg-gray-900 p-8 sm:p-4">
							<div class="mb-5 flex items-center gap-3">
								<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg bg-gradient-to-r p-3 sm:p-2">
									<Power />
								</div>
								<h4 class="bg-gradient-to-r bg-clip-text font-extrabold text-transparent">Remote Start</h4>
							</div>
							<div class="select-text">
								<h5>If you have other devices on your network, you can start them remotely with Wake On LAN.</h5>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>

		<div id="features4" class="flex flex-col items-center justify-center rounded-xl py-20">
			<div class="mb-10 text-center">
				<h2 class="mb-5 bg-gradient-to-r bg-clip-text text-center text-4xl font-extrabold text-white md:text-8xl">Packed with features</h2>
				<h3 class="italic text-gray-100">Every feature you need to monitor your computer.</h3>
			</div>
			<div class="flex max-w-2xl flex-wrap items-center justify-center gap-1">
				{#each features as item}
					<div
						class="transparent-900 hover:border-cores-current transform rounded-xl border border-gray-500 p-3 text-lg shadow-xl backdrop-blur-xl duration-150 ease-in-out hover:scale-95"
					>
						<p class="text-[#d3cfcf]">{item}</p>
					</div>
				{/each}
			</div>
		</div>

		<div id="downloads" class="flex min-h-screen justify-center rounded-xl py-20">
			<div class="flex w-[50%] select-text flex-wrap items-center justify-between sm:w-full sm:px-2 md:flex-nowrap">
				<div class="flex w-full flex-col items-center justify-center">
					<!-- card 0 -->
					<div class="mb-10 text-center">
						<h2 class="mb-5 bg-gradient-to-r bg-clip-text text-center text-4xl font-extrabold text-white md:text-8xl">Download</h2>
						<h3 class="italic text-gray-100">Download the latest version of Cores now to get started.</h3>
					</div>

					<div class="flex w-full flex-row items-center justify-center gap-5 sm:flex-col">
						<!-- windows installer -->
						<button
							on:click={() => {
								changeDownload("windows")
							}}
							class={selectedDownload === "windows" ? "selectedDownloadButton" : "downloadButton"}
						>
							<Microsoft class="h-12 w-12 text-current" />
							<h2 class="text-current">Windows</h2>
						</button>

						<button
							on:click={() => {
								changeDownload("linux")
							}}
							class={selectedDownload === "linux" ? "selectedDownloadButton" : "downloadButton"}
						>
							<Ubuntu class="h-12 w-12 text-current" />
							<h2 class="text-current">Linux</h2>
						</button>

						<button
							on:click={() => {
								changeDownload("macos")
							}}
							class={selectedDownload === "macos" ? "selectedDownloadButton" : "downloadButton"}
						>
							<Apple class="h-12 w-12 text-current" />
							<h2 class="text-current">macOS</h2>
						</button>
					</div>

					<div class="transparent-900 mt-10 w-[85%] space-y-8 rounded-xl border border-gray-500 p-8 text-center shadow-xl sm:p-4">
						{#if selectedDownload === "windows"}
							<h3>Download the installer or get the app from the Microsoft Store.</h3>
							<div class="flex flex-row flex-wrap items-center justify-center gap-3">
								<a href="https://link.levminer.com/cores-installer" target="_blank" class="smallButton">
									<Download width="24" height="24" />
									Download
								</a>
								<a href="https://link.levminer.com/cores-ms-store" target="_blank" class="smallButton">
									<Microsoft width="24" height="24" />
									Microsoft Store
								</a>
							</div>
							<div class="border-t-2 pt-6">
								<h3 class="text-lg">System requirements: Windows 10 or 11, 64bit processor</h3>
							</div>
						{:else if selectedDownload === "linux"}
							<h3>Download the package for your distro or get the daemon.</h3>
							<div class="flex flex-row items-center justify-center gap-3">
								<a href="https://github.com/levminer/cores/releases/latest" target="_blank" class="smallButton">
									<Download width="24" height="24" />
									Download
								</a>
							</div>
							<div class="border-t-2 pt-6">
								<h3 class="text-lg">System requirements: Linux, 64bit or arm64 processor</h3>
							</div>
						{:else}
							<h3>Download and install the app with the installer.</h3>
							<div class="flex flex-row items-center justify-center gap-3">
								<a href="https://github.com/levminer/cores/releases/latest" target="_blank" class="smallButton">
									<Download width="24" height="24" />
									Download
								</a>
							</div>
							<div class="border-t-2 pt-6">
								<h3 class="text-lg">System requirements: macOS Big Sur, arm64 (Apple Silicon) processor</h3>
							</div>
						{/if}
					</div>
				</div>
			</div>
		</div>

		<div id="pricing" class="flex min-h-screen justify-center rounded-xl bg-gradient-to-r from-fuchsia-600 to-pink-600 py-20">
			<div class="flex select-text flex-wrap items-center justify-between px-20 sm:w-full sm:px-2 md:flex-nowrap">
				<div class="flex w-full flex-col">
					<div class="mb-10">
						<h2 class="mb-5 bg-gradient-to-r bg-clip-text text-center text-4xl font-extrabold text-white md:text-8xl">Pricing</h2>
						<h3 class="text-center italic text-gray-100">Do you hate subscriptions too? Buy Cores now and get life time access.</h3>
					</div>

					<div class="mt-5 flex flex-row justify-between gap-5 rounded-xl bg-gray-900 sm:flex-col">
						<div class="flex w-full flex-col gap-3 rounded-xl p-8 sm:p-4">
							<div
								class="transparent-900 border-cores-alternative flex w-full transform flex-col gap-1 rounded-xl border-2 px-5 py-5 text-xl font-semibold shadow-md duration-100 md:flex-row"
							>
								<div class="flex w-full flex-col">
									<div class="mb-3 text-left">
										<h2 class="bg-gradient-to-r from-purple-400 to-pink-600 bg-clip-text font-extrabold text-transparent">
											Cores Pro
										</h2>
									</div>
									<div>
										<div class="flex flex-row items-center gap-1 text-left">
											<Check class="h-5 w-5 text-green-500" />
											<h5>For personal use, one-time purchase, no subscriptions</h5>
										</div>
										<div class="flex flex-row items-center gap-1 text-left">
											<Check class="h-5 w-5 text-green-500" />
											<h5>Monitor up to 5 devices</h5>
										</div>
										<div class="flex flex-row items-center gap-1 text-left">
											<Check class="h-5 w-5 text-green-500" />
											<h5>Access any device remotely</h5>
										</div>
										<div class="flex flex-row items-center gap-1 text-left">
											<Check class="h-5 w-5 text-green-500" />
											<h5>Export data as .csv and image</h5>
										</div>
									</div>
								</div>
								<div class="flex flex-col items-end">
									<div class="flex w-full justify-center md:justify-end">
										<h2 class="text-center text-3xl font-semibold">
											$9.99 <p class="text-xs text-gray-200">One time purchase</p>
										</h2>
									</div>

									<a
										href="https://link.levminer.com/buy-cores-app?utm_source=app"
										class="button bg-cores-alternative hover:text-cores-alternative border-cores-alternative mt-5 w-full gap-2 font-bold text-white hover:translate-y-0.5 hover:animate-pulse"
									>
										<ShoppingCart />
										Buy
									</a>
								</div>
							</div>

							<div
								class="transparent-900 border-cores-alternative flex w-full transform flex-col gap-1 rounded-xl border-2 px-5 py-5 text-xl font-semibold shadow-md duration-100 md:flex-row"
							>
								<div class="flex w-full flex-col">
									<div class="mb-3 text-left">
										<h2 class="bg-gradient-to-r from-rose-600 to-violet-400 bg-clip-text font-extrabold text-transparent">
											Cores Advanced
										</h2>
									</div>
									<div>
										<div class="flex flex-row items-center gap-1 text-left">
											<Check class="h-5 w-5 text-green-500" />
											<h5>For personal and business use, monthly subscription, cancel anytime</h5>
										</div>
										<div class="flex flex-row items-center gap-1 text-left">
											<Check class="h-5 w-5 text-green-500" />
											<h5>Choose how many devices you want to monitor</h5>
										</div>
										<div class="flex flex-row items-center gap-1 text-left">
											<Check class="h-5 w-5 text-green-500" />
											<h5>Access any device remotely</h5>
										</div>
										<div class="flex flex-row items-center gap-1 text-left">
											<Check class="h-5 w-5 text-green-500" />
											<h5>Export data as .csv and image</h5>
										</div>
									</div>
								</div>
								<div class="flex flex-col items-end">
									<div class="flex w-full justify-center md:justify-end">
										<h2 class="text-center text-3xl font-semibold">
											$2.99 <p class="text-xs text-gray-200">/month/device</p>
										</h2>
									</div>
									<a
										href="mailto:support@coresmonitor.com"
										class="button bg-cores-alternative hover:text-cores-alternative border-cores-alternative mt-5 w-full gap-2 font-bold text-white hover:translate-y-0.5 hover:animate-pulse"
									>
										<Mail />
										Get a quote
									</a>
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>

		<div id="newsletter" class="flex flex-col items-center justify-center rounded-xl bg-gradient-to-r from-gray-700 to-black py-44">
			<div class="mb-10 text-center">
				<h2 class="mb-5 bg-gradient-to-r bg-clip-text text-center text-4xl font-extrabold text-white md:text-8xl">Interested in Cores?</h2>
				<h3 class="italic text-gray-100">Subscribe to the newsletter and get the latest updates.</h3>
			</div>

			<form class="flex w-1/4 flex-col space-y-4 sm:w-[60%]" action="https://cores.lemonsqueezy.com/email-subscribe/external" method="post">
				<div>
					<label for="email">Email</label><br />
					<input class="input w-full" type="email" name="email" id="email" required />
				</div>

				<button class="button w-full" type="submit">Subscribe</button>
			</form>
		</div>
	</div>

	<div class="mb-44">
		<GetStarted />
	</div>
</div>

<Footer />

<script lang="ts">
	import {
		ArrowRight,
		Check,
		Gauge,
		Network,
		Play,
		Radio,
		Download,
		Power,
		Moon,
		MonitorSmartphone,
		ShoppingCart,
		Mail,
		Feather,
		Webhook,
		Terminal,
		Server,
		ArrowDownToLine,
	} from "lucide-svelte"
	import { onDestroy, onMount } from "svelte"
	import { Microsoft, Ubuntu, Apple } from "svelte-bootstrap-icons"
	import Footer from "../../components/Footer.svelte"
	import Header from "../../components/Header.svelte"
	import GetStarted from "../../components/GetStarted.svelte"

	// prettier-ignore
	let selectedDownload = "windows"
	const features = [
		"CPU Load",
		"CPU Temperature",
		"CPU Clock Speed",
		"CPU Voltage",
		"CPU Power Usage",
		"RAM Usage",
		"RAM Speed",
		"Virtual RAM Usage",
		"GPU Load",
		"GPU Temperature",
		"GPU Power Usage",
		"GPU Clock Speed",
		"GPU Fan Speed",
		"GPU Driver Date",
		"Network Interface Speed",
		"Network Upload/Download Speed",
		"Network Data Usage",
		"Drive Temperature",
		"Drive Health",
		"Drive Read/Write Speed",
		"BIOS Date",
		"Fan Speed",
		"Battery info",
		"Remote Access",
		"Runs In The Background",
		"Minimal Resource Usage",
	]

	const changeDownload = (value: string) => {
		selectedDownload = value
	}

	onMount(() => {
		// smooth scroll
		document.querySelector("#layout")?.classList.add("scroll-smooth")

		// play videos on hover
		for (let i = 0; i < 1; i++) {
			const video = document.getElementById(`video${i}`) as HTMLVideoElement
			const play = document.getElementById(`play${i}`) as HTMLElement

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
		document.querySelector("#layout")?.classList.remove("scroll-smooth")

		for (let i = 0; i < 1; i++) {
			const video = document.getElementById(`video${i}`) as HTMLVideoElement
			const play = document.getElementById(`play${i}`) as HTMLElement

			play.removeEventListener("click", () => {})
			video.removeEventListener("mouseleave", () => {})
		}
	})
</script>
