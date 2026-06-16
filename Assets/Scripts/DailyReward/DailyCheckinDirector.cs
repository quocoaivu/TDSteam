using System;
using Data;
using Tutorial;
using UnityEngine;

public class DailyCheckinDirector : MonoBehaviour
{
    public static DailyCheckinDirector Instance { get; private set; }

    private bool firstTimeOfSession = true;

    private enum SpecialOfferCode
    {
        Starter,
        Trio,
        LandSky,
        TwoGods
    }
    
	private void Awake()
	{
		if (DailyCheckinDirector.Instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		DailyCheckinDirector.Instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public void WorldMapCheckIn(TutorialWorldMap worldMapTutorial)
	{
		if (worldMapTutorial.IsShowingTutorial())
		{
			return;
		}
		CheckDailyBooster();
		int mapIDUnlocked = MapProgressStore.Instance.GetMapIDUnlocked();
		if (mapIDUnlocked > 2)
		{
			CheckDailyReward();
			ShowSpecialOfferPopup();
		}
	}

	public void CheckDailyBooster()
	{
		DateTime dateTime = GameKit.GetLastTimeCheckInSubscription(SubscriptionType.dailyBooster);
		DateTime endSubscriptionTime = GameKit.GetEndSubscriptionTime(SubscriptionType.dailyBooster);
		if (!GameKit.IsSubscriptionActive(SubscriptionType.dailyBooster))
		{
			if ((GameKit.GetNow() - endSubscriptionTime).Days > 14)
			{
				return;
			}
			if ((GameKit.GetNow() - dateTime).Days > 14)
			{
				return;
			}
			if (dateTime.DayOfYear == endSubscriptionTime.DayOfYear || (dateTime - endSubscriptionTime).Minutes >= 0)
			{
				return;
			}
		}
		UnityEngine.Debug.LogFormat("__Check daily booster: lastCheckIn day {0}, today {1}", new object[]
		{
			dateTime.DayOfYear,
			GameKit.GetNow().DayOfYear
		});
		if (dateTime.DayOfYear == GameKit.GetNow().DayOfYear)
		{
			return;
		}
		int dayOfYear = GameKit.GetNow().DayOfYear;
		if ((endSubscriptionTime - GameKit.GetNow()).TotalMinutes < 0.0)
		{
			dayOfYear = endSubscriptionTime.DayOfYear;
		}
		int num = 0;
		while (num < 15 && dateTime.DayOfYear != dayOfYear)
		{
			num++;
			UnityEngine.Debug.LogFormat("___Counting daily booster day, lastcheck before {0} and after {1}, target {2}", new object[]
			{
				dateTime.DayOfYear,
				dateTime.AddDays(1.0).DayOfYear,
				dayOfYear
			});
			dateTime = dateTime.AddDays(1.0);
		}
		GameKit.SetLastTimeCheckInSubscription(SubscriptionType.dailyBooster, GameKit.GetNow());
		SalePackSetupRecord dataSaleBundle = ShopPackRecord.GetDataSaleBundle("kd.sale.bundle.dailybooster");
		int[] itemids = dataSaleBundle.Itemids;
		int[] itemquatities = dataSaleBundle.Itemquatities;
		int subcribedur = dataSaleBundle.Subcribedur;
		int days = (GameKit.GetEndSubscriptionTime(SubscriptionType.dailyBooster) - GameKit.GetNow()).Days;
		int num2 = dataSaleBundle.Subcribedur - days;
		string customTitle = string.Format("{0}\n<size=60%>Day {1} ({2})", GameKit.GetLocalization("dailyBooster"), num2, string.Format(GameKit.GetLocalization("DAYS_LEFT"), days));
		PrizeItem[] array = new PrizeItem[itemids.Length];
		for (int i = 0; i < itemids.Length; i++)
		{
			int num3 = itemquatities[i] / subcribedur * num;
			array[i] = new PrizeItem(PrizeKind.Item, itemids[i], num3, true);
			PowerUpItemStore.Instance.ChangeItemQuantity(itemids[i], num3);
		}
		PrizePriorityDialogHandler rewardPriorityPopupController = PriorityDialogDirector.Instance.CreatePopup(PriorityDialogDirector.Instance.rewardPopupPrefab, DialogPriorityEnum.Normal) as PrizePriorityDialogHandler;
		rewardPriorityPopupController.SetRewardData(array, customTitle);
	}

	public void CheckDailyReward()
	{
		int currentDay = DailyRewardStore.Instance.GetCurrentDay();
		bool flag = DailyRewardStore.Instance.IsReceivedReward(currentDay);
		if (flag)
		{
			return;
		}
		PriorityDialogDirector.Instance.CreatePopup(PriorityDialogDirector.Instance.dailyRewardPopupPrefab, DialogPriorityEnum.Normal);
	}

	public void ShowSpecialOfferPopup()
	{
		bool flag = false;
		if (GameKit.GetNow().DayOfYear != PlayerPrefs.GetInt("LastDayShowSpecialOffer"))
		{
			flag = true;
		}
		if (firstTimeOfSession)
		{
			flag = true;
		}
		if (!flag)
		{
			return;
		}
		PlayerPrefs.SetInt("LastDayShowSpecialOffer", GameKit.GetNow().DayOfYear);
		switch (SaleBundleStore.Instance.GetCurrentAvailableSpecialPackIndex())
		{
		case 0:
			PriorityDialogDirector.Instance.CreatePopup(PriorityDialogDirector.Instance.offerStarterPopup, DialogPriorityEnum.Normal);
			break;
		case 1:
			PriorityDialogDirector.Instance.CreatePopup(PriorityDialogDirector.Instance.offerTrioPopup, DialogPriorityEnum.Normal);
			break;
		case 2:
			PriorityDialogDirector.Instance.CreatePopup(PriorityDialogDirector.Instance.offerLandskyPopup, DialogPriorityEnum.Normal);
			break;
		case 3:
			PriorityDialogDirector.Instance.CreatePopup(PriorityDialogDirector.Instance.offerTwoGodPopup, DialogPriorityEnum.Normal);
			break;
		default:
			if (!GameKit.IsSubscriptionActive(SubscriptionType.dailyBooster) && firstTimeOfSession)
			{
				PriorityDialogDirector.Instance.CreatePopup(PriorityDialogDirector.Instance.dailyBoosterPopup, DialogPriorityEnum.Normal);
			}
			break;
		}
		firstTimeOfSession = false;
	}
}
