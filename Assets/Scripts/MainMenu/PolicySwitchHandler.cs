using System;

namespace MainMenu
{
	public class PolicySwitchHandler : SwitchHandler
	{
		public override void OnClick()
		{
			base.OnClick();
			MonoSingleton<UIRootHandler>.Instance.PolicyPopupController.Init();
		}
	}
}
