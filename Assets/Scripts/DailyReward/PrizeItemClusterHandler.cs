using System;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;

namespace DailyReward
{
	public class PrizeItemClusterHandler : GameplayDialogHandler
	{
        [SerializeField]
        private List<DailyPrizeItem> listDailyRewardItem = new List<DailyPrizeItem>();

        public void Init()
		{
			Show();
			for (int i = 0; i < listDailyRewardItem.Count; i++)
			{
				DailyPrizeSetupRecord data = ConfigRegistry.Instance.dailyRewardConfig.dataArray[i];
				listDailyRewardItem[i].Init(i, data);
			}
		}

		public void RefreshStatus()
		{
			Show();
			for (int i = 0; i < listDailyRewardItem.Count; i++)
			{
				listDailyRewardItem[i].RefreshStatus();
			}
		}

		private void Show()
		{
			OpenWithScaleAnimation();
		}

		public void Hide()
		{
			CloseWithScaleAnimation();
		}
	}
}
