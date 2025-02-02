<div class="flex h-screen">
	{#if $state.showMenu}
		<DesktopNavigation />
	{/if}

	<div class="scroll w-full overflow-hidden overflow-y-scroll">
		{#if build.number.startsWith("alpha") || build.number.startsWith("beta")}
			<BuildNumber />
		{/if}

		<UpdateAlert />

		<div class="top" />

		{#if $hardwareInfo.cpu === undefined || loading}
			<Loading mode="desktop" />
		{:else}
			<RouteTransition>
				<Route path="/onboarding"><Onboarding /></Route>

				<Boundary onError={console.error}>
					<Route path="/home"><Home /></Route>
				</Boundary>

				<Boundary onError={console.error}>
					<Route path="/cpu"><Cpu /></Route>
				</Boundary>

				<Boundary onError={console.error}>
					<Route path="/ram"><Ram /></Route>
				</Boundary>

				<Boundary onError={console.error}>
					<Route path="/gpu"><Gpu /></Route>
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
					{#if !$state.plan}
						<Route path="/connections"><Onboarding /></Route>
					{:else}
						<Route path="/connections"><Connections /></Route>
					{/if}
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
	import build from "../../../../build.json"
	import { invoke } from "@tauri-apps/api/core"
	import posthog from "posthog-js"
	import {
		DesktopNavigation,
		BuildNumber,
		Loading,
		RouteTransition,
		Onboarding,
		Settings,
		Home,
		Cpu,
		Gpu,
		Ram,
		System,
		Network,
		Connections,
		Storage,
		hardwareStatistics,
		setHardwareStatistics,
		initializeSettings,
		settings,
		state,
		generateMinutesData,
		generateSecondsData,
		setHardwareInfo,
		hardwareInfo,
		supabaseClient,
		UpdateAlert,
	} from "ui"

	$: loading = true

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

				if (WSData.type == "initialMinutesData") {
					$hardwareStatistics.minutes.push(generateSecondsData(WSData.data))
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
		const authenticate = async () => {
			const { data: userData, error: userError } = await supabaseClient.auth.getUser()

			if (!userError && userData !== null) {
				// User logged in
				const { data, error } = await supabaseClient.from("user").select("*").single()

				if (data.plan === "personal" || data.plan === "business") {
					// User is on a paid plan
					$state.showMenu = true
					$state.plan = data.plan
					router.goto("/home")
				} else {
					// User is on a free plan
					router.goto("/onboarding")
				}
			} else {
				// User not logged in
				$state.showMenu = false
				router.goto("/onboarding")
			}

			loading = false
		}

		authenticate()

		// Scroll to the top of the page on route change
		router.subscribe(() => {
			document.querySelector(".top").scrollIntoView()
		})

		// 60s date comparison
		const date = new Date()
		date.setSeconds(date.getSeconds() + 60)

		// Update hardware statistics
		const updateHardwareStats = (input: HardwareInfo) => {
			if (Object.keys(input).length !== 0) {
				if ($hardwareStatistics.minutes.length >= 62) {
					$hardwareStatistics.minutes.shift()
				}

				if ($hardwareStatistics.seconds.length >= 61) {
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
