namespace Items
{
	// One tower item the player holds during a run. Built from an ItemSpec (ItemFactory). An item only
	// fits the tower whose Id == towerID, and equipping it adds statValue of statType to that tower via
	// TowerEquipment. Items are in-run (picked up in a match, reset between runs), so this is a plain
	// runtime object, not a persisted save. Instances are tracked by reference (the player may hold
	// several copies of the same spec).
	public class TowerItem
	{
		public int itemId;

		public int towerID;

		public string name;

		public StatType statType;

		public int statValue;

		public int rarity;

		public string icon;

		public TowerItem(int itemId, int towerID, string name, StatType statType, int statValue, int rarity, string icon)
		{
			this.itemId = itemId;
			this.towerID = towerID;
			this.name = name;
			this.statType = statType;
			this.statValue = statValue;
			this.rarity = rarity;
			this.icon = icon;
		}
	}
}
