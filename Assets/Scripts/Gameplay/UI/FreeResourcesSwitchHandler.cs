using System;

namespace Gameplay
{
	public class FreeResourcesSwitchHandler : SwitchHandler
	{
		public override void OnClick()
		{
			base.OnClick();
			OpenPopup();
		}

		private void OpenPopup()
		{
			if (MonoSingleton<GameRecord>.Instance.IsGameOver)
			{
				return;
			}
			MonoSingleton<UIRootHandler>.Instance.freeResourcesPopupController.Init();
		}
	}
}
