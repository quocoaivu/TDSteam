using System;
using Data;
using UnityEngine;
using UnityEngine.UI;

namespace Upgrade
{
	public class UpgradeTierButton : SwitchHandler
	{
        [Space]
        [Header("Parent")]
        [SerializeField]
        private TowerUpgradeGroupController groupController;

        [Space]
        [SerializeField]
        private int towerID;

        [SerializeField]
        private int tierID;

        [SerializeField]
        private int upgradeAbilityID;

        [Space]
        [Header("Star Required")]
        [SerializeField]
        private Text starRequire;

        [SerializeField]
        private GameObject starHolder;

        [Space]
        [Header("Upgraded Image")]
        [SerializeField]
        private GameObject upgradedImage;

        private Image image;

        [Header("Image material")]
        [SerializeField]
        private Material material;


        private void Awake()
		{
			image = base.GetComponent<Image>();
		}

		private void Start()
		{
			starRequire.text = GlobalUpgradeStore.Instance.GetStarRequireForUpgrade(towerID, tierID).ToString();
		}

		public override void OnClick()
		{
			base.OnClick();
			GlobalUpgradePopupController.Instance.ShowSelectedUpgradeImage(base.transform.position);
			GlobalUpgradePopupController.Instance.upgradeInformationController.InitData(image.sprite, tierID, upgradeAbilityID, towerID);
			GlobalUpgradePopupController.Instance.currentTowerIDSelected = towerID;
			GlobalUpgradePopupController.Instance.currentTierSelected = tierID;
			if (tierID <= groupController.currentUpgradeLevel)
			{
				GlobalUpgradePopupController.Instance.TryShowButtonUpgrade(false);
			}
			else if (tierID == groupController.currentUpgradeLevel + 1)
			{
				GlobalUpgradePopupController.Instance.TryShowButtonUpgrade(true);
			}
			else
			{
				GlobalUpgradePopupController.Instance.TryShowButtonUpgrade(false);
			}
		}

		public void InitDefaultData()
		{
			if (image == null)
			{
				image = base.GetComponent<Image>();
			}
			GlobalUpgradePopupController.Instance.upgradeInformationController.InitData(image.sprite, tierID, upgradeAbilityID, towerID);
		}

		public void ViewCanUpgrade()
		{
			material.SetFloat("_EffectAmount", 0f);
			upgradedImage.SetActive(false);
			starHolder.SetActive(true);
		}

		public void ViewCannotUpgrade()
		{
			material.SetFloat("_EffectAmount", 1f);
			upgradedImage.SetActive(false);
			starHolder.SetActive(true);
		}

		public void ViewUpgraded()
		{
			material.SetFloat("_EffectAmount", 0f);
			upgradedImage.SetActive(true);
			starHolder.SetActive(false);
		}
	}
}
