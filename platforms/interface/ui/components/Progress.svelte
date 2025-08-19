<Progress.Root {value} max={100} class="transparent-900 shadow-mini-inset relative h-[10px] w-[95%] overflow-hidden rounded-full">
	{#if value > 0}
		<div
			class="shadow-mini-inset h-full w-full flex-1 rounded-full transition-all duration-1000 ease-in-out"
			style={`transform: translateX(-${100 - (100 * (Math.round(value) ?? 0)) / 100}%); background-color: ${$localSettings.colors?.min ?? "#35cbfd"};`}
		></div>
	{/if}
</Progress.Root>

<script lang="ts">
	import { Progress } from "bits-ui"
	import { localSettings } from "../stores/localSettings"

	interface Props {
		value: number
	}

	let { value }: Props = $props()
</script>
