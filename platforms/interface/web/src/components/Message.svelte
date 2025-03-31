<div class="transparent-800 flex w-full select-text flex-row flex-wrap items-center justify-between rounded-xl p-4 text-left">
	{#if message.type === "text"}
		<p class="text-lg text-gray-200">{message.message}</p>
		<p class="text-xs text-gray-400">{formatTime(message.created_at)}</p>
	{/if}

	{#if message.type === "file"}
		<button
			on:click={() => {
				if (message?.message) {
					getURL(message.message)
				}
			}}
			class="text-lg italic text-gray-200 underline">{message.message}</button
		>
		<p class="text-xs text-gray-400">{formatTime(message.created_at)}</p>
	{/if}
</div>

<script lang="ts">
	import { supabaseClient } from "ui"
	import type { Database } from "../../../ui/utils/database"
	import type { User as UserType } from "@supabase/supabase-js"
	export let message: Database["public"]["Tables"]["messages"]["Row"]
	export let user: UserType | null

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

	const getURL = async (fileName: string) => {
		const { data, error } = await supabaseClient.storage.from("messages").createSignedUrl(`${user?.id}/${fileName}`, 60 * 60)

		console.log(data, error)

		if (data && !error) {
			// open link in new tab
			window.open(data.signedUrl, "_blank")
		}
	}
</script>
