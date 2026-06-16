using System;
using Services.PlatformSpecific;
using UnityEngine;
using UnityEngine.UI;

public class DailyAmplifierShopItem : SalePackItem
{
	public override void Init()
	{
		base.Init();
		bool flag = GameKit.IsSubscriptionActive(SubscriptionType.dailyBooster);
		int subcribedur = bundleParam.Subcribedur;
		SubscriptionType subId = SubscriptionType.dailyBooster;
		purchaseButton.interactable = !flag;
		string arg = string.Empty;
		if (!flag)
		{
			arg = string.Format(GameKit.GetLocalization("DAYS"), subcribedur);
		}
		else
		{
			int days = (GameKit.GetEndSubscriptionTime(subId) - GameKit.GetNow()).Days;
			arg = string.Format(GameKit.GetLocalization("DAYS_LEFT"), days);
			currentPrice.text = "SUBSCRIBED";
		}
		title.text = string.Format("{0} - {1}", GameKit.GetLocalization(subId.ToString()), arg);
		if (subscribeId < 0)
		{
			subscribeId = GameKit.GetUniqueId();
		}
		GameSignalCenter.Instance.Unsubscribe(subscribeId, GameSignalKind.OnCompletePurchase);
		if (!flag)
		{
			GameSignalCenter.Instance.Subscribe(GameSignalKind.OnCompletePurchase, new SimpleListenerRecord(GameKit.GetUniqueId(), new GameSignalCenter.SimpleSubscribeMethod(Init)));
		}
	}

	public override void SetListItem()
	{
		int[] itemids = bundleParam.Itemids;
		int[] itemquatities = bundleParam.Itemquatities;
		int subcribedur = bundleParam.Subcribedur;
		PrizeItem[] array = new PrizeItem[itemids.Length];
		for (int i = 0; i < itemids.Length; i++)
		{
			array[i] = new PrizeItem(PrizeKind.Item, itemids[i], itemquatities[i] / subcribedur, true);
		}
		itemGroup.InitListItems(array);
	}

	public override void ProcessPurchase()
	{
		//NativeSpecificServicesSource.Services.InApPurchase.PurchaseSubscription(productID, SubscriptionType.dailyBooster);
	}

	public Button purchaseButton;

	public int subscribeId = -1;
}
