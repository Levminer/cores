<div class="radialBg flex min-h-screen flex-col items-center justify-center">
	{#if step === "welcome"}
		<div class="mx-auto flex w-1/3 flex-col justify-center rounded-2xl bg-black/30 p-10 shadow-md backdrop-blur-xl">
			<div class="text-center">
				<h2>Welcome to Cores!</h2>
				<h3>With Cores you can monitor your Windows, Linux <br /> or macOS device remotely, from anywhere.</h3>
			</div>

			<div class="flex w-full flex-col items-center gap-3 rounded-xl p-8 sm:p-4">
				<button
					onclick={() => {
						takeStep("login")
					}}
					class="transparent-900 flex transform flex-row items-center justify-center gap-3 rounded-xl px-5 py-5 text-xl font-semibold shadow-md duration-100 hover:translate-y-1"
				>
					<div class="text-left">
						<h2>Get started</h2>
					</div>
				</button>
			</div>
		</div>
	{/if}

	{#if step === "login"}
		<div class="flex w-full">
			<Login
				googleLoginFn={() => {
					login("google")
				}}
				appleLoginFn={() => {
					login("apple")
				}}
			/>
		</div>
	{/if}

	{#if step === "pricing"}
		<div class="mx-auto flex w-[50%] flex-col justify-center rounded-2xl bg-black/30 p-10 shadow-md backdrop-blur-xl sm:w-[95%]">
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
								$7.99 <p class="text-xs text-gray-200">One time purchase</p>
							</h2>
						</div>
						<div>
							<button
								onclick={() => {
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

				<div class="flex flex-row items-stretch justify-center gap-3">
					<div class="flex w-1/2 flex-wrap justify-center text-lg">
						<div class="mx-auto flex w-full flex-col justify-between rounded-xl border-2 border-purple-400 p-5 text-left">
							<div>
								<h2
									class="mb-1 bg-gradient-to-r from-purple-400 to-pink-600 bg-clip-text text-left text-3xl font-extrabold text-transparent"
								>
									1 week trial
								</h2>
								<p class="text-base leading-tight">
									You can use all features of Cores for 1 week. You can try out remote connections and advanced features during the
									trial.
								</p>
							</div>
							<div>
								<button onclick={trial} class="smallButton mt-3 w-full">
									<CircleCheck />
									Continue
								</button>
							</div>
						</div>
					</div>

					<div class="flex w-1/2 flex-wrap justify-center text-lg">
						<div class="mx-auto flex w-full flex-col justify-between space-y-5 rounded-xl border-2 border-purple-400 p-5 text-left">
							<div>
								<h2
									class="mb-1 bg-gradient-to-r from-purple-400 to-pink-600 bg-clip-text text-left text-3xl font-extrabold text-transparent"
								>
									Activate license
								</h2>
								<p class="text-base leading-tight">If you already purchased Cores, please activate your license key.</p>
							</div>
							<div>
								<ModularDialog
									title={"Activate Cores"}
									description={"Use the license key from your purchase confirmation email to activate Cores. If you don't remember you key, please contact us at support@coresmonitor.com."}
								>
									{#snippet openButton()}
										<Dialog.Trigger class="smallButton mt-3 w-full">
											<CircleCheck />
											Activate license
										</Dialog.Trigger>
									{/snippet}

									{#snippet confirmButton()}
										<Dialog.Close on:click={() => activate()} class="smallButton">
											<CircleCheck />
											Activate license
										</Dialog.Close>
									{/snippet}

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

	{#if step === "connections"}
		<div class="mx-auto flex w-1/2 flex-col justify-center rounded-2xl bg-black/30 p-10 shadow-md backdrop-blur-xl">
			<div class="text-center">
				<h2>Remote Connections</h2>
				<h3>With Cores you can easily set up remote monitoring on your devices.</h3>
			</div>
			<div class="flex w-full flex-col gap-3 rounded-xl p-8 sm:p-4">
				<button
					onclick={() => {
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
					onclick={() => {
						open("https://coresmonitor.com/home?utm_source=desktop")
					}}
					class="transparent-900 flex w-full transform flex-row items-center gap-3 rounded-xl px-5 py-5 text-xl font-semibold shadow-md duration-100 hover:translate-y-1"
				>
					<Globe size="30" />

					<div class="text-left">
						<h2>Web dashboard</h2>

						<h3>Access your computer from any device using the web dashboard.</h3>
					</div>
				</button>
				<button
					onclick={() => {
						open("https://link.levminer.com/cores-ios?utm_source=desktop")
					}}
					class="transparent-900 flex w-full transform flex-row items-center gap-3 rounded-xl px-5 py-5 text-xl font-semibold shadow-md duration-100 hover:translate-y-1"
				>
					<svg class="h-[30px] w-[30px] text-current" role="img" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
						<title>App Store</title>
						<path
							fill="currentColor"
							d="M8.8086 14.9194l6.1107-11.0368c.0837-.1513.1682-.302.2437-.4584.0685-.142.1267-.2854.1646-.4403.0803-.3259.0588-.6656-.066-.9767-.1238-.3095-.3417-.5678-.6201-.7355a1.4175 1.4175 0 0 0-.921-.1924c-.3207.043-.6135.1935-.8443.4288-.1094.1118-.1996.2361-.2832.369-.092.1463-.175.2979-.259.4492l-.3864.6979-.3865-.6979c-.0837-.1515-.1667-.303-.2587-.4492-.0837-.1329-.1739-.2572-.2835-.369-.2305-.2353-.5233-.3857-.844-.429a1.4181 1.4181 0 0 0-.921.1926c-.2784.1677-.4964.426-.6203.7355-.1246.311-.1461.6508-.066.9767.038.155.0962.2984.1648.4403.0753.1564.1598.307.2437.4584l1.248 2.2543-4.8625 8.7825H2.0295c-.1676 0-.3351-.0007-.5026.0092-.1522.009-.3004.0284-.448.0714-.3108.0906-.5822.2798-.7783.548-.195.2665-.3006.5929-.3006.9279 0 .3352.1057.6612.3006.9277.196.2683.4675.4575.7782.548.1477.043.296.0623.4481.0715.1675.01.335.009.5026.009h13.0974c.0171-.0357.059-.1294.1-.2697.415-1.4151-.6156-2.843-2.0347-2.843zM3.113 18.5418l-.7922 1.5008c-.0818.1553-.1644.31-.2384.4705-.067.1458-.124.293-.1611.452-.0785.3346-.0576.6834.0645 1.0029.1212.3175.3346.583.607.7549.2727.172.5891.2416.9013.1975.3139-.044.6005-.1986.8263-.4402.1072-.1148.1954-.2424.2772-.3787.0902-.1503.1714-.3059.2535-.4612L6 19.4636c-.0896-.149-.9473-1.4704-2.887-.9218m20.5861-3.0056a1.4707 1.4707 0 0 0-.779-.5407c-.1476-.0425-.2961-.0616-.4483-.0705-.1678-.0099-.3352-.0091-.503-.0091H18.648l-4.3891-7.817c-.6655.7005-.9632 1.485-1.0773 2.1976-.1655 1.0333.0367 2.0934.546 3.0004l5.2741 9.3933c.084.1494.167.299.2591.4435.0837.131.1739.2537.2836.364.231.2323.5238.3809.8449.4232.3192.0424.643-.0244.9217-.1899.2784-.1653.4968-.4204.621-.7257.1246-.3072.146-.6425.0658-.9641-.0381-.1529-.0962-.2945-.165-.4346-.0753-.1543-.1598-.303-.2438-.4524l-1.216-2.1662h1.596c.1677 0 .3351.0009.5029-.009.1522-.009.3007-.028.4483-.0705a1.4707 1.4707 0 0 0 .779-.5407A1.5386 1.5386 0 0 0 24 16.452a1.539 1.539 0 0 0-.3009-.9158Z"
						/>
					</svg>

					<div class="text-left">
						<h2>iOS app</h2>

						<h3>Access your computer from your iPhone.</h3>
					</div>
				</button>
			</div>

			<div class="mx-auto">
				<button
					onclick={() => {
						takeStep("tips")
					}}
					class="transparent-900 flex transform flex-row items-center justify-center gap-3 rounded-xl px-5 py-5 text-xl font-semibold shadow-md duration-100 hover:translate-y-1"
				>
					<div class="text-left">
						<h2>Continue</h2>
					</div>
				</button>
			</div>
		</div>
	{/if}

	{#if step === "tips"}
		<div class="mx-auto flex w-1/2 flex-col justify-center rounded-2xl bg-black/30 p-10 shadow-md backdrop-blur-xl">
			<div class="text-center">
				<h2>Welcome to Cores!</h2>
				<h3>Check out the recommended actions.</h3>
			</div>
			<div class="flex w-full flex-col gap-3 rounded-xl p-8 sm:p-4">
				<button
					onclick={() => {
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
					onclick={() => {
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
					onclick={() => {
						router.goto("/settings", true)
					}}
					class="transparent-900 flex w-full transform flex-row items-center gap-3 rounded-xl px-5 py-5 text-xl font-semibold shadow-md duration-100 hover:translate-y-1"
				>
					<Settings size="30" />

					<div class="text-left">
						<h2>Configure monitoring settings</h2>

						<h3>You can configure how often sensors are refreshed.</h3>
					</div>
				</button>
			</div>
		</div>
	{/if}

	<ModularDialog open={redirectDialog} title={"Login"} description={"Opening browser for login..."}>
		{#snippet confirmButton()}
			<Dialog.Close
				on:click={() => {
					redirectDialog = false
				}}
				class="smallButton"
			>
				<CircleX class="h-5 w-5" />
				Cancel
			</Dialog.Close>
		{/snippet}
		<div class="flex flex-col flex-wrap gap-3">
			<p class="text-sm text-gray-200">
				Browser didn't open? <button class="underline" onclick={() => open(url)}>Open</button> or
				<button class="underline" onclick={() => navigator.clipboard.writeText(url)}>copy link</button>
			</p>
		</div>
	</ModularDialog>
</div>

<script lang="ts">
	import { open } from "@tauri-apps/plugin-shell"
	import { appState } from "../stores/state.ts"
	import { settings } from "../stores/settings.ts"
	import { Dialog } from "bits-ui"
	import { router } from "@baileyherbert/tinro"
	import { Home, CircleCheck, Settings, Check, ShoppingCart, Mail, MonitorSmartphone, CircleX, Globe } from "lucide-svelte"
	import { onMount } from "svelte"
	import { start, cancel, onUrl } from "@fabianlars/tauri-plugin-oauth"
	import { supabaseClient } from "../utils/supabase.ts"
	import { Login, ModularDialog } from "ui"
	import posthog from "posthog-js"
	import type { Provider, User } from "@supabase/supabase-js"

	type Steps = "welcome" | "login" | "pricing" | "tips" | "connections"
	let step = $state("" as Steps)
	let key = $state("")
	let user = $state(null as User | null)
	let redirectDialog = $state(false)
	let url = $state("")

	onMount(async () => {
		const { data: userData, error: userError } = await supabaseClient.auth.getUser()
		user = userData.user

		if (!userError) {
			takeStep("pricing")
		} else {
			takeStep("welcome")
		}
	})

	onMount(() => {
		const handleKeydown = (event: KeyboardEvent) => {
			if (event.key === "Escape" && event.metaKey) {
				$appState.showMenu = true
				router.goto("/home", true)
			}
		}
		document.addEventListener("keydown", handleKeydown)

		return () => {
			document.removeEventListener("keydown", handleKeydown)
		}
	})

	const takeStep = (nextStep: Steps, showMenu: boolean = false) => {
		step = nextStep
		posthog.capture(nextStep)

		if (showMenu) {
			$appState.showMenu = true
		}
	}

	const trial = () => {
		// check if date is more than a week ago
		const licenseActivated = user?.created_at ? new Date(user.created_at) : new Date()
		const sevenDaysAgo = new Date(new Date().getTime() - 7 * 24 * 60 * 60 * 1000)

		posthog.capture("trial")
		$appState.plan = "trial"

		if (licenseActivated < sevenDaysAgo) {
			if (import.meta.env.PROD) {
				$settings.remoteConnections = false
			}
			return alert("Your free trial expired. Please buy Cores to continue.")
		}

		setTimeout(() => {
			takeStep("connections", true)
		}, 250)
	}

	const login = async (provider: Provider) => {
		try {
			// Start server
			const port = await start({
				response: `<div style="display:flex;flex-direction:column;align-items:center;justify-content:center;height:100vh;font-family:Arial,sans-serif;background:#1a1a1a;margin:0;position:fixed;top:0;left:0;right:0;bottom:0"><h2 style="color:white;margin:0">Authentication Completed</h2><p style="color:#999">You can now close this page and return to the app.</p></div><style>body{margin:0;padding:0;background:#1a1a1a}</style>`,
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

				if (userData?.plan === "personal" || userData?.plan === "business") {
					// User is on a paid plan
					$appState.plan = userData?.plan
					$appState.showMenu = true
					router.goto("/home")
				}

				redirectDialog = false
				cancel(port)

				takeStep("pricing")
			})

			const { data, error } = await supabaseClient.auth.signInWithOAuth({
				provider: provider,
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
				redirectDialog = true
				open(data.url as string)
				url = data.url as string
			}
		} catch (error) {
			alert(
				`Failed to login. A browser window should open where you can login, please try again or restart the app. Need help? Send an email to support@coresmonitor.com.\nError: ${error}`,
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
					takeStep("connections", true)
					$appState.plan = "personal"
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
