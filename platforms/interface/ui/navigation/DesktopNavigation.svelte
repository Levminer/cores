<div class="transparent-900 flex h-full flex-col justify-between overflow-visible rounded-tr-xl shadow-2xl">
	<div class="flex flex-col space-y-2 pt-2">
		<a href="/home" class={pathname === "/home" ? "selectedMenuButton menuButton" : "menuButton"}>
			<Home class="h-6 w-6" />
		</a>

		<a href="/cpu" class={pathname === "/cpu" ? "selectedMenuButton menuButton" : "menuButton"}>
			<Cpu class="h-6 w-6" />
		</a>

		<a href="/ram" class={pathname === "/ram" ? "selectedMenuButton menuButton" : "menuButton"}>
			<Memory height={24} width={24} />
		</a>

		<a href="/gpu" class={pathname === "/gpu" ? "selectedMenuButton menuButton" : "menuButton"}>
			<GpuCard height={24} width={24} />
		</a>

		<a href="/storage" class={pathname === "/storage" ? "selectedMenuButton menuButton" : "menuButton"}>
			<HardDrive class="h-6 w-6" />
		</a>

		<a href="/network" class={pathname === "/network" ? "selectedMenuButton menuButton" : "menuButton"}>
			<Network class="h-6 w-6" />
		</a>

		<a href="/system" class={pathname === "/system" ? "selectedMenuButton menuButton" : "menuButton"}>
			<PcDisplay width={24} height={24} />
		</a>
	</div>

	<div class="flex flex-col space-y-2 pb-2">
		<a href="/connections" class={pathname === "/connections" ? "selectedMenuButton menuButton" : "menuButton"}>
			<MonitorSmartphone class="h-6 w-6" />
		</a>
		<a href="/settings" class={pathname === "/settings" ? "selectedMenuButton menuButton" : "menuButton"}>
			<Settings class="h-6 w-6" />
		</a>
	</div>
</div>

<script lang="ts">
	import { Cpu, HardDrive, Home, MonitorSmartphone, Network, Settings } from "lucide-svelte"
	import { GpuCard, Memory, PcDisplay } from "svelte-bootstrap-icons"

	interface Props {
		pathname: string
	}

	let { pathname }: Props = $props()
</script>
