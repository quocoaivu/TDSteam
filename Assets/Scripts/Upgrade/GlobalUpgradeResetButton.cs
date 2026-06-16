using System;
using Data;
using Services.PlatformSpecific;

namespace Upgrade
{
	public class GlobalUpgradeResetButton : SwitchHandler
	{
		public override void OnClick()
		{
			base.OnClick();
			DoReset();
		}

		private void DoReset()
		{
			SendEventResetUpgrade();
			GlobalUpgradePopupController.Instance.Reset();
			GlobalUpgradeStore.Instance.OnStarChange(true);
		}

		private void SendEventResetUpgrade()
		{
			int currentStar = GlobalUpgradePopupController.Instance.currentStar;
			int currentStar2 = PlayerCurrencyStore.Instance.GetCurrentStar();
			//NativeSpecificServicesSource.Services.Analytics.SendEvent_ResetGlobalUpgrade(currentStar, currentStar2);
		}
	}
}
