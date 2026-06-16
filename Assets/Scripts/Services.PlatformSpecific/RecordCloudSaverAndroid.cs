using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
//using System.Threading.Tasks;
using Data;
//using Firebase;
//using Firebase.Database;
//using Firebase.Unity.Editor;
using LifetimePopup;
using GameCore;
using Newtonsoft.Json;
using Parameter;
using UnityEngine;

namespace Services.PlatformSpecific
{
	public class RecordCloudSaverAndroid : MonoBehaviour, IRecordCloudSaver
	{
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action OnDataBackupCompletedEvent;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action OnDataRestoreCompletedEvent;

		private void Update()
		{
			if (isDataCollected)
			{
				dataRestoreDeliver.DispatchToAllDataWriter(dataRestoreDeliver);
				base.StartCoroutine(BackToMainMenu());
				SendEvent_DataRestoreComplete();
				isDataCollected = false;
			}
			if (dataCollectFailedFlag)
			{
				string notiContent = Singleton<AlertSynopsis>.Instance.GetNotiContent(149);
				MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init(notiContent, false, false);
				dataCollectFailedFlag = false;
			}
		}

		private void SendEvent_DataBackupComplete()
		{
			if (OnDataBackupCompletedEvent != null)
			{
				OnDataBackupCompletedEvent();
			}
		}

		private void SendEvent_DataRestoreComplete()
		{
			if (OnDataRestoreCompletedEvent != null)
			{
				OnDataRestoreCompletedEvent();
			}
		}

		public void FirebaseInit()
		{
			//FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(DataCloudSaverConstants.FIREBASE_URL);
			//reference = FirebaseDatabase.DefaultInstance.RootReference;
		}

		public void AutoBackUpData()
		{
			if (GameUtils.IsInternetConnectionAvailable())
			{
				if (NativeSpecificServicesSource.Services.UserProfile.IsLoggedIn_Facebook() || NativeSpecificServicesSource.Services.UserProfile.IsLoggedIn_Google())
				{
					userID = NativeSpecificServicesSource.Services.UserProfile.GetFirebaseUserID();
					if (string.IsNullOrEmpty(userID))
					{
						UnityEngine.Debug.Log("Auto Backup - User ID cannot be empty!");
						return;
					}
					int mapIDUnlocked = MapProgressStore.Instance.GetMapIDUnlocked();
					if (mapIDUnlocked < 4)
					{
						UnityEngine.Debug.Log("Can mo khoa map id4 de su dung auto backup!");
						return;
					}
					BackupData_Hero();
					BackupData_GlobalUpgrade();
					BackupData_Map();
					BackupData_Theme();
					BackupData_UserProfile();
					BackupData_PowerUpItem();
					BackupData_Tutorial();
					BackupData_DailyTrial();
					BackupData_Offers();
					BackupData_FreeResources();
					BackupData_SaleBundle();
					BackupData_DailyReward();
					SendEvent_DataBackupComplete();
					UnityEngine.Debug.Log("Auto Backup - Done!");
				}
				else
				{
					UnityEngine.Debug.Log("Auto Backup - Failed - Require Login Auth!");
				}
			}
		}

		public void BackupData()
		{
			userID = NativeSpecificServicesSource.Services.UserProfile.GetFirebaseUserID();
			if (string.IsNullOrEmpty(userID))
			{
				UnityEngine.Debug.Log("Backup - User ID cannot be empty!");
				return;
			}
			MonoSingleton<LifespanSurface>.Instance.LoadingProgressPopupController.Open();
			BackupData_Hero();
			BackupData_GlobalUpgrade();
			BackupData_Map();
			BackupData_Theme();
			BackupData_UserProfile();
			BackupData_PowerUpItem();
			BackupData_Tutorial();
			BackupData_DailyTrial();
			BackupData_Offers();
			BackupData_FreeResources();
			BackupData_SaleBundle();
			BackupData_DailyReward();
			SendEvent_DataBackupComplete();
			MonoSingleton<LifespanSurface>.Instance.LoadingProgressPopupController.Close();
			string notiContent = Singleton<AlertSynopsis>.Instance.GetNotiContent(132);
			MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init(notiContent, false, false);
		}

