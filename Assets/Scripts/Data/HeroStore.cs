using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using MetaGame;
using Parameter;
using UnityEngine;

namespace Data
{
	public class HeroStore : MonoBehaviour
	{
        private static string DB_NAME = "/heroInfor.dat";

        private HeroSerializeRecord data = new HeroSerializeRecord();

        public static int maxLevel = 9;

        public static int maxSkillLevel = 3;

        private static HeroStore instance;
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            instance = null;
        }
        
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public event Action OnSkillPointChangeEvent;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action OnHeroLevelChangeEvent;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action OnBuyNewHero;

		public static HeroStore Instance
		{
			get
			{
				return HeroStore.instance;
			}
		}

		private void Awake()
		{
			HeroStore.instance = this;
			SaveDefaultData();
			Load();
		}

		private void SaveDefaultData()
		{
			if (!File.Exists(Application.persistentDataPath + HeroStore.DB_NAME))
			{
				FileStream fileStream = File.Create(Application.persistentDataPath + HeroStore.DB_NAME);
				BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
				data.ListHeroesData = new Dictionary<int, HeroRecord>();
				int num = 20;
				for (int i = 0; i < num; i++)
				{
					HeroRecord heroData = new HeroRecord();
					heroData.level = 0;
					data.ListHeroesData.Add(i, heroData);
				}
				data.ListHeroOwned = new List<int>();
				int item = 2;
				data.ListHeroOwned.Add(item);
				InitDefaultDataSkillPoint();
				InitDefaultDataPet();
				binaryFormatter.Serialize(fileStream, data);
				fileStream.Close();
			}
		}

		public void Save(int heroID, int level, int totalExp)
		{
			FileStream fileStream = File.Create(Application.persistentDataPath + HeroStore.DB_NAME);
			BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
			data.ListHeroesData[heroID].level = level;
			data.ListHeroesData[heroID].totalExp = totalExp;
			binaryFormatter.Serialize(fileStream, data);
			fileStream.Close();
		}

		public void SaveAll()
		{
			FileStream fileStream = File.Create(Application.persistentDataPath + HeroStore.DB_NAME);
			BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
			binaryFormatter.Serialize(fileStream, data);
			fileStream.Close();
		}

		private void Load()
		{
			string path = Application.persistentDataPath + HeroStore.DB_NAME;
			try
			{
				using (FileStream fileStream = File.Open(path, FileMode.Open))
				{
					BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
					data = (HeroSerializeRecord)binaryFormatter.Deserialize(fileStream);
				}
			}
			catch (Exception e)
			{
				UnityEngine.Debug.LogError("HeroStore: failed to load save, regenerating default. " + e.Message);
				if (File.Exists(path))
				{
					File.Delete(path);
				}
				SaveDefaultData();
			}
		}

		public int GetCurrentHeroLevel(int heroID)
		{
			int level;
			switch (FormatDirector.Instance.gameMode)
			{
				case GameFormat.CampaignMode:
					level = data.ListHeroesData[heroID].level;
					break;
				case GameFormat.DailyTrialMode:
					level = HeroStore.maxLevel;
					break;
				case GameFormat.TournamentMode:
					level = data.ListHeroesData[heroID].level;
					break;
				default:
					level = data.ListHeroesData[heroID].level;
					break;
			}
			return level;
		}

		public int GetCurrentHeroTotalExp(int heroID)
		{
			return data.ListHeroesData[heroID].totalExp;
		}

		public void AddExp(int heroID, int amountEXP)
		{
			if (IsReachMaxLevel(heroID))
			{
				return;
			}
			data.ListHeroesData[heroID].totalExp += amountEXP;
			if (data.ListHeroesData[heroID].totalExp >= HeroParameterManager.Instance.GetEXPForCurrentLevel(heroID, 9))
			{
				UnityEngine.Debug.Log("Hero Ä‘Ã£ Ä‘áº¡t max level");
				Save(heroID, 9, HeroParameterManager.Instance.GetEXPForCurrentLevel(heroID, 9));
			}
			if (amountEXP >= HeroParameterManager.Instance.GetEXPForCurrentLevel(heroID, 9))
			{
				UnityEngine.Debug.Log("Hero nháº­n Ä‘Æ°á»£c nhiá»u exp quÃ¡ max level luÃ´n rá»“i");
				Save(heroID, 9, HeroParameterManager.Instance.GetEXPForCurrentLevel(heroID, 9));
				return;
			}
			for (int i = 0; i < 10; i++)
			{
				if (data.ListHeroesData[heroID].totalExp < HeroParameterManager.Instance.GetEXPForCurrentLevel(heroID, i))
				{
					data.ListHeroesData[heroID].level = i;
					break;
				}
			}
			Save(heroID, data.ListHeroesData[heroID].level, data.ListHeroesData[heroID].totalExp);
		}

