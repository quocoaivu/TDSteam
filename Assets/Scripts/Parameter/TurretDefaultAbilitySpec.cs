using System;
using System.Collections.Generic;

namespace Parameter
{
	public class TurretDefaultAbilitySpec
	{
		public static TurretDefaultAbilitySpec Instance
		{
			get
			{
				if (TurretDefaultAbilitySpec.instance == null)
				{
					TurretDefaultAbilitySpec.instance = new TurretDefaultAbilitySpec();
				}
				return TurretDefaultAbilitySpec.instance;
			}
		}

		public void SetParameter(TurretDefaultAbility towerDefaultSkill)
		{
			int count = listTowerDefaultSkillParams.Count;
			if (count <= towerDefaultSkill.id)
			{
				listTowerDefaultSkillParams.Add(towerDefaultSkill);
			}
		}

		public TurretDefaultAbility GetTowerParameter(int towerID, int towerLevel)
		{
			TurretDefaultAbility result = default(TurretDefaultAbility);
			foreach (TurretDefaultAbility towerDefaultSkill in listTowerDefaultSkillParams)
			{
				if (towerDefaultSkill.towerID == towerID && towerDefaultSkill.towerLevel == towerLevel)
				{
					result = towerDefaultSkill;
				}
			}
			return result;
		}

		private List<TurretDefaultAbility> listTowerDefaultSkillParams = new List<TurretDefaultAbility>();

		private static TurretDefaultAbilitySpec instance;
		[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			instance = null;
		}
	}
}
