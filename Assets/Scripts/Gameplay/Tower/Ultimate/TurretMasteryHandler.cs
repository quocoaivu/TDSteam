using System;
using System.Collections.Generic;

namespace Gameplay
{
	public class TurretMasteryHandler : TurretHandler
	{
		public override void OnAppear()
		{
			base.OnAppear();
			ClearDataUpgrade();
			AddDefaultData();
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
			ClearDataUpgrade();
		}

		private void ClearDataUpgrade()
		{
			foreach (TurretMasteryShared towerUltimateCommon in listTowerUltimate)
			{
				towerUltimateCommon.OnReturnPool();
			}
			currentLevelUpgrade.Clear();
		}

		private void AddDefaultData()
		{
			foreach (TurretMasteryShared towerUltimateCommon in listTowerUltimate)
			{
				towerUltimateCommon.InitTowerModel(base.TowerModel);
			}
			currentLevelUpgrade.Add(-1);
			currentLevelUpgrade.Add(-1);
		}

		public List<TurretMasteryShared> listTowerUltimate = new List<TurretMasteryShared>();

		public List<int> currentLevelUpgrade = new List<int>();
	}
}
