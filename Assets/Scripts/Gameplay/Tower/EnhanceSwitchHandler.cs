using System;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class EnhanceSwitchHandler : ControlTowerButtonController
	{
		public int UltimateBranch
		{
			get
			{
				return ultimateBranch;
			}
		}

		private void Awake()
		{
			GetAllComponents();
		}

		private void GetAllComponents()
		{
			button = base.GetComponent<Button>();
			imageButton = base.GetComponent<Image>();
		}

		public void Init(TurretEntity towerModel, bool _isAllowedToUpgrade, int price)
		{
			this.towerModel = towerModel;
			isAllowedToUpgrade = _isAllowedToUpgrade;
			currentLevelPrice.text = price.ToString();
			if (!isAllowedToUpgrade)
			{
				LockButton();
			}
			else
			{
				UnLockButton();
			}
		}

		public void SetImageSprite(int towerID, int towerLevel, int ultimateBranch, int skillID, bool isAllowedToUpgrade)
		{
		}

		public void LockButton()
		{
			if (imageButton == null)
			{
				GetAllComponents();
			}
			imageButton.sprite = lockImage;
			button.enabled = false;
			imageButton.SetNativeSize();
			currentLevelPrice.gameObject.SetActive(false);
		}

		public void UnLockButton()
		{
			if (imageButton == null)
			{
				GetAllComponents();
			}
			if (towerModel.OriginalParameter.level < 2)
			{
				imageButton.sprite = normalImage;
			}
			if (towerModel.OriginalParameter.level == 2)
			{
				imageButton.sprite = Common.AssetLoader.Load<Sprite>(string.Format("TowerUltimateUpgradeIcon/ultimate_{0}_{1}", towerModel.Id, ultimateBranch));
			}
			button.enabled = true;
			imageButton.SetNativeSize();
			currentLevelPrice.gameObject.SetActive(true);
		}

		public override void OnClick()
		{
			base.OnClick();
			if (canUpgrade)
			{
				if (buttonStatus == GameplaySwitchHandler.ButtonStatus.Available)
				{
					OnClickAvailable();
				}
				else if (buttonStatus == GameplaySwitchHandler.ButtonStatus.Confirm)
				{
					OnConfirm();
				}
			}
		}

		protected override void OnClickAvailable()
		{
			base.OnClickAvailable();
			MonoSingleton<UIRootHandler>.Instance.UpgradeTowerPopupController.nextLevelInfomationPopoup.Init(UltimateBranch, towerModel.Id, towerModel.Level, towerModel.transform);
			TurretRangeHandler component = GameplayDirector.Instance.CurrentTowerRange.GetComponent<TurretRangeHandler>();
			component.target = towerModel.transform;
			if (towerModel != null)
			{
				if (isUltimateUpgrade)
				{
					component.SetRangeAttackMax((float)TowerParameterManager.Instance.GetRangeMax(towerModel.Id, towerModel.Level + 1 + ultimateBranch) / GameRecord.PIXEL_PER_UNIT);
				}
				else
				{
					component.SetRangeAttackMax((float)TowerParameterManager.Instance.GetRangeMax(towerModel.Id, towerModel.Level + 1) / GameRecord.PIXEL_PER_UNIT);
				}
			}
		}

		protected override void OnConfirm()
		{
			base.OnClick();
			MonoSingleton<UIRootHandler>.Instance.UpgradeTowerPopupController.OnUpgrade(UltimateBranch);
			MonoSingleton<UIRootHandler>.Instance.UpgradeTowerPopupController.nextLevelInfomationPopoup.Close();
		}

		public void UpdateStatusButton(CanEnhanceStanding canUpgradeStatus)
		{
			if (!isAllowedToUpgrade)
			{
				return;
			}
			if (canUpgradeStatus != CanEnhanceStanding.CanUpgrade)
			{
				if (canUpgradeStatus != CanEnhanceStanding.CannotUpgrade)
				{
					if (canUpgradeStatus == CanEnhanceStanding.MaxUpgrade)
					{
						MaxUpgrade();
					}
				}
				else
				{
					CannotUpgrade();
				}
			}
			else
			{
				CanUpgrade();
			}
		}

		private void MaxUpgrade()
		{
			base.GetComponent<Button>().enabled = false;
		}

		private void CanUpgrade()
		{
			canUpgrade = true;
			button.enabled = true;
			material.SetFloat("_EffectAmount", 0f);
			currentLevelPrice.color = Color.yellow;
		}

		private void CannotUpgrade()
		{
			canUpgrade = false;
			button.enabled = false;
			material.SetFloat("_EffectAmount", 1f);
			currentLevelPrice.color = Color.white;
		}

		private Image imageButton;

		private Button button;

		private bool canUpgrade;

		private bool isAllowedToUpgrade;

		private TurretEntity towerModel;

		[Space]
		[Header("General Variable")]
		[SerializeField]
		private bool isUltimateUpgrade;

		[SerializeField]
		private int ultimateBranch;

		[SerializeField]
		private Text currentLevelPrice;

		[SerializeField]
		private Sprite normalImage;

		[SerializeField]
		private Sprite lockImage;

		[Header("Image material")]
		[SerializeField]
		private Material material;
	}
}
