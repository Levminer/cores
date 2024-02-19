using System.Diagnostics;
using System.Runtime.Versioning;

namespace lib;
public class Counters {
	public string Type {
		get; set;
	}

	public PerformanceCounter Counter {
		get; set;
	}
}
[SupportedOSPlatform("Windows")]
public class GPULoad {
	public List<Sensor> GPUUsage { get; set; } = new List<Sensor> { new() { Name = "3D" }, new() { Name = "Copy" }, new() { Name = "Video Encode" }, new() { Name = "Video Decode" } };
	public float GPULastLoad = 0;

	public async Task GetInfo() {
		var gpuEngineCategory = new PerformanceCounterCategory("GPU Engine");
		var gpuEngineNames = gpuEngineCategory.GetInstanceNames();
		var d3dCounters = new List<Counters>();
		GPUUsage = new List<Sensor> { new() { Name = "3D" }, new() { Name = "Copy" }, new() { Name = "Video Encode" }, new() { Name = "Video Decode" } };
		GPULastLoad = 0;

		foreach (string counterName in gpuEngineNames) {
			if (!gpuEngineCategory.InstanceExists(counterName)) {
				continue; // Skip this instance if it doesn't exist
			}

			if (counterName.EndsWith("engtype_3D")) {
				foreach (PerformanceCounter counter in gpuEngineCategory.GetCounters(counterName)) {
					if (counter.CounterName == "Utilization Percentage") {
						d3dCounters.Add(new Counters { Counter = counter, Type = "3d" });
					}
				}
			}

			if (counterName.EndsWith("engtype_VideoEncode")) {
				foreach (PerformanceCounter counter in gpuEngineCategory.GetCounters(counterName)) {
					if (counter.CounterName == "Utilization Percentage") {
						d3dCounters.Add(new Counters { Counter = counter, Type = "Video Encode" });
					}
				}
			}

			if (counterName.EndsWith("engtype_VideoDecode")) {
				foreach (PerformanceCounter counter in gpuEngineCategory.GetCounters(counterName)) {
					if (counter.CounterName == "Utilization Percentage") {
						d3dCounters.Add(new Counters { Counter = counter, Type = "Video Decode" });
					}
				}
			}

			if (counterName.EndsWith("engtype_Copy")) {
				foreach (PerformanceCounter counter in gpuEngineCategory.GetCounters(counterName)) {
					if (counter.CounterName == "Utilization Percentage") {
						d3dCounters.Add(new Counters { Counter = counter, Type = "Copy" });
					}
				}
			}
		}

		for (int i = 0; i < d3dCounters.Count; i++) {
			var c = d3dCounters[i];

			try {
				c.Counter.NextValue();
			}
			catch (Exception) {
				// Counter is not available
				Debug.WriteLine("Counter not available");
			}
		}

		// Wait 1000ms to gather data
		await Task.Delay(1000);

		for (var i = 0; i < d3dCounters.Count; i++) {
			try {
				var c = d3dCounters[i];
				var val = c.Counter.NextValue();

				if (val > 0) {
					switch (c.Type) {
						case "3d":
							GPUUsage[0].Value += val;
							break;
						case "Copy":
							GPUUsage[1].Value += val;
							break;
						case "Video Encode":
							GPUUsage[2].Value += val;
							break;
						case "Video Decode":
							GPUUsage[3].Value += val;
							break;

					}
				}
			}
			catch (Exception) {
				// Counter is not available
				Debug.WriteLine("Counter not available");
			}
		}

		GPULastLoad = GPUUsage.Max(t => t.Value);
	}
}
