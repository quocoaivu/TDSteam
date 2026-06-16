using System;
using System.Collections.Generic;
using Data;
using GameCore;
using Services.PlatformSpecific;
using UnityEngine;
using UnityEngine.UI;

public class TestSetupMono : MonoSingleton<TestSetupMono>
{
    [Space]
    [Header("CURRENCY")]
    [SerializeField]
    private InputField inputFieldGem;

    [Space]
    [Header("CURRENCY")]
    [SerializeField]
    private InputField inputFieldStar;

    private int gemNumber;

    private int starsNumber;

    [Space]
    [Header("UNLOCK MAP")]
    [SerializeField]
    private Toggle toggleUnlockAllMap;

    private int unlockAllMap;

    [Space]
    [Header("UNLOCK THEME")]
    [SerializeField]
    private Toggle toggleUnlockAllTheme;

    private int unlockAllTheme;

    [Space]
    [Header("USE CUSTOM CSV")]
    [SerializeField]
    private Toggle toggleUseCustomCSV;

    private int useCustomCSV;

    [Space]
    [Header("DEFAULT TOURNAMENT MAP")]
    public InputField inputTourMap;

    private int tourDefaultMapId;

    public Toggle toggleTestSeasonReward;

    public Toggle toggleTestFriendReward;

    public Toggle toggleTestFriendRewardNoFakeUser;

    [Space]
    [Header("PUSH NOTIFY")]
    public InputField inputTitle;

    public InputField inputContent;

    public InputField inputDelaySecond;

    [Space]
    [Header("Unlock Hero")]
    public InputField inputHeroID;

    private const string keyTest = "TEST";

    private const string KeyGemNumber = "TESTgemNumber";

    private const string KeyStarsNumber = "TESTstarsNumber";

    private const string KeyUnlockAllMap = "TESTunlockAllMap";

    private const string KeyUnlockAllTheme = "TESTunlockAllTheme";

    private const string keyUseCustomCSV = "TESTcustomCSV";

    private const string keyDefaultTourMap = "TESTtourDefaultMap";
    
	public void Init()
	{
		LoadPlayerprefs();
		ApplyValue();
	}

	public void ApplyValue()
	{
		Singleton<TestSetup>.Instance.gemNumber = gemNumber;
		Singleton<TestSetup>.Instance.starsNumber = starsNumber;
		Singleton<TestSetup>.Instance.unlockAllMaps = (unlockAllMap == 1);
		Singleton<TestSetup>.Instance.unlockAllThemes = (unlockAllTheme == 1);
		Singleton<TestSetup>.Instance.useCustomCSV = (useCustomCSV == 1);
		Singleton<TestSetup>.Instance.tourDefaultMapId = tourDefaultMapId;
	}

	private void SaveToPlayerprefs()
	{
		PlayerPrefs.SetInt("TESTgemNumber", gemNumber);
		PlayerPrefs.SetInt("TESTstarsNumber", starsNumber);
		PlayerPrefs.SetInt("TESTunlockAllMap", unlockAllMap);
		PlayerPrefs.SetInt("TESTunlockAllTheme", unlockAllTheme);
		PlayerPrefs.SetInt("TESTcustomCSV", useCustomCSV);
		PlayerPrefs.SetInt("TESTtourDefaultMap", tourDefaultMapId);
	}

	private void LoadValueFromUI()
	{
		gemNumber = int.Parse(inputFieldGem.text);
		starsNumber = int.Parse(inputFieldStar.text);
		unlockAllMap = ((!toggleUnlockAllMap.isOn) ? 0 : 1);
		unlockAllTheme = ((!toggleUnlockAllTheme.isOn) ? 0 : 1);
		useCustomCSV = ((!toggleUseCustomCSV.isOn) ? 0 : 1);
		tourDefaultMapId = int.Parse(inputTourMap.text);
		GameKit.isTestingSeasonReward = toggleTestSeasonReward.isOn;
		GameKit.isTestingFriendReward = toggleTestFriendReward.isOn;
		GameKit.isTestingFriendRewardNoFakeUser = toggleTestFriendRewardNoFakeUser.isOn;
	}

	private void SynUiFromValue()
	{
		inputFieldGem.text = gemNumber.ToString();
		inputFieldStar.text = starsNumber.ToString();
		toggleUnlockAllMap.isOn = (unlockAllMap == 1);
		toggleUnlockAllTheme.isOn = (unlockAllTheme == 1);
		toggleUseCustomCSV.isOn = (useCustomCSV == 1);
		inputTourMap.text = tourDefaultMapId.ToString();
	}

	private void LoadPlayerprefs()
	{
		gemNumber = PlayerPrefs.GetInt("TESTgemNumber", 0);
		starsNumber = PlayerPrefs.GetInt("TESTstarsNumber", 0);
		unlockAllMap = PlayerPrefs.GetInt("TESTunlockAllMap", 0);
		unlockAllTheme = PlayerPrefs.GetInt("TESTunlockAllTheme", 0);
		useCustomCSV = PlayerPrefs.GetInt("TESTcustomCSV", 0);
		tourDefaultMapId = PlayerPrefs.GetInt("TESTtourDefaultMap", 0);
	}

