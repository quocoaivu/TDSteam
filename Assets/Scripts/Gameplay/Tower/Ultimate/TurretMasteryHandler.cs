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
				currentLevelUpgrade.Add(-1);
			}
		}

		// listTowerUltimate is wired in the prefab in (branch, skillId) order: [b0s0, b0s1, b1s0, b1s1].
		// So a skill's ability index is branch * 2 + skillId.
		private const int SKILLS_PER_BRANCH = 2;

		private int AbilityIndex(int branch, int skillId)
		{
			return branch * SKILLS_PER_BRANCH + skillId;
		}

		// Turns a skill on at the given tier (from the equipped skill-item's rarity). No-op if the prefab
		// doesn't have that ability wired yet (index out of range).
		public void ActivateSkill(int branch, int skillId, int tier)
		{
			int index = AbilityIndex(branch, skillId);
			if (index < 0 || index >= listTowerUltimate.Count)
			{
				return;
			}
			currentLevelUpgrade[index] = tier;
			listTowerUltimate[index].UnlockUltimate(tier);
		}

		// Turns a skill off (skill-item unequipped).
		public void DeactivateSkill(int branch, int skillId)
		{
			int index = AbilityIndex(branch, skillId);
			if (index < 0 || index >= listTowerUltimate.Count)
			{
				return;
			}
			currentLevelUpgrade[index] = -1;
			listTowerUltimate[index].LockUltimate();
		}

		// Forwarded once per tower attack so per-shot abilities can roll their effect. Inactive abilities
		// gate on their own unlock flag, so calling them all is safe.
		public void OnTowerAttack()
		{
			for (int i = 0; i < listTowerUltimate.Count; i++)
			{
				listTowerUltimate[i].OnTowerAttack();
			}
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
