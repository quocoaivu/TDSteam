using System;
using Data;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace HeroCamp
{
	public class EnhanceNBuyClusterHandler : MonoBehaviour
	{
		private void OnEnable()
		{
			if (PlayerCurrencyStore.Instance != null)
			{
				PlayerCurrencyStore.Instance.OnGemChangeEvent += Instance_OnGemChangeEvent;
			}
			UpdateGemAmount();
		}

		private void OnDisable()
		{
			if (PlayerCurrencyStore.Instance != null)
			{
				PlayerCurrencyStore.Instance.OnGemChangeEvent -= Instance_OnGemChangeEvent;
			}
		}

		private void Instance_OnGemChangeEvent()
		{
			UpdateGemAmount();
		}

		private void UpdateGemAmount()
		{
			totalGemText.text = PlayerCurrencyStore.Instance.GetCurrentGem().ToString();
		}

		public void RefreshStatus()
		{
			currentHeroID = HeroBarracksDialogHandler.Instance.currentHeroID;
			HideAllButton();
			if (HeroStore.Instance.IsHeroOwned(currentHeroID))
			{
				UpdateUpgradeButtonStatus();
				RefreshUpgradePrice();
			}
			else if (Singleton<UnlockHeroSpec>.Instance.IsHeroUnlockByGem(currentHeroID))
			{
				buyButtonController.gameObject.SetActive(true);
				buyButtonController.InitBuyPriceGem(Singleton<UnlockHeroSpec>.Instance.GetGemAmountToUnlockHero(currentHeroID));
			}
			else if (Singleton<UnlockHeroSpec>.Instance.IsHeroUnlockByRealMoney(currentHeroID))
			{
				buyButtonController.gameObject.SetActive(true);
				buyButtonController.InitBuyPriceMoney();
			}
			else
			{
				lockButton.SetActive(true);
			}
		}

		private void HideAllButton()
		{
			upgradeButtonController.gameObject.SetActive(false);
			buyButtonController.gameObject.SetActive(false);
			lockButton.SetActive(false);
		}

		private void RefreshUpgradePrice()
		{
			upgradeButtonController.InitUpgradePrice(HeroGemCalculator.GetGemAmountToLevelUp(currentHeroID));
		}

		private void UpdateUpgradeButtonStatus()
		{
			upgradeButtonController.gameObject.SetActive(true);
			if (HeroStore.Instance.IsReachMaxLevel(currentHeroID))
			{
				upgradeButtonController.SetMaxLevel();
			}
			else
			{
				upgradeButtonController.SetNormal();
			}
		}

		[Space]
		[Header("Upgrade Button")]
		[SerializeField]
		private EnhanceSwitchHandler upgradeButtonController;

		[Space]
		[Header("Buy Button")]
		[SerializeField]
		private BuySwitchHandler buyButtonController;

		[Space]
		[Header("Lock Button")]
		[SerializeField]
		private GameObject lockButton;

		[Space]
		[Header("Total Gem")]
		[SerializeField]
		private Text totalGemText;

		private int currentHeroID;
	}
}
