using System;
using System.Collections.Generic;
using DailyReward;
using LifetimePopup;
using UnityEngine;

public class PriorityDialogDirector : MonoBehaviour
{
    [Space]
    [Header("UI parent")]
    public Transform popupParent;

    [Header("Priority popup prefab")]
    public PrizePriorityDialogHandler rewardPopupPrefab;

    public SpecialOfferPriorityPopupController offerStarterPopup;

    public SpecialOfferPriorityPopupController offerTrioPopup;

    public SpecialOfferPriorityPopupController offerLandskyPopup;

    public SpecialOfferPriorityPopupController offerTwoGodPopup;

    public SpecialOfferPriorityPopupController dailyBoosterPopup;

    public DailyPrizeDialogHandler dailyRewardPopupPrefab;

    public AskToRateDialogHandler ratePopupPrefab;

    public static PriorityDialogDirector Instance;

    private List<GameplayPriorityDialogHandler> popupQueue = new List<GameplayPriorityDialogHandler>();

    private GameplayPriorityDialogHandler currentPopup;


    private void Awake()
	{
		if (PriorityDialogDirector.Instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		PriorityDialogDirector.Instance = this;
		base.transform.SetParent(null);
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		Initialization();
	}

	private void Initialization()
	{
	}

	public GameplayPriorityDialogHandler CreatePopup(GameplayPriorityDialogHandler popupPrefab, DialogPriorityEnum priority)
	{
		GameplayPriorityDialogHandler gameplayPriorityPopupController = ObjectCache.Spawn<GameplayPriorityDialogHandler>(popupPrefab, popupParent, Vector3.zero);
		gameplayPriorityPopupController.InitPriority(priority);
		return gameplayPriorityPopupController;
	}

	public void AddPopup(GameplayPriorityDialogHandler popup)
	{
		DialogPriorityEnum priority = popup.priority;
		if (priority != DialogPriorityEnum.Normal)
		{
			if (priority == DialogPriorityEnum.Highest)
			{
				popupQueue.Insert(0, popup);
			}
		}
		else
		{
			popupQueue.Add(popup);
		}
		popup.gameObject.SetActive(false);
		TryShowNextPopupInQueue();
	}

	public void RemoveCurrentPopup(GameplayPriorityDialogHandler popup)
	{
		if (popup != currentPopup)
		{
			return;
		}
		currentPopup = null;
		TryShowNextPopupInQueue();
	}

	private void TryShowNextPopupInQueue()
	{
		if (currentPopup != null)
		{
			return;
		}
		if (popupQueue.Count <= 0)
		{
			return;
		}
		currentPopup = popupQueue[0];
		popupQueue.RemoveAt(0);
		currentPopup.OpenWithScaleAnimation();
	}
}
