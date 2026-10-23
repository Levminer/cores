export const generateSecondsData = (input: HardwareInfo): Stats => {
	return {
		cpu: {
			temperature: {
				value: Math.round(input.cpu.temperature.map((sensor) => sensor.value).reduce((a, b) => a + b, 0) / input.cpu.temperature.length),
				min: Math.round(input.cpu.temperature.map((sensor) => sensor.min).reduce((a, b) => a + b, 0) / input.cpu.temperature.length),
				max: Math.round(input.cpu.temperature.map((sensor) => sensor.max).reduce((a, b) => a + b, 0) / input.cpu.temperature.length),
			},

			clock: {
				value: Math.round(input.cpu.clock.map((sensor) => sensor.value).reduce((a, b) => a + b, 0) / input.cpu.clock.length),
				min: Math.round(input.cpu.clock.map((sensor) => sensor.min).reduce((a, b) => a + b, 0) / input.cpu.clock.length),
				max: Math.round(input.cpu.clock.map((sensor) => sensor.max).reduce((a, b) => a + b, 0) / input.cpu.clock.length),
			},

			load: Math.round(input.cpu.maxLoad),
			power: Math.round(input.cpu.power.reduce((a, b) => a + b.value, 0)),
			voltage: parseFloat((input.cpu.voltage.reduce((a, b) => a + b.value, 0) / input.cpu.clock.length).toFixed(2)),
		},

		ram: {
			physicalUsage: Math.round(input.ram.load[2]?.value ?? 0),
			virtualUsage: Math.round(input.ram.load[5]?.value ?? 0),
			temperature:
				input.ram.temperature && input.ram.temperature.length > 0
					? {
							value: Math.round(
								input.ram.temperature.map((sensor) => sensor.value).reduce((a, b) => a + b, 0) / input.ram.temperature.length,
							),
							min: Math.round(
								input.ram.temperature.map((sensor) => sensor.min).reduce((a, b) => a + b, 0) / input.ram.temperature.length,
							),
							max: Math.round(
								input.ram.temperature.map((sensor) => sensor.max).reduce((a, b) => a + b, 0) / input.ram.temperature.length,
							),
						}
					: undefined,
		},

		gpu: {
			cards:
				input.gpu.cards?.map((card) => {
					return {
						temperature: {
							value: Math.round(card.temperature.map((sensor) => sensor.value).reduce((a, b) => a + b, 0) / card.temperature.length),
							min: Math.round(card.temperature.map((sensor) => sensor.min).reduce((a, b) => a + b, 0) / card.temperature.length),
							max: Math.round(card.temperature.map((sensor) => sensor.max).reduce((a, b) => a + b, 0) / card.temperature.length),
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

// The server sends one minutes point per window and resends its whole history on every connect.
// Skip points the list already has and drop points older than 24 hours.
export const appendMinutesData = (minutes: Stats[], input: HardwareInfo): Stats[] => {
	const point = generateSecondsData(input)
	const last = minutes.at(-1)

	if (last?.timestamp && point.timestamp && Date.parse(point.timestamp) <= Date.parse(last.timestamp)) {
		return minutes
	}

	const cutoff = Date.now() - 24 * 60 * 60 * 1000

	return [...minutes.filter((item) => Date.parse(item.timestamp ?? "") >= cutoff), point]
}
