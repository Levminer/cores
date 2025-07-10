<div class="transparent-800 flex w-full select-text flex-row flex-wrap items-center justify-between rounded-xl p-4 text-left">
	{#if message.type === "text"}
		<div>
			<p class="text-lg text-gray-200">{message.message}</p>
			<p class="text-xs text-gray-400">{formatTime(message.created_at)}</p>
		</div>
		<div class="flex flex-row items-center justify-center gap-3">
			<button
				class="rounded-lg bg-white p-2"
				onclick={() => {
					if (message?.message) {
						navigator.clipboard.writeText(message.message)
						return alert("Link copied to clipboard!")
					}
				}}
			>
				<Clipboard class="h-5 w-5 text-black" />
			</button>
			<button
				class="bg-popup-red rounded-lg p-2"
				onclick={() => {
					if (message?.message) {
						deleteMessage(message.message_id)
					}
				}}
			>
				<Trash class="h-5 w-5 text-white" />
			</button>
		</div>
	{/if}

	{#if message.type === "file"}
		<div>
			<p class="text-lg italic text-gray-200 underline">{message.message}</p>
			<p class="text-xs text-gray-400">{formatTime(message.created_at)}</p>
		</div>
		<div class="flex flex-row items-center justify-center gap-3">
			<button
				onclick={() => {
					if (message?.message) {
						getURL(message.message)
					}
				}}
				class="rounded-lg bg-white p-2"
			>
				<ExternalLink class="h-5 w-5 text-black" />
			</button>
			<button
				class="bg-popup-red rounded-lg p-2"
				onclick={() => {
					if (message?.message) {
						deleteMessage(message.message_id)
					}
				}}
			>
				<Trash class="h-5 w-5 text-white" />
			</button>
		</div>
	{/if}
</div>

<script lang="ts">
	import { supabaseClient } from "ui"
	import type { Database } from "../../../ui/utils/database"
	import type { User as UserType } from "@supabase/supabase-js"
	import { Clipboard, ExternalLink, Trash } from "lucide-svelte"
	interface Props {
		message: Database["public"]["Tables"]["messages"]["Row"];
		user: UserType | null;
	}

	let { message, user }: Props = $props();

	const formatTime = (time: string) => {
		const date = new Date(time)
		const options: Intl.DateTimeFormatOptions = {
			year: "numeric",
			month: "2-digit",
			day: "2-digit",
			hour: "2-digit",
			minute: "2-digit",
			second: "2-digit",
		}
		return date.toLocaleString("hu-HU", options)
	}

	const deleteMessage = async (messageId: string) => {
		if (confirm("Are you sure you want to delete this message?")) {
			const { data, error } = await supabaseClient.from("messages").delete().eq("message_id", messageId)

			location.reload()
			console.log(data, error)
		}
	}

	const getURL = async (fileName: string) => {
		const { data, error } = await supabaseClient.storage.from("messages").createSignedUrl(`${user?.id}/${fileName}`, 60 * 60 * 24)

		console.log(data, error)

		if (data && !error) {
			// copy link to clipboard
			navigator.clipboard.writeText(`https://rd.coresmonitor.com?link=${data.signedUrl}`)

			setTimeout(() => {
				const newWindow = window.open(data.signedUrl, "_blank")

				if (!newWindow || newWindow.closed || typeof newWindow.closed == "undefined") {
					return alert("Link copied to clipboard!")
				}

				newWindow.focus()
			}, 10)
		}
	}
</script>
