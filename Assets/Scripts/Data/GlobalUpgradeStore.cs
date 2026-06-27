using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using GameCore;
using UnityEngine;

namespace Data
{
	public class GlobalUpgradeStore : BaseMonoBehaviour
	{
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action OnStarChangeEvent;

		public static GlobalUpgradeStore Instance
		{
			get
			{
				return GlobalUpgradeStore.instance;
			}
		}

		private void Awake()
		{
			GlobalUpgradeStore.instance = this;
			ReadParameterUpgrade();
			ReadDataStarUpgradeRequire();
			SaveDefaultData();
			LoadData();
		}

		public void OnStarChange(bool isDispatchEventChange)
		{
			if (isDispatchEventChange && OnStarChangeEvent != null)
			{
				OnStarChangeEvent();
			}
		}

		private void ReadParameterUpgrade()
		{
			string file = "Parameters/global_upgrade_parameter";
			List<Dictionary<string, object>> list = CSVLoader.Read(file);
			for (int i = 0; i < list.Count; i++)
			{
				List<int> list2 = new List<int>();
				list2.Add((int)list[i]["tower_0"]);
				list2.Add((int)list[i]["tower_1"]);
				list2.Add((int)list[i]["tower_2"]);
				list2.Add((int)list[i]["tower_3"]);
				tierParameters.Add(new TierOptionSpec(list2));
			}
		}

		private void ReadDataStarUpgradeRequire()
		{
			string file = "Parameters/global_upgrade_star_require";
			List<Dictionary<string, object>> list = CSVLoader.Read(file);
			for (int i = 0; i < list.Count; i++)
			{
				int num = (int)list[i]["tower_id"];
				List<int> list2 = new List<int>();
				list2.Add((int)list[i]["star_tier_0"]);
				list2.Add((int)list[i]["star_tier_1"]);
				list2.Add((int)list[i]["star_tier_2"]);
				list2.Add((int)list[i]["star_tier_3"]);
				list2.Add((int)list[i]["star_tier_4"]);
				starParameters.Add(new StarEnhanceSpec(list2));
			}
		}

		public int GetStarRequireForUpgrade(int towerID, int tier)
		{
			int result;
			if (towerID < 0)
			{
				result = -1;
			}
			else
			{
				if (tier < 0)
				{
					result = starParameters[towerID].Value[tier + 1];
				}
				else
				{
					result = starParameters[towerID].Value[tier];
				}
				if (tier > 4)
				{
					result = 0;
				}
			}
			return result;
		}

		public int GetUpgradeValue(int upgradeID, int towerID)
		{
			return tierParameters[upgradeID].Value[towerID];
		}

		// One default upgrade entry per tower id (currentUpgradeLevel -1 = not upgraded). Keep this in sync
		// with the tower count in tower_parameter.txt so new towers (e.g. the Supporter, id 4) get an entry.
		private const int TOWER_COUNT = 5;

		private void SaveDefaultData()
		{
			if (!File.Exists(Application.persistentDataPath + GlobalUpgradeStore.DB_NAME))
			{
				FileStream fileStream = File.Create(Application.persistentDataPath + GlobalUpgradeStore.DB_NAME);
				BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
				data.ListUpgradeData = new Dictionary<int, GlobalUpgradeStore.UpgradeData>();
				for (int towerID = 0; towerID < TOWER_COUNT; towerID++)
				{
					GlobalUpgradeStore.UpgradeData upgradeData = new GlobalUpgradeStore.UpgradeData();
					upgradeData.towerID = towerID;
					upgradeData.currentUpgradeLevel = -1;
					data.ListUpgradeData.Add(towerID, upgradeData);
				}
				binaryFormatter.Serialize(fileStream, data);
				fileStream.Close();
			}
		}

		public void Save(int _towerID, int _currentUpgradeLevel)
		{
			FileStream fileStream = File.Create(Application.persistentDataPath + GlobalUpgradeStore.DB_NAME);
			BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
			data.ListUpgradeData[_towerID].currentUpgradeLevel = _currentUpgradeLevel;
			binaryFormatter.Serialize(fileStream, data);
			fileStream.Close();
		}

