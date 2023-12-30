<header class="mt-10 rounded-xl bg-[#110A30] text-white">
	<div class="container mx-auto flex flex-col flex-wrap items-center space-y-6 p-5 md:flex-row md:space-y-0">
		<a class="flex">
			<img class="h-8 w-8" src={"/favicon.ico"} alt="Cores logo" />
			<span class="ml-5 text-xl text-white">Cores</span>
		</a>
		<nav class="flex flex-wrap items-center justify-center text-xl md:ml-auto md:mr-auto font-semibold">
			<a href="#features" class="mr-5 hover:text-white">Features</a>
			<a href="#pricing" class="mr-5 hover:text-white">Pricing</a>
			<a href="/home" class="mr-5 hover:text-white">Web app</a>
		</nav>
		<div class="flex space-x-2">
			<a href="/account" target="_blank" rel="noreferrer" class="smallButton text-lg">
				<CircleUserRound />
				Account
			</a>
		</div>
	</div>
</header>

<script lang="ts">
	import { CircleUserRound, Github, Rocket } from "lucide-svelte"
</script>
