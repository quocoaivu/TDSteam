using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using FreeResources;
using Gameplay;
using LifetimePopup;
using GameCore;
using Newtonsoft.Json;
using Parameter;
using Services.PlatformSpecific;
using UnityEngine;
using UnityEngine.UI;
using WorldMap;

namespace Tournament
{
	public class ArenaDialogHandler : GameplayDialogHandler
	{
		public void Init()
		{
			OpenWithScaleAnimation();
			countdownUpdate = 50f;
			scrollHandle.offsetMin = new Vector2(scrollHandle.offsetMin.x, 0f);
			scrollHandle.offsetMax = new Vector2(scrollHandle.offsetMax.x, 0f);
			if (!isInited)
			{
				isUserSelfInfoRetrieved = false;
				isGroupPlayerRetrieved = false;
				isBlessedHeroRetrieved = false;
				isSeasonInfoRetrieved = false;
				isPriceConstantsRetrieved = false;
				notifyFlag = false;
				loadingTimeoutCountdown = 20f;
				prepareObject.SetActive(true);
				friendRewardNotify.SetActive(false);
				GameSignalCenter.Instance.Subscribe(GameSignalKind.OnTournamentMapRuleReceived, new SimpleListenerRecord(GameKit.GetUniqueId(), new GameSignalCenter.SimpleSubscribeMethod(OnFinishReadingTournamentMapRule)));
				GameSignalCenter.Instance.Subscribe(GameSignalKind.OnTournamentPriceConstantsReceived, new SimpleListenerRecord(GameKit.GetUniqueId(), new GameSignalCenter.SimpleSubscribeMethod(OnFinishReadingTournamentPriceConstants)));
				rankEntries.Add(sampleRankEntry);
				GlobeZoneRecord.Instance.MapRuleLoader.ReadTournamentMapRule();
				GlobeZoneRecord.Instance.MapRuleLoader.ReadTournamentPriceConstants();
				ReadSeasonInfoData();
				ReadUserSelfData();
			}
			else
			{
				prepareObject.SetActive(false);
			}
			OnGroupTabClicked();
			isStartTourWithAdsAvai = IsStartTourWithAdsAvailable();
			availableAdBtnBg.SetActive(isStartTourWithAdsAvai);
			unavailableAdBtnBg.SetActive(!isStartTourWithAdsAvai);
			SendEvent_EndGameTournament();
		}

		private void SendEvent_EndGameTournament()
		{
			//NativeSpecificServicesSource.Services.Analytics.SendEvent_EndGameTournament();
		}

		public override void Update()
		{
			base.Update();
			if (!isInited)
			{
				if (loadingTimeoutCountdown > 0f)
				{
					loadingTimeoutCountdown -= Time.deltaTime;
					if (loadingTimeoutCountdown <= 0f)
					{
						CloseWithScaleAnimation();
						MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init("No response from server. Please check your connection and come back later!", "OK", null, null);
					}
				}
				if (!isUserSelfInfoRetrieved || !isGroupPlayerRetrieved || !isBlessedHeroRetrieved || !isSeasonInfoRetrieved || !isPriceConstantsRetrieved)
				{
					return;
				}
				loadingTimeoutCountdown = -1f;
				if (notifyFlag)
				{
					if (!Debug.isDebugBuild)
					{
						CloseWithScaleAnimation();
					}
					else
					{
						isInited = true;
						SetupUI();
					}
					MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init(notifyContent, "OK", notifyCallback, null);
				}
				else
				{
					isInited = true;
					SetupUI();
				}
			}
			countdownUpdate -= Time.deltaTime;
			if (countdownUpdate <= 0f)
			{
				countdownUpdate = 50f;
				SetupSeasonTimeLeft();
				SetupNextFreeEntryTimeLeft();
			}
		}

		private void SetupUI()
		{
			prepareObject.SetActive(false);
			ZoneRuleSpec.Instance.SetCurrentSeasonID(seasonInfo.seasonNumber);
			GameKit.blessedHeroId = ZoneRuleSpec.Instance.GetBlessedHeroID();
			GameKit.SetRewardSprite(new PrizeItem(PrizeKind.SingleHero, GameKit.blessedHeroId, 1, false), blessedHeroIcon);
			seasonInfoText.text = string.Format("{0} : {1}", seasonInfo.seasonNumber + 1, seasonInfo.seasonStartDate.ToString("dd.MM.yyyy"));
			SetupSeasonTimeLeft();
			GameKit.WriteTimeStamp(ArenaDialogHandler.END_SEASON_TIME_KEY, seasonInfo.seasonEndDate);
			SetupCurrentLeague(userSelfInfo);
			nextFreeEntryTime = GetNextFreeEntryTime();
			SetupNextFreeEntryTimeLeft();
			gemQuantityToStartGameText.text = GameKit.GetGemQuantityTostartTour(false).ToString();
			if (userSelfInfo.recSeasonReward < seasonInfo.seasonNumber - 1)
			{
				OnReceiveLastSeasonReward();
			}
			//if (userSelfInfo.recFriendReward < seasonInfo.seasonNumber - 1 && NativeSpecificServicesSource.Services.UserProfile.IsLoggedIn_Facebook())
			//{
			//	friendRewardNotify.SetActive(true);
			//}
			int yourIndex = ArenaDialogHandler.GetYourIndex(groupPlayers);
			if (!groupPlayers[yourIndex].isYou)
			{
				groupPlayers.Add(new ArenaPlayerDetail(userSelfInfo.name, HeroStore.Instance.GetListHeroIDOwned(), new TimeSpan(0L), true, userSelfInfo.countryCode));
				ArenaDialogHandler.SortList(groupPlayers);
			}
			if (groupTab.isFocused)
			{
				OnGroupTabClicked();
			}
			SetupCurrentPrizeAndNextPrize();
			List<ArenaPrizeSetupRecord> leagueAllPrize = GameKit.GetLeagueAllPrize(1000);
			int num = leagueAllPrize[0].Itemquantities[0];
			// Guard unassigned serialized refs (dropped during the prefab rename/migration):
			// requireLoginFBDescText is {fileID: 0} in PanelTournament2, so the unguarded
			// .text below NRE'd and blocked the hub from finishing SetupUI.
			if (inviteFriendDescText != null)
			{
				inviteFriendDescText.text = string.Format(GameKit.GetLocalization("INVITE_FRIEND_REQUIRE"), num);
			}
			if (requireLoginFBDescText != null)
			{
				requireLoginFBDescText.text = string.Format(GameKit.GetLocalization("FB_LOGIN_REQUIRE"), num);
			}
			UnityEngine.Debug.LogFormat("+++userselfinfo tier: {0} gameTools {1}", new object[]
			{
				userSelfInfo.curtier,
				GameKit.tourUserSelfInfo.curtier
			});
			GameSignalCenter.Instance.Trigger(GameSignalKind.OnTournamentTierUp, null);
		}

		public void SetupCurrentLeague(ArenaPlayerSelfDetail userData)
		{
			int curtier = userData.curtier;
			leagueTitleText.text = GameKit.GetLocalization("LEAGUE_" + curtier);
			for (int i = leagueIcons.Count - 1; i >= 0; i--)
			{
				leagueIcons[i].SetActive(false);
			}
			leagueIcons[curtier].SetActive(true);
		}

		public void SetupCurrentPrizeAndNextPrize()
		{
			List<PrizeItem> rewards = null;
			List<PrizeItem> rewards2 = null;
			int lowerRank;
			int upperRank;
			int lowerRank2;
			int upperRank2;
			GetPrizeData(out lowerRank, out upperRank, out rewards, out lowerRank2, out upperRank2, out rewards2);
			curPrizeBlock.Init(upperRank, lowerRank, rewards);
			nextPrizeBlock.Init(upperRank2, lowerRank2, rewards2);
		}

		public void SetupSeasonTimeLeft()
		{
			if (!isSeasonInfoRetrieved)
			{
				return;
			}
			TimeSpan timeSpan = seasonInfo.seasonEndDate - ArenaDialogHandler.GetNow();
			seasonTimeLeftText.text = string.Format("{0}d{1}h{2}m", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes);
		}

		public void SetupNextFreeEntryTimeLeft()
		{
			if (!isUserSelfInfoRetrieved)
			{
				return;
			}
			TimeSpan timeSpan = nextFreeEntryTime - ArenaDialogHandler.GetNow();
			if (timeSpan.TotalSeconds < 0.0)
			{
				nextFreeEntryText.text = string.Empty;
				GameKit.SaveNumOfPlayTourCount(0);
			}
			else
			{
				nextFreeEntryText.text = string.Format(GameKit.GetLocalization("FREE_ENTRY_TEXT"), string.Format("{0}h{1}m", timeSpan.Hours, timeSpan.Minutes));
			}
		}

		public void SetupLeaderboard(List<ArenaPlayerDetail> players)
		{
			if (players == null)
			{
				return;
			}
			int count = players.Count;
			scrollContent.sizeDelta = new Vector2(scrollContent.sizeDelta.x, (float)count * heightOfRankEntry + 50f);
			for (int i = rankEntries.Count; i < count; i++)
			{
				ArenaRankEntryDirector tourRankEntryManager = UnityEngine.Object.Instantiate<ArenaRankEntryDirector>(sampleRankEntry, sampleRankEntry.transform.parent);
				tourRankEntryManager.transform.localPosition = sampleRankEntry.transform.localPosition + new Vector3(0f, (float)(-(float)i) * heightOfRankEntry, 0f);
				rankEntries.Add(tourRankEntryManager);
			}
			for (int j = count; j < rankEntries.Count; j++)
			{
				rankEntries[j].gameObject.SetActive(false);
			}
			for (int k = 0; k < count; k++)
			{
				rankEntries[k].gameObject.SetActive(true);
				rankEntries[k].Init(players[k]);
			}
			int yourIndex = ArenaDialogHandler.GetYourIndex(players);
			Vector3 localPosition = scrollContent.localPosition;
			localPosition.y = heightOfRankEntry * (float)(yourIndex - 3);
			scrollContent.localPosition = localPosition;
		}

		public static int GetYourIndex(List<ArenaPlayerDetail> players)
		{
			for (int i = players.Count - 1; i >= 0; i--)
			{
				if (players[i].isYou)
				{
					return i;
				}
			}
			return players.Count - 1;
		}

		private void OpenStartGamePopup()
		{
			GameSignalCenter.Instance.Trigger(GameSignalKind.EventPlayTournament, new SignalTriggerRecord(SignalTriggerKind.PlayTournament, 1, true));
			startGamePopupController.Init();
		}

		public void OnGroupTabClicked()
		{
			if (groupPlayers == null)
			{
				scrollContent.gameObject.SetActive(false);
				loadingIcon.SetActive(true);
			}
			else
			{
				scrollContent.gameObject.SetActive(true);
				loadingIcon.SetActive(false);
			}
			groupTab.SetFocus(true);
			friendTab.SetFocus(false);
			loginFbObj.SetActive(false);
			inviteFbObj.SetActive(false);
			SetupLeaderboard(groupPlayers);
		}

		public void OnFriendTabClicked()
		{
			if (!NativeSpecificServicesSource.Services.UserProfile.IsLoggedIn_Facebook())
			{
				scrollContent.gameObject.SetActive(false);
				loadingIcon.SetActive(false);
				loginFbObj.SetActive(true);
				inviteFbObj.SetActive(false);
			}
			else
			{
				loginFbObj.SetActive(false);
				if (friendPlayers == null)
				{
					scrollContent.gameObject.SetActive(false);
					loadingIcon.SetActive(true);
					inviteFbObj.SetActive(false);
					if (!isReadingFbFriendScores)
					{
						isReadingFbFriendScores = true;
						ReadTournamentFriends(userSelfInfo);
					}
				}
				else
				{
					scrollContent.gameObject.SetActive(true);
					loadingIcon.SetActive(false);
					inviteFbObj.SetActive(friendPlayers.Count < GameKit.requiredNumOfTourFriend);
					if (seasonInfo.seasonNumber - 1 >= 0 && userSelfInfo.recFriendReward < seasonInfo.seasonNumber - 1 && userSelfInfo.lastscore > 0)
					{
						List<ArenaPlayerDetail> list = new List<ArenaPlayerDetail>();
						list.Add(new ArenaPlayerDetail(userSelfInfo.name, GameKit.DecodeHeroList(userSelfInfo.heroesCode), new TimeSpan(0, 0, 0, 0, userSelfInfo.lastscore), true, userSelfInfo.countryCode));
						for (int i = friendsUserData.Count - 1; i >= 0; i--)
						{
							if (friendsUserData[i].lastscore > 0)
							{
								list.Add(new ArenaPlayerDetail(friendsUserData[i].name, GameKit.DecodeHeroList(friendsUserData[i].heroes), new TimeSpan(0, 0, 0, 0, friendsUserData[i].lastscore), false, friendsUserData[i].country));
							}
						}
						if (list.Count >= GameKit.requiredNumOfTourFriend)
						{
							if (tourReceiveRewardPopup == null)
							{
								tourReceiveRewardPopup = UnityEngine.Object.Instantiate<ArenaReceivePrizeDialog>(tourReceiveRewardPrefab, base.transform);
							}
							tourReceiveRewardPopup.InitFriendReward(userSelfInfo, seasonInfo.seasonNumber - 1, seasonInfo, list);
						}
					}
				}
			}
			friendTab.SetFocus(true);
			groupTab.SetFocus(false);
			friendRewardNotify.SetActive(false);
			SetupLeaderboard(friendPlayers);
		}

