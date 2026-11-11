// Average of a sensor field, 0 when there are no sensors
export const averageSensors = (sensors: Sensor[], field: "value" | "min" | "max" = "value"): number => {
	if (sensors.length === 0) {
		return 0
	}

	return sensors.reduce((a, b) => a + (b[field] ?? 0), 0) / sensors.length
}

// The package sensor already includes the cores, graphics and memory sensors, so they are only summed if it's missing
export const cpuPowerUsage = (sensors: Sensor[]): number => {
	const cpuPackage = sensors.find((sensor) => sensor.name?.includes("Package"))

	return cpuPackage ? cpuPackage.value : sensors.reduce((a, b) => a + b.value, 0)
}

export const generateSecondsData = (input: HardwareInfo): Stats => {
	return {
		cpu: {
			temperature: {
				value: Math.round(averageSensors(input.cpu.temperature, "value")),
				min: Math.round(averageSensors(input.cpu.temperature, "min")),
				max: Math.round(averageSensors(input.cpu.temperature, "max")),
			},

			clock: {
				value: Math.round(averageSensors(input.cpu.clock, "value")),
				min: Math.round(averageSensors(input.cpu.clock, "min")),
				max: Math.round(averageSensors(input.cpu.clock, "max")),
			},

			load: Math.round(input.cpu.maxLoad),
			power: Math.round(cpuPowerUsage(input.cpu.power)),
			voltage: parseFloat(averageSensors(input.cpu.voltage).toFixed(2)),
		},

		ram: {
			physicalUsage: Math.round(input.ram.load[2]?.value ?? 0),
			virtualUsage: Math.round(input.ram.load[5]?.value ?? 0),
			temperature:
				input.ram.temperature && input.ram.temperature.length > 0
					? {
							value: Math.round(averageSensors(input.ram.temperature, "value")),
							min: Math.round(averageSensors(input.ram.temperature, "min")),
							max: Math.round(averageSensors(input.ram.temperature, "max")),
						}
					: undefined,
		},

		gpu: {
			cards:
				input.gpu.cards?.map((card) => {
					return {
						temperature: {
							value: Math.round(averageSensors(card.temperature, "value")),
							min: Math.round(averageSensors(card.temperature, "min")),
							max: Math.round(averageSensors(card.temperature, "max")),
						},

						clock: {
							value: Math.round(card.clock[0]?.value ?? 0),
							min: Math.round(card.clock[0]?.min ?? 0),
							max: Math.round(card.clock[0]?.max ?? 0),
						},

						load: Math.round(card.maxLoad),
						power: Math.round(card.power.reduce((a, b) => a + b.value, 0)),
						fan: Math.round(card.fan.reduce((a, b) => a + b.value, 0)),
						memory: parseFloat((card.memory[0]?.value ?? 0).toFixed(1)),
					}
				}) ?? [],
		},

		network: input.system.network.interfaces.map((int) => {
			return {
				throughputUpload: parseFloat((int.throughputUpload / 1_048_576).toFixed(2)),
				throughputDownload: parseFloat((int.throughputDownload / 1_048_576).toFixed(2)),
				downloadedData: parseFloat(int.downloadData.toFixed(2)),
				uploadedData: parseFloat(int.uploadData.toFixed(2)),
			}
		}),

		storage: input.system.storage.disks.map((item) => {
			return {
				throughputRead: parseFloat((item.throughputRead / 1_048_576).toFixed(2)),
				throughputWrite: parseFloat((item.throughputWrite / 1_048_576).toFixed(2)),
				temperature: {
					value: Math.round(item.temperature.value),
					min: Math.round(item.temperature.min),
					max: Math.round(item.temperature.max),
				},
			}
		}),

		fan: input.system.superIO.fan
			.filter((item) => item.value !== 0)
			.map((item, i) => {
				return {
					speed: {
						value: Math.round(item.value),
						min: Math.round(item.min),
						max: Math.round(item.max),
					},
					control: {
						value: Math.round(input.system.superIO.fanControl[i].value),
						min: Math.round(input.system.superIO.fanControl[i].min),
						max: Math.round(input.system.superIO.fanControl[i].max),
					},
				}
			}),

		timestamp: input.timestamp,
	}
}

// The server records one minutes point per 15 minutes and keeps them for 24 hours
const MINUTES_RETENTION = 24 * 60 * 60 * 1000

// The server sends its history oldest first on every connect, and after that only its newest point.
// So points the list holds that are newer than an incoming one are no longer on the server (e.g. its clock was corrected),
// those are dropped. Points the list already has are skipped and points older than 24 hours are dropped.
export const appendMinutesData = (minutes: Stats[], input: HardwareInfo): Stats[] => {
	const point = generateSecondsData(input)
	const time = Date.parse(point.timestamp ?? "")

	if (Number.isNaN(time)) {
		return minutes
	}

	const cutoff = Date.now() - MINUTES_RETENTION
	const kept = minutes.filter((item) => {
		const itemTime = Date.parse(item.timestamp ?? "")
		return itemTime >= cutoff && itemTime <= time
	})
	const exists = kept.some((item) => Date.parse(item.timestamp ?? "") === time)

	if (exists && kept.length === minutes.length) {
		return minutes
	}

	return exists ? kept : [...kept, point]
}
