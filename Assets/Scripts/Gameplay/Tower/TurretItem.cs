using System;
using MetaGame;
using Parameter;
using Common;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class TurretItem : ControlTowerButtonController
	{
		private void Awake()
		{
			GetAllComponents();
		}

		private void GetAllComponents()
		{
			button = base.GetComponent<Button>();
			buttonImage = base.GetComponent<Image>();
		}

		public override void Init(bool _isAllowedToUse, Sprite spriteNormal, Sprite lockImage)
		{
			base.Init(_isAllowedToUse, spriteNormal, lockImage);
			isAllowedToUse = _isAllowedToUse;
			if (isAllowedToUse)
			{
				button.enabled = true;
				buttonImage.sprite = spriteNormal;
				buttonImage.SetNativeSize();
			}
			else
			{
				button.enabled = false;
				buttonImage.sprite = lockImage;
				buttonImage.SetNativeSize();
			}
			setPrice();
		}

		private void setPrice()
		{
			if (isAllowedToUse)
			{
				textPrice.text = TowerParameterManager.Instance.GetPrice(towerID, 0).ToString();
			}
			else
			{
				textPrice.text = string.Empty;
			}
		}

		public override void UpdateBuyState()
		{
			base.UpdateBuyState();
			bool flag = TowerParameterManager.Instance.GetPrice(towerID, 0) <= MonoSingleton<GameRecord>.Instance.Money;
			if (!button || !buttonImage)
			{
				GetAllComponents();
			}
			if (isAllowedToUse)
			{
				if (flag)
				{
					button.enabled = true;
					material.SetFloat("_EffectAmount", 0f);
					textPrice.color = Color.yellow;
				}
				else
				{
					button.enabled = false;
					material.SetFloat("_EffectAmount", 1f);
					textPrice.color = Color.white;
				}
			}
		}

		public override void OnClick()
		{
			base.OnClick();
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
			MonoSingleton<UIRootHandler>.Instance.UpgradeTowerPopupController.nextLevelInfomationPopoup.Init(-1, towerID, 0, MonoSingleton<ConstructSectorDirector>.Instance.listRegions[Setup.Instance.currentTowerRegionIDSelected].transform);
			TurretRangeHandler component = GameplayDirector.Instance.CurrentTowerRange.GetComponent<TurretRangeHandler>();
			component.target = MonoSingleton<ConstructSectorDirector>.Instance.listRegions[Setup.Instance.currentTowerRegionIDSelected].transform;
			component.SetRangeAttackMax((float)TowerParameterManager.Instance.GetRangeMax(towerID, 0) / GameRecord.PIXEL_PER_UNIT);
			onSelectToBuy.Dispatch();
		}

		protected override void OnConfirm()
		{
			base.OnClick();
			button.enabled = false;
			base.Invoke("doBuy", 0.01f);
		}

		private void doBuy()
		{
			TurretEntity towerModel = MonoSingleton<TowerPool>.Instance.GetTower(towerID, 0);
			towerModel.StartBuild(towerID, 0, Setup.Instance.currentTowerRegionIDSelected);
			towerModel.Appear();
			towerModel.transform.position = MonoSingleton<ConstructSectorDirector>.Instance.listRegions[Setup.Instance.currentTowerRegionIDSelected].transform.position;
			MonoSingleton<GameRecord>.Instance.DecreaseMoney(towerModel.OriginalParameter.buildCost);
			MonoSingleton<TurretControlSfxHandler>.Instance.PlayBuild(towerID);
			MonoSingleton<UIRootHandler>.Instance.BuyTowerPopupController.Close();
			MonoSingleton<ConstructSectorDirector>.Instance.listRegions[Setup.Instance.currentTowerRegionIDSelected].DisplayNotBuildable();
			onBuyTowerComplete.Dispatch();
		}

		[Space]
		[SerializeField]
		private OrderedUnityEvent onSelectToBuy;

		[SerializeField]
		private OrderedUnityEvent onBuyTowerComplete;

		[Space]
		[SerializeField]
		private int towerID;

		[SerializeField]
		private Text textPrice;

		private Button button;

		private Image buttonImage;

		private bool isAllowedToUse;

		[Header("Image material")]
		[SerializeField]
		private Material material;
	}
}
