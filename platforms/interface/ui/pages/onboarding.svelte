<div class="from-cores-current to-cores-max flex min-h-screen flex-col items-center justify-center bg-gradient-to-r">
	<!-- step 1 -->
	<div class="step1 mx-auto flex w-1/2 flex-col justify-center rounded-2xl bg-black/30 p-20 shadow-md backdrop-blur-xl">
		<div class="mb-10 text-center">
			<h1 class="mb-2">Let's start monitoring!</h1>
		</div>
		<div class="mx-auto flex w-full items-center justify-center">
			<button
				on:click={step2}
				class="transparent-900 flex items-center justify-center gap-3 rounded-xl px-20 py-5 text-xl font-semibold shadow-md"
			>
				<MoveRight />
			</button>
		</div>
	</div>

	<!-- step 2 -->
	<div class="step2 mx-auto hidden w-1/2 flex-col justify-center rounded-2xl bg-black/30 p-10 shadow-md backdrop-blur-xl">
		<div class="mb-10 text-center">
			<h1 class="mb-2">Activate Cores</h1>
		</div>
		<div class="flex w-full flex-1 flex-row gap-5">
			<div class="flex w-1/2 flex-col justify-center rounded-xl border-2 border-purple-600 bg-purple-700 p-3 text-center">
				<h2 class="mb-2">Activate Cores</h2>
				<h3 class="mb-10">Use the license key from your purchase confirmation email to activate Cores.</h3>

				<Dialog
					action={activate}
					title={"Activate Cores"}
					description={"Use the license key from your purchase confirmation email to activate Cores."}
				>
					<span class="flex flex-row items-center justify-center gap-1" slot="button">
						<CircleCheck />
						Activate
					</span>
					<div>
						<h5>License key</h5>
						<input class="input mt-1" type="text" id="key" />
					</div>
				</Dialog>
			</div>
			<div class="flex w-1/2 flex-col justify-center rounded-xl border-2 border-pink-600 bg-pink-700 p-3 text-center">
				<h2 class="mb-2">Buy Cores</h2>
				<h3 class="mb-10">If you didn't purchase Cores yet you can buy it for $14<sup>99</sup> now.</h3>
				<button
					on:click={() => {
						open("https://link.levminer.com/buy-cores-app")
					}}
					class="button mx-1"
				>
					<ShoppingCart />
					Buy
				</button>
			</div>
		</div>
		<div class="mt-5 flex justify-center">
			<Popover.Root>
				<Popover.Trigger title="Trial over! Activate Cores!" class="disabled:cursor-not-allowed disabled:opacity-50" disabled={trialOver}
					>Get started for free</Popover.Trigger
				>
				<Popover.Content
					class="z-30 w-full max-w-[300px] rounded-[12px] border border-cyan-600 bg-cyan-700 p-3 text-left"
					transition={flyAndScale}
					sideOffset={8}
				>
					<div class="flex flex-col text-center">
						<h4 class="mb-2">Get started for free</h4>
						<!-- <h5>You can use Cores for 7 days, after that you will need to buy it.</h5> -->
						 <h5>During the beta period you can use Cores for free, please consider buying it to support the development.</h5>
					</div>
					<Popover.Close on:click={free} class="smallButton my-4 w-full">
						<CircleCheck />
						Continue</Popover.Close
					>
				</Popover.Content>
			</Popover.Root>
		</div>
	</div>

	<!-- step 3 -->
	<div class="step3 mx-auto hidden w-1/2 flex-col justify-center rounded-2xl bg-black/30 p-20 shadow-md backdrop-blur-xl">
		<div class="mb-10 text-center">
			<h1 class="mb-2">Welcome to Cores!</h1>
		</div>
		<div class="mx-auto flex w-full flex-col items-center justify-center">
			<div class="flex flex-col gap-3">
				<button
					on:click={() => {
						router.goto("/home", true)
					}}
					class="transparent-900 flex items-center justify-center gap-3 rounded-xl px-20 py-5 text-xl font-semibold shadow-md"
				>
					<Home />
					Explore the home screen
				</button>
				<button
					on:click={() => {
						router.goto("/connections", true)
					}}
					class="transparent-900 flex items-center justify-center gap-3 rounded-xl px-20 py-5 text-xl font-semibold shadow-md"
				>
					<Globe />
					Setup remote connections
				</button>
				<button
					on:click={() => {
						router.goto("/settings", true)
					}}
					class="transparent-900 flex items-center justify-center gap-3 rounded-xl px-20 py-5 text-xl font-semibold shadow-md"
				>
					<Settings />
					Configure monitoring settings
				</button>
			</div>
		</div>
	</div>
</div>

<script lang="ts">
	import { open } from "@tauri-apps/plugin-shell"
	import Dialog from "ui/components/dialog.svelte"
	import { router } from "@baileyherbert/tinro"
	import { Globe, MoveRight, ShoppingCart, Home, CircleCheck, Settings } from "lucide-svelte"
	import { settings } from "../stores/settings.ts"
	import { Popover } from "bits-ui"
	import { flyAndScale } from "../utils/transitions.ts"
	import build from "../../../../build.json"

	let trialOver = false

	// Check if trial is over
	/* if ($settings.licenseActivated) {
		let dateActivated = new Date($settings.licenseActivated)
		let dateNow = new Date()
		let diff = dateNow.getTime() - dateActivated.getTime()
		let days = Math.ceil(diff / (1000 * 3600 * 24))

		if (days > 7) {
			trialOver = true
		}
	} */

	const step2 = () => {
		document.querySelector(".step1").classList.add("hidden")
		document.querySelector(".step2").classList.remove("hidden")
		document.querySelector(".step2").classList.add("flex")
	}

	const step3 = () => {
		document.querySelector(".step2").classList.add("hidden")
		document.querySelector(".step3").classList.remove("hidden")
		document.querySelector(".step3").classList.add("flex")
	}

	const free = () => {
		$settings.licenseKey = "free"
		$settings.licenseActivated = new Date().toISOString()
		setTimeout(() => {
			step3()
		}, 250)
	}

	const activate = async () => {
		const key = document.querySelector("#key") as HTMLInputElement

		if (key.value !== "") {
			const url = "https://api.lemonsqueezy.com/v1/licenses/activate"
			const options = {
				method: "POST",
				headers: { "content-type": "application/json" },
				body: JSON.stringify({ license_key: key.value, instance_name: `CoresDesktop-${build.number}` }),
			}

			try {
				const response = await fetch(url, options)
				const data = await response.json()

				if (data.activated && data?.meta.store_id === 62942) {
					$settings.licenseKey = key.value
					$settings.licenseActivated = new Date().toISOString()
					step3()
				} else {
					alert(`Failed to activate: ${data.error}. Please reach out to cores@levminer.com if you need help.`)
				}
			} catch (error) {
				alert("Failed to send activation request, please try again or reach out to cores@levminer.com if you need help.")
				console.error(error)
			}
		}
	}
</script>
