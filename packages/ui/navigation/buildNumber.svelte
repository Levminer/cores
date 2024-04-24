<div class="build sticky top-0 z-20 w-full bg-black p-1 text-center">
	<p class="buildContent text-lg font-bold">You are running a pre release version of Cores - Version {build.version} - Build {build.number}</p>
	{#if build.version === "0.17.1"}
		<div class="bg-red-900 text-white text-2xl">This version of Cores is no longer supported. Please delete this version and migrate to the new app. <a class="underline" href="https://github.com/Levminer/cores/releases/latest">Download</a></div>
	{/if}
	
</div>

<script lang="ts">
	import build from "../../../build.json"
</script>
