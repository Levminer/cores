using HidSharp.Utility;
using lib;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Runtime.Versioning;
using System.Text.Json;
using System.Timers;

namespace cli;

class Program {
	internal static HardwareInfo HardwareInfo = new();
	internal static System.Timers.Timer Timer = new();
	internal static JsonSerializerOptions SerializerOptions = new() {
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		WriteIndented = true
	};

	// Handle app crash
	private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
		Debug.WriteLine("App crashed");
		SentrySdk.CaptureException((Exception)e.ExceptionObject);
	}

	static void Main(string[] args) {
		SentrySdk.Init(settings => {
			settings.Dsn = "https://9d82458dfddf56230ce675882bcc093a@o4506670275428352.ingest.sentry.io/4506671395897344";
			settings.AutoSessionTracking = true;
			settings.IsGlobalModeEnabled = true;
			settings.EnableTracing = true;
		});

		AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

		Task.Run(() => {
			HardwareInfo.GetInfo();
		});

		var mes = JsonSerializer.Serialize(HardwareInfo.API);
		Console.WriteLine(mes);

		Timer = new System.Timers.Timer(3000);
		Timer.Elapsed += RefreshAPI;
		Timer.AutoReset = true;
		Timer.Enabled = true;
		Timer.Start();

		Console.ReadLine();
	}

	public static void RefreshAPI(Object source, ElapsedEventArgs e) {
		Task.Run(() => {
			HardwareInfo.Refresh();
		});

		//Console.WriteLine(hw.);
		//Console.WriteLine(hw.gpuUsage.d3dVideoEncode);
		//Console.WriteLine(hw.gpuUsage.d3dVideoDecode);
		//Console.WriteLine(hw.gpuUsage.d3dCopy);
		//Console.WriteLine("------");

		var mes = JsonSerializer.Serialize(HardwareInfo.API);
		Console.WriteLine(mes);
	}
}

//public class GPUUsage {
//	public float d3d3D {
//		get; set;
//	}

//	public float d3dVideoEncode {
//		get; set;
//	}

//	public float d3dVideoDecode {
//		get; set;
//	}

//	public float d3dCopy {
//		get; set;
//	}
//}

//public class Counters {
//	public string Type {
//		get; set;
//	}

//	public PerformanceCounter Counter {
//		get; set;
//	}
//}

//[SupportedOSPlatform("Windows")]
//public class GPULoad {
//	static PerformanceCounterCategory gpuEngineCategory = new("GPU Engine");
//	string[] gpuEngineNames = gpuEngineCategory.GetInstanceNames();
//	static List<Counters> d3dCounters = new();
//	public GPUUsage gpuUsage = new();

//	public async void GetInfo() {
//		gpuEngineCategory = new PerformanceCounterCategory("GPU Engine");
//		gpuEngineNames = gpuEngineCategory.GetInstanceNames();

//		d3dCounters.Clear();
//		gpuUsage = new GPUUsage();

//		foreach (string counterName in gpuEngineNames) {
//			if (!gpuEngineCategory.InstanceExists(counterName)) {
//				continue; // Skip this instance if it doesn't exist
//			}

//			if (counterName.EndsWith("engtype_3D")) {
//				foreach (PerformanceCounter counter in gpuEngineCategory.GetCounters(counterName)) {
//					if (counter.CounterName == "Utilization Percentage") {
//						d3dCounters.Add(new Counters { Counter = counter, Type = "3d" });
//					}
//				}
//			}

//			if (counterName.EndsWith("engtype_VideoEncode")) {
//				foreach (PerformanceCounter counter in gpuEngineCategory.GetCounters(counterName)) {
//					if (counter.CounterName == "Utilization Percentage") {
//						d3dCounters.Add(new Counters { Counter = counter, Type = "Video Encode" });
//					}
//				}
//			}

//			if (counterName.EndsWith("engtype_VideoDecode")) {
//				foreach (PerformanceCounter counter in gpuEngineCategory.GetCounters(counterName)) {
//					if (counter.CounterName == "Utilization Percentage") {
//						d3dCounters.Add(new Counters { Counter = counter, Type = "Video Decode" });
//					}
//				}
//			}

//			if (counterName.EndsWith("engtype_Copy")) {
//				foreach (PerformanceCounter counter in gpuEngineCategory.GetCounters(counterName)) {
//					if (counter.CounterName == "Utilization Percentage") {
//						d3dCounters.Add(new Counters { Counter = counter, Type = "Copy" });
//					}
//				}
//			}
//		}

//		d3dCounters.ForEach(x => {
//			_ = x.Counter.NextValue();
//		});

//		await Task.Delay(1000);

//		for (var i = 0; i < d3dCounters.Count; i++) {
//			var x = d3dCounters[i];

//			if (PerformanceCounterCategory.CounterExists(x.Counter.CounterName, "GPU Engine")) {
//				var val = x.Counter.NextValue();

//				if (val > 0) {
//					switch (x.Type) {
//						case "3d":
//							gpuUsage.d3d3D += val;
//							break;
//						case "Video Encode":
//							gpuUsage.d3dVideoEncode += val;
//							break;
//						case "Video Decode":
//							gpuUsage.d3dVideoDecode += val;
//							break;
//						case "Copy":
//							gpuUsage.d3dCopy += val;
//							break;
//					}
//				}
//			} else {
//				x.Counter.Dispose();
//				d3dCounters.RemoveAt(i);
//			}
//		}
//	}
//}
