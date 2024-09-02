<div
	class="fixed bottom-0 left-1/2 z-50 h-20 w-full max-w-full -translate-x-1/2 rounded-none border border-gray-600 bg-gray-700 duration-200 ease-in-out md:bottom-4 md:h-16 md:max-w-2xl md:rounded-full"
>
	<div class="no-scrollbar mx-auto flex h-full max-w-2xl flex-1 overflow-x-auto">
		<a
			aria-label="Home"
			href="/home"
			class="group inline-flex flex-shrink-0 flex-grow flex-col items-center justify-center rounded-s-full px-5 text-gray-200 duration-200 ease-in-out hover:bg-gray-800 hover:text-white sm:rounded-none"
		>
			<Home />
		</a>
		<a
			aria-label="CPU"
			href="/cpu"
			class="group inline-flex flex-shrink-0 flex-grow flex-col items-center justify-center px-5 text-gray-200 duration-200 ease-in-out hover:bg-gray-800 hover:text-white"
		>
			<Cpu />
		</a>

		<a
			aria-label="RAM"
			href="/ram"
			class="group inline-flex flex-shrink-0 flex-grow flex-col items-center justify-center px-5 text-gray-200 duration-200 ease-in-out hover:bg-gray-800 hover:text-white"
		>
			<Memory height="24" width="24" />
		</a>

		<a
			aria-label="GPU"
			href="/gpu"
			class="group inline-flex flex-shrink-0 flex-grow flex-col items-center justify-center px-5 text-gray-200 duration-200 ease-in-out hover:bg-gray-800 hover:text-white"
		>
			<GpuCard height="24" width="24" />
		</a>

		<a
			aria-label="Storage"
			href="/storage"
			class="group inline-flex flex-shrink-0 flex-grow flex-col items-center justify-center px-5 text-gray-200 duration-200 ease-in-out hover:bg-gray-800 hover:text-white"
		>
			<HardDrive />
		</a>

		<a
			aria-label="Network"
			href="/network"
			class="group inline-flex flex-shrink-0 flex-grow flex-col items-center justify-center px-5 text-gray-200 duration-200 ease-in-out hover:bg-gray-800 hover:text-white"
		>
			<Network />
		</a>

		<a
			aria-label="Network"
			href="/system"
			class="group inline-flex flex-shrink-0 flex-grow flex-col items-center justify-center px-5 text-gray-200 duration-200 ease-in-out hover:bg-gray-800 hover:text-white"
		>
			<PcDisplay height="24" width="24" />
		</a>

		<a
			aria-label="Connections"
			href="/connections"
			class="group inline-flex flex-shrink-0 flex-grow flex-col items-center justify-center px-5 text-gray-200 duration-200 ease-in-out hover:bg-gray-800 hover:text-white"
		>
			<MonitorSmartphone />
		</a>

		<a
			aria-label="Settings"
			href="/settings"
			class="group inline-flex flex-shrink-0 flex-grow flex-col items-center justify-center rounded-e-full px-5 text-gray-200 duration-200 ease-in-out hover:bg-gray-800 hover:text-white sm:rounded-none"
		>
			<Settings />
		</a>
	</div>
</div>

<script lang="ts">
	import { Cpu, HardDrive, Home, MonitorSmartphone, Network, Settings } from "lucide-svelte"
	import { GpuCard, Memory, PcDisplay } from "svelte-bootstrap-icons"
</script>
