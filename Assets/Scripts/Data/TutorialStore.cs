using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Tutorial;
using UnityEngine;

namespace Data
{
	public class TutorialStore : MonoBehaviour
	{
        public static string TUTORIAL_ID_SELECT_MAP = "TutorialSelectMapPassed";

        public static string TUTORIAL_ID_START_GAME_MAP_LEVEL = "TutorialMapLevelStartGame";

        public static string TUTORIAL_ID_BRING_FIRST_HERO = "TutorialBringFirstHeroPassed";

        public static string TUTORIAL_ID_BUILD_TOWER = "TutorialBuildtowerPassed";

        public static string TUTORIAL_ID_HERO_MOVE = "TutorialHeroMovePassed";

        public static string TUTORIAL_ID_CALL_ENEMY = "TutorialCallEnemyPassed";

        public static string TUTORIAL_ID_USE_SPEED_UP = "TutorialUseSpeedUpPassed";

        public static string TUTORIAL_ID_HERO_SKILL = "TutorialUseHeroSkillPassed";

        public static string TUTORIAL_ID_LUCKY_CHEST = "TutorialOpenLuckyChest";

        public static string TUTORIAL_ID_GET_LUCKY_CHEST_BY_VIDEO = "TutorialGetLuckyChestByVideo";

        public static string TUTORIAL_ID_WORLD_MAP = "WorldMapTutorialPassed";

        public static string TUTORIAL_ID_GO_HERO_CAMP_1ST = "TutorialToHeroCampPanelPassed";

        public static string TUTORIAL_ID_UPGRADE_HERO_LEVEL = "TutorialUpgradeHeroByGemPassed";

        public static string TUTORIAL_ID_GO_GLOBAL_UPGRADE = "TutorialToUpgradePanelPassed";

        public static string TUTORIAL_ID_GLOBAL_UPGRADE = "TutorialInUpgradePanelPassed";

        public static string TUTORIAL_ID_SELECT_SECOND_MAP = "TutorialSelectSecondMapPassed";

        public static string TUTORIAL_ID_BRING_SECOND_HERO = "TutorialBringSecondHeroPassed";

        public static string TUTORIAL_ID_GO_HERO_CAMP_2ND = "TutorialToHeroCampPanel2Passed";

        public static string TUTORIAL_ID_UPGRADE_HERO_SKILL = "TutorialUseHeroSkillPointPassed";

        public static string TUTORIAL_ID_GO_TOURNAMENT = "TutorialToTournament";

        private static string DB_NAME = "/tutorialInfor.dat";

        private TutorialRecord data = new TutorialRecord();

        public TutorialBase currentTutorial;

        private static TutorialStore instance;
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            instance = null;
        }
        
		public static TutorialStore Instance
		{
			get
			{
				return TutorialStore.instance;
			}
		}

		public string[] AllTutorialKeys
		{
			get
			{
				return allTutorialKeys;
			}
			set
			{
				allTutorialKeys = value;
			}
		}

		private void Awake()
		{
			TutorialStore.instance = this;
			SaveDefaultData();
			Load();
		}

		private void SaveDefaultData()
		{
			if (!File.Exists(Application.persistentDataPath + TutorialStore.DB_NAME))
			{
				FileStream fileStream = File.Create(Application.persistentDataPath + TutorialStore.DB_NAME);
				BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
				data.ListTutorialData = new Dictionary<string, bool>();
				foreach (string key in AllTutorialKeys)
				{
					if (!data.ListTutorialData.ContainsKey(key))
					{
						data.ListTutorialData.Add(key, PlayerPrefs.GetInt(key, 0) == 1);
					}
				}
				Common.SSRTrace.Log(data);
				binaryFormatter.Serialize(fileStream, data);
				fileStream.Close();
			}
		}

		public void SaveAll()
		{
			FileStream fileStream = File.Create(Application.persistentDataPath + TutorialStore.DB_NAME);
			BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
			binaryFormatter.Serialize(fileStream, data);
			fileStream.Close();
		}

