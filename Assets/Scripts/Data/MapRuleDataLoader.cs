using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using MetaGame;
using GameCore;
using Parameter;
using UnityEngine;
using UnityEngine.Networking;

namespace Data
{
	public class MapRuleDataLoader : MonoBehaviour
	{

        [Header("Tournament data")]
        [SerializeField]
        private string jsonDataTournamentMapRuleString;

        public string jsonDataTournamentPriceConstantPath;

        public string jsonDataTournamentRewardPath = "https://firebasestorage.googleapis.com/v0/b/kingdom-defense-82890624.appspot.com/o/tournament_reward.txt?alt=media&token=52f10978-c646-4f13-a220-89a3546d7c04";

        private JArray jsonDataTournamentMapRule;
        
		private void Awake()
		{
			GameFormat gameMode = FormatDirector.Instance.gameMode;
			if (gameMode == GameFormat.CampaignMode)
			{
				ReadCampaignMapRule();
			}
			else if (gameMode == GameFormat.DailyTrialMode)
			{
				ReadDailyTrialMapRule();
			}
			// Tournament map rule is loaded on-demand via ReadTournamentMapRule().
		}

		private void ReadCampaignMapRule()
		{
			string text = "Parameters/map_rule";
			try
			{
				List<Dictionary<string, object>> list = CSVLoader.Read(text);
				for (int i = 0; i < list.Count; i++)
				{
					CampaignMapRule mapRuleParameter = default(CampaignMapRule);
					mapRuleParameter.mapID = (int)list[i]["map_id"];
					mapRuleParameter.tower_0_level = (int)list[i]["tower_0_level"];
					mapRuleParameter.tower_1_level = (int)list[i]["tower_1_level"];
					mapRuleParameter.tower_2_level = (int)list[i]["tower_2_level"];
					mapRuleParameter.tower_3_level = (int)list[i]["tower_3_level"];
					mapRuleParameter.tower_4_level = (int)list[i]["tower_4_level"];
					mapRuleParameter.hero_allowed_max = (int)list[i]["hero_allowed_max"];
					mapRuleParameter.have_tower_preview = (int)list[i]["have_tower_preview"];
					mapRuleParameter.tutorial_param_0 = (string)list[i]["param_0"];
					mapRuleParameter.tutorial_param_1 = (string)list[i]["param_1"];
					mapRuleParameter.tutorial_param_2 = (string)list[i]["param_2"];
					mapRuleParameter.tip_infor_id = (int)list[i]["tip_infor_id"];
					ZoneRuleSpec.Instance.SetMapRuleParameter(mapRuleParameter);
				}
			}
			catch (Exception)
			{
				MapRuleDataLoader.ShowError(text);
				throw;
			}
		}

		private void ReadDailyTrialMapRule()
		{
			int currentDayIndex = DailyTrialStore.Instance.GetCurrentDayIndex();
			string text = "Parameters/MapDailyTrial/daily_trial_map_rule_" + currentDayIndex;
			try
			{
				List<Dictionary<string, object>> list = CSVLoader.Read(text);
				for (int i = 0; i < list.Count; i++)
				{
					DailyTrialMapRule mapRuleParameter = default(DailyTrialMapRule);
					mapRuleParameter.wave = (int)list[i]["wave"];
					mapRuleParameter.tower_0_level = (int)list[i]["tower_0_level"];
					mapRuleParameter.tower_1_level = (int)list[i]["tower_1_level"];
					mapRuleParameter.tower_2_level = (int)list[i]["tower_2_level"];
					mapRuleParameter.tower_3_level = (int)list[i]["tower_3_level"];
					mapRuleParameter.tower_4_level = (int)list[i]["tower_4_level"];
					mapRuleParameter.bonus_gold = (int)list[i]["bonus_gold"];
					mapRuleParameter.bonus_health = (int)list[i]["bonus_health"];
					mapRuleParameter.new_tower_event = (int)list[i]["new_tower_event"];
					mapRuleParameter.new_tower_id_slot_0 = (int)list[i]["new_tower_id_slot0"];
					mapRuleParameter.new_tower_id_slot_1 = (int)list[i]["new_tower_id_slot1"];
					mapRuleParameter.new_tower_id_slot_2 = (int)list[i]["new_tower_id_slot2"];
					ZoneRuleSpec.Instance.SetMapRuleParameter(mapRuleParameter);
				}
			}
			catch (Exception)
			{
				MapRuleDataLoader.ShowError(text);
				throw;
			}
		}

