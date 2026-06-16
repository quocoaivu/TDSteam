using System;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Parameter
{
	public class TowerParameterManager
	{
		public static TowerParameterManager Instance
		{
			get
			{
				if (TowerParameterManager.instance == null)
				{
					TowerParameterManager.instance = new TowerParameterManager();
				}
				return TowerParameterManager.instance;
			}
		}

		public void SetTowerParameter(TurretSpec tower)
		{
			tower = ApplyBuff(tower);
			int count = listTower.Count;
			if (count <= tower.id)
			{
				List<TurretSpec> list = new List<TurretSpec>();
				list.Insert(tower.level, tower);
				listTower.Insert(tower.id, list);
			}
			else
			{
				List<TurretSpec> list2 = listTower[tower.id];
				list2.Insert(tower.level, tower);
			}
		}

		public TurretSpec GetTowerParameter(int id, int level)
		{
			if (CheckParameter(id, level))
			{
				return listTower[id][level];
			}
			return default(TurretSpec);
		}

		public void ModifyTowerParameter(int id, int level, TurretSpec newParameter)
		{
			listTower[id][level] = newParameter;
		}

		private bool CheckParameter(int id, int level)
		{
			return id >= 0 && level >= 0 && id < GetNumberOfTower() && level < GetNumberOfLevel();
		}

		public int GetMinDamage(int towerID, int towerLevel)
		{
			int result;
			if (CheckParameter(towerID, towerLevel))
			{
				if (listTower[towerID][towerLevel].damage_Physics_min == 0)
				{
					result = listTower[towerID][towerLevel].damage_Magic_min;
				}
				else
				{
					result = listTower[towerID][towerLevel].damage_Physics_min;
				}
			}
			else
			{
				result = -1;
			}
			return result;
		}

		public int GetMaxDamage(int towerID, int towerLevel)
		{
			int result;
			if (CheckParameter(towerID, towerLevel))
			{
				if (listTower[towerID][towerLevel].damage_Physics_max == 0)
				{
					result = listTower[towerID][towerLevel].damage_Magic_max;
				}
				else
				{
					result = listTower[towerID][towerLevel].damage_Physics_max;
				}
			}
			else
			{
				result = -1;
			}
			return result;
		}

		public bool isPhysicsAttack(int towerID)
		{
			return listTower[towerID][0].damage_Physics_max > 0;
		}

		public float GetCooldownTime(int id, int level)
		{
			if (CheckParameter(id, level))
			{
				return (float)listTower[id][level].reload / 1000f;
			}
			return -1f;
		}

		public int GetAttackSpeed(int id, int level)
		{
			if (CheckParameter(id, level))
			{
				return listTower[id][level].reload;
			}
			return -1;
		}

		public int GetRangeMax(int id, int level)
		{
			if (CheckParameter(id, level))
			{
				return listTower[id][level].attackRangeMax;
			}
			return -1;
		}

		public int GetUnitHealth(int id, int level)
		{
			if (CheckParameter(id, level))
			{
				return listTower[id][level].unit_health;
			}
			return -1;
		}

		public int GetUnitArmor(int id, int level)
		{
			if (CheckParameter(id, level))
			{
				return listTower[id][level].unit_armor_physics;
			}
			return -1;
		}

		public int GetPrice(int id, int level)
		{
			if (CheckParameter(id, level))
			{
				return listTower[id][level].price;
			}
			return -1;
		}

		// Tower level scheme (single source of truth, shared by upgrade + UI):
		//   levels 0..MAX_BASE_LEVEL : linear base tiers; the ultimate branch is always 0 here.
		//   at MAX_BASE_LEVEL the player picks an ultimate branch, which forks into two terminal levels:
		//     branch 0 -> ULTIMATE_LEVEL_BRANCH_0, branch 1 -> ULTIMATE_LEVEL_BRANCH_1.
		public const int MAX_BASE_LEVEL = 2;

		public const int ULTIMATE_LEVEL_BRANCH_0 = 3;

		public const int ULTIMATE_LEVEL_BRANCH_1 = 4;

		// The level a tower becomes after one upgrade from currentLevel. For base tiers pass
		// ultimateBranch = 0 (=> currentLevel + 1); at MAX_BASE_LEVEL pass the chosen branch (0 or 1)
		// to reach ULTIMATE_LEVEL_BRANCH_0 / _BRANCH_1.
		public int GetUpgradeTargetLevel(int currentLevel, int ultimateBranch)
		{
			return currentLevel + 1 + ultimateBranch;
		}

		// Which ultimate branch a tower already sitting at this level belongs to (-1 = not an ultimate level).
		public int GetUltimateBranchByLevel(int level)
		{
			if (level == ULTIMATE_LEVEL_BRANCH_0)
			{
				return 0;
			}
			if (level == ULTIMATE_LEVEL_BRANCH_1)
			{
				return 1;
			}
			return -1;
		}

		public int GetNumberOfTower()
		{
			return listTower.Count;
		}

		public int GetNumberOfLevel()
		{
			if (GetNumberOfTower() > 0)
			{
				return listTower[0].Count;
			}
			return 0;
		}

		public bool GetIsAirAttack(int id)
		{
			return listTower[id][0].isAirAttack;
		}

		public int GetGoldProduce(int id, int level)
		{
			if (CheckParameter(id, level))
			{
				return listTower[id][level].goldProduce;
			}
			return -1;
		}

		public float GetAutoCollectProduceGoldTime(int id, int level)
		{
			if (CheckParameter(id, level))
			{
				return (float)listTower[id][level].autoCollectTime / 1000f;
			}
			return -1f;
		}

		private TurretSpec ApplyBuff(TurretSpec tower)
		{
			if (tower.id == 0)
			{
				int currentUpgradeLevel = GlobalUpgradeStore.Instance.GetCurrentUpgradeLevel(0);
				for (int i = 0; i <= currentUpgradeLevel; i++)
				{
					if (i == 0)
					{
						int num = 100 - GlobalUpgradeStore.Instance.GetUpgradeValue(0, 0);
						tower.price = (int)((float)(tower.price * num) / 100f);
					}
					if (i == 1)
					{
						int upgradeValue = GlobalUpgradeStore.Instance.GetUpgradeValue(1, 0);
						tower.attackRangeMax += upgradeValue;
					}
					if (i == 2)
					{
						int upgradeValue2 = GlobalUpgradeStore.Instance.GetUpgradeValue(7, 0);
						tower.ignoreArmorChance += upgradeValue2;
					}
					if (i == 3)
					{
						int upgradeValue3 = GlobalUpgradeStore.Instance.GetUpgradeValue(2, 0);
						tower.attackRangeMax += upgradeValue3;
					}
					if (i == 4)
					{
						int upgradeValue4 = GlobalUpgradeStore.Instance.GetUpgradeValue(6, 0);
						tower.criticalStrikeChance += upgradeValue4;
					}
				}
			}
			if (tower.id == 1)
			{
				int currentUpgradeLevel2 = GlobalUpgradeStore.Instance.GetCurrentUpgradeLevel(1);
				for (int j = 0; j <= currentUpgradeLevel2; j++)
				{
					if (j == 0)
					{
						int num = 100 - GlobalUpgradeStore.Instance.GetUpgradeValue(0, 1);
						tower.price = (int)((float)(tower.price * num) / 100f);
					}
					if (j == 1)
					{
						int upgradeValue5 = GlobalUpgradeStore.Instance.GetUpgradeValue(12, 1);
						tower.unit_armor_physics += upgradeValue5;
					}
					if (j == 2)
					{
						int upgradeValue = GlobalUpgradeStore.Instance.GetUpgradeValue(1, 1);
						tower.attackRangeMax += upgradeValue;
						int upgradeValue6 = GlobalUpgradeStore.Instance.GetUpgradeValue(3, 1);
						tower.reload -= upgradeValue6;
					}
					if (j == 3)
					{
						int upgradeValue7 = GlobalUpgradeStore.Instance.GetUpgradeValue(13, 1);
						tower.unit_health += upgradeValue7;
					}
					if (j == 4)
					{
						int upgradeValue8 = GlobalUpgradeStore.Instance.GetUpgradeValue(5, 1);
						tower.ignoreReloadChance += upgradeValue8;
					}
				}
			}
			if (tower.id == 2)
			{
				int currentUpgradeLevel3 = GlobalUpgradeStore.Instance.GetCurrentUpgradeLevel(2);
				for (int k = 0; k <= currentUpgradeLevel3; k++)
				{
					if (k == 0)
					{
						int num = 100 - GlobalUpgradeStore.Instance.GetUpgradeValue(0, 2);
						tower.price = (int)((float)(tower.price * num) / 100f);
					}
					if (k == 1)
					{
						UnityEngine.Debug.Log("nang cap 1: giam thoi gian reload!");
						int upgradeValue6 = GlobalUpgradeStore.Instance.GetUpgradeValue(3, 2);
						tower.reload -= upgradeValue6;
					}
					if (k == 2)
					{
						int upgradeValue9 = GlobalUpgradeStore.Instance.GetUpgradeValue(8, 2);
						tower.damage_Physics_min += upgradeValue9;
						tower.damage_Physics_max += upgradeValue9;
					}
					if (k == 3)
					{
						int upgradeValue = GlobalUpgradeStore.Instance.GetUpgradeValue(1, 2);
						tower.attackRangeMax += upgradeValue;
					}
					if (k == 4)
					{
						int upgradeValue10 = GlobalUpgradeStore.Instance.GetUpgradeValue(4, 2);
						tower.reload -= upgradeValue10;
					}
				}
			}
			if (tower.id == 3)
			{
				int currentUpgradeLevel4 = GlobalUpgradeStore.Instance.GetCurrentUpgradeLevel(3);
				for (int l = 0; l <= currentUpgradeLevel4; l++)
				{
					if (l == 0)
					{
						int num = 100 - GlobalUpgradeStore.Instance.GetUpgradeValue(0, 3);
						tower.price = (int)((float)(tower.price * num) / 100f);
					}
					if (l == 1)
					{
						int upgradeValue = GlobalUpgradeStore.Instance.GetUpgradeValue(1, 3);
						tower.attackRangeMax += upgradeValue;
					}
					if (l == 2)
					{
						int upgradeValue9 = GlobalUpgradeStore.Instance.GetUpgradeValue(8, 3);
						tower.damage_Magic_min += upgradeValue9;
						tower.damage_Magic_max += upgradeValue9;
					}
					if (l == 3)
					{
						int upgradeValue4 = GlobalUpgradeStore.Instance.GetUpgradeValue(6, 3);
						tower.criticalStrikeChance += upgradeValue4;
					}
					if (l == 4)
					{
						string debuffKey = DamageVfxType.Slow.ToString();
						int upgradeValue11 = GlobalUpgradeStore.Instance.GetUpgradeValue(9, 3);
						int upgradeValue12 = GlobalUpgradeStore.Instance.GetUpgradeValue(11, 3);
						int upgradeValue13 = GlobalUpgradeStore.Instance.GetUpgradeValue(10, 3);
						tower.debuffKey = debuffKey;
						tower.debuffChance = upgradeValue11;
						tower.debuffEffectDuration = upgradeValue12;
						tower.debuffEffectValue = upgradeValue13;
					}
				}
			}
			return tower;
		}

		private List<List<TurretSpec>> listTower = new List<List<TurretSpec>>();

		private static TowerParameterManager instance;
		[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			instance = null;
		}
	}
}
