using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Data
{
	public class UserProfileStore : MonoBehaviour
	{
        private static string DB_NAME = "/userProfile.dat";

        private static string LEAGUE_PREFIX = "LEAGUE_";

        private PlayerDossierRecord data = new PlayerDossierRecord();

        private static UserProfileStore instance;
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            instance = null;
        }
        
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public event Action OnUserInforChangeEvent;

		public static UserProfileStore Instance
		{
			get
			{
				return UserProfileStore.instance;
			}
		}

		private void Awake()
		{
			UserProfileStore.instance = this;
			SaveDefaultData();
			Load();
		}

		private void Start()
		{
			UnityEngine.Debug.Log("+++subscribe league");
			GameSignalCenter.Instance.Subscribe(GameSignalKind.OnTournamentTierUp, new SimpleListenerRecord(GameKit.GetUniqueId(), new GameSignalCenter.SimpleSubscribeMethod(OnLeagueChange)));
		}

		private void SaveDefaultData()
		{
			if (!File.Exists(Application.persistentDataPath + UserProfileStore.DB_NAME))
			{
				FileStream fileStream = File.Create(Application.persistentDataPath + UserProfileStore.DB_NAME);
				BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
				data.userID = "empty";
				data.userName = "Nameless hero";
				data.renameCount = 0;
				data.renameItemQuantity = 1;
				data.userCountryCode = "gb";
				data.league = 0;
				data.lastTimeBackup = DateTime.Now.ToString();
				binaryFormatter.Serialize(fileStream, data);
				fileStream.Close();
			}
		}

		public void SaveAll()
		{
			FileStream fileStream = File.Create(Application.persistentDataPath + UserProfileStore.DB_NAME);
			BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
			binaryFormatter.Serialize(fileStream, data);
			fileStream.Close();
		}

		private void Load()
		{
			string path = Application.persistentDataPath + UserProfileStore.DB_NAME;
			try
			{
				using (FileStream fileStream = File.Open(path, FileMode.Open))
				{
					BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
					data = (PlayerDossierRecord)binaryFormatter.Deserialize(fileStream);
				}
			}
			catch (Exception e)
			{
				UnityEngine.Debug.LogError("UserProfileStore: failed to load save, regenerating default. " + e.Message);
				if (File.Exists(path))
				{
					File.Delete(path);
				}
				SaveDefaultData();
			}
		}

		public void ClearSaveButton()
		{
			UnityEngine.Debug.Log("tested clear data");
			print(Application.persistentDataPath);
		}

		public void SetUserID(string value)
		{
			data.userID = value;
			SaveAll();
			if (OnUserInforChangeEvent != null)
			{
				OnUserInforChangeEvent();
			}
		}

		public void ClearUserID()
		{
			SetUserID(string.Empty);
		}

		public string GetUserID()
		{
			Load();
			return data.userID;
		}

		public void SaveLastTimeBackup()
		{
			data.lastTimeBackup = DateTime.Now.ToString();
			SaveAll();
		}

		public string GetLastTimeBackup()
		{
			Load();
			return data.lastTimeBackup;
		}

		private void OnLeagueChange()
		{
			int leagueValue = GetLeagueValue();
			int curtier = GameKit.tourUserSelfInfo.curtier;
			UnityEngine.Debug.LogFormat("_____read write data, curLeague from server {0}, league from user info {1}", new object[]
			{
				leagueValue,
				curtier
			});
			if (curtier != leagueValue)
			{
				SetLeague(curtier);
			}
		}

		public void SetLeague(int value)
		{
			data.league = value;
			SaveAll();
		}

		public int GetLeagueValue()
		{
			Load();
			return data.league;
		}

		public string GetLeagueName()
		{
			Load();
			string result = string.Empty;
			if (data.league < 0)
			{
				result = string.Empty;
			}
			else
			{
				string key = UserProfileStore.LEAGUE_PREFIX + data.league;
				result = GameKit.GetLocalization(key);
			}
			return result;
		}

		public void SetRegionCode(string value)
		{
			data.userCountryCode = value;
			SaveAll();
			if (OnUserInforChangeEvent != null)
			{
				OnUserInforChangeEvent();
			}
		}

		public string GetUserRegionCode()
		{
			Load();
			return data.userCountryCode;
		}

		public string GetUserName()
		{
			Load();
			UnityEngine.Debug.Log("Get user name " + data.userName);
			return data.userName;
		}

		public void SetUserName(string name)
		{
			data.userName = name;
			SaveAll();
			UnityEngine.Debug.Log("Set user name " + data.userName);
			if (OnUserInforChangeEvent != null)
			{
				OnUserInforChangeEvent();
			}
		}

		public int GetRenameCount()
		{
			Load();
			return data.renameCount;
		}

		public void IncreaseRenameCount()
		{
			Load();
			data.renameCount++;
			SaveAll();
		}

		public int GetRenameItemQuantity()
		{
			Load();
			return data.renameItemQuantity;
		}

		public void RenameItemQuantityChange(int value)
		{
			Load();
			data.renameItemQuantity += value;
			SaveAll();
		}

		public int GetRenameCost()
		{
			return 50;
		}

		public void RestoreDataFromCloud(PlayerRecord_PlayerDossier restoredData)
		{
			data.userID = restoredData.userID;
			data.userName = restoredData.userName;
			data.renameCount = restoredData.renameCount;
			data.renameItemQuantity = restoredData.renameItemQuantity;
			data.userCountryCode = restoredData.countryCode;
			data.league = restoredData.league;
			data.lastTimeBackup = restoredData.lastTimeBackup;
			UnityEngine.Debug.Log(data);
			SaveAll();
		}
	}
}
