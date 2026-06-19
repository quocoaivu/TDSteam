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

		public Items.StatType statType;

		public int statValue;

		public int rarity;

		public string icon;
	}
}