	public void ClearFreeResoucesData()
	{
		FreeResourcesStore.Instance.ResetFreeResourcesDailyData();
	}

	public void SkipAllTutorial()
	{
		TutorialStore.Instance.SkipAllTutorials();
	}

	public void OnGotoTomorrowBtnClicked()
	{
		for (int i = 0; i < 3; i++)
		{
			SubscriptionType subId = (SubscriptionType)i;
			if (GameKit.IsSubscriptionActive(subId))
			{
				GameKit.SetEndSubscriptionTime(subId, GameKit.GetEndSubscriptionTime(subId).AddDays(-1.0));
				GameKit.SetLastTimeCheckInSubscription(subId, GameKit.GetLastTimeCheckInSubscription(subId).AddDays(-1.0));
			}
		}
		byte[] listRunningEvent = GameKit.GetListRunningEvent();
		for (int j = 0; j < listRunningEvent.Length; j++)
		{
			GameKit.WriteTimeStamp(GameKit.EVENT_ENDTIME_PREFIX + listRunningEvent[j], GameKit.ReadTimeStamp(GameKit.EVENT_ENDTIME_PREFIX + listRunningEvent[j]).AddDays(-1.0));
		}
		GameKit.SetDayOfYearUpdateEvent(GameKit.GetNow().AddDays(-1.0).DayOfYear);
		SignalQuestSystem.Instance.Init();
		NextDayDailyReward();
		UnityEngine.Debug.Log("you are on tomorrow");
	}

	public void OnUnsubcribeAllBtnClicked()
	{
		for (int i = 0; i < 3; i++)
		{
			SubscriptionType subId = (SubscriptionType)i;
			GameKit.SetEndSubscriptionTime(subId, GameKit.GetNow().AddDays(-1.0));
			GameKit.SetLastTimeCheckInSubscription(subId, GameKit.GetNow().AddDays(-1.0));
		}
	}

	public void OnCompleteAllEventBtnClicked()
	{
		List<RunningSignalRecord> listEvent = new List<RunningSignalRecord>();
		List<SignalSetupRecord> list = new List<SignalSetupRecord>();
		byte[] listRunningEvent = GameKit.GetListRunningEvent();
		List<int> listUnclaimedReward = GameKit.GetListUnclaimedReward();
		for (int i = 0; i < listUnclaimedReward.Count; i++)
		{
			list.Add(ConfigRegistry.Instance.eventIdToEventData[listUnclaimedReward[i]]);
		}
		for (int j = 0; j < listRunningEvent.Length; j++)
		{
			list.Add(ConfigRegistry.Instance.eventIdToEventData[(int)listRunningEvent[j]]);
		}
		GameKit.SaveListRunningEvent(listEvent);
		GameKit.SaveListUnclaimedReward(list);
		GameKit.SetDayOfYearUpdateEvent(GameKit.GetNow().AddDays(-1.0).DayOfYear);
		SignalQuestSystem.Instance.Init();
		UnityEngine.Debug.Log(">>>> complete all events");
	}

	public void OnAlmostCompleteEventBtnclicked()
	{
		List<RunningSignalRecord> list = new List<RunningSignalRecord>();
		byte[] listRunningEvent = GameKit.GetListRunningEvent();
		for (int i = 0; i < listRunningEvent.Length; i++)
		{
			RunningSignalRecord runningEventData = new RunningSignalRecord();
			runningEventData.configData = ConfigRegistry.Instance.eventIdToEventData[(int)listRunningEvent[i]];
			GameKit.GetRunningEventProgress(runningEventData);
			runningEventData.curProgress.Value = runningEventData.configData.Targetquantity - 1;
			list.Add(runningEventData);
		}
		GameKit.SaveListRunningEvent(list);
		GameKit.SetDayOfYearUpdateEvent(GameKit.GetNow().AddDays(-1.0).DayOfYear);
		SignalQuestSystem.Instance.Init();
		UnityEngine.Debug.Log(">>>>>> almost complete all events");
	}

	public void NextDayDailyReward()
	{
		DailyRewardStore.Instance.TryIncreaseDay();
	}

	public void PushNotify()
	{
		//NativeSpecificServicesSource.Services.GameNotification.PushNotify(inputContent.text, int.Parse(inputDelaySecond.text));
		GameUtils.ClearInputField(inputContent);
		GameUtils.ClearInputField(inputDelaySecond);
	}

	public void UnlockHero()
	{
		if (!string.IsNullOrEmpty(inputHeroID.text))
		{
			int num = int.Parse(inputHeroID.text);
			if (!HeroStore.Instance.IsHeroOwned(num))
			{
				HeroStore.Instance.UnlockHero(num);
				GameUtils.ClearInputField(inputHeroID);
				UnityEngine.Debug.Log("Test unlock hero ID = " + num);
			}
			else
			{
				UnityEngine.Debug.Log("Hero da duoc unlock!");
			}
		}
		else
		{
			UnityEngine.Debug.Log("Input field is empty!");
		}
	}

	public void Open()
	{
		base.gameObject.SetActive(true);
		LoadPlayerprefs();
		SynUiFromValue();
	}

	public void Close()
	{
		LoadValueFromUI();
		ApplyValue();
		SaveToPlayerprefs();
		base.gameObject.SetActive(false);
	}
}
