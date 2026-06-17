using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Data
{
	// Persistent meta currency spent on permanent tower skill-tree nodes between matches.
	// This is distinct from the per-hero "skill points" in HeroStore. Mirrors PlayerCurrencyStore's
	// binary-save approach, but as a lazy singleton so it needs no scene wiring (like TowerParameterManager).
	public class TowerSkillPointStore
	{
		public static TowerSkillPointStore Instance
		{
			get
			{
				if (TowerSkillPointStore.instance == null)
				{
					TowerSkillPointStore.instance = new TowerSkillPointStore();
					TowerSkillPointStore.instance.Load();
				}
				return TowerSkillPointStore.instance;
			}
		}

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action OnSkillPointChangeEvent;

		public int GetCurrentSkillPoint()
		{
			return data.totalSkillPoint;
		}

		// Adds skill points (e.g. earned after finishing a match) and persists.
		public void AddSkillPoint(int amount, bool isDispatchEventChange)
		{
			if (amount <= 0)
			{
				return;
			}
			data.totalSkillPoint += amount;
			Save();
			DispatchChangeEvent(isDispatchEventChange);
		}

		// Spends `amount` only when affordable. Returns false and changes nothing if too poor.
		public bool TrySpend(int amount)
		{
			if (amount < 0 || data.totalSkillPoint < amount)
			{
				return false;
			}
			data.totalSkillPoint -= amount;
			Save();
			DispatchChangeEvent(true);
			return true;
		}

		private void Load()
		{
			string path = Application.persistentDataPath + TowerSkillPointStore.DB_NAME;
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
					data = (TowerSkillPointData)binaryFormatter.Deserialize(fileStream);
				}
			}
			catch (Exception e)
			{
				UnityEngine.Debug.LogError("TowerSkillPointStore: failed to load save, regenerating default. " + e.Message);
				if (File.Exists(path))
				{
					File.Delete(path);
				}
				SaveDefaultData();
			}
		}

		private void SaveDefaultData()
		{
			data = new TowerSkillPointData();
			Save();
		}

		private void Save()
		{
			FileStream fileStream = File.Create(Application.persistentDataPath + TowerSkillPointStore.DB_NAME);
			BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
			binaryFormatter.Serialize(fileStream, data);
			fileStream.Close();
		}

		private void DispatchChangeEvent(bool isDispatchEventChange)
		{
			if (isDispatchEventChange && OnSkillPointChangeEvent != null)
			{
				OnSkillPointChangeEvent();
			}
		}

		private static string DB_NAME = "/towerSkillPointInfor.dat";

		private TowerSkillPointData data = new TowerSkillPointData();

		private static TowerSkillPointStore instance;

		[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			instance = null;
		}

		[Serializable]
		public class TowerSkillPointData
		{
			public int totalSkillPoint;
		}
	}
}
