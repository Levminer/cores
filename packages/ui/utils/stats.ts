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

			load: Math.round(input.cpu.lastLoad),
			power: Math.round(input.cpu.power.reduce((a, b) => a + b.value, 0)),
			voltage: parseFloat((input.cpu.voltage.reduce((a, b) => a + b.value, 0) / input.cpu.clock.length).toFixed(2)),
		},

		ram: {
			physicalUsage: Math.round(input.ram.load[2].value),
			virtualUsage: Math.round(input.ram.load[5].value),
		},

		gpu: {
			temperature: {
				value: Math.round(input.gpu.temperature.map((sensor) => sensor.value).reduce((a, b) => a + b, 0) / input.gpu.temperature.length),
				min: Math.round(input.gpu.temperature.map((sensor) => sensor.min).reduce((a, b) => a + b, 0) / input.gpu.temperature.length),
				max: Math.round(input.gpu.temperature.map((sensor) => sensor.max).reduce((a, b) => a + b, 0) / input.gpu.temperature.length),
			},

			clock: {
				value: Math.round(input.gpu.clock[0].value),
				min: Math.round(input.gpu.clock[0].min),
				max: Math.round(input.gpu.clock[0].max),
			},

			load: Math.round(input.gpu.lastLoad),
			power: Math.round(input.gpu.power.reduce((a, b) => a + b.value, 0)),
			fan: Math.round(input.gpu.fan.reduce((a, b) => a + b.value, 0)),
			memory: parseFloat(input.gpu.memory[0].value.toFixed(1)),
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
	}
}

