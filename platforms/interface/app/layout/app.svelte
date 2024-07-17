<div class="flex h-screen">
	{#if $settings.licenseKey !== ""}
		<DesktopNavigation />
	{/if}

	<div class="scroll w-full overflow-hidden overflow-y-scroll">
		<BuildNumber />

		<div class="top" />

		{#if $hardwareInfo.cpu === undefined}
			<Loading mode="desktop" />
		{:else}
			<RouteTransition>
				<Route path="/onboarding"><Onboarding /></Route>

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
					<Route path="/system"><System /></Route>
				</Boundary>

				<Boundary onError={console.error}>
					<Route path="/connections"><Connections /></Route>
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
	import System from "ui/pages/system.svelte"
	import Onboarding from "ui/pages/onboarding.svelte"
	import Connections from "ui/pages/connections.svelte"
	import RouteTransition from "ui/navigation/routeTransition.svelte"
	import BuildNumber from "ui/navigation/buildNumber.svelte"
	import { hardwareStatistics, setHardwareStatistics } from "ui/stores/hardwareStatistics"
	import { initializeSettings, settings } from "ui/stores/settings"
	import { setHardwareInfo, hardwareInfo } from "ui/stores/hardwareInfo"
	import Loading from "ui/navigation/loading.svelte"
	import { generateMinutesData, generateSecondsData } from "ui/utils/stats"
	import build from "../../../../build.json"
	import DesktopNavigation from "ui/navigation/desktopNavigation.svelte"
	import { invoke } from "@tauri-apps/api/core"
	import { check } from "@tauri-apps/plugin-updater"
	import { relaunch } from "@tauri-apps/plugin-process"
	import { ask } from "@tauri-apps/plugin-dialog"
	import posthog from "posthog-js"

	onMount(async () => {
		let sendAnalytics = true
		let retries = 0

		await initializeSettings()

		// Change background color if Mica
		const setBackgroundColor = async () => {
			const systemInfo: SystemInfo = await invoke("system_info")

			if (systemInfo.osName !== "Windows") {
				document.querySelector("body").style.background = "#0a0a0a"
			}

			if (systemInfo.osName === "Windows" && systemInfo.osVersion < "10.0.22000") {
				document.querySelector("body").style.background = "#0a0a0a"
			}
		}

		setBackgroundColor()

		// Connect to local WebSocket server
		const connectToWSServer = () => {
			let ws = new WebSocket("ws://localhost:5390/ws")

			ws.onopen = () => {
				console.log("Local WS Connection established")
			}

			ws.onmessage = (event) => {
				const WSData: NetworkMessage = JSON.parse(event.data)

				if (WSData.type == "data" || WSData.type == "initialData") {
					// Check if RAM load data is available
					// RAM load is rarely empty, might be a bug
					if (WSData.data.ram.load.length > 0) {
						setHardwareInfo(WSData.data)
						updateHardwareStats(WSData.data)
					}
				}

				if (WSData.type == "secondsData") {
					for (let i = 0; i < 3; i++) {
						updateHardwareStats(WSData.data)
					}
				}

				if (WSData.type == "minutesData") {
					for (let i = 0; i < 3; i++) {
						$hardwareStatistics.minutes.push(generateSecondsData(WSData.data))
					}
				}
			}

			ws.onclose = (e) => {
				console.log("Socket is closed.  Reconnecting...", e.reason)
				setTimeout(() => {
					if (retries == 10) {
						retries = 0
					}

					connectToWSServer()

					console.log(`Reconnecting... (Retry #${retries + 1})`)
					retries++
				}, 1000 * retries)
			}
		}

		connectToWSServer()

		const analytics = async () => {
			if (sendAnalytics && !build.dev) {
				posthog.init("phc_2zbUPXXhnelCYP2VLXeWZvKy0hykzQA7edSOsFrYZaa", {
					api_host: "https://eu.i.posthog.com",
					capture_pageview: false,
					capture_pageleave: false,
					persistence: "localStorage",
					autocapture: false,
				})

				const systemInfo: SystemInfo = await invoke("system_info")

				posthog.capture("hardware_info", {
					distinct_id: $settings.userId,
					remote_connections: $settings.remoteConnections,
					version: build.version,
					build: build.number,
					cpu: systemInfo.cpuName,
					gpu: systemInfo.gpuName,
					os: systemInfo.osName,
					ram: Math.round(systemInfo.totalMem / 1024 / 1024 / 1024),
					date: new Date().toISOString().split("T")[0],
				})

				sendAnalytics = false
			}
		}

		analytics()

		// Navigate to the home page on load (webview bug)
		if ($settings.licenseKey === "" || $settings.licenseKey === "free") {
			router.goto("/onboarding")

			let dateActivated = new Date($settings.licenseActivated)
			let dateNow = new Date()
			let diff = dateNow.getTime() - dateActivated.getTime()
			let days = Math.ceil(diff / (1000 * 3600 * 24))

			if (days > 7) {
				$settings.licenseKey = ""
			}
		} else {
			router.goto("/home")
		}

		// Scroll to the top of the page on route change
		router.subscribe(() => {
			document.querySelector(".top").scrollIntoView()
		})

		// Check for updates
		const checkForUpdates = async () => {
			if (!build.dev) {
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
