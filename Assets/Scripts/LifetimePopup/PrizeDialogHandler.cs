using System;
using Gameplay;
using RewardPopup;
using UnityEngine;

namespace LifetimePopup
{
	public class PrizeDialogHandler : GameplayDialogHandler
	{
        [Header("Controllers")]
        [SerializeField]
        private PrizeItemClusterHandler generalItemGroupController;

        public void Init(PrizeItem[] listData)
		{
			OpenWithScaleAnimation();
			base.transform.SetAsLastSibling();
			generalItemGroupController.InitListItems(listData);
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