		public void LevelUpTo(int heroID, int heroLevel)
		{
			for (int i = 0; i < heroLevel; i++)
			{
				LevelUp(heroID);
			}
		}

		public void LevelUp(int heroID)
		{
			int currentHeroLevel = GetCurrentHeroLevel(heroID);
			if (IsReachMaxLevel(heroID))
			{
				return;
			}
			UnityEngine.Debug.Log("Current exp = " + data.ListHeroesData[heroID].totalExp);
			UnityEngine.Debug.Log("Exp level tiep theo = " + HeroParameterManager.Instance.GetEXPForCurrentLevel(heroID, currentHeroLevel));
			UnityEngine.Debug.Log("Exp can thiet de len level = " + GetExpToLevelUp(heroID));
			AddExp(heroID, GetExpToLevelUp(heroID));
		}

		public bool IsReachMaxLevel(int heroID)
		{
			bool result = false;
			int currentHeroLevel = GetCurrentHeroLevel(heroID);
			if (currentHeroLevel >= HeroStore.maxLevel)
			{
				UnityEngine.Debug.Log("Hero Ä‘Ã£ max level!");
				result = true;
			}
			return result;
		}

		public int GetExpToLevelUp(int heroID)
		{
			int currentHeroLevel = GetCurrentHeroLevel(heroID);
			return HeroParameterManager.Instance.GetEXPForCurrentLevel(heroID, currentHeroLevel) - data.ListHeroesData[heroID].totalExp;
		}

		public int GetCurrentExp(int heroID)
		{
			int currentHeroLevel = GetCurrentHeroLevel(heroID);
			int result;
			if (currentHeroLevel > 0)
			{
				result = GetCurrentHeroTotalExp(heroID) - HeroParameterManager.Instance.GetEXPForCurrentLevel(heroID, currentHeroLevel - 1);
			}
			else
			{
				result = GetCurrentHeroTotalExp(heroID);
			}
			return result;
		}

		public void OnLevelChange(bool isDispatchEventChange)
		{
			if (isDispatchEventChange && OnHeroLevelChangeEvent != null)
			{
				OnHeroLevelChangeEvent();
			}
		}

		public int GetHeroOwnedAmount()
		{
			return data.ListHeroOwned.Count;
		}

		public List<int> GetListHeroIDOwned()
		{
			return data.ListHeroOwned;
		}

		public bool IsHeroOwned(int heroID)
		{
			return data.ListHeroOwned.Contains(heroID);
		}

		public bool IsHeroOwned(List<int> listHeroID)
		{
			bool result = false;
			foreach (int heroID in listHeroID)
			{
				if (IsHeroOwned(heroID))
				{
					result = true;
				}
			}
			return result;
		}

		public void UnlockHero(int heroID)
		{
			data.ListHeroOwned.Add(heroID);
			SaveAll();
			UnityEngine.Debug.Log("Unlock Hero " + heroID);
			if (OnBuyNewHero != null)
			{
				OnBuyNewHero();
			}
		}

		public List<int> GetListHeroIDNotOwned()
		{
			List<int> list = new List<int>();
			List<int> listHeroID = HeroParameterManager.Instance.GetListHeroID();
			foreach (int num in listHeroID)
			{
				if (!IsHeroOwned(num))
				{
					list.Add(num);
				}
			}
			return list;
		}

		public void IncreaseSkillLevel(int heroID, int skillID)
		{
			if (data.ListHeroesData[heroID].skillPoints == null)
			{
				InitDefaultDataSkillPoint();
				SaveAll();
			}
			data.ListHeroesData[heroID].skillPoints[skillID]++;
			SaveAll();
		}

		public int GetSkillPoint(int heroID, int skillID)
		{
			int result;
			if (data.ListHeroesData[heroID].skillPoints != null)
			{
				result = data.ListHeroesData[heroID].skillPoints[skillID];
			}
			else
			{
				InitDefaultDataSkillPoint();
				SaveAll();
				result = data.ListHeroesData[heroID].skillPoints[skillID];
			}
			return result;
		}

