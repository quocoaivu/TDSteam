using System;
using Data;
using UnityEngine;
using UnityEngine.UI;

namespace Upgrade
{
	public class GlobalUpgradeApplyButton : SwitchHandler
	{

        [SerializeField]
        private GlobalUpgradePopupController upgradePopupController;

        [SerializeField]
        private Image imageButton;

        [SerializeField]
        private Button button;

        private RGBToMono rGBToGrayscale;

        private void Awake()
		{
			rGBToGrayscale = base.GetComponent<RGBToMono>();
		}

		public override void OnClick()
		{
			base.OnClick();
			int starRequiredForUpgrade = GetStarRequiredForUpgrade();
			if (isEnoughStarToUpgrade(starRequiredForUpgrade))
			{
				DoUpgrade(starRequiredForUpgrade);
			}
		}

		private void DoUpgrade(int starRequired)
		{
			if (canUpgrade())
			{
				upgradePopupController.upgradeGroupControllers[upgradePopupController.currentTowerIDSelected].currentUpgradeLevel++;
				upgradePopupController.currentStar -= starRequired;
				upgradePopupController.upgradeGroupControllers[upgradePopupController.currentTowerIDSelected].RefreshListTier();
				GlobalUpgradeStore.Instance.Save(upgradePopupController.currentTowerIDSelected, upgradePopupController.upgradeGroupControllers[upgradePopupController.currentTowerIDSelected].currentUpgradeLevel);
				upgradePopupController.TryShowButtonUpgrade(false);
				upgradePopupController.CalculateCurrentStar();
				GlobalUpgradePopupController.Instance.ShowUpgradedEffect();
				UISfxDirector.Instance.PlayUpgradeSuccess();
				GlobalUpgradeStore.Instance.OnStarChange(true);
			}
		}

		public void RefreshButtonStatus()
		{
			int starRequiredForUpgrade = GetStarRequiredForUpgrade();
			if (isEnoughStarToUpgrade(starRequiredForUpgrade))
			{
				ViewCanUpgrade();
			}
			else
			{
				ViewCannotUpgrade();
			}
		}

		private int GetStarRequiredForUpgrade()
		{
			return GlobalUpgradeStore.Instance.GetStarRequireForUpgrade(upgradePopupController.currentTowerIDSelected, upgradePopupController.currentTierSelected);
		}

		private bool canUpgrade()
		{
			bool result = true;
			if (upgradePopupController.currentTowerIDSelected < 0)
			{
				UnityEngine.Debug.Log("ChÆ°a chá»n tower!");
				result = false;
			}
			if (upgradePopupController.upgradeGroupControllers[upgradePopupController.currentTowerIDSelected].currentUpgradeLevel == 4)
			{
				UnityEngine.Debug.Log("ÄÃ£ nÃ¢ng cáº¥p max!");
				result = false;
			}
			return result;
		}

		private bool isEnoughStarToUpgrade(int starRequired)
		{
			return upgradePopupController.currentStar >= starRequired;
		}

		private void ViewCanUpgrade()
		{
			button.enabled = true;
			if (!rGBToGrayscale)
			{
				rGBToGrayscale = base.GetComponent<RGBToMono>();
			}
			rGBToGrayscale.SwitchToRGB();
		}

		private void ViewCannotUpgrade()
		{
			button.enabled = false;
			if (!rGBToGrayscale)
			{
				rGBToGrayscale = base.GetComponent<RGBToMono>();
			}
			rGBToGrayscale.SwitchToGrayscale();
		}
	}
}
