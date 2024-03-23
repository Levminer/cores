using HidSharp.Utility;
using lib;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Runtime.Versioning;
using System.Text.Json;
using System.Timers;

namespace cli;

internal class Program {
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

	public static void Main(string[] args) {
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

		var mes = JsonSerializer.Serialize(HardwareInfo.API, SerializerOptions);
		Console.WriteLine(mes);

		Timer = new System.Timers.Timer(3000);
		Timer.Elapsed += RefreshAPI;
		Timer.AutoReset = true;
		Timer.Enabled = true;
		Timer.Start();

		Console.ReadLine();
	}

	public static void RefreshAPI(object source, ElapsedEventArgs e) {
		Task.Run(() => {
			HardwareInfo.Refresh();
		});

		var mes = JsonSerializer.Serialize(HardwareInfo.API);
		Console.WriteLine(mes);
	}
}
