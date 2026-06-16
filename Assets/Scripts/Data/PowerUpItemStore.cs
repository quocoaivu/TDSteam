using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;

namespace Data
{
	public class PowerUpItemStore : MonoBehaviour
	{

        private ObscuredInt[] encodedItemQuantity;

        private static string DB_NAME = "/powerUpItemQuantity.dat";

        private int totalPowerupItem = 9;

        private static PowerUpItemStore instance;
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            instance = null;
        }

        //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public event Action OnItemQuantityChangeEvent;

		public static PowerUpItemStore Instance
		{
			get
			{
				return PowerUpItemStore.instance;
			}
		}

		private void Awake()
		{
			PowerUpItemStore.instance = this;
			SaveDefaultData();
			Load();
		}

		private void SaveDefaultData()
		{
			if (!File.Exists(Application.persistentDataPath + PowerUpItemStore.DB_NAME))
			{
				FileStream fileStream = File.Create(Application.persistentDataPath + PowerUpItemStore.DB_NAME);
				BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
				binaryFormatter.Serialize(fileStream, new PowerUpItemRecord
				{
					itemQuantity = new int[9]
				});
				fileStream.Close();
			}
		}

		public void Save(int powerUpItemID, int quantity)
		{
			SetEncodedItemQuantity(powerUpItemID, quantity);
			SaveAll();
		}

		public void SaveAll()
		{
			FileStream fileStream = File.Create(Application.persistentDataPath + PowerUpItemStore.DB_NAME);
			BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
			binaryFormatter.Serialize(fileStream, new PowerUpItemRecord
			{
				itemQuantity = GetDecodedItemQuantities()
			});
			fileStream.Close();
		}

		private void Load()
		{
			string path = Application.persistentDataPath + PowerUpItemStore.DB_NAME;
			PowerUpItemRecord powerUpItemData;
			try
			{
				using (FileStream fileStream = File.Open(path, FileMode.Open))
				{
					BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
					powerUpItemData = (PowerUpItemRecord)binaryFormatter.Deserialize(fileStream);
				}
			}
			catch (Exception e)
			{
				UnityEngine.Debug.LogError("PowerUpItemStore: failed to load save, regenerating default. " + e.Message);
				if (File.Exists(path))
				{
					File.Delete(path);
				}
				SaveDefaultData();
				powerUpItemData = new PowerUpItemRecord
				{
					itemQuantity = new int[9]
				};
			}
			encodedItemQuantity = new ObscuredInt[9];
			for (int i = 0; i < totalPowerupItem; i++)
			{
				if (i < powerUpItemData.itemQuantity.Length)
				{
					SetEncodedItemQuantity(i, powerUpItemData.itemQuantity[i]);
				}
				else
				{
					SetEncodedItemQuantity(i, 0);
				}
			}
		}

		public int GetCurrentItemQuantity(int itemID)
		{
			return GetDecodedItemQuanity(itemID);
		}

		public void ChangeItemQuantity(int itemID, int addedQuantity)
		{
			Save(itemID, GetDecodedItemQuanity(itemID) + addedQuantity);
			if (OnItemQuantityChangeEvent != null)
			{
				OnItemQuantityChangeEvent();
			}
		}

		public void RestoreDataFromCloud(PlayerRecord_PowerupItem restoredData)
		{
			encodedItemQuantity = new ObscuredInt[]
			{
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0
			};
			for (int i = 0; i < totalPowerupItem; i++)
			{
				if (i < restoredData.listDataPowerupItems.Count)
				{
					SetEncodedItemQuantity(i, restoredData.listDataPowerupItems[i].quantity);
				}
				else
				{
					SetEncodedItemQuantity(i, 0);
				}
			}
			SaveAll();
		}

		private void SetEncodedItemQuantity(int itemId, int quantity)
		{
			encodedItemQuantity[itemId] = quantity + GameKit.deltaValue;
		}

		private int GetDecodedItemQuanity(int itemId)
		{
			return encodedItemQuantity[itemId] - GameKit.deltaValue;
		}

		private int[] GetDecodedItemQuantities()
		{
			int[] array = new int[encodedItemQuantity.Length];
			for (int i = encodedItemQuantity.Length - 1; i >= 0; i--)
			{
				array[i] = GetDecodedItemQuanity(i);
			}
			return array;
		}
	}
}
