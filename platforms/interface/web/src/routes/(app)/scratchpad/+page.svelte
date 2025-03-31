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
					<Message {message} {user} />
				{/each}
			</div>

			<div class="flex w-full flex-row items-center justify-center gap-3">
				<input type="file" id="file" class="hidden" on:change={uploadFile} />
				<button
					on:click={() => {
						const fileInput = document.getElementById("file")
						fileInput?.click()
					}}
					class="flex h-14 items-center justify-center rounded-xl bg-white p-4"
				>
					<Plus color="black" />
				</button>
				<input
					class="input h-14 w-full"
					placeholder="Write your message here, press Enter to submit"
					type="text"
					bind:value={message}
					on:keydown={sendMessage}
				/>
			</div>
		</div>
	</div>
{/if}

<script lang="ts">
	import { goto } from "$app/navigation"
	import { onMount } from "svelte"
	import { Loading, supabaseClient } from "ui"
	import type { User as UserType } from "@supabase/supabase-js"
	import type { Database } from "../../../../../ui/utils/database"
	import { NotebookPen, Plus } from "lucide-svelte"
	import Message from "../../../components/Message.svelte"

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
				const { data, error } = await supabaseClient.from("messages").insert([{ message: message, user_id: user?.id, type: "text" }])

				console.log(data, error)

				message = ""
			}
		}
	}

	const uploadFile = async (event: Event) => {
		const fileInput = event.target as HTMLInputElement

		// upload file
		if (fileInput.files && fileInput.files.length > 0) {
			const file = fileInput.files[0]
			const { data, error } = await supabaseClient.storage.from("messages").upload(`/${user?.id}/${file.name}`, file)

			console.log(data, error)

			if (data && !error) {
				const { data: fileData, error: fileError } = await supabaseClient
					.from("messages")
					.insert([{ message: `${file.name}`, user_id: user?.id, type: "file" }])

				console.log(fileData, fileError)
			}
		}
	}
</script>
