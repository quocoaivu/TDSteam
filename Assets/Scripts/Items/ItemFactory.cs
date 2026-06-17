using Parameter;
using UnityEngine;

namespace Items
{
	// Builds TowerItem instances. Shared by drops (ItemDropRoller) and the shop (ItemShopPanel) so the
	// "random tower + slot -> ability name" rule lives in one place. Per-tower (Phase 6): branch is fixed
	// by the canonical prefab (Priest id 4 tops at L3 -> branch 0, others at L4 -> branch 1).
	public static class ItemFactory
	{
		public const int TOWER_COUNT = 5;

		public const int SLOT_COUNT = 2;

		public static TowerItem CreateRandom()
		{
			int towerID = Random.Range(0, TOWER_COUNT);
			int slotIndex = Random.Range(0, SLOT_COUNT);
			return Create(towerID, slotIndex, 0);
		}

		public static TowerItem Create(int towerID, int slotIndex, int level)
		{
			int branch = (towerID == 4) ? 0 : 1;
			string name = TurretAbilitySpec.Instance.GetSkillName(towerID, branch, slotIndex);
			return new TowerItem(towerID, slotIndex, level, name);
		}
	}
}
