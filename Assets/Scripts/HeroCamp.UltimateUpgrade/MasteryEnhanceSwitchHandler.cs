using System;

namespace HeroCamp.UltimateUpgrade
{
	public class MasteryEnhanceSwitchHandler : SwitchHandler
	{
		public override void OnClick()
		{
			base.OnClick();
			InitUltimateUpgradePopup();
		}

		private void InitUltimateUpgradePopup()
		{
			HeroBarracksDialogHandler.Instance.UltimateUpgradePopupController.Init();
		}

		public void Show()
		{
			base.gameObject.SetActive(true);
		}

		public void Hide()
		{
			base.gameObject.SetActive(false);
		}
	}
}
