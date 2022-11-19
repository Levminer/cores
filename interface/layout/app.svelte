<div class="flex h-screen">
	<Navigation />

	<div class="w-full overflow-hidden overflow-y-scroll">
		<div class="top" />

		<RouteTransition>
			<Route path="/"><Home /></Route>
		</RouteTransition>
	</div>
</div>

<script lang="ts">
	import { Route, router } from "tinro"
	import Navigation from "../components/navigation.svelte"
	import Home from "../pages/home.svelte"
	import RouteTransition from "../components/routeTransition.svelte"
</script>
