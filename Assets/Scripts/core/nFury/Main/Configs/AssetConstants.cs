using System;
using System.Collections.Generic;
using nFury.Utils;

namespace nFury.Main.Configs
{
	public static class AssetConstants
	{
		public const string REBEL_ASSET_NAME = "rbl";
		public const string EMPIRE_ASSET_NAME = "emp";

		public const string LOD1_EXTENSION = "-lod1";

		public const string GUI_SHARED_ASSET = "gui_shared";
		public const string GUI_HUD_ASSET = "gui_hud";
		public const string GUI_MISC_ASSET = "gui_misc";
		

		public static readonly HashSet<string> LOCAL_BUNDLE_NAMES = new HashSet<string>
		{
			"gui_loading_screen",
			"gui_shared",
			"gui_hud",
		};
		public static readonly string[] GUI_ALWAYS_LOADED_SCREENS = new string[]
		{
			"gui_building",
			"gui_campaigns",
			"gui_troop_training",
			(!UnityUtils.IsWideScreen()) ? "gui_store_2row" : "gui_store_1row",
			"gui_battle_log",
			"gui_leaderboards",
			"gui_settings",
			"gui_squad",
			"gui_battle_report"
		};
	}
}