		private void InitDefaultDataSkillPoint()
		{
			int num = 20;
			for (int i = 0; i < num; i++)
			{
				HeroRecord heroData = data.ListHeroesData[i];
				int[] array = new int[4];
				array[0] = 1;
				heroData.skillPoints = array;
			}
		}

		public int GetCurrentSkillPoint(int heroID)
		{
			int currentHeroLevel = GetCurrentHeroLevel(heroID);
			return GetTotalSkillPoint(heroID, currentHeroLevel) - GetUsedSkillPoint(heroID);
		}

		private int GetTotalSkillPoint(int heroID, int heroLevel)
		{
			return HeroParameterManager.Instance.GetTotalSkillPoint(heroID, heroLevel);
		}

		public int GetUsedSkillPoint(int heroID)
		{
			int num = 0;
			if (data.ListHeroesData[heroID].skillPoints == null)
			{
				InitDefaultDataSkillPoint();
				SaveAll();
			}
			for (int i = 0; i < data.ListHeroesData[heroID].skillPoints.Length; i++)
			{
				num += data.ListHeroesData[heroID].skillPoints[i];
			}
			return num;
		}

		public bool IsMaxSkill(int heroID, int skillID)
		{
			return GetSkillPoint(heroID, skillID) == HeroStore.maxSkillLevel;
		}

		public void ResetSkillPoint(int heroID)
		{
			if (data.ListHeroesData[heroID].skillPoints == null)
			{
				InitDefaultDataSkillPoint();
				SaveAll();
			}
			data.ListHeroesData[heroID].skillPoints[0] = 1;
			data.ListHeroesData[heroID].skillPoints[1] = 0;
			data.ListHeroesData[heroID].skillPoints[2] = 0;
			data.ListHeroesData[heroID].skillPoints[3] = 0;
			SaveAll();
		}

		public void OnSkillPointChange(bool isDispatchEventChange)
		{
			if (isDispatchEventChange && OnSkillPointChangeEvent != null)
			{
				OnSkillPointChangeEvent();
			}
		}

		public int GetHeroOwnPetAmount()
		{
			return GetListHeroIDOwnPet().Count;
		}

		public List<int> GetListHeroIDOwnPet()
		{
			List<int> list = new List<int>();
			List<int> listHeroIDOwned = GetListHeroIDOwned();
			foreach (int num in listHeroIDOwned)
			{
				if (IsPetUnlocked(num))
				{
					list.Add(num);
				}
			}
			return list;
		}

		private void InitDefaultDataPet()
		{
			int num = 20;
			for (int i = 0; i < num; i++)
			{
				data.ListHeroesData[i].havePet = false;
			}
		}

		public void UnlockPet(int heroID)
		{
			data.ListHeroesData[heroID].havePet = true;
			SaveAll();
		}

		public bool IsPetUnlocked(int heroID)
		{
			bool result;
			switch (FormatDirector.Instance.gameMode)
			{
				case GameFormat.CampaignMode:
					result = data.ListHeroesData[heroID].havePet;
					break;
				case GameFormat.DailyTrialMode:
					result = true;
					break;
				case GameFormat.TournamentMode:
					result = data.ListHeroesData[heroID].havePet;
					break;
				default:
					result = data.ListHeroesData[heroID].havePet;
					break;
			}
			return result;
		}

		public bool IsPetAvailable(int petID)
		{
			return true;
		}

		public void RestoreDataFromCloud(PlayerRecord_Hero restoredData)
		{
			foreach (PlayerRecord_Hero_Unique userData_Hero_Unique in restoredData.listHeroData)
			{
				HeroRecord heroData = new HeroRecord();
				heroData.level = userData_Hero_Unique.level;
				heroData.totalExp = userData_Hero_Unique.exp;
				if (userData_Hero_Unique.isOwned && !data.ListHeroOwned.Contains(userData_Hero_Unique.id))
				{
					data.ListHeroOwned.Add(userData_Hero_Unique.id);
				}
				heroData.havePet = userData_Hero_Unique.ownedPet;
				heroData.skillPoints = new int[4];
				for (int i = 0; i < heroData.skillPoints.Length; i++)
				{
					heroData.skillPoints[i] = userData_Hero_Unique.skillUpgraded[i];
				}
				data.ListHeroesData[userData_Hero_Unique.id] = heroData;
			}
			UnityEngine.Debug.Log(data);
			SaveAll();
		}
	}
}
