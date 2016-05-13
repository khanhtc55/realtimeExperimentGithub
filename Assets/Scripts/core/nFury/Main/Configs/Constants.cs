using nFury.Externals.FileManagement;
using System;
namespace nFury.Main.Configs
{
	public static class Constants
	{
		public const float WORLD_ROTATION = 90f;
		public const string CDN_ROOT = "http://localhost/";
		public const string UTIL_SERVER = "http://10.217.129.48/";
		public const string REPLAY_SERVER = "http://10.217.129.48:4444/";
		public const string REMOTE_PRIMARY = "https://n7-starts-web-active.playdom.com/j";
		public const string REMOTE_SECONDARY = "https://n7-starts-integration-app-active.playdom.com/j";
		public const string LOCAL = "http://localhost:8080/starts";
		public const string SERVER = "https://n7-starts-web-active.playdom.com/j";

		public const string BI_URL = "https://weblogger.data.disney.com/cp?";
		public const string APP_STORE_URL = "http://itunes.apple.com/app/id847985808?mt=8";
		public const string ITUNES_APP_ID = "847985808";

		public const string GOOGLE_PLAY_URL = "market://details?id=com.lucasarts.starts_goo";
		public const string STEP_TIMING_START = "start";
		public const string STEP_TIMING_INTERMEDIATE = "inter";
		public const string STEP_TIMING_END = "end";

		public const string LOCAL_DIR = "/src/maps";

		public const string DEFAULT_CEE_FILE = "cee.json";
		public const string DEFAULT_REPLAY_FILE = "replay.json";
		public const string DEFAULT_BATTLE_RECORD_FILE = "battleRecord.json";
		public const string CMS_BASE_PATCH_FILE = "patches/base.json.zip";

		public const int BUILDING_SMALL = 1;
		public const int BUILDING_MEDIUM = 2;
		public const int BUILDING_LARGE = 4;
		public const int MINIMUM_BUILDING_GAP = 1;
		public const int ARROW_PADDING = 10;
		public const int ARROW_BUILDING_OFFSET = 75;

		public const FmsMode FMS_MODE = FmsMode.Passthrough;

		public const string BATTLE_GAMEPLAY_VERSION = "6.0";
		public const string PREF_PLAYER_ID = "prefPlayerId";
		public const string PREF_PLAYER_SECRET = "prefPlayerSecret";
		public const string UNLOCKABLE_BY_REWARD = "reward";
		public const string TIMED_EVENT_DATE_FORMAT = "HH:mm,dd-MM-yyyy";
		public const string CRYPTO_ALGORITHM_SHA256 = "HmacSHA256";
	}
}
