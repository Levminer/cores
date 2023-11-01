<div class="flex h-screen">
	<div class="scroll w-full overflow-hidden overflow-y-scroll">
		<BuildNumber />

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
				<Route path="/system"><System /></Route>
			</Boundary>

			<Boundary onError={console.error}>
				<Route path="/settings"><Settings /></Route>
			</Boundary>
		</RouteTransition>
	</div>
</div>

<script lang="ts">
	import { Route, router } from "@baileyherbert/tinro"
	import Home from "../pages/home.svelte"
	import Settings from "../pages/settings.svelte"
	import System from "../pages/system.svelte"
	import RouteTransition from "../components/routeTransition.svelte"
	import BuildNumber from "../components/buildNumber.svelte"
	import CPU from "../pages/cpu.svelte"
	import GPU from "../pages/gpu.svelte"
	import RAM from "../pages/ram.svelte"
	import { onMount } from "svelte"
	import { hardwareStatistics, setHardwareStatistics } from "../stores/hardwareStatistics"
	import { settings } from "../stores/settings"
	// @ts-ignore
	import { Boundary } from "@crownframework/svelte-error-boundary"
	import { setHardwareInfo } from "../stores/hardwareInfo"
	import Network from "../pages/network.svelte"

	onMount(() => {
		// Navigate to the home page on load (webview bug)
		router.goto("/")

		// Scroll to the top of the page on route change
		router.subscribe((data) => {
			document.querySelector(".top").scrollIntoView()
		})

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
	})
</script>
