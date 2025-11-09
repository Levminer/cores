<button {onclick} class="mx-auto flex flex-1">
	<span
		class="transparent-800 group m-1 inline-flex flex-shrink-0 flex-grow flex-col items-center justify-center rounded-lg p-1 px-3 text-gray-200 duration-200 ease-in-out"
	>
		<p class="text-xl">{selected ? "60m" : "60s"}</p>
	</span>
</button>

<script lang="ts">
	let { selected = false, onclick }: { selected: boolean; onclick: () => void } = $props()
</script>
