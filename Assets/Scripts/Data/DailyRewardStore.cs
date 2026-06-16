using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Data
{
	public class DailyRewardStore : MonoBehaviour
	{
        private static string DB_NAME = "/dailyRewardData.dat";

        private DailyPrizeSerializeRecord data = new DailyPrizeSerializeRecord();

        private int minDay;

        private int maxDay = 13;

        private static DailyRewardStore instance;
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            instance = null;
        }
        
		public static DailyRewardStore Instance
		{
			get
			{
				return DailyRewardStore.instance;
			}
		}

		private void Awake()
		{
			DailyRewardStore.instance = this;
			SaveDefaultData();
			Load();
		}

		private void SaveDefaultData()
		{
			if (!File.Exists(Application.persistentDataPath + DailyRewardStore.DB_NAME))
			{
				FileStream fileStream = File.Create(Application.persistentDataPath + DailyRewardStore.DB_NAME);
				BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
				data.ListDailyRewarData = new List<DailyPrizeRecord>();
				data.currentDay = 0;
				for (int i = 0; i <= maxDay; i++)
				{
					DailyPrizeRecord dailyRewardData = new DailyPrizeRecord();
					dailyRewardData.day = i;
					dailyRewardData.isReceivedReward = false;
					dailyRewardData.isReceivedBonus = false;
					data.ListDailyRewarData.Insert(i, dailyRewardData);
				}
				binaryFormatter.Serialize(fileStream, data);
				fileStream.Close();
			}
		}

		private void Load()
		{
			string path = Application.persistentDataPath + DailyRewardStore.DB_NAME;
			try
			{
				using (FileStream fileStream = File.Open(path, FileMode.Open))
				{
					BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
					data = (DailyPrizeSerializeRecord)binaryFormatter.Deserialize(fileStream);
				}
			}
			catch (Exception e)
			{
				UnityEngine.Debug.LogError("DailyRewardStore: failed to load save, regenerating default. " + e.Message);
				if (File.Exists(path))
				{
					File.Delete(path);
				}
				SaveDefaultData();
			}
		}

		private void SaveAll()
		{
			FileStream fileStream = File.Create(Application.persistentDataPath + DailyRewardStore.DB_NAME);
			BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
			binaryFormatter.Serialize(fileStream, data);
			fileStream.Close();
		}

		public void TryIncreaseDay()
		{
			Load();
			if (data.ListDailyRewarData[data.currentDay].isReceivedReward)
			{
				UnityEngine.Debug.Log("Ä‘Ã£ nháº­n reward, tÄƒng ngÃ y!");
				data.currentDay++;
				if (data.currentDay > maxDay)
				{
					UnityEngine.Debug.Log("reset ngÃ y nháº­n dailyreward");
					data.currentDay = minDay;
					ResetData();
				}
				SaveAll();
			}
			else
			{
				UnityEngine.Debug.Log("chÆ°a nháº­n reward, chÆ°a tÄƒng ngÃ y!");
			}
		}

		public int GetCurrentDay()
		{
			Load();
			return data.currentDay;
		}

		public bool IsReceivedReward(int dayIndex)
		{
			Load();
			return data.ListDailyRewarData[dayIndex].isReceivedReward;
		}

		public void SetReceiveRewardStatus(int dayIndex)
		{
			data.ListDailyRewarData[dayIndex].isReceivedReward = true;
			SaveAll();
		}

		public bool IsReceivedBonus(int dayIndex)
		{
			Load();
			return data.ListDailyRewarData[dayIndex].isReceivedBonus;
		}

		public void SetReceiveBonusStatus(int dayIndex)
		{
			data.ListDailyRewarData[dayIndex].isReceivedBonus = true;
			SaveAll();
		}

		public void ResetData()
		{
			for (int i = 0; i <= maxDay; i++)
			{
				data.ListDailyRewarData[i].isReceivedReward = false;
				data.ListDailyRewarData[i].isReceivedBonus = false;
			}
			SaveAll();
		}

		public void RestoreDataFromCloud(PlayerRecord_DailyPrize restoredData)
		{
			data.ListDailyRewarData = new List<DailyPrizeRecord>();
			if (restoredData != null)
			{
				data.currentDay = restoredData.currentDay;
			}
			for (int i = 0; i <= maxDay; i++)
			{
				if (restoredData == null)
				{
					DailyPrizeRecord dailyRewardData = new DailyPrizeRecord();
					dailyRewardData.day = i;
					dailyRewardData.isReceivedReward = false;
					dailyRewardData.isReceivedBonus = false;
					data.ListDailyRewarData.Insert(i, dailyRewardData);
				}
				else
				{
					DailyPrizeRecord dailyRewardData2 = new DailyPrizeRecord();
					dailyRewardData2.day = i;
					if (restoredData.listDailyRewardData == null)
					{
						dailyRewardData2.isReceivedReward = false;
						dailyRewardData2.isReceivedBonus = false;
					}
					else if (restoredData.listDailyRewardData[i] != null)
					{
						dailyRewardData2.isReceivedReward = restoredData.listDailyRewardData[i].isReceivedReward;
						dailyRewardData2.isReceivedBonus = restoredData.listDailyRewardData[i].isReceivedBonus;
					}
					else
					{
						dailyRewardData2.isReceivedReward = false;
						dailyRewardData2.isReceivedBonus = false;
					}
					data.ListDailyRewarData.Insert(i, dailyRewardData2);
				}
			}
			UnityEngine.Debug.Log(data);
			SaveAll();
		}
	}
}
