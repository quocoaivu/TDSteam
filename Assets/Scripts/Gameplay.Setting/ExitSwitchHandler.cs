using System;

namespace Gameplay.Setting
{
	public class ExitSwitchHandler : SwitchHandler
	{
		public override void OnClick()
		{
			base.OnClick();
			MonoSingleton<UIRootHandler>.Instance.settingPopupController.InitConfirmGroup(CancelGameKind.Quit);
		}
	}
}