export const generateMinutesData = (input: HardwareInfo, $hardwareStatistics: HardwareStatistics): Stats => {
	return {
		cpu: {
			temperature: {
				value: Math.round(
					$hardwareStatistics.seconds.map((sensor) => sensor.cpu.temperature.value).reduce((a, b) => a + b, 0) /
						$hardwareStatistics.seconds.length,
				),
				min: Math.round(
					$hardwareStatistics.seconds.map((sensor) => sensor.cpu.temperature.min).reduce((a, b) => a + b, 0) /
						$hardwareStatistics.seconds.length,
				),
				max: Math.round(
					$hardwareStatistics.seconds.map((sensor) => sensor.cpu.temperature.max).reduce((a, b) => a + b, 0) /
						$hardwareStatistics.seconds.length,
				),
			},

			clock: {
				value: Math.round(
					$hardwareStatistics.seconds.map((sensor) => sensor.cpu.clock.value).reduce((a, b) => a + b, 0) /
						$hardwareStatistics.seconds.length,
				),
				min: Math.round(
					$hardwareStatistics.seconds.map((sensor) => sensor.cpu.clock.min).reduce((a, b) => a + b, 0) / $hardwareStatistics.seconds.length,
				),
				max: Math.round(
					$hardwareStatistics.seconds.map((sensor) => sensor.cpu.clock.max).reduce((a, b) => a + b, 0) / $hardwareStatistics.seconds.length,
				),
			},

			load: Math.round(
				$hardwareStatistics.seconds.map((sensor) => sensor.cpu.load).reduce((a, b) => a + b, 0) / $hardwareStatistics.seconds.length,
			),
			power: Math.round(
				$hardwareStatistics.seconds.map((sensor) => sensor.cpu.power).reduce((a, b) => a + b, 0) / $hardwareStatistics.seconds.length,
			),
			voltage: parseFloat(
				(
					$hardwareStatistics.seconds.map((sensor) => sensor.cpu.voltage).reduce((a, b) => a + b, 0) / $hardwareStatistics.seconds.length
				).toFixed(2),
			),
		},

		ram: {
			physicalUsage: Math.round(
				$hardwareStatistics.seconds.map((sensor) => sensor.ram.physicalUsage).reduce((a, b) => a + b, 0) / $hardwareStatistics.seconds.length,
			),
			virtualUsage: Math.round(
				$hardwareStatistics.seconds.map((sensor) => sensor.ram.virtualUsage).reduce((a, b) => a + b, 0) / $hardwareStatistics.seconds.length,
			),
		},

		gpu: {
			temperature: {
				value: Math.round(
					$hardwareStatistics.seconds.map((sensor) => sensor.gpu.temperature.value).reduce((a, b) => a + b, 0) /
						$hardwareStatistics.seconds.length,
				),
				min: Math.round(
					$hardwareStatistics.seconds.map((sensor) => sensor.gpu.temperature.min).reduce((a, b) => a + b, 0) /
						$hardwareStatistics.seconds.length,
				),
				max: Math.round(
					$hardwareStatistics.seconds.map((sensor) => sensor.gpu.temperature.max).reduce((a, b) => a + b, 0) /
						$hardwareStatistics.seconds.length,
				),
			},

			clock: {
				value: Math.round(
					$hardwareStatistics.seconds.map((sensor) => sensor.gpu.clock.value).reduce((a, b) => a + b, 0) /
						$hardwareStatistics.seconds.length,
				),
				min: Math.round(
					$hardwareStatistics.seconds.map((sensor) => sensor.gpu.clock.min).reduce((a, b) => a + b, 0) / $hardwareStatistics.seconds.length,
				),
				max: Math.round(
					$hardwareStatistics.seconds.map((sensor) => sensor.gpu.clock.max).reduce((a, b) => a + b, 0) / $hardwareStatistics.seconds.length,
				),
			},

			load: Math.round(
				$hardwareStatistics.seconds.map((sensor) => sensor.gpu.load).reduce((a, b) => a + b, 0) / $hardwareStatistics.seconds.length,
			),
			power: Math.round(
				$hardwareStatistics.seconds.map((sensor) => sensor.gpu.power).reduce((a, b) => a + b, 0) / $hardwareStatistics.seconds.length,
			),
			fan: Math.round(
				$hardwareStatistics.seconds.map((sensor) => sensor.gpu.fan).reduce((a, b) => a + b, 0) / $hardwareStatistics.seconds.length,
			),
			memory: parseFloat(
				(
					$hardwareStatistics.seconds.map((sensor) => sensor.gpu.memory).reduce((a, b) => a + b, 0) / $hardwareStatistics.seconds.length
				).toFixed(1),
			),
		},

		network: input.system.network.interfaces.map((item, i) => {
			let throughputDownload = parseFloat(
				(
					$hardwareStatistics.seconds.map((sensor) => sensor.network[i]).reduce((a, b) => a + b.throughputDownload, 0) /
					$hardwareStatistics.seconds.length
				).toFixed(2),
			)

			let throughputUpload = parseFloat(
				(
					$hardwareStatistics.seconds.map((sensor) => sensor.network[i]).reduce((a, b) => a + b.throughputUpload, 0) /
					$hardwareStatistics.seconds.length
				).toFixed(2),
			)

			let downloadedData = parseFloat(
				(
					$hardwareStatistics.seconds.map((sensor) => sensor.network[i]).reduce((a, b) => a + b.downloadedData, 0) /
					$hardwareStatistics.seconds.length
				).toFixed(2),
			)

			let uploadedData = parseFloat(
				(
					$hardwareStatistics.seconds.map((sensor) => sensor.network[i]).reduce((a, b) => a + b.uploadedData, 0) /
					$hardwareStatistics.seconds.length
				).toFixed(2),
			)

			return {
				throughputUpload,
				throughputDownload,
				downloadedData,
				uploadedData,
			}
		}),

		storage: input.system.storage.disks.map((item, i) => {
			let throughputRead = parseFloat(
				(
					$hardwareStatistics.seconds.map((sensor) => sensor.storage[i]).reduce((a, b) => a + b.throughputRead, 0) /
					$hardwareStatistics.seconds.length
				).toFixed(2),
			)

			let throughputWrite = parseFloat(
				(
					$hardwareStatistics.seconds.map((sensor) => sensor.storage[i]).reduce((a, b) => a + b.throughputWrite, 0) /
					$hardwareStatistics.seconds.length
				).toFixed(2),
			)

			let temperature = {
				value: Math.round(
					$hardwareStatistics.seconds.map((sensor) => sensor.storage[i].temperature.value).reduce((a, b) => a + b, 0) /
						$hardwareStatistics.seconds.length,
				),
				min: Math.round(
					$hardwareStatistics.seconds.map((sensor) => sensor.storage[i].temperature.min).reduce((a, b) => a + b, 0) /
						$hardwareStatistics.seconds.length,
				),
				max: Math.round(
					$hardwareStatistics.seconds.map((sensor) => sensor.storage[i].temperature.max).reduce((a, b) => a + b, 0) /
						$hardwareStatistics.seconds.length,
				),
			}

			return {
				throughputRead,
				throughputWrite,
				temperature,
			}
		}),
	}
}