		private void BackupData_Hero()
		{
			//PlayerRecord_Hero userData_Hero = new PlayerRecord_Hero();
			//userData_Hero.listHeroData = new List<PlayerRecord_Hero_Unique>();
			//List<int> listHeroIDOwned = HeroStore.Instance.GetListHeroIDOwned();
			//for (int i = 0; i < listHeroIDOwned.Count; i++)
			//{
			//	PlayerRecord_Hero_Unique userData_Hero_Unique = new PlayerRecord_Hero_Unique();
			//	userData_Hero_Unique.id = listHeroIDOwned[i];
			//	userData_Hero_Unique.level = HeroStore.Instance.GetCurrentHeroLevel(listHeroIDOwned[i]);
			//	userData_Hero_Unique.exp = HeroStore.Instance.GetCurrentHeroTotalExp(listHeroIDOwned[i]);
			//	userData_Hero_Unique.isOwned = true;
			//	userData_Hero_Unique.ownedPet = HeroStore.Instance.IsPetUnlocked(listHeroIDOwned[i]);
			//	userData_Hero_Unique.skillUpgraded = new List<int>();
			//	for (int j = 0; j < 4; j++)
			//	{
			//		userData_Hero_Unique.skillUpgraded.Add(HeroStore.Instance.GetSkillPoint(listHeroIDOwned[i], j));
			//	}
			//	userData_Hero.listHeroData.Add(userData_Hero_Unique);
			//}
			//string rawJsonValueAsync = JsonConvert.SerializeObject(userData_Hero);
			//reference.Child(userID).Child(DataCloudSaverConstants.FIREBASE_NODE_USERDATA_HERO).SetRawJsonValueAsync(rawJsonValueAsync);
		}

		private void BackupData_GlobalUpgrade()
		{
			//PlayerRecord_GlobalEnhance userData_GlobalUpgrade = new PlayerRecord_GlobalEnhance();
			//userData_GlobalUpgrade.listUpgradedTower = new List<PlayerRecord_GlobalEnhance_Unique>();
			//for (int i = 0; i < 4; i++)
			//{
			//	PlayerRecord_GlobalEnhance_Unique userData_GlobalUpgrade_Unique = new PlayerRecord_GlobalEnhance_Unique();
			//	userData_GlobalUpgrade_Unique.towerID = i;
			//	userData_GlobalUpgrade_Unique.towerUpgradedLevel = GlobalUpgradeStore.Instance.GetCurrentUpgradeLevel(i);
			//	userData_GlobalUpgrade.listUpgradedTower.Add(userData_GlobalUpgrade_Unique);
			//}
			//string rawJsonValueAsync = JsonConvert.SerializeObject(userData_GlobalUpgrade);
			//reference.Child(userID).Child(DataCloudSaverConstants.FIREBASE_NODE_USERDATA_GLOBALUPGRADE).SetRawJsonValueAsync(rawJsonValueAsync);
		}

		private void BackupData_Map()
		{
			//PlayerRecord_Zone userData_Map = new PlayerRecord_Zone();
			//userData_Map.mapIDUnlocked = MapProgressStore.Instance.GetMapIDUnlocked();
			//userData_Map.lastMapIDPlayed = MapProgressStore.Instance.GetLastMapIDPlayed();
			//userData_Map.lastMapModeChoose = MapProgressStore.Instance.GetLastMapModeChoose();
			//userData_Map.listDataMap = new List<PlayerRecord_Zone_Unique>();
			//for (int i = 0; i <= userData_Map.mapIDUnlocked; i++)
			//{
			//	PlayerRecord_Zone_Unique userData_Map_Unique = new PlayerRecord_Zone_Unique();
			//	userData_Map_Unique.mapID = i;
			//	userData_Map_Unique.starEarned = MapProgressStore.Instance.GetStarEarnedByMap(i);
			//	userData_Map_Unique.playCount = MapProgressStore.Instance.GetCurrentPlayCount(i);
			//	userData_Map_Unique.playCount_victory = MapProgressStore.Instance.GetCurrentPlayCount_Victory(i);
			//	userData_Map_Unique.playCount_defeat = MapProgressStore.Instance.GetCurrentPlayCount_Defeat(i);
			//	userData_Map.listDataMap.Add(userData_Map_Unique);
			//}
			//string rawJsonValueAsync = JsonConvert.SerializeObject(userData_Map);
			//reference.Child(userID).Child(DataCloudSaverConstants.FIREBASE_NODE_USERDATA_MAP).SetRawJsonValueAsync(rawJsonValueAsync);
		}

		private void BackupData_Theme()
		{
			//PlayerRecord_Skin userData_Theme = new PlayerRecord_Skin();
			//userData_Theme.lastThemeIDPlayed = ThemeStore.Instance.GetLastThemeIDPlayed();
			//userData_Theme.listThemeIDUnlocked = new List<int>();
			//int themeIDUnlocked = ThemeStore.Instance.GetThemeIDUnlocked();
			//for (int i = 0; i <= themeIDUnlocked; i++)
			//{
			//	userData_Theme.listThemeIDUnlocked.Add(i);
			//}
			//string rawJsonValueAsync = JsonConvert.SerializeObject(userData_Theme);
			//reference.Child(userID).Child(DataCloudSaverConstants.FIREBASE_NODE_USERDATA_THEME).SetRawJsonValueAsync(rawJsonValueAsync);
		}

