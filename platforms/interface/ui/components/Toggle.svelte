<div class="flex items-center space-x-4">
	<Label.Root for="marketing" class="text-xl">{checked ? "On" : "Off"}</Label.Root>
	<Switch.Root
		bind:checked
		onCheckedChange={onChange}
		id="marketing"
		class="data-[state=unchecked]:shadow-mini-inset peer inline-flex h-[28px] min-h-[28px] w-[48px] shrink-0 cursor-pointer items-center rounded-full border-2 border-white transition-colors disabled:cursor-not-allowed disabled:opacity-50 data-[state=checked]:bg-white data-[state=unchecked]:bg-transparent"
	>
		<Switch.Thumb
			class="data-[state=unchecked]:shadow-mini pointer-events-none relative left-1 block size-[16px] shrink-0 rounded-full bg-white transition-transform data-[state=checked]:translate-x-[21px] data-[state=unchecked]:translate-x-0 data-[state=checked]:bg-black"
		/>
	</Switch.Root>
</div>

<script lang="ts">
	import { Switch, Label } from "bits-ui"

	export let checked = false
	export let onChange = () => {}
</script>
