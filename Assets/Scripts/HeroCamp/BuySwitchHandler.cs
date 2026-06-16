using System;
using Data;
using LifetimePopup;
using Parameter;
using Services.PlatformSpecific;
using UnityEngine;
using UnityEngine.UI;

namespace HeroCamp
{
	public class BuySwitchHandler : SwitchHandler
	{
		public override void OnClick()
		{
			base.OnClick();
			SendEventClickButtonBuy();
			currentHeroID = HeroBarracksDialogHandler.Instance.currentHeroID;
			if (Singleton<UnlockHeroSpec>.Instance.IsHeroUnlockByGem(currentHeroID))
			{
				TryToBuyHeroByGem();
			}
			if (Singleton<UnlockHeroSpec>.Instance.IsHeroUnlockByRealMoney(currentHeroID))
			{
				TryToBuyHeroByRealMoney();
			}
		}

		public void InitBuyPriceGem(int price)
		{
			miniGemIcon.SetActive(true);
			buyValue_Gem.gameObject.SetActive(true);
			buyValue_Money.gameObject.SetActive(false);
			buyValue_Gem.text = price.ToString();
		}

		private void TryToBuyHeroByGem()
		{
			int currentGem = PlayerCurrencyStore.Instance.GetCurrentGem();
			int gemAmountToUnlockHero = Singleton<UnlockHeroSpec>.Instance.GetGemAmountToUnlockHero(currentHeroID);
			if (currentGem >= gemAmountToUnlockHero)
			{
				UnityEngine.Debug.Log("ÄÃ£ Ä‘á»§ Gem Ä‘á»ƒ mua, Process Buy");
				SendEventBuyHeroByGemCompleted();
				HeroStore.Instance.UnlockHero(currentHeroID);
				PlayerCurrencyStore.Instance.ChangeGem(-gemAmountToUnlockHero, true);
				HeroBarracksDialogHandler.Instance.UpgradeNBuyGroupController.RefreshStatus();
				HeroBarracksDialogHandler.Instance.ShowUnlockEffect(currentHeroID);
				HeroBarracksDialogHandler.Instance.ShowLevelUpEffect();
				UISfxDirector.Instance.PlayBuySuccess();
			}
			else
			{
				UnityEngine.Debug.Log("KhÃ´ng Ä‘á»§ Gem!");
				string notiContent = Singleton<AlertSynopsis>.Instance.GetNotiContent(20);
				MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init(notiContent, true, true);
			}
		}

		public void InitBuyPriceMoney()
		{
			miniGemIcon.SetActive(false);
			buyValue_Money.gameObject.SetActive(true);
			buyValue_Gem.gameObject.SetActive(false);
			currentHeroID = HeroBarracksDialogHandler.Instance.currentHeroID;
			string heroItemID = MonoSingleton<LifespanSurface>.Instance.StorePopupController.ShopItemLookup.GetHeroItemID(currentHeroID);
            decimal localizedProductPrice = NativeSpecificServicesSource.Services.InApPurchase.GetLocalizedProductPrice(heroItemID);
            string isocurrencyCode = NativeSpecificServicesSource.Services.InApPurchase.GetISOCurrencyCode(heroItemID);
            int noDecimalFracment = (int)BitConverter.GetBytes(decimal.GetBits(localizedProductPrice)[3])[2];
            buyValue_Money.text = NativeSpecificServicesSource.Services.InApPurchase.GetFormatedProductPrice(isocurrencyCode, localizedProductPrice, noDecimalFracment);
        }

		private void TryToBuyHeroByRealMoney()
		{
			currentHeroID = HeroBarracksDialogHandler.Instance.currentHeroID;
			string heroItemID = MonoSingleton<LifespanSurface>.Instance.StorePopupController.ShopItemLookup.GetHeroItemID(currentHeroID);
            //NativeSpecificServicesSource.Services.InApPurchase.PurchaseHero(heroItemID);
        }

		private void SendEventClickButtonBuy()
		{
            //NativeSpecificServicesSource.Services.Analytics.SendEvent_ClickButtonBuyHero();
        }

		private void SendEventBuyHeroByGemCompleted()
		{
			string heroName = Singleton<HeroSynopsis>.Instance.GetHeroName(currentHeroID);
			int currentGem = PlayerCurrencyStore.Instance.GetCurrentGem();
			int gemAmountToUnlockHero = Singleton<UnlockHeroSpec>.Instance.GetGemAmountToUnlockHero(currentHeroID);
			int currentGem2 = currentGem - gemAmountToUnlockHero;
			int maxMapIDUnlocked = MapProgressStore.Instance.GetMapIDUnlocked() + 1;
			int heroOwnedAmount = HeroStore.Instance.GetHeroOwnedAmount();
            //NativeSpecificServicesSource.Services.Analytics.SendEvent_BoughtHero(currentHeroID, heroName, currentGem, currentGem2, maxMapIDUnlocked, heroOwnedAmount);
        }

		[SerializeField]
		private Text buyValue_Gem;

		[SerializeField]
		private Text buyValue_Money;

		[SerializeField]
		private GameObject miniGemIcon;

		private int currentHeroID;
	}
}
