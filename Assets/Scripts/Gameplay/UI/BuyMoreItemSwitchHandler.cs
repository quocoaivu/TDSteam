using System;
using LifetimePopup;

namespace Gameplay
{
	public class BuyMoreItemSwitchHandler : SwitchHandler
	{
		public override void OnClick()
		{
			base.OnClick();
			if (MonoSingleton<GameRecord>.Instance.IsGameOver)
			{
				return;
			}
			MonoSingleton<UIRootHandler>.Instance.settingPopupController.Open();
			MonoSingleton<LifespanSurface>.Instance.StorePopupController.Init();
			MonoSingleton<LifespanSurface>.Instance.StorePopupController.TabsGroupController.InitSelectedTab(1);
			MonoSingleton<LifespanSurface>.Instance.StorePopupController.TabsGroupController.HighlightButton(1);
		}
	}
}
