import Home from "./pages/Home.svelte"
import Cpu from "./pages/Cpu.svelte"
import Ram from "./pages/Ram.svelte"
import Gpu from "./pages/Gpu.svelte"
import Storage from "./pages/Storage.svelte"
import System from "./pages/System.svelte"
import Network from "./pages/Network.svelte"
import Connections from "./pages/Connections.svelte"
import Onboarding from "./pages/Onboarding.svelte"
import Settings from "./pages/Settings.svelte"

import BuildNumber from "./navigation/BuildNumber.svelte"
import DesktopNavigation from "./navigation/DesktopNavigation.svelte"
import Loading from "./navigation/Loading.svelte"
import RouteTransition from "./navigation/RouteTransition.svelte"

import GaugeChart from "./charts/GaugeChart.svelte"
import LineChart from "./charts/LineChart.svelte"
import MeterChart from "./charts/MeterChart.svelte"

import Login from "./components/Login.svelte"
import ModularDialog from "./components/ModularDialog.svelte"
import Progress from "./components/Progress.svelte"
import SaveDataButton from "./components/SaveDataButton.svelte"
import Select from "./components/Select.svelte"
import Toggle from "./components/Toggle.svelte"
import ToggleButton from "./components/ToggleButton.svelte"
import UpdateAlert from "./components/UpdateAlert.svelte"
import ConnectionServer from "./components/ConnectionServer.svelte"

import { hardwareInfo, getHardwareInfo, setHardwareInfo } from "./stores/hardwareInfo.ts"
import { hardwareStatistics, getHardwareStatistics, setHardwareStatistics } from "./stores/hardwareStatistics.ts"
import { settings, getSettings, setSettings, initializeSettings } from "./stores/settings.ts"
import { state, getState, setState } from "./stores/state.ts"

import { generateMinutesData, generateSecondsData } from "./utils/stats.ts"
import { supabaseClient } from "./utils/supabase.ts"
import { flyAndScale } from "./utils/transitions.ts"
import { addConnectionCode, deleteConnectionCode, editConnectionCode } from "./utils/connection.ts"

export {
	Home,
	Loading,
	Cpu,
	Ram,
	Gpu,
	Storage,
	System,
	Network,
	Connections,
	Onboarding,
	Settings,
	BuildNumber,
	DesktopNavigation,
	RouteTransition,
	GaugeChart,
	LineChart,
	MeterChart,
	Login,
	ModularDialog,
	Progress,
	SaveDataButton,
	Select,
	Toggle,
	ToggleButton,
	UpdateAlert,
	ConnectionServer,
	hardwareInfo,
	getHardwareInfo,
	setHardwareInfo,
	hardwareStatistics,
	getHardwareStatistics,
	setHardwareStatistics,
	settings,
	getSettings,
	setSettings,
	initializeSettings,
	state,
	getState,
	setState,
	generateMinutesData,
	generateSecondsData,
	supabaseClient,
	flyAndScale,
	addConnectionCode,
	deleteConnectionCode,
	editConnectionCode,
}