		private void BackupData_UserProfile()
		{
			//string rawJsonValueAsync = JsonConvert.SerializeObject(new PlayerRecord_PlayerDossier
			//{
			//	userID = UserProfileStore.Instance.GetUserID(),
			//	userName = UserProfileStore.Instance.GetUserName(),
			//	renameCount = UserProfileStore.Instance.GetRenameCount(),
			//	renameItemQuantity = UserProfileStore.Instance.GetRenameItemQuantity(),
			//	rank = "empty",
			//	league = UserProfileStore.Instance.GetLeagueValue(),
			//	exp = 0,
			//	countryCode = UserProfileStore.Instance.GetUserRegionCode(),
			//	totalGem = PlayerCurrencyStore.Instance.GetCurrentGem(),
			//	lastTimeBackup = UserProfileStore.Instance.GetLastTimeBackup()
			//});
			//reference.Child(userID).Child(DataCloudSaverConstants.FIREBASE_NODE_USERDATA_USERPROFILE).SetRawJsonValueAsync(rawJsonValueAsync);
		}

		private void BackupData_PowerUpItem()
		{
			//PlayerRecord_PowerupItem userData_PowerupItem = new PlayerRecord_PowerupItem();
			//userData_PowerupItem.listDataPowerupItems = new List<PlayerRecord_PowerupItem_Unique>();
			//for (int i = 0; i < 9; i++)
			//{
			//	PlayerRecord_PowerupItem_Unique userData_PowerupItem_Unique = new PlayerRecord_PowerupItem_Unique();
			//	userData_PowerupItem_Unique.itemID = i;
			//	userData_PowerupItem_Unique.quantity = PowerUpItemStore.Instance.GetCurrentItemQuantity(i);
			//	userData_PowerupItem.listDataPowerupItems.Add(userData_PowerupItem_Unique);
			//}
			//string rawJsonValueAsync = JsonConvert.SerializeObject(userData_PowerupItem);
			//reference.Child(userID).Child(DataCloudSaverConstants.FIREBASE_NODE_USERDATA_POWERUPITEM).SetRawJsonValueAsync(rawJsonValueAsync);
		}

		private void BackupData_Tutorial()
		{
			//PlayerRecord_Tutorial userData_Tutorial = new PlayerRecord_Tutorial();
			//userData_Tutorial.ListTutorialData = new Dictionary<string, bool>();
			//string[] allTutorialKeys = TutorialStore.Instance.AllTutorialKeys;
			//foreach (string text in allTutorialKeys)
			//{
			//	bool tutorialStatus = TutorialStore.Instance.GetTutorialStatus(text);
			//	if (!userData_Tutorial.ListTutorialData.ContainsKey(text))
			//	{
			//		userData_Tutorial.ListTutorialData.Add(text, tutorialStatus);
			//	}
			//}
			//string rawJsonValueAsync = JsonConvert.SerializeObject(userData_Tutorial);
			//reference.Child(userID).Child(DataCloudSaverConstants.FIREBASE_NODE_USERDATA_TUTORIAL).SetRawJsonValueAsync(rawJsonValueAsync);
		}

		private void BackupData_Offers()
		{
			//PlayerRecord_Deal userData_Offer = new PlayerRecord_Deal();
			//userData_Offer.ListOfferData = new Dictionary<string, bool>();
			//string[] allOfferKeys = OffersStore.Instance.AllOfferKeys;
			//foreach (string text in allOfferKeys)
			//{
			//	bool value = OffersStore.Instance.IsOfferProcessed(text);
			//	if (!userData_Offer.ListOfferData.ContainsKey(text))
			//	{
			//		userData_Offer.ListOfferData.Add(text, value);
			//	}
			//}
			//string rawJsonValueAsync = JsonConvert.SerializeObject(userData_Offer);
			//reference.Child(userID).Child(DataCloudSaverConstants.FIREBASE_NODE_USERDATA_OFFER).SetRawJsonValueAsync(rawJsonValueAsync);
		}

		private void BackupData_FreeResources()
		{
			//string rawJsonValueAsync = JsonConvert.SerializeObject(new PlayerRecord_FreeResources
			//{
			//	isUserGetRewardLoggedInFacebook = FreeResourcesStore.Instance.IsUserGetReward_LogInFacebook(),
			//	isUserGetRewardLikeFanpage = FreeResourcesStore.Instance.IsUserGetReward_LikeFanpage(),
			//	isUserGetRewardJoinGroup = FreeResourcesStore.Instance.IsUserGetReward_JoinGroup(),
			//	currentSharePerDay = FreeResourcesStore.Instance.GetCurrentSharePerDay(),
			//	currentWatchAdsPerDay = FreeResourcesStore.Instance.GetCurrentWatchAdsPerDay(),
			//	currentGemCollectedByInvite = FreeResourcesStore.Instance.GetCurrentGemCollectedByInvite()
			//});
			//reference.Child(userID).Child(DataCloudSaverConstants.FIREBASE_NODE_USERDATA_FREERESOURCES).SetRawJsonValueAsync(rawJsonValueAsync);
		}

