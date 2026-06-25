namespace Items
{
	// One tower item the player holds during a run. Built from an ItemSpec (ItemFactory). An item only
	// fits the tower whose Id == towerID, and equipping it adds its stats to that tower via
	// TowerEquipment. Items are in-run (picked up in a match, reset between runs), so this is a plain
	// runtime object, not a persisted save. Instances are tracked by reference (the player may hold
	// several copies of the same spec).
	public class TowerItem
	{
		public int itemId;

		public int towerID;

		public string name;

		// Parallel arrays: up to 3 stats per item (matched by index).
		public StatType[] statTypes;

		public int[] statValues;

		public int rarity;

		public string icon;

		public TowerItem(int itemId, int towerID, string name, StatType[] statTypes, int[] statValues, int rarity, string icon)
		{
			this.itemId = itemId;
			this.towerID = towerID;
			this.name = name;
			this.statTypes = statTypes;
			this.statValues = statValues;
			this.rarity = rarity;
			this.icon = icon;
		}
	}
}
