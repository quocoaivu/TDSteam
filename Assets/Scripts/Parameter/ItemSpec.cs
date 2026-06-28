namespace Parameter
{
	// One row of tower_item_parameter.txt: the static definition of a tower item. Runtime TowerItem
	// instances are built from these specs by ItemFactory. Loaded by TowerDataLoader, held in
	// ItemSpecCatalog. Mirrors TurretAbilitySpecs / TowerSkillNode (plain struct data).
	public struct ItemSpec
	{
		public int itemId;

		public int towerId;

		public string name;

		// Up to 3 stats per item (stat_type / stat_type_2 / stat_type_3 columns in CSV).
		public Items.StatType[] statTypes;

		public int[] statValues;

		public int rarity;

		public string icon;

		// Skill this item unlocks (Hệ B). -1/-1 = a plain stat item (no skill). When set, equipping the
		// item activates the matching tower Ability at tier = rarity (see TowerEquipment / TurretMasteryHandler).
		public int skillBranch;

		public int skillId;
	}
}
