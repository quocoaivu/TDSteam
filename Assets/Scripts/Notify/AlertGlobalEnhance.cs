using System;
using Data;
using Upgrade;

namespace Notify
{
	public class AlertGlobalEnhance : AlertTrooper
	{
		protected override bool ShouldShowNotify()
		{
			return isEnoughStarForNextUpgrade();
		}

		private bool isEnoughStarForNextUpgrade()
		{
			bool result = false;
			int[] array = new int[4];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = GlobalUpgradeStore.Instance.GetCurrentUpgradeLevel(i);
			}
			int currentStar = EnhanceStarCalculator.GetCurrentStar();
			for (int j = 0; j < 4; j++)
			{
				int starRequireForUpgrade = GlobalUpgradeStore.Instance.GetStarRequireForUpgrade(j, array[j]);
				if (currentStar >= starRequireForUpgrade)
				{
					result = true;
				}
			}
			return result;
		}
	}
}
