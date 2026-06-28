using System;
using System.Collections.Generic;

namespace Parameter
{
	public class TurretAbilitySpec
	{
		public static TurretAbilitySpec Instance
		{
			get
			{
				if (TurretAbilitySpec.instance == null)
				{
					TurretAbilitySpec.instance = new TurretAbilitySpec();
				}
				return TurretAbilitySpec.instance;
			}
		}

		public void ClearTowerSkillData()
		{
			listTowerSkillParams.Clear();
		}

		public void SetParameter(TurretAbilitySpecs towerSkillCost)
		{
			listTowerSkillParams.Add(towerSkillCost);
		}

		public List<int> GetListParamNumber(int towerID, int ultimateBranch, int skillID, int skillLevel)
		{
			List<int> list = new List<int>();
			foreach (TurretAbilitySpecs towerSkillParam in listTowerSkillParams)
			{
				if (towerSkillParam.towerID == towerID && towerSkillParam.ultimateBranch == ultimateBranch && towerSkillParam.skillID == skillID && towerSkillParam.skillLevel == skillLevel)
				{
					string paramPreview = towerSkillParam.paramPreview;
					string[] array = paramPreview.Split(new char[]
					{
						'_'
					}, StringSplitOptions.RemoveEmptyEntries);
					foreach (string s in array)
					{
						list.Add(int.Parse(s));
					}
				}
			}
			return list;
		}

		public string GetParam(int towerID, int ultimateBranch, int skillID, int paramID)
		{
			string result = string.Empty;
			foreach (TurretAbilitySpecs towerSkillParam in listTowerSkillParams)
			{
				if (towerSkillParam.towerID == towerID && towerSkillParam.ultimateBranch == ultimateBranch && towerSkillParam.skillID == skillID)
				{
					result = string.Concat(new object[]
					{
						GetParamBySkillLevel(towerID, ultimateBranch, skillID, 0, paramID),
						"/",
						GetParamBySkillLevel(towerID, ultimateBranch, skillID, 1, paramID),
						"/",
						GetParamBySkillLevel(towerID, ultimateBranch, skillID, 2, paramID)
					});
				}
			}
			return result;
		}

		// Skill param at the equipped tier PLUS the permanent skill-tree bonus (nodes targeting this skill's
		// param). Negative tree deltas (trade-off) are allowed; the final value is floored at 0 so a param
		// like arrow-count or proc-chance never goes negative. Abilities read params through this.
		public int GetParamWithTree(int towerID, int ultimateBranch, int skillID, int skillLevel, int paramID)
		{
			int result = GetParamBySkillLevel(towerID, ultimateBranch, skillID, skillLevel, paramID);
			result += TowerSkillTreeSpec.Instance.GetSkillParamBonus(towerID, ultimateBranch, skillID, paramID);
			return result < 0 ? 0 : result;
		}

		public int GetParamBySkillLevel(int towerID, int ultimateBranch, int skillID, int skillLevel, int paramID)
		{
			int result = -1;
			foreach (TurretAbilitySpecs towerSkillParam in listTowerSkillParams)
			{
				if (towerSkillParam.towerID == towerID && towerSkillParam.ultimateBranch == ultimateBranch && towerSkillParam.skillID == skillID && towerSkillParam.skillLevel == skillLevel)
				{
					switch (paramID)
					{
					case 0:
						result = towerSkillParam.param0;
						break;
					case 1:
						result = towerSkillParam.param1;
						break;
					case 2:
						result = towerSkillParam.param2;
						break;
					case 3:
						result = towerSkillParam.param3;
						break;
					case 4:
						result = towerSkillParam.param4;
						break;
					}
				}
			}
			return result;
		}

		// Display name of an ability (= an item). Only the level-0 row carries the name in the CSV.
		public string GetSkillName(int towerID, int ultimateBranch, int skillID)
		{
			foreach (TurretAbilitySpecs towerSkillParam in listTowerSkillParams)
			{
				if (towerSkillParam.towerID == towerID && towerSkillParam.ultimateBranch == ultimateBranch && towerSkillParam.skillID == skillID && !string.IsNullOrEmpty(towerSkillParam.skillName))
				{
					return towerSkillParam.skillName;
				}
			}
			return string.Empty;
		}

		public int GetUltimateSkillUpgradeCost(int towerID, int ultimateBranch, int skillID, int skillLevel)
		{
			int result = -1;
			foreach (TurretAbilitySpecs towerSkillParam in listTowerSkillParams)
			{
				if (towerSkillParam.towerID == towerID && towerSkillParam.ultimateBranch == ultimateBranch && towerSkillParam.skillID == skillID && towerSkillParam.skillLevel == skillLevel)
				{
					result = towerSkillParam.upgradeCost;
				}
			}
			return result;
		}

		private List<TurretAbilitySpecs> listTowerSkillParams = new List<TurretAbilitySpecs>();

		private static TurretAbilitySpec instance;
		[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			instance = null;
		}
	}
}
