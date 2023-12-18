<button on:click class="mx-auto flex flex-1">
	<a
		class="m-1 transparent-800 group inline-flex flex-shrink-0 flex-grow flex-col items-center justify-center rounded-full p-1 px-5 text-gray-200 duration-200 ease-in-out"
	>
		<p class="text-xl">{selected ? "60m" : "60s"}</p>
	</a>
</button>

<script lang="ts">
	export let selected = false
</script>
