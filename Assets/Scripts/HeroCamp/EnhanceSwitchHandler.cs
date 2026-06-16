using System;
using Data;
using LifetimePopup;
using Parameter;
using Services.PlatformSpecific;
using UnityEngine;
using UnityEngine.UI;

namespace HeroCamp
{
	public class EnhanceSwitchHandler : SwitchHandler
	{
		public void InitUpgradePrice(int price)
		{
			upgradePriceText.text = price.ToString();
		}

		public override void OnClick()
		{
			base.OnClick();
			currentHeroID = HeroBarracksDialogHandler.Instance.currentHeroID;
			if (HeroStore.Instance.IsReachMaxLevel(currentHeroID))
			{
				UnityEngine.Debug.Log("KhÃ´ng thá»ƒ nÃ¢ng cáº¥p thÃªm");
				return;
			}
			if (HeroGemCalculator.IsEnoughGemToUpgrade(currentHeroID))
			{
				LevelUpHero();
			}
			else
			{
				UnityEngine.Debug.Log("KhÃ´ng Ä‘á»§ Gem!");
				string notiContent = Singleton<AlertSynopsis>.Instance.GetNotiContent(20);
				MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init(notiContent, true, true);
			}
		}

		private void LevelUpHero()
		{
			SendEventUpgradeHeroLevel();
			int gemAmountToLevelUp = HeroGemCalculator.GetGemAmountToLevelUp(currentHeroID);
			PlayerCurrencyStore.Instance.ChangeGem(-gemAmountToLevelUp, true);
			HeroStore.Instance.LevelUp(currentHeroID);
			HeroBarracksDialogHandler.Instance.HeroLevelInformation.DisplayLevelUpHero();
			if (HeroStore.Instance.IsReachMaxLevel(currentHeroID))
			{
				SetMaxLevel();
			}
			UISfxDirector.Instance.PlayUpgradeSuccess();
			HeroStore.Instance.OnLevelChange(true);
		}

		private void SendEventUpgradeHeroLevel()
		{
			string heroName = Singleton<HeroSynopsis>.Instance.GetHeroName(currentHeroID);
			int currentGem = PlayerCurrencyStore.Instance.GetCurrentGem();
			int gemAmountToLevelUp = HeroGemCalculator.GetGemAmountToLevelUp(currentHeroID);
			int currentGem2 = currentGem - gemAmountToLevelUp;
			int currentHeroLevel = HeroStore.Instance.GetCurrentHeroLevel(currentHeroID) + 2;
			int maxMapIDUnlocked = MapProgressStore.Instance.GetMapIDUnlocked() + 1;
			int heroOwnedAmount = HeroStore.Instance.GetHeroOwnedAmount();
			//NativeSpecificServicesSource.Services.Analytics.SendEvent_UpgradeHeroLevel(heroName, currentGem, currentGem2, currentHeroLevel, maxMapIDUnlocked, heroOwnedAmount);
		}

		public void SetNormal()
		{
			button.enabled = true;
			upgradePriceText.enabled = true;
			statusText.gameObject.SetActive(true);
			maxLevelText.gameObject.SetActive(false);
			statusText.text = Singleton<AlertSynopsis>.Instance.GetNotiContent(9);
			imageButton.sprite = normalSprite;
			miniGem.SetActive(true);
		}

		public void SetMaxLevel()
		{
			button.enabled = false;
			upgradePriceText.enabled = false;
			statusText.gameObject.SetActive(false);
			maxLevelText.gameObject.SetActive(true);
			maxLevelText.text = Singleton<AlertSynopsis>.Instance.GetNotiContent(16);
			imageButton.sprite = maxLevelsprite;
			miniGem.SetActive(false);
		}

		[SerializeField]
		private Text statusText;

		[SerializeField]
		private Text maxLevelText;

		[SerializeField]
		private Text upgradePriceText;

		[SerializeField]
		private GameObject miniGem;

		[SerializeField]
		private Sprite normalSprite;

		[SerializeField]
		private Sprite maxLevelsprite;

		[SerializeField]
		private Button button;

		[SerializeField]
		private Image imageButton;

		private int currentHeroID;
	}
}
