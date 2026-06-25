namespace Parameter
{
	public struct TurretSpec
	{
		// --- Identity ---
		public int id;
		public string name;
		public int level; // pool index only; not a gameplay tier

		// --- Core ---
		public int damage;            // flat damage per shot
		public float attackSpeed;     // shots per second
		public float range;           // attack radius in world units
		public float projectileSpeed; // world units per second (0 = instant/melee)
		public DamageType damageType;

		// --- Targeting ---
		public TargetPriority targetPriority;
		public bool canTargetAir;
		public int pierceCount;       // how many enemies the projectile passes through
		public bool isRoundAttack;    // tower seeks all enemies in range simultaneously

		// --- Economy ---
		public int buildCost;
		public int sellValue;

		// --- Debuff / Special ---
		public float slowPercent;    // % speed reduction applied on hit
		public float slowDuration;   // seconds
		public float poisonDPS;      // damage per second while poisoned
		public float poisonDuration; // seconds
		public float critChance;     // % chance to crit
		public float critMultiplier; // damage multiplier on crit (e.g. 2.0 = double)

		// --- AoE ---
		public float aoeRadius; // world units; 0 = single target

		// --- Armor pierce (internal, populated by skill tree) ---
		public int ignoreArmorChance;

		// --- Barracks units (Knights) ---
		public int unit_health;
		public int unit_armor;
		public int unit_moveSpeed;
		public int unit_attackRange;
		public int unit_attackCooldown;

		// --- Supporter ---
		public int goldProduce;
		public int autoCollectTime; // milliseconds
	}
}
