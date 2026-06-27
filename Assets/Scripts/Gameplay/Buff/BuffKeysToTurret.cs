using System;

namespace Gameplay
{
	public static class BuffKeysToTurret
	{
        public const string Silent = "Silent";

        public const string DamageDecrementCommon = "DamageDecrementCommon";

        public const string DamageIncrementCommon = "DamageIncrementCommon";

        public const string AttackRangeIncrementCommon = "AttackRangeIncrementCommon";

        public const string HellfireCooldownTimeDecrementCommon = "HellfireCooldownTimeDecrementCommon";

        public const string InstantKillRateIncrementCommon = "InstantKillRateIncrementCommon";

        public const string CritIncrementCommon = "CritIncrementCommon";


        public const string INCREASE_ATTACK_SPEED_BY_PERCENTAGE = "IncreaseAttackSpeedByPercentage";

        // Item-only keys for non-combat towers (read at spawn/tick, not via OnBuffValueChanged).
        public const string ITEM_MINION_HEALTH_PERCENT = "ItemMinionHealthPercent";

        public const string ITEM_MINION_ARMOR_FLAT = "ItemMinionArmorFlat";

        // Flat HP/sec regen added to spawned units by items (e.g. Field Medic). Read in BuildSpawnSpec.
        public const string ITEM_MINION_HP_REGEN_FLAT = "ItemMinionHpRegenFlat";

        public const string ITEM_GOLD_PRODUCE_FLAT = "ItemGoldProduceFlat";

        // Flat % added to a Supporter aura's damage amp by items (e.g. Amplifier Crystal). Read each
        // aura tick in TurretAbilityBuffAura.CastAura so it applies live when equipped mid-match.
        public const string ITEM_AURA_DAMAGE_AMP_FLAT = "ItemAuraDamageAmpFlat";

        // Slow-on-hit % added by items (e.g. Frost Arrow). Read at fire time in the strike handler,
        // not via OnBuffValueChanged, and stacked on top of the tower's base slowPercent.
        public const string SlowOnHitIncrementCommon = "SlowOnHitIncrementCommon";

        // Extra pass-through targets added by items (e.g. Piercing Bolt). Read at fire time and stacked
        // on top of the tower's base pierceCount.
        public const string PierceCountIncrementCommon = "PierceCountIncrementCommon";

        // Poison-on-hit DPS added by items (e.g. Venom Tip). Read at fire time and stacked on top of
        // the tower's base poisonDPS.
        public const string PoisonDpsIncrementCommon = "PoisonDpsIncrementCommon";

        // % bonus damage vs air enemies added by items (e.g. Falcon Sight). Read at fire time; applied
        // only when the locked target is an air enemy.
        public const string AirDamageIncrementCommon = "AirDamageIncrementCommon";

        // % added to the crit damage multiplier by items (e.g. Assassin's Mark): value 50 = +0.5x.
        // Read at fire time and stacked on top of the tower's base critMultiplier.
        public const string CritDamageIncrementCommon = "CritDamageIncrementCommon";

        // % added to the AoE splash radius by items (e.g. Seismic Core). Read at fire time; multiplies
        // the tower's base aoeRadius, so it only matters on towers that already have AoE (e.g. StoneGod).
        public const string AoeRadiusIncrementCommon = "AoeRadiusIncrementCommon";

        // % magic-armor penetration added by items (e.g. Spell Piercer). Read at fire time and stacked
        // on top of the tower's base magicPenetration; only matters for magic-damage towers (MagicDragon).
        public const string MagicPenIncrementCommon = "MagicPenIncrementCommon";
        public static string[] AllKeys
		{
			get
			{
				return BuffKeysToTurret.allKeys;
			}
			set
			{
				BuffKeysToTurret.allKeys = value;
			}
		}

		private static string[] allKeys = new string[]
		{
			"Silent",
			"DamageDecrementCommon",
			"DamageIncrementCommon",
			"AttackRangeIncrementCommon",
			"HellfireCooldownTimeDecrementCommon",
			"InstantKillRateIncrementCommon",
			"CritIncrementCommon",
			"IncreaseAttackSpeedByPercentage",
			"SlowOnHitIncrementCommon",
			"PierceCountIncrementCommon",
			"PoisonDpsIncrementCommon",
			"AirDamageIncrementCommon",
			"CritDamageIncrementCommon",
			"AoeRadiusIncrementCommon",
			"MagicPenIncrementCommon"
		};

	}
}
