using System;
using Data;
using LifetimePopup;
using Parameter;
using Services.PlatformSpecific;
using UnityEngine;
using UnityEngine.UI;

namespace HeroCamp.UltimateUpgrade
{
	public class EnhanceSwitchHandler : SwitchHandler
	{
		public void Init(MasteryEnhanceDialogHandler ultimateUpgradePopupController, int heroID)
		{
			this.ultimateUpgradePopupController = ultimateUpgradePopupController;
			this.heroID = heroID;
			upgradeValue.text = HeroGemCalculator.GetGemAmountToUnlockPet(heroID).ToString();
		}

		public override void OnClick()
		{
			base.OnClick();
			TryToUnlockPet();
		}

		private void TryToUnlockPet()
		{
			if (HeroStore.Instance.IsHeroOwned(heroID))
			{
				if (HeroStore.Instance.IsReachMaxLevel(heroID))
				{
					if (HeroGemCalculator.IsEnoughGemToUnlockPet(heroID))
					{
						UnlockPet();
					}
					else
					{
						string notiContent = Singleton<AlertSynopsis>.Instance.GetNotiContent(20);
						MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init(notiContent, true, true);
					}
				}
				else
				{
					string notiContent2 = Singleton<AlertSynopsis>.Instance.GetNotiContent(115);
					MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init(notiContent2, false, false);
				}
			}
			else
			{
				string notiContent3 = Singleton<AlertSynopsis>.Instance.GetNotiContent(116);
				MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init(notiContent3, false, false);
			}
		}

		private void UnlockPet()
		{
			int gemAmountToUnlockPet = HeroGemCalculator.GetGemAmountToUnlockPet(heroID);
			PlayerCurrencyStore.Instance.ChangeGem(-gemAmountToUnlockPet, true);
			HeroStore.Instance.UnlockPet(heroID);
			ultimateUpgradePopupController.UpdateUpgradeButtonState();
			ultimateUpgradePopupController.CastEffectUpgrade();
			HeroBarracksDialogHandler.Instance.RefreshHeroInformation();
			SendEvent_UnlockPet();
		}

		private void SendEvent_UnlockPet()
		{
			int petID = HeroParameterManager.Instance.GetPetID(heroID);
			int heroOwnedAmount = HeroStore.Instance.GetHeroOwnedAmount();
			int heroOwnPetAmount = HeroStore.Instance.GetHeroOwnPetAmount();
			string petName = GameKit.GetPetName(petID);
			//NativeSpecificServicesSource.Services.Analytics.SendEvent_UnlockPet(heroOwnedAmount, heroOwnPetAmount, petName);
		}

		public void Show()
		{
			base.gameObject.SetActive(true);
		}

		public void Hide()
		{
			base.gameObject.SetActive(false);
		}

		[SerializeField]
		private Text upgradeValue;

		private MasteryEnhanceDialogHandler ultimateUpgradePopupController;

		private int heroID;
	}
}
