<div class="flex h-screen">
	<DesktopNavigation />

	<div class="scroll w-full overflow-hidden overflow-y-scroll">
		<BuildNumber />

		<div class="top" />

		{#if $hardwareInfo.cpu === undefined}
			<Loading />
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
	import { settings } from "ui/stores/settings"
	import { setHardwareInfo, hardwareInfo } from "ui/stores/hardwareInfo"
	import Loading from "ui/navigation/loading.svelte"
	import { generateMinutesData, generateSecondsData } from "ui/utils/stats"
	import { init as initAnalytics, trackEvent } from "@aptabase/web"
	import build from "../../../build.json"
	import DesktopNavigation from "ui/navigation/desktopNavigation.svelte"

	initAnalytics("A-EU-8347557657", { appVersion: build.version })

	onMount(() => {
		// TODO: Set background color to #0a0a0a when os not supports mica

		// connect to local ws server
		const ws = new WebSocket("ws://localhost:5391")

		ws.onopen = () => {
			console.log("Local WS Connection established")
		}

		ws.onmessage = (event) => {
			const data = JSON.parse(event.data)

			setHardwareInfo(data)
			updateHardwareStats(data)
		}

		// TODO: Reconnect to ws server if connection is lost

		let sendAnalytics = true

		// Navigate to the home page on load (webview bug)
		router.goto("/home")

		// Scroll to the top of the page on route change
		router.subscribe(() => {
			document.querySelector(".top").scrollIntoView()
		})

		// @ts-ignore - Receive settings from the webview - DEPRECATED
		window.chrome.webview.addEventListener("message", (arg: { data: Message }) => {
			if (arg.data.name === "settings") {
				console.log("New settings")

				$settings = JSON.parse(arg.data.content)
			}
		})

		// @ts-ignore - Receive api data from the webview - DEPRECATED
		window.chrome.webview.addEventListener("message", (arg: { data: Message }) => {
			if (arg.data.name === "api") {
				let parsed: HardwareInfo = JSON.parse(arg.data.content)

				if (sendAnalytics && !build.dev) {
					console.log("Sending analytics")

					trackEvent("hardware_info", {
						version: build.version,
						build: build.number,
						cpu: parsed.cpu.name ?? "N/A",
						gpu: parsed.gpu.name ?? "N/A",
						os: parsed.system.os.name ?? "N/A",
						ram: `${Math.round((parsed.ram.load[0]?.value ?? 0) + (parsed.ram.load[1]?.value ?? 0))} GB`,
						date: new Date().toISOString().split("T")[0],
					})

					sendAnalytics = false
				}

				setHardwareInfo(parsed)
				updateHardwareStats(parsed)
			}
		})

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
	:global(body) {
		background-color: transparent !important;
	}
</style>