		private void BackupData_DailyTrial()
		{
			//PlayerRecord_DailyOrdeal userData_DailyTrial = new PlayerRecord_DailyOrdeal();
			//userData_DailyTrial.currentDay = DailyTrialStore.Instance.GetCurrentDayIndex();
			//userData_DailyTrial.listDataDailyTrial = new List<PlayerRecord_DailyOrdeal_Unique>();
			//for (int i = 0; i <= 6; i++)
			//{
			//	PlayerRecord_DailyOrdeal_Unique userData_DailyTrial_Unique = new PlayerRecord_DailyOrdeal_Unique();
			//	userData_DailyTrial_Unique.day = i;
			//	userData_DailyTrial_Unique.missionDoneTier = DailyTrialStore.Instance.GetDoneMissionTier(i);
			//	userData_DailyTrial_Unique.playCount = DailyTrialStore.Instance.GetPlayCount(i);
			//	userData_DailyTrial.listDataDailyTrial.Add(userData_DailyTrial_Unique);
			//}
			//string rawJsonValueAsync = JsonConvert.SerializeObject(userData_DailyTrial);
			//reference.Child(userID).Child(DataCloudSaverConstants.FIREBASE_NODE_USERDATA_DAILYTRIAL).SetRawJsonValueAsync(rawJsonValueAsync);
		}

		private void BackupData_SaleBundle()
		{
			//PlayerRecord_SalePack userData_SaleBundle = new PlayerRecord_SalePack();
			//userData_SaleBundle.ListSaleBundleData = new List<PlayerRecord_SalePack_Unique>();
			//string[] productIDSpecialPack = MarketingSetup.productIDSpecialPack;
			//for (int i = 0; i < productIDSpecialPack.Length; i++)
			//{
			//	PlayerRecord_SalePack_Unique userData_SaleBundle_Unique = new PlayerRecord_SalePack_Unique();
			//	userData_SaleBundle_Unique.bundleID = productIDSpecialPack[i];
			//	userData_SaleBundle_Unique.isBought = SaleBundleStore.Instance.GetSpecialPackBuyStatus(userData_SaleBundle_Unique.bundleID);
			//	userData_SaleBundle_Unique.isExpired = SaleBundleStore.Instance.GetSpecialPackExpireStatus(userData_SaleBundle_Unique.bundleID);
			//	userData_SaleBundle.ListSaleBundleData.Add(userData_SaleBundle_Unique);
			//}
			//string rawJsonValueAsync = JsonConvert.SerializeObject(userData_SaleBundle);
			//reference.Child(userID).Child(DataCloudSaverConstants.FIREBASE_NODE_USERDATA_SALEBUNDLE).SetRawJsonValueAsync(rawJsonValueAsync);
		}

		private void BackupData_DailyReward()
		{
			//PlayerRecord_DailyPrize userData_DailyReward = new PlayerRecord_DailyPrize();
			//userData_DailyReward.listDailyRewardData = new List<PlayerRecord_DailyPrize_Unique>();
			//userData_DailyReward.currentDay = DailyRewardStore.Instance.GetCurrentDay();
			//for (int i = 0; i <= 13; i++)
			//{
			//	PlayerRecord_DailyPrize_Unique userData_DailyReward_Unique = new PlayerRecord_DailyPrize_Unique();
			//	userData_DailyReward_Unique.day = i;
			//	userData_DailyReward_Unique.isReceivedReward = DailyRewardStore.Instance.IsReceivedReward(i);
			//	userData_DailyReward_Unique.isReceivedBonus = DailyRewardStore.Instance.IsReceivedBonus(i);
			//	userData_DailyReward.listDailyRewardData.Insert(i, userData_DailyReward_Unique);
			//}
			//string rawJsonValueAsync = JsonConvert.SerializeObject(userData_DailyReward);
			//reference.Child(userID).Child(DataCloudSaverConstants.FIREBASE_NODE_USERDATA_DAILYREWARD).SetRawJsonValueAsync(rawJsonValueAsync);
		}

		public void RestoreData()
		{
			MonoSingleton<LifespanSurface>.Instance.LoadingProgressPopupController.Open();
			userID = NativeSpecificServicesSource.Services.UserProfile.GetFirebaseUserID();
			if (string.IsNullOrEmpty(userID))
			{
				UnityEngine.Debug.Log("Restore - User ID cannot be empty!");
				return;
			}
			base.StartCoroutine(_RestoreData());
		}

