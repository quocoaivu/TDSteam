using System;
using System.Collections.Generic;

namespace Parameter
{
	public class ZoneRuleSpec
	{
		public static ZoneRuleSpec Instance
		{
			get
			{
				if (ZoneRuleSpec.instance == null)
				{
					ZoneRuleSpec.instance = new ZoneRuleSpec();
				}
				return ZoneRuleSpec.instance;
			}
		}

		public void SetMapRuleParameter(CampaignMapRule mapRule)
		{
			int count = listMapRuleCampaign.Count;
			if (count <= mapRule.mapID)
			{
				listMapRuleCampaign.Insert(mapRule.mapID, mapRule);
			}
		}

		public CampaignMapRule GetCampaignMapRuleParameter(int mapID)
		{
			return listMapRuleCampaign[mapID];
		}

		public bool IsTowerAllowed_Campaign(int mapID, int towerID)
		{
			return GetMaxLevelTowerCanUpgrade_Campaign(mapID, towerID) >= 0;
		}

		public int GetMaxLevelTowerCanUpgrade_Campaign(int mapID, int towerID)
		{
			int result = -1;
			switch (towerID)
			{
			case 0:
				result = listMapRuleCampaign[mapID].tower_0_level;
				break;
			case 1:
				result = listMapRuleCampaign[mapID].tower_1_level;
				break;
			case 2:
				result = listMapRuleCampaign[mapID].tower_2_level;
				break;
			case 3:
				result = listMapRuleCampaign[mapID].tower_3_level;
				break;
			case 4:
				result = listMapRuleCampaign[mapID].tower_4_level;
				break;
			}
			return result;
		}

		public int[] GetMaxTowerLevelByMapID(int mapID)
		{
			int[] array = new int[5];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = GetMaxLevelTowerCanUpgrade_Campaign(mapID, i);
			}
			return array;
		}

		public int GetMaxHeroAllowed(int mapID)
		{
			return listMapRuleCampaign[mapID].hero_allowed_max;
		}

		public bool HaveTutorialTowerInMap(int mapID)
		{
			List<TurretTutorial> listTowerTutorialInMap = getListTowerTutorialInMap(mapID);
			return listTowerTutorialInMap.Count > 0;
		}

		public bool HaveNormalInfor(int mapID)
		{
			return getTipInforID(mapID) > 0;
		}

		public int getTipInforID(int mapID)
		{
			return listMapRuleCampaign[mapID].tip_infor_id;
		}

		public bool HaveUltimateInfor(int mapID)
		{
			bool result = false;
			List<TurretTutorial> listTowerTutorialInMap = getListTowerTutorialInMap(mapID);
			foreach (TurretTutorial towerTutorial in listTowerTutorialInMap)
			{
				if (towerTutorial.level >= 3)
				{
					result = true;
				}
			}
			return result;
		}

		public List<TurretTutorial> getListTowerTutorialInMap(int mapID)
		{
			List<TurretTutorial> list = new List<TurretTutorial>();
			foreach (CampaignMapRule campaignMap in listMapRuleCampaign)
			{
				if (campaignMap.mapID == mapID)
				{
					string[] array = campaignMap.tutorial_param_0.Split(new char[]
					{
						'_'
					}, StringSplitOptions.RemoveEmptyEntries);
					if (array.Length == 2)
					{
						TurretTutorial item = new TurretTutorial
						{
							id = int.Parse(array[0]),
							level = int.Parse(array[1])
						};
						list.Add(item);
					}
					string[] array2 = campaignMap.tutorial_param_1.Split(new char[]
					{
						'_'
					}, StringSplitOptions.RemoveEmptyEntries);
					if (array2.Length == 2)
					{
						TurretTutorial item2 = new TurretTutorial
						{
							id = int.Parse(array2[0]),
							level = int.Parse(array2[1])
						};
						list.Add(item2);
					}
					string[] array3 = campaignMap.tutorial_param_2.Split(new char[]
					{
						'_'
					}, StringSplitOptions.RemoveEmptyEntries);
					if (array3.Length == 2)
					{
						TurretTutorial item3 = new TurretTutorial
						{
							id = int.Parse(array3[0]),
							level = int.Parse(array3[1])
						};
						list.Add(item3);
					}
				}
			}
			return list;
		}

