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

		// The skill this item unlocks (Hệ B): branch + skill id of a tower Ability. -1 = plain stat item.
		public int skillBranch;

		public int skillId;

		// True when this item unlocks a skill (vs. a plain stat item). Skill items activate the matching
		// Ability on equip; the tier comes from rarity.
		public bool IsSkillItem
		{
			get
			{
				return skillId >= 0;
			}
		}

		public TowerItem(int itemId, int towerID, string name, StatType[] statTypes, int[] statValues, int rarity, string icon, int skillBranch, int skillId)
		{
			this.itemId = itemId;
			this.towerID = towerID;
			this.name = name;
			this.statTypes = statTypes;
			this.statValues = statValues;
			this.rarity = rarity;
			this.icon = icon;
			this.skillBranch = skillBranch;
			this.skillId = skillId;
		}
	}
}
