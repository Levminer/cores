export type Json = string | number | boolean | null | { [key: string]: Json | undefined } | Json[]

export type Database = {
	// Allows to automatically instantiate createClient with right options
	// instead of createClient<Database, { PostgrestVersion: 'XX' }>(URL, KEY)
	__InternalSupabase: {
		PostgrestVersion: "14.1"
	}
	graphql_public: {
		Tables: {
			[_ in never]: never
		}
		Views: {
			[_ in never]: never
		}
		Functions: {
			graphql: {
				Args: {
					extensions?: Json
					operationName?: string
					query?: string
					variables?: Json
				}
				Returns: Json
			}
		}
		Enums: {
			[_ in never]: never
		}
		CompositeTypes: {
			[_ in never]: never
		}
	}
	public: {
		Tables: {
			network_device: {
				Row: {
					created_at: string
					mac: string | null
					name: string | null
					network_device_id: string
					user_id: string | null
				}
				Insert: {
					created_at?: string
					mac?: string | null
					name?: string | null
					network_device_id?: string
					user_id?: string | null
				}
				Update: {
					created_at?: string
					mac?: string | null
					name?: string | null
					network_device_id?: string
					user_id?: string | null
				}
				Relationships: [
					{
						foreignKeyName: "network_device_user_id_fkey"
						columns: ["user_id"]
						isOneToOne: false
						referencedRelation: "user"
						referencedColumns: ["user_id"]
					},
				]
			}
			notification: {
				Row: {
					body: string | null
					created_at: string
					data: Json | null
					notification_id: string
					read_at: string | null
					title: string | null
					user_id: string
				}
				Insert: {
					body?: string | null
					created_at?: string
					data?: Json | null
					notification_id?: string
					read_at?: string | null
					title?: string | null
					user_id: string
				}
				Update: {
					body?: string | null
					created_at?: string
					data?: Json | null
					notification_id?: string
					read_at?: string | null
					title?: string | null
					user_id?: string
				}
				Relationships: [
					{
						foreignKeyName: "notification_user_id_fkey"
						columns: ["user_id"]
						isOneToOne: false
						referencedRelation: "user"
						referencedColumns: ["user_id"]
					},
				]
			}
			push_token: {
				Row: {
					created_at: string
					push_token_id: string
					token: string | null
					user_id: string | null
				}
				Insert: {
					created_at?: string
					push_token_id?: string
					token?: string | null
					user_id?: string | null
				}
				Update: {
					created_at?: string
					push_token_id?: string
					token?: string | null
					user_id?: string | null
				}
				Relationships: [
					{
						foreignKeyName: "push_token_user_id_fkey"
						columns: ["user_id"]
						isOneToOne: false
						referencedRelation: "user"
						referencedColumns: ["user_id"]
					},
				]
			}
			remote_connection: {
				Row: {
					code: string | null
					created_at: string
					name: string | null
					remote_connection_id: string
					user_id: string | null
				}
				Insert: {
					code?: string | null
					created_at?: string
					name?: string | null
					remote_connection_id?: string
					user_id?: string | null
				}
				Update: {
					code?: string | null
					created_at?: string
					name?: string | null
					remote_connection_id?: string
					user_id?: string | null
				}
				Relationships: [
					{
						foreignKeyName: "remote_connection_user_id_fkey"
						columns: ["user_id"]
						isOneToOne: false
						referencedRelation: "user"
						referencedColumns: ["user_id"]
					},
				]
			}
			temperature: {
				Row: {
					created_at: string
					external_temperature: number | null
					humidity: number | null
					temperature: number | null
					temperature_id: string
				}
				Insert: {
					created_at?: string
					external_temperature?: number | null
					humidity?: number | null
					temperature?: number | null
					temperature_id?: string
				}
				Update: {
					created_at?: string
					external_temperature?: number | null
					humidity?: number | null
					temperature?: number | null
					temperature_id?: string
				}
				Relationships: []
			}
			user: {
				Row: {
					avatar_url: string | null
					created_at: string
					email: string | null
					name: string | null
					plan: string | null
					user_id: string
				}
				Insert: {
					avatar_url?: string | null
					created_at?: string
					email?: string | null
					name?: string | null
					plan?: string | null
					user_id?: string
				}
				Update: {
					avatar_url?: string | null
					created_at?: string
					email?: string | null
					name?: string | null
					plan?: string | null
					user_id?: string
				}
				Relationships: []
			}
		}
		Views: {
			[_ in never]: never
		}
		Functions: {
			[_ in never]: never
		}
		Enums: {
			[_ in never]: never
		}
		CompositeTypes: {
			[_ in never]: never
		}
	}
}

type DatabaseWithoutInternals = Omit<Database, "__InternalSupabase">

type DefaultSchema = DatabaseWithoutInternals[Extract<keyof Database, "public">]