		public void OnLoginFbButtonClicked()
		{
			CloseWithScaleAnimation();
			MonoSingleton<WorldMap.UIRootHandler>.Instance.userProfilePopupController.Init();
		}

		public void OnInviteFriendClicked()
		{
			//NativeSpecificServicesSource.Services.UserProfile.InviteFriend_Facebook();
		}

		public void OnTourRuleButtonClicked()
		{
			if (tournamentRulePopup == null)
			{
				tournamentRulePopup = UnityEngine.Object.Instantiate<GameplayDialogHandler>(tourRulePopupPrefab, base.transform);
			}
			tournamentRulePopup.OpenWithScaleAnimation();
		}

		public void OnReceiveLastSeasonReward()
		{
			if (seasonInfo.seasonNumber - 1 < 0 || userSelfInfo.lastgroupid < 0)
			{
				return;
			}
			if (tourReceiveRewardPopup == null)
			{
				tourReceiveRewardPopup = UnityEngine.Object.Instantiate<ArenaReceivePrizeDialog>(tourReceiveRewardPrefab, base.transform);
			}
			tourReceiveRewardPopup.Init(userSelfInfo, seasonInfo.seasonNumber - 1, seasonInfo);
		}

		public void OnChatButtonClicked()
		{
			Application.OpenURL("https://www.facebook.com/KingdomDefense.BestTDGame/");
		}

		public void OnBestPlayerButtonClicked()
		{
		}

		public bool IsReadyToStart()
		{
			if (!GameUtils.IsInternetConnectionAvailable())
			{
				string notiContent = Singleton<AlertSynopsis>.Instance.GetNotiContent(119);
				MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init(notiContent, false, false);
				return false;
			}
			return isGroupPlayerRetrieved && isBlessedHeroRetrieved && isInited;
		}

		public void OnStartWithGemButtonClicked()
		{
			if (!IsReadyToStart())
			{
				return;
			}
			int currentGem = PlayerCurrencyStore.Instance.GetCurrentGem();
			int gemQuantityTostartTour = GameKit.GetGemQuantityTostartTour(false);
			if (gemQuantityTostartTour <= currentGem)
			{
				PlayerCurrencyStore.Instance.ChangeGem(-gemQuantityTostartTour, true);
				IncreaseNumOfPlayTime();
				OpenStartGamePopup();
			}
			else
			{
				UnityEngine.Debug.Log("KhÃ´ng Ä‘á»§ Gem!");
				string notiContent = Singleton<AlertSynopsis>.Instance.GetNotiContent(20);
				MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init(notiContent, true, true);
			}
		}

		public void OnStartWithAdsButtonClicked()
		{
			if (!IsReadyToStart())
			{
				return;
			}
			if (isStartTourWithAdsAvai)
			{
				FreeResources.ClipPlayerDirector.Instance.PlayRewardVideo(delegate(bool completed)
				{
					if (completed)
					{
						IncreaseNumOfPlayTime();
						OpenStartGamePopup();
					}
				});
			}
			else
			{
				string notiContent = Singleton<AlertSynopsis>.Instance.GetNotiContent(19);
				MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init(notiContent, false, false);
			}
		}

		public void OnAllPrizeBtnClicked()
		{
			if (tourAllPrizePopup == null)
			{
				tourAllPrizePopup = UnityEngine.Object.Instantiate<ArenaAllPrizeDialogHandler>(allprizePopupPrefab, base.transform);
			}
			tourAllPrizePopup.Init(GetLeagueIndex());
		}

		public void OnFinishReadingTournamentMapRule()
		{
			try
			{
				isBlessedHeroRetrieved = true;
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
				isBlessedHeroRetrieved = true;
				OnErrorOccur("(Error 100) Galahad gets lost :( Where am I?");
			}
		}

		public void OnFinishReadingTournamentPriceConstants()
		{
			isPriceConstantsRetrieved = true;
		}

		// TEST / no-backend safety: the cloud layer (DataCloudSaver) is a stub returning
		// empty data, and NativeSpecificServicesSource.Services is null when the scene is
		// played directly (boot never ran). In test mode, or when the service is missing,
		// feed an empty snapshot synchronously so the default-data branches run instead of
		// NRE-ing on Services.DataCloudSaver. Mirrors RecordCloudSaverEditor.
		private void RetrieveTournamentData(string path, Action<IRecordSnapshot> callback)
		{
			if (GeneralVariable.GeneralDefine.TOURNAMENT_USE_LOCAL_DATA
				|| NativeSpecificServicesSource.Services == null
				|| NativeSpecificServicesSource.Services.DataCloudSaver == null)
			{
				callback(new RecordSnapshotEditor());
				return;
			}
			NativeSpecificServicesSource.Services.DataCloudSaver.RetrieveDataWithMainThreadCallback(path, callback);
		}

		public void ReadSeasonInfoData()
		{
			RetrieveTournamentData("Tournament/Seasoninfo", delegate(IRecordSnapshot task)
			{
				try
				{
					if (!task.IsFaulted())
					{
						string rawJsonValue = task.GetRawJsonValue();
						if (string.IsNullOrEmpty(rawJsonValue))
						{
							int num = PlayerPrefs.GetInt("TESTtourDefaultMap", 0) % 6;
							UnityEngine.Debug.Log("test default map id " + num);
							seasonInfo = new ArenaSeasonDetail(num, ArenaDialogHandler.GetNow(), ArenaDialogHandler.GetNow().AddDays(6.0));
						}
						else
						{
							TourSeasonConfig tourSeasonConfig = JsonConvert.DeserializeObject<TourSeasonConfig>(rawJsonValue);
							if (tourSeasonConfig.lockTournament)
							{
								OnLockTournament(tourSeasonConfig.lockReason);
								if (!Debug.isDebugBuild)
								{
									return;
								}
							}
							seasonInfo = new ArenaSeasonDetail(tourSeasonConfig.number, GameKit.FromUnixTimeToDateTime(tourSeasonConfig.startTime), GameKit.FromUnixTimeToDateTime(tourSeasonConfig.endTime), tourSeasonConfig.minVersion, tourSeasonConfig.groupBaseonTier);
						}
						UnityEngine.Debug.LogFormat("cur ver {0} min ver {1}", new object[]
						{
							Application.version,
							seasonInfo.minVersion
						});
						if (!seasonInfo.IsCurVersionUptodate())
						{
							AddSimpleNotification("Please update to the newest version on store", delegate
							{
								Application.OpenURL(NativeSpecificServicesSource.Services.StoreLink);
							});
						}
						GameKit.tourSeasonInfo = seasonInfo;
						isSeasonInfoRetrieved = true;
					}
				}
				catch (Exception message)
				{
					UnityEngine.Debug.LogError(message);
					OnErrorOccur("(Error 105) Galahad loses track of time :( What time is it?");
					isSeasonInfoRetrieved = true;
				}
			});
		}

		public void ReadUserSelfData()
		{
			string uid = UserProfileStore.Instance.GetUserID();
			RetrieveTournamentData("Tournament/Users/" + uid, delegate(IRecordSnapshot task)
			{
				try
				{
					if (!task.IsFaulted())
					{
						UnityEngine.Debug.Log("++++ read user info  successful !!!!! " + uid);
						string rawJsonValue = task.GetRawJsonValue();
						if (string.IsNullOrEmpty(rawJsonValue))
						{
							userSelfInfo = new ArenaPlayerSelfDetail(-999, 0, -1, -1, -1, UserProfileStore.Instance.GetUserName(), 0, -1, GameKit.GetEncodedHeroList(HeroStore.Instance.GetListHeroIDOwned()), UserProfileStore.Instance.GetUserRegionCode(), false);
							GameKit.WriteTimeStamp(ArenaDialogHandler.NEXT_FREE_TIME_KEY, ArenaDialogHandler.GetNow());
						}
						else
						{
							TourUserData tourUserData = JsonConvert.DeserializeObject<TourUserData>(rawJsonValue);
							if (tourUserData.curgroupid < -1)
							{
								CustomInvoke(new Action(ReadUserSelfData), 1f);
								return;
							}
							userSelfInfo = new ArenaPlayerSelfDetail(tourUserData.curgroupid, tourUserData.curtier, tourUserData.lastgroupid, tourUserData.recFriendReward, tourUserData.recSeasonReward, tourUserData.name, tourUserData.score, tourUserData.lastscore, tourUserData.heroes, tourUserData.country, tourUserData.tierup);
							if (userSelfInfo.curgroupid >= 0)
							{
								ReadTournamentGroup(userSelfInfo);
							}
							if (GameKit.isTestingSeasonReward)
							{
								GameKit.isTestingSeasonReward = false;
								userSelfInfo.recSeasonReward = -1;
							}
							if (GameKit.isTestingFriendReward || GameKit.isTestingFriendRewardNoFakeUser)
							{
								userSelfInfo.recFriendReward = -1;
								if (userSelfInfo.lastscore <= 0)
								{
									userSelfInfo.lastscore = 123;
								}
								GameKit.isTestingFriendRewardNoFakeUser = false;
							}
						}
						GameKit.tourUserSelfInfo = userSelfInfo;
						if (userSelfInfo.curgroupid < 0)
						{
							groupPlayers = new List<ArenaPlayerDetail>
							{
								new ArenaPlayerDetail(userSelfInfo.name, HeroStore.Instance.GetListHeroIDOwned(), new TimeSpan(0L), true, userSelfInfo.countryCode)
							};
							GameKit.tourplayers = groupPlayers;
							ReadTournamentAllGroupInfo();
						}
						isUserSelfInfoRetrieved = true;
					}
				}
				catch (Exception message)
				{
					UnityEngine.Debug.LogError(message);
					OnErrorOccur("(Error 110) Galahad misses his mom :( Oh no, He's about to cry!!");
					isUserSelfInfoRetrieved = true;
				}
			});
		}

