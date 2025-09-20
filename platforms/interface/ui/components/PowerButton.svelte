<ModularDialog title={"Power"} description={"Left your computer on? You can turn it off remotely."}>
	{#snippet openButton()}
		<Dialog.Trigger class="button">
			<Power />
			Power
		</Dialog.Trigger>
	{/snippet}
	<div class="w-full space-y-5">
		<div class="flex flex-col gap-1 p-3">
			<div>
				<Dialog.Close
					onclick={() => {
						action("sleep")
					}}
					class="smallButton mt-3 w-full"
				>
					<Moon />
					Sleep
				</Dialog.Close>
			</div>
			<div>
				<Dialog.Close
					onclick={() => {
						action("shutdown")
					}}
					class="smallButton mt-3 w-full"
				>
					<Power />
					Shut down
				</Dialog.Close>
			</div>
			<div>
				<Dialog.Close
					onclick={() => {
						action("restart")
					}}
					class="smallButton mt-3 w-full"
				>
					<RotateCcw />
					Restart
				</Dialog.Close>
			</div>
		</div>
	</div>
</ModularDialog>

<script lang="ts">
	import { ModularDialog, settings } from "ui"
	import { Dialog } from "bits-ui"
	import { Moon, Power, RotateCcw } from "lucide-svelte"

	let { action = (type: string) => {} } = $props()
</script>