		private IEnumerator _RestoreData()
		{
			JobCompleteVerifier checker = new JobCompleteVerifier();
			//yield return new WaitForThreadedJobWithVerifier(delegate()
			//{
			//	FirebaseDatabase.DefaultInstance.GetReference(userID).GetValueAsync().ContinueWith(delegate(Task<DataSnapshot> task)
			//	{
			//		checker.isTaskCompleted = true;
			//		if (!task.IsFaulted)
			//		{
			//			if (task.IsCompleted)
			//			{
			//				DataSnapshot result = task.Result;
			//				if (result != null && result.ChildrenCount > 0L)
			//				{
			//					dataRestoreDeliver = new RecordRestoreDeliver();
			//					isDataCollected = false;
			//					RestoreData_Hero();
			//					RestoreData_GlobalUpgrade();
			//					RestoreData_Map();
			//					RestoreData_Theme();
			//					RestoreData_UserProfile();
			//					RestoreData_PowerUpItem();
			//					RestoreData_Tutorial();
			//					RestoreData_Offers();
			//					RestoreData_FreeResources();
			//					RestoreData_SaleBundle();
			//					RestoreData_DailyReward();
			//					RestoreData_DailyTrial();
			//				}
			//				else
			//				{
			//					dataCollectFailedFlag = true;
			//				}
			//			}
			//		}
			//	});
			//}, checker, System.Threading.ThreadPriority.Lowest);
			MonoSingleton<LifespanSurface>.Instance.LoadingProgressPopupController.Close();
			yield break;
		}

		private IEnumerator BackToMainMenu()
		{
			string notify = Singleton<AlertSynopsis>.Instance.GetNotiContent(135);
			MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init(notify, false, false);
			yield return new WaitForSeconds(1f);
			LoadingScreen.Instance.ShowLoading();
			yield return new WaitForSeconds(1f);
			GameSceneLoader.Instance.LoadScene(GameSceneLoader.MainMenuSceneName);
			yield break;
		}

		private bool IsDataGetCompleted()
		{
			return isDataCollected;
		}

		private bool CheckIfUserHaveCloudData()
		{
			return false;
		}

		private void RestoreData_Hero()
		{
			//FirebaseDatabase.DefaultInstance.GetReference(userID).Child(DataCloudSaverConstants.FIREBASE_NODE_USERDATA_HERO).GetValueAsync().ContinueWith(delegate(Task<DataSnapshot> task)
			//{
			//	if (task.IsFaulted)
			//	{
			//		UnityEngine.Debug.LogError("Restore data hero failed!");
			//	}
			//	else if (task.IsCompleted)
			//	{
			//		DataSnapshot result = task.Result;
			//		string rawJsonValue = result.GetRawJsonValue();
			//		PlayerRecord_Hero userData_Hero = JsonConvert.DeserializeObject<PlayerRecord_Hero>(rawJsonValue);
			//		dataRestoreDeliver.userData_Hero = new PlayerRecord_Hero();
			//		dataRestoreDeliver.userData_Hero = userData_Hero;
			//	}
			//});
		}

		private void RestoreData_GlobalUpgrade()
		{
			//FirebaseDatabase.DefaultInstance.GetReference(userID).Child(DataCloudSaverConstants.FIREBASE_NODE_USERDATA_GLOBALUPGRADE).GetValueAsync().ContinueWith(delegate(Task<DataSnapshot> task)
			//{
			//	if (task.IsFaulted)
			//	{
			//		UnityEngine.Debug.LogError("Restore data global upgrade failed!");
			//	}
			//	else if (task.IsCompleted)
			//	{
			//		DataSnapshot result = task.Result;
			//		string rawJsonValue = result.GetRawJsonValue();
			//		PlayerRecord_GlobalEnhance userData_GlobalUpgrade = JsonConvert.DeserializeObject<PlayerRecord_GlobalEnhance>(rawJsonValue);
			//		dataRestoreDeliver.userData_GlobalUpgrade = new PlayerRecord_GlobalEnhance();
			//		dataRestoreDeliver.userData_GlobalUpgrade = userData_GlobalUpgrade;
			//	}
			//});
		}

		private void RestoreData_Map()
		{
			//FirebaseDatabase.DefaultInstance.GetReference(userID).Child(DataCloudSaverConstants.FIREBASE_NODE_USERDATA_MAP).GetValueAsync().ContinueWith(delegate(Task<DataSnapshot> task)
			//{
			//	if (task.IsFaulted)
			//	{
			//		UnityEngine.Debug.LogError("Restore data map failed!");
			//	}
			//	else if (task.IsCompleted)
			//	{
			//		DataSnapshot result = task.Result;
			//		string rawJsonValue = result.GetRawJsonValue();
			//		PlayerRecord_Zone userData_Map = JsonConvert.DeserializeObject<PlayerRecord_Zone>(rawJsonValue);
			//		dataRestoreDeliver.userData_Map = new PlayerRecord_Zone();
			//		dataRestoreDeliver.userData_Map = userData_Map;
			//	}
			//});
		}

		private void RestoreData_Theme()
		{
			//FirebaseDatabase.DefaultInstance.GetReference(userID).Child(DataCloudSaverConstants.FIREBASE_NODE_USERDATA_THEME).GetValueAsync().ContinueWith(delegate(Task<DataSnapshot> task)
			//{
			//	if (task.IsFaulted)
			//	{
			//		UnityEngine.Debug.LogError("Restore data theme failed!");
			//	}
			//	else if (task.IsCompleted)
			//	{
			//		DataSnapshot result = task.Result;
			//		string rawJsonValue = result.GetRawJsonValue();
			//		PlayerRecord_Skin userData_Theme = JsonConvert.DeserializeObject<PlayerRecord_Skin>(rawJsonValue);
			//		dataRestoreDeliver.userData_Theme = new PlayerRecord_Skin();
			//		dataRestoreDeliver.userData_Theme = userData_Theme;
			//	}
			//});
		}

