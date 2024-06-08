import { copyFileSync } from "fs"

copyFileSync("target/release/rtc.dll", "platforms/windows/service/lib.dll")
