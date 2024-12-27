import { mkdirSync, existsSync, copyFileSync } from "fs"
import { platform } from "os"
import json from "../package.json" assert { type: "json" }

const os = platform()
const version = json.version

if (!existsSync("./target/release/upload")) {
	mkdirSync("./target/release/upload")
}

if (os === "win32") {
	try {
		copyFileSync(`./target/release/bundle/msi/Cores_${version}_x64_en-US.msi`, `./target/release/upload/cores-${version}-windows-x64.msi`)
		copyFileSync(`./target/release/bundle/msi/Cores_${version}_x64_en-US.msi.zip`, `./target/release/upload/cores-${version}-windows-x64.zip`)
		copyFileSync(`./target/release/bundle/msi/Cores_${version}_x64_en-US.msi.zip.sig`, `./target/release/upload/cores-${version}-windows-x64.sig`)
	} catch (err) {
		console.log("File not found")
	}
} else if (os === "darwin") {
	try {
		copyFileSync(`./target/release/bundle/dmg/Cores_${version}_aarch64.dmg`, `./target/release/upload/cores-${version}-macos-arm64.dmg`)
		copyFileSync("./target/release/bundle/macos/Cores.app.tar.gz", `./target/release/upload/cores-${version}-macos-arm64.tar.gz`)
		copyFileSync("./target/release/bundle/macos/Cores.app.tar.gz.sig", `./target/release/upload/cores-${version}-macos-arm64.sig`)
	} catch (err) {
		console.log("File not found", err)
	}
} else {
	try {
		copyFileSync(`./target/release/bundle/appimage/Cores_${version}_amd64.AppImage`, `./target/release/upload/cores-${version}-linux-x64.appimage`)
		copyFileSync(`./target/release/bundle/deb/Cores_${version}_amd64.deb`, `./target/release/upload/cores-${version}-linux-x64.deb`)
        copyFileSync(`./target/release/bundle/rpm/Cores-${version}-1.x86_64.rpm`, `./target/release/upload/cores-${version}-linux-x64.rpm`)
	} catch (err) {
		console.log("File not found")
	}
}