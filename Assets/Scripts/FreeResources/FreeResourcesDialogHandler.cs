using System;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;

namespace FreeResources
{
	public class FreeResourcesDialogHandler : GameplayDialogHandler
	{
        [Space]
        [SerializeField]

        private List<FreeResourcesSwitchHandler> listFreeResourcesButton = new List<FreeResourcesSwitchHandler>();
        public void Init()
		{
			OpenWithScaleAnimation();
			base.transform.SetAsLastSibling();
			foreach (FreeResourcesSwitchHandler freeResourcesButtonController in listFreeResourcesButton)
			{
				freeResourcesButtonController.InitData();
			}
		}

		public override void OpenWithScaleAnimation()
		{
			base.OpenWithScaleAnimation();
		}

		public override void CloseWithScaleAnimation()
		{
			base.CloseWithScaleAnimation();
		}
	}
}
