import { createClient } from "@supabase/supabase-js"
import type { Database } from "./database.d.ts"

const PUBLIC_SUPABASE_ANON_KEY =
	"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImFpbG5sc2xocGdlZHRsYnhma2N6Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3MzUzMzQ2NjMsImV4cCI6MjA1MDkxMDY2M30.5P1xGPLsk-60AA-YG1aGHA53PQ_Lo8x_Gr_MzO38liY"
const PUBLIC_SUPABASE_URL = "https://ailnlslhpgedtlbxfkcz.supabase.co"

export const supabaseClient = createClient<Database>(PUBLIC_SUPABASE_URL, PUBLIC_SUPABASE_ANON_KEY)
