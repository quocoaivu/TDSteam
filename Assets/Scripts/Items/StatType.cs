namespace Items
{
	// The stat an item adds to its tower. Each maps to one shared buff key in TowerEquipment.
	// Damage/AttackSpeed/Crit/Range fit attacking towers; Health/Armor fit the Knights barracks
	// (applied to spawned units); GoldProduce fits the Supporter; Slow adds slow-on-hit to a
	// projectile tower (e.g. Frost Arrow); Pierce adds pass-through targets (e.g. Piercing Bolt);
	// Poison adds poison-on-hit DPS (e.g. Venom Tip); AirDamage adds % bonus damage vs air enemies
	// (e.g. Falcon Sight); CritDamage adds % to the crit damage multiplier (e.g. Assassin's Mark);
	// AoeRadius adds % to the splash radius of an AoE tower (e.g. Seismic Core on StoneGod);
	// MagicPen adds % magic-armor penetration to a magic tower (e.g. Spell Piercer on MagicDragon);
	// HpRegen adds flat HP/sec regen to spawned units (e.g. Field Medic on Knights barracks);
	// AuraDamage adds flat % to a Supporter aura's damage buff (e.g. Amplifier Crystal).
	public enum StatType
	{
		Damage,
		AttackSpeed,
		Crit,
		Range,
		Health,
		Armor,
		GoldProduce,
		Slow,
		Pierce,
		Poison,
		AirDamage,
		CritDamage,
		AoeRadius,
		MagicPen,
		HpRegen,
		AuraDamage
	}
}
