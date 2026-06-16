using System;
using System.Collections.Generic;

[Serializable]
public class PlayerRecord_DailyPrize
{
	public int currentDay;

	public List<PlayerRecord_DailyPrize_Unique> listDailyRewardData;
}
