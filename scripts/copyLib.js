import { copyFileSync } from "fs"

copyFileSync("library/target/release/lib.dll", "core/lib.dll")
