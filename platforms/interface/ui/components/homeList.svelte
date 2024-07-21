<div class="mx-10 flex-row justify-evenly space-y-5 pb-10 pt-10 sm:mx-3">
	<Accordion>
		<svelte:fragment slot="header">
			<div class="flex items-center gap-3">
				<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
					<Cpu />
				</div>
				<h2>CPU</h2>
			</div>
			<h3>{$hardwareInfo.cpu.name}</h3>
		</svelte:fragment>
		<svelte:fragment slot="content">
			<div class="flex flex-row items-center px-8">
				<div class="w-1/3">
					{#each $hardwareInfo.cpu.load as item, i}
						<div>
							<div class="flex w-[95%] flex-row justify-between">
								<p class="text-sm">{item.name?.replaceAll("CPU", "")}</p>
								<p class="text-sm text-[#969696]">{Math.round(item.value)}%</p>
							</div>
							<Progress value={item.value} />
						</div>
					{/each}
				</div>
				<div class="w-1/3">
					{#each $hardwareInfo.cpu.load as item, i}
						<div>
							<div class="flex w-[95%] flex-row justify-between">
								<p class="text-sm">{item.name?.replaceAll("CPU", "")}</p>
								<p class="text-sm text-[#969696]">{Math.round(item.min)}%</p>
							</div>
							<Progress value={item.min} />
						</div>
					{/each}
				</div>
				<div class="w-1/3">
					{#each $hardwareInfo.cpu.load as item, i}
						<div>
							<div class="flex w-[95%] flex-row justify-between">
								<p class="text-sm">{item.name?.replaceAll("CPU", "")}</p>
								<p class="text-sm text-[#969696]">{Math.round(item.max)}%</p>
							</div>
							<Progress value={item.max} />
						</div>
					{/each}
				</div>
			</div>
		</svelte:fragment>
	</Accordion>

	<Accordion>
		<svelte:fragment slot="header">
			<div class="flex items-center gap-3">
				<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
					<Memory height={24} width={24} />
				</div>
				<h2>RAM</h2>
			</div>
			<h3>{$hardwareInfo.ram.info[0]?.manufacturerName ?? "Generic Memory"}</h3>
		</svelte:fragment>
		<svelte:fragment slot="content">
			<h2>content</h2>
		</svelte:fragment>
	</Accordion>

	<Accordion>
		<svelte:fragment slot="header">
			<div class="flex items-center gap-3">
				<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
					<GpuCard height={24} width={24} />
				</div>
				<h2>GPU</h2>
			</div>
			<h3>{$hardwareInfo.gpu.name}</h3>
		</svelte:fragment>
		<svelte:fragment slot="content">
			<h2>content</h2>
		</svelte:fragment>
	</Accordion>

	<Accordion>
		<svelte:fragment slot="header">
			<div class="flex items-center gap-3">
				<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
					<HardDrive />
				</div>
				<h2>Drive</h2>
			</div>
			<h3>{$hardwareInfo.system.storage.disks[0]?.name ?? "Generic Drive"}</h3>
		</svelte:fragment>
		<svelte:fragment slot="content">
			<h2>content</h2>
		</svelte:fragment>
	</Accordion>

	<Accordion>
		<svelte:fragment slot="header">
			<div class="flex items-center gap-3">
				<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
					<Network />
				</div>
				<h2>Interface</h2>
			</div>
			<h3>{$hardwareInfo.system.network.interfaces[0]?.name ?? "Generic Interface"}</h3>
		</svelte:fragment>
		<svelte:fragment slot="content">
			<h2>content</h2>
		</svelte:fragment>
	</Accordion>
</div>

<script lang="ts">
	import { GpuCard, Memory } from "svelte-bootstrap-icons"
	import { hardwareInfo } from "../stores/hardwareInfo.ts"
	import Accordion from "./accordion.svelte"
	import {
		Gauge,
		CircuitBoard,
		Clock,
		Fan,
		HardDrive,
		Monitor,
		Network,
		Plug,
		Thermometer,
		Zap,
		Cpu,
		Battery,
		LayoutGrid,
		LayoutList,
	} from "lucide-svelte"
	import Progress from "./progress.svelte"
</script>
