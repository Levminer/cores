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
			<img width="280" height="440" src="https://cdn.levminer.com/cores/phone2.webp" alt="Cores on on a phone" />
		</div>
	</div>

	<div class="mx-auto mb-20 w-11/12 space-y-20 sm:w-full">
		<div id="features" class="flex justify-center rounded-xl bg-gradient-to-r from-indigo-500 to-cyan-500 py-20">
			<div class="flex select-text flex-col flex-wrap items-center justify-between gap-10 px-20 sm:w-full sm:px-2 md:flex-nowrap">
				<div class="mb-10 text-center">
					<h2 class="mb-5 text-balance bg-gradient-to-r bg-clip-text text-center text-4xl font-extrabold text-white md:text-6xl">
						Monitor any system with ease
					</h2>
				</div>

				<div class="flex flex-col items-center justify-center gap-5">
					<div class="flex w-full flex-col gap-5 md:flex-row">
						<div
							class="hover:border-cores-min w-full rounded-xl border-2 border-gray-700 bg-gray-900 p-8 duration-150 ease-in sm:w-full sm:p-4"
						>
							<div class="mb-5 flex items-center gap-3">
								<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg bg-gradient-to-r p-3 sm:p-2">
									<House />
								</div>
								<h4 class="bg-gradient-to-r bg-clip-text font-extrabold text-transparent">Informative home screen</h4>
							</div>
							<div class="select-text">
								<h5>Keep track of every component in one place.</h5>
							</div>
							<div class="rounded-xl bg-black p-3">
								<img class="rounded" width="500" src="https://cdn.levminer.com/cores/home.png" alt="web dashboard" />
							</div>
						</div>

						<div
							class="hover:border-cores-min w-full rounded-xl border-2 border-gray-700 bg-gray-900 p-8 duration-150 ease-in sm:w-full sm:p-4"
						>
							<div class="mb-5 flex items-center gap-3">
								<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg bg-gradient-to-r p-3 sm:p-2">
									<ChartLine />
								</div>
								<h4 class="bg-gradient-to-r bg-clip-text font-extrabold text-transparent">Detailed Statistics</h4>
							</div>
							<div class="select-text">
								<h5>See detailed statistics about every hardware component in your system.</h5>
							</div>
							<div class="rounded-xl bg-black p-3">
								<img
									class="rounded"
									width="500"
									src="https://pub-59a3ca658f4c4ece9990b6c50534538e.r2.dev/cores/stats.png"
									alt="web dashboard"
								/>
							</div>
						</div>
					</div>
					<div class="flex w-full flex-col gap-5 md:flex-row">
						<div
							class="hover:border-cores-min w-full rounded-xl border-2 border-gray-700 bg-gray-900 p-8 duration-150 ease-in sm:w-full sm:p-4"
						>
							<div class="mb-5 flex items-center gap-3">
								<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg bg-gradient-to-r p-3 sm:p-2">
									<Feather />
								</div>
								<h4 class="bg-gradient-to-r bg-clip-text font-extrabold text-transparent">Lightweight</h4>
							</div>
							<div class="select-text">
								<h5>Cores runs as a background service with minimal resource usage.</h5>
							</div>
						</div>

						<div
							class="hover:border-cores-min w-full rounded-xl border-2 border-gray-700 bg-gray-900 p-8 duration-150 ease-in sm:w-full sm:p-4"
						>
							<div class="mb-5 flex items-center gap-3">
								<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg bg-gradient-to-r p-3 sm:p-2">
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
			</div>
		</div>

		<div id="features2" class="flex justify-center rounded-xl bg-gradient-to-r from-red-500 to-orange-500 py-20">
			<div class="flex select-text flex-col flex-wrap items-center justify-between gap-10 px-20 sm:w-full sm:px-2 md:flex-nowrap">
				<div class="flex w-full flex-col items-center justify-center">
					<!-- card 0 -->
					<div class="mb-10 text-center">
						<h2 class="mb-5 text-balance bg-gradient-to-r bg-clip-text text-center text-4xl font-extrabold text-white md:text-6xl">
							Easy Remote Access
						</h2>
					</div>
				</div>

				<div class="flex flex-col items-center justify-center gap-5">
					<div class="flex w-full flex-col gap-5 md:flex-row">
						<div
							class="w-full rounded-xl border-2 border-gray-700 bg-gray-900 p-8 duration-150 ease-in hover:border-orange-400 sm:w-full sm:p-4"
						>
							<div class="mb-5 flex items-center gap-3">
								<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg bg-gradient-to-r p-3 sm:p-2">
									<Radio />
								</div>
								<h4 class="bg-gradient-to-r bg-clip-text font-extrabold text-transparent">Easy Remote Monitoring</h4>
							</div>
							<div class="select-text">
								<h5>You can monitor your system from any browser. No subscriptions.</h5>
							</div>
							<div class="rounded-xl bg-black p-3">
								<img class="rounded" width="500" src="https://cdn.levminer.com/cores/home2.png" alt="web dashboard" />
							</div>
						</div>

						<div
							class="w-full rounded-xl border-2 border-gray-700 bg-gray-900 p-8 duration-150 ease-in hover:border-orange-400 sm:w-full sm:p-4"
						>
							<div class="mb-5 flex items-center gap-3">
								<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg bg-gradient-to-r p-3 sm:p-2">
									<Server />
								</div>
								<h4 class="bg-gradient-to-r bg-clip-text font-extrabold text-transparent">All your devices in one place</h4>
							</div>
							<div class="select-text">
								<h5>You can monitor all your devices easily from a central place.</h5>
							</div>
							<div class="rounded-xl bg-black p-3">
								<img class="rounded" width="500" src="https://cdn.levminer.com/cores/devices.png" alt="web dashboard" />
							</div>
						</div>
					</div>
					<div class="flex w-full flex-col gap-5 md:flex-row">
						<div
							class="w-full rounded-xl border-2 border-gray-700 bg-gray-900 p-8 duration-150 ease-in hover:border-orange-400 sm:w-full sm:p-4"
						>
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

						<div
							class="w-full rounded-xl border-2 border-gray-700 bg-gray-900 p-8 duration-150 ease-in hover:border-orange-400 sm:w-full sm:p-4"
						>
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

		<div id="features5" class="flex flex-col items-center justify-center rounded-xl py-20">
			<div class="mb-10 text-center">
				<h2 class="mb-5 bg-gradient-to-r bg-clip-text text-center text-4xl font-extrabold text-white md:text-8xl">Use cases</h2>
			</div>
			<div class="flex max-w-6xl flex-col flex-wrap gap-10 text-center md:flex-row md:flex-nowrap">
				<div class="transparent-900 flex w-full flex-col items-center rounded-xl border border-gray-500 p-10 shadow-xl md:w-1/3">
					<div class="bg-cores-min flex h-28 w-28 items-center justify-center rounded-full text-6xl font-medium text-white">
						<Gamepad2 class="h-16 w-16" />
					</div>
					<h2 class="my-5">Gamers and power users</h2>
					<h3>Keep an eye on your system and monitor everything remotely.</h3>
				</div>

				<div class="transparent-900 flex w-full flex-col items-center rounded-xl border border-gray-500 p-10 shadow-xl md:w-1/3">
					<div class="bg-cores-current flex h-28 w-28 items-center justify-center rounded-full text-6xl font-medium text-white">
						<Server class="h-16 w-16" />
					</div>
					<h2 class="my-5">Homelab</h2>
					<h3>Monitor your Raspberry Pi or your enterprise-grade server at home.</h3>
				</div>

				<div class="transparent-900 flex w-full flex-col items-center rounded-xl border border-gray-500 p-10 shadow-xl md:w-1/3">
					<div class="bg-cores-max flex h-28 w-28 items-center justify-center rounded-full text-6xl font-medium text-white">
						<MonitorSmartphone class="h-16 w-16" />
					</div>
					<h2 class="my-5">Office PCs</h2>
					<h3>You can monitor the whole department remotely.</h3>
				</div>
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
							onclick={() => {
								changeDownload("windows")
							}}
							class={selectedDownload === "windows" ? "selectedDownloadButton" : "downloadButton"}
						>
							<Microsoft class="h-12 w-12 text-current" />
							<h2 class="text-current">Windows</h2>
						</button>

						<button
							onclick={() => {
								changeDownload("linux")
							}}
							class={selectedDownload === "linux" ? "selectedDownloadButton" : "downloadButton"}
						>
							<Ubuntu class="h-12 w-12 text-current" />
							<h2 class="text-current">Linux</h2>
						</button>

						<button
							onclick={() => {
								changeDownload("macos")
							}}
							class={selectedDownload === "macos" ? "selectedDownloadButton" : "downloadButton"}
						>
							<Apple class="h-12 w-12 text-current" />
							<h2 class="text-current">macOS</h2>
						</button>

						<button
							onclick={() => {
								changeDownload("ios")
							}}
							class={selectedDownload === "ios" ? "selectedDownloadButton" : "downloadButton"}
						>
							<svg class="h-12 w-12 text-current" role="img" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
								<title>App Store</title>
								<path
									fill="currentColor"
									d="M8.8086 14.9194l6.1107-11.0368c.0837-.1513.1682-.302.2437-.4584.0685-.142.1267-.2854.1646-.4403.0803-.3259.0588-.6656-.066-.9767-.1238-.3095-.3417-.5678-.6201-.7355a1.4175 1.4175 0 0 0-.921-.1924c-.3207.043-.6135.1935-.8443.4288-.1094.1118-.1996.2361-.2832.369-.092.1463-.175.2979-.259.4492l-.3864.6979-.3865-.6979c-.0837-.1515-.1667-.303-.2587-.4492-.0837-.1329-.1739-.2572-.2835-.369-.2305-.2353-.5233-.3857-.844-.429a1.4181 1.4181 0 0 0-.921.1926c-.2784.1677-.4964.426-.6203.7355-.1246.311-.1461.6508-.066.9767.038.155.0962.2984.1648.4403.0753.1564.1598.307.2437.4584l1.248 2.2543-4.8625 8.7825H2.0295c-.1676 0-.3351-.0007-.5026.0092-.1522.009-.3004.0284-.448.0714-.3108.0906-.5822.2798-.7783.548-.195.2665-.3006.5929-.3006.9279 0 .3352.1057.6612.3006.9277.196.2683.4675.4575.7782.548.1477.043.296.0623.4481.0715.1675.01.335.009.5026.009h13.0974c.0171-.0357.059-.1294.1-.2697.415-1.4151-.6156-2.843-2.0347-2.843zM3.113 18.5418l-.7922 1.5008c-.0818.1553-.1644.31-.2384.4705-.067.1458-.124.293-.1611.452-.0785.3346-.0576.6834.0645 1.0029.1212.3175.3346.583.607.7549.2727.172.5891.2416.9013.1975.3139-.044.6005-.1986.8263-.4402.1072-.1148.1954-.2424.2772-.3787.0902-.1503.1714-.3059.2535-.4612L6 19.4636c-.0896-.149-.9473-1.4704-2.887-.9218m20.5861-3.0056a1.4707 1.4707 0 0 0-.779-.5407c-.1476-.0425-.2961-.0616-.4483-.0705-.1678-.0099-.3352-.0091-.503-.0091H18.648l-4.3891-7.817c-.6655.7005-.9632 1.485-1.0773 2.1976-.1655 1.0333.0367 2.0934.546 3.0004l5.2741 9.3933c.084.1494.167.299.2591.4435.0837.131.1739.2537.2836.364.231.2323.5238.3809.8449.4232.3192.0424.643-.0244.9217-.1899.2784-.1653.4968-.4204.621-.7257.1246-.3072.146-.6425.0658-.9641-.0381-.1529-.0962-.2945-.165-.4346-.0753-.1543-.1598-.303-.2438-.4524l-1.216-2.1662h1.596c.1677 0 .3351.0009.5029-.009.1522-.009.3007-.028.4483-.0705a1.4707 1.4707 0 0 0 .779-.5407A1.5386 1.5386 0 0 0 24 16.452a1.539 1.539 0 0 0-.3009-.9158Z"
								/>
							</svg>
							<h2 class="text-current">iOS</h2>
						</button>
					</div>

					<div class="transparent-900 mt-10 w-[85%] space-y-8 rounded-xl border border-gray-500 p-8 text-center shadow-xl sm:p-4">
						{#if selectedDownload === "windows"}
							<h3>Download the installer or get the app from the Microsoft Store.</h3>
							<div class="flex flex-row flex-wrap items-center justify-center gap-3">
								<a
									href="https://link.levminer.com/cores-installer"
									target="_blank"
									class="smallButton bg-cores-alternative hover:text-cores-alternative border-cores-alternative gap-2 font-bold text-white hover:translate-y-0.5 hover:animate-pulse"
								>
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
								<a
									href="https://github.com/levminer/cores/releases/latest"
									target="_blank"
									class="smallButton bg-cores-alternative hover:text-cores-alternative border-cores-alternative gap-2 font-bold text-white hover:translate-y-0.5 hover:animate-pulse"
								>
									<Download width="24" height="24" />
									Download
								</a>
							</div>
							<div class="border-t-2 pt-6">
								<h3 class="text-lg">System requirements: Linux, 64bit or arm64 processor</h3>
							</div>
						{:else if selectedDownload === "ios"}
							<h3>Download the iOS version from the App Store.</h3>
							<div class="flex flex-row items-center justify-center gap-3">
								<a
									href="https://link.levminer.com/cores-ios"
									target="_blank"
									class="smallButton bg-cores-alternative hover:text-cores-alternative border-cores-alternative gap-2 font-bold text-white hover:translate-y-0.5 hover:animate-pulse"
								>
									<Download width="24" height="24" />
									Download
								</a>
							</div>
							<div class="border-t-2 pt-6">
								<h3 class="text-lg">System requirements: iOS 15.1 or later</h3>
							</div>
						{:else}
							<h3>Download and install the app with the installer.</h3>
							<div class="flex flex-row items-center justify-center gap-3">
								<a
									href="https://github.com/levminer/cores/releases/latest"
									target="_blank"
									class="smallButton bg-cores-alternative hover:text-cores-alternative border-cores-alternative gap-2 font-bold text-white hover:translate-y-0.5 hover:animate-pulse"
								>
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
											$7.99 <p class="text-xs text-gray-200">One time purchase</p>
										</h2>
									</div>

									<a
										href="https://link.levminer.com/buy-cores-app?utm_source=web"
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
										href="https://link.levminer.com/buy-cores-app?utm_source=web"
										class="button bg-cores-alternative hover:text-cores-alternative border-cores-alternative mt-5 w-full gap-2 font-bold text-white hover:translate-y-0.5 hover:animate-pulse"
									>
										<ShoppingCart />
										Buy
									</a>
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>

		<div id="faq" class="flex select-text flex-col items-center justify-center rounded-xl bg-gradient-to-r from-gray-700 to-black py-44">
			<div class="mb-10 text-center">
				<h2 class="mb-5 bg-gradient-to-r bg-clip-text text-center text-4xl font-extrabold text-white md:text-8xl">FAQ</h2>
			</div>

			<div class="flex w-full flex-col space-y-5 px-10 md:w-2/3">
				<div class="rounded-xl bg-gray-700 p-5">
					<h4>Do you offer any guarantee?</h4>
					<h3>
						We offer 7 days money back guarantee. If you are not happy with Cores, you can reach out to us, and get your money back within
						the first week, no questions asked.
					</h3>
				</div>

				<div class="rounded-xl bg-gray-700 p-5">
					<h4>How can I activate my license key?</h4>
					<h3>First download the app, log in with your Google account and then you can activate your license key on the next screen.</h3>
				</div>

				<div class="rounded-xl bg-gray-700 p-5">
					<h4>Do you have a question or concern?</h4>
					<h3>
						Send an email to <a class="underline" href="mailto:support@coresmonitor.com">support@coresmonitor.com</a> for assistance.
					</h3>
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
		ChartLine,
		Gamepad2,
		House,
	} from "lucide-svelte"
	import { onDestroy, onMount } from "svelte"
	import { Microsoft, Ubuntu, Apple } from "svelte-bootstrap-icons"
	import Footer from "../../components/Footer.svelte"
	import Header from "../../components/Header.svelte"
	import GetStarted from "../../components/GetStarted.svelte"
	import { browser } from "$app/environment"

	let selectedDownload = $state("windows")
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
	})

	onDestroy(() => {
		if (browser) {
			document.querySelector("#layout")?.classList.remove("scroll-smooth")
		}
	})
</script>
