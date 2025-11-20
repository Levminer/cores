{#if page.url.pathname !== "/embed"}
	<AppHeader />
	<Navigation />
{/if}

{@render children()}

{#if page.url.pathname !== "/embed"}
	<div class="mb-32"></div>
{/if}

<script lang="ts">
	import { EzRTCClient } from "ezrtc"
	import { onMount } from "svelte"
	import { state } from "../../stores/state.ts"
	import AppHeader from "../../components/AppHeader.svelte"
	import { hardwareStatistics, hardwareInfo, settings, setHardwareStatistics, generateMinutesData, generateSecondsData, setHardwareInfo } from "ui"
	import Navigation from "../../components/Navigation.svelte"
	import { page } from "$app/state"

	let { children } = $props()

	let client: EzRTCClient | undefined

	onMount(() => {
		// Connect to server when user selected a connection
		state.subscribe((data) => {
			if (data.currentCode !== "" && data.state === "waiting") {
				connect()
			}

			if (data.state === "disconnected") {
				client?.peerConnection.close()
				sessionStorage.removeItem("hardwareInfo")
				sessionStorage.removeItem("hardwareStatistics")
				location.reload()
			}

			if (data.state === "swapping") {
				client?.peerConnection.close()
				sessionStorage.removeItem("hardwareInfo")
				sessionStorage.removeItem("hardwareStatistics")
				// @ts-ignore
				$hardwareInfo.cpu = {}
				location.reload()
			}

			if (data.message !== "") {
				console.log("Message sent to host")
				client?.sendMessage(data.message)
				data.message = ""
			}
		})

		// Reconnect if data is present from previous session
		if ($settings.connectionCode!.startsWith("crs_") && $hardwareInfo.cpu !== undefined) {
			connect()
		}
	})

	const connect = async () => {
		let iceServers: RTCIceServer[] = [{ urls: "stun:stun.cloudflare.com:3478" }]

		if (import.meta.env.VITE_TURN_SERVER_URL) {
			try {
				const res = await fetch(import.meta.env.VITE_TURN_SERVER_URL)
				const data = await res.json()

				iceServers = iceServers.concat(data)
				console.log("Fetched TURN credentials")
			} catch (error) {
				console.log("Failed to fetch TURN credentials", error)
			}
		}

		if ($settings.connectionCode!.startsWith("crs_")) {
			$state.state = "loading"

			client = new EzRTCClient(`wss://${$settings.connectionURL}/one-to-many`, $settings.connectionCode, iceServers)
		}

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

		client?.onMessage((message) => {
			const WSData: NetworkMessage = JSON.parse(message)

			if ($state.state !== "connected") {
				$state.state = "connected"
			}

			if (WSData.type == "data" || WSData.type == "initialData") {
				// Check if RAM load data is available
				// RAM load is rarely empty, might be a bug
				if (WSData.data.ram.load.length > 0) {
					setHardwareInfo(WSData.data)
				}

				if (WSData.type == "data") {
					updateHardwareStats(WSData.data)
				}
			}

			if (WSData.type == "secondsData") {
				updateHardwareStats(WSData.data)
			}

			if (WSData.type == "initialMinutesData") {
				$hardwareStatistics.minutes.push(generateSecondsData(WSData.data))
			}

			if (WSData.type == "minutesData") {
				$hardwareStatistics.minutes.push(generateSecondsData(WSData.data))
			}
		})
	}
</script>
