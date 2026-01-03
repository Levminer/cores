<ModularDialog title={"Cores Mobile"} description={"You can also use Cores on your iOS or Android device."}>
	{#snippet openButton()}
		{@render button()}
	{/snippet}

	<div class="flex w-full flex-row space-x-36">
		<div class="flex w-1/2 flex-col items-center justify-center rounded-xl border-2 border-white p-3">
			<img width="160" height="160" alt="qrcode" src="https://cdn.levminer.com/cores/ios.png" class="m-1 rounded-2xl" />
			<button
				onclick={() => {
					openUrl("https://link.levminer.com/cores-ios?utm_source=app_dialog")
				}}
				class="smallButton mt-3 w-full">App Store</button
			>
		</div>

		<div class="flex w-1/2 flex-col items-center justify-center rounded-xl border-2 border-white p-3">
			<img width="160" height="160" alt="qrcode" src="https://cdn.levminer.com/cores/android.png" class="m-1 rounded-2xl" />
			<button
				onclick={() => {
					openUrl("https://link.levminer.com/crs-android?utm_source=app_dialog")
				}}
				class="smallButton mt-3 w-full">Play Store</button
			>
		</div>
	</div>
</ModularDialog>

<script lang="ts">
	import { ModularDialog } from ".."
	import type { Snippet } from "svelte"
	import { open as openUrl } from "@tauri-apps/plugin-shell"

	interface Props {
		button: Snippet
	}

	let { button }: Props = $props()
</script>
