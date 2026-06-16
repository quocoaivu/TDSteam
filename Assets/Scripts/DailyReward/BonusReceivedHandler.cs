using System;
using Data;
using Gameplay;
using UnityEngine;

namespace DailyReward
{
	public class BonusReceivedHandler : GameplayDialogHandler
	{
        [SerializeField]
        private DailyPrizeItem rewardItem;

        [SerializeField]
        private DailyPrizeItem bonusItem;

        private int currentDay;

        private DailyPrizeSetupRecord data;

        private PrizeKind rewardType;

        private int rewardItemID;

        private int rewardItemQuantity;

        private BonusKind bonusType;

        private int bonusItemID;

        private int bonusItemQuantity;

        private DailyPrizeDialogHandler dailyRewardPopup;

        public void Init(DailyPrizeSetupRecord data, DailyPrizeDialogHandler dailyRewardPopup)
		{
			this.dailyRewardPopup = dailyRewardPopup;
			Show();
			this.data = data;
			currentDay = DailyRewardStore.Instance.GetCurrentDay();
			rewardType = data.REWARDTYPE;
			rewardItemID = data.Rewardid;
			rewardItemQuantity = data.Rewardquantity;
			bonusType = data.BONUSTYPE;
			bonusItemID = data.Bonusid;
			bonusItemQuantity = data.Bonusquantity;
			rewardItem.SetView(0, rewardType, rewardItemID, rewardItemQuantity);
			bonusItem.SetView(0, bonusType, bonusItemID, bonusItemQuantity);
		}

		public void Claim()
		{
			bool flag = DailyRewardStore.Instance.IsReceivedBonus(currentDay);
			BonusKind bonusType = this.bonusType;
			if (bonusType != BonusKind.Gem)
			{
				if (bonusType == BonusKind.Item)
				{
					if (!flag)
					{
						PowerUpItemStore.Instance.ChangeItemQuantity(bonusItemID, bonusItemQuantity);
						DailyRewardStore.Instance.SetReceiveBonusStatus(currentDay);
					}
				}
			}
			else if (!flag)
			{
				PlayerCurrencyStore.Instance.ChangeGem(bonusItemQuantity, true);
				DailyRewardStore.Instance.SetReceiveBonusStatus(currentDay);
			}
			DailyRewardStore.Instance.SetReceiveRewardStatus(currentDay);
			Hide();
			dailyRewardPopup.RefreshRewardItemGroup();
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
