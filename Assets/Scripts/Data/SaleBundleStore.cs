using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Data
{
	public class SaleBundleStore : MonoBehaviour
	{
        private static string DB_NAME = "/saleBundleInfor.dat";

        private SalePackRecord data;

        private string[] listSpecialBundleID = MarketingSetup.productIDSpecialPack;

        private static SaleBundleStore instance;
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            instance = null;
        }
        
		public static SaleBundleStore Instance
		{
			get
			{
				return SaleBundleStore.instance;
			}
		}

		private void Awake()
		{
			SaleBundleStore.instance = this;
			SaveDefaultData();
			Load();
		}

		private void OnApplicationQuit()
		{
			SetLastTimePlay();
		}

		private void SaveDefaultData()
		{
			if (!File.Exists(Application.persistentDataPath + SaleBundleStore.DB_NAME))
			{
				FileStream fileStream = File.Create(Application.persistentDataPath + SaleBundleStore.DB_NAME);
				BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
				data = new SalePackRecord();
				data.ListSpecialBundleData = new List<SerializePackItem>();
				for (int i = 0; i < listSpecialBundleID.Length; i++)
				{
					SerializePackItem serializeBundleItem = new SerializePackItem();
					SalePackSetupRecord dataSaleBundle = ShopPackRecord.GetDataSaleBundle(listSpecialBundleID[i]);
					serializeBundleItem.bundleID = dataSaleBundle.Productid;
					serializeBundleItem.isExpired = false;
					serializeBundleItem.isBought = false;
					data.ListSpecialBundleData.Insert(i, serializeBundleItem);
				}
				UnityEngine.Debug.Log(data);
				binaryFormatter.Serialize(fileStream, data);
				fileStream.Close();
			}
		}

		public void SaveAll()
		{
			FileStream fileStream = File.Create(Application.persistentDataPath + SaleBundleStore.DB_NAME);
			BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
			binaryFormatter.Serialize(fileStream, data);
			fileStream.Close();
		}

		private void Load()
		{
			string path = Application.persistentDataPath + SaleBundleStore.DB_NAME;
			try
			{
				using (FileStream fileStream = File.Open(path, FileMode.Open))
				{
					BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
					data = (SalePackRecord)binaryFormatter.Deserialize(fileStream);
				}
			}
			catch (Exception e)
			{
				UnityEngine.Debug.LogError("SaleBundleStore: failed to load save, regenerating default. " + e.Message);
				if (File.Exists(path))
				{
					File.Delete(path);
				}
				SaveDefaultData();
			}
		}

		public void SetLastTimePlay()
		{
			data.lastTimePlayed = GameKit.GetNow().ToBinary().ToString();
			SaveAll();
		}

		public DateTime GetLastTimePlay()
		{
			Load();
			string lastTimePlayed = data.lastTimePlayed;
			long dateData = Convert.ToInt64(lastTimePlayed);
			return DateTime.FromBinary(dateData);
		}

		public int GetCurrentSpecialPackIndex()
		{
			int result = -1;
			if (data == null || data.ListSpecialBundleData == null)
			{
				Load();
			}
			for (int i = 0; i < listSpecialBundleID.Length; i++)
			{
				if (data == null)
				{
					UnityEngine.Debug.LogError("NULL DATA, PLS CHECK");
					return -1;
				}
				if (data.ListSpecialBundleData == null)
				{
					UnityEngine.Debug.LogError("NULL DATA.ListSpecialBundleData, PLS CHECK");
					return -1;
				}
				SerializePackItem serializeBundleItem = GetSerializeBundleItem(listSpecialBundleID[i]);
				if (!serializeBundleItem.isBought && !serializeBundleItem.isExpired)
				{
					result = i;
					break;
				}
			}
			return result;
		}

		private SerializePackItem GetSerializeBundleItem(string bundleID)
		{
			SerializePackItem result = null;
			foreach (SerializePackItem serializeBundleItem in data.ListSpecialBundleData)
			{
				if (serializeBundleItem.bundleID.Equals(bundleID))
				{
					result = serializeBundleItem;
				}
			}
			return result;
		}

		public int GetCurrentAvailableSpecialPackIndex()
		{
			Load();
			int result = -1;
			int currentSpecialPackIndex = GetCurrentSpecialPackIndex();
			if (currentSpecialPackIndex >= 0)
			{
				if (ShopPackRecord.GetDataSaleBundle(listSpecialBundleID[currentSpecialPackIndex]).Bundletype.Equals(ShopPackKind.Starter.ToString()))
				{
					result = currentSpecialPackIndex;
				}
				if (ShopPackRecord.GetDataSaleBundle(listSpecialBundleID[currentSpecialPackIndex]).Bundletype.Equals(ShopPackKind.TimeLimited.ToString()))
				{
					int condition = ShopPackRecord.GetDataSaleBundle(listSpecialBundleID[currentSpecialPackIndex]).Condition;
					int mapIDUnlocked = MapProgressStore.Instance.GetMapIDUnlocked();
					if (mapIDUnlocked > condition)
					{
						result = currentSpecialPackIndex;
					}
				}
			}
			return result;
		}

		public TimeSpan getCountdownTime(string bundleID)
		{
			TimeSpan result = default(TimeSpan);
			DateTime lastTimePlay = GetLastTimePlay();
			TimeSpan t = GameKit.GetNow().Subtract(lastTimePlay);
			int timecountdown = ShopPackRecord.GetDataSaleBundle(bundleID).Timecountdown;
			TimeSpan timeSpan = new TimeSpan(timecountdown, 0, 0);
			TimeSpan timeSpan2 = timeSpan.Add(-t);
			if (timeSpan2.TotalSeconds > 0.0)
			{
				result = timeSpan2;
			}
			else
			{
				result = TimeSpan.MinValue;
			}
			return result;
		}

		public bool GetSpecialPackExpireStatus(string bundleID)
		{
			Load();
			return GetSerializeBundleItem(bundleID).isExpired;
		}

		public void SetSpecialPackExpired(string bundleID)
		{
			SerializePackItem serializeBundleItem = GetSerializeBundleItem(bundleID);
			serializeBundleItem.isExpired = true;
			SaveAll();
		}

		public bool GetSpecialPackBuyStatus(string bundleID)
		{
			Load();
			return GetSerializeBundleItem(bundleID).isBought;
		}

		public void SetSpecialPackBought(string bundleID)
		{
			SerializePackItem serializeBundleItem = GetSerializeBundleItem(bundleID);
			serializeBundleItem.isBought = true;
			SaveAll();
		}

		public void RestoreDataFromCloud(PlayerRecord_SalePack restoredData)
		{
			data.ListSpecialBundleData = new List<SerializePackItem>();
			for (int i = 0; i < listSpecialBundleID.Length; i++)
			{
				if (restoredData == null)
				{
					SerializePackItem serializeBundleItem = new SerializePackItem();
					serializeBundleItem.bundleID = listSpecialBundleID[i];
					serializeBundleItem.isExpired = false;
					serializeBundleItem.isBought = false;
					data.ListSpecialBundleData.Insert(i, serializeBundleItem);
				}
				else
				{
					SerializePackItem serializeBundleItem2 = new SerializePackItem();
					serializeBundleItem2.bundleID = listSpecialBundleID[i];
					if (restoredData.ListSaleBundleData == null)
					{
						serializeBundleItem2.isExpired = false;
						serializeBundleItem2.isBought = false;
					}
					else if (restoredData.ListSaleBundleData[i] != null)
					{
						serializeBundleItem2.isBought = restoredData.ListSaleBundleData[i].isBought;
						serializeBundleItem2.isExpired = restoredData.ListSaleBundleData[i].isExpired;
					}
					else
					{
						serializeBundleItem2.isExpired = false;
						serializeBundleItem2.isBought = false;
					}
					data.ListSpecialBundleData.Insert(i, serializeBundleItem2);
				}
			}
			UnityEngine.Debug.Log(data);
			SaveAll();
		}
	}
}
