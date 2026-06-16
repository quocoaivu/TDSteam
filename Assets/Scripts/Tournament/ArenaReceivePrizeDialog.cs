using System;
using System.Collections.Generic;
using Data;
using Gameplay;
using LifetimePopup;
using Newtonsoft.Json;
using Services.PlatformSpecific;
using Tournament;
using UnityEngine;
using UnityEngine.UI;

public class ArenaReceivePrizeDialog : GameplayDialogHandler
{
	public void Init(ArenaPlayerSelfDetail userData, int lastSeasonNumber, ArenaSeasonDetail seasonInfo)
	{
		if (lastSeasonNumber < 0)
		{
			base.gameObject.SetActive(false);
			return;
		}
		tourRewardType = ArenaPrizeKind.SeasonReward;
		this.userData = userData;
		GetBtnObj.SetActive(false);
		this.lastSeasonNumber = lastSeasonNumber;
		seasonInfoText.text = string.Format("SEASON {0}", seasonInfo.seasonNumber + 1 - 1);
		OpenWithScaleAnimation();
		ReadLastTournamentGroup();
	}

	public void InitFriendReward(ArenaPlayerSelfDetail userData, int lastSeasonNumber, ArenaSeasonDetail seasonInfo, List<ArenaPlayerDetail> friendPlayers)
	{
		if (lastSeasonNumber < 0)
		{
			base.gameObject.SetActive(false);
			return;
		}
		tourRewardType = ArenaPrizeKind.FriendReward;
		this.userData = userData;
		this.lastSeasonNumber = lastSeasonNumber;
		loadingIcon.SetActive(false);
		seasonInfoText.text = string.Format("SEASON {0}", seasonInfo.seasonNumber + 1 - 1);
		OpenWithScaleAnimation();
		SetupLeaderboard(GameKit.GetLeagueAllPrize(1000), friendPlayers);
		int yourIndex = ArenaDialogHandler.GetYourIndex(friendPlayers);
		if (yourIndex < GameKit.requiredNumOfTourFriend)
		{
			//NativeSpecificServicesSource.Services.Analytics.SendEvent_ReceiveFriendReward();
		}
	}

	public void ReadLastTournamentGroup()
	{
		int lastgroupid = userData.lastgroupid;
        NativeSpecificServicesSource.Services.DataCloudSaver.RetrieveDataWithMainThreadCallback("Tournament/Lastseasongroups/" + lastgroupid, delegate (IRecordSnapshot task)
        {
			if (task.IsFaulted())
			{
				return;
			}
			string rawJsonValue = task.GetRawJsonValue();
			UnityEngine.Debug.Log("++last group json: " + rawJsonValue);
			loadingIcon.SetActive(false);
			if (string.IsNullOrEmpty(rawJsonValue))
			{
				Close();
			}
			else
			{
				Dictionary<string, TourSeasonGroupMember> entries = JsonConvert.DeserializeObject<Dictionary<string, TourSeasonGroupMember>>(rawJsonValue);
				List<ArenaPlayerDetail> tourGroupList = ArenaDialogHandler.GetTourGroupList(entries);
				int num = (!userData.tierup) ? userData.curtier : (userData.curtier - 1);
				if (num < 0)
				{
					num = 0;
				}
				List<ArenaPrizeSetupRecord> leagueAllPrize = GameKit.GetLeagueAllPrize(num);
				SetupLeaderboard(leagueAllPrize, tourGroupList);
				GetBtnObj.SetActive(true);
			}
		});
	}

	public void SetupLeaderboard(List<ArenaPrizeSetupRecord> prizeData, List<ArenaPlayerDetail> playerList)
	{
		if (!isInited)
		{
			isInited = true;
			rankEntries.Add(sampleRankEntry);
			scrollHandle.offsetMin = new Vector2(scrollHandle.offsetMin.x, 0f);
			scrollHandle.offsetMax = new Vector2(scrollHandle.offsetMax.x, 0f);
		}
		int yourIndex = ArenaDialogHandler.GetYourIndex(playerList);
		if (!playerList[yourIndex].isYou)
		{
			playerList.Add(new ArenaPlayerDetail(userData.name, new List<int>
			{
				2
			}, new TimeSpan(0L), true, userData.countryCode));
		}
		ArenaDialogHandler.SortList(playerList);
		yourIndex = ArenaDialogHandler.GetYourIndex(playerList);
		int count = playerList.Count;
		for (int i = rankEntries.Count; i < count; i++)
		{
			ArenaRankEntryDirector tourRankEntryManager = UnityEngine.Object.Instantiate<ArenaRankEntryDirector>(sampleRankEntry, sampleRankEntry.transform.parent);
			tourRankEntryManager.transform.localPosition = sampleRankEntry.transform.localPosition + new Vector3(0f, (float)(-(float)i) * heightOfRankEntry, 0f);
			rankEntries.Add(tourRankEntryManager);
		}
		for (int j = 0; j < count; j++)
		{
			rankEntries[j].gameObject.SetActive(true);
		}
		for (int k = count; k < rankEntries.Count; k++)
		{
			rankEntries[k].gameObject.SetActive(false);
		}
		scrollContent.sizeDelta = new Vector2(scrollContent.sizeDelta.x, (float)playerList.Count * heightOfRankEntry + 50f);
		for (int l = 0; l < count; l++)
		{
			rankEntries[l].Init(playerList[l]);
		}
		for (int m = 1; m < count; m++)
		{
			rankEntries[m].transform.localPosition = sampleRankEntry.transform.localPosition + new Vector3(0f, (float)(-(float)m) * heightOfRankEntry, 0f);
		}
		Vector3 localPosition = scrollContent.localPosition;
		localPosition.y = heightOfRankEntry * (float)(yourIndex - 3);
		scrollContent.localPosition = localPosition;
		count = prizeData.Count;
		curReward = null;
		for (int n = 0; n < count; n++)
		{
			if (prizeData[n].Rankrangelower >= yourIndex + 1 && yourIndex + 1 >= prizeData[n].Rankrangeupper)
			{
				curReward = GameKit.GetTournamentRewardList(prizeData[n]);
				break;
			}
		}
		if (curReward == null)
		{
			curReward = GameKit.GetTournamentRewardList(prizeData[count - 1]);
		}
	}

	public void OnGetRewardBtnClicked()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		string userID = UserProfileStore.Instance.GetUserID();
		dictionary.Add(string.Format("Tournament/Users/{0}/{1}", userID, (tourRewardType != ArenaPrizeKind.SeasonReward) ? "recFriendReward" : "recSeasonReward"), lastSeasonNumber);
		NativeSpecificServicesSource.Services.DataCloudSaver.UpdateData(dictionary, null);
		for (int i = 0; i < curReward.Count; i++)
		{
			if (curReward[i].rewardType == PrizeKind.Gem)
			{
				PlayerCurrencyStore.Instance.ChangeGem(curReward[i].value, true);
			}
			else if (curReward[i].rewardType == PrizeKind.Item)
			{
				PowerUpItemStore.Instance.ChangeItemQuantity(curReward[i].itemID, curReward[i].value);
			}
		}
		if (MonoSingleton<LifespanSurface>.Instance.RewardPopupController == null)
		{
			UnityEngine.Debug.LogError("null reward popup");
		}
		MonoSingleton<LifespanSurface>.Instance.RewardPopupController.Init(curReward.ToArray());
		CloseWithScaleAnimation();
	}

	public GameObject GetBtnObj;

	public ArenaRankEntryDirector sampleRankEntry;

	public RectTransform scrollContent;

	public RectTransform scrollHandle;

	public float heightOfRankEntry = 90f;

	public GameObject loadingIcon;

	public Text seasonInfoText;

	private List<ArenaRankEntryDirector> rankEntries = new List<ArenaRankEntryDirector>();

	private ArenaPlayerSelfDetail userData;

	private List<PrizeItem> curReward;

	private int lastSeasonNumber;

	private ArenaPrizeKind tourRewardType;

	private bool isInited;
}
