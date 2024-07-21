<Accordion.Root class="w-full">
	<Accordion.Item value={id} class="group ">
		<Accordion.Header>
			<Accordion.Trigger
				class="transparent-900 flex w-full items-center justify-between p-8 transition-all [&[data-state=closed]]:rounded-xl [&[data-state=open]>span>svg]:rotate-180 [&[data-state=open]]:rounded-t-xl"
			>
				<div class="flex items-center justify-center gap-5">
					<slot name="header" />
				</div>
				<span class="inline-flex size-8 items-center justify-center rounded-[8px] bg-transparent transition-all">
					<ChevronDown class="transition-all duration-200" />
				</span>
			</Accordion.Trigger>
		</Accordion.Header>
		<Accordion.Content transition={slide} transitionConfig={{ duration: 200 }} class="transparent-900 rounded-b-xl pb-[25px]">
			<slot name="content" />
		</Accordion.Content>
	</Accordion.Item>
</Accordion.Root>

<script lang="ts">
	import { Accordion } from "bits-ui"
	import { slide } from "svelte/transition"
	import { ChevronDown } from "lucide-svelte"

	let id = crypto.randomUUID()
</script>
