using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using MetaGame;
using UnityEngine;

namespace Data
{
	public class ThemeStore : MonoBehaviour
	{
        private static string DB_NAME = "/themeInfor.dat";

        private SkinSerializeRecord data = new SkinSerializeRecord();

        private static int themeIDMax = 2;

        private static int statueMaxLevel = 6;

        private List<CriterionSpec> conditionParameters = new List<CriterionSpec>();

        private static ThemeStore instance;
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            instance = null;
        }
        
		public static ThemeStore Instance
		{
			get
			{
				return ThemeStore.instance;
			}
		}

		private void Awake()
		{
			ThemeStore.instance = this;
			SaveDefaultData();
			Load();
			ReadParameterUnlockTheme();
		}

		private void SaveDefaultData()
		{
			if (!File.Exists(Application.persistentDataPath + ThemeStore.DB_NAME))
			{
				FileStream fileStream = File.Create(Application.persistentDataPath + ThemeStore.DB_NAME);
				BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
				data.ListThemeIDUnlocked = new List<int>();
				int item = 0;
				data.ListThemeIDUnlocked.Add(item);
				data.lastThemeIDPlayed = 0;
				binaryFormatter.Serialize(fileStream, data);
				fileStream.Close();
			}
		}

		public void SaveThemeUnlockData(int themeIDUnlocked)
		{
			FileStream fileStream = File.Create(Application.persistentDataPath + ThemeStore.DB_NAME);
			BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
			if (!data.ListThemeIDUnlocked.Contains(themeIDUnlocked))
			{
				data.ListThemeIDUnlocked.Add(themeIDUnlocked);
			}
			binaryFormatter.Serialize(fileStream, data);
			fileStream.Close();
		}

		public void SaveAll()
		{
			FileStream fileStream = File.Create(Application.persistentDataPath + ThemeStore.DB_NAME);
			BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
			binaryFormatter.Serialize(fileStream, data);
			fileStream.Close();
		}

		public void SaveLastThemePlayed(int themeID)
		{
			FileStream fileStream = File.Create(Application.persistentDataPath + ThemeStore.DB_NAME);
			BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
			data.lastThemeIDPlayed = themeID;
			binaryFormatter.Serialize(fileStream, data);
			fileStream.Close();
		}

		private void Load()
		{
			string path = Application.persistentDataPath + ThemeStore.DB_NAME;
			try
			{
				using (FileStream fileStream = File.Open(path, FileMode.Open))
				{
					BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
					data = (SkinSerializeRecord)binaryFormatter.Deserialize(fileStream);
				}
			}
			catch (Exception e)
			{
				UnityEngine.Debug.LogError("ThemeStore: failed to load save, regenerating default. " + e.Message);
				if (File.Exists(path))
				{
					File.Delete(path);
				}
				SaveDefaultData();
			}
		}

		public int GetThemeIDUnlocked()
		{
			int result;
			if (PlayerPrefs.GetInt("TESTunlockAllTheme", 0) > 0)
			{
				result = ThemeStore.themeIDMax;
			}
			else
			{
				result = data.ListThemeIDUnlocked[data.ListThemeIDUnlocked.Count - 1];
			}
			return result;
		}

		public bool IsNextThemeUnlock(int themeID)
		{
			return PlayerPrefs.GetInt("TESTunlockAllTheme", 0) > 0 || data.ListThemeIDUnlocked.Contains(themeID + 1);
		}

		public bool IsReachMaxTheme(int themeID)
		{
			return themeID == ThemeStore.themeIDMax;
		}

		public Dictionary<int, string> GetListCondition(int themeID)
		{
			Dictionary<int, string> dictionary = new Dictionary<int, string>();
			if (themeID != 1)
			{
				if (themeID == 2)
				{
					foreach (CriterionSpec conditionParameter in conditionParameters)
					{
						if (conditionParameter.isEnable_theme2)
						{
							dictionary.Add(conditionParameter.id, conditionParameter.condition_theme2);
						}
					}
				}
			}
			else
			{
				foreach (CriterionSpec conditionParameter2 in conditionParameters)
				{
					if (conditionParameter2.isEnable_theme1)
					{
						dictionary.Add(conditionParameter2.id, conditionParameter2.condition_theme1);
					}
				}
			}
			return dictionary;
		}

		public string GetDescription(int conditionType, int themeID)
		{
			string result = string.Empty;
			if (themeID != 1)
			{
				if (themeID == 2)
				{
					result = conditionParameters[conditionType].description_theme2;
				}
			}
			else
			{
				result = conditionParameters[conditionType].description_theme1;
			}
			return result;
		}

		public int GetLastThemeIDPlayed()
		{
			return data.lastThemeIDPlayed;
		}

		public int GetTotalTheme()
		{
			return ThemeStore.themeIDMax + 1;
		}

		public void RestoreDataFromCloud(PlayerRecord_Skin restoredData)
		{
			data.lastThemeIDPlayed = restoredData.lastThemeIDPlayed;
			data.ListThemeIDUnlocked = new List<int>();
			foreach (int item in restoredData.listThemeIDUnlocked)
			{
				data.ListThemeIDUnlocked.Add(item);
			}
			UnityEngine.Debug.Log(data);
			SaveAll();
		}

		private void ReadParameterUnlockTheme()
		{
			string text = "Parameters/Conditions/theme_unlock_parameter_" + Setup.Instance.LanguageID;
			try
			{
				List<Dictionary<string, object>> list = CSVLoader.Read(text);
				for (int i = 0; i < list.Count; i++)
				{
					CriterionSpec conditionParameter = new CriterionSpec();
					conditionParameter.id = (int)list[i]["id"];
					conditionParameter.conditionType = (string)list[i]["condition_type"];
					conditionParameter.isEnable_theme1 = ((int)list[i]["is_enable_theme_1"] == 1);
					conditionParameter.condition_theme1 = (string)list[i]["condition_theme_1"];
					conditionParameter.description_theme1 = (string)list[i]["description_theme1"];
					conditionParameter.isEnable_theme2 = ((int)list[i]["is_enable_theme_2"] == 1);
					conditionParameter.condition_theme2 = (string)list[i]["condition_theme_2"];
					conditionParameter.description_theme2 = (string)list[i]["description_theme2"];
					conditionParameters.Add(conditionParameter);
				}
			}
			catch (Exception)
			{
				ThemeStore.ShowError(text);
				throw;
			}
		}

		private static void ShowError(string filePath)
		{
			UnityEngine.Debug.LogError("File " + filePath + ".csv khÃ´ng tá»“n táº¡i hoáº·c dá»¯ liá»‡u trong file khÃ´ng Ä‘Ãºng Ä‘á»‹nh dáº¡ng.");
		}
	}
}
