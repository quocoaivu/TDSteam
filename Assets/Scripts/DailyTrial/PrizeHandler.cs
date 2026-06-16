using System;
using Data;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace DailyTrial
{
	public class PrizeHandler : MonoBehaviour
	{
        [SerializeField]
        private int missionTier;

        [SerializeField]
        private Animator animator;

        [SerializeField]
        private Image rewardAvatar;

        [SerializeField]
        private Text rewardQuantity;

        [SerializeField]
        private Text waveRank;

        private DailyOrdealPrizeSpecs param;


        private int currentDay;
        private void Start()
		{
			currentDay = DailyTrialStore.Instance.GetCurrentDayIndex();
			waveRank.text = DailyOrdealSpec.Instance.GetRewardParameter(currentDay, missionTier).wave_require.ToString();
		}

		public void InitRewardInfor()
		{
			currentDay = DailyTrialStore.Instance.GetCurrentDayIndex();
			int doneMissionTier = DailyTrialStore.Instance.GetDoneMissionTier(currentDay);
			if (missionTier > doneMissionTier)
			{
				ProcessReward();
			}
			else
			{
				animator.Play("OpenNoReward");
			}
		}

		private void ProcessReward()
		{
			animator.Play("OpenReward");
			param = DailyOrdealSpec.Instance.GetRewardParameter(currentDay, missionTier);
			int num = 0;
			if (param.gem_bonus > 0)
			{
				num = param.gem_bonus;
				PlayerCurrencyStore.Instance.ChangeGem(num, false);
				rewardAvatar.sprite = Common.AssetLoader.Load<Sprite>("LuckyChest/Items/lucky_item_gem");
			}
			if (param.item_freezing_bonus > 0)
			{
				num = param.item_freezing_bonus;
				PowerUpItemStore.Instance.ChangeItemQuantity(0, num);
				rewardAvatar.sprite = Common.AssetLoader.Load<Sprite>("LuckyChest/Items/lucky_item_pw_0");
			}
			if (param.item_meteor_bonus > 0)
			{
				num = param.item_meteor_bonus;
				PowerUpItemStore.Instance.ChangeItemQuantity(1, num);
				rewardAvatar.sprite = Common.AssetLoader.Load<Sprite>("LuckyChest/Items/lucky_item_pw_1");
			}
			if (param.item_healing_bonus > 0)
			{
				num = param.item_healing_bonus;
				PowerUpItemStore.Instance.ChangeItemQuantity(2, num);
				rewardAvatar.sprite = Common.AssetLoader.Load<Sprite>("LuckyChest/Items/lucky_item_pw_2");
			}
			if (param.item_goldchest_bonus > 0)
			{
				num = param.item_goldchest_bonus;
				PowerUpItemStore.Instance.ChangeItemQuantity(3, num);
				rewardAvatar.sprite = Common.AssetLoader.Load<Sprite>("LuckyChest/Items/lucky_item_pw_3");
			}
			rewardQuantity.text = num.ToString();
			DailyTrialStore.Instance.SetDoneMissionTier(currentDay, missionTier);
			UISfxDirector.Instance.PlayluckyChestSound();
		}
	}
}
