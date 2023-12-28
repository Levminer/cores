<div class="flex h-screen flex-col">
	<div class="scroll w-full overflow-hidden overflow-y-scroll">
		<BuildNumber />

		<Navigation />

		<div class="top" />

		<div>
			<slot />
		</div>
	</div>
</div>

<svelte:head>
	<title>Cores</title>
	<meta name="description" content="Cores - Hardware monitor - Monitor CPU/RAM/GPU usage like clock speed, voltage, memory usage, temperature" />
	<meta property="og:title" content="Cores - Hardware monitor" />
	<meta property="og:image" content="https://www.levminer.com/og.png" />
	<meta property="og:type" content="website" />
	<meta
		property="og:description"
		content="Cores - Hardware monitor - Monitor CPU/RAM/GPU usage like clock speed, voltage, memory usage, temperature"
	/>
	<meta property="og:locale" content="en_US" />
</svelte:head>

<script lang="ts">
	import "ui/styles/index.css"
	import { onMount } from "svelte"
	import { hardwareStatistics, setHardwareStatistics } from "ui/stores/hardwareStatistics"
	import init, { WebRtcClient } from "../../../../crates/client/pkg/lib.js"
	import { settings } from "ui/stores/settings"
	import { setHardwareInfo } from "ui/stores/hardwareInfo"
	import BuildNumber from "ui/navigation/BuildNumber.svelte"
	import Navigation from "ui/navigation/Navigation.svelte"
	import { onNavigate } from "$app/navigation"
	import { generateMinutesData, generateSecondsData } from "ui/utils/stats"

	onNavigate((navigation) => {
		document.querySelector(".top")!.scrollIntoView()

		if (!document.startViewTransition) return

		return new Promise((resolve) => {
			document.startViewTransition(async () => {
				resolve()
				await navigation.complete
			})
		})
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
