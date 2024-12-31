# Cores

-   Cores is a hardware monitor background service with remote connection support and a modern UI. With Cores, you can monitor temperature sensors, fan speeds, voltages, load sensors, and clock speeds. It allows you to view detailed information about your motherboard, CPU, GPU, RAM, BIOS, drives, fans, monitors, and network interfaces. Additionally, Cores enables you to monitor your system remotely from anywhere using a web browser.

## Features

-   💻 Monitor CPU/RAM/GPU usage like clock speed, voltage, memory usage and load
-   📡 Remote monitoring, you can monitor your system from any device with a web browser
-   🌡️ CPU/GPU/Drive temperatures
-   📈 See historical charts 
-   💾 Keep an eye on your SSD health and usage
-   ❄️ Fan speed and RPM information
-   🛜 Network speed and usage stats
-   🔋 Battery health, cycles and capacity

## Screenshot

<img src="https://raw.githubusercontent.com/Levminer/cores/dev/.github/screenshots/home.png?raw=true">

## Download

-   Latest release version for users that want a stable and polished experience.

[![Latest release](https://img.shields.io/github/v/release/levminer/cores?label=Release)](https://github.com/Levminer/cores/releases/latest)
[![Download](https://img.shields.io/badge/Windows,%20Linux,%20macOS-download-brightgreen)](https://www.coresmonitor.com/#downloads)
[![Updated](https://img.shields.io/github/last-commit/levminer/cores/dev?color=yellowgreen&label=Updated)](https://github.com/Levminer/cores)

-   Also available on the [Microsoft Store](https://link.levminer.com/cores-ms-store).

## Project structure and motivation

-   Cores is a background service that runs in the background and monitors your computer's hardware components. It's built on top of [Libre Hardware Monitor](https://github.com/LibreHardwareMonitor/LibreHardwareMonitor). It provides additional features like GPU usage and drive health monitoring. On Windows it's written in C# and runs as a Windows service. It provides an option to connect to your computer remotely with a P2P connection for monitoring. It also provides a REST API and a WebSocket server if you want to use the data.

-   The UI is built with Svelte and it's available on the desktop as a Tauri desktop app. You can access it on the [website](https://www.coresmonitor.com/home) and you can remotely connect to your computer if you enabled remote connections and copied your connection code.

## License

-   This software is licensed under: [GPL-3.0](https://github.com/Levminer/cores/blob/dev/LICENSE.md)
-   You can buy the software as an individual on the [website](https://www.coresmonitor.com/#pricing). If you are planning to use this software as a business please contact me at: support@coresmonitor.com
-   Credits: [Libre Hardware Monitor](https://github.com/LibreHardwareMonitor/LibreHardwareMonitor), [Mac Monitor](https://github.com/vladkens/macmon), [Resources](https://github.com/nokyan/resources)
