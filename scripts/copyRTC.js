import { copyFileSync } from "fs"

copyFileSync("target/release/rtc.dll", "platforms/windows/service/rtc.dll")
copyFileSync("target/release/rtc.dll", "platforms/core/rtc.dll")