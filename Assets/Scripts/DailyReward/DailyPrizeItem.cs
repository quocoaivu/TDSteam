using System;
using Data;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace DailyReward
{
	public class DailyPrizeItem : MonoBehaviour
	{
        [SerializeField]
        private Text titleDay;

        [SerializeField]
        private Image icon;

        [SerializeField]
        private Text itemQuantity;

        [SerializeField]
        private GameObject selected;

        [SerializeField]
        private GameObject received;

        private int day;

        private PrizeKind rewardType;

        private int rewardItemID;

        private int rewardItemQuantity;

        private BonusKind bonusType;

        private int bonusItemID;

        private int bonusItemQuantity;

        private bool isReceived;

        public void Init(int day, DailyPrizeSetupRecord data)
		{
			this.day = day;
			rewardType = data.REWARDTYPE;
			rewardItemID = data.Rewardid;
			rewardItemQuantity = data.Rewardquantity;
			bonusType = data.BONUSTYPE;
			bonusItemID = data.Bonusid;
			bonusItemQuantity = data.Bonusquantity;
			SetView(day, rewardType, rewardItemID, rewardItemQuantity);
		}

		public void SetView(int day, PrizeKind rewardType, int itemID, int itemAmount)
		{
			if (titleDay)
			{
				titleDay.text = Singleton<AlertSynopsis>.Instance.GetNotiContent(53) + " " + (day + 1).ToString();
			}
			if (rewardType != PrizeKind.Gem)
			{
				if (rewardType == PrizeKind.Item)
				{
					icon.sprite = Common.AssetLoader.Load<Sprite>(string.Format("LuckyChest/Items/lucky_item_pw_{0}", itemID));
				}
			}
			else
			{
				icon.sprite = Common.AssetLoader.Load<Sprite>("LuckyChest/Items/lucky_item_gem");
			}
			itemQuantity.text = itemAmount.ToString();
		}

		public void SetView(int day, BonusKind bonusType, int itemID, int itemAmount)
		{
			if (titleDay)
			{
				titleDay.text = Singleton<AlertSynopsis>.Instance.GetNotiContent(53) + " " + (day + 1).ToString();
			}
			if (bonusType != BonusKind.Gem)
			{
				if (bonusType == BonusKind.Item)
				{
					icon.sprite = Common.AssetLoader.Load<Sprite>(string.Format("LuckyChest/Items/lucky_item_pw_{0}", itemID));
				}
			}
			else
			{
				icon.sprite = Common.AssetLoader.Load<Sprite>("LuckyChest/Items/lucky_item_gem");
			}
			itemQuantity.text = itemAmount.ToString();
		}

		public void RefreshStatus()
		{
			int currentDay = DailyRewardStore.Instance.GetCurrentDay();
			isReceived = DailyRewardStore.Instance.IsReceivedReward(day);
			ShowNotReceive();
			if (isReceived)
			{
				ShowReceiced();
				ShowHighlight();
			}
			else
			{
				ShowNotReceive();
			}
		}

		public void ShowHighlight()
		{
			if (selected)
			{
				selected.SetActive(true);
			}
		}

		public void ShowReceiced()
		{
			if (received)
			{
				received.SetActive(true);
			}
		}

		public void ShowNotReceive()
		{
			if (selected)
			{
				selected.SetActive(false);
			}
			if (received)
			{
				received.SetActive(false);
			}
		}
	}
}
