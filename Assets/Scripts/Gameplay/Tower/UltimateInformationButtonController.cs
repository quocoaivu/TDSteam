using System;

namespace Gameplay
{
	public class UltimateInformationButtonController : SwitchHandler
	{
		public override void OnClick()
		{
			base.OnClick();
			MonoSingleton<UIRootHandler>.Instance.UpgradeTowerPopupController.ultimateInforGroup.TogglePopup();
		}
	}
}
