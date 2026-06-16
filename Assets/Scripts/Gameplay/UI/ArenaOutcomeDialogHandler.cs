using System;
using System.Collections.Generic;
using Data;
using LifetimePopup;
using GameCore;
using Services.PlatformSpecific;
using Tournament;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class ArenaOutcomeDialogHandler : GameplayDialogHandler
	{
		public void Init()
		{
			OpenWithScaleAnimation();
			scrollHandle.offsetMin = new Vector2(scrollHandle.offsetMin.x, 0f);
			scrollHandle.offsetMax = new Vector2(scrollHandle.offsetMax.x, 0f);
			if (!isInited)
			{
				isInited = true;
				rankEntries.Add(sampleRankEntry);
				for (int i = 1; i < GameKit.tourplayers.Count; i++)
				{
					ArenaRankEntryDirector tourRankEntryManager = UnityEngine.Object.Instantiate<ArenaRankEntryDirector>(sampleRankEntry, sampleRankEntry.transform.parent);
					tourRankEntryManager.transform.localPosition = sampleRankEntry.transform.localPosition + new Vector3(0f, (float)(-(float)i) * heightOfRankEntry, 0f);
					rankEntries.Add(tourRankEntryManager);
				}
				scrollContent.sizeDelta = new Vector2(scrollContent.sizeDelta.x, (float)GameKit.tourplayers.Count * heightOfRankEntry + 50f);
			}
			if (!GameUtils.IsInternetConnectionAvailable())
			{
				string content = "Can't summit result! Please check your internet connection!";
				MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init(content, "Retry", new Action(UpdateResultToServer), null);
			}
			else
			{
				UpdateResultToServer();
			}
			InitResult();
		}

		private void UpdateResultToServer()
		{
			if (GameKit.tourUserSelfInfo.curgroupid >= -1)
			{
				SendResult();
			}
			else
			{
				InitTournamentUserDataOnDb();
			}
		}

		private void InitResult()
		{
			int milliseconds = (int)(MonoSingleton<GameRecord>.Instance.tournamentBattleTime * 1000f);
			TimeSpan time = new TimeSpan(0, 0, 0, 0, milliseconds);
			timeResult.text = string.Format("{0}:{1:00}.{2:000}", (int)time.TotalMinutes, time.Seconds, time.Milliseconds);
			int yourIndex = ArenaDialogHandler.GetYourIndex(GameKit.tourplayers);
			if (time.TotalMilliseconds > GameKit.tourplayers[yourIndex].time.TotalMilliseconds)
			{
				GameKit.tourplayers[yourIndex].time = time;
				GameKit.tourplayers[yourIndex].heroIds = MonoSingleton<GameRecord>.Instance.ListHeroesIdsSelected;
				ArenaDialogHandler.SortList(GameKit.tourplayers);
				yourIndex = ArenaDialogHandler.GetYourIndex(GameKit.tourplayers);
			}
			int count = GameKit.tourplayers.Count;
			for (int i = 0; i < count; i++)
			{
				rankEntries[i].Init(GameKit.tourplayers[i]);
			}
			Vector3 localPosition = scrollContent.localPosition;
			localPosition.y = heightOfRankEntry * (float)(yourIndex - 3);
			scrollContent.localPosition = localPosition;
			yourIndex = ArenaDialogHandler.GetYourIndex(GameKit.tourplayers);
		}

		private void SendResult()
		{
			int num = (int)(MonoSingleton<GameRecord>.Instance.tournamentBattleTime * 1000f);
			TimeSpan timeSpan = new TimeSpan(0, 0, 0, 0, num);
			int yourIndex = ArenaDialogHandler.GetYourIndex(GameKit.tourplayers);
			if (timeSpan.TotalMilliseconds <= GameKit.tourplayers[yourIndex].time.TotalMilliseconds)
			{
				UnityEngine.Debug.LogFormat("not the highest score, yours {0} score {1} highest {2} ", new object[]
				{
					timeSpan.TotalMilliseconds,
					num,
					GameKit.tourplayers[yourIndex].time.TotalMilliseconds
				});
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			string userID = UserProfileStore.Instance.GetUserID();
			if (string.IsNullOrEmpty(userID))
			{
				return;
			}
			int encodedHeroList = GameKit.GetEncodedHeroList(MonoSingleton<GameRecord>.Instance.ListHeroesIdsSelected);
			int curgroupid = GameKit.tourUserSelfInfo.curgroupid;
			if (curgroupid < 0)
			{
				int.TryParse(PickNewGroup(MonoSingleton<GameRecord>.Instance.ListHeroesIdsSelected), out curgroupid);
				dictionary[string.Format("Tournament/Users/{0}/curgroupid", userID)] = curgroupid;
				UnityEngine.Debug.LogFormat("+_+_+ decided to join group {0}", new object[]
				{
					curgroupid
				});
			}
			dictionary[string.Format("Tournament/Curseasongroups/{0}/{1}/heroes", curgroupid, userID)] = encodedHeroList;
			dictionary[string.Format("Tournament/Curseasongroups/{0}/{1}/score", curgroupid, userID)] = num;
			if (!string.IsNullOrEmpty(GameKit.tourUserSelfInfo.name))
			{
				dictionary[string.Format("Tournament/Curseasongroups/{0}/{1}/name", curgroupid, userID)] = GameKit.tourUserSelfInfo.name;
			}
			dictionary[string.Format("Tournament/Curseasongroups/{0}/{1}/country", curgroupid, userID)] = GameKit.tourUserSelfInfo.countryCode;
			dictionary[string.Format("Tournament/Users/{0}/score", userID)] = num;
			dictionary[string.Format("Tournament/Users/{0}/heroes", userID)] = encodedHeroList;
            if (NativeSpecificServicesSource.Services.UserProfile.IsLoggedIn_Facebook())
            {
                string uidOfUser = NativeSpecificServicesSource.Services.UserProfile.GetUidOfUser();
                if (!string.IsNullOrEmpty(uidOfUser))
                {
                    dictionary[string.Format("Tournament/FBToUid/{0}", uidOfUser)] = userID;
                }
            }
            UnityEngine.Debug.Log("1111 update score to " + num);
            NativeSpecificServicesSource.Services.DataCloudSaver.UpdateData(dictionary, null);
        }

		public TourUserData InitTournamentUserDataOnDb()
		{
			string userID = UserProfileStore.Instance.GetUserID();
			if (string.IsNullOrEmpty(userID))
			{
				return null;
			}
			int num = (int)(MonoSingleton<GameRecord>.Instance.tournamentBattleTime * 1000f);
			TourUserData tourUserData = new TourUserData();
			string text = PickNewGroup(MonoSingleton<GameRecord>.Instance.ListHeroesIdsSelected);
			int.TryParse(text, out tourUserData.curgroupid);
			tourUserData.heroes = GameKit.GetEncodedHeroList(MonoSingleton<GameRecord>.Instance.ListHeroesIdsSelected);
			tourUserData.lastgroupid = -1;
			tourUserData.name = UserProfileStore.Instance.GetUserName();
			tourUserData.recFriendReward = GameKit.tourSeasonInfo.seasonNumber - 1;
			tourUserData.recFriendReward = GameKit.tourSeasonInfo.seasonNumber - 1;
			tourUserData.score = num;
			tourUserData.country = UserProfileStore.Instance.GetUserRegionCode();
            NativeSpecificServicesSource.Services.DataCloudSaver.WriteData(tourUserData, "Tournament/Users/" + userID);
            UnityEngine.Debug.Log("___11___ add new tour user to group " + tourUserData.curgroupid);
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary[string.Format("Tournament/Curseasongroups/{0}/{1}/heroes", text, userID)] = tourUserData.heroes;
			dictionary[string.Format("Tournament/Curseasongroups/{0}/{1}/score", text, userID)] = num;
			dictionary[string.Format("Tournament/Curseasongroups/{0}/{1}/name", text, userID)] = tourUserData.name;
			dictionary[string.Format("Tournament/Curseasongroups/{0}/{1}/country", text, userID)] = tourUserData.country;
            NativeSpecificServicesSource.Services.DataCloudSaver.UpdateData(dictionary, null);
            if (NativeSpecificServicesSource.Services.UserProfile.IsLoggedIn_Facebook())
            {
                string uidOfUser = NativeSpecificServicesSource.Services.UserProfile.GetUidOfUser();
                if (!string.IsNullOrEmpty(uidOfUser))
                {
                    NativeSpecificServicesSource.Services.DataCloudSaver.WriteData(userID, "Tournament/FBToUid/" + uidOfUser);
                }
            }
            return tourUserData;
        }

		private string PickNewGroup(List<int> heroes)
		{
			int thresQuantity = (int)((float)GameKit.maxUserPerTourGroup * 0.75f);
			int num = (int)((float)GameKit.maxUserPerTourGroup * 0.88f);
			bool flag = IsMyTeamPremium(heroes);
			int num2 = 0;
			int num3 = 0;
			foreach (KeyValuePair<string, TourGroupInfo> keyValuePair in GameKit.allGroupInfos)
			{
				num3++;
				if (keyValuePair.Value.quantity >= num)
				{
					num2++;
				}
			}
			int num4 = 0;
			if (num2 >= num3)
			{
				num4 = GetNewGroupId(num3);
				//NativeSpecificServicesSource.Services.DataCloudSaver.WriteNewGroupInfoTransaction(num4, flag, 0);
			}
			else
			{
				float num5 = -9999f;
				KeyValuePair<string, TourGroupInfo> keyValuePair2 = default(KeyValuePair<string, TourGroupInfo>);
				foreach (KeyValuePair<string, TourGroupInfo> keyValuePair3 in GameKit.allGroupInfos)
				{
					if (keyValuePair3.Value.quantity < GameKit.maxUserPerTourGroup && IsGroupSuitable(keyValuePair3.Key))
					{
						float groupScore = GetGroupScore(keyValuePair3, thresQuantity, flag);
						UnityEngine.Debug.LogFormat("____Group {0} has score of {1}", new object[]
						{
							keyValuePair3.Key,
							groupScore
						});
						if (groupScore > num5)
						{
							num5 = groupScore;
							keyValuePair2 = keyValuePair3;
						}
					}
				}
				if (num5 > -9000f)
				{
					int.TryParse(keyValuePair2.Key, out num4);
					//NativeSpecificServicesSource.Services.DataCloudSaver.WriteGroupInfoTransaction("Tournament/Groupinfo/" + num4, flag, -1);
				}
				else
				{
					num4 = GetNewGroupId(num3);
					//NativeSpecificServicesSource.Services.DataCloudSaver.WriteNewGroupInfoTransaction(num4, flag, 0);
				}
			}
			return num4.ToString();
		}

		private bool IsGroupSuitable(string groupKey)
		{
			if (!GameKit.tourSeasonInfo.isChoosingGroupBaseOnTier)
			{
				return true;
			}
			int num;
			int.TryParse(groupKey, out num);
			return num % 100 == GameKit.tourUserSelfInfo.curtier;
		}

		private int GetNewGroupId(int curNumOfGroup)
		{
			for (int i = 0; i <= curNumOfGroup; i++)
			{
				int result = i;
				if (GameKit.tourSeasonInfo.isChoosingGroupBaseOnTier)
				{
					result = i * 100 + GameKit.tourUserSelfInfo.curtier;
				}
				if (!GameKit.allGroupInfos.ContainsKey(result.ToString()))
				{
					return result;
				}
			}
			return 10000;
		}

		private bool IsMyTeamPremium(List<int> heroes)
		{
			for (int i = heroes.Count - 1; i >= 0; i--)
			{
				if (heroes[i] != 1 && heroes[i] != 2)
				{
					return true;
				}
			}
			return false;
		}

		private float GetGroupScore(KeyValuePair<string, TourGroupInfo> group, int thresQuantity, bool isMyTeamPremium)
		{
			float num = 0f;
			if (group.Value.quantity < thresQuantity)
			{
				num += 10f - (float)(thresQuantity - group.Value.quantity) * 1f / (float)thresQuantity * 10f;
			}
			else
			{
				num += 10f - (float)(group.Value.quantity - thresQuantity) * 1f / (float)(GameKit.maxUserPerTourGroup - thresQuantity);
			}
			if (isMyTeamPremium)
			{
				num += (float)(GameKit.maxUserPerTourGroup - group.Value.premiumCount) * 10f / (float)GameKit.maxUserPerTourGroup;
			}
			else
			{
				num += 10f - (float)(GameKit.maxUserPerTourGroup - group.Value.premiumCount) * 10f / (float)GameKit.maxUserPerTourGroup;
			}
			return num;
		}

		[SerializeField]
		private Text timeResult;

		public ArenaRankEntryDirector sampleRankEntry;

		public float heightOfRankEntry;

		public RectTransform scrollContent;

		public RectTransform scrollHandle;

		private List<ArenaRankEntryDirector> rankEntries = new List<ArenaRankEntryDirector>();

		private bool isInited;
	}
}