		private void RestoreData_UserProfile()
		{
			//FirebaseDatabase.DefaultInstance.GetReference(userID).Child(DataCloudSaverConstants.FIREBASE_NODE_USERDATA_USERPROFILE).GetValueAsync().ContinueWith(delegate(Task<DataSnapshot> task)
			//{
			//	if (task.IsFaulted)
			//	{
			//		UnityEngine.Debug.LogError("Restore data user profile failed!");
			//	}
			//	else if (task.IsCompleted)
			//	{
			//		DataSnapshot result = task.Result;
			//		string rawJsonValue = result.GetRawJsonValue();
			//		PlayerRecord_PlayerDossier userData_UserProfile = JsonConvert.DeserializeObject<PlayerRecord_PlayerDossier>(rawJsonValue);
			//		dataRestoreDeliver.userData_UserProfile = new PlayerRecord_PlayerDossier();
			//		dataRestoreDeliver.userData_UserProfile = userData_UserProfile;
			//	}
			//});
		}

		private void RestoreData_PowerUpItem()
		{
			//FirebaseDatabase.DefaultInstance.GetReference(userID).Child(DataCloudSaverConstants.FIREBASE_NODE_USERDATA_POWERUPITEM).GetValueAsync().ContinueWith(delegate(Task<DataSnapshot> task)
			//{
			//	if (task.IsFaulted)
			//	{
			//		UnityEngine.Debug.LogError("Restore data powerup item failed!");
			//	}
			//	else if (task.IsCompleted)
			//	{
			//		DataSnapshot result = task.Result;
			//		string rawJsonValue = result.GetRawJsonValue();
			//		PlayerRecord_PowerupItem userData_PowerupItem = JsonConvert.DeserializeObject<PlayerRecord_PowerupItem>(rawJsonValue);
			//		dataRestoreDeliver.userData_PowerupItem = new PlayerRecord_PowerupItem();
			//		dataRestoreDeliver.userData_PowerupItem = userData_PowerupItem;
			//	}
			//});
		}

		private void RestoreData_Tutorial()
		{
			//FirebaseDatabase.DefaultInstance.GetReference(userID).Child(DataCloudSaverConstants.FIREBASE_NODE_USERDATA_TUTORIAL).GetValueAsync().ContinueWith(delegate(Task<DataSnapshot> task)
			//{
			//	if (task.IsFaulted)
			//	{
			//		UnityEngine.Debug.LogError("Restore data tutorial failed!");
			//	}
			//	else if (task.IsCompleted)
			//	{
			//		DataSnapshot result = task.Result;
			//		string rawJsonValue = result.GetRawJsonValue();
			//		PlayerRecord_Tutorial userData_Tutorial = new PlayerRecord_Tutorial();
			//		userData_Tutorial.ListTutorialData = new Dictionary<string, bool>();
			//		userData_Tutorial = JsonConvert.DeserializeObject<PlayerRecord_Tutorial>(rawJsonValue);
			//		dataRestoreDeliver.userData_Tutorial = new PlayerRecord_Tutorial();
			//		dataRestoreDeliver.userData_Tutorial = userData_Tutorial;
			//	}
			//});
		}

		private void RestoreData_Offers()
		{
			//FirebaseDatabase.DefaultInstance.GetReference(userID).Child(DataCloudSaverConstants.FIREBASE_NODE_USERDATA_OFFER).GetValueAsync().ContinueWith(delegate(Task<DataSnapshot> task)
			//{
			//	if (task.IsFaulted)
			//	{
			//		UnityEngine.Debug.LogError("Restore data offer failed!");
			//	}
			//	else if (task.IsCompleted)
			//	{
			//		DataSnapshot result = task.Result;
			//		string rawJsonValue = result.GetRawJsonValue();
			//		PlayerRecord_Deal userData_Offer = JsonConvert.DeserializeObject<PlayerRecord_Deal>(rawJsonValue);
			//		dataRestoreDeliver.userData_Offer = new PlayerRecord_Deal();
			//		dataRestoreDeliver.userData_Offer = userData_Offer;
			//	}
			//});
		}

		private void RestoreData_FreeResources()
		{
			//FirebaseDatabase.DefaultInstance.GetReference(userID).Child(DataCloudSaverConstants.FIREBASE_NODE_USERDATA_FREERESOURCES).GetValueAsync().ContinueWith(delegate(Task<DataSnapshot> task)
			//{
			//	if (task.IsFaulted)
			//	{
			//		UnityEngine.Debug.LogError("Restore data free resouces failed!");
			//	}
			//	else if (task.IsCompleted)
			//	{
			//		DataSnapshot result = task.Result;
			//		string rawJsonValue = result.GetRawJsonValue();
			//		PlayerRecord_FreeResources userData_FreeResources = JsonConvert.DeserializeObject<PlayerRecord_FreeResources>(rawJsonValue);
			//		dataRestoreDeliver.userData_FreeResources = new PlayerRecord_FreeResources();
			//		dataRestoreDeliver.userData_FreeResources = userData_FreeResources;
			//	}
			//});
		}

