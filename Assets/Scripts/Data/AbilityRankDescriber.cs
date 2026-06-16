using GameCore;
using Parameter;
using UnityEngine.Serialization;

namespace Data
{
	public class AbilityRankDescriber : BaseMonoBehaviour
	{
        [FormerlySerializedAs("abilitiesRank")]
        public AbilityRankConfig abilityRankConfig;

        private static AbilityRankDescriber instance;
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            instance = null;
        }

        public static AbilityRankDescriber Instance
		{
			get
			{
				return AbilityRankDescriber.instance;
			}
		}

		private void Awake()
		{
			AbilityRankDescriber.instance = this;
		}

		public string GetArmorDescriptionByValue(int armor)
		{
			string result = string.Empty;
			if (IsValueBelowMin(armor, abilityRankConfig.ArmorValue[0]))
			{
				result = Singleton<AlertSynopsis>.Instance.GetNotiContent(abilityRankConfig.ArmorDescription[0]);
			}
			else if (IsValueInRange(armor, abilityRankConfig.ArmorValue[0], abilityRankConfig.ArmorValue[1]))
			{
				result = Singleton<AlertSynopsis>.Instance.GetNotiContent(abilityRankConfig.ArmorDescription[1]);
			}
			else if (IsValueInRange(armor, abilityRankConfig.ArmorValue[1], abilityRankConfig.ArmorValue[2]))
			{
				result = Singleton<AlertSynopsis>.Instance.GetNotiContent(abilityRankConfig.ArmorDescription[2]);
			}
			else if (IsValueAboveMax(armor, abilityRankConfig.ArmorValue[2]))
			{
				result = Singleton<AlertSynopsis>.Instance.GetNotiContent(abilityRankConfig.ArmorDescription[3]);
			}
			if (armor == 0)
			{
				result = Singleton<AlertSynopsis>.Instance.GetNotiContent(abilityRankConfig.ArmorDescription[4]);
			}
			return result;
		}

		public string GetAttackSpeedDescriptionByValue(int reloadTime)
		{
			string result = string.Empty;
			if (IsValueAboveMax(reloadTime, abilityRankConfig.AttackSpeedValue[0]))
			{
				result = Singleton<AlertSynopsis>.Instance.GetNotiContent(abilityRankConfig.AttackSpeedDescription[0]);
			}
			else if (IsValueInRange(reloadTime, abilityRankConfig.AttackSpeedValue[1], abilityRankConfig.AttackSpeedValue[0]))
			{
				result = Singleton<AlertSynopsis>.Instance.GetNotiContent(abilityRankConfig.AttackSpeedDescription[1]);
			}
			else if (IsValueInRange(reloadTime, abilityRankConfig.AttackSpeedValue[2], abilityRankConfig.AttackSpeedValue[1]))
			{
				result = Singleton<AlertSynopsis>.Instance.GetNotiContent(abilityRankConfig.AttackSpeedDescription[2]);
			}
			else if (IsValueBelowMin(reloadTime, abilityRankConfig.AttackSpeedValue[2]))
			{
				result = Singleton<AlertSynopsis>.Instance.GetNotiContent(abilityRankConfig.AttackSpeedDescription[3]);
			}
			return result;
		}

		public string GetAttackRangeDescriptionByValue(int attackRange)
		{
			string result = string.Empty;
			if (IsValueBelowMin(attackRange, abilityRankConfig.AttackRangeValue[0]))
			{
				result = Singleton<AlertSynopsis>.Instance.GetNotiContent(abilityRankConfig.AttackRangeDescription[0]);
			}
			else if (IsValueInRange(attackRange, abilityRankConfig.AttackRangeValue[0], abilityRankConfig.AttackRangeValue[1]))
			{
				result = Singleton<AlertSynopsis>.Instance.GetNotiContent(abilityRankConfig.AttackRangeDescription[1]);
			}
			else if (IsValueInRange(attackRange, abilityRankConfig.AttackRangeValue[1], abilityRankConfig.AttackRangeValue[2]))
			{
				result = Singleton<AlertSynopsis>.Instance.GetNotiContent(abilityRankConfig.AttackRangeDescription[2]);
			}
			else if (IsValueAboveMax(attackRange, abilityRankConfig.AttackRangeValue[2]))
			{
				result = Singleton<AlertSynopsis>.Instance.GetNotiContent(abilityRankConfig.AttackRangeDescription[3]);
			}
			return result;
		}

		public string GetMoveSpeedDescriptionByValue(int moveSpeed)
		{
			string result = string.Empty;
			if (IsValueBelowMin(moveSpeed, abilityRankConfig.MSpeedValue[0]))
			{
				result = Singleton<AlertSynopsis>.Instance.GetNotiContent(abilityRankConfig.MSpeedDescription[0]);
			}
			else if (IsValueInRange(moveSpeed, abilityRankConfig.MSpeedValue[0], abilityRankConfig.MSpeedValue[1]))
			{
				result = Singleton<AlertSynopsis>.Instance.GetNotiContent(abilityRankConfig.MSpeedDescription[1]);
			}
			else if (IsValueInRange(moveSpeed, abilityRankConfig.MSpeedValue[1], abilityRankConfig.MSpeedValue[2]))
			{
				result = Singleton<AlertSynopsis>.Instance.GetNotiContent(abilityRankConfig.MSpeedDescription[2]);
			}
			else if (IsValueAboveMax(moveSpeed, abilityRankConfig.MSpeedValue[2]))
			{
				result = Singleton<AlertSynopsis>.Instance.GetNotiContent(abilityRankConfig.MSpeedDescription[3]);
			}
			return result;
		}

		private bool IsValueInRange(int value, int minValue, int maxValue)
		{
			return value >= minValue && value <= maxValue;
		}

		private bool IsValueAboveMax(int value, int maxValue)
		{
			return value > maxValue;
		}

		private bool IsValueBelowMin(int value, int minValue)
		{
			return value < minValue;
		}
	}
}
