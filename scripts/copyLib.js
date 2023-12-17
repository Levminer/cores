import { copyFileSync } from "fs"

copyFileSync("crates/library/target/release/lib.dll", "core/lib.dll")
