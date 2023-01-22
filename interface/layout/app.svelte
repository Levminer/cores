<div class="flex h-screen">
	<Navigation />

	<div class="scroll w-full overflow-hidden overflow-y-scroll">
		<BuildNumber />

		<div class="top" />

		<RouteTransition>
			<Route path="/"><Home /></Route>
			<Route path="/settings"><Settings /></Route>
			<Route path="/system"><System /></Route>
			<Route path="/cpu"><CPU /></Route>
			<Route path="/gpu"><GPU /></Route>
			<Route path="/ram"><RAM /></Route>
		</RouteTransition>
	</div>
</div>

<script lang="ts">
	import { Route, router } from "@baileyherbert/tinro"
	import Navigation from "../components/navigation.svelte"
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

	// Navigate to the home page on load (webview bug)
	onMount(() => {
		router.goto("/")
	})

	// Update hardware statistics
	onMount(() => {
		let observer: MutationObserver

		// @ts-ignore - Receive settings from the webview
		window.chrome.webview.addEventListener("message", (arg) => {
			if (arg.data.name === "settings") {
				console.log("New settings")

				$settings = JSON.parse(arg.data.content)
			}
		})

		// 60s date comparison
		const date = new Date()
		date.setSeconds(date.getSeconds() + 60)

		// Watch for changes in the DOM
		observer = new MutationObserver(() => {
			const input: HardwareInfo = JSON.parse(document.querySelector<HTMLInputElement>("#api").textContent)

			if (Object.keys(input).length !== 0) {
				// Shift the array if it's longer than 60
				if ($hardwareStatistics.cpu.temperature.seconds.max.length > 60) {
					$hardwareStatistics.cpu.temperature.seconds.max.shift()
					$hardwareStatistics.cpu.temperature.seconds.min.shift()
					$hardwareStatistics.cpu.temperature.seconds.value.shift()
					$hardwareStatistics.cpu.power.seconds.shift()
					$hardwareStatistics.cpu.load.seconds.shift()
					$hardwareStatistics.cpu.clock.seconds.max.shift()
					$hardwareStatistics.cpu.clock.seconds.min.shift()
					$hardwareStatistics.cpu.clock.seconds.value.shift()
					$hardwareStatistics.cpu.voltage.seconds.shift()

					$hardwareStatistics.ram.physicalUsage.seconds.shift()
					$hardwareStatistics.ram.virtualUsage.seconds.shift()

					$hardwareStatistics.gpu.temperature.seconds.max.shift()
					$hardwareStatistics.gpu.temperature.seconds.min.shift()
					$hardwareStatistics.gpu.temperature.seconds.value.shift()
					$hardwareStatistics.gpu.power.seconds.shift()
				}

				// Shift the array if it's longer than 60
				if ($hardwareStatistics.cpu.temperature.minutes.max.length > 60) {
					$hardwareStatistics.cpu.temperature.minutes.max.shift()
					$hardwareStatistics.cpu.temperature.minutes.min.shift()
					$hardwareStatistics.cpu.temperature.minutes.value.shift()
					$hardwareStatistics.cpu.power.minutes.shift()
					$hardwareStatistics.cpu.load.minutes.shift()
					$hardwareStatistics.cpu.clock.minutes.max.shift()
					$hardwareStatistics.cpu.clock.minutes.min.shift()
					$hardwareStatistics.cpu.clock.minutes.value.shift()
					$hardwareStatistics.cpu.voltage.minutes.shift()

					$hardwareStatistics.ram.physicalUsage.minutes.shift()
					$hardwareStatistics.ram.virtualUsage.minutes.shift()

					$hardwareStatistics.gpu.temperature.minutes.max.shift()
					$hardwareStatistics.gpu.temperature.minutes.min.shift()
					$hardwareStatistics.gpu.temperature.minutes.value.shift()
					$hardwareStatistics.gpu.power.minutes.shift()
				}

				// 60 second statistics
				$hardwareStatistics.cpu.temperature.seconds.max.push(Math.round(input.cpu.temperature.reduce((a, b) => a + b.max, 0) / input.cpu.temperature.length))
				$hardwareStatistics.cpu.temperature.seconds.min.push(Math.round(input.cpu.temperature.reduce((a, b) => a + b.min, 0) / input.cpu.temperature.length))
				$hardwareStatistics.cpu.temperature.seconds.value.push(Math.round(input.cpu.temperature.reduce((a, b) => a + b.value, 0) / input.cpu.temperature.length))
				$hardwareStatistics.cpu.power.seconds.push(Math.round(input.cpu.power.reduce((a, b) => a + b.value, 0)))
				$hardwareStatistics.cpu.load.seconds.push(Math.round(input.cpu.lastLoad))
				$hardwareStatistics.cpu.clock.seconds.max.push(Math.round(input.cpu.clock.reduce((a, b) => a + b.max, 0) / input.cpu.clock.length))
				$hardwareStatistics.cpu.clock.seconds.min.push(Math.round(input.cpu.clock.reduce((a, b) => a + b.min, 0) / input.cpu.clock.length))
				$hardwareStatistics.cpu.clock.seconds.value.push(Math.round(input.cpu.clock.reduce((a, b) => a + b.value, 0) / input.cpu.clock.length))
				$hardwareStatistics.cpu.voltage.seconds.push(parseFloat((input.cpu.voltage.reduce((a, b) => a + b.value, 0) / input.cpu.voltage.length).toFixed(2)))

				$hardwareStatistics.ram.physicalUsage.seconds.push(Math.round(input.ram.load[2].value))
				$hardwareStatistics.ram.virtualUsage.seconds.push(Math.round(input.ram.load[5].value))

				$hardwareStatistics.gpu.temperature.seconds.max.push(Math.round(input.gpu.temperature.reduce((a, b) => a + b.max, 0) / input.gpu.temperature.length))
				$hardwareStatistics.gpu.temperature.seconds.min.push(Math.round(input.gpu.temperature.reduce((a, b) => a + b.min, 0) / input.gpu.temperature.length))
				$hardwareStatistics.gpu.temperature.seconds.value.push(Math.round(input.gpu.temperature.reduce((a, b) => a + b.value, 0) / input.gpu.temperature.length))
				$hardwareStatistics.gpu.power.seconds.push(Math.round(input.gpu.power.reduce((a, b) => a + b.value, 0)))

				// 60 minute statistics
				if (date.getSeconds() === new Date().getSeconds()) {
					$hardwareStatistics.cpu.temperature.minutes.max.push(Math.round($hardwareStatistics.cpu.temperature.seconds.max.reduce((a, b) => a + b, 0) / $hardwareStatistics.cpu.temperature.seconds.max.length))
					$hardwareStatistics.cpu.temperature.minutes.min.push(Math.round($hardwareStatistics.cpu.temperature.seconds.min.reduce((a, b) => a + b, 0) / $hardwareStatistics.cpu.temperature.seconds.min.length))
					$hardwareStatistics.cpu.temperature.minutes.value.push(Math.round($hardwareStatistics.cpu.temperature.seconds.value.reduce((a, b) => a + b, 0) / $hardwareStatistics.cpu.temperature.seconds.value.length))
					$hardwareStatistics.cpu.power.minutes.push(Math.round($hardwareStatistics.cpu.power.seconds.reduce((a, b) => a + b, 0) / $hardwareStatistics.cpu.power.seconds.length))
					$hardwareStatistics.cpu.load.minutes.push(Math.round($hardwareStatistics.cpu.load.seconds.reduce((a, b) => a + b, 0) / $hardwareStatistics.cpu.load.seconds.length))
					$hardwareStatistics.cpu.clock.minutes.max.push(Math.round($hardwareStatistics.cpu.clock.seconds.max.reduce((a, b) => a + b, 0) / $hardwareStatistics.cpu.clock.seconds.max.length))
					$hardwareStatistics.cpu.clock.minutes.min.push(Math.round($hardwareStatistics.cpu.clock.seconds.min.reduce((a, b) => a + b, 0) / $hardwareStatistics.cpu.clock.seconds.min.length))
					$hardwareStatistics.cpu.clock.minutes.value.push(Math.round($hardwareStatistics.cpu.clock.seconds.value.reduce((a, b) => a + b, 0) / $hardwareStatistics.cpu.clock.seconds.value.length))
					$hardwareStatistics.cpu.voltage.minutes.push(parseFloat(($hardwareStatistics.cpu.voltage.seconds.reduce((a, b) => a + b, 0) / $hardwareStatistics.cpu.voltage.seconds.length).toFixed(2)))

					$hardwareStatistics.ram.physicalUsage.minutes.push(Math.round($hardwareStatistics.ram.physicalUsage.seconds.reduce((a, b) => a + b, 0) / $hardwareStatistics.ram.physicalUsage.seconds.length))
					$hardwareStatistics.ram.virtualUsage.minutes.push(Math.round($hardwareStatistics.ram.virtualUsage.seconds.reduce((a, b) => a + b, 0) / $hardwareStatistics.ram.virtualUsage.seconds.length))

					$hardwareStatistics.gpu.temperature.minutes.max.push(Math.round($hardwareStatistics.gpu.temperature.seconds.max.reduce((a, b) => a + b, 0) / $hardwareStatistics.gpu.temperature.seconds.max.length))
					$hardwareStatistics.gpu.temperature.minutes.min.push(Math.round($hardwareStatistics.gpu.temperature.seconds.min.reduce((a, b) => a + b, 0) / $hardwareStatistics.gpu.temperature.seconds.min.length))
					$hardwareStatistics.gpu.temperature.minutes.value.push(Math.round($hardwareStatistics.gpu.temperature.seconds.value.reduce((a, b) => a + b, 0) / $hardwareStatistics.gpu.temperature.seconds.value.length))
					$hardwareStatistics.gpu.power.minutes.push(Math.round($hardwareStatistics.gpu.power.seconds.reduce((a, b) => a + b, 0) / $hardwareStatistics.gpu.power.seconds.length))

					date.setSeconds(date.getSeconds() + 60)
				}

				// Update the sessionStorage
				const data: HardwareStatistics = {
					cpu: {
						temperature: {
							seconds: {
								max: $hardwareStatistics.cpu.temperature.seconds.max,
								min: $hardwareStatistics.cpu.temperature.seconds.min,
								value: $hardwareStatistics.cpu.temperature.seconds.value,
							},

							minutes: {
								max: $hardwareStatistics.cpu.temperature.minutes.max,
								min: $hardwareStatistics.cpu.temperature.minutes.min,
								value: $hardwareStatistics.cpu.temperature.minutes.value,
							},
						},
						power: {
							seconds: $hardwareStatistics.cpu.power.seconds,
							minutes: $hardwareStatistics.cpu.power.minutes,
						},
						load: {
							seconds: $hardwareStatistics.cpu.load.seconds,
							minutes: $hardwareStatistics.cpu.load.minutes,
						},
						clock: {
							seconds: {
								max: $hardwareStatistics.cpu.clock.seconds.max,
								min: $hardwareStatistics.cpu.clock.seconds.min,
								value: $hardwareStatistics.cpu.clock.seconds.value,
							},

							minutes: {
								max: $hardwareStatistics.cpu.clock.minutes.max,
								min: $hardwareStatistics.cpu.clock.minutes.min,
								value: $hardwareStatistics.cpu.clock.minutes.value,
							},
						},
						voltage: {
							seconds: $hardwareStatistics.cpu.voltage.seconds,
							minutes: $hardwareStatistics.cpu.voltage.minutes,
						},
					},

					ram: {
						physicalUsage: {
							seconds: $hardwareStatistics.ram.physicalUsage.seconds,
							minutes: $hardwareStatistics.ram.physicalUsage.minutes,
						},

						virtualUsage: {
							seconds: $hardwareStatistics.ram.virtualUsage.seconds,
							minutes: $hardwareStatistics.ram.virtualUsage.minutes,
						},
					},

					gpu: {
						temperature: {
							seconds: {
								max: $hardwareStatistics.gpu.temperature.seconds.max,
								min: $hardwareStatistics.gpu.temperature.seconds.min,
								value: $hardwareStatistics.gpu.temperature.seconds.value,
							},

							minutes: {
								max: $hardwareStatistics.gpu.temperature.minutes.max,
								min: $hardwareStatistics.gpu.temperature.minutes.min,
								value: $hardwareStatistics.gpu.temperature.minutes.value,
							},
						},
						power: {
							seconds: $hardwareStatistics.gpu.power.seconds,
							minutes: $hardwareStatistics.gpu.power.minutes,
						},
					},
				}

				setHardwareStatistics(data)
			}
		})

		// Start observing the target
		observer.observe(document.querySelector("#api"), {
			attributes: true,
			childList: true,
			characterData: true,
		})
	})
</script>