		public void ReadTournamentMapRule()
		{
			if (ZoneRuleSpec.Instance.IsMapRuleRead)
			{
				GameSignalCenter.Instance.Trigger(GameSignalKind.OnTournamentMapRuleReceived, null);
				return;
			}
			// TEST: load from local Resources while the Firebase backend is down.
			if (GeneralVariable.GeneralDefine.TOURNAMENT_USE_LOCAL_DATA)
			{
				TextAsset textAsset = Resources.Load<TextAsset>(GeneralVariable.GeneralDefine.TOURNAMENT_LOCAL_DIR + "map_tournament_rule");
				ParseTournamentMapRule(textAsset.text);
				ZoneRuleSpec.Instance.IsMapRuleRead = true;
				GameSignalCenter.Instance.Trigger(GameSignalKind.OnTournamentMapRuleReceived, null);
				return;
			}
			if (GameUtils.IsInternetConnectionAvailable())
			{
				base.StartCoroutine(GetStoragedDataTournamentMapRule());
			}
		}

		private IEnumerator GetStoragedDataTournamentMapRule()
		{
			UnityWebRequest www = UnityWebRequest.Get(jsonDataTournamentMapRuleString);
			yield return www.SendWebRequest();
			if (www.result == UnityWebRequest.Result.ConnectionError)
			{
				UnityEngine.Debug.Log(www.error);
			}
			else
			{
				ParseTournamentMapRule(www.downloadHandler.text);
				ZoneRuleSpec.Instance.IsMapRuleRead = true;
				yield return new WaitForEndOfFrame();
				yield return new WaitForEndOfFrame();
				yield return new WaitForSeconds(0.5f);
				GameSignalCenter.Instance.Trigger(GameSignalKind.OnTournamentMapRuleReceived, null);
			}
			yield break;
		}

		private void ParseTournamentMapRule(string rawJson)
		{
			jsonDataTournamentMapRule = JArray.Parse(rawJson);
			{
				for (int i = 0; i < jsonDataTournamentMapRule.Count; i++)
				{
					TournamentMapRule mapRuleParameter = default(TournamentMapRule);
					if (jsonDataTournamentMapRule[i]["seasonID"] != null)
					{
						mapRuleParameter.seasonID = (string)jsonDataTournamentMapRule[i]["seasonID"];
					}
					if (jsonDataTournamentMapRule[i]["mapID"] != null)
					{
						mapRuleParameter.mapID = (int)jsonDataTournamentMapRule[i]["mapID"];
					}
					if (jsonDataTournamentMapRule[i]["hero_blessed_id"] != null)
					{
						mapRuleParameter.blessedHeroID = (int)jsonDataTournamentMapRule[i]["hero_blessed_id"];
					}
					if (jsonDataTournamentMapRule[i]["powerup_item_limit"] != null)
					{
						mapRuleParameter.powerupItemLimit = (int)jsonDataTournamentMapRule[i]["powerup_item_limit"];
					}
					if (jsonDataTournamentMapRule[i]["tower_0_level"] != null)
					{
						mapRuleParameter.tower_0_level = (int)jsonDataTournamentMapRule[i]["tower_0_level"];
					}
					if (jsonDataTournamentMapRule[i]["tower_1_level"] != null)
					{
						mapRuleParameter.tower_1_level = (int)jsonDataTournamentMapRule[i]["tower_1_level"];
					}
					if (jsonDataTournamentMapRule[i]["tower_2_level"] != null)
					{
						mapRuleParameter.tower_2_level = (int)jsonDataTournamentMapRule[i]["tower_2_level"];
					}
					if (jsonDataTournamentMapRule[i]["tower_3_level"] != null)
					{
						mapRuleParameter.tower_3_level = (int)jsonDataTournamentMapRule[i]["tower_3_level"];
					}
					if (jsonDataTournamentMapRule[i]["tower_4_level"] != null)
					{
						mapRuleParameter.tower_4_level = (int)jsonDataTournamentMapRule[i]["tower_4_level"];
					}
					if (jsonDataTournamentMapRule[i]["wave_loop_begin"] != null)
					{
						mapRuleParameter.wave_loop_begin = (int)jsonDataTournamentMapRule[i]["wave_loop_begin"];
					}
					if (jsonDataTournamentMapRule[i]["wave_loop_end"] != null)
					{
						mapRuleParameter.wave_loop_end = (int)jsonDataTournamentMapRule[i]["wave_loop_end"];
					}
					if (jsonDataTournamentMapRule[i]["health_increase_percentage_per_loop"] != null)
					{
						mapRuleParameter.health_increase_percentage_per_loop = (int)jsonDataTournamentMapRule[i]["health_increase_percentage_per_loop"];
					}
					if (jsonDataTournamentMapRule[i]["damage_increase_percentage_per_loop"] != null)
					{
						mapRuleParameter.damage_increase_percentage_per_loop = (int)jsonDataTournamentMapRule[i]["damage_increase_percentage_per_loop"];
					}
					ZoneRuleSpec.Instance.SetMapRuleParameter(mapRuleParameter);
				}
			}
		}

