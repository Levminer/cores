<div class="flex h-screen">
	{#if $appState.showMenu}
		<DesktopNavigation pathname={$page.url.pathname} />
	{/if}

	<div class="scroll w-full overflow-hidden overflow-y-scroll">
		{#if build.number.startsWith("alpha") || build.number.startsWith("beta")}
			<BuildNumber />
		{/if}

		<UpdateAlert />
		<NewsAlert />

		<div class="top"></div>

		{#if $hardwareInfo.cpu === undefined || loading}
			<Loading mode="desktop" />
		{:else}
			{@render children()}
		{/if}
	</div>
</div>

<script lang="ts">
	import build from "../../../../../build.json"
	import "../../../ui/styles/index.css"
	import "../app.css"
	import { onNavigate, goto } from "$app/navigation"
	import { page } from "$app/stores"
	import {
		appState,
		BuildNumber,
		DesktopNavigation,
		UpdateAlert,
		NewsAlert,
		hardwareInfo,
		Loading,
		initializeSettings,
		setHardwareInfo,
		generateSecondsData,
		hardwareStatistics,
		settings,
		supabaseClient,
		appendMinutesData,
		MINUTES_FALLBACK_INTERVAL,
		setHardwareStatistics,
	} from "ui"
	import { invoke } from "@tauri-apps/api/core"
	import posthog from "posthog-js"

	let { children } = $props()

	let loading = $state(true)

	$effect(() => {
		let ws: WebSocket

		const init = async () => {
			let sendAnalytics = true
			let retries = 0

			await initializeSettings()

			// Change background color if Mica
			const setBackgroundColor = async () => {
				const systemInfo: SystemInfo = await invoke("system_info")

				if (systemInfo.osName !== "Windows") {
					document.querySelector("body")!.style.background = "#0a0a0a"
				}

				if (systemInfo.osName === "Windows" && systemInfo.osVersion < "10.0.22000") {
					document.querySelector("body")!.style.background = "#0a0a0a"
				}
			}

			setBackgroundColor()

			// Connect to local WebSocket server
			const connectToWSServer = () => {
				ws = new WebSocket("ws://localhost:5390/ws")

				let lastMinutesData = Date.now()

				const addMinutesData = (input: HardwareInfo) => {
					lastMinutesData = Date.now()

					setHardwareStatistics({
						seconds: $hardwareStatistics.seconds,
						minutes: appendMinutesData($hardwareStatistics.minutes, input),
					})
				}

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
						}

						if (WSData.type == "data") {
							updateHardwareStats(WSData.data)

							// Older services don't push minutes data live, record a snapshot instead when none arrived for a window
							if (Date.now() - lastMinutesData > MINUTES_FALLBACK_INTERVAL) {
								addMinutesData({ ...WSData.data, timestamp: WSData.data.timestamp ?? new Date().toISOString() })
							}
						}
					}

					if (WSData.type == "secondsData") {
						updateHardwareStats(WSData.data)
					}

					// initialMinutesData repeats the first minutesData point, so it is ignored
					if (WSData.type == "minutesData") {
						addMinutesData(WSData.data)
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
				posthog.init("phc_2zbUPXXhnelCYP2VLXeWZvKy0hykzQA7edSOsFrYZaa", {
					api_host: "https://eu.i.posthog.com",
					capture_pageview: false,
					capture_pageleave: false,
					persistence: "localStorage",
					autocapture: false,
				})

				if (sendAnalytics && !build.dev) {
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
				try {
					// Unix check
					const systemInfo: SystemInfo = await invoke("system_info")
					if (systemInfo.osName !== "Windows" && systemInfo.osName !== "macOS") {
						$appState.showMenu = true
						$appState.plan = "unix"
						goto("/home")
						loading = false
						return
					}

					// Check supabase health
					const res = await fetch(
						"https://ailnlslhpgedtlbxfkcz.supabase.co/auth/v1/health?apikey=sb_publishable_Zi57ivjBxc5SSqaEESn0Gg_qfhNTYIT",
					)

					if (!res.ok) {
						throw new Error("Failed to check Supabase health")
					}

					// Login
					const { data: userData, error: userError } = await supabaseClient.auth.getUser()

					if (!userError && userData !== null) {
						// User logged in
						const { data, error } = await supabaseClient.from("user").select("*").single()

						if (data?.plan === "personal" || data?.plan === "business") {
							// User is on a paid plan
							$appState.showMenu = true
							$appState.plan = data.plan
							goto("/home")
						} else {
							// User is on a free plan
							goto("/onboarding")
						}
					} else {
						// User not logged in
						$appState.showMenu = false
						goto("/onboarding")
					}

					loading = false
				} catch (error) {
					alert(
						`Failed to connect to the server, continuing in offline mode. Please check your internet connection and try again.\n${error}`,
					)
					$appState.showMenu = true
					// $appState.plan
					goto("/home")
					loading = false
				}
			}

			authenticate()

			// Scroll to the top of the page on route change
			/* router.subscribe(() => {
				document.querySelector(".top")!.scrollIntoView()
			}) */

			// Update hardware statistics
			const updateHardwareStats = (input: HardwareInfo) => {
				if (Object.keys(input).length !== 0) {
					if ($hardwareStatistics.seconds.length >= 61) {
						$hardwareStatistics.seconds.shift()
					}

					setHardwareStatistics({
						seconds: [...$hardwareStatistics.seconds, generateSecondsData(input)],
						minutes: $hardwareStatistics.minutes,
					})
				}
			}
		}

		init()

		// Cleanup function for $effect
		return () => {
			if (ws) {
				ws.close()
			}
		}
	})

	onNavigate((navigation) => {
		if (!document.startViewTransition) return

		return new Promise((resolve) => {
			document.startViewTransition(async () => {
				resolve()
				await navigation.complete
				document.querySelector(".top")!.scrollIntoView()
			})
		})
	})
</script>
