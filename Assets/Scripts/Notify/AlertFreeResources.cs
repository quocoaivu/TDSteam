using System;
using Data;

namespace Notify
{
	public class AlertFreeResources : AlertTrooper
	{
		protected override bool ShouldShowNotify()
		{
			return isFreeResoucesAvailable();
		}

		private bool isFreeResoucesAvailable()
		{
			return !FreeResourcesStore.Instance.IsUserGetReward_LogInFacebook() || !FreeResourcesStore.Instance.IsUserGetReward_LikeFanpage() || !FreeResourcesStore.Instance.IsUserGetReward_JoinGroup() || FreeResourcesStore.Instance.GetCurrentSharePerDay() > 0 || FreeResourcesStore.Instance.GetCurrentWatchAdsPerDay() > 0 || FreeResourcesStore.Instance.GetCurrentGemCollectedByInvite() < 100;
		}
	}
}
