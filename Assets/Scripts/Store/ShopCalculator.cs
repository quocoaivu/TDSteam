using System;
using Data;
using Parameter;

public static class ShopCalculator
{
	public static bool IsEnoughMoneyToBuy(int itemID)
	{
		int currentGem = PlayerCurrencyStore.Instance.GetCurrentGem();
		int price = Singleton<PowerUpItemSpec>.Instance.GetPrice(itemID);
		return currentGem >= price;
	}
}
