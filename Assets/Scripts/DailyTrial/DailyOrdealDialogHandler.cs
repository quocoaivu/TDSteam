using System;
using Gameplay;
using UnityEngine;

namespace DailyTrial
{
	public class DailyOrdealDialogHandler : GameplayDialogHandler
	{
        [Space]
        [Header("Controllers")]
        [SerializeField]

        private DailyTabsClusterHandler dailyTabsGroupController;
        public void Init()
		{
			OpenWithScaleAnimation();
			dailyTabsGroupController.InitTabsData();
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
