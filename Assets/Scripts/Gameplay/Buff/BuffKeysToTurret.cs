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
			"IncreaseAttackSpeedByPercentage"
		};

	}
}
