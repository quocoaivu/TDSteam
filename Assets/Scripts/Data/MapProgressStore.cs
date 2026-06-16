using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using MetaGame;
using UnityEngine;

namespace Data
{
	public class MapProgressStore : MonoBehaviour
	{
        private static string DB_NAME = "/mapInfor.dat";

        private ZoneSerializeRecord data = new ZoneSerializeRecord();

        private static int mapIDMax = 100;

        private static int currentMapIDMax = 17;

        private static int modeResultMax = 3;

        private static MapProgressStore instance;
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            instance = null;
        }

        public static MapProgressStore Instance
		{
			get
			{
				return MapProgressStore.instance;
			}
		}

		private void Awake()
		{
			MapProgressStore.instance = this;
			SaveDefaultData();
			Load();
			UpdateDataVersionFrom105To106();
		}

		private void UpdateDataVersionFrom105To106()
		{
			string key = "UpdateDataVersionFrom105To106";
			if (PlayerPrefs.GetInt(key, 0) == 0 && data.ListMapsData.Count <= 12)
			{
				data.mapIDUnlocked = 12;
				for (int i = 12; i < 18; i++)
				{
					ZoneRecord value = new ZoneRecord();
					data.ListMapsData.Add(i, value);
				}
				SaveAll();
				PlayerPrefs.SetInt(key, 1);
			}
		}

		private void SaveDefaultData()
		{
			if (!File.Exists(Application.persistentDataPath + MapProgressStore.DB_NAME))
			{
				FileStream fileStream = File.Create(Application.persistentDataPath + MapProgressStore.DB_NAME);
				BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
				data.mapIDUnlocked = 0;
				data.lastMapIDPlayed = 0;
				data.ListMapsData = new Dictionary<int, ZoneRecord>();
				for (int i = 0; i < MapProgressStore.mapIDMax + 1; i++)
				{
					ZoneRecord value = new ZoneRecord();
					data.ListMapsData.Add(i, value);
				}
				binaryFormatter.Serialize(fileStream, data);
				fileStream.Close();
			}
		}

		public void SaveAll()
		{
			FileStream fileStream = File.Create(Application.persistentDataPath + MapProgressStore.DB_NAME);
			BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
			binaryFormatter.Serialize(fileStream, data);
			fileStream.Close();
		}

		public void SaveLastMapPlayed(int mapIDPlayed)
		{
			data.lastMapIDPlayed = mapIDPlayed;
			SaveAll();
		}

		public void SaveLastMapModeChoose(int mapModeChoose)
		{
			data.lastMapModeChoose = mapModeChoose;
			SaveAll();
		}

		private void SaveMapModeResult(int _mapID, int result)
		{
			GetMapDataByID(_mapID).modePassed = result;
			SaveAll();
		}

		private void Load()
		{
			string path = Application.persistentDataPath + MapProgressStore.DB_NAME;
			try
			{
				using (FileStream fileStream = File.Open(path, FileMode.Open))
				{
					BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
					data = (ZoneSerializeRecord)binaryFormatter.Deserialize(fileStream);
				}
			}
			catch (Exception e)
			{
				// Corrupted / incompatible save: drop it and regenerate defaults instead of crashing Awake.
				UnityEngine.Debug.LogError("MapProgressStore: failed to load map save, regenerating default. " + e.Message);
				if (File.Exists(path))
				{
					File.Delete(path);
				}
				SaveDefaultData();
			}
		}

		private ZoneRecord GetMapDataByID(int _mapID)
		{
			ZoneRecord result = new ZoneRecord();
			if (data.ListMapsData.ContainsKey(_mapID))
			{
				result = data.ListMapsData[_mapID];
			}
			return result;
		}

		public int GetCurrentPlayCount(int _mapID)
		{
			return GetMapDataByID(_mapID).playCount;
		}

		public void IncreaseMapPlaycount(int _mapID)
		{
			GetMapDataByID(_mapID).playCount++;
			SaveAll();
		}