		public void SetMapRuleParameter(DailyTrialMapRule mapRule)
		{
			int count = listMapRuleDaily.Count;
			if (count <= mapRule.wave)
			{
				listMapRuleDaily.Insert(mapRule.wave, mapRule);
			}
		}

		public DailyTrialMapRule GetDailyTrialMapRuleParameter(int wave)
		{
			return listMapRuleDaily[wave];
		}

		public int GetGoldBonusForCallEnemy(int wave)
		{
			return listMapRuleDaily[wave].bonus_gold;
		}

		public int GetHealthBonusForCallEnemy(int wave)
		{
			return listMapRuleDaily[wave].bonus_health;
		}

		public bool IsTowerAllowed_Daily(int wave, int towerID)
		{
			return GetMaxLevelTowerCanUpgrade_Daily(wave, towerID) >= 0;
		}

		public int GetMaxLevelTowerCanUpgrade_Daily(int wave, int towerID)
		{
			int result = -1;
			switch (towerID)
			{
			case 0:
				result = listMapRuleDaily[wave].tower_0_level;
				break;
			case 1:
				result = listMapRuleDaily[wave].tower_1_level;
				break;
			case 2:
				result = listMapRuleDaily[wave].tower_2_level;
				break;
			case 3:
				result = listMapRuleDaily[wave].tower_3_level;
				break;
			case 4:
				result = listMapRuleDaily[wave].tower_4_level;
				break;
			}
			return result;
		}

		public bool HaveEventNewTower(int wave)
		{
			return listMapRuleDaily[wave].new_tower_event == 1;
		}

		public List<int> GetListTowerIDForPopup(int wave)
		{
			List<int> list = new List<int>();
			if (listMapRuleDaily[wave].new_tower_id_slot_0 >= 0)
			{
				list.Add(listMapRuleDaily[wave].new_tower_id_slot_0);
			}
			if (listMapRuleDaily[wave].new_tower_id_slot_1 >= 0)
			{
				list.Add(listMapRuleDaily[wave].new_tower_id_slot_1);
			}
			if (listMapRuleDaily[wave].new_tower_id_slot_2 >= 0)
			{
				list.Add(listMapRuleDaily[wave].new_tower_id_slot_2);
			}
			return list;
		}

		public void SetMapRuleParameter(TournamentMapRule mapRule)
		{
			int count = listMapRuleTournament.Count;
			string text = mapRule.seasonID;
			text = text.Remove(0, 2);
			int num = int.Parse(text);
			if (count <= num)
			{
				listMapRuleTournament.Add(mapRule);
			}
		}

		public TournamentMapRule GetTournamentMapRuleParameter(int mapID)
		{
			TournamentMapRule result = default(TournamentMapRule);
			foreach (TournamentMapRule tournamentMap in listMapRuleTournament)
			{
				if (tournamentMap.mapID == mapID)
				{
					result = tournamentMap;
				}
			}
			return result;
		}

		public void AddTournamentConstants(int leagueIndex, ArenaPriceConstant data)
		{
			leagueIndexToPriceConstants.Add(leagueIndex, data);
		}

		public bool IsTowerAllowed_Tournament(string seasonID, int towerID)
		{
			return GetMaxLevelTowerCanUpgrade_Tournament(seasonID, towerID) >= 0;
		}

