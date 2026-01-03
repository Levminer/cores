<ModularDialog title={data?.title} description={data?.message} open={openDialog}>
	{#snippet confirmButton()}
		<Dialog.Close
			onclick={() => {
				showMore()
			}}
			class="smallButton"
		>
			<SquareArrowOutUpRight />
			Show more
		</Dialog.Close>
	{/snippet}
</ModularDialog>

<script lang="ts">
	import { localSettings } from "../stores/localSettings"
	import { open } from "@tauri-apps/plugin-shell"
	import ModularDialog from "./ModularDialog.svelte"
	import { Dialog } from "bits-ui"
	import { SquareArrowOutUpRight } from "lucide-svelte"

	interface NewsData {
		title: string
		message: string
		link: string
		date: string
		image: string
	}

	let data = $state() as NewsData | undefined
	let openDialog = $state(false)

	$effect(() => {
		fetchNews()
	})

	const fetchNews = async () => {
		try {
			const res = await fetch("https://www.coresmonitor.com/api/message")
			data = (await res.json()) as NewsData

			if (new Date($localSettings.newsDate) < new Date(data.date)) {
				openDialog = true
				$localSettings.newsDate = data.date
			}
		} catch (error) {
			console.log("Failed to fetch news", error)
		}
	}

	const showMore = () => {
		if (data?.link) {
			open(data.link)
		}

		close()
	}
</script>
