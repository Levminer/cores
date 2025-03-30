{#if loading}
	<Loading />
{:else}
	<div class="transparent-900 m-10 mx-auto w-11/12 rounded-xl sm:w-full">
		<div class="mx-10 flex flex-col gap-5 pb-10 pt-10 sm:mx-3 sm:flex-wrap">
			<div class="transparent-800 flex w-full flex-row flex-wrap items-center justify-between rounded-xl p-8 text-left sm:p-4">
				<div class="flex flex-col items-start gap-3">
					<div class="flex items-center gap-3">
						<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
							<NotebookPen />
						</div>
						<h2>Scratchpad</h2>
					</div>
					<h3>This is your scratchpad. You can write anything you want, its automatically synced.</h3>
				</div>
			</div>

			<div class="overlayScroll flex max-h-96 flex-col gap-5 overflow-y-auto">
				<!-- Set a max height and enable scrolling -->
				{#each messages as message}
					<div class="transparent-800 flex w-full select-text flex-row flex-wrap items-center justify-between rounded-xl p-4 text-left">
						<p class="text-lg text-gray-200">{message.message}</p>
						<p class="text-sm text-gray-400">{formatTime(message.created_at)}</p>
					</div>
				{/each}
			</div>

			<input class="input" placeholder="Write your message here, press Enter to submit" type="text" bind:value={message} on:keydown={sendMessage} />
		</div>
	</div>
{/if}

<script lang="ts">
	import { goto } from "$app/navigation"
	import { onMount } from "svelte"
	import { Loading, supabaseClient } from "ui"
	import type { User as UserType } from "@supabase/supabase-js"
	import type { Database } from "../../../../../ui/utils/database"
	import { NotebookPen } from "lucide-svelte"

	$: loading = true
	$: user = null as UserType | null
	$: messages = [] as Database["public"]["Tables"]["messages"]["Row"][]
	$: message = "" as string

	onMount(() => {
		supabaseClient
			.channel("messages")
			.on("postgres_changes", { event: "*", schema: "public", table: "messages" }, (payload) => {
				const message = payload.new as Database["public"]["Tables"]["messages"]["Row"]
				console.log("Change received!", payload)
				console.log("New message:", message)
				messages = [message, ...messages]
			})
			.subscribe()
	})

	onMount(async () => {
		const { data: userData, error: userError } = await supabaseClient.auth.getUser()

		console.log(userData, userError)

		if (!userError && userData !== null) {
			user = userData.user

			const { data: messagesData, error: messagesError } = await supabaseClient
				.from("messages")
				.select("*")
				.order("created_at", { ascending: false })

			if (!messagesError && messagesData.length > 0) {
				messages = messagesData
			}

			loading = false
		} else {
			goto("/login")
		}
	})

	const sendMessage = async (event: KeyboardEvent) => {
		console.log(event)
		// check if key is enter
		if (event.key === "Enter") {
			if (message !== "") {
				const { data, error } = await supabaseClient.from("messages").insert([{ message: message, user_id: user?.id }])

				console.log(data, error)

				message = ""
			}
		}
	}

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
</script>
