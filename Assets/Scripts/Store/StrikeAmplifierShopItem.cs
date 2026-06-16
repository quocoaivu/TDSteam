using System;
using LifetimePopup;
using Services.PlatformSpecific;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StrikeAmplifierShopItem : SalePackItem
{
    public SubscriptionType subTypeEnum;

    public TextMeshProUGUI boosterDesc;

    public Button purchaseButton;

    private int subscribeId;

    public override void Init()
	{
		base.Init();
		int subcribedur = bundleParam.Subcribedur;
		int num = bundleParam.Itemquatities[0];
		bool flag = GameKit.IsSubscriptionActive(subTypeEnum);
		string arg = string.Empty;
		if (!flag)
		{
			arg = string.Format(GameKit.GetLocalization("DAYS"), subcribedur);
		}
		else
		{
			int days = (GameKit.GetEndSubscriptionTime(subTypeEnum) - GameKit.GetNow()).Days;
			if (days > 0)
			{
				arg = string.Format(GameKit.GetLocalization("DAYS_LEFT"), days);
			}
			else
			{
				arg = string.Format("{0}H left", Mathf.RoundToInt((float)(GameKit.GetEndSubscriptionTime(subTypeEnum) - GameKit.GetNow()).TotalHours));
			}
			currentPrice.text = "SUBSCRIBED";
		}
		title.text = string.Format("{0} - {1}", GameKit.GetLocalization(subTypeEnum.ToString()), arg);
		//boosterDesc.text = string.Format(GameKit.GetLocalization("ATTACK_BOOSTER_DESC"), subcribedur, num);
		purchaseButton.interactable = !flag;
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

	public override void ProcessPurchase()
	{
		if (GameKit.IsSubscriptionActive(SubscriptionType.doubleAttack) || GameKit.IsSubscriptionActive(SubscriptionType.fiftyPercentAtkBoost))
		{
			MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init(GameKit.GetLocalization("TWO_BOOSTER_WARNING"), "OK", null, null);
			return;
		}
		//NativeSpecificServicesSource.Services.InApPurchase.PurchaseSubscription(productID, subTypeEnum);
	}
}
