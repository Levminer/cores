<AppHeader />
<Navigation />

{#if $state.state === "loading" && url !== "/settings"}
	<Loading />
{:else}
	<slot />
	<div class="mb-32" />
{/if}

<script lang="ts">
	import { setHardwareInfo, hardwareInfo } from "ui/stores/hardwareInfo"
	import Navigation from "ui/navigation/navigation.svelte"
	import { hardwareStatistics, setHardwareStatistics } from "ui/stores/hardwareStatistics"
	import { settings } from "ui/stores/settings"
	import { generateMinutesData, generateSecondsData } from "ui/utils/stats"
	import { page } from "$app/stores"
	import { onNavigate } from "$app/navigation"
	import { EzRTCClient } from "ezrtc"
	import Loading from "ui/navigation/loading.svelte"
	import { onMount } from "svelte"
	import { state } from "../../stores/state.ts"
	import AppHeader from "../../components/appHeader.svelte"

	$: url = $page.url.pathname

	onNavigate((navigation) => {
		url = navigation.to?.url.pathname ?? "/"
	})

	onMount(() => {
		// Connect to server when user selected a connection
		state.subscribe((data) => {
			if (data.currentCode !== "" && data.state === "waiting") {
				connect()
			}
		})

		// Reconnect if data is present from previous session
		if ($settings.connectionCode!.startsWith("crs_") && $hardwareInfo.cpu !== undefined) {
			connect()
		}
	})

	const connect = () => {
		let client: EzRTCClient | undefined

		if ($settings.connectionCode!.startsWith("crs_")) {
			$state.state = "loading"

			client = new EzRTCClient("wss://rtc-usw.levminer.com/one-to-many", $settings.connectionCode, [
				{
					urls: [
						"stun:stun.cloudflare.com:3478",
						"turn:turn.cloudflare.com:3478?transport=udp",
						"turn:turn.cloudflare.com:3478?transport=tcp",
						"turns:turn.cloudflare.com:5349?transport=tcp",
					],
					username: "fb223515f7951ff1a0df74572785c5f0c07026f6840cd21bcb259fb102b27930be9b17814d148e255f0189dc17a002ef",
					credential: "7e0d734323e9671cdeb8067eabde1d80e0018280cfb7f5e91ada94e13218fd3af1d268c45705e1bd8f929b3bc1cd8f71",
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

			$state.state = "connected"

			if (WSData.type == "data" || WSData.type == "initialData") {
				// Check if RAM load data is available
				// RAM load is rarely empty, might be a bug
				if (WSData.data.ram.load.length > 0) {
					setHardwareInfo(WSData.data)
					updateHardwareStats(WSData.data)
				}
			}
		})
	}
</script>
