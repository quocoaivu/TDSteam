using System;
using Data;
using UnityEngine;

namespace DailyReward
{
	public class DailyPrizeDialogHandler : GameplayPriorityDialogHandler
	{
        [SerializeField]
        private PrizeItemClusterHandler rewardItemGroupController;

        [SerializeField]
        private PrizeReceivedHandler rewardReceivedController;

        [SerializeField]
        private BonusReceivedHandler bonusReceivedController;

        public override void InitPriority(DialogPriorityEnum priority)
		{
			base.InitPriority(priority);
			Init();
		}

		public void Init()
		{
			rewardItemGroupController.gameObject.SetActive(false);
			rewardReceivedController.gameObject.SetActive(false);
			bonusReceivedController.gameObject.SetActive(false);
			int currentDay = DailyRewardStore.Instance.GetCurrentDay();
			bool flag = DailyRewardStore.Instance.IsReceivedReward(currentDay);
			DailyPrizeSetupRecord data = ConfigRegistry.Instance.dailyRewardConfig.dataArray[currentDay];
			if (flag)
			{
				RefreshRewardItemGroup();
			}
			else
			{
				rewardReceivedController.Init(data, this);
			}
		}

		public void RefreshRewardItemGroup()
		{
			rewardItemGroupController.Init();
			rewardItemGroupController.RefreshStatus();
		}

		public override void Open()
		{
			base.Open();
			base.gameObject.SetActive(true);
		}
	}
}
