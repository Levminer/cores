<div class="transparent-900 flex h-full flex-col justify-between overflow-visible rounded-tr-xl shadow-2xl">
	<div class="flex flex-col space-y-2 pt-2">
		<a href="/home" use:active data-active-class="selectedMenuButton" class="menuButton">
			<Home class="h-6 w-6" />
		</a>

		<a href="/cpu" use:active data-active-class="selectedMenuButton" class="menuButton">
			<Cpu class="h-6 w-6" />
		</a>

		<a href="/ram" use:active data-active-class="selectedMenuButton" class="menuButton">
			<Memory height={24} width={24} />
		</a>

		<a href="/gpu" use:active data-active-class="selectedMenuButton" class="menuButton">
			<GpuCard height={24} width={24} />
		</a>

		<a href="/storage" use:active data-active-class="selectedMenuButton" class="menuButton">
			<HardDrive class="h-6 w-6" />
		</a>

		<a href="/network" use:active data-active-class="selectedMenuButton" class="menuButton">
			<Network class="h-6 w-6" />
		</a>

		<a href="/system" use:active data-active-class="selectedMenuButton" class="menuButton">
			<PcDisplay width={24} height={24} />
		</a>
	</div>

	<div class="flex flex-col space-y-2 pb-2">
		<a href="/connections" use:active data-active-class="selectedMenuButton" class="menuButton">
			<MonitorSmartphone class="h-6 w-6" />
		</a>
		<a href="/settings" use:active data-active-class="selectedMenuButton" class="menuButton">
			<Settings class="h-6 w-6" />
		</a>
	</div>
</div>

<script lang="ts">
	import { active } from "@baileyherbert/tinro"
	import { Cpu, HardDrive, Home, MonitorSmartphone, Network, Settings } from "lucide-svelte"
	import { GpuCard, Memory, PcDisplay } from "svelte-bootstrap-icons"
</script>
