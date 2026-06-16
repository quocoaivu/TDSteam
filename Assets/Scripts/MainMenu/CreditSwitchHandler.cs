using System;

namespace MainMenu
{
	public class CreditSwitchHandler : SwitchHandler
	{
		public override void OnClick()
		{
			base.OnClick();
			MonoSingleton<UIRootHandler>.Instance.CreditPopupController.Init();
		}
	}
}
