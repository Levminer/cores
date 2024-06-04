<DropdownMenu.Root>
	<DropdownMenu.Trigger
		class="hover:bg-cores-current inline-flex items-center justify-center gap-2 rounded-full bg-gray-700 px-3 py-2 text-xl font-semibold duration-200 ease-in-out"
	>
		<Power size="30" />
		<p class="hidden md:block">Power</p>
	</DropdownMenu.Trigger>
	<DropdownMenu.Content
		class="bg-background shadow-popover w-full max-w-[229px] rounded-xl bg-gray-700 p-1"
		transition={flyAndScale}
		sideOffset={8}
	>
		<DropdownMenu.Item
			on:click={() => {
				connect("sleep")
			}}
			class="flex cursor-pointer select-none items-center gap-3 rounded-xl p-2 duration-200 ease-in-out data-[highlighted]:bg-gray-600"
		>
			<div class="flex items-center">
				<Moon />
			</div>
			<div class="flex flex-col items-start">
				<h4>Sleep</h4>
			</div>
		</DropdownMenu.Item>
		<DropdownMenu.Separator class="my-1 -ml-1 -mr-1 block h-px bg-gray-600" />
		<DropdownMenu.Item
			on:click={() => {
				connect("shutdown")
			}}
			class="flex cursor-pointer select-none items-center gap-3 rounded-xl p-2 duration-200 ease-in-out data-[highlighted]:bg-gray-600"
		>
			<div class="flex items-center">
				<Power />
			</div>
			<div class="flex flex-col items-start">
				<h4>Shut down</h4>
			</div>
		</DropdownMenu.Item>
		<DropdownMenu.Separator class="my-1 -ml-1 -mr-1 block h-px bg-gray-600" />
		<DropdownMenu.Item
			on:click={() => {
				connect("restart")
			}}
			class="flex cursor-pointer select-none items-center gap-3 rounded-xl p-2 duration-200 ease-in-out data-[highlighted]:bg-gray-600"
		>
			<div class="flex items-center">
				<RotateCcw />
			</div>
			<div class="flex flex-col items-start">
				<h4>Restart</h4>
			</div>
		</DropdownMenu.Item>
	</DropdownMenu.Content>
</DropdownMenu.Root>

<script lang="ts">
	import { Avatar, DropdownMenu } from "bits-ui"
	import { flyAndScale } from "../utils/transitions.ts"
	import { Moon, Power, RotateCcw, UserCircle } from "lucide-svelte"
	import { settings } from "ui/stores/settings.ts"
	import { EzRTCClient } from "ezrtc"

	let connect = (action: string) => {
		let client: EzRTCClient
		let sent = false

		client = new EzRTCClient("wss://rtc-usw.levminer.com/one-to-many", $settings.connectionCode, [
			{
				urls: "stun:stun.relay.metered.ca:80",
			},
			{
				urls: "turn:standard.relay.metered.ca:80",
				username: "56feef2e09dcd8d33c5f67eb",
				credential: "ynk5rIg6gGh4lEAk",
			},
		])

		client.onMessage((message) => {
			if (!sent) {
				client.dataChannel.send(action)
				sent = true
			}
		})

		/* client.peerConnection.onconnectionstatechange = (event) => {
			console.log("EEEEEEEEVVVVVEEEEENNNNttt")
		} */
	}
</script>
