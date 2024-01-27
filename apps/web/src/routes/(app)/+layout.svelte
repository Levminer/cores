{#if $hardwareInfo.cpu === undefined}
	{#if url === "/settings"}
		<Navigation />
		<slot />
	{:else}
		<Loading showTips={true} />
	{/if}
{:else}
	<Navigation />
	<slot />
{/if}

<script lang="ts">
	import { setHardwareInfo, hardwareInfo } from "ui/stores/hardwareInfo"
	import Loading from "ui/navigation/loading.svelte"
	import Navigation from "ui/navigation/navigation.svelte"
	import { onMount } from "svelte"
	import { hardwareStatistics, setHardwareStatistics } from "ui/stores/hardwareStatistics"
	import init, { WebRtcClient } from "../../../../../crates/client/pkg/lib.js"
	import { settings } from "ui/stores/settings"
	import { generateMinutesData, generateSecondsData } from "ui/utils/stats"
	import { page } from "$app/stores"
	import { onNavigate } from "$app/navigation"

	$: url = $page.url.pathname

	onNavigate((navigation) => {
		url = navigation.to?.url.pathname ?? "/"
	})

	onMount(() => {
		let client: WebRtcClient | undefined

		init().then(() => {
			if ($settings.connectionCode!.startsWith("crs_")) {
				client = new WebRtcClient($settings.connectionCode!)
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

		setInterval(() => {
			if (client !== undefined) {
				let message = client.get_message()

				if (message !== undefined && message !== "") {
					let parsed = JSON.parse(message)

					setHardwareInfo(parsed)
					updateHardwareStats(parsed)
				}
			}
		}, 3000)
	})
</script>