		public void ReadTournamentAllGroupInfo()
		{
			RetrieveTournamentData("Tournament/Groupinfo", delegate(IRecordSnapshot task)
			{
				try
				{
					if (!task.IsFaulted())
					{
						string rawJsonValue = task.GetRawJsonValue();
						Dictionary<string, TourGroupInfo> dictionary = JsonConvert.DeserializeObject<Dictionary<string, TourGroupInfo>>(rawJsonValue);
						GameKit.allGroupInfos = dictionary;
						if (dictionary == null)
						{
							GameKit.allGroupInfos = new Dictionary<string, TourGroupInfo>();
						}
						isGroupPlayerRetrieved = true;
					}
				}
				catch (Exception message)
				{
					UnityEngine.Debug.LogError(message);
					OnErrorOccur("(Error 130) Galahad is shy :( Is it because of that girl in the tournament group?");
					isGroupPlayerRetrieved = true;
				}
			});
		}

		public void ReadTournamentGroup(ArenaPlayerSelfDetail userData)
		{
			int curgroupid = userData.curgroupid;
			RetrieveTournamentData("Tournament/Curseasongroups/" + curgroupid, delegate(IRecordSnapshot task)
			{
				try
				{
					if (!task.IsFaulted())
					{
						UnityEngine.Debug.Log("++++ read Group data successful !!!!! ");
						string rawJsonValue = task.GetRawJsonValue();
						if (string.IsNullOrEmpty(rawJsonValue))
						{
							groupPlayers = new List<ArenaPlayerDetail>
							{
								new ArenaPlayerDetail(userSelfInfo.name, HeroStore.Instance.GetListHeroIDOwned(), new TimeSpan(0L), true, userSelfInfo.countryCode)
							};
							GameKit.tourplayers = groupPlayers;
							isGroupPlayerRetrieved = true;
						}
						else
						{
							Dictionary<string, TourSeasonGroupMember> entries = JsonConvert.DeserializeObject<Dictionary<string, TourSeasonGroupMember>>(rawJsonValue);
							groupPlayers = ArenaDialogHandler.GetTourGroupList(entries);
							GameKit.tourplayers = groupPlayers;
							isGroupPlayerRetrieved = true;
						}
					}
				}
				catch (Exception message)
				{
					UnityEngine.Debug.LogError(message);
					OnErrorOccur("(Error 120) Galahad is shy :( Is it because of that girl in the tournament group?");
					isGroupPlayerRetrieved = true;
				}
			});
		}

		private IEnumerator RetryReadTournamentGroup(ArenaPlayerSelfDetail userData)
		{
			yield return new WaitForSeconds(2f);
			ReadTournamentGroup(userData);
			yield break;
		}

		public void ReadTournamentFriends(ArenaPlayerSelfDetail userData)
		{
			UnityEngine.Debug.Log("start read tour friends");
			if (!NativeSpecificServicesSource.Services.UserProfile.IsLoggedIn_Facebook())
			{
				return;
			}
			NativeSpecificServicesSource.Services.UserProfile.GetUidsOfUserFriends(delegate(List<string> friendList)
			{
				string text = "++Friends num: " + friendList.Count + ",";
				int num = Mathf.Min(friendList.Count, 10);
				for (int i = 0; i < num; i++)
				{
					text = text + friendList[i] + "__";
				}
				UnityEngine.Debug.Log(text);
				base.StartCoroutine(GetTourFriendList(friendList));
			});
		}

		public static List<ArenaPlayerDetail> GetTourGroupList(Dictionary<string, TourSeasonGroupMember> entries)
		{
			List<ArenaPlayerDetail> list = new List<ArenaPlayerDetail>();
			string userID = UserProfileStore.Instance.GetUserID();
			foreach (KeyValuePair<string, TourSeasonGroupMember> entry in entries)
			{
				list.Add(new ArenaPlayerDetail(entry, userID));
			}
			ArenaDialogHandler.SortList(list);
			return list;
		}

