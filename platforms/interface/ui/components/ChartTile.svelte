<div class="transparent-800 rounded-xl p-8 sm:p-4">
	<div class="flex items-baseline justify-between">
		<div class="mb-5 flex items-center gap-3">
			<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
				{@render icon()}
			</div>
			<h2><span class="line-clamp-1">{item}</span> {title}</h2>
		</div>
		<div class="flex flex-row gap-3">
			{#if import.meta.env.VITE_CORES_MODE === "host"}
				<SaveDataButton
					props={{
						id: props.id,
						statistics: props.statistics,
					}}
				/>
			{/if}
			<ToggleButton selected={minutes} onclick={() => (minutes = !minutes)} />
		</div>
	</div>

	<div>
		<LineChart
			props={{
				...props,
			}}
		/>
	</div>
</div>

<script lang="ts">
	import LineChart from "../charts/LineChart.svelte"
	import SaveDataButton from "./SaveDataButton.svelte"
	import ToggleButton from "./ToggleButton.svelte"
	import type { Snippet } from "svelte"

	interface Props {
		title: string
		item: string
		icon: Snippet
		minutes: boolean
		props: {
			id: string
			statistics: {
				label: string
				data: number[]
				fill?: boolean
				color?: "min" | "max" | "current" | "yellow" | "orange"
			}[]
			unit: string
			time: string
			min?: number
			max?: number
			step?: number
			timestamp?: string[]
		}
	}

	let { props, title, item, minutes = $bindable(), icon }: Props = $props()
</script>
