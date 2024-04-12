<Navigation />
{#if url === "/connect"}
	<Connect {connect} />
{:else if loading}
	<Loading showTips={true} />
{:else}
	<slot />
{/if}

<script lang="ts">
	import { setHardwareInfo, hardwareInfo } from "ui/stores/hardwareInfo"
	import Navigation from "ui/navigation/navigation.svelte"
	import { hardwareStatistics, setHardwareStatistics } from "ui/stores/hardwareStatistics"
	import { settings } from "ui/stores/settings"
	import { generateMinutesData, generateSecondsData } from "ui/utils/stats"
	import { page } from "$app/stores"
	import { onNavigate } from "$app/navigation"
	import { EzrtcClient as EzRTCClient } from "ezrtc"
	import Loading from "ui/navigation/loading.svelte"
	import Connect from "../../components/connect.svelte"
	let loading = false

	$: url = $page.url.pathname

	onNavigate((navigation) => {
		url = navigation.to?.url.pathname ?? "/"
	})

	const connect = () => {
		loading = true

		let client: EzRTCClient | undefined

		if ($settings.connectionCode!.startsWith("crs_")) {
			console.log("MI A FOS")
			client = new EzRTCClient("wss://rtc-usw.levminer.com/one-to-many", $settings.connectionCode, [
				{
					urls: "stun:stun.relay.metered.ca:80",
				},
				{
					urls: "turn:standard.relay.metered.ca:80",
					username: "56feef2e09dcd8d33c5f67eb",
					credential: "ynk5rIg6gGh4lEAk",
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
			if (message !== undefined && message !== "") {
				let parsed = JSON.parse(message)
				loading = false

				setHardwareInfo(parsed)
				updateHardwareStats(parsed)
			}
		})
	}
</script>