		private void RestoreData_SaleBundle()
		{
			//FirebaseDatabase.DefaultInstance.GetReference(userID).Child(DataCloudSaverConstants.FIREBASE_NODE_USERDATA_SALEBUNDLE).GetValueAsync().ContinueWith(delegate(Task<DataSnapshot> task)
			//{
			//	if (task.IsFaulted)
			//	{
			//		UnityEngine.Debug.LogError("Restore data sale bundle failed!");
			//	}
			//	else if (task.IsCompleted)
			//	{
			//		DataSnapshot result = task.Result;
			//		string rawJsonValue = result.GetRawJsonValue();
			//		PlayerRecord_SalePack userData_SaleBundle = new PlayerRecord_SalePack();
			//		userData_SaleBundle = JsonConvert.DeserializeObject<PlayerRecord_SalePack>(rawJsonValue);
			//		dataRestoreDeliver.userData_SaleBundle = new PlayerRecord_SalePack();
			//		dataRestoreDeliver.userData_SaleBundle = userData_SaleBundle;
			//	}
			//});
		}

		private void RestoreData_DailyReward()
		{
			//FirebaseDatabase.DefaultInstance.GetReference(userID).Child(DataCloudSaverConstants.FIREBASE_NODE_USERDATA_DAILYREWARD).GetValueAsync().ContinueWith(delegate(Task<DataSnapshot> task)
			//{
			//	if (task.IsFaulted)
			//	{
			//		UnityEngine.Debug.LogError("Restore data daily reward failed!");
			//	}
			//	else if (task.IsCompleted)
			//	{
			//		DataSnapshot result = task.Result;
			//		string rawJsonValue = result.GetRawJsonValue();
			//		PlayerRecord_DailyPrize userData_DailyReward = new PlayerRecord_DailyPrize();
			//		userData_DailyReward = JsonConvert.DeserializeObject<PlayerRecord_DailyPrize>(rawJsonValue);
			//		dataRestoreDeliver.userData_DailyReward = new PlayerRecord_DailyPrize();
			//		dataRestoreDeliver.userData_DailyReward = userData_DailyReward;
			//	}
			//});
		}

		private void RestoreData_DailyTrial()
		{
			//FirebaseDatabase.DefaultInstance.GetReference(userID).Child(DataCloudSaverConstants.FIREBASE_NODE_USERDATA_DAILYTRIAL).GetValueAsync().ContinueWith(delegate(Task<DataSnapshot> task)
			//{
			//	if (task.IsFaulted)
			//	{
			//		UnityEngine.Debug.LogError("Restore data daily trial item failed!");
			//	}
			//	else if (task.IsCompleted)
			//	{
			//		DataSnapshot result = task.Result;
			//		string rawJsonValue = result.GetRawJsonValue();
			//		PlayerRecord_DailyOrdeal userData_DailyTrial = JsonConvert.DeserializeObject<PlayerRecord_DailyOrdeal>(rawJsonValue);
			//		dataRestoreDeliver.userData_DailyTrial = new PlayerRecord_DailyOrdeal();
			//		dataRestoreDeliver.userData_DailyTrial = userData_DailyTrial;
			//		isDataCollected = true;
			//	}
			//});
		}

		public void RetrieveData(string dbRefPath, Action<IRecordSnapshot> callback)
		{
			RetrieveDataWithMainThreadCallback(dbRefPath, callback);
		}

		public void RetrieveDataWithMainThreadCallback(string dbRef, Action<IRecordSnapshot> callback)
		{
			base.StartCoroutine(_RetrieveDataWithMainThreadCallback(dbRef, callback));
		}

		private IEnumerator _RetrieveDataWithMainThreadCallback(string dbRef, Action<IRecordSnapshot> callback)
		{
			//JobCompleteVerifier checker = new JobCompleteVerifier();
			//Task<DataSnapshot> mTask = null;
			//yield return new WaitForThreadedJobWithVerifier(delegate()
			//{
			//	FirebaseDatabase.DefaultInstance.GetReference(dbRef).GetValueAsync().ContinueWith(delegate(Task<DataSnapshot> task)
			//	{
			//		mTask = task;
			//		checker.isTaskCompleted = true;
			//	});
			//}, checker, System.Threading.ThreadPriority.Lowest);
			//if (callback != null && mTask != null)
			//{
			//	callback(new RecordSnapshotAndroid(mTask.Result, mTask.IsFaulted, mTask.IsCompleted));
			//}
			yield break;
		}

