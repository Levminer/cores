{#if loading}
	<Loading />
{:else}
	<div class="transparent-900 m-10 mx-auto w-11/12 rounded-xl sm:w-full">
		<div class="mx-10 flex flex-col gap-5 pb-10 pt-10 sm:mx-3 sm:flex-wrap">
			<!-- account -->
			<div class="transparent-800 flex w-full flex-row flex-wrap items-center justify-between rounded-xl p-8 text-left sm:p-4">
				<div class="flex flex-col items-start gap-3">
					<div class="flex items-center gap-3">
						<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
							<User />
						</div>
						<h2>Account</h2>
					</div>
					<h3>Email: {user?.email}</h3>
				</div>

				<div class="flex flex-col items-start gap-3 sm:my-5">
					<button
						on:click={async () => {
							await supabaseClient.auth.signOut()
							goto("/home")
						}}
						class="button"
					>
						<LogOut />
						Log out
					</button>
				</div>
			</div>

			<!-- about -->
			<div class="transparent-800 flex w-full flex-row flex-wrap items-center justify-between rounded-xl p-8 text-left sm:p-4">
				<div class="flex flex-col items-start gap-3">
					<div class="flex items-center gap-3">
						<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
							<Info />
						</div>
						<h2>About Cores</h2>
					</div>
					<h3>Information about your Cores build and your computer.</h3>
				</div>

				<div class="flex flex-col items-start gap-3 sm:my-5">
					<button
						on:click={() => {
							alert(`Cores: ${version} \n\nRelease date: ${date} \nBuild number: ${number}\nServer: ${PUBLIC_CONNECTION_URL} \n\nCreated by: Lőrik Levente`)
						}}
						class="button"
					>
						<Info />
						About Cores
					</button>
				</div>
			</div>
		</div>
	</div>
{/if}

<script lang="ts">
	import { supabaseClient, Loading } from "ui"
	import { onMount } from "svelte"
	import { goto } from "$app/navigation"
	import { LogOut, User } from "lucide-svelte"
	import { Info, Megaphone, Github } from "lucide-svelte"
	import { version, number, date } from "../../../../../../../build.json"
	import type { User as UserType } from "@supabase/supabase-js"
	import { PUBLIC_CONNECTION_URL } from "$env/static/public"

	$: loading = true
	$: user = null as UserType | null

	onMount(async () => {
		const { data: userData, error: userError } = await supabaseClient.auth.getUser()

		console.log(userData, userError)

		if (!userError && userData !== null) {
			user = userData.user
			loading = false
		} else {
			goto("/login")
		}
	})
</script>
