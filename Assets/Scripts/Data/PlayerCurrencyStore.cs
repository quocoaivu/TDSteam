using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Data
{
	public class PlayerCurrencyStore : MonoBehaviour
	{
        private static string DB_NAME = "/playerCurrencyInfor.dat";

        private PlayerCoinageRecord data = new PlayerCoinageRecord();

        private static PlayerCurrencyStore instance;
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            instance = null;
        }

        //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public event Action OnGemChangeEvent;

		public static PlayerCurrencyStore Instance
		{
			get
			{
				return PlayerCurrencyStore.instance;
			}
		}

		private void Awake()
		{
			PlayerCurrencyStore.instance = this;
			SaveDefaultData();
			Load();
			LoadTestData();
		}

		private void LoadTestData()
		{
			if (PlayerPrefs.GetInt("TESTgemNumber", 0) > 0)
			{
				ChangeGem(Singleton<TestSetup>.Instance.gemNumber, true);
			}
		}

		private void SaveDefaultData()
		{
			if (!File.Exists(Application.persistentDataPath + PlayerCurrencyStore.DB_NAME))
			{
				FileStream fileStream = File.Create(Application.persistentDataPath + PlayerCurrencyStore.DB_NAME);
				BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
				data.totalGem = 20;
				binaryFormatter.Serialize(fileStream, data);
				fileStream.Close();
			}
		}

		public void Save(int gem)
		{
			FileStream fileStream = File.Create(Application.persistentDataPath + PlayerCurrencyStore.DB_NAME);
			BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
			data.totalGem = gem;
			binaryFormatter.Serialize(fileStream, data);
			fileStream.Close();
		}

		public void SaveAll()
		{
			FileStream fileStream = File.Create(Application.persistentDataPath + PlayerCurrencyStore.DB_NAME);
			BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
			binaryFormatter.Serialize(fileStream, data);
			fileStream.Close();
		}

		private void Load()
		{
			string path = Application.persistentDataPath + PlayerCurrencyStore.DB_NAME;
			try
			{
				using (FileStream fileStream = File.Open(path, FileMode.Open))
				{
					BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
					data = (PlayerCoinageRecord)binaryFormatter.Deserialize(fileStream);
				}
			}
			catch (Exception e)
			{
				UnityEngine.Debug.LogError("PlayerCurrencyStore: failed to load save, regenerating default. " + e.Message);
				if (File.Exists(path))
				{
					File.Delete(path);
				}
				SaveDefaultData();
			}
		}

		public int GetCurrentGem()
		{
			Load();
			return data.totalGem;
		}

		public int GetCurrentStar()
		{
			int result;
			if (PlayerPrefs.GetInt("TESTstarsNumber", 0) > 0)
			{
				result = Singleton<TestSetup>.Instance.starsNumber;
			}
			else
			{
				result = MapProgressStore.Instance.GetTotalStarEarned();
			}
			return result;
		}

		public void ChangeGem(int gemAmount, bool isDispatchEventChange)
		{
			if (gemAmount < 0)
			{
				GameSignalCenter.Instance.Trigger(GameSignalKind.EventUseGem, new SignalTriggerRecord(SignalTriggerKind.UseGem, Mathf.Abs(gemAmount), true));
			}
			data.totalGem += gemAmount;
			Save(data.totalGem);
			DispatchGemChangeEvent(isDispatchEventChange);
		}

		private void DispatchGemChangeEvent(bool isDispatchEventChange)
		{
			if (isDispatchEventChange && OnGemChangeEvent != null)
			{
				OnGemChangeEvent();
			}
		}

		public void RestoreDataFromCloud(PlayerRecord_PlayerDossier restoredData)
		{
			data.totalGem = restoredData.totalGem;
			SaveAll();
			DispatchGemChangeEvent(true);
		}
	}
}
