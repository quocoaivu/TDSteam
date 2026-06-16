using System;
using Data;

namespace Upgrade
{
	public static class EnhanceStarCalculator
	{
		public static int GetCurrentStar()
		{
			int currentStar = PlayerCurrencyStore.Instance.GetCurrentStar();
			int num = 0;
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j <= GlobalUpgradeStore.Instance.GetCurrentUpgradeLevel(i); j++)
				{
					int starRequireForUpgrade = GlobalUpgradeStore.Instance.GetStarRequireForUpgrade(i, j);
					num += starRequireForUpgrade;
				}
			}
			return currentStar - num;
		}
	}
}
