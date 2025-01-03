{#if loading}
	<Loading />
{:else}
	<div class="flex min-h-screen flex-col items-center justify-center">
		<div class="flex w-full">
			<Login loginFn={login} />
		</div>
	</div>
{/if}

<script lang="ts">
	import Login from "ui/components/login.svelte"
	import { supabaseClient } from "ui/utils/supabase"
	import { PUBLIC_URL } from "$env/static/public"
	import { onMount } from "svelte"
	import Loading from "ui/navigation/loading.svelte"
	import { goto } from "$app/navigation"

	$: loading = true

	onMount(async () => {
		const { data: userData, error: userError } = await supabaseClient.auth.getUser()

		if (!userError && userData !== null) {
			goto("/home")
		} else {
			loading = false
		}
	})

	const login = async () => {
		const { data, error } = await supabaseClient.auth.signInWithOAuth({
			provider: "google",
			options: {
				redirectTo: `${PUBLIC_URL}/login`,
			},
		})

		console.log(data, error)
	}
</script>
