using System;
using System.Collections.Generic;

namespace Parameter
{
	public class TurretSynopsis : Singleton<TurretSynopsis>
	{
		public void ClearTowerData()
		{
			listTowerDes.Clear();
		}

		public void SetTowerParameter(TurretBrief tower)
		{
			int count = listTowerDes.Count;
			if (count <= tower.id)
			{
				List<TurretBrief> list = new List<TurretBrief>();
				list.Insert(tower.level, tower);
				listTowerDes.Insert(tower.id, list);
			}
			else
			{
				List<TurretBrief> list2 = listTowerDes[tower.id];
				list2.Insert(tower.level, tower);
			}
		}

		public TurretBrief GetTowerParameter(int id, int level)
		{
			if (CheckParameter(id, level))
			{
				return listTowerDes[id][level];
			}
			return default(TurretBrief);
		}

		public int GetNumberOfTower()
		{
			return listTowerDes.Count;
		}

		public int GetNumberOfLevel()
		{
			if (GetNumberOfTower() > 0)
			{
				return listTowerDes[0].Count;
			}
			return 0;
		}

		private bool CheckParameter(int id, int level)
		{
			return id >= 0 && level >= 0 && id < GetNumberOfTower() && level < GetNumberOfLevel();
		}

		public void ClearTowerSkillData()
		{
			listTowerSkillDes.Clear();
		}

		public void SetTowerSkillParameter(TurretAbilityBrief towerSkillDes)
		{
			listTowerSkillDes.Add(towerSkillDes);
		}

		public List<TurretAbilityBrief> GetListTowerSkillDes()
		{
			return listTowerSkillDes;
		}

		public string GetTowerName(int towerId)
		{
			if (towerId < listTowerDes.Count)
			{
				return listTowerDes[towerId][0].name;
			}
			return null;
		}

		public string GetTowerType(int towerId)
		{
			if (towerId < listTowerDes.Count)
			{
				return listTowerDes[towerId][0].type;
			}
			return null;
		}

		public string GetTowerDescription(int towerId, int towerLevel)
		{
			if (towerId < listTowerDes.Count)
			{
				return listTowerDes[towerId][towerLevel].fullDescription;
			}
			return null;
		}

		public string GetTowerShortDescription(int towerId, int towerLevel)
		{
			if (towerId < listTowerDes.Count)
			{
				return listTowerDes[towerId][towerLevel].shortDescription;
			}
			return null;
		}

		public string GetTowerUnlockDescription(int towerId, int towerLevel)
		{
			if (towerId < listTowerDes.Count)
			{
				return listTowerDes[towerId][towerLevel].unlockDescription;
			}
			return null;
		}

		public string GetTowerUltimateName(int towerId, int towerLevel, int skillID)
		{
			string result = string.Empty;
			int ultimateBranchByLevel = TowerParameterManager.Instance.GetUltimateBranchByLevel(towerLevel);
			foreach (TurretAbilityBrief towerSkillDes in listTowerSkillDes)
			{
				if (towerSkillDes.id == towerId && towerSkillDes.level == towerLevel && towerSkillDes.ultimateBranch == ultimateBranchByLevel && towerSkillDes.skillID == skillID)
				{
					result = towerSkillDes.ultimateName;
				}
			}
			return result;
		}

		public string GetTowerUltimateDescription(int towerId, int towerLevel, int skillID)
		{
			string result = string.Empty;
			int ultimateBranchByLevel = TowerParameterManager.Instance.GetUltimateBranchByLevel(towerLevel);
			foreach (TurretAbilityBrief towerSkillDes in listTowerSkillDes)
			{
				if (towerSkillDes.id == towerId && towerSkillDes.level == towerLevel && towerSkillDes.ultimateBranch == ultimateBranchByLevel && towerSkillDes.skillID == skillID)
				{
					List<int> listParamNumber = TurretAbilitySpec.Instance.GetListParamNumber(towerId, ultimateBranchByLevel, skillID, 0);
					switch (listParamNumber.Count)
					{
					case 0:
						result = towerSkillDes.ultimateDescription;
						break;
					case 1:
					{
						string param = TurretAbilitySpec.Instance.GetParam(towerId, ultimateBranchByLevel, skillID, listParamNumber[0]);
						result = string.Format(towerSkillDes.ultimateDescription, param);
						break;
					}
					case 2:
					{
						string param2 = TurretAbilitySpec.Instance.GetParam(towerId, ultimateBranchByLevel, skillID, listParamNumber[0]);
						string param3 = TurretAbilitySpec.Instance.GetParam(towerId, ultimateBranchByLevel, skillID, listParamNumber[1]);
						result = string.Format(towerSkillDes.ultimateDescription, param2, param3);
						break;
					}
					case 3:
					{
						string param4 = TurretAbilitySpec.Instance.GetParam(towerId, ultimateBranchByLevel, skillID, listParamNumber[0]);
						string param5 = TurretAbilitySpec.Instance.GetParam(towerId, ultimateBranchByLevel, skillID, listParamNumber[1]);
						string param6 = TurretAbilitySpec.Instance.GetParam(towerId, ultimateBranchByLevel, skillID, listParamNumber[2]);
						result = string.Format(towerSkillDes.ultimateDescription, param4, param5, param6);
						break;
					}
					}
				}
			}
			return result;
		}

		private List<List<TurretBrief>> listTowerDes = new List<List<TurretBrief>>();

		private List<TurretAbilityBrief> listTowerSkillDes = new List<TurretAbilityBrief>();
	}
}
