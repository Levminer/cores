<div class="flex h-screen">
	<DesktopNavigation />

	<div class="scroll w-full overflow-hidden overflow-y-scroll">
		<BuildNumber />

		<div class="top" />

		{#if $hardwareInfo.cpu === undefined}
			<DesktopLoading />
		{:else}
			<RouteTransition>
				<Boundary onError={console.error}>
					<Route path="/home"><Home /></Route>
				</Boundary>

				<Boundary onError={console.error}>
					<Route path="/cpu"><CPU /></Route>
				</Boundary>

				<Boundary onError={console.error}>
					<Route path="/ram"><RAM /></Route>
				</Boundary>

				<Boundary onError={console.error}>
					<Route path="/gpu"><GPU /></Route>
				</Boundary>

				<Boundary onError={console.error}>
					<Route path="/network"><Network /></Route>
				</Boundary>

				<Boundary onError={console.error}>
					<Route path="/storage"><Storage /></Route>
				</Boundary>

				<Boundary onError={console.error}>
					<Route path="/settings"><Settings /></Route>
				</Boundary>
			</RouteTransition>
		{/if}
	</div>
</div>

<script lang="ts">
	// @ts-ignore - no types
	import { Boundary } from "@crownframework/svelte-error-boundary"
	import { onMount } from "svelte"
	import { Route, router } from "@baileyherbert/tinro"
	import Home from "ui/pages/home.svelte"
	import Settings from "ui/pages/settings.svelte"
	import CPU from "ui/pages/cpu.svelte"
	import GPU from "ui/pages/gpu.svelte"
	import RAM from "ui/pages/ram.svelte"
	import Storage from "ui/pages/storage.svelte"
	import Network from "ui/pages/network.svelte"
	import RouteTransition from "ui/navigation/routeTransition.svelte"
	import BuildNumber from "ui/navigation/buildNumber.svelte"
	import { hardwareStatistics, setHardwareStatistics } from "ui/stores/hardwareStatistics"
	import { initializeSettings } from "ui/stores/settings"
	import { setHardwareInfo, hardwareInfo } from "ui/stores/hardwareInfo"
	import DesktopLoading from "ui/navigation/desktopLoading.svelte"
	import { generateMinutesData, generateSecondsData } from "ui/utils/stats"
	import { init as initAnalytics, trackEvent } from "@aptabase/web"
	import build from "../../../build.json"
	import DesktopNavigation from "ui/navigation/desktopNavigation.svelte"
	import { invoke } from "@tauri-apps/api/core"
	import { check } from "@tauri-apps/plugin-updater"
	import { relaunch } from "@tauri-apps/plugin-process"
	import { ask } from "@tauri-apps/plugin-dialog"

	initAnalytics("A-EU-8117718240", { appVersion: build.version })

	onMount(async () => {
		let sendAnalytics = true
		let retries = 0

		initializeSettings()

		// Change background color if Mica
		const setBackgroundColor = async () => {
			const systemInfo: SystemInfo = await invoke("system_info")

			if (systemInfo.osName === "Windows" && systemInfo.osVersion < "10.0.22000") {
				document.querySelector("body").style.background = "#0a0a0a"
			}
		}

		setBackgroundColor()

		// Connect to local WebSocket server
		const connectToWSServer = () => {
			let ws = new WebSocket("ws://localhost:5391")

			ws.onopen = () => {
				console.log("Local WS Connection established")
			}

			ws.onmessage = (event) => {
				const data: HardwareInfo = JSON.parse(event.data)

				if (sendAnalytics && !build.dev) {
					console.log("Sending analytics")

					trackEvent("hardware_info", {
						version: build.version,
						build: build.number,
						cpu: data.cpu.name ?? "N/A",
						gpu: data.gpu.name ?? "N/A",
						os: data.system.os.name ?? "N/A",
						ram: `${Math.round((data.ram.load[0]?.value ?? 0) + (data.ram.load[1]?.value ?? 0))} GB`,
						date: new Date().toISOString().split("T")[0],
					})

					sendAnalytics = false
				}

				setHardwareInfo(data)
				updateHardwareStats(data)
			}

			ws.onclose = (e) => {
				console.log("Socket is closed.  Reconnecting...", e.reason)
				setTimeout(() => {
					if (retries == 3) {
						sessionStorage.clear()
						location.reload()
					}

					connectToWSServer()

					console.log(`Reconnecting... (Retry #${retries + 1})`)
					retries++
				}, 1000)
			}
		}

		connectToWSServer()

		// Navigate to the home page on load (webview bug)
		router.goto("/home")

		// Scroll to the top of the page on route change
		router.subscribe(() => {
			document.querySelector(".top").scrollIntoView()
		})

		// Check for updates
		const checkForUpdates = async () => {
			if (build.dev) {
				console.log("Checking for updates")

				const update = await check()

				console.log("Update:", update)

				if (update.available) {
					const result = await ask("A new version of Cores is available. Do you want to update?", {
						title: "Cores update available",
					})

					if (result) {
						await update.downloadAndInstall((event) => {
							console.log("Downloading update", event)
						})
						await relaunch()
					}
				}
			}
		}

		checkForUpdates()

		// 60s date comparison
		const date = new Date()
		date.setSeconds(date.getSeconds() + 60)

		// Update hardware statistics
		const updateHardwareStats = (input: HardwareInfo) => {
			if (Object.keys(input).length !== 0) {
				if ($hardwareStatistics.minutes.length > 60) {
					$hardwareStatistics.minutes.shift()
				}

				if ($hardwareStatistics.seconds.length > 60) {
					$hardwareStatistics.seconds.shift()
				}

				let secondsData: Stats = generateSecondsData(input)

				if (date.getTime() < new Date().getTime()) {
					let minutesData: Stats = generateMinutesData(input, $hardwareStatistics)

					// Update 60s timer
					date.setSeconds(date.getSeconds() + 60)

					setHardwareStatistics({
						seconds: [...$hardwareStatistics.seconds, secondsData],
						minutes: [...$hardwareStatistics.minutes, minutesData],
					})
				} else {
					setHardwareStatistics({
						seconds: [...$hardwareStatistics.seconds, secondsData],
						minutes: [...$hardwareStatistics.minutes],
					})
				}
			}
		}
	})
</script>

<style>
</style>
