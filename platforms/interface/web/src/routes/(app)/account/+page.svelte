{#if loading}
	<Loading />
{:else}
	<div class="transparent-900 m-10 mx-auto w-11/12 rounded-xl sm:w-full">
		<div class="mx-10 flex flex-col gap-5 pb-10 pt-10 sm:mx-3 sm:flex-wrap">
			<!-- account -->
			{#if import.meta.env.VITE_LOGIN}
				<div class="transparent-800 flex w-full flex-row flex-wrap items-center justify-between rounded-xl p-8 text-left sm:p-4">
					<div class="flex flex-col items-start gap-3">
						<div class="flex items-center gap-3">
							<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
								<User />
							</div>
							<h2>Account</h2>
						</div>
						<h3>Email: {user?.email ?? "Not logged in"}</h3>
					</div>

					<div class="flex flex-col items-start gap-3 sm:my-5">
						{#if user?.email}
							<button
								onclick={async () => {
									await supabaseClient.auth.signOut({ scope: "local" })
									goto("/home")
								}}
								class="button"
							>
								<LogOut />
								Log out
							</button>
						{:else}
							<button
								onclick={async () => {
									await supabaseClient.auth.signOut({ scope: "local" })
									goto("/login")
								}}
								class="button"
							>
								<User />
								Log in
							</button>
						{/if}
					</div>
				</div>
			{/if}

			<!-- connection server -->
			{#if !import.meta.env.VITE_LOGIN}
				 <div class="transparent-800 flex w-full flex-row flex-wrap items-center justify-between rounded-xl p-8 text-left sm:p-4">
				<div class="flex flex-col items-start gap-3">
					<div class="flex items-center gap-3">
						<div class="transparent-900 flex aspect-square items-center justify-center rounded-lg p-3 sm:p-2">
							<Server />
						</div>
						<h2>Connection server</h2>
					</div>
					<h3>You can use the default connection server or host your own.</h3>
				</div>

				<div class="flex flex-col items-start gap-3 sm:my-5">
					<ConnectionServer />
				</div>
			</div>
			{/if}

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
						onclick={() => {
							alert(
								`Cores: ${version} \n\nRelease date: ${date} \nBuild number: ${number}\nServer: ${$settings.connectionURL} \n\nCreated by: Lőrik Levente`,
							)
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
	import { supabaseClient, Loading, ConnectionServer, settings } from "ui"
	import { onMount } from "svelte"
	import { goto } from "$app/navigation"
	import { LogOut, Server, User } from "lucide-svelte"
	import { Info } from "lucide-svelte"
	import { version, number, date } from "../../../../../../../build.json"
	import type { User as UserType } from "@supabase/supabase-js"

	let loading = $state(true)
	let user = $state(null as UserType | null)

	onMount(async () => {
		const { data: userData, error: userError } = await supabaseClient.auth.getUser()

		console.log(userData, userError)

		if (!userError && userData !== null) {
			user = userData.user
			loading = false
		} else {
			loading = false
		}
	})
</script>
