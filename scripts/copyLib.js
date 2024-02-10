import { copyFileSync } from "fs"

copyFileSync("target/release/lib.dll", "platforms/windows/desktop/lib.dll")
