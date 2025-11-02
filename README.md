# Cores

- Cores is a hardware monitor background service with remote connection support and a modern UI. With Cores, you can monitor temperature sensors, fan speeds, voltages, load sensors, and clock speeds. It allows you to view detailed information about your motherboard, CPU, GPU, RAM, BIOS, drives, fans, monitors, and network interfaces. Additionally, Cores enables you to monitor your system remotely from anywhere using a web browser.

## Features

- 💻 Monitor CPU/RAM/GPU usage like clock speed, voltage, memory usage and load
- 📡 Remote monitoring, you can monitor your system from any device with a web browser
- 🌡️ CPU/GPU/Drive temperatures
- 📈 See historical charts
- 💾 Keep an eye on your SSD health and usage
- ❄️ Fan speed and RPM information
- 🛜 Network speed and usage stats
- 🔋 Battery health, cycles and capacity

## Screenshot

<img src="https://raw.githubusercontent.com/Levminer/cores/dev/.github/screenshots/home.png?raw=true">

## Download

- Latest release version for users that want a stable and polished experience.

[![Latest release](https://img.shields.io/github/v/release/levminer/cores?label=Release)](https://github.com/Levminer/cores/releases/latest)
[![Download](https://img.shields.io/badge/Windows,%20Linux,%20macOS-download-brightgreen)](https://www.coresmonitor.com/#downloads)
[![Updated](https://img.shields.io/github/last-commit/levminer/cores/dev?color=yellowgreen&label=Updated)](https://github.com/Levminer/cores)

- Also available on the [Microsoft Store](https://link.levminer.com/cores-ms-store).

## Project structure and motivation

- Cores is a background service that runs in the background and monitors your computer's hardware components. On Windows it's built on top of [Libre Hardware Monitor](https://github.com/LibreHardwareMonitor/LibreHardwareMonitor), it provides additional features like GPU usage and drive health monitoring, it's written in C# and runs as a Windows service. On Linux it's written in Rust and runs as a daemon in the background or when you launch the desktop app. The background service/daemon provides an option to connect to your computer remotely with a P2P connection powered by WebRTC for monitoring. It also provides a REST API and a WebSocket server if you want to use the data. The UI is built with Svelte and it's available on the desktop as a Tauri desktop app or you can access the [dashboard](https://www.coresmonitor.com/home) in a web browser and you can remotely connect to your computer if you enabled remote connections.

1. Desktop (Tauri)
1. Daemon on Linux and macOS (Rust), Background service on Windows (C#)
1. Web dashboard (Svelte)
1. Remote connection server (Rust)

## Self-hosting

You can use this simple docker compose file, you can host both the dashboard and the server on your own machine. Make sure you have Docker installed and running.

```yml
services:
    dashboard:
        image: ghcr.io/levminer/cores/dashboard:latest
        ports:
            - "3000:3000"
    server:
        image: ghcr.io/levminer/cores/server:latest
        ports:
            - "9001:9001"
```

You have to change the connection server URL to point to your server URL.

1. Windows: `%PROGRAMDATA%\Cores\settings.json` > connectionURL > Restart the service
1. Linux: `$HOME/.config/Cores/settings.json` > connectionURL > Restart the daemon

## License

- This software is licensed under: [GPL-3.0](https://github.com/Levminer/cores/blob/dev/LICENSE.md)
- You can buy the software as an individual on the [website](https://www.coresmonitor.com/#pricing). If you are planning to use this software as a business please contact me at: support@coresmonitor.com
- Credits: [Libre Hardware Monitor](https://github.com/LibreHardwareMonitor/LibreHardwareMonitor), [Mac Monitor](https://github.com/vladkens/macmon), [Resources](https://github.com/nokyan/resources)
