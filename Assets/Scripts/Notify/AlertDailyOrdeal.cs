using System;
using Data;

namespace Notify
{
	public class AlertDailyOrdeal : AlertTrooper
	{
		protected override bool ShouldShowNotify()
		{
			return isFreeResoucesAvailable();
		}

		private bool isFreeResoucesAvailable()
		{
			int currentDayIndex = DailyTrialStore.Instance.GetCurrentDayIndex();
			return !DailyTrialStore.Instance.IsDoneMaxTierMission(currentDayIndex);
		}
	}
}
