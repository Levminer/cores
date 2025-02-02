<Progress.Root {value} max={100} class="transparent-900 shadow-mini-inset relative h-[10px] w-[95%] overflow-hidden rounded-full">
	<div
		class="shadow-mini-inset h-full w-full flex-1 rounded-full bg-[#35cbfd] transition-all duration-1000 ease-in-out"
		style={`transform: translateX(-${100 - (100 * (Math.round(value) ?? 0)) / 100}%)`}
	/>
</Progress.Root>

<script lang="ts">
	import { Progress } from "bits-ui"

	export let value
</script>
