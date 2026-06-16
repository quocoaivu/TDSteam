using System;

public class DailyBoosterOfferPriorityPopupController : SpecialOfferPriorityPopupController
{
	public override void InitPriority(DialogPriorityEnum priority)
	{
		base.InitPriority(priority);
		subscribeId = GameKit.GetUniqueId();
		GameSignalCenter.Instance.Subscribe(GameSignalKind.OnCompletePurchase, new SimpleListenerRecord(GameKit.GetUniqueId(), new GameSignalCenter.SimpleSubscribeMethod(OnSomethingWasPurchased)));
	}

	private void OnSomethingWasPurchased()
	{
		if (GameKit.IsSubscriptionActive(SubscriptionType.dailyBooster))
		{
			CloseWithScaleAnimation();
		}
	}

	public override void Close()
	{
		base.Close();
		if (subscribeId > 0)
		{
			GameSignalCenter.Instance.Unsubscribe(subscribeId, GameSignalKind.OnCompletePurchase);
		}
		GameSignalCenter.Instance.Unsubscribe((saleBundleItem as DailyAmplifierShopItem).subscribeId, GameSignalKind.OnCompletePurchase);
	}

	private int subscribeId = -1;
}