		public void UpdateData(Dictionary<string, object> updateList, string dbRefPath = null)
		{
			//DatabaseReference databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
			//if (!string.IsNullOrEmpty(dbRefPath))
			//{
			//	databaseReference = databaseReference.Child(dbRefPath);
			//}
			//databaseReference.UpdateChildrenAsync(updateList);
		}

		public void WriteData(object data, string dbRefPath)
		{
			WriteDataWithMainThreadCallback(data, dbRefPath, null);
		}

		public void WriteDataWithMainThreadCallback(object data, string dbRefPath, Action<IRecordSnapshot> callback)
		{
			base.StartCoroutine(_WriteDataWithMainThreadCallback(data, dbRefPath, callback));
		}

		private IEnumerator _WriteDataWithMainThreadCallback(object data, string dbRefPath, Action<IRecordSnapshot> callback)
		{
			//JobCompleteVerifier checker = new JobCompleteVerifier();
			//Task mTask = null;
			//string senderJSON = JsonConvert.SerializeObject(data);
			//yield return new WaitForThreadedJobWithVerifier(delegate()
			//{
			//	reference.Child(dbRefPath).SetRawJsonValueAsync(senderJSON).ContinueWith(delegate(Task task)
			//	{
			//		mTask = task;
			//		checker.isTaskCompleted = true;
			//	});
			//}, checker, System.Threading.ThreadPriority.Lowest);
			//if (callback != null)
			//{
			//	callback(new RecordSnapshotAndroid(null, mTask.IsFaulted, mTask.IsCompleted));
			//}
			yield break;
		}

		public void WriteGroupInfoTransaction(string groupInfoPath, bool isUserPremium, int tier = -1)
		{
			//reference.Child(groupInfoPath).RunTransaction(delegate(MutableData mutableData)
			//{
			//	Dictionary<string, object> dictionary = mutableData.Value as Dictionary<string, object>;
			//	if (dictionary != null)
			//	{
			//		WriteGroupInfo_UpdateGroupData(dictionary, isUserPremium);
			//		mutableData.Value = dictionary;
			//	}
			//	return TransactionResult.Success(mutableData);
			//}).ContinueWith(delegate(Task<DataSnapshot> task)
			//{
			//	if (task.Exception != null)
			//	{
			//		UnityEngine.Debug.Log(task.Exception.ToString());
			//	}
			//});
		}

		public void WriteNewGroupInfoTransaction(int newGroupId, bool isUserPremium, int tier = -1)
		{
			//string groupId = newGroupId.ToString();
			//reference.Child("Tournament/Groupinfo/").RunTransaction(delegate(MutableData mutableData)
			//{
			//	Dictionary<string, object> dictionary = mutableData.Value as Dictionary<string, object>;
			//	if (dictionary != null)
			//	{
			//		if (!dictionary.ContainsKey(groupId))
			//		{
			//			dictionary.Add(groupId, new Dictionary<string, object>());
			//		}
			//		Dictionary<string, object> groupInfo = dictionary[groupId] as Dictionary<string, object>;
			//		WriteGroupInfo_UpdateGroupData(groupInfo, isUserPremium);
			//		mutableData.Value = dictionary;
			//	}
			//	return TransactionResult.Success(mutableData);
			//}).ContinueWith(delegate(Task<DataSnapshot> task)
			//{
			//	if (task.Exception != null)
			//	{
			//		UnityEngine.Debug.Log(task.Exception.ToString());
			//	}
			//});
		}

		private void WriteGroupInfo_UpdateGroupData(Dictionary<string, object> groupInfo, bool isUserPremium)
		{
			if (groupInfo.ContainsKey("quantity"))
			{
				long num = (long)groupInfo["quantity"];
				groupInfo["quantity"] = num + 1L;
			}
			else
			{
				groupInfo["quantity"] = 1;
			}
			if (isUserPremium)
			{
				if (groupInfo.ContainsKey("premiumCount"))
				{
					long num2 = (long)groupInfo["premiumCount"];
					groupInfo["premiumCount"] = num2 + 1L;
				}
				else
				{
					groupInfo["premiumCount"] = 1;
				}
			}
		}

		public void ClaimPremiumUserInfor(string userID, string userName, string userEmail, string userPhoneNumber)
		{
			//string rawJsonValueAsync = JsonConvert.SerializeObject(new PlayerRecord_PremiumDetail
			//{
			//	userID = userID,
			//	userName = userName,
			//	userEmail = userEmail,
			//	userPhoneNUmber = userPhoneNumber
			//});
			//reference.Child(DataCloudSaverConstants.FIREBASE_NODE_USERDATA_PREMIUMINFOR).Child(userID).SetRawJsonValueAsync(rawJsonValueAsync);
		}

		//private DatabaseReference reference;

		private string userID = string.Empty;

		//private DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;

		private RecordRestoreDeliver dataRestoreDeliver;

		private bool isDataCollected;

		private bool dataCollectFailedFlag;
	}
}
