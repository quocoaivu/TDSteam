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

		// Equips an item into ability slot `slotIndex` at power `level`, activating the matching ability.
		// Reuses the existing UnlockUltimate path (same as the retired mastery-purchase flow), so no
		// prefab/ability changes are needed.
		public void EquipItem(int slotIndex, int level)
		{
			if (slotIndex < 0 || slotIndex >= listTowerUltimate.Count)
			{
				return;
			}
			currentLevelUpgrade[slotIndex] = level;
			listTowerUltimate[slotIndex].UnlockUltimate(level);
		}

		// -1 = slot empty (no item equipped yet).
		public int GetEquippedLevel(int slotIndex)
		{
			if (slotIndex < 0 || slotIndex >= currentLevelUpgrade.Count)
			{
				return -1;
			}
			return currentLevelUpgrade[slotIndex];
		}

		public List<TurretMasteryShared> listTowerUltimate = new List<TurretMasteryShared>();

		public List<int> currentLevelUpgrade = new List<int>();
	}
}
