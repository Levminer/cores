import { renameSync } from "fs"
import { platform } from "os"
import json from "../package.json" with { type: "json" }

const os = platform()
const version = json.version

if (!existsSync("./target/release/upload")) {
	mkdirSync("./target/release/upload")
}

if (os === "win32") {
	try {
		renameSync(`./target/release/bundle/msi/Cores_${version}_x64_en-US.msi`, `./target/release/upload/cores-${version}-windows-x64.msi`)
		renameSync(`./target/release/bundle/msi/Cores_${version}_x64_en-US.msi.zip`, `./target/release/upload/cores-${version}-windows-x64.zip`)
		renameSync(`./target/release/bundle/msi/Cores_${version}_x64_en-US.msi.zip.sig`, `./target/release/upload/cores-${version}-windows-x64.sig`)
	} catch (err) {
		console.log("File not found")
	}
} else if (os === "darwin") {
	try {
		renameSync(`./target/release/bundle/dmg/Cores_${version}_aarch64.dmg`, `./target/release/upload/cores-${version}-macos-arm64.dmg`)
		renameSync("./target/release/bundle/macos/Cores.app.tar.gz", `./target/release/upload/cores-${version}-macos-arm64.tar.gz`)
		renameSync("./target/release/bundle/macos/Cores.app.tar.gz.sig", `./target/release/upload/cores-${version}-macos-arm64.sig`)
	} catch (err) {
		console.log("File not found", err)
	}
} else {
	try {
		renameSync(`./target/release/bundle/appimage/Cores_${version}_amd64.AppImage`, `./target/release/upload/cores-${version}-linux-x64.appimage`)
		renameSync(`./target/release/bundle/deb/Cores_${version}_amd64.deb`, `./target/release/upload/cores-${version}-linux-x64.deb`)
		renameSync(`./target/release/bundle/rpm/Cores-${version}-1.x86_64.rpm`, `./target/release/upload/cores-${version}-linux-x64.rpm`)
		renameSync(`./target/release/coresd`, `./target/release/upload/coresd-${version}-linux-x64`)
		renameSync(`./target/aarch64-unknown-linux-gnu/release/coresd`, `./target/release/upload/coresd-${version}-linux-arm64`)
	} catch (err) {
		console.log("File not found")
	}
}
