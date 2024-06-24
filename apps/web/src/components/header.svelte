<nav class="sticky start-0 top-0 z-20 w-full border-b border-gray-600 bg-gray-900 px-20 sm:static sm:px-0">
	<div class="mx-auto flex max-w-screen-xl flex-wrap items-center justify-between p-3">
		<a href="/#cores" class="flex items-center space-x-3">
			<img class="h-8 w-8" src={"/favicon.ico"} alt="Cores logo" />
			<span class="self-center whitespace-nowrap text-2xl font-semibold text-white">Cores</span>
		</a>
		<div class="flex space-x-2 md:order-2">
			<button
				on:click={() => {
					const settings = JSON.parse(localStorage.getItem("settings") || "{}")

					if (settings.connectionCode !== undefined && settings.connectionCode !== "") {
						goto("/home")
					} else {
						goto("/get-started")
					}
				}}
				class="bg-cores-current hover:text-cores-current rounded-2xl px-4 py-2 text-xl font-semibold duration-200 ease-in-out hover:bg-white hover:from-transparent hover:to-transparent"
			>
				Launch app
			</button>
		</div>
		<div class="w-full items-center justify-between md:order-1 md:flex md:w-auto">
			<ul
				class="mt-4 flex flex-col rounded-lg border border-gray-700 p-4 text-lg font-medium md:mt-0 md:flex-row md:space-x-8 md:border-0 md:p-0"
			>
				<li>
					<a href="/#features" class="hover:text-cores-current block rounded px-3 py-2 font-semibold text-white duration-200 ease-in md:p-0"
						>Features</a
					>
				</li>
				<li>
					<a href="/#downloads" class="hover:text-cores-current block rounded px-3 py-2 font-semibold text-white duration-200 ease-in md:p-0"
						>Download</a
					>
				</li>
				<li>
					<a href="/#pricing" class="hover:text-cores-current block rounded px-3 py-2 font-semibold text-white duration-200 ease-in md:p-0"
						>Pricing</a
					>
				</li>
			</ul>
		</div>
	</div>
</nav>

<script lang="ts">
	import { goto } from "$app/navigation"
</script>
