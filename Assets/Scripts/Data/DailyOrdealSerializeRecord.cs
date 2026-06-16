using System;
using System.Collections.Generic;

namespace Data
{
	[Serializable]
	public class DailyOrdealSerializeRecord
	{
        public int currentDay;

        private Dictionary<int, DailyOrdealRecord> listDailyTrialData;

        public Dictionary<int, DailyOrdealRecord> ListDailyTrialData
		{
			get
			{
				return listDailyTrialData;
			}
			set
			{
				listDailyTrialData = value;
			}
		}
	}
}
