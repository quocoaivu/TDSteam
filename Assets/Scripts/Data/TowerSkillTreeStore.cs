using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Data
{
	// Persistent record of which tower skill-tree nodes the player has permanently unlocked,
	// keyed by tower id. Cost/prerequisite checks live in the buy flow (caller); this store only
	// remembers unlock state. Mirrors GlobalUpgradeStore's binary-save approach, as a lazy singleton.
	public class TowerSkillTreeStore
	{
		public static TowerSkillTreeStore Instance
		{
			get
			{
				if (TowerSkillTreeStore.instance == null)
				{
					TowerSkillTreeStore.instance = new TowerSkillTreeStore();
					TowerSkillTreeStore.instance.Load();
				}
				return TowerSkillTreeStore.instance;
			}
		}

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action OnTreeChangeEvent;

		public bool IsNodeUnlocked(int towerID, int nodeID)
		{
			List<int> list;
			return data.unlockedNodes.TryGetValue(towerID, out list) && list.Contains(nodeID);
		}

		// Returns a copy so callers can't mutate the stored list directly.
		public List<int> GetUnlockedNodes(int towerID)
		{
			List<int> list;
			if (data.unlockedNodes.TryGetValue(towerID, out list))
			{
				return new List<int>(list);
			}
			return new List<int>();
		}

		// Records a node as unlocked (no-op if already unlocked) and persists.
		public void UnlockNode(int towerID, int nodeID)
		{
			List<int> list;
			if (!data.unlockedNodes.TryGetValue(towerID, out list))
			{
				list = new List<int>();
				data.unlockedNodes[towerID] = list;
			}
			if (list.Contains(nodeID))
			{
				return;
			}
			list.Add(nodeID);
			Save();
			DispatchChangeEvent();
		}

		private void Load()
		{
			string path = Application.persistentDataPath + TowerSkillTreeStore.DB_NAME;
			if (!File.Exists(path))
			{
				SaveDefaultData();
				return;
			}
			try
			{
				using (FileStream fileStream = File.Open(path, FileMode.Open))
				{
					BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
					data = (TowerSkillTreeData)binaryFormatter.Deserialize(fileStream);
				}
				if (data.unlockedNodes == null)
				{
					data.unlockedNodes = new Dictionary<int, List<int>>();
				}
			}
			catch (Exception e)
			{
				UnityEngine.Debug.LogError("TowerSkillTreeStore: failed to load save, regenerating default. " + e.Message);
				if (File.Exists(path))
				{
					File.Delete(path);
				}
				SaveDefaultData();
			}
		}

		private void SaveDefaultData()
		{
			data = new TowerSkillTreeData();
			Save();
		}

		private void Save()
		{
			FileStream fileStream = File.Create(Application.persistentDataPath + TowerSkillTreeStore.DB_NAME);
			BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
			binaryFormatter.Serialize(fileStream, data);
			fileStream.Close();
		}

		private void DispatchChangeEvent()
		{
			if (OnTreeChangeEvent != null)
			{
				OnTreeChangeEvent();
			}
		}

		private static string DB_NAME = "/towerSkillTreeInfor.dat";

		private TowerSkillTreeData data = new TowerSkillTreeData();

		private static TowerSkillTreeStore instance;

		[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			instance = null;
		}

		[Serializable]
		public class TowerSkillTreeData
		{
			public Dictionary<int, List<int>> unlockedNodes = new Dictionary<int, List<int>>();
		}
	}
}
