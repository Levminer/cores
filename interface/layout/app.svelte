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
	import { Route, router } from "tinro"
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
	import { setSettings, settings } from "../stores/settings"

	// Navigate to the home page on load (webview bug)
	onMount(() => {
		router.goto("/")
	})

	// Update hardware statistics
	onMount(() => {
		let observer: MutationObserver

		// @ts-ignore
		window.chrome.webview.addEventListener("message", (arg) => {
			if (arg.data.name === "settings") {
				console.log("New settings")

				$settings = JSON.parse(arg.data.content)
			}
		})

		// Watch for changes in the DOM
		observer = new MutationObserver(() => {
			const input: HardwareInfo = JSON.parse(document.querySelector<HTMLInputElement>("#api").textContent)

			if (Object.keys(input).length !== 0) {
				// Shift the array if it's longer than 60s
				if ($hardwareStatistics.cpu.temperature.max.length > 60) {
					$hardwareStatistics.cpu.temperature.max.shift()
					$hardwareStatistics.cpu.temperature.min.shift()
					$hardwareStatistics.cpu.temperature.value.shift()

					$hardwareStatistics.cpu.power.shift()

					$hardwareStatistics.cpu.load.shift()

					$hardwareStatistics.ram.usage.physical.shift()
					$hardwareStatistics.ram.usage.virtual.shift()

					$hardwareStatistics.gpu.temperature.max.shift()
					$hardwareStatistics.gpu.temperature.min.shift()
					$hardwareStatistics.gpu.temperature.value.shift()

					$hardwareStatistics.gpu.power.shift()
				}

				// CPU temperatures (60s)
				$hardwareStatistics.cpu.temperature.max.push(Math.round(input.cpu.temperature.reduce((a, b) => a + b.max, 0) / input.cpu.temperature.length))
				$hardwareStatistics.cpu.temperature.min.push(Math.round(input.cpu.temperature.reduce((a, b) => a + b.min, 0) / input.cpu.temperature.length))
				$hardwareStatistics.cpu.temperature.value.push(Math.round(input.cpu.temperature.reduce((a, b) => a + b.value, 0) / input.cpu.temperature.length))

				// CPU power usage (60s)
				$hardwareStatistics.cpu.power.push(Math.round(input.cpu.power.reduce((a, b) => a + b.value, 0)))

				// AVG CPU threads load (60s)
				$hardwareStatistics.cpu.load.push(Math.round(input.cpu.lastLoad))

				// RAM usage (60s)
				$hardwareStatistics.ram.usage.physical.push(Math.round(input.ram.load[2].value))
				$hardwareStatistics.ram.usage.virtual.push(Math.round(input.ram.load[5].value))

				// GPU temperatures (60s)
				$hardwareStatistics.gpu.temperature.max.push(Math.round(input.gpu.temperature.reduce((a, b) => a + b.max, 0) / input.gpu.temperature.length))
				$hardwareStatistics.gpu.temperature.min.push(Math.round(input.gpu.temperature.reduce((a, b) => a + b.min, 0) / input.gpu.temperature.length))
				$hardwareStatistics.gpu.temperature.value.push(Math.round(input.gpu.temperature.reduce((a, b) => a + b.value, 0) / input.gpu.temperature.length))

				// GPU power usage (60s)
				$hardwareStatistics.gpu.power.push(Math.round(input.gpu.power.reduce((a, b) => a + b.value, 0)))

				// Update the sessionStorage
				const data: HardwareStatistics = {
					cpu: {
						temperature: {
							max: $hardwareStatistics.cpu.temperature.max,
							min: $hardwareStatistics.cpu.temperature.min,
							value: $hardwareStatistics.cpu.temperature.value,
						},
						power: $hardwareStatistics.cpu.power,
						load: $hardwareStatistics.cpu.load,
					},

					ram: {
						usage: {
							physical: $hardwareStatistics.ram.usage.physical,
							virtual: $hardwareStatistics.ram.usage.virtual,
						},
					},

					gpu: {
						temperature: {
							max: $hardwareStatistics.gpu.temperature.max,
							min: $hardwareStatistics.gpu.temperature.min,
							value: $hardwareStatistics.gpu.temperature.value,
						},
						power: $hardwareStatistics.gpu.power,
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