		public void SaveAll()
		{
			FileStream fileStream = File.Create(Application.persistentDataPath + GlobalUpgradeStore.DB_NAME);
			BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
			binaryFormatter.Serialize(fileStream, data);
			fileStream.Close();
		}

		private void LoadData()
		{
			string path = Application.persistentDataPath + GlobalUpgradeStore.DB_NAME;
			try
			{
				using (FileStream fileStream = File.Open(path, FileMode.Open))
				{
					BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
					binaryFormatter.Binder = new LegacyTypeBinder();
					data = (GlobalUpgradeStore.UpgradeSerializeData)binaryFormatter.Deserialize(fileStream);
				}
			}
			catch (Exception e)
			{
				// Old saves were serialized under the previous class name (ReadWriteDataGlobalUpgrade);
				// the binder remaps them. If that still fails (genuine corruption), regenerate defaults.
				UnityEngine.Debug.LogError("GlobalUpgradeStore: failed to load save, regenerating default. " + e.Message);
				if (File.Exists(path))
				{
					File.Delete(path);
				}
				SaveDefaultData();
			}
		}

		public int GetCurrentUpgradeLevel(int _towerID)
		{
			LoadData();
			// Towers added after a save was written (e.g. the Supporter) have no upgrade entry yet.
			// Treat a missing tower as "not upgraded" (-1) instead of throwing KeyNotFoundException,
			// which would otherwise abort TowerDataLoader.Awake and skip every later loader (items, skills).
			if (data.ListUpgradeData.TryGetValue(_towerID, out UpgradeData upgrade))
			{
				return upgrade.currentUpgradeLevel;
			}
			return -1;
		}

		public void RestoreDataFromCloud(PlayerRecord_GlobalEnhance restoredData)
		{
			data.ListUpgradeData = new Dictionary<int, GlobalUpgradeStore.UpgradeData>();
			foreach (PlayerRecord_GlobalEnhance_Unique userData_GlobalUpgrade_Unique in restoredData.listUpgradedTower)
			{
				GlobalUpgradeStore.UpgradeData upgradeData = new GlobalUpgradeStore.UpgradeData();
				upgradeData.towerID = userData_GlobalUpgrade_Unique.towerID;
				upgradeData.currentUpgradeLevel = userData_GlobalUpgrade_Unique.towerUpgradedLevel;
				data.ListUpgradeData.Add(upgradeData.towerID, upgradeData);
			}
			UnityEngine.Debug.Log(data);
			SaveAll();
		}

		private List<TierOptionSpec> tierParameters = new List<TierOptionSpec>();

		private List<StarEnhanceSpec> starParameters = new List<StarEnhanceSpec>();

		private static GlobalUpgradeStore instance;

		[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			instance = null;
		}
		private static string DB_NAME = "/upgradeInfor.dat";

		private GlobalUpgradeStore.UpgradeSerializeData data = new GlobalUpgradeStore.UpgradeSerializeData();

		[Serializable]
		public class UpgradeData
		{
			public int towerID;

			public int currentUpgradeLevel;
		}

		[Serializable]
		public class UpgradeSerializeData
		{
			public Dictionary<int, GlobalUpgradeStore.UpgradeData> ListUpgradeData
			{
				get
				{
					return listUpgradeData;
				}
				set
				{
					listUpgradeData = value;
				}
			}

			private Dictionary<int, GlobalUpgradeStore.UpgradeData> listUpgradeData;
		}

		// Remaps pre-rename type names (Data.ReadWriteDataGlobalUpgrade+...) stored in old save files
		// so existing saves still deserialize into the renamed nested types.
		private sealed class LegacyTypeBinder : SerializationBinder
		{
			public override Type BindToType(string assemblyName, string typeName)
			{
				typeName = typeName.Replace("ReadWriteDataGlobalUpgrade", "GlobalUpgradeStore");
				return Type.GetType(string.Format("{0}, {1}", typeName, assemblyName));
			}
		}
	}
}
