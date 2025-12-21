{#if loading}
	<Loading />
{:else}
	<div class="flex min-h-screen flex-col items-center justify-center">
		<div class="flex w-full">
			<Login googleLoginFn={() => login("google")} appleLoginFn={() => login("apple")} />
		</div>
	</div>
{/if}

<script lang="ts">
	import { onMount } from "svelte"
	import { goto } from "$app/navigation"
	import { Loading, Login, supabaseClient } from "ui"
	import type { Provider } from "@supabase/supabase-js"
	import { env } from "$env/dynamic/public"

	let loading = $state(true)

	onMount(async () => {
		const { data: userData, error: userError } = await supabaseClient.auth.getUser()

		if (!userError && userData !== null) {
			goto("/home")
		} else {
			loading = false
		}
	})

	const login = async (provider: Provider) => {
		const { data, error } = await supabaseClient.auth.signInWithOAuth({
			provider: provider,
			options: {
				redirectTo: `${env.PUBLIC_REDIRECT_URL}/login`,
			},
		})

		console.log(data, error)
	}
</script>
