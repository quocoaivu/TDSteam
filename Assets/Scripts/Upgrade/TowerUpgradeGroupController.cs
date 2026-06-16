using System;
using System.Collections.Generic;
using GameCore;

namespace Upgrade
{
	public class TowerUpgradeGroupController : BaseMonoBehaviour
	{
        public List<UpgradeTierButton> listTierUpgrade = new List<UpgradeTierButton>();
		public int currentUpgradeLevel;

        public void RefreshListTier()
		{
			for (int i = 0; i < listTierUpgrade.Count; i++)
			{
				if (i <= currentUpgradeLevel)
				{
					listTierUpgrade[i].ViewUpgraded();
				}
				else if (i == currentUpgradeLevel + 1)
				{
					listTierUpgrade[i].ViewCanUpgrade();
				}
				else
				{
					listTierUpgrade[i].ViewCannotUpgrade();
				}
			}
		}
	}
}
