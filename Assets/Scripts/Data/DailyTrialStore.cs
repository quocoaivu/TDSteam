using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Parameter;
using UnityEngine;

namespace Data
{
	public class DailyTrialStore : MonoBehaviour
	{
        private static string DB_NAME = "/dailyTrialData.dat";

        private DailyOrdealSerializeRecord data = new DailyOrdealSerializeRecord();

        private int missionDoneMaxTier = 2;

        private int minDay;

        private int maxDay = 6;

        private static DailyTrialStore instance;
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            instance = null;
        }
        
		public static DailyTrialStore Instance
		{
			get
			{
				return DailyTrialStore.instance;
			}
		}

		private void Awake()
		{
			DailyTrialStore.instance = this;
			ReadInputParam();
			ReadRewardParam();
			SaveDefaultData();
			Load();
		}

		private void ReadInputParam()
		{
			string text = "Parameters/MapDailyTrial/map_daily_param";
			try
			{
				List<Dictionary<string, object>> list = CSVLoader.Read(text);
				for (int i = 0; i < list.Count; i++)
				{
					DailyOrdealSpecs parameter = default(DailyOrdealSpecs);
					parameter.day = (int)list[i]["day"];
					parameter.map_id = (int)list[i]["map_id"];
					parameter.title = (string)list[i]["title"];
					parameter.tower_level_max_0 = (int)list[i]["tower_level_max_0"];
					parameter.tower_level_max_1 = (int)list[i]["tower_level_max_1"];
					parameter.tower_level_max_2 = (int)list[i]["tower_level_max_2"];
					parameter.tower_level_max_3 = (int)list[i]["tower_level_max_3"];
					parameter.tower_level_max_4 = (int)list[i]["tower_level_max_4"];
					parameter.input_hero_id_slot_0 = (int)list[i]["input_hero_id_slot_0"];
					parameter.input_hero_id_slot_1 = (int)list[i]["input_hero_id_slot_1"];
					parameter.input_hero_id_slot_2 = (int)list[i]["input_hero_id_slot_2"];
					parameter.input_hero_level = (int)list[i]["input_hero_level"];
					parameter.input_pi_freezing = (int)list[i]["input_pi_freezing"];
					parameter.input_pi_meteor = (int)list[i]["input_pi_meteor"];
					parameter.input_pi_healing = (int)list[i]["input_pi_healing"];
					parameter.input_pi_goldchest = (int)list[i]["input_pi_goldchest"];
					DailyOrdealSpec.Instance.SetParameter(parameter);
				}
			}
			catch (Exception)
			{
				DailyTrialStore.ShowError(text);
				throw;
			}
		}

		private void ReadRewardParam()
		{
			string text = "Parameters/MapDailyTrial/map_daily_reward";
			try
			{
				List<Dictionary<string, object>> list = CSVLoader.Read(text);
				for (int i = 0; i < list.Count; i++)
				{
					DailyOrdealPrizeSpecs rewardParameter = default(DailyOrdealPrizeSpecs);
					rewardParameter.day = (int)list[i]["day"];
					rewardParameter.reward_level = (int)list[i]["reward_level"];
					rewardParameter.condition = (string)list[i]["condition"];
					rewardParameter.wave_require = (int)list[i]["wave_require"];
					rewardParameter.gem_bonus = (int)list[i]["gem_bonus"];
					rewardParameter.item_freezing_bonus = (int)list[i]["item_freezing_bonus"];
					rewardParameter.item_meteor_bonus = (int)list[i]["item_meteor_bonus"];
					rewardParameter.item_healing_bonus = (int)list[i]["item_healing_bonus"];
					rewardParameter.item_goldchest_bonus = (int)list[i]["item_goldchest_bonus"];
					DailyOrdealSpec.Instance.SetRewardParameter(rewardParameter);
				}
			}
			catch (Exception)
			{
				DailyTrialStore.ShowError(text);
				throw;
			}
		}

		private static void ShowError(string filePath)
		{
			UnityEngine.Debug.LogError("File " + filePath + ".csv khÃ´ng tá»“n táº¡i hoáº·c dá»¯ liá»‡u trong file khÃ´ng Ä‘Ãºng Ä‘á»‹nh dáº¡ng.");
		}

		private void SaveDefaultData()
		{
			if (!File.Exists(Application.persistentDataPath + DailyTrialStore.DB_NAME))
			{
				FileStream fileStream = File.Create(Application.persistentDataPath + DailyTrialStore.DB_NAME);
				BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
				data.currentDay = 0;
				data.ListDailyTrialData = new Dictionary<int, DailyOrdealRecord>();
				for (int i = 0; i <= maxDay; i++)
				{
					DailyOrdealRecord dailyTrialData = new DailyOrdealRecord();
					dailyTrialData.missionDoneTier = -1;
					dailyTrialData.playCount = 0;
					data.ListDailyTrialData.Add(i, dailyTrialData);
				}
				binaryFormatter.Serialize(fileStream, data);
				fileStream.Close();
			}
		}

		private void Load()
		{
			string path = Application.persistentDataPath + DailyTrialStore.DB_NAME;
			try
			{
				using (FileStream fileStream = File.Open(path, FileMode.Open))
				{
					BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
					data = (DailyOrdealSerializeRecord)binaryFormatter.Deserialize(fileStream);
				}
			}
			catch (Exception e)
			{
				UnityEngine.Debug.LogError("DailyTrialStore: failed to load save, regenerating default. " + e.Message);
				if (File.Exists(path))
				{
					File.Delete(path);
				}
				SaveDefaultData();
			}
		}

		private void SaveAll()
		{
			FileStream fileStream = File.Create(Application.persistentDataPath + DailyTrialStore.DB_NAME);
			BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
			binaryFormatter.Serialize(fileStream, data);
			fileStream.Close();
		}

		public void IncreaseDay()
		{
			Load();
			data.currentDay++;
			if (data.currentDay > maxDay)
			{
				data.currentDay = maxDay;
			}
			SaveAll();
		}

		public void IncreasePlayCount(int day)
		{
			Load();
			data.ListDailyTrialData[day].playCount++;
			SaveAll();
		}

		public int GetPlayCount(int day)
		{
			Load();
			return data.ListDailyTrialData[day].playCount;
		}

		public int GetCurrentDayIndex()
		{
			Load();
			return data.currentDay;
		}

		public bool IsDoneMaxTierMission(int day)
		{
			Load();
			return data.ListDailyTrialData[day].missionDoneTier == 2;
		}

		public int GetDoneMissionTier(int day)
		{
			Load();
			return data.ListDailyTrialData[day].missionDoneTier;
		}

		public void SetDoneMissionTier(int day, int currentTier)
		{
			Load();
			data.ListDailyTrialData[day].missionDoneTier = currentTier;
			SaveAll();
		}

		public int GetHeroDailyTrialLevel()
		{
			return 9;
		}

		public void RestoreDataFromCloud(PlayerRecord_DailyOrdeal restoredData)
		{
			data.ListDailyTrialData = new Dictionary<int, DailyOrdealRecord>();
			data.currentDay = restoredData.currentDay;
			for (int i = 0; i <= maxDay; i++)
			{
				DailyOrdealRecord dailyTrialData = new DailyOrdealRecord();
				dailyTrialData.missionDoneTier = restoredData.listDataDailyTrial[i].missionDoneTier;
				dailyTrialData.playCount = restoredData.listDataDailyTrial[i].playCount;
				data.ListDailyTrialData.Add(i, dailyTrialData);
			}
			UnityEngine.Debug.Log(data);
			SaveAll();
		}
	}
}
