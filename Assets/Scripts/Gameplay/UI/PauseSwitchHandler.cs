using System;

namespace Gameplay
{
	public class PauseSwitchHandler : SwitchHandler
	{
		public override void OnClick()
		{
			base.OnClick();
			if (MonoSingleton<GameRecord>.Instance.IsGameOver)
			{
				return;
			}
			MonoSingleton<UIRootHandler>.Instance.settingPopupController.Open();
		}
	}
}
