using System;
using System.Collections.Generic;

namespace GeneralVariable
{
	public class GeneralDefine
	{
		public static string NONE = string.Empty;

		public static string HERO_TAG = "Hero";

		public static string ALLY_TAG = "Ally";

		public static string ENEMY_TAG = "Enemy";

		public static string TOWER_TAG = "Tower";

		public static string PET_TAG = "Pet";

		public static string ROAD_TAG = "Road";

		public static string BUILD_REGION_TAG = "BuildRegion";

		public static string UN_MOVEABLE_TAG = "UnMoveable";

		public static string ANIM_BOSS_IN_MAP = "AnimBossInMap";

		public static string DROPPED_GOLD = "DroppedGold";

		public static string WATCH_OFFER_BUTTON = "WatchOfferButton";

		public static string SELECT_ENEMY_INDICATOR_PATH = "FXs/SelectEnemyIndicator";

		public static string SELECT_ENEMY_INDICATOR_NAME = "SelectEnemyIndicator(Clone)";

		public static string TUNNEL_IN_NAME = "TunnelIn";

		public static string TUNNEL_OUT_NAME = "TunnelOut";

		public static int CONNECTION_TIMEOUT = 60;

		// TEST ONLY: load tournament content (map rule / reward / price / waves) from
		// local Resources instead of the (currently dead) Firebase Storage URLs.
		// Set to false once a real backend (Steam/Nakama/...) is wired. See
		// Resources/parameters/maptournament/json form/.
		public static bool TOURNAMENT_USE_LOCAL_DATA = true;

		public static string TOURNAMENT_LOCAL_DIR = "parameters/maptournament/json form/";

		public static List<string> listPermissionLogin = new List<string>
		{
			"public_profile",
			"email",
			"user_friends"
		};
	}
}
