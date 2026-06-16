using System;
using UnityEngine;

namespace WorldMap
{
	public class PrimerSwitchHandler : SwitchHandler
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
			MonoSingleton<UIRootHandler>.Instance.guidePopupController.Init();
		}
	}
}