		public IEnumerator GetTourFriendList(List<string> fbIds)
		{
			friendsUserData.Clear();
			List<ArenaPlayerDetail> rtn = new List<ArenaPlayerDetail>();
			rtn.Add(new ArenaPlayerDetail(userSelfInfo.name, GameKit.DecodeHeroList(userSelfInfo.heroesCode), new TimeSpan(0, 0, 0, 0, userSelfInfo.score), true, userSelfInfo.countryCode));
			int friendCountdown = fbIds.Count;
			for (int i = fbIds.Count - 1; i >= 0; i--)
			{
				NativeSpecificServicesSource.Services.DataCloudSaver.RetrieveData("Tournament/FBToUid/" + fbIds[i], delegate(IRecordSnapshot task)
				{
					if (task.IsCompleted())
					{
						string rawJsonValue = task.GetRawJsonValue();
						UnityEngine.Debug.Log("++fb id to uid json: " + rawJsonValue);
						if (string.IsNullOrEmpty(rawJsonValue))
						{
							friendCountdown--;
						}
						else
						{
							string str = JsonConvert.DeserializeObject<string>(rawJsonValue);
                            NativeSpecificServicesSource.Services.DataCloudSaver.RetrieveData("Tournament/Users/" + str, delegate (IRecordSnapshot secondTask)
                            {
								string rawJsonValue2 = secondTask.GetRawJsonValue();
								UnityEngine.Debug.Log("___ new friend user data json: " + rawJsonValue2);
								if (!string.IsNullOrEmpty(rawJsonValue2))
								{
									TourUserData tourUserData = JsonConvert.DeserializeObject<TourUserData>(rawJsonValue2);
									friendsUserData.Add(tourUserData);
									if (tourUserData.score > 0)
									{
										rtn.Add(new ArenaPlayerDetail(tourUserData.name, GameKit.DecodeHeroList(tourUserData.heroes), new TimeSpan(0, 0, 0, 0, tourUserData.score), false, tourUserData.country));
									}
								}
								friendCountdown--;
							});
						}
					}
					else
					{
						friendCountdown--;
					}
				});
			}
			while (friendCountdown > 0)
			{
				yield return null;
			}
			if (GameKit.isTestingFriendReward)
			{
				GameKit.isTestingFriendReward = false;
				AddTestFriendDataForTesting();
			}
			ArenaDialogHandler.SortList(rtn);
			friendPlayers = rtn;
			if (friendTab.isFocused)
			{
				OnFriendTabClicked();
			}
			yield break;
		}

		private void AddTestFriendDataForTesting()
		{
			for (int i = 0; i < 5; i++)
			{
				TourUserData tourUserData = new TourUserData();
				tourUserData.name = "tester" + i;
				tourUserData.heroes = 3;
				tourUserData.lastscore = (i + 1) * 100;
				tourUserData.score = i + 1;
				tourUserData.country = "us";
				friendsUserData.Add(tourUserData);
			}
		}

		public static void SortList(List<ArenaPlayerDetail> players)
		{
			int count = players.Count;
			for (int i = 0; i < count - 1; i++)
			{
				for (int j = i + 1; j < count; j++)
				{
					if (players[i].time.TotalMilliseconds < players[j].time.TotalMilliseconds)
					{
						ArenaPlayerDetail value = players[i];
						players[i] = players[j];
						players[j] = value;
					}
				}
			}
			for (int k = 0; k < count; k++)
			{
				players[k].rank = k;
			}
		}

		public int GetLeagueIndex()
		{
			if (userSelfInfo != null)
			{
				return userSelfInfo.curtier;
			}
			return 0;
		}

		public static DateTime GetNow()
		{
			return GameKit.GetNow();
		}

		public void GetPrizeData(out int curLowerRange, out int curUpperRange, out List<PrizeItem> curReward, out int nextLowerRange, out int nextUpperRange, out List<PrizeItem> nextReward)
		{
			int yourIndex = ArenaDialogHandler.GetYourIndex(groupPlayers);
			List<ArenaPrizeSetupRecord> leagueAllPrize = GameKit.GetLeagueAllPrize(GetLeagueIndex());
			int index = 1;
			int index2 = 0;
			int count = leagueAllPrize.Count;
			for (int i = 0; i < count; i++)
			{
				if (leagueAllPrize[i].Rankrangelower >= yourIndex + 1 && yourIndex + 1 >= leagueAllPrize[i].Rankrangeupper)
				{
					index = i;
					index2 = ((i <= 0) ? i : (i - 1));
					break;
				}
			}
			curLowerRange = leagueAllPrize[index].Rankrangelower;
			curUpperRange = leagueAllPrize[index].Rankrangeupper;
			curReward = GameKit.GetTournamentRewardList(leagueAllPrize[index]);
			nextLowerRange = leagueAllPrize[index2].Rankrangelower;
			nextUpperRange = leagueAllPrize[index2].Rankrangeupper;
			nextReward = GameKit.GetTournamentRewardList(leagueAllPrize[index2]);
		}

