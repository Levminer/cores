#![cfg_attr(not(debug_assertions), windows_subsystem = "windows")]

use std::sync::Mutex;
use tauri::{
    menu::{MenuBuilder, MenuItemBuilder},
    tray::{MouseButton, MouseButtonState, TrayIconEvent},
    Manager,
};
use tauri_plugin_shell::{
    process::{CommandChild, CommandEvent},
    ShellExt,
};

pub mod service;
pub mod settings;
pub mod utils;

use hardwareinfo::settings::WindowState;

struct GlobalState {
    child: Option<CommandChild>,
}

fn main() {
    let _sentry = sentry::init((
        "https://da874903dead91a5de908b045a106aca@o4506670275428352.ingest.us.sentry.io/4507476699578368",
        sentry::ClientOptions {
            release: sentry::release_name!(),
            auto_session_tracking: true,
            traces_sample_rate: 0.8,
            ..Default::default()
        },
    ));

    tauri::Builder::default()
        .plugin(tauri_plugin_shell::init())
        .plugin(tauri_plugin_dialog::init())
        .plugin(tauri_plugin_process::init())
        .plugin(tauri_plugin_oauth::init())
        .plugin(tauri_plugin_updater::Builder::new().build())
        .plugin(tauri_plugin_single_instance::init(|app, _args, _cwd| {
            let window = app
                .get_webview_window("main")
                .expect("Failed to get webview window");

            window.show().expect("Failed to show window");
            window.set_focus().expect("Failed to set focus");
        }))
        .invoke_handler(tauri::generate_handler![
            settings::get_settings,
            settings::set_settings,
            settings::save_window_state,
            service::start_service,
            service::stop_service,
            service::restart_service,
            utils::system_info
        ])
        .setup(|app| {
            app.manage(Mutex::new(GlobalState { child: None }));

            // Restore window state
            let settings = settings::get_settings();
            let window = app
                .get_webview_window("main")
                .expect("Failed to get main window");

            // Try to find the monitor where the window was previously located
            if let Ok(available_monitors) = window.available_monitors() {
                let monitor_index = settings.window_state.monitor_index as usize;
                let target_monitor = available_monitors.get(monitor_index)
                    .or_else(|| available_monitors.first());

                if let Some(monitor) = target_monitor {
                    // Calculate position relative to the monitor
                    let monitor_x = monitor.position().x;
                    let monitor_y = monitor.position().y;
                    
                    // Set position on the correct monitor
                    let _ = window.set_position(tauri::PhysicalPosition::new(
                        monitor_x + settings.window_state.x,
                        monitor_y + settings.window_state.y,
                    ));
                    let _ = window.set_size(tauri::PhysicalSize::new(
                        settings.window_state.width,
                        settings.window_state.height,
                    ));
                    
                    if settings.window_state.maximized {
                        let _ = window.maximize();
                    }
                }
            }

            let toggle_window_item =
                MenuItemBuilder::with_id("toggle_windows", "Show/Hide Cores").build(app)?;
            let exit_item = MenuItemBuilder::with_id("exit", "Exit").build(app)?;
            let menu = MenuBuilder::new(app)
                .items(&[&toggle_window_item, &exit_item])
                .build()?;

            let tray = app.tray_by_id("main").expect("Failed to get tray");

            if cfg!(target_os = "windows") {
                tray.set_show_menu_on_left_click(false).unwrap();
            }

            tray.set_menu(Some(menu)).expect("Failed to set menu");
            tray.on_menu_event(move |app, event| match event.id().as_ref() {
                "toggle_windows" => {
                    let window = app
                        .get_webview_window("main")
                        .expect("Failed to get window");

                    if window.is_visible().expect("Failed to check visibility") {
                        window.hide().expect("Failed to hide window");
                    } else {
                        window.show().expect("Failed to show window");
                        window.set_focus().expect("Failed to set focus");
                    }
                }
                "exit" => {
                    app.exit(0);
                }
                _ => (),
            });

            if cfg!(target_os = "windows") {
                tray.on_tray_icon_event(|tray, event| {
                    if let TrayIconEvent::Click {
                        button: MouseButton::Left,
                        button_state: MouseButtonState::Up,
                        ..
                    } = event
                    {
                        let app = tray.app_handle();
                        let window = app
                            .get_webview_window("main")
                            .expect("Failed to get window");

                        if window.is_visible().expect("Failed to check visibility") {
                            window.hide().expect("Failed to hide window");
                        } else {
                            window.show().expect("Failed to show window");
                            window.set_focus().expect("Failed to set focus");
                        }
                    }
                });
            }

            if cfg!(target_os = "linux") || cfg!(target_os = "macos") {
                let sidecar_command = app
                    .shell()
                    .sidecar("coresd")
                    .expect("Failed to get sidecar");
                let (mut rx, child) = sidecar_command.spawn().expect("Failed to spawn sidecar");

                let state = app.state::<Mutex<GlobalState>>();
                state.lock().expect("Failed to lock state").child = Some(child);

                tauri::async_runtime::spawn(async move {
                    // read events such as stdout
                    while let Some(event) = rx.recv().await {
                        if let CommandEvent::Stdout(line_bytes) = event {
                            let line = String::from_utf8_lossy(&line_bytes);
                            println!("sidecar stdout: {}", line);
                        }
                    }
                });
            }

            Ok(())
        })
        .on_window_event(|window, event| match event {
            tauri::WindowEvent::CloseRequested { api, .. } => {
                api.prevent_close();

                let settings = settings::get_settings();

                if settings.minimize_to_tray {
                    window.hide().expect("Failed to hide window");
                } else {
                    window.app_handle().exit(0)
                }
            }
            tauri::WindowEvent::Moved(_) | tauri::WindowEvent::Resized(_) => {
                // Save window state on move or resize
                if let Ok(position) = window.outer_position() {
                    if let Ok(size) = window.outer_size() {
                        if let Ok(is_maximized) = window.is_maximized() {
                            // Find which monitor the window is currently on
                            let mut monitor_index = 0u32;
                            if let Ok(monitors) = window.available_monitors() {
                                for (idx, monitor) in monitors.iter().enumerate() {
                                    let monitor_pos = monitor.position();
                                    let monitor_size = monitor.size();
                                    
                                    // Check if window center is on this monitor
                                    let window_center_x = position.x + (size.width as i32) / 2;
                                    let window_center_y = position.y + (size.height as i32) / 2;
                                    
                                    if window_center_x >= monitor_pos.x 
                                        && window_center_x < (monitor_pos.x + monitor_size.width as i32)
                                        && window_center_y >= monitor_pos.y
                                        && window_center_y < (monitor_pos.y + monitor_size.height as i32) {
                                        monitor_index = idx as u32;
                                        break;
                                    }
                                }
                            }
                            
                            // Store relative coordinates within the monitor
                            let monitor_offset_x = if monitor_index > 0 {
                                if let Ok(monitors) = window.available_monitors() {
                                    if let Some(monitor) = monitors.get(monitor_index as usize) {
                                        monitor.position().x
                                    } else {
                                        0
                                    }
                                } else {
                                    0
                                }
                            } else {
                                0
                            };
                            
                            let monitor_offset_y = if monitor_index > 0 {
                                if let Ok(monitors) = window.available_monitors() {
                                    if let Some(monitor) = monitors.get(monitor_index as usize) {
                                        monitor.position().y
                                    } else {
                                        0
                                    }
                                } else {
                                    0
                                }
                            } else {
                                0
                            };
                            
                            let window_state = WindowState {
                                x: position.x - monitor_offset_x,
                                y: position.y - monitor_offset_y,
                                width: size.width,
                                height: size.height,
                                maximized: is_maximized,
                                monitor_index,
                            };
                            settings::save_window_state(window_state);
                        }
                    }
                }
            }
            _ => {}
        })
        .build(tauri::generate_context!())
        .expect("error while running tauri application")
        .run(|app, event| match event {
            tauri::RunEvent::Exit { .. } => {
                let window = app
                    .get_webview_window("main")
                    .expect("Failed to get webview window");

                sentry::end_session_with_status(sentry::protocol::SessionStatus::Exited);

                let state = window.app_handle().state::<Mutex<GlobalState>>();
                let child = state.lock().expect("Failed to lock state").child.take();

                if let Some(child) = child {
                    let res = child.kill();
                    println!("Sent kill to child process: {:?}", res);
                }
            }
            _ => {}
        });
}
