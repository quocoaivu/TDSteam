using System;
using System.Collections.Generic;

namespace Parameter
{
	public class DailyOrdealSpec
	{
		public static DailyOrdealSpec Instance
		{
			get
			{
				if (DailyOrdealSpec.instance == null)
				{
					DailyOrdealSpec.instance = new DailyOrdealSpec();
				}
				return DailyOrdealSpec.instance;
			}
		}

		public void SetParameter(DailyOrdealSpecs param)
		{
			int count = listInputData.Count;
			if (count <= param.day)
			{
				listInputData.Add(param);
			}
		}

		private bool CheckParameter(int day)
		{
			return day < GetNumberOfParam();
		}

		public DailyOrdealSpecs GetParameter(int day)
		{
			if (CheckParameter(day))
			{
				return listInputData[day];
			}
			return default(DailyOrdealSpecs);
		}

		public int GetNumberOfParam()
		{
			return listInputData.Count;
		}

		public int[] getListInputItem(int day)
		{
			return new int[]
			{
				listInputData[day].input_pi_freezing,
				listInputData[day].input_pi_meteor,
				listInputData[day].input_pi_healing,
				listInputData[day].input_pi_goldchest,
				0,
				0,
				0,
				0,
				0
			};
		}

		public List<int> getListInputHeroesID(int day)
		{
			List<int> list = new List<int>();
			if (listInputData[day].input_hero_id_slot_0 >= 0)
			{
				list.Add(listInputData[day].input_hero_id_slot_0);
			}
			if (listInputData[day].input_hero_id_slot_1 >= 0)
			{
				list.Add(listInputData[day].input_hero_id_slot_1);
			}
			if (listInputData[day].input_hero_id_slot_2 >= 0)
			{
				list.Add(listInputData[day].input_hero_id_slot_2);
			}
			return list;
		}

		public bool IsTowerAllow(int day, int towerID)
		{
			return GetMaxLevelTowerCanUpgrade(day, towerID) >= 0;
		}

		public int GetMaxLevelTowerCanUpgrade(int day, int towerID)
		{
			int result = -1;
			switch (towerID)
			{
			case 0:
				result = listInputData[day].tower_level_max_0;
				break;
			case 1:
				result = listInputData[day].tower_level_max_1;
				break;
			case 2:
				result = listInputData[day].tower_level_max_2;
				break;
			case 3:
				result = listInputData[day].tower_level_max_3;
				break;
			case 4:
				result = listInputData[day].tower_level_max_4;
				break;
			}
			return result;
		}

		public string GetTitle(int day)
		{
			string empty = string.Empty;
			return GameKit.GetLocalization(string.Format(DailyOrdealSpec.DAILY_TRIAL_PREFIX, day));
		}

		public void SetRewardParameter(DailyOrdealPrizeSpecs reward)
		{
			int count = listRewardData.Count;
			if (count <= reward.day)
			{
				List<DailyOrdealPrizeSpecs> list = new List<DailyOrdealPrizeSpecs>();
				list.Insert(reward.reward_level, reward);
				listRewardData.Insert(reward.day, list);
			}
			else
			{
				List<DailyOrdealPrizeSpecs> list2 = listRewardData[reward.day];
				list2.Insert(reward.reward_level, reward);
			}
		}

		public DailyOrdealPrizeSpecs GetRewardParameter(int day, int rewardLevel)
		{
			if (CheckParameter(day, rewardLevel))
			{
				return listRewardData[day][rewardLevel];
			}
			return default(DailyOrdealPrizeSpecs);
		}

		private bool CheckParameter(int day, int rewardLevel)
		{
			return day < GetNumberOfRewardParam() && rewardLevel <= GetNumberOfLevelReward();
		}

		public int GetNumberOfRewardParam()
		{
			return listRewardData.Count;
		}

		public int GetNumberOfLevelReward()
		{
			if (GetNumberOfRewardParam() > 0)
			{
				return listRewardData[0].Count;
			}
			return 0;
		}

		public int[] GetListWaveRank(int day)
		{
			return new int[]
			{
				GetRewardParameter(day, 0).wave_require,
				GetRewardParameter(day, 1).wave_require,
				GetRewardParameter(day, 2).wave_require
			};
		}

		public int[] getListRewardValue(int day)
		{
			int[] array = new int[5];
			for (int i = 0; i < GetNumberOfLevelReward(); i++)
			{
				array[0] += GetRewardParameter(day, i).item_freezing_bonus;
				array[1] += GetRewardParameter(day, i).item_meteor_bonus;
				array[2] += GetRewardParameter(day, i).item_healing_bonus;
				array[3] += GetRewardParameter(day, i).item_goldchest_bonus;
				array[4] += GetRewardParameter(day, i).gem_bonus;
			}
			return array;
		}

		private static DailyOrdealSpec instance;

		[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			instance = null;
		}
		private static string DAILY_TRIAL_PREFIX = "TITLE_DAILY_TRIAL_{0}";

		public List<DailyOrdealSpecs> listInputData = new List<DailyOrdealSpecs>();

		public List<List<DailyOrdealPrizeSpecs>> listRewardData = new List<List<DailyOrdealPrizeSpecs>>();
	}
}