		public void IncreaseMapPlaycount(int _mapID, BattleStanding status)
		{
			if (status != BattleStanding.Victory)
			{
				if (status == BattleStanding.Defeat)
				{
					GetMapDataByID(_mapID).playCount_defeat++;
					SaveAll();
				}
			}
			else
			{
				GetMapDataByID(_mapID).playCount_victory++;
				SaveAll();
			}
		}

		public int GetCurrentPlayCount_Victory(int _mapID)
		{
			return GetMapDataByID(_mapID).playCount_victory;
		}

		public int GetCurrentPlayCount_Defeat(int _mapID)
		{
			return GetMapDataByID(_mapID).playCount_defeat;
		}

		public void SaveStarEarned(int _mapID, int _starEarned)
		{
			int starEarnedByMap = GetStarEarnedByMap(_mapID);
			if (_starEarned > starEarnedByMap)
			{
				GetMapDataByID(_mapID).starEarned = _starEarned;
				SaveAll();
			}
		}

		public int GetStarEarnedByMap(int _mapID)
		{
			return GetMapDataByID(_mapID).starEarned;
		}

		public int GetTotalStarEarned()
		{
			int num = 0;
			for (int i = 0; i <= MapProgressStore.currentMapIDMax; i++)
			{
				num += GetMapDataByID(i).starEarned;
			}
			return num;
		}

		public int GetMapIDPassed()
		{
			int result = -1;
			for (int i = 0; i < MapProgressStore.currentMapIDMax; i++)
			{
				if (GetStarEarnedByMap(i) > 0)
				{
					result = i;
				}
			}
			return result;
		}

		public int GetMapIDUnlocked()
		{
			int mapIDUnlocked;
			if (PlayerPrefs.GetInt("TESTunlockAllMap", 0) > 0)
			{
				mapIDUnlocked = MapProgressStore.currentMapIDMax;
			}
			else
			{
				mapIDUnlocked = data.mapIDUnlocked;
			}
			return mapIDUnlocked;
		}

		public void IncreaseMapIdUnlock(int currentMapID)
		{
			int mapIDUnlocked = GetMapIDUnlocked();
			if (currentMapID == mapIDUnlocked)
			{
				data.mapIDUnlocked = currentMapID + 1;
				SaveAll();
			}
		}

		public int GetTotalMap()
		{
			return MapProgressStore.currentMapIDMax + 1;
		}

		public int GetLastMapIDPlayed()
		{
			return data.lastMapIDPlayed;
		}

		public int GetMapModeResult(int mapID)
		{
			return GetMapDataByID(mapID).modePassed;
		}

		public void IncreaseModeResult(int mapID)
		{
			int num = (int)CrossSceneData.Instance.BattleDifficulty;
			num++;
			if (num > GetMapDataByID(mapID).modePassed)
			{
				num = Mathf.Clamp(num, 0, MapProgressStore.modeResultMax);
				SaveMapModeResult(mapID, num);
			}
		}

		public int GetLastMapModeChoose()
		{
			return data.lastMapModeChoose;
		}

		public void RestoreDataFromCloud(PlayerRecord_Zone restoredData)
		{
			data.mapIDUnlocked = restoredData.mapIDUnlocked;
			data.lastMapIDPlayed = restoredData.lastMapIDPlayed;
			data.lastMapModeChoose = restoredData.lastMapModeChoose;
			for (int i = 0; i < restoredData.listDataMap.Count; i++)
			{
				ZoneRecord mapData = new ZoneRecord();
				mapData.starEarned = restoredData.listDataMap[i].starEarned;
				mapData.playCount = restoredData.listDataMap[i].playCount;
				mapData.playCount_victory = restoredData.listDataMap[i].playCount_victory;
				mapData.playCount_defeat = restoredData.listDataMap[i].playCount_defeat;
				data.ListMapsData[restoredData.listDataMap[i].mapID] = mapData;
			}
			SaveAll();
		}
	}
}