		public int GetBlessedHeroId()
		{
			return 0;
		}

		public bool IsStartTourWithAdsAvailable()
		{
			return FreeResources.ClipPlayerDirector.Instance.CheckIfVideoExits();
		}

		public DateTime GetNextFreeEntryTime()
		{
			return GameKit.ReadTimeStamp(ArenaDialogHandler.NEXT_FREE_TIME_KEY);
		}

		public void IncreaseNumOfPlayTime()
		{
			int numOfPlayTourCount = GameKit.GetNumOfPlayTourCount();
			if (numOfPlayTourCount == 0)
			{
				GameKit.WriteTimeStamp(ArenaDialogHandler.NEXT_FREE_TIME_KEY, ArenaDialogHandler.GetNow().AddHours(12.0));
			}
			GameKit.SaveNumOfPlayTourCount(numOfPlayTourCount + 1);
		}

		public void OnErrorOccur(string errorMessage)
		{
			notifyContent = string.Format("{0}\nPlease reload or come back later.", errorMessage);
			notifyFlag = true;
		}

		public void OnLockTournament(string lockReason)
		{
			notifyContent = string.Format("{0}\nPlease comeback in a few minutes.", lockReason);
			notifyFlag = true;
		}

		public void AddSimpleNotification(string notification, Action callback = null)
		{
			notifyContent = notification;
			notifyFlag = true;
			notifyCallback = callback;
		}

		[SerializeField]
		private BeginGameDialogHandler startGamePopupController;

		private GameplayDialogHandler tournamentRulePopup;

		private ArenaReceivePrizeDialog tourReceiveRewardPopup;

		public GameplayDialogHandler tourRulePopupPrefab;

		public ArenaReceivePrizeDialog tourReceiveRewardPrefab;

		private ArenaAllPrizeDialogHandler tourAllPrizePopup;

		public ArenaAllPrizeDialogHandler allprizePopupPrefab;

		public ArenaTabDirector groupTab;

		public ArenaTabDirector friendTab;

		public GameObject friendRewardNotify;

		public Text seasonInfoText;

		public Text seasonTimeLeftText;

		public ArenaRankEntryDirector sampleRankEntry;

		public RectTransform scrollContent;

		public RectTransform scrollHandle;

		public Text leagueTitleText;

		public List<GameObject> leagueIcons;

		public ArenaRankPrizeDirector curPrizeBlock;

		public ArenaRankPrizeDirector nextPrizeBlock;

		public Image blessedHeroIcon;

		public Text gemQuantityToStartGameText;

		public GameObject availableAdBtnBg;

		public GameObject unavailableAdBtnBg;

		public Text nextFreeEntryText;

		public float heightOfRankEntry;

		public GameObject loadingIcon;

		public GameObject prepareObject;

		public GameObject inviteFbObj;

		public GameObject loginFbObj;

		public Text inviteFriendDescText;

		public Text requireLoginFBDescText;

		private List<ArenaRankEntryDirector> rankEntries = new List<ArenaRankEntryDirector>();

		private List<TourUserData> friendsUserData = new List<TourUserData>();

		private List<ArenaPlayerDetail> groupPlayers;

		private List<ArenaPlayerDetail> friendPlayers;

		private ArenaSeasonDetail seasonInfo;

		private ArenaPlayerSelfDetail userSelfInfo;

		private float countdownUpdate;

		private float loadingTimeoutCountdown;

		private bool isStartTourWithAdsAvai;

		private DateTime nextFreeEntryTime;

		private bool isInited;

		private bool isSeasonInfoRetrieved;

		private bool isUserSelfInfoRetrieved;

		private bool isGroupPlayerRetrieved;

		private bool isBlessedHeroRetrieved;

		private bool isPriceConstantsRetrieved;

		private bool notifyFlag;

		private string notifyContent = string.Empty;

		private Action notifyCallback;

		public static string NEXT_FREE_TIME_KEY = "NEXTFTKEY";

		public static string END_SEASON_TIME_KEY = "ENDSSTKEY";

		private int maxFriendNumber = 20;

		private bool isReadingFbFriendScores;
	}
}
