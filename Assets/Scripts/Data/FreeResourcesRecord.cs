using System;

namespace Data
{
	[Serializable]
	public class FreeResourcesRecord
	{
		public bool isUserGetRewardLoggedInFacebook;

		public bool isUserGetRewardLikeFanpage;

		public bool isUserGetRewardJoinGroup;

		public int currentSharePerDay;

		public int currentWatchAdsPerDay;

		public int currentGemCollectedByInvite;
	}
}
