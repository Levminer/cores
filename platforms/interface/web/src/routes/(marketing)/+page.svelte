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
					MacBook Pro
				</h2>
			</div>
		</div>

		<h3 class="mt-5">
			Monitor your devices in real-time. <br /> Access detailed information about your computers anytime, anywhere.
		</h3>

		<div class="mb-10 mt-5 flex flex-row items-center justify-center gap-5">
			<a
				href="#downloads"
				class="bg-cores-alternative hover:text-cores-alternative transform rounded-2xl px-5 py-4 text-xl font-semibold duration-200 ease-in-out hover:translate-y-0.5 hover:bg-white hover:from-transparent hover:to-transparent"
				>Download Cores</a
			>
			<a class="flex text-lg font-semibold duration-200 ease-in-out hover:text-gray-200" href="#features">More info <ArrowRight /></a>
		</div>

		<div class="flex w-full flex-row flex-wrap items-center justify-center">
			<img width="850" height="474" src="https://cdn.levminer.com/cores/laptop.webp" alt="Cores on desktop computer" />
			<img width="250" height="483" src="https://cdn.levminer.com/cores/phone.webp" alt="Cores on on a phone" />
		</div>
	</div>

	<div class="mx-auto mb-20 w-11/12 space-y-20 sm:w-full">
		<div id="features" class="flex min-h-screen justify-center rounded-xl bg-gradient-to-r from-indigo-500 to-cyan-500 py-20">
			<div class="flex select-text flex-col flex-wrap items-center justify-between gap-10 px-20 sm:w-full sm:px-2 md:flex-nowrap">
				<div class="mb-10 text-center">
					<h2 class="mb-5 bg-gradient-to-r bg-clip-text text-center text-4xl font-extrabold text-white md:text-8xl">Easy Remote Access</h2>
					<h3 class="text-2xl italic text-gray-100">Monitor your devices from anywhere.</h3>
				</div>
				<div class="relative flex w-[900px] items-center justify-center overflow-hidden rounded-xl bg-black p-3 sm:w-full">
					<video id="video1" muted loop src="https://cdn.levminer.com/cores/remote.webm" />
					<div class="absolute flex items-center justify-center">
						<Play id="play1" fill="white" class="h-20 w-20 rounded-full bg-black/80 p-5 backdrop-blur-sm" />
					</div>
				</div>
				<div class="flex w-[900px] flex-col sm:w-full">
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
								<h3>You can connect directly to multiple computers. No cloud, no central server powered by WebRTC.</h3>
							</div>
						</div>
					</div>

					<div class="mt-5 flex flex-row justify-between gap-5 sm:flex-col">
						<!-- card 1 -->
						<div class="w-full rounded-xl bg-gray-900 p-8 sm:p-4">
							<div class="mb-5 flex items-center gap-3">
								<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg bg-gradient-to-r p-3 sm:p-2">
									<Moon />
								</div>
								<h2 class="bg-gradient-to-r bg-clip-text font-extrabold text-transparent">Left in a hurry?</h2>
							</div>
							<div class="select-text">
								<h3>You can shut down or put your computer to sleep remotely.</h3>
							</div>
						</div>

						<!-- card 2 -->
						<div class="w-full rounded-xl bg-gray-900 p-8 sm:p-4">
							<div class="mb-5 flex items-center gap-3">
								<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg bg-gradient-to-r p-3 sm:p-2">
									<Power />
								</div>
								<h2 class="bg-gradient-to-r bg-clip-text font-extrabold text-transparent">Remote start</h2>
							</div>
							<div class="select-text">
								<h3>If you have other devices on your network, you can start them remotely with Wake On LAN.</h3>
							</div>
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
						<h2 class="mb-5 text-balance bg-gradient-to-r bg-clip-text text-center text-4xl font-extrabold text-white md:text-8xl">
							Monitor every component
						</h2>
						<h3 class="text-2xl italic text-gray-100">Track every component with detailed charts and visualizations.</h3>
					</div>

					<div class="relative flex w-[80%] items-center justify-center overflow-hidden rounded-xl bg-black p-3">
						<video id="video0" muted loop src="https://cdn.levminer.com/cores/home.webm" />
						<div class="absolute flex items-center justify-center">
							<Play id="play0" fill="white" class="h-20 w-20 rounded-full bg-black/80 p-5 backdrop-blur-sm" />
						</div>
					</div>
				</div>

				<div class="flex w-[900px] flex-col sm:w-full">
					<div class="mt-5 flex flex-row justify-between gap-5 sm:flex-col">
						<!-- card 1 -->
						<div class="w-full rounded-xl bg-gray-900 p-8 sm:p-4">
							<div class="mb-5 flex items-center gap-3">
								<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg bg-gradient-to-r p-3 sm:p-2">
									<MonitorSmartphone />
								</div>
								<h2 class="bg-gradient-to-r bg-clip-text font-extrabold text-transparent">Cross-platform</h2>
							</div>
							<div class="select-text">
								<h3>You can monitor your system on Windows, Linux and MacOS.</h3>
							</div>
						</div>

						<div class="w-full rounded-xl bg-gray-900 p-8 sm:p-4">
							<div class="mb-5 flex items-center gap-3">
								<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg bg-gradient-to-r p-3 sm:p-2">
									<Gauge />
								</div>
								<h2 class="bg-gradient-to-r bg-clip-text font-extrabold text-transparent">Light on your system</h2>
							</div>
							<div class="select-text">
								<h3>Cores runs as a background service with minimal resource usage.</h3>
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
			<div class="flex select-text flex-wrap items-center justify-between gap-10 px-20 sm:w-full sm:px-2 md:flex-nowrap">
				<div class="flex w-full flex-col">
					<!-- card 0 -->
					<div class="mb-10 text-center">
						<h2 class="mb-5 bg-gradient-to-r bg-clip-text text-center text-4xl font-extrabold text-white md:text-8xl">Download</h2>
						<h3 class="italic text-gray-100">Download the latest version of Cores now to get started.</h3>
					</div>

					<div class="flex w-full flex-row justify-between gap-5 sm:flex-col">
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

					<div class="transparent-900 mt-10 w-full space-y-8 rounded-xl border border-gray-500 p-8 text-center shadow-xl sm:p-4">
						{#if selectedDownload === "windows"}
							<h3>Download the installer or get the app from the Microsoft Store.</h3>
							<div class="flex flex-row items-center justify-center gap-3">
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
							<h3>Coming soon!</h3>
							<a href="#newsletter" class="underline">Subscribe to get notified</a>
						{/if}
					</div>
				</div>
			</div>
		</div>

		<div id="pricing" class="flex min-h-screen justify-center rounded-xl bg-gradient-to-r from-fuchsia-600 to-pink-600 py-20">
			<div class="flex select-text flex-wrap items-center justify-between gap-10 px-20 sm:w-full sm:px-2 md:flex-nowrap">
				<div class="flex w-full flex-col">
					<div class="mb-10">
						<h2 class="mb-5 bg-gradient-to-r bg-clip-text text-center text-4xl font-extrabold text-white md:text-8xl">Pricing</h2>
						<h3 class="text-center italic text-gray-100">Do you hate subscriptions too? Buy Cores now and get life time access.</h3>
					</div>

					<div class="mt-5 flex flex-row justify-between gap-5 sm:flex-col">
						<div class="w-full rounded-xl bg-gray-900 p-8 sm:p-4">
							<div class=" flex flex-row flex-wrap gap-3">
								<div class="mx-auto mt-5 flex max-w-lg flex-col gap-5 rounded-xl border-2 border-purple-400 p-5 sm:flex-col">
									<h1
										class="bg-gradient-to-r from-purple-400 to-pink-600 bg-clip-text px-5 text-center text-4xl font-extrabold text-transparent"
									>
										Cores for Personal
									</h1>
									<div class="px-5 text-center sm:w-full">
										<h2 class="text-[3rem] font-semibold">
											$9.99 <p class="text-sm text-gray-200">One time purchase</p>
										</h2>

										<div class="text-center">
											<a
												href="https://link.levminer.com/buy-cores-app"
												class="button bg-cores-alternative hover:text-cores-alternative border-cores-alternative mt-5 w-full font-bold text-white hover:translate-y-0.5 hover:animate-pulse"
											>
												Buy
											</a>
											<div class="mt-3">
												<a rel="noreferrer" href="#downloads" class="underline duration-200 hover:text-gray-200"
													>Get started for free</a
												>
											</div>
										</div>
									</div>
									<div
										class="flex flex-col justify-center space-y-2 border-t-2 border-gray-600 px-5 pt-3 text-left text-lg sm:w-full"
									>
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
											<p>Access your device remotely</p>
										</div>
										<div class="flex items-center space-x-2">
											<Check class="h-5 w-5 text-green-500" />
											<p>Use on up to 5 devices</p>
										</div>
									</div>
								</div>
								<div class="mx-auto mt-5 flex max-w-lg flex-col gap-5 rounded-xl border-2 border-purple-400 p-5 sm:flex-col">
									<h1
										class="bg-gradient-to-r from-purple-400 to-pink-600 bg-clip-text px-5 text-center text-4xl font-extrabold text-transparent"
									>
										Cores for Business
									</h1>
									<div class="px-5 text-center sm:w-full">
										<h2 class="text-[3rem] font-semibold">
											$4.99 <p class="text-sm text-gray-200">/month/device</p>
										</h2>

										<div class="text-center">
											<a
												href="mailto:cores@levminer.com"
												class="button bg-cores-alternative hover:text-cores-alternative border-cores-alternative mt-5 w-full font-bold text-white hover:translate-y-0.5 hover:animate-pulse"
											>
												Get a quote
											</a>
											<div class="mt-3">
												<a rel="noreferrer" href="#downloads" class="underline duration-200 hover:text-gray-200"
													>Get started for free</a
												>
											</div>
										</div>
									</div>
									<div
										class="flex flex-col justify-center space-y-2 border-t-2 border-gray-600 px-5 pt-3 text-left text-lg sm:w-full"
									>
										<div class="flex items-center space-x-2">
											<Check class="h-5 w-5 text-green-500" />
											<p>For business & commercial use</p>
										</div>
										<div class="flex items-center space-x-2">
											<Check class="h-5 w-5 text-green-500" />
											<p>Monthly subscription, cancel anytime</p>
										</div>
										<div class="flex items-center space-x-2">
											<Check class="h-5 w-5 text-green-500" />
											<p>Access any device remotely</p>
										</div>
										<div class="flex items-center space-x-2">
											<Check class="h-5 w-5 text-green-500" />
											<p>Volume discount</p>
										</div>
									</div>
								</div>
							</div>

							<div class="mt-4 flex flex-wrap justify-center gap-2 text-center text-lg">
								<p>Want to use Cores as a business or have a question?</p>
								<a class="underline" href="mailto:cores@levminer.com">Reach out</a>
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
	import { ArrowRight, Check, Gauge, Network, Play, Radio, Download, Power, Moon, MonitorSmartphone } from "lucide-svelte"
	import { onDestroy, onMount } from "svelte"
	import { Microsoft, Ubuntu, Apple } from "svelte-bootstrap-icons"
	import Footer from "../../components/footer.svelte"
	import Header from "../../components/header.svelte"
	import GetStarted from "../../components/getStarted.svelte"

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
		for (let i = 0; i < 2; i++) {
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
		document.querySelector("#layout")?.classList.remove("scroll-smooth")

		for (let i = 0; i < 2; i++) {
			const video = document.getElementById(`video${i}`) as HTMLVideoElement
			const play = document.getElementById(`play${i}`) as HTMLElement

			video.removeEventListener("mouseenter", () => {})
			video.removeEventListener("mouseleave", () => {})
			play.removeEventListener("click", () => {})
		}
	})
</script>
