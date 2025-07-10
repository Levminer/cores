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
	import { onMount } from "svelte"
	import { goto } from "$app/navigation"
	import { Loading, Login, supabaseClient } from "ui"

	let loading = $state(true)

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
				redirectTo: `${import.meta.env.VITE_URL}/login`,
			},
		})

		console.log(data, error)
	}
</script>