export type Tables<
	DefaultSchemaTableNameOrOptions extends keyof (DefaultSchema["Tables"] & DefaultSchema["Views"]) | { schema: keyof DatabaseWithoutInternals },
	TableName extends DefaultSchemaTableNameOrOptions extends {
		schema: keyof DatabaseWithoutInternals
	}
		? keyof (DatabaseWithoutInternals[DefaultSchemaTableNameOrOptions["schema"]]["Tables"] &
				DatabaseWithoutInternals[DefaultSchemaTableNameOrOptions["schema"]]["Views"])
		: never = never,
> = DefaultSchemaTableNameOrOptions extends {
	schema: keyof DatabaseWithoutInternals
}
	? (DatabaseWithoutInternals[DefaultSchemaTableNameOrOptions["schema"]]["Tables"] &
			DatabaseWithoutInternals[DefaultSchemaTableNameOrOptions["schema"]]["Views"])[TableName] extends {
			Row: infer R
		}
		? R
		: never
	: DefaultSchemaTableNameOrOptions extends keyof (DefaultSchema["Tables"] & DefaultSchema["Views"])
		? (DefaultSchema["Tables"] & DefaultSchema["Views"])[DefaultSchemaTableNameOrOptions] extends {
				Row: infer R
			}
			? R
			: never
		: never

export type TablesInsert<
	DefaultSchemaTableNameOrOptions extends keyof DefaultSchema["Tables"] | { schema: keyof DatabaseWithoutInternals },
	TableName extends DefaultSchemaTableNameOrOptions extends {
		schema: keyof DatabaseWithoutInternals
	}
		? keyof DatabaseWithoutInternals[DefaultSchemaTableNameOrOptions["schema"]]["Tables"]
		: never = never,
> = DefaultSchemaTableNameOrOptions extends {
	schema: keyof DatabaseWithoutInternals
}
	? DatabaseWithoutInternals[DefaultSchemaTableNameOrOptions["schema"]]["Tables"][TableName] extends {
			Insert: infer I
		}
		? I
		: never
	: DefaultSchemaTableNameOrOptions extends keyof DefaultSchema["Tables"]
		? DefaultSchema["Tables"][DefaultSchemaTableNameOrOptions] extends {
				Insert: infer I
			}
			? I
			: never
		: never

export type TablesUpdate<
	DefaultSchemaTableNameOrOptions extends keyof DefaultSchema["Tables"] | { schema: keyof DatabaseWithoutInternals },
	TableName extends DefaultSchemaTableNameOrOptions extends {
		schema: keyof DatabaseWithoutInternals
	}
		? keyof DatabaseWithoutInternals[DefaultSchemaTableNameOrOptions["schema"]]["Tables"]
		: never = never,
> = DefaultSchemaTableNameOrOptions extends {
	schema: keyof DatabaseWithoutInternals
}
	? DatabaseWithoutInternals[DefaultSchemaTableNameOrOptions["schema"]]["Tables"][TableName] extends {
			Update: infer U
		}
		? U
		: never
	: DefaultSchemaTableNameOrOptions extends keyof DefaultSchema["Tables"]
		? DefaultSchema["Tables"][DefaultSchemaTableNameOrOptions] extends {
				Update: infer U
			}
			? U
			: never
		: never

export type Enums<
	DefaultSchemaEnumNameOrOptions extends keyof DefaultSchema["Enums"] | { schema: keyof DatabaseWithoutInternals },
	EnumName extends DefaultSchemaEnumNameOrOptions extends {
		schema: keyof DatabaseWithoutInternals
	}
		? keyof DatabaseWithoutInternals[DefaultSchemaEnumNameOrOptions["schema"]]["Enums"]
		: never = never,
> = DefaultSchemaEnumNameOrOptions extends {
	schema: keyof DatabaseWithoutInternals
}
	? DatabaseWithoutInternals[DefaultSchemaEnumNameOrOptions["schema"]]["Enums"][EnumName]
	: DefaultSchemaEnumNameOrOptions extends keyof DefaultSchema["Enums"]
		? DefaultSchema["Enums"][DefaultSchemaEnumNameOrOptions]
		: never

export type CompositeTypes<
	PublicCompositeTypeNameOrOptions extends keyof DefaultSchema["CompositeTypes"] | { schema: keyof DatabaseWithoutInternals },
	CompositeTypeName extends PublicCompositeTypeNameOrOptions extends {
		schema: keyof DatabaseWithoutInternals
	}
		? keyof DatabaseWithoutInternals[PublicCompositeTypeNameOrOptions["schema"]]["CompositeTypes"]
		: never = never,
> = PublicCompositeTypeNameOrOptions extends {
	schema: keyof DatabaseWithoutInternals
}
	? DatabaseWithoutInternals[PublicCompositeTypeNameOrOptions["schema"]]["CompositeTypes"][CompositeTypeName]
	: PublicCompositeTypeNameOrOptions extends keyof DefaultSchema["CompositeTypes"]
		? DefaultSchema["CompositeTypes"][PublicCompositeTypeNameOrOptions]
		: never

export const Constants = {
	graphql_public: {
		Enums: {},
	},
	public: {
		Enums: {},
	},
} as const
