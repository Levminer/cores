<ModularDialog title={"Feedback"} description={"Feedback is always welcome! Report issues or request features."}>
	<slot slot="openButton">
		<Dialog.Trigger class="button">
			<Megaphone />
			Feedback
		</Dialog.Trigger>
	</slot>
	<div class="w-full">
		<Tabs.Root value="file" class="">
			<Tabs.List class="grid w-full grid-cols-2 gap-1 rounded-xl border-2 p-1 text-sm font-semibold leading-[0.01em]">
				<Tabs.Trigger
					value="file"
					class="h-10 rounded-xl bg-transparent py-2 text-base data-[state=active]:bg-white data-[state=active]:text-black"
				>
					GitHub
				</Tabs.Trigger>
				<Tabs.Trigger
					value="image"
					class="h-10 rounded-xl bg-transparent py-2 text-base data-[state=active]:bg-white data-[state=active]:text-black"
				>
					Email
				</Tabs.Trigger>
			</Tabs.List>
			<Tabs.Content value="file" class="pt-10">
				<div>
					<h5 class="mb-3">Report Issues or Request Features</h5>
					<button
						on:click={() => {
							open("https://github.com/levminer/cores/issues")
						}}
						class="button w-full">Open GitHub</button
					>
				</div>
			</Tabs.Content>
			<Tabs.Content value="image" class="pt-10">
				<div>
					<h5 class="mb-3">Send an email to support@coresmonitor.com</h5>
					<button
						on:click={() => {
							open("mailto:feedback@coresmonitor.com")
						}}
						class="button w-full">Send Email</button
					>
				</div>
			</Tabs.Content>
		</Tabs.Root>
	</div>
</ModularDialog>

<script lang="ts">
	import { Dialog, Tabs } from "bits-ui"
	import ModularDialog from "./modularDialog.svelte"
	import { settings } from "../stores/settings.ts"
	import { Megaphone, Github } from "lucide-svelte"
	import { open } from "@tauri-apps/plugin-shell"
</script>
