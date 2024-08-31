<Dialog.Root bind:open>
	<slot name="openButton" />

	<Dialog.Portal>
		<Dialog.Overlay transition={fade} transitionConfig={{ duration: 150 }} class="fixed inset-0 z-50 bg-black/70" />
		<Dialog.Content
			transition={flyAndScale}
			class="fixed left-[50%] top-[50%] z-50 w-full max-w-xl translate-x-[-50%] translate-y-[-50%] rounded-2xl border border-gray-600 bg-gray-800 p-5 shadow-md outline-none sm:max-w-[490px] md:w-full"
		>
			<Dialog.Title class="-mt-1.5 flex w-full items-center justify-center text-lg font-semibold tracking-tight">{title}</Dialog.Title>
			<Separator.Root class="-mx-5 mb-6 mt-5 block h-px bg-gray-600" />
			<Dialog.Description class="text-sm text-gray-200">
				{description}
			</Dialog.Description>
			<div class="flex flex-col items-start gap-1 pb-11 pt-7">
				<slot />
			</div>
			<div class="flex w-full justify-between">
				<slot name="deleteButton" />
				<slot name="confirmButton" />
			</div>

			<Dialog.Close
				class="focus-visible:ring-foreground focus-visible:ring-offset-background active:scale-98 absolute right-5 top-5 rounded-md focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-offset-2"
			>
				<div>
					<X class="text-foreground size-5" />
					<span class="sr-only">Close</span>
				</div>
			</Dialog.Close>
		</Dialog.Content>
	</Dialog.Portal>
</Dialog.Root>

<script lang="ts">
	import { Dialog, Separator } from "bits-ui"
	import { flyAndScale } from "../utils/transitions.ts"
	import { X } from "lucide-svelte"
	import { fade } from "svelte/transition"

	export let title = ""
	export let description = ""
	export let open = false
</script>
