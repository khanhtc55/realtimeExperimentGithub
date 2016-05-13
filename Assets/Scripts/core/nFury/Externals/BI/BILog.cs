using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
namespace nFury.Externals.BI
{
	public class BILog
	{
		public const string APP_CLICK_TRACK = "click_track";
		public const string TAG_AUTHORIZATION = "authorization";
		public const string TAG_DEVICE_INFO = "device_info";
		public const string TAG_ERROR = "error";
		public const string TAG_GAME_ACTION = "game_action";
		public const string TAG_GEO = "geo";
		public const string TAG_LOG_IN = "clicked_link";
		public const string TAG_MONEY = "money";
		public const string TAG_NETWORK_MAPPING_INFO = "network_mapping_info";
		public const string TAG_PERFORMANCE = "performance";
		public const string TAG_PLAYER_INFO = "player_info";
		public const string TAG_SEND_MESSAGE = "send_message";
		public const string TAG_STEP_TIMING = "step_timing";
		public const string TAG_USER_INFO = "user_info";
		public const string PARAM_ACTION = "action";
		public const string PARAM_ALLOY_BALANCE = "alloy_balance";
		public const string PARAM_APP = "app";
		public const string PARAM_APP_LOCALE = "app_locale";
		public const string PARAM_BIRTHDAY_DATE = "birthday_date";
		public const string PARAM_CLEARABLE_UNIT = "clearable_units";
		public const string PARAM_CONTEXT = "context";
		public const string PARAM_CREDIT_BALANCE = "credit_balance";
		public const string PARAM_CRYSTAL_BALANCE = "crystal_balance";
		public const string PARAM_DEVICE_ID = "device_id";
		public const string PARAM_DEVICE_TYPE = "device_type";
		public const string PARAM_DISPLAY_STATE = "display_state";
		public const string PARAM_DROIDS_AVAILABLE = "droids_available";
		public const string PARAM_ELAPSED_TIME_MS = "elapsed_time_ms";
		public const string PARAM_ENGAGED = "engaged";
		public const string PARAM_FACTION = "faction";
		public const string PARAM_FPS = "fps";
		public const string PARAM_FURTHEST_MISSION_COMPLETE = "furthest_mission_complete";
		public const string PARAM_GENDER = "gender";
		public const string PARAM_GOOGLE_ADVERTISING_ID = "google_advertising_id";
		public const string PARAM_IMEI = "imei";
		public const string PARAM_IOS_ADVERTISING_ID = "ios_advertising_id";
		public const string PARAM_IOS_VENDOR_ID = "ios_vendor_id";
		public const string PARAM_IS_NEW_USER = "is_new_user";
		public const string PARAM_LANG = "lang";
		public const string PARAM_LEVEL = "level";
		public const string PARAM_LIFETIME_SPEND = "lifetime_spend";
		public const string PARAM_LOCALE = "locale";
		public const string PARAM_LOCATION = "location";
		public const string PARAM_LOG_APP = "log_app";
		public const string PARAM_MACHINE = "machine";
		public const string PARAM_MEMORY_USED = "memory_used";
		public const string PARAM_MESSAGE = "message";
		public const string PARAM_MODEL = "model";
		public const string PARAM_NETWORK = "network";
		public const string PARAM_NUM_SENT = "num_sent";
		public const string PARAM_OS_VERSION = "os_version";
		public const string PARAM_OTHER_KEY = "other_key";
		public const string PARAM_PATH_NAME = "path_name";
		public const string PARAM_REASON = "reason";
		public const string PARAM_SECONDARY_NETWORK = "secondary_network";
		public const string PARAM_SECONDARY_USER_ID = "secondary_user_id";
		public const string PARAM_SEND_TIMESTAMP = "send_timestamp";
		public const string PARAM_SHIELD_TIMER = "shield_timer";
		public const string PARAM_STARS_EARNED = "stars_earned";
		public const string PARAM_STEP = "step";
		public const string PARAM_SQUAD_ID = "squad_id";
		public const string PARAM_SQUAD_NAME = "squad_name";
		public const string PARAM_TAG = "tag";
		public const string PARAM_TARGET_USER_ID = "target_user_id";
		public const string PARAM_TIME_SINCE_START = "time_since_start";
		public const string PARAM_TIMESTAMP = "timestamp";
		public const string PARAM_TIMESTAMP_MS = "timestamp_ms";
		public const string PARAM_TRACKING_CODE = "tracking_code";
		public const string PARAM_TROPHY_BALANCE = "trophy_balance";
		public const string PARAM_TYPE = "type";
		public const string PARAM_USER_ID = "user_id";
		public const string PARAM_VIEW_NETWORK = "view_network";
		public const string VALUE_ALL_DROIDS_BUSY = "all_droids_busy";
		public const string VALUE_ACTION_ABOUT = "about";
		public const string VALUE_ACTION_ADD_CRYSTALS = "add_crystals";
		public const string VALUE_ACTION_ADD_DROID = "add_droid";
		public const string VALUE_ACTION_ARMY = "army";
		public const string VALUE_ACTION_ATTACK = "attack";
		public const string VALUE_ACTION_BATTLE_LOG = "battle_log";
		public const string VALUE_ACTION_SPECOPS = "campaign";
		public const string VALUE_ACTION_CHAPTER = "chapter";
		public const string VALUE_ACTION_CHAT = "chat";
		public const string VALUE_ACTION_CLOSE = "close";
		public const string VALUE_ACTION_CREATE = "create";
		public const string VALUE_ACTION_DAMAGE_PROTECTION = "damage_protection";
		public const string VALUE_ACTION_DEFENSES = "defenses";
		public const string VALUE_ACTION_DEMOTE = "demote";
		public const string VALUE_ACTION_EDIT = "edit";
		public const string VALUE_ACTION_FAN_FORUMS = "fan_forums";
		public const string VALUE_ACTION_FB_CONNECT = "fb_connect";
		public const string VALUE_ACTION_FB_DISCONNECT = "fb_disconnect";
		public const string VALUE_ACTION_HELP = "help";
		public const string VALUE_ACTION_JOIN_ACCEPT = "join_accept";
		public const string VALUE_ACTION_JOIN_REJECT = "join_reject";
		public const string VALUE_ACTION_JOIN_REQUEST = "join_request";
		public const string VALUE_ACTION_LEADERBOARD = "leaderboard";
		public const string VALUE_ACTION_LEAVE = "leave";
		public const string VALUE_ACTION_MEMBER = "member";
		public const string VALUE_ACTION_MUSIC_OFF = "music_off";
		public const string VALUE_ACTION_MUSIC_ON = "music_on";
		public const string VALUE_ACTION_NEWSPAPER = "newspaper";
		public const string VALUE_ACTION_OFFICER = "officer";
		public const string VALUE_ACTION_PLAYER_BANNED = "player_banned";
		public const string VALUE_ACTION_POINTS_SHOP = "points_shop";
		public const string VALUE_ACTION_PRIVATE = "private";
		public const string VALUE_ACTION_PRIZES = "prizes";
		public const string VALUE_ACTION_PROMOTE = "promote";
		public const string VALUE_ACTION_PROTECTION = "protection";
		public const string VALUE_ACTION_PUBLIC = "public";
		public const string VALUE_ACTION_PVE = "PvE";
		public const string VALUE_ACTION_PVP = "PvP";
		public const string VALUE_ACTION_REFERRAL_STORE = "referral_store";
		public const string VALUE_ACTION_RESOURCES = "resources";
		public const string VALUE_ACTION_REVENGE = "revenge";
		public const string VALUE_ACTION_REWARDS = "rewards";
		public const string VALUE_ACTION_TROOP_REQUEST = "requestTroops";
		public const string VALUE_ACTION_SETTINGS = "settings";
		public const string VALUE_ACTION_SEND_TROOPS = "sendTroops";
		public const string VALUE_ACTION_START = "start";
		public const string VALUE_ACTION_SFX_OFF = "sfx_off";
		public const string VALUE_ACTION_SFX_ON = "sfx_on";
		public const string VALUE_ACTION_SHOP = "shop";
		public const string VALUE_ACTION_SQUAD = "squad";
		public const string VALUE_ACTION_SQUAD_ACTION = "squad_action";
		public const string VALUE_ACTION_SQUAD_INFO = "squad_info";
		public const string VALUE_ACTION_TOURNAMENT_LEADERBOARD = "tournament_leaderboard";
		public const string VALUE_ACTION_TREASURE = "treasure";
		public const string VALUE_ACTION_TURRETS = "turrets";
		public const string VALUE_ACTION_VIEW = "view";
		public const string VALUE_ACTION_YES = "yes";
		public const string VALUE_ACTION_NO = "no";
		public const string VALUE_AGE_GATE_END = "age_gate_end";
		public const string VALUE_AGE_GATE_START = "age_gate_start";
		public const string VALUE_MESSAGE_NO_TOURNAMENT = "no_tournament";
		public const string VALUE_MESSAGE_NO_TROOPS = "no_troops";
		public const string VALUE_MESSAGE_TIME_OUT = "time_out";
		public const string VALUE_MESSAGE_NONE_FOUND = "none_found";
		public const string VALUE_ASSET_LOAD_END = "assetload_end";
		public const string VALUE_ASSET_LOAD_START = "assetload_start";
		public const string VALUE_AUTH_STEP_ALLOW = "allow";
		public const string VALUE_AUTH_STEP_DISALLOW = "disallow";
		public const string VALUE_ACTION_CUSTOM_ASK = "01_custom_ask";
		public const string VALUE_ACTION_CUSTOM_ALLOW = "02_custom_allow";
		public const string VALUE_ACTION_CUSTOM_DENY = "02_custom_deny";
		public const string VALUE_ACTION_STANDARD_ASK = "03_standard_ask";
		public const string VALUE_ACTION_STANDARD_ALLOW = "04_standard_allow";
		public const string VALUE_ACTION_STANDARD_DENY = "04_standard_deny";
		public const string VALUE_AUTH_TYPE_FACEBOOK_CONNECT = "facebook_connect";
		public const string VALUE_CONTEXT_PUSH_NOTIFICATION = "push_notification";
		public const string VALUE_BUY = "buy";
		public const string VALUE_CLOSE = "close";
		public const string VALUE_CONTEXT_DROID = "droid";
		public const string VALUE_CONTEXT_FUE = "FUE";
		public const string VALUE_CONTEXT_FACTION_CHOICE = "faction_choice";
		public const string VALUE_CONTEXT_LOAD_PVE = "load_PvE";
		public const string VALUE_CONTEXT_LOAD_PVP = "load_PvP";
		public const string VALUE_CONTEXT_LOAD_SPECOPS = "load_campaign";
		public const string VALUE_CONTEXT_PAGE_LOAD = "page_load";
		public const string VALUE_CONTEXT_IAP = "iap";
		public const string VALUE_CONTEXT_PVE = "PvE";
		public const string VALUE_CONTEXT_PVP = "PvP";
		public const string VALUE_CONTEXT_SPECOPS = "campaign";
		public const string VALUE_CONTEXT_RATEAPP = "rateapp";
		public const string VALUE_CONTEXT_UI_HUD = "UI_HUD";
		public const string VALUE_CONTEXT_UI_SETTINGS = "UI_settings";
		public const string VALUE_CONTEXT_UI_SHOP = "UI_shop";
		public const string VALUE_CONTEXT_UI_SHOP_TREASURE = "UI_shop_treasure";
		public const string VALUE_CONTEXT_UI_MONEY_FLOW = "UI_money_flow";
		public const string VALUE_CONTEXT_UI_LEADERBOARD = "UI_leaderboard";
		public const string VALUE_CONTEXT_UI_LEADERBOARD_EXPAND = "UI_leaderboard_expand";
		public const string VALUE_CONTEXT_UI_SQUAD_MEMBER = "UI_squad_member";
		public const string VALUE_CONTEXT_UI_SQUAD_NONMEMBER = "UI_squad_nonmember";
		public const string VALUE_CONTEXT_UI_ATTACK = "UI_attack";
		public const string VALUE_CONTEXT_UI_PVE_MISSION = "UI_PvE_mission";
		public const string VALUE_CONTEXT_UI_SPECOPS_MISSION = "UI_campaign_mission";
		public const string VALUE_CONTEXT_UI_TOURNAMENT_TIERS = "UI_tournament_tiers";
		public const string VALUE_CONTEXT_UI_TOURNAMENT_END = "UI_tournament_end";
		public const string VALUE_CONTEXT_UI_IAP_DISCLAIMER = "UI_IAP_disclaimer";
		public const string VALUE_CONTEXT_UI_IDLEPOP = "UI_idlepop";
		public const string VALUE_CONTEXT_UI_NEWSPAPER = "UI_newspaper";
		public const string VALUE_CONTEXT_INTRO_ANIM = "text_crawl";
		public const string VALUE_CONTEXT_VISIT_PLAYER = "visit_player";
		public const string VALUE_END = "end";
		public const string VALUE_EXPAND = "expand";
		public const string VALUE_FEATURED = "featured";
		public const string VALUE_FRIENDS = "friends";
		public const string VALUE_GENDER_MALE = "male";
		public const string VALUE_GENDER_FEMALE = "female";
		public const string VALUE_INFO = "info";
		public const string VALUE_JOIN = "join";
		public const string VALUE_INITIALIZE_AUDIO_END = "init_audio_end";
		public const string VALUE_INITIALIZE_AUDIO_START = "init_audio_start";
		public const string VALUE_INITIALIZE_BOARD_END = "init_board_end";
		public const string VALUE_INITIALIZE_BOARD_START = "init_board_start";
		public const string VALUE_INITIALIZE_GAMEDATA_END = "init_gamedata_end";
		public const string VALUE_INITIALIZE_GAMEDATA_START = "init_gamedata_start";
		public const string VALUE_INITIALIZE_GENERAL_SYSTEMS_END = "init_general_sys_end";
		public const string VALUE_INITIALIZE_GENERAL_SYSTEMS_START = "init_general_sys_start";
		public const string VALUE_INITIALIZE_WORLD_END = "init_world_end";
		public const string VALUE_INITIALIZE_WORLD_START = "init_world_start";
		public const string VALUE_IS_NEW_USER_TRUE = "1";
		public const string VALUE_IS_NEW_USER_FALSE = "0";
		public const string VALUE_METADATA_LOAD_END = "metadata_end";
		public const string VALUE_METADATA_LOAD_START = "metadata_start";
		public const string VALUE_MOBILE = "mobile";
		public const string VALUE_NETWORK_FACEBOOK = "f";
		public const string VALUE_NETWORK_GPGS = "gp";
		public const string VALUE_NETWORK_GAME_CENTER = "gc";
		public const string VALUE_NETWORK_URBAN_AIRSHIP = "ur";
		public const string VALUE_NO_OPPONENT_FOUND = "no_opponent_found";
		public const string VALUE_NOT_ENOUGH_HARD_CURRENCY = "not_enough_hard_currency";
		public const string VALUE_NOT_ENOUGH_SOFT_CURRENCY = "not_enough_soft_currency";
		public const string VALUE_PATH_DEFAULT = "default";
		public const string VALUE_PRELOAD_ASSETS_END = "preload_assets_end";
		public const string VALUE_PRELOAD_ASSETS_START = "preload_assets_start";
		public const string VALUE_SPEED_UP = "speed_up";
		public const string VALUE_PLAYERS = "players";
		public const string VALUE_TOURNAMENT = "tournament";
		public const string VALUE_SEARCH = "search";
		public const string VALUE_STRING_DATA_START = "string_data_start";
		public const string VALUE_STRING_DATA_END = "string_data_end";
		public const string VALUE_SQUADS = "squads";
		public const string VALUE_START = "start";
		public const string VALUE_UI_PVP = "UI_PvP";
		public const string VALUE_VISIT = "visit";
		public const string VALUE_ATTACK = "attack";
		public const string VALUE_DEFENSE = "defense";
		public const string VALUE_SKIP = "skip";
		public const string VALUE_FINISH = "finish";
		public const string VALUE_UPGRADE_ALL_WALLS = "upgrade_all_walls";
		private Dictionary<string, string> paramDict;
		public string Url
		{
			get;
			set;
		}
		public BILog(string url)
		{
			this.paramDict = new Dictionary<string, string>();
			this.Url = url;
		}
		public void AddParam(string key, string value)
		{
			if (this.paramDict.ContainsKey(key))
			{
				this.paramDict[key] = WWW.EscapeURL(value);
			}
			else
			{
				this.paramDict.Add(key, WWW.EscapeURL(value));
			}
		}
		public void Reset()
		{
			this.paramDict.Clear();
		}
		public string ToURL()
		{
			StringBuilder stringBuilder = new StringBuilder(this.Url);
			foreach (string current in this.paramDict.Keys)
			{
				stringBuilder.Append("&");
				stringBuilder.Append(current);
				stringBuilder.Append("=");
				stringBuilder.Append(this.paramDict[current]);
			}
			return stringBuilder.ToString();
		}
	}
}
