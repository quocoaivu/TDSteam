using System;
using Data;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace DailyTrial
{
	public class QuestHandler : MonoBehaviour
	{

        [SerializeField]
        private int missionTier;

        [SerializeField]
        private Image rewardAvatar;

        [SerializeField]
        private Text rewardQuantity;

        [SerializeField]
        private Text missionDescription;

        [SerializeField]
        private int missionNotifyID;

        [Space]
        [SerializeField]
        private GameObject rewardGroup;

        [SerializeField]
        private GameObject notifyDone;

        [SerializeField]
        private GameObject notifyUnDone;


        private DailyOrdealPrizeSpecs param;
        public void InitState()
		{
			int currentDayIndex = DailyTrialStore.Instance.GetCurrentDayIndex();
			int doneMissionTier = DailyTrialStore.Instance.GetDoneMissionTier(currentDayIndex);
			param = DailyOrdealSpec.Instance.GetRewardParameter(currentDayIndex, missionTier);
			if (doneMissionTier < 0)
			{
				ViewMissionUnDone();
			}
			else if (missionTier > doneMissionTier && doneMissionTier >= 0)
			{
				ViewMissionUnDone();
			}
			else
			{
				ViewMissionDone();
			}
		}

		private void ViewMissionDone()
		{
			rewardGroup.SetActive(false);
			notifyUnDone.SetActive(false);
			notifyDone.SetActive(true);
			missionDescription.text = Singleton<AlertSynopsis>.Instance.GetNotiContent(missionNotifyID) + " " + param.wave_require.ToString();
		}

		private void ViewMissionUnDone()
		{
			rewardGroup.SetActive(true);
			notifyUnDone.SetActive(true);
			notifyDone.SetActive(false);
			int num = 0;
			if (param.gem_bonus > 0)
			{
				num = param.gem_bonus;
				rewardAvatar.sprite = Common.AssetLoader.Load<Sprite>("LuckyChest/Items/lucky_item_gem");
			}
			if (param.item_freezing_bonus > 0)
			{
				num = param.item_freezing_bonus;
				rewardAvatar.sprite = Common.AssetLoader.Load<Sprite>("LuckyChest/Items/lucky_item_pw_0");
			}
			if (param.item_meteor_bonus > 0)
			{
				num = param.item_meteor_bonus;
				rewardAvatar.sprite = Common.AssetLoader.Load<Sprite>("LuckyChest/Items/lucky_item_pw_1");
			}
			if (param.item_healing_bonus > 0)
			{
				num = param.item_healing_bonus;
				rewardAvatar.sprite = Common.AssetLoader.Load<Sprite>("LuckyChest/Items/lucky_item_pw_2");
			}
			if (param.item_goldchest_bonus > 0)
			{
				num = param.item_goldchest_bonus;
				rewardAvatar.sprite = Common.AssetLoader.Load<Sprite>("LuckyChest/Items/lucky_item_pw_3");
			}
			rewardQuantity.text = num.ToString();
			missionDescription.text = Singleton<AlertSynopsis>.Instance.GetNotiContent(missionNotifyID) + " " + param.wave_require.ToString();
		}
	}
}
