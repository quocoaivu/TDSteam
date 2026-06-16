using System;
using System.Collections.Generic;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class TurretAbilityEnhanceSwitchHandler : ControlTowerButtonController
	{
		private void Update()
		{
			UpdateStatusButton();
		}

		public void Init(TurretEntity towerModel)
		{
			this.towerModel = towerModel;
			towerUltimateController = towerModel.towerUltimateController;
			SetImageSprite();
			int num = towerUltimateController.currentLevelUpgrade[skillID];
			int ultimateBranchByLevel = TowerParameterManager.Instance.GetUltimateBranchByLevel(towerModel.Level);
			if (num < maxUpgrade)
			{
				upgradeCost = TurretAbilitySpec.Instance.GetUltimateSkillUpgradeCost(towerModel.Id, ultimateBranchByLevel, skillID, num + 1);
				if (towerModel.Id == 4)
				{
					currentLevelPriceHolder.SetActive(false);
				}
				else
				{
					currentLevelPriceHolder.SetActive(true);
				}
				currentLevelPrice.text = upgradeCost.ToString();
			}
			else
			{
				currentLevelPriceHolder.SetActive(false);
			}
			HideAllTier();
			DisplayUpgradedTier();
		}

		public void SetImageSprite()
		{
			ultimateBranch = TowerParameterManager.Instance.GetUltimateBranchByLevel(towerModel.Level);
			imageButton.overrideSprite = Common.AssetLoader.Load<Sprite>(string.Format("TowerUltimateUpgradeIcon/ultimate_{0}_{1}_{2}", towerModel.Id, ultimateBranch, skillID));
			if (towerModel.Id == 4)
			{
				button.enabled = false;
				imageButton.enabled = false;
			}
			else
			{
				button.enabled = true;
				imageButton.enabled = true;
			}
		}

		public override void OnClick()
		{
			base.OnClick();
			if (!canUpgrade)
			{
				return;
			}
			if (towerUltimateController.currentLevelUpgrade[skillID] == maxUpgrade)
			{
				UnityEngine.Debug.Log("Khong the nang cap them");
				return;
			}
			if (buttonStatus == GameplaySwitchHandler.ButtonStatus.Available)
			{
				OnClickAvailable();
			}
			else if (buttonStatus == GameplaySwitchHandler.ButtonStatus.Confirm)
			{
				OnConfirm();
			}
		}

		protected override void OnClickAvailable()
		{
			base.OnClickAvailable();
		}

		protected override void OnConfirm()
		{
			base.OnClick();
			List<int> currentLevelUpgrade= new List<int>();
			int index = skillID;
			(currentLevelUpgrade = towerUltimateController.currentLevelUpgrade)[index = skillID] = currentLevelUpgrade[index] + 1;
			MonoSingleton<GameRecord>.Instance.DecreaseMoney(upgradeCost);
			DisplayUpgradedTier();
			towerModel.towerUltimateController.listTowerUltimate[skillID].UnlockUltimate(towerUltimateController.currentLevelUpgrade[skillID]);
			MonoSingleton<UIRootHandler>.Instance.UpgradeTowerPopupController.Close();
			GameplayDirector.Instance.CurrentTowerRange.GetComponent<TurretRangeHandler>().HideRange();
		}

		private void HideAllTier()
		{
			if (towerModel.Id == 4)
			{
				tiersHolder.SetActive(false);
			}
			else
			{
				tiersHolder.SetActive(true);
			}
			for (int i = 0; i < ultimateTiers.Length; i++)
			{
				ultimateTiers[i].SetActive(false);
			}
		}

		private void DisplayUpgradedTier()
		{
			if (towerUltimateController.currentLevelUpgrade[skillID] < 0)
			{
				return;
			}
			for (int i = 0; i <= towerUltimateController.currentLevelUpgrade[skillID]; i++)
			{
				ultimateTiers[i].SetActive(true);
			}
		}

		public void UpdateStatusButton()
		{
			if (upgradeCost <= MonoSingleton<GameRecord>.Instance.Money)
			{
				CanUpgrade();
			}
			else
			{
				CannotUpgrade();
			}
		}

		private void CanUpgrade()
		{
			canUpgrade = true;
			material.SetFloat("_EffectAmount", 0f);
			currentLevelPrice.color = Color.yellow;
		}

		private void CannotUpgrade()
		{
			canUpgrade = false;
			material.SetFloat("_EffectAmount", 1f);
			currentLevelPrice.color = Color.white;
		}

		[SerializeField]
		private int skillID;

		private int ultimateBranch;

		private TurretEntity towerModel;

		private TurretMasteryHandler towerUltimateController;

		private int maxUpgrade = 2;

		private bool canUpgrade;

		private bool isAllowedToUpgrade;

		[Space]
		[Header("General Variable")]
		[SerializeField]
		private Text currentLevelPrice;

		[SerializeField]
		private GameObject currentLevelPriceHolder;

		private int upgradeCost;

		[SerializeField]
		private Button button;

		[SerializeField]
		private Image imageButton;

		[Space]
		[Header("Tiers")]
		[SerializeField]
		private GameObject tiersHolder;

		[SerializeField]
		private GameObject[] ultimateTiers = new GameObject[3];

		[Space]
		[Header("Image material")]
		[SerializeField]
		private Material material;
	}
}
