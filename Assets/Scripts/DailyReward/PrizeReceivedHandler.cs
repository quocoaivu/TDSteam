using System;
using Data;
using Gameplay;
using Services.PlatformSpecific;
using UnityEngine;

namespace DailyReward
{
	public class PrizeReceivedHandler : GameplayDialogHandler
	{

        [SerializeField]
        private BonusReceivedHandler bonusReceivedController;

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

		public void BonusItem()
		{
			bool flag = DailyRewardStore.Instance.IsReceivedReward(currentDay);
			PrizeKind rewardType = this.rewardType;
			if (rewardType != PrizeKind.Gem)
			{
				if (rewardType == PrizeKind.Item)
				{
					if (!flag)
					{
						PowerUpItemStore.Instance.ChangeItemQuantity(rewardItemID, rewardItemQuantity);
						DailyRewardStore.Instance.SetReceiveRewardStatus(currentDay);
					}
				}
			}
			else if (!flag)
			{
				PlayerCurrencyStore.Instance.ChangeGem(rewardItemQuantity, true);
				DailyRewardStore.Instance.SetReceiveRewardStatus(currentDay);
			}
			//NativeSpecificServicesSource.Services.Ad.ShowOfferVideo(new OfferVideoCallback(OfferVideoCallback));
		}

		private void OfferVideoCallback(bool completed)
		{
			if (completed)
			{
				Hide();
				bonusReceivedController.Init(data, dailyRewardPopup);
			}
			else
			{
				dailyRewardPopup.CloseWithScaleAnimation();
			}
		}

		public void Claim()
		{
			if (DailyRewardStore.Instance == null)
			{
				UnityEngine.Debug.LogWarning("[PrizeReceivedHandler] Claim bá» qua: DailyRewardStore.Instance == null. CÃ³ thá»ƒ do popup init khÃ´ng hoÃ n táº¥t (Spine Star errors).");
				Hide();
				return;
			}
			bool flag = DailyRewardStore.Instance.IsReceivedReward(currentDay);
			PrizeKind rewardType = this.rewardType;
			if (rewardType != PrizeKind.Gem)
			{
				if (rewardType == PrizeKind.Item)
				{
					if (!flag)
					{
						PowerUpItemStore.Instance.ChangeItemQuantity(rewardItemID, rewardItemQuantity);
						DailyRewardStore.Instance.SetReceiveRewardStatus(currentDay);
					}
				}
			}
			else if (!flag)
			{
				PlayerCurrencyStore.Instance.ChangeGem(rewardItemQuantity, true);
				DailyRewardStore.Instance.SetReceiveRewardStatus(currentDay);
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
