<AppHeader />
<Navigation />

<slot />
<div class="mb-32" />

<script lang="ts">
	import { setHardwareInfo, hardwareInfo } from "ui/stores/hardwareInfo"
	import Navigation from "ui/navigation/navigation.svelte"
	import { hardwareStatistics, setHardwareStatistics } from "ui/stores/hardwareStatistics"
	import { settings } from "ui/stores/settings"
	import { generateMinutesData, generateSecondsData } from "ui/utils/stats"
	import { EzRTCClient } from "ezrtc"
	import { onMount } from "svelte"
	import { state } from "../../stores/state.ts"
	import AppHeader from "../../components/appHeader.svelte"

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

	const connect = () => {
		if ($settings.connectionCode!.startsWith("crs_")) {
			$state.state = "loading"

			client = new EzRTCClient("wss://rtc-usw.levminer.com/one-to-many", $settings.connectionCode, [
				{
					urls: "stun:stun.relay.metered.ca:80",
				},
				{
					urls: "turn:standard.relay.metered.ca:80",
					username: "34a987bde7c718428704bde7",
					credential: "hZA1e3RHAhw70JoP",
				},
				{
					urls: "turn:standard.relay.metered.ca:443",
					username: "34a987bde7c718428704bde7",
					credential: "hZA1e3RHAhw70JoP",
				},
			])
		}

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
		})
	}
</script>
