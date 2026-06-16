using System;
using UnityEngine;

namespace Gameplay
{
	public class ControlTowerButtonController : GameplaySwitchHandler
	{
        [SerializeField]
        protected GameObject confirmImage;


        protected GameplaySwitchHandler.ButtonStatus buttonStatus;
        public virtual void Init(bool _isAllowedToUse, Sprite spriteNormal, Sprite lockImage)
		{
		}

		public override void OnClick()
		{
			base.OnClick();
			GameSignalCenter.Instance.Trigger(GameSignalKind.OnClickButton, new TappedObjectRecord(TappedObjectKind.TowerControlBtn));
		}

		protected virtual void OnClickAvailable()
		{
			buttonStatus = GameplaySwitchHandler.ButtonStatus.Confirm;
			confirmImage.SetActive(true);
			MonoSingleton<UIRootHandler>.Instance.UpgradeTowerPopupController.groupControllTowerButtons.DisableConfirmOtherButtons(this);
			MonoSingleton<UIRootHandler>.Instance.BuyTowerPopupController.groupControllTowerButtons.DisableConfirmOtherButtons(this);
		}

		protected virtual void OnConfirm()
		{
			confirmImage.SetActive(false);
			MonoSingleton<UIRootHandler>.Instance.UpgradeTowerPopupController.Close();
		}

		public virtual void DisableConfirm()
		{
			if (buttonStatus == GameplaySwitchHandler.ButtonStatus.Confirm)
			{
				buttonStatus = GameplaySwitchHandler.ButtonStatus.Available;
				confirmImage.SetActive(false);
			}
		}

		public virtual void UpdateBuyState()
		{
		}

	}
}
