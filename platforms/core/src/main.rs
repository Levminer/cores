// Prevents additional console window on Windows in release, DO NOT REMOVE!!
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

struct GlobalState {
    child: Option<CommandChild>,
}

fn main() {
    let _sentry = sentry::init((
        "https://da874903dead91a5de908b045a106aca@o4506670275428352.ingest.us.sentry.io/4507476699578368",
        sentry::ClientOptions {
            release: sentry::release_name!(),
            auto_session_tracking: true,
            ..Default::default()
        },
    ));

    tauri::Builder::default()
        .plugin(tauri_plugin_shell::init())
        .plugin(tauri_plugin_dialog::init())
        .plugin(tauri_plugin_process::init())
        .plugin(tauri_plugin_updater::Builder::new().build())
        .plugin(tauri_plugin_single_instance::init(|app, _args, _cwd| {
            let window = app.get_webview_window("main").unwrap();

            window.show().unwrap();
            window.set_focus().unwrap();
        }))
        .invoke_handler(tauri::generate_handler![
            settings::get_settings,
            settings::set_settings,
            service::start_service,
            service::stop_service,
            service::restart_service,
            utils::system_info
        ])
        .setup(|app| {
            app.manage(Mutex::new(GlobalState { child: None }));

            let toggle_window_item =
                MenuItemBuilder::with_id("toggle_windows", "Show/Hide Cores").build(app)?;
            let exit_item = MenuItemBuilder::with_id("exit", "Exit").build(app)?;
            let menu = MenuBuilder::new(app)
                .items(&[&toggle_window_item, &exit_item])
                .build()?;

            let tray = app.tray_by_id("main").unwrap();
            tray.set_menu(Some(menu)).unwrap();
            tray.on_menu_event(move |app, event| match event.id().as_ref() {
                "toggle_windows" => {
                    let window = app.get_webview_window("main").unwrap();

                    if window.is_visible().unwrap() {
                        window.hide().unwrap();
                    } else {
                        window.show().unwrap();
                        window.set_focus().unwrap();
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
                        let window = app.get_webview_window("main").unwrap();

                        if window.is_visible().unwrap() {
                            window.hide().unwrap();
                        } else {
                            window.show().unwrap();
                            window.set_focus().unwrap();
                        }
                    }
                });
            }

            if cfg!(target_os = "linux") || cfg!(target_os = "macos") {
                let sidecar_command = app.shell().sidecar("coresd").unwrap();
                let (mut rx, child) = sidecar_command.spawn().expect("Failed to spawn sidecar");

                let state = app.state::<Mutex<GlobalState>>();
                state.lock().unwrap().child = Some(child);

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
                    window.hide().unwrap();
                } else {
                    window.app_handle().exit(0)
                }
            }
            _ => {}
        })
        .build(tauri::generate_context!())
        .expect("error while running tauri application")
        .run(|app, event| match event {
            tauri::RunEvent::Exit { .. } => {
                let window = app.get_webview_window("main").unwrap();

                let state = window.app_handle().state::<Mutex<GlobalState>>();
                let child = state.lock().unwrap().child.take();

                if let Some(child) = child {
                    let res = child.kill();
                    println!("Sent kill to child process: {:?}", res);
                }
            }
            _ => {}
        });
}
