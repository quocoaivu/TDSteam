namespace Items
{
	// One tower item the player holds during a run. Per-tower (Phase 6): an item only fits the tower
	// whose canonical prefab physically carries the matching ability. slotIndex indexes
	// TurretMasteryHandler.listTowerUltimate (== skillID); level (0..2) is the power tier passed to
	// UnlockUltimate. Items are in-run (picked up in a match, reset between runs), so this is a plain
	// runtime object, not a persisted save.
	public class TowerItem
	{
		public int towerID;

		public int slotIndex;

		public int level;

		public string name;

		public TowerItem(int towerID, int slotIndex, int level, string name)
		{
			this.towerID = towerID;
			this.slotIndex = slotIndex;
			this.level = level;
			this.name = name;
		}
	}
}
