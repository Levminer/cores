<div class="flex h-screen flex-col">
	<div class="scroll w-full overflow-hidden overflow-y-scroll">
		<BuildNumber />

		{#if $settings.mode === "client"}
			<Navigation />
		{/if}

		<div class="top" />

		<RouteTransition>
			<Boundary onError={console.error}>
				<Route path="/"><Home /></Route>
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
	</div>
</div>

<script lang="ts">
	// @ts-ignore - no types
	import { Boundary } from "@crownframework/svelte-error-boundary"
	import { onMount } from "svelte"
	import { Route, router } from "@baileyherbert/tinro"
	import Home from "@pages/home.svelte"
	import Settings from "@pages/settings.svelte"
	import CPU from "@pages/cpu.svelte"
	import GPU from "@pages/gpu.svelte"
	import RAM from "@pages/ram.svelte"
	import Storage from "@pages/storage.svelte"
	import Network from "@pages/network.svelte"
	import Navigation from "@components/navigation.svelte"
	import RouteTransition from "@components/routeTransition.svelte"
	import BuildNumber from "@components/buildNumber.svelte"
	import { hardwareStatistics, setHardwareStatistics } from "../stores/hardwareStatistics"
	import init, { WebRtcHost, WebRtcClient } from "../../crates/client/pkg/lib.js"
	import { settings } from "../stores/settings"
	import { setHardwareInfo } from "../stores/hardwareInfo"

	onMount(() => {
		let host: WebRtcHost | undefined
		let client: WebRtcClient | undefined

		init().then(() => {
			if ($settings.mode === "app") {
				host = new WebRtcHost()
			} else {
				client = new WebRtcClient()
			}
		})

		// Navigate to the home page on load (webview bug)
		if ($settings.mode === "app") {
			router.goto("/")
		}

		// Scroll to the top of the page on route change
		router.subscribe(() => {
			document.querySelector(".top").scrollIntoView()
		})

		if ($settings.mode === "app") {
			// @ts-ignore - Receive settings from the webview
			window.chrome.webview.addEventListener("message", (arg: { data: Message }) => {
				if (arg.data.name === "settings") {
					console.log("New settings")

					$settings = JSON.parse(arg.data.content)
				}
			})

			// @ts-ignore - Receive api data from the webview
			window.chrome.webview.addEventListener("message", (arg: { data: Message }) => {
				if (arg.data.name === "api") {
					let parsed = JSON.parse(arg.data.content)

					if (host !== undefined) {
						host.send_message_to_clients(JSON.stringify(parsed))
					}

					setHardwareInfo(parsed)
					updateHardwareStats(parsed)
				}
			})

			// @ts-ignore - Receive navigation info
			window.chrome.webview.addEventListener("message", (arg: { data: Message }) => {
				if (arg.data.name === "navigation") {
					if (arg.data.content === "home") {
						router.goto("/")
					} else {
						router.goto(arg.data.content)
					}
				}
			})
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

				let secondsData: Stats = {
					cpu: {
						temperature: {
							value: Math.round(
								input.cpu.temperature.map((sensor) => sensor.value).reduce((a, b) => a + b, 0) / input.cpu.temperature.length,
							),
							min: Math.round(
								input.cpu.temperature.map((sensor) => sensor.min).reduce((a, b) => a + b, 0) / input.cpu.temperature.length,
							),
							max: Math.round(
								input.cpu.temperature.map((sensor) => sensor.max).reduce((a, b) => a + b, 0) / input.cpu.temperature.length,
							),
						},

						clock: {
							value: Math.round(input.cpu.clock.map((sensor) => sensor.value).reduce((a, b) => a + b, 0) / input.cpu.clock.length),
							min: Math.round(input.cpu.clock.map((sensor) => sensor.min).reduce((a, b) => a + b, 0) / input.cpu.clock.length),
							max: Math.round(input.cpu.clock.map((sensor) => sensor.max).reduce((a, b) => a + b, 0) / input.cpu.clock.length),
						},

						load: Math.round(input.cpu.lastLoad),
						power: Math.round(input.cpu.power.reduce((a, b) => a + b.value, 0)),
						voltage: parseFloat((input.cpu.voltage.reduce((a, b) => a + b.value, 0) / input.cpu.clock.length).toFixed(2)),
					},

					ram: {
						physicalUsage: Math.round(input.ram.load[2].value),
						virtualUsage: Math.round(input.ram.load[5].value),
					},

					gpu: {
						temperature: {
							value: Math.round(
								input.gpu.temperature.map((sensor) => sensor.value).reduce((a, b) => a + b, 0) / input.gpu.temperature.length,
							),
							min: Math.round(
								input.gpu.temperature.map((sensor) => sensor.min).reduce((a, b) => a + b, 0) / input.gpu.temperature.length,
							),
							max: Math.round(
								input.gpu.temperature.map((sensor) => sensor.max).reduce((a, b) => a + b, 0) / input.gpu.temperature.length,
							),
						},

						clock: {
							value: Math.round(input.gpu.clock[0].value),
							min: Math.round(input.gpu.clock[0].value),
							max: Math.round(input.gpu.clock[0].value),
						},

						load: Math.round(input.gpu.lastLoad),
						power: Math.round(input.gpu.power.reduce((a, b) => a + b.value, 0)),
						fan: Math.round(input.gpu.fan.reduce((a, b) => a + b.value, 0)),
						memory: parseFloat(input.gpu.memory[0].value.toFixed(1)),
					},

					network: input.system.network.interfaces.map((int) => {
						return {
							throughputUpload: parseFloat((int.throughputUpload / 1_048_576).toFixed(2)),
							throughputDownload: parseFloat((int.throughputDownload / 1_048_576).toFixed(2)),
						}
					}),

					storage: input.system.storage.disks.map((item) => {
						return {
							throughputRead: parseFloat((item.throughputRead / 1_048_576).toFixed(2)),
							throughputWrite: parseFloat((item.throughputWrite / 1_048_576).toFixed(2)),
						}
					}),
				}

				if (date.getTime() < new Date().getTime()) {
					let minutesData: Stats = {
						cpu: {
							temperature: {
								value: Math.round(
									$hardwareStatistics.seconds.map((sensor) => sensor.cpu.temperature.value).reduce((a, b) => a + b, 0) /
										$hardwareStatistics.seconds.length,
								),
								min: Math.round(
									$hardwareStatistics.seconds.map((sensor) => sensor.cpu.temperature.min).reduce((a, b) => a + b, 0) /
										$hardwareStatistics.seconds.length,
								),
								max: Math.round(
									$hardwareStatistics.seconds.map((sensor) => sensor.cpu.temperature.max).reduce((a, b) => a + b, 0) /
										$hardwareStatistics.seconds.length,
								),
							},

							clock: {
								value: Math.round(
									$hardwareStatistics.seconds.map((sensor) => sensor.cpu.clock.value).reduce((a, b) => a + b, 0) /
										$hardwareStatistics.seconds.length,
								),
								min: Math.round(
									$hardwareStatistics.seconds.map((sensor) => sensor.cpu.clock.min).reduce((a, b) => a + b, 0) /
										$hardwareStatistics.seconds.length,
								),
								max: Math.round(
									$hardwareStatistics.seconds.map((sensor) => sensor.cpu.clock.max).reduce((a, b) => a + b, 0) /
										$hardwareStatistics.seconds.length,
								),
							},

							load: Math.round(
								$hardwareStatistics.seconds.map((sensor) => sensor.cpu.load).reduce((a, b) => a + b, 0) /
									$hardwareStatistics.seconds.length,
							),
							power: Math.round(
								$hardwareStatistics.seconds.map((sensor) => sensor.cpu.power).reduce((a, b) => a + b, 0) /
									$hardwareStatistics.seconds.length,
							),
							voltage: parseFloat(
								(
									$hardwareStatistics.seconds.map((sensor) => sensor.cpu.voltage).reduce((a, b) => a + b, 0) /
									$hardwareStatistics.seconds.length
								).toFixed(2),
							),
						},

						ram: {
							physicalUsage: Math.round(
								$hardwareStatistics.seconds.map((sensor) => sensor.ram.physicalUsage).reduce((a, b) => a + b, 0) /
									$hardwareStatistics.seconds.length,
							),
							virtualUsage: Math.round(
								$hardwareStatistics.seconds.map((sensor) => sensor.ram.virtualUsage).reduce((a, b) => a + b, 0) /
									$hardwareStatistics.seconds.length,
							),
						},

						gpu: {
							temperature: {
								value: Math.round(
									$hardwareStatistics.seconds.map((sensor) => sensor.gpu.temperature.value).reduce((a, b) => a + b, 0) /
										$hardwareStatistics.seconds.length,
								),
								min: Math.round(
									$hardwareStatistics.seconds.map((sensor) => sensor.gpu.temperature.min).reduce((a, b) => a + b, 0) /
										$hardwareStatistics.seconds.length,
								),
								max: Math.round(
									$hardwareStatistics.seconds.map((sensor) => sensor.gpu.temperature.max).reduce((a, b) => a + b, 0) /
										$hardwareStatistics.seconds.length,
								),
							},

							clock: {
								value: Math.round(
									$hardwareStatistics.seconds.map((sensor) => sensor.gpu.clock.value).reduce((a, b) => a + b, 0) /
										$hardwareStatistics.seconds.length,
								),
								min: Math.round(
									$hardwareStatistics.seconds.map((sensor) => sensor.gpu.clock.min).reduce((a, b) => a + b, 0) /
										$hardwareStatistics.seconds.length,
								),
								max: Math.round(
									$hardwareStatistics.seconds.map((sensor) => sensor.gpu.clock.max).reduce((a, b) => a + b, 0) /
										$hardwareStatistics.seconds.length,
								),
							},

							load: Math.round(
								$hardwareStatistics.seconds.map((sensor) => sensor.gpu.load).reduce((a, b) => a + b, 0) /
									$hardwareStatistics.seconds.length,
							),
							power: Math.round(
								$hardwareStatistics.seconds.map((sensor) => sensor.gpu.power).reduce((a, b) => a + b, 0) /
									$hardwareStatistics.seconds.length,
							),
							fan: Math.round(
								$hardwareStatistics.seconds.map((sensor) => sensor.gpu.fan).reduce((a, b) => a + b, 0) /
									$hardwareStatistics.seconds.length,
							),
							memory: parseFloat(
								(
									$hardwareStatistics.seconds.map((sensor) => sensor.gpu.memory).reduce((a, b) => a + b, 0) /
									$hardwareStatistics.seconds.length
								).toFixed(1),
							),
						},

						network: input.system.network.interfaces.map((item, i) => {
							let throughputDownload = parseFloat(
								(
									$hardwareStatistics.seconds.map((sensor) => sensor.network[i]).reduce((a, b) => a + b.throughputDownload, 0) /
									$hardwareStatistics.seconds.length
								).toFixed(2),
							)

							let throughputUpload = parseFloat(
								(
									$hardwareStatistics.seconds.map((sensor) => sensor.network[i]).reduce((a, b) => a + b.throughputUpload, 0) /
									$hardwareStatistics.seconds.length
								).toFixed(2),
							)

							return {
								throughputUpload,
								throughputDownload,
							}
						}),

						storage: input.system.storage.disks.map((item, i) => {
							let throughputRead = parseFloat(
								(
									$hardwareStatistics.seconds.map((sensor) => sensor.storage[i]).reduce((a, b) => a + b.throughputRead, 0) /
									$hardwareStatistics.seconds.length
								).toFixed(2),
							)

							let throughputWrite = parseFloat(
								(
									$hardwareStatistics.seconds.map((sensor) => sensor.storage[i]).reduce((a, b) => a + b.throughputWrite, 0) /
									$hardwareStatistics.seconds.length
								).toFixed(2),
							)

							return {
								throughputRead,
								throughputWrite,
							}
						}),
					}

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

		if ($settings.mode === "client") {
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
		}
	})
</script>
