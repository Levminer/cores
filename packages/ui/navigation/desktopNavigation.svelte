<div class="flex h-full flex-col justify-between overflow-visible p-2">
	<div class="flex flex-col space-y-3">
		<a href="/home" use:active data-active-class="selectedMenuButton" class="menuButton">
			<Home class="h-5 w-5" />
		</a>

		<a href="/cpu" use:active data-active-class="selectedMenuButton" class="menuButton">
			<Cpu class="h-5 w-5" />
		</a>

		<a href="/ram" use:active data-active-class="selectedMenuButton" class="menuButton">
			<Memory height={20} width={20} />
		</a>

		<a href="/gpu" use:active data-active-class="selectedMenuButton" class="menuButton">
			<GpuCard height={20} width={20} />
		</a>

		<a href="/storage" use:active data-active-class="selectedMenuButton" class="menuButton">
			<HardDrive class="h-5 w-5" />
		</a>

		<a href="/network" use:active data-active-class="selectedMenuButton" class="menuButton">
			<Network class="h-5 w-5" />
		</a>

		<a href="/system" use:active data-active-class="selectedMenuButton" class="menuButton">
			<PcDisplay width={20} height={20} />
		</a>
	</div>

	<div class="flex flex-col space-y-3">
		<a href="/connections" use:active data-active-class="selectedMenuButton" class="menuButton">
			<MonitorSmartphone class="h-5 w-5" />
		</a>
		<a href="/settings" use:active data-active-class="selectedMenuButton" class="menuButton">
			<Settings class="h-5 w-5" />
		</a>
	</div>
</div>

<script lang="ts">
	import { active } from "@baileyherbert/tinro"
	import { Cpu, HardDrive, Home, MonitorSmartphone, Network, Settings } from "lucide-svelte"
	import { GpuCard, Memory, PcDisplay } from "svelte-bootstrap-icons"
</script>
