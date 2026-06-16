using System;
using System.Collections.Generic;

namespace Data
{
	[Serializable]
	public class DailyPrizeSerializeRecord
	{
        public int currentDay;

        private List<DailyPrizeRecord> listDailyRewardData;

        public List<DailyPrizeRecord> ListDailyRewarData
		{
			get
			{
				return listDailyRewardData;
			}
			set
			{
				listDailyRewardData = value;
			}
		}
	}
}
