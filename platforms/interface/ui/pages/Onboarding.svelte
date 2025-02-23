<div class="radialBg flex min-h-screen flex-col items-center justify-center">
	{#if step === "welcome"}
		<div class="transparent-900 mx-auto flex w-1/3 flex-col justify-center rounded-2xl p-10 shadow-md backdrop-blur-xl sm:w-[95%]">
			<div class="mb-10 text-center">
				<h2>Let's start monitoring!</h2>
				<h3>Start using Cores with just a few clicks.</h3>
			</div>
			<div class="mx-auto flex w-full items-center justify-center">
				<button
					on:click={stepLogin}
					class="transparent-800 flex items-center justify-center gap-3 rounded-xl px-20 py-5 text-xl font-semibold shadow-md"
				>
					<MoveRight />
				</button>
			</div>
		</div>
	{/if}

	{#if step === "login"}
		<div class="flex w-full">
			<Login loginFn={login} withoutLoginFn={stepPricing} />
		</div>
	{/if}

	{#if step === "pricing"}
		<div class="mx-auto flex w-[50%] flex-col justify-center rounded-2xl bg-black/30 p-5 shadow-md backdrop-blur-xl sm:w-[95%]">
			<div class="text-center">
				<h2>Purchase Cores</h2>
				<h3>Purchase Cores to unlock all features and support the development.</h3>
			</div>
			<div class="flex w-full flex-col gap-3 rounded-xl p-8 sm:p-4">
				<div
					class="transparent-900 border-cores-alternative flex w-full transform flex-row gap-1 rounded-xl border-2 px-5 py-5 text-xl font-semibold shadow-md duration-100"
				>
					<div class="flex w-full flex-col">
						<div class="mb-3 text-left">
							<h2 class="bg-gradient-to-r from-purple-400 to-pink-600 bg-clip-text font-extrabold text-transparent">Cores Pro</h2>
						</div>
						<div>
							<div class="flex flex-row items-center gap-1 text-left">
								<Check class="h-5 w-5 text-green-500" />
								<h5>For personal use, one-time purchase, no subscriptions</h5>
							</div>
							<div class="flex flex-row items-center gap-1 text-left">
								<Check class="h-5 w-5 text-green-500" />
								<h5>Monitor up to 5 devices</h5>
							</div>
							<div class="flex flex-row items-center gap-1 text-left">
								<Check class="h-5 w-5 text-green-500" />
								<h5>Access any device remotely</h5>
							</div>
							<div class="flex flex-row items-center gap-1 text-left">
								<Check class="h-5 w-5 text-green-500" />
								<h5>Export data as .csv and image</h5>
							</div>
						</div>
					</div>
					<div class="flex flex-col items-end">
						<div>
							<h2 class="text-center text-3xl font-semibold">
								$9.99 <p class="text-xs text-gray-200">One time purchase</p>
							</h2>
						</div>
						<div>
							<button
								on:click={() => {
									posthog.capture("buy")
									open(`https://link.levminer.com/buy-cores-app?utm_source=app`)
								}}
								class="button bg-cores-alternative hover:text-cores-alternative border-cores-alternative mt-5 w-full gap-2 font-bold text-white hover:translate-y-0.5 hover:animate-pulse"
							>
								<ShoppingCart />
								Buy
							</button>
						</div>
					</div>
				</div>

				<div
					class="transparent-900 border-cores-alternative flex w-full transform flex-row gap-1 rounded-xl border-2 px-5 py-5 text-xl font-semibold shadow-md duration-100"
				>
					<div class="flex w-full flex-col">
						<div class="mb-3 text-left">
							<h2 class="bg-gradient-to-r from-rose-600 to-violet-400 bg-clip-text font-extrabold text-transparent">Cores Advanced</h2>
						</div>
						<div>
							<div class="flex flex-row items-center gap-1 text-left">
								<Check class="h-5 w-5 text-green-500" />
								<h5>For personal and business use, monthly subscription, cancel anytime</h5>
							</div>
							<div class="flex flex-row items-center gap-1 text-left">
								<Check class="h-5 w-5 text-green-500" />
								<h5>Choose how many devices you want to monitor</h5>
							</div>
							<div class="flex flex-row items-center gap-1 text-left">
								<Check class="h-5 w-5 text-green-500" />
								<h5>Access any device remotely</h5>
							</div>
							<div class="flex flex-row items-center gap-1 text-left">
								<Check class="h-5 w-5 text-green-500" />
								<h5>Export data as .csv and image</h5>
							</div>
						</div>
					</div>
					<div class="flex flex-col items-end">
						<div>
							<h2 class="text-center text-3xl font-semibold">
								$2.99 <p class="text-xs text-gray-200">/month/device</p>
							</h2>
						</div>
						<a
							href="mailto:support@coresmonitor.com"
							class="button bg-cores-alternative hover:text-cores-alternative border-cores-alternative mt-5 w-full font-bold text-white hover:translate-y-0.5 hover:animate-pulse"
						>
							<Mail />
							Get a quote
						</a>
					</div>
				</div>

				<div class="flex flex-row items-stretch justify-center gap-3">
					{#if variant === "free" || variant === undefined}
						<div class="flex w-1/2 flex-wrap justify-center text-lg">
							<div class="mx-auto flex w-full flex-col justify-between rounded-xl border-2 border-purple-400 p-5 text-left">
								<div>
									<h2
										class="mb-1 bg-gradient-to-r from-purple-400 to-pink-600 bg-clip-text text-left text-3xl font-extrabold text-transparent"
									>
										Continue for free
									</h2>
									<p class="text-base leading-tight">
										You can use the basic features of Cores for free. For remote connections and advanced features please activate
										Cores.
									</p>
								</div>
								<div>
									<button on:click={free} class="smallButton mt-3 w-full">
										<CircleCheck />
										Continue
									</button>
								</div>
							</div>
						</div>
					{:else}
						<div class="flex w-1/2 flex-wrap justify-center text-lg">
							<div class="mx-auto flex w-full flex-col justify-between rounded-xl border-2 border-purple-400 p-5 text-left">
								<div>
									<h2
										class="mb-1 bg-gradient-to-r from-purple-400 to-pink-600 bg-clip-text text-left text-3xl font-extrabold text-transparent"
									>
										1 week trial
									</h2>
									<p class="text-base leading-tight">
										You can use the basic features of Cores for 1 week. For remote connections and advanced features please
										activate Cores.
									</p>
								</div>
								<div>
									<button on:click={free} class="smallButton mt-3 w-full">
										<CircleCheck />
										Continue
									</button>
								</div>
							</div>
						</div>
					{/if}
					<div class="flex w-1/2 flex-wrap justify-center text-lg">
						<div class="mx-auto flex w-full flex-col justify-between space-y-5 rounded-xl border-2 border-purple-400 p-5 text-left">
							<div>
								<h2
									class="mb-1 bg-gradient-to-r from-purple-400 to-pink-600 bg-clip-text text-left text-3xl font-extrabold text-transparent"
								>
									Activate license
								</h2>
								<p class="text-base leading-tight">If you already purchased Cores, please activate your license key.</p>
								{#if $settings.licenseKey && $settings.licenseKey !== "" && $settings.licenseKey !== "free"}
									<p class="select-text text-base leading-tight">
										Your previous license key: <span class="font-bold">{$settings.licenseKey}</span>
									</p>
								{/if}
							</div>
							<div>
								<ModularDialog
									title={"Activate Cores"}
									description={"Use the license key from your purchase confirmation email to activate Cores. If you don't remember you key, please contact us at support@coresmonitor.com."}
								>
									<slot slot="openButton">
										<Dialog.Trigger class="smallButton mt-3 w-full">
											<CircleCheck />
											Activate license
										</Dialog.Trigger>
									</slot>
									<slot slot="confirmButton">
										<Dialog.Close on:click={() => activate()} class="smallButton">
											<CircleCheck />
											Activate license
										</Dialog.Close>
									</slot>
									<div>
										<h5>License key</h5>
										<input bind:value={key} class="input mt-1" type="text" id="key" />
									</div>
								</ModularDialog>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	{/if}

	{#if step === "tips"}
		<div class="mx-auto flex w-1/2 flex-col justify-center rounded-2xl bg-black/30 p-10 shadow-md backdrop-blur-xl">
			<div class="text-center">
				<h1 class="mb-5">Welcome to Cores!</h1>
			</div>
			<div class="flex w-full flex-col gap-3 rounded-xl p-8 sm:p-4">
				<button
					on:click={() => {
						router.goto("/home", true)
					}}
					class="transparent-900 flex w-full transform flex-row items-center gap-3 rounded-xl px-5 py-5 text-xl font-semibold shadow-md duration-100 hover:translate-y-1"
				>
					<Home size="30" />

					<div class="text-left">
						<h2>Explore the home screen</h2>

						<h3>You can see every component on the home screen.</h3>
					</div>
				</button>

				<button
					on:click={() => {
						router.goto("/connections", true)
					}}
					class="transparent-900 flex w-full transform flex-row items-center gap-3 rounded-xl px-5 py-5 text-xl font-semibold shadow-md duration-100 hover:translate-y-1"
				>
					<MonitorSmartphone size="30" />

					<div class="text-left">
						<h2>Setup remote connections</h2>

						<h3>Setup remote connections to monitor your computer from anywhere.</h3>
					</div>
				</button>

				<button
					on:click={() => {
						router.goto("/settings", true)
					}}
					class="transparent-900 flex w-full transform flex-row items-center gap-3 rounded-xl px-5 py-5 text-xl font-semibold shadow-md duration-100 hover:translate-y-1"
				>
					<Settings size="30" />

					<div class="text-left">
						<h2>Configure monitoring settings</h2>

						<h3>You can configure the refresh rate and interval of the sensors.</h3>
					</div>
				</button>
			</div>
		</div>
	{/if}
</div>

<script lang="ts">
	import { open } from "@tauri-apps/plugin-shell"
	import { state } from "../stores/state.ts"
	import { Dialog } from "bits-ui"
	import { router } from "@baileyherbert/tinro"
	import { MoveRight, Home, CircleCheck, Settings, Check, ShoppingCart, Mail, MonitorSmartphone } from "lucide-svelte"
	import { settings } from "../stores/settings.ts"
	import { onMount } from "svelte"
	import { start, cancel, onUrl } from "@fabianlars/tauri-plugin-oauth"
	import { supabaseClient } from "../utils/supabase.ts"
	import { Login, ModularDialog } from "ui"
	import posthog from "posthog-js"

	$: step = "" as "welcome" | "login" | "pricing" | "tips"
	$: key = ""
	$: variant = "free" as "free" | "paid"

	onMount(async () => {
		const ff = posthog.getFeatureFlag("raider") as "free" | "paid"
		variant = ff

		const { data: userData, error: userError } = await supabaseClient.auth.getUser()

		if (!userError) {
			stepPricing()
		} else {
			step = "welcome"

			posthog.capture("welcome")
		}
	})

	const stepLogin = () => {
		step = "login"

		posthog.capture("login")
	}

	const stepPricing = () => {
		step = "pricing"

		posthog.capture("pricing")
	}

	const stepTips = () => {
		step = "tips"
		$state.showMenu = true

		posthog.capture("tips")
	}

	const free = () => {
		if ($settings.licenseKey === "free" && variant === "paid") {
			// check if date is more than a week ago
			const licenseActivated = new Date($settings.licenseActivated)
			const sevenDaysAgo = new Date(new Date().getTime() - 7 * 24 * 60 * 60 * 1000)

			if (licenseActivated < sevenDaysAgo) {
				return alert("Your license has expired. Please activate your license key.")
			}
		}

		$settings.licenseKey = "free"
		$settings.licenseActivated = new Date().toISOString()

		posthog.capture("free")

		setTimeout(() => {
			stepTips()
		}, 250)
	}

	const login = async () => {
		try {
			// Start server
			const port = await start({
				response: `<div style="display:flex;flex-direction:column;align-items:center;justify-content:center;height:100vh;font-family:Arial,sans-serif;background:#1a1a1a;margin:0;position:fixed;top:0;left:0;right:0;bottom:0"><h2 style="color:white;margin:0">Authentication Completed</h2><p style="color:#999">You can now close this page and return to the app.</p></div><style>body{margin:0;padding:0;background:#1a1a1a}</style>`,
				ports: [5380, 5385],
			})

			// Listen for OAuth result
			await onUrl(async (url) => {
				const urlParams = new URLSearchParams(url.split("#")[1])
				const accessToken = urlParams.get("access_token")!
				const refreshToken = urlParams.get("refresh_token")!

				const { data, error } = await supabaseClient.auth.setSession({
					access_token: accessToken,
					refresh_token: refreshToken,
				})

				const { data: userData, error: userError } = await supabaseClient.from("user").select("*").single()

				if (error || userError) {
					alert(
						`Failed to login, please restart the app and try again or send an email to support@coresmonitor.com if you need help.\nError: ${
							error || userError
						}`,
					)
				}

				if (userData.plan === "personal" || userData.plan === "business") {
					// User is on a paid plan
					$state.plan = userData.plan
					$state.showMenu = true
					router.goto("/home")
				}

				cancel(port)

				stepPricing()
			})

			const { data, error } = await supabaseClient.auth.signInWithOAuth({
				provider: "google",
				options: {
					redirectTo: `http://localhost:${port}`,
					skipBrowserRedirect: true,
				},
			})

			if (error) {
				alert(
					`Failed to login, please restart the app and try again or send an email to support@coresmonitor.com if you need help.\nError: ${error}`,
				)
			}

			if (data) {
				console.log(data)
				open(data.url)
			}
		} catch (error) {
			alert(
				`Failed to login, please restart the app and try again or send an email to support@coresmonitor.com if you need help.\nError: ${error}`,
			)
		}
	}

	const activate = async () => {
		const { data, error } = await supabaseClient.auth.getUser()

		if (error) {
			alert("Failed to get user data, please log in.")
			return location.reload()
		}

		if (key !== "") {
			const url = `https://crs-activate.deno.dev?license_key=${key}&user_id=${data.user.id}`
			const options = {
				method: "GET",
			}

			try {
				const response = await fetch(url, options)
				const data = await response.json()

				if (data.activated && data?.meta.store_id === 62942) {
					stepTips()
					$state.plan = "personal"
				} else {
					alert(`Failed to activate: ${data.error}. Please reach out to support@coresmonitor.com if you need help.`)
				}
			} catch (error) {
				alert("Failed to send activation request, please try again or reach out to support@coresmonitor.com if you need help.")
				console.error(error)
			}
		}
	}
</script>
