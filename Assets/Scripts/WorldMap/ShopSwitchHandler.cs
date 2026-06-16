using System;
using LifetimePopup;
using UnityEngine;

namespace WorldMap
{
	public class ShopSwitchHandler : SwitchHandler
	{
        [SerializeField]

        private float timeToOpen;

        public override void OnClick()
		{
			base.OnClick();
			base.CustomInvoke(new Action(DoOpen), timeToOpen);
		}

		private void DoOpen()
		{
			MonoSingleton<LifespanSurface>.Instance.StorePopupController.Init();
		}
	}
}
