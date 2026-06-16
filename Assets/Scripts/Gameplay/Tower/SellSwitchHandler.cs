using System;
using UnityEngine;

namespace Gameplay
{
	public class SellSwitchHandler : ControlTowerButtonController
	{
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

		protected override void OnConfirm()
		{
			base.OnConfirm();
			MonoSingleton<UIRootHandler>.Instance.UpgradeTowerPopupController.OnSell();
			UISfxDirector.Instance.PlayClick();
			UnityEngine.Debug.Log("Confirm sell!");
		}
	}
}
