<div class="flex h-screen">
	<Navigation />

	<div class="w-full overflow-hidden overflow-y-scroll">
		<BuildNumber />

		<div class="top" />

		<RouteTransition>
			<Route path="/"><Home /></Route>
			<Route path="/settings"><Settings /></Route>
			<Route path="/system"><System /></Route>
			<Route path="/cpu"><CPU /></Route>
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
	import { onMount } from "svelte"
	import { hardwareStatistics, setHardwareStatistics } from "../stores/hardwareStatistics"

	// Navigate to the home page on load (webview bug)
	onMount(() => {
		router.goto("/")
	})

	// Update hardware statistics
	onMount(() => {
		let observer: MutationObserver

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
				}

				// CPU temperatures (60s)
				$hardwareStatistics.cpu.temperature.max.push(Math.round(input.cpu.temperature.reduce((a, b) => a + b.max, 0) / input.cpu.temperature.length))
				$hardwareStatistics.cpu.temperature.min.push(Math.round(input.cpu.temperature.reduce((a, b) => a + b.min, 0) / input.cpu.temperature.length))
				$hardwareStatistics.cpu.temperature.value.push(Math.round(input.cpu.temperature.reduce((a, b) => a + b.value, 0) / input.cpu.temperature.length))

				// CPU power usage (60s)
				$hardwareStatistics.cpu.power.push(Math.round(input.cpu.power.reduce((a, b) => a + b.value, 0)))

				// AVG CPU threads load (60s)
				$hardwareStatistics.cpu.load.push(Math.round(input.cpu.lastLoad))

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