		public void ReadTournamentPriceConstants()
		{
			if (ZoneRuleSpec.Instance.IsPriceConstantRead)
			{
				GameSignalCenter.Instance.Trigger(GameSignalKind.OnTournamentPriceConstantsReceived, null);
				return;
			}
			// TEST: load from local Resources while the Firebase backend is down.
			if (GeneralVariable.GeneralDefine.TOURNAMENT_USE_LOCAL_DATA)
			{
				TextAsset rewardAsset = Resources.Load<TextAsset>(GeneralVariable.GeneralDefine.TOURNAMENT_LOCAL_DIR + "tournament_reward");
				ParseTournamentReward(rewardAsset.text);
				TextAsset constantsAsset = Resources.Load<TextAsset>(GeneralVariable.GeneralDefine.TOURNAMENT_LOCAL_DIR + "tournament_constants");
				ParseTournamentPriceConstants(constantsAsset.text);
				ZoneRuleSpec.Instance.IsPriceConstantRead = true;
				GameSignalCenter.Instance.Trigger(GameSignalKind.OnTournamentPriceConstantsReceived, null);
				return;
			}
			if (GameUtils.IsInternetConnectionAvailable())
			{
				base.StartCoroutine(GetStoragedDataTournamentPriceConstants());
			}
		}

		private IEnumerator GetStoragedDataTournamentPriceConstants()
		{
			UnityWebRequest rewardWWW = UnityWebRequest.Get(jsonDataTournamentRewardPath);
			yield return rewardWWW.SendWebRequest();
			if (rewardWWW.result == UnityWebRequest.Result.ConnectionError)
			{
				UnityEngine.Debug.Log(rewardWWW.error);
			}
			else
			{
				ParseTournamentReward(rewardWWW.downloadHandler.text);
			}
			UnityWebRequest www = UnityWebRequest.Get(jsonDataTournamentPriceConstantPath);
			yield return www.SendWebRequest();
			if (www.result == UnityWebRequest.Result.ConnectionError)
			{
				UnityEngine.Debug.Log(www.error);
			}
			else
			{
				ParseTournamentPriceConstants(www.downloadHandler.text);
				ZoneRuleSpec.Instance.IsPriceConstantRead = true;
				yield return new WaitForEndOfFrame();
				yield return new WaitForEndOfFrame();
				yield return new WaitForEndOfFrame();
				GameSignalCenter.Instance.Trigger(GameSignalKind.OnTournamentPriceConstantsReceived, null);
			}
			yield break;
		}

		private void ParseTournamentReward(string rawJson)
		{
			JArray array = JArray.Parse(rawJson);
			// ArenaPrizeSetup is a ScriptableObject; must use CreateInstance, not `new`
			// (the original `new` only never errored because the HTTP path was dead).
			ArenaPrizeSetup tournamentPrizeConfig = ScriptableObject.CreateInstance<ArenaPrizeSetup>();
			tournamentPrizeConfig.dataArray = new ArenaPrizeSetupRecord[array.Count];
			for (int i = 0; i < array.Count; i++)
			{
				ArenaPrizeSetupRecord tournamentPrizeConfigData = new ArenaPrizeSetupRecord();
				tournamentPrizeConfigData.Leagueindex = (int)array[i]["LeagueIndex"];
				tournamentPrizeConfigData.Rankrangelower = (int)array[i]["RankRangeLower"];
				tournamentPrizeConfigData.Rankrangeupper = (int)array[i]["RankRangeUpper"];
				tournamentPrizeConfigData.Itemtypes = (string)array[i]["ItemTypes"];
				tournamentPrizeConfigData.Itemquantities = GameKit.DecodeStringToIntArray((string)array[i]["ItemQuantities"]);
				tournamentPrizeConfig.dataArray[i] = tournamentPrizeConfigData;
			}
			ConfigRegistry.Instance.tournamentPrizeConfig = tournamentPrizeConfig;
		}

		private void ParseTournamentPriceConstants(string rawJson)
		{
			JArray result = JArray.Parse(rawJson);
			for (int j = 0; j < result.Count; j++)
			{
				ArenaPriceConstant tournamentPriceConstant = new ArenaPriceConstant();
				int leagueIndex = 0;
				if (result[j]["LeagueIndex"] != null)
				{
					leagueIndex = (int)result[j]["LeagueIndex"];
				}
				if (result[j]["Jfactor"] != null)
				{
					float.TryParse(result[j]["Jfactor"].ToString(), out tournamentPriceConstant.jFactor);
				}
				if (result[j]["InitGemQuantity"] != null)
				{
					tournamentPriceConstant.initGemQuantity = (int)result[j]["InitGemQuantity"];
				}
				ZoneRuleSpec.Instance.AddTournamentConstants(leagueIndex, tournamentPriceConstant);
			}
		}

		private static void ShowError(string filePath)
		{
			UnityEngine.Debug.LogError("File " + filePath + ".csv khÃ´ng tá»“n táº¡i hoáº·c dá»¯ liá»‡u trong file khÃ´ng Ä‘Ãºng Ä‘á»‹nh dáº¡ng.");
		}
	}
}
