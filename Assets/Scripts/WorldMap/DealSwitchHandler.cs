using System;
using LifetimePopup;
using UnityEngine;

namespace WorldMap
{
	public class DealSwitchHandler : SwitchHandler
	{
		public override void OnClick()
		{
			base.OnClick();
			MonoSingleton<LifespanSurface>.Instance.StorePopupController.Init();
			base.CustomInvoke(new Action(DoOpen), Time.deltaTime);
		}

		private void DoOpen()
		{
			MonoSingleton<LifespanSurface>.Instance.StorePopupController.TabsGroupController.InitSelectedTab(2);
			MonoSingleton<LifespanSurface>.Instance.StorePopupController.TabsGroupController.HighlightButton(2);
		}
	}
}