		private void Load()
		{
			string path = Application.persistentDataPath + TutorialStore.DB_NAME;
			try
			{
				using (FileStream fileStream = File.Open(path, FileMode.Open))
				{
					BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
					data = (TutorialRecord)binaryFormatter.Deserialize(fileStream);
				}
			}
			catch (Exception e)
			{
				UnityEngine.Debug.LogError("TutorialStore: failed to load save, regenerating default. " + e.Message);
				if (File.Exists(path))
				{
					File.Delete(path);
				}
				SaveDefaultData();
			}
		}

		public bool GetTutorialStatus(string tutorialID)
		{
			EnsureLoaded();
			bool result;
			if (data.ListTutorialData.ContainsKey(tutorialID))
			{
				result = data.ListTutorialData[tutorialID];
			}
			else
			{
				data.ListTutorialData.Add(tutorialID, false);
				SaveAll();
				result = data.ListTutorialData[tutorialID];
			}
			return result;
		}

		public void SetTutorialStatus(string tutorialID, bool value)
		{
			EnsureLoaded();
			data.ListTutorialData[tutorialID] = value;
			SaveAll();
		}

		// Náº¡p dá»¯ liá»‡u tá»« Ä‘Ä©a Ä‘Ãºng 1 láº§n. TrÆ°á»›c Ä‘Ã¢y Load() cháº¡y má»—i láº§n Get/Set gÃ¢y má»Ÿ file +
		// BinaryFormatter.Deserialize láº·p láº¡i. data Ä‘Ã£ lÃ  nguá»“n chÃ¢n lÃ½ trong RAM sau láº§n náº¡p Ä‘áº§u.
		private void EnsureLoaded()
		{
			if (data.ListTutorialData == null)
			{
				Load();
			}
		}

		public void SkipAllTutorials()
		{
			EnsureLoaded();
			foreach (string tutorialID in AllTutorialKeys)
			{
				data.ListTutorialData[tutorialID] = true;
			}
			SaveAll();
			Common.SSRTrace.Log("Skip All Tutorial!");
		}

		public void SetCurrentTutorialPass()
		{
			if (currentTutorial)
			{
				currentTutorial.SetTutorialPassed();
			}
		}

		public void RestoreDataFromCloud(PlayerRecord_Tutorial restoredData)
		{
			data.ListTutorialData = new Dictionary<string, bool>();
			foreach (string key in AllTutorialKeys)
			{
				if (!data.ListTutorialData.ContainsKey(key))
				{
					if (restoredData.ListTutorialData.ContainsKey(key))
					{
						data.ListTutorialData.Add(key, restoredData.ListTutorialData[key]);
					}
					else
					{
						data.ListTutorialData.Add(key, false);
					}
				}
			}
			Common.SSRTrace.Log(data);
			SaveAll();
		}

		private string[] allTutorialKeys = new string[]
		{
			TutorialStore.TUTORIAL_ID_SELECT_MAP,
			TutorialStore.TUTORIAL_ID_START_GAME_MAP_LEVEL,
			TutorialStore.TUTORIAL_ID_BRING_FIRST_HERO,
			TutorialStore.TUTORIAL_ID_BUILD_TOWER,
			TutorialStore.TUTORIAL_ID_HERO_MOVE,
			TutorialStore.TUTORIAL_ID_CALL_ENEMY,
			TutorialStore.TUTORIAL_ID_USE_SPEED_UP,
			TutorialStore.TUTORIAL_ID_HERO_SKILL,
			TutorialStore.TUTORIAL_ID_LUCKY_CHEST,
			TutorialStore.TUTORIAL_ID_GET_LUCKY_CHEST_BY_VIDEO,
			TutorialStore.TUTORIAL_ID_WORLD_MAP,
			TutorialStore.TUTORIAL_ID_GO_HERO_CAMP_1ST,
			TutorialStore.TUTORIAL_ID_UPGRADE_HERO_LEVEL,
			TutorialStore.TUTORIAL_ID_GO_GLOBAL_UPGRADE,
			TutorialStore.TUTORIAL_ID_GLOBAL_UPGRADE,
			TutorialStore.TUTORIAL_ID_SELECT_SECOND_MAP,
			TutorialStore.TUTORIAL_ID_BRING_SECOND_HERO,
			TutorialStore.TUTORIAL_ID_GO_HERO_CAMP_2ND,
			TutorialStore.TUTORIAL_ID_UPGRADE_HERO_SKILL,
			TutorialStore.TUTORIAL_ID_GO_TOURNAMENT
		};
	}
}
