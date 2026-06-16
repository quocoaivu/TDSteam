using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Data
{
	public class OffersStore : MonoBehaviour
	{
        public static string KEY_OFFER_STARTER = "key_offer_starter";

        public static string KEY_OFFER_SPECIAL = "key_offer_special";

        public static string KEY_INSTALL_GOE = "key_install_goe";

        private static string DB_NAME = "/offerInfor.dat";

        private DealRecord data = new DealRecord();

        private static OffersStore instance;
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            instance = null;
        }
        
		public static OffersStore Instance
		{
			get
			{
				return OffersStore.instance;
			}
		}

		public string[] AllOfferKeys
		{
			get
			{
				return allOfferKeys;
			}
			set
			{
				allOfferKeys = value;
			}
		}

		private void Awake()
		{
			OffersStore.instance = this;
			SaveDefaultData();
			Load();
		}

		private void SaveDefaultData()
		{
			if (!File.Exists(Application.persistentDataPath + OffersStore.DB_NAME))
			{
				FileStream fileStream = File.Create(Application.persistentDataPath + OffersStore.DB_NAME);
				BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
				data.ListOfferData = new Dictionary<string, bool>();
				foreach (string key in AllOfferKeys)
				{
					if (!data.ListOfferData.ContainsKey(key))
					{
						data.ListOfferData.Add(key, PlayerPrefs.GetInt(key, 0) == 1);
					}
				}
				UnityEngine.Debug.Log(data);
				binaryFormatter.Serialize(fileStream, data);
				fileStream.Close();
			}
		}

		public void SaveAll()
		{
			FileStream fileStream = File.Create(Application.persistentDataPath + OffersStore.DB_NAME);
			BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
			binaryFormatter.Serialize(fileStream, data);
			fileStream.Close();
		}

		private void Load()
		{
			string path = Application.persistentDataPath + OffersStore.DB_NAME;
			try
			{
				using (FileStream fileStream = File.Open(path, FileMode.Open))
				{
					BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
					data = (DealRecord)binaryFormatter.Deserialize(fileStream);
				}
			}
			catch (Exception e)
			{
				UnityEngine.Debug.LogError("OffersStore: failed to load save, regenerating default. " + e.Message);
				if (File.Exists(path))
				{
					File.Delete(path);
				}
				SaveDefaultData();
			}
		}

		public bool IsOfferProcessed(string offerID)
		{
			Load();
			bool result;
			if (data.ListOfferData.ContainsKey(offerID))
			{
				result = data.ListOfferData[offerID];
			}
			else
			{
				data.ListOfferData.Add(offerID, false);
				SaveAll();
				result = data.ListOfferData[offerID];
			}
			return result;
		}

		public void SetOfferStatus(string offerID, bool value)
		{
			Load();
			data.ListOfferData[offerID] = value;
			SaveAll();
		}

		public void RestoreDataFromCloud(PlayerRecord_Deal restoredData)
		{
			data.ListOfferData = new Dictionary<string, bool>();
			foreach (string key in AllOfferKeys)
			{
				if (!data.ListOfferData.ContainsKey(key))
				{
					if (restoredData == null)
					{
						data.ListOfferData.Add(key, false);
					}
					else if (restoredData.ListOfferData.ContainsKey(key))
					{
						data.ListOfferData.Add(key, restoredData.ListOfferData[key]);
					}
					else
					{
						data.ListOfferData.Add(key, false);
					}
				}
			}
			UnityEngine.Debug.Log(data);
			SaveAll();
		}

		private string[] allOfferKeys = new string[]
		{
			OffersStore.KEY_OFFER_STARTER,
			OffersStore.KEY_OFFER_SPECIAL,
			OffersStore.KEY_INSTALL_GOE
		};
	}
}
