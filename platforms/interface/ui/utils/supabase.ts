import { createClient } from "@supabase/supabase-js"
import type { Database } from "./database.d.ts"

const PUBLIC_SUPABASE_ANON_KEY =
	"sb_publishable_Zi57ivjBxc5SSqaEESn0Gg_qfhNTYIT"
const PUBLIC_SUPABASE_URL = "https://ailnlslhpgedtlbxfkcz.supabase.co"

export const supabaseClient = createClient<Database>(PUBLIC_SUPABASE_URL, PUBLIC_SUPABASE_ANON_KEY)