		public int GetMaxLevelTowerCanUpgrade_Tournament(string seasonID, int towerID)
		{
			int result = 0;
			foreach (TournamentMapRule tournamentMap in listMapRuleTournament)
			{
				if (tournamentMap.seasonID.Equals(seasonID))
				{
					switch (towerID)
					{
					case 0:
						result = tournamentMap.tower_0_level;
						break;
					case 1:
						result = tournamentMap.tower_1_level;
						break;
					case 2:
						result = tournamentMap.tower_2_level;
						break;
					case 3:
						result = tournamentMap.tower_3_level;
						break;
					case 4:
						result = tournamentMap.tower_4_level;
						break;
					}
				}
			}
			return result;
		}

		public int GetCurrentSeasonMapID()
		{
			int result = 0;
			string currentSeasonID = GetCurrentSeasonID();
			foreach (TournamentMapRule tournamentMap in listMapRuleTournament)
			{
				if (tournamentMap.seasonID.Equals(currentSeasonID))
				{
					result = tournamentMap.mapID;
				}
			}
			return result;
		}

		public int GetBlessedHeroID()
		{
			int result = 0;
			string currentSeasonID = GetCurrentSeasonID();
			foreach (TournamentMapRule tournamentMap in listMapRuleTournament)
			{
				if (tournamentMap.seasonID.Equals(currentSeasonID))
				{
					result = tournamentMap.blessedHeroID;
				}
			}
			return result;
		}

		public int GetPowerupItemLimit()
		{
			int result = 0;
			string currentSeasonID = GetCurrentSeasonID();
			foreach (TournamentMapRule tournamentMap in listMapRuleTournament)
			{
				if (tournamentMap.seasonID.Equals(currentSeasonID))
				{
					result = tournamentMap.powerupItemLimit;
				}
			}
			return result;
		}

		public string GetCurrentSeasonID()
		{
			return curSeasonID;
		}

		public void SetCurrentSeasonID(int seasonNumber)
		{
			curSeasonID = "ss" + seasonNumber % 6;
		}

		public int GetEndlessWaveLoopBegin()
		{
			int result = 0;
			string currentSeasonID = GetCurrentSeasonID();
			foreach (TournamentMapRule tournamentMap in listMapRuleTournament)
			{
				if (tournamentMap.seasonID.Equals(currentSeasonID))
				{
					result = tournamentMap.wave_loop_begin;
				}
			}
			return result;
		}

		public int GetEndlessWaveLoopEnd()
		{
			int result = 0;
			string currentSeasonID = GetCurrentSeasonID();
			foreach (TournamentMapRule tournamentMap in listMapRuleTournament)
			{
				if (tournamentMap.seasonID.Equals(currentSeasonID))
				{
					result = tournamentMap.wave_loop_end;
				}
			}
			return result;
		}

		public int GetEndlessHealthIncreasePercentage()
		{
			int result = 0;
			string currentSeasonID = GetCurrentSeasonID();
			foreach (TournamentMapRule tournamentMap in listMapRuleTournament)
			{
				if (tournamentMap.seasonID.Equals(currentSeasonID))
				{
					result = tournamentMap.health_increase_percentage_per_loop;
				}
			}
			return result;
		}

		public int GetEndlessDamageIncreasePercentage()
		{
			int result = 0;
			string currentSeasonID = GetCurrentSeasonID();
			foreach (TournamentMapRule tournamentMap in listMapRuleTournament)
			{
				if (tournamentMap.seasonID.Equals(currentSeasonID))
				{
					result = tournamentMap.damage_increase_percentage_per_loop;
				}
			}
			return result;
		}

		public List<CampaignMapRule> listMapRuleCampaign = new List<CampaignMapRule>();

		public List<DailyTrialMapRule> listMapRuleDaily = new List<DailyTrialMapRule>();

		public List<TournamentMapRule> listMapRuleTournament = new List<TournamentMapRule>();

		public Dictionary<int, ArenaPriceConstant> leagueIndexToPriceConstants = new Dictionary<int, ArenaPriceConstant>();

		public string curSeasonID = "ss0";

		public bool IsMapRuleRead;

		public bool IsPriceConstantRead;

		private static ZoneRuleSpec instance;
		[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			instance = null;
		}
	}
}
