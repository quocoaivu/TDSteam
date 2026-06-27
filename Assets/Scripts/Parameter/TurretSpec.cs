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
		public int damageFalloff;    // % damage reduction from center to edge of the blast (0 = flat)
		public bool selfDamage;      // reserved — whether the blast also hits friendly units
		public float minRange;       // world units; enemies closer than this are ignored (blind spot)
		public bool splashTargeting; // prefer the enemy sitting in the densest cluster

		// --- Armor pierce (internal, populated by skill tree) ---
		public int ignoreArmorChance;

		// --- Barracks units (Knights) ---
		public int unit_health;
		public int unit_armor;
		public int unit_moveSpeed;
		public int unit_attackRange;
		public int unit_attackCooldown;
		public int unit_hpRegen;      // HP restored per second (0 = no regen)
		public int unit_shield;       // flat damage absorbed before HP is lost (0 = none)
		public int unit_maxUnits;     // how many units the barracks keeps deployed
		public int unit_respawnTime;  // ms before a dead unit respawns
		public int unit_deployRange;  // pixels; how far the rally flag can be placed from the tower
		public int unit_aggroRange;   // pixels; reserved — see TurretSummonMinionHandler note
		public int unit_leashRange;   // pixels; reserved — see TurretSummonMinionHandler note

		// --- Magic (Magic Dragon) ---
		public MagicElement magicElement; // Arcane / Fire / Ice / Lightning (flavor tag)
		public int magicPenetration;      // % of enemy magic resistance ignored
		public int arcaneBounce;          // reserved — bounces to next enemy (0 = no bounce)
		public float beamDuration;        // reserved — continuous beam channel time (seconds)

		// --- Supporter ---
		public int goldProduce;       // gold produced per interval
		public int autoCollectTime;   // milliseconds
		public float goldInterval;    // seconds between gold ticks (0 = derive from attackSpeed)
		public int goldOnKill;        // reserved — bonus gold per enemy killed in aura range
		public float goldMultiplier;  // reserved — multiplies gold gained in range (1 = none)
		public int interestRate;      // reserved — % interest on current gold per tick
		public float auraRadius;      // world units; radius of the support buff aura
		public int damageAmp;         // % bonus damage for towers in the aura
		public int attackSpeedBonus;  // % bonus attack speed for towers in the aura
		public int rangeBonus;        // % bonus range for towers in the aura
		public int cooldownReduction; // reserved — % tower skill cooldown reduction in the aura
	}
}
