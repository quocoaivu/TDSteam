using System.Collections.Generic;
using Data;
using Gameplay;
using UnityEngine;

namespace Parameter
{
	public class TowerParameterManager
	{
		public static TowerParameterManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new TowerParameterManager();
				}
				return instance;
			}
		}

		public void SetTowerParameter(TurretSpec tower)
		{
			tower = ApplyBuff(tower);
			while (towers.Count <= tower.id)
			{
				towers.Add(default(TurretSpec));
			}
			towers[tower.id] = tower;
		}

		// Returns spec for this tower. Level parameter is ignored (all towers are single-tier now).
		public TurretSpec GetTowerParameter(int id, int level = 0)
		{
			if (id >= 0 && id < towers.Count)
			{
				return towers[id];
			}
			return default(TurretSpec);
		}

		public void ModifyTowerParameter(int id, TurretSpec newParameter)
		{
			if (id >= 0 && id < towers.Count)
			{
				towers[id] = newParameter;
			}
		}

		// Base spec with all permanently-unlocked skill-tree node deltas applied.
		public TurretSpec GetStatWithTree(int id)
		{
			TurretSpec spec = GetTowerParameter(id);
			List<int> unlockedNodes = TowerSkillTreeStore.Instance.GetUnlockedNodes(id);
			for (int i = 0; i < unlockedNodes.Count; i++)
			{
				TowerSkillNode node;
				if (!TowerSkillTreeSpec.Instance.TryGetNode(id, unlockedNodes[i], out node))
				{
					continue;
				}
				spec.damage += node.dmgAdd;
				spec.range += node.rangeAdd / GameRecord.PIXEL_PER_UNIT;
				spec.critChance += node.critAdd;
				spec.ignoreArmorChance += node.pierceAdd;
				spec.unit_health += node.healthAdd;
				spec.unit_armor += node.armorAdd;
				spec.goldProduce += node.goldAdd;
				if (node.autocollectReduce > 0)
				{
					spec.autoCollectTime = Mathf.Max(100, spec.autoCollectTime - node.autocollectReduce);
				}
				// reload reduction → convert to attackSpeed increase
				if (node.reloadReduce > 0 && spec.attackSpeed > 0)
				{
					float reloadMs = 1000f / spec.attackSpeed;
					reloadMs -= node.reloadReduce;
					if (reloadMs < 100f) reloadMs = 100f;
					spec.attackSpeed = 1000f / reloadMs;
				}
			}
			return spec;
		}

		// Returns reload time in ms for legacy callers (AbilityRankDescriber expects ms int).
		public int GetAttackSpeed(int id, int level = 0)
		{
			float spd = GetTowerParameter(id).attackSpeed;
			return spd > 0 ? (int)(1000f / spd) : 0;
		}

		// Always 1 level per tower now; kept so BulletPool guard still compiles.
		public int GetNumberOfLevel()
		{
			return towers.Count > 0 ? 1 : 0;
		}

		public int GetNumberOfTower()
		{
			return towers.Count;
		}

		public int GetPrice(int id, int level = 0)
		{
			return GetTowerParameter(id).buildCost;
		}

		public int GetRangeMax(int id, int level = 0)
		{
			// Return in pixels for legacy callers that divide by PIXEL_PER_UNIT themselves.
			return (int)(GetTowerParameter(id).range * GameRecord.PIXEL_PER_UNIT);
		}

		public int GetMinDamage(int id, int level = 0)
		{
			return GetTowerParameter(id).damage;
		}

		public int GetMaxDamage(int id, int level = 0)
		{
			return GetTowerParameter(id).damage;
		}

		public float GetCooldownTime(int id, int level = 0)
		{
			float spd = GetTowerParameter(id).attackSpeed;
			return spd > 0 ? 1f / spd : 999f;
		}

		public int GetUnitHealth(int id, int level = 0)
		{
			return GetTowerParameter(id).unit_health;
		}

		public int GetUnitArmor(int id, int level = 0)
		{
			return GetTowerParameter(id).unit_armor;
		}

		public bool GetIsAirAttack(int id)
		{
			return GetTowerParameter(id).canTargetAir;
		}

		public int GetGoldProduce(int id, int level = 0)
		{
			return GetTowerParameter(id).goldProduce;
		}

		public float GetAutoCollectProduceGoldTime(int id, int level = 0)
		{
			return GetTowerParameter(id).autoCollectTime / 1000f;
		}

		// Kept for callers that haven't been updated yet; same as GetPrice.
		public int GetUpgradeTargetLevel(int currentLevel, int ultimateBranch)
		{
			return currentLevel + 1 + ultimateBranch;
		}

		public int GetUltimateBranchByLevel(int level)
		{
			if (level == ULTIMATE_LEVEL_BRANCH_0) return 0;
			if (level == ULTIMATE_LEVEL_BRANCH_1) return 1;
			return -1;
		}

		public bool isPhysicsAttack(int id)
		{
			return GetTowerParameter(id).damageType == DamageType.Physical;
		}

		public const int MAX_BASE_LEVEL = 2;
		public const int ULTIMATE_LEVEL_BRANCH_0 = 3;
		public const int ULTIMATE_LEVEL_BRANCH_1 = 4;

		private TurretSpec ApplyBuff(TurretSpec tower)
		{
			int id = tower.id;
			int upgradeLevel = GlobalUpgradeStore.Instance.GetCurrentUpgradeLevel(id);

			for (int i = 0; i <= upgradeLevel; i++)
			{
				if (i == 0)
				{
					int discount = 100 - GlobalUpgradeStore.Instance.GetUpgradeValue(0, id);
					tower.buildCost = (int)(tower.buildCost * discount / 100f);
				}
				if (id == 0)
				{
					if (i == 1) tower.range += GlobalUpgradeStore.Instance.GetUpgradeValue(1, id) / GameRecord.PIXEL_PER_UNIT;
					if (i == 2) tower.ignoreArmorChance += GlobalUpgradeStore.Instance.GetUpgradeValue(7, id);
					if (i == 3) tower.range += GlobalUpgradeStore.Instance.GetUpgradeValue(2, id) / GameRecord.PIXEL_PER_UNIT;
					if (i == 4) tower.critChance += GlobalUpgradeStore.Instance.GetUpgradeValue(6, id);
				}
				else if (id == 1)
				{
					if (i == 1) tower.unit_armor += GlobalUpgradeStore.Instance.GetUpgradeValue(12, id);
					if (i == 2) tower.range += GlobalUpgradeStore.Instance.GetUpgradeValue(1, id) / GameRecord.PIXEL_PER_UNIT;
					if (i == 3) tower.unit_health += GlobalUpgradeStore.Instance.GetUpgradeValue(13, id);
				}
				else if (id == 2)
				{
					if (i == 1)
					{
						int reloadReduce = GlobalUpgradeStore.Instance.GetUpgradeValue(3, id);
						if (tower.attackSpeed > 0)
						{
							float ms = 1000f / tower.attackSpeed - reloadReduce;
							if (ms < 100f) ms = 100f;
							tower.attackSpeed = 1000f / ms;
						}
					}
					if (i == 2) tower.damage += GlobalUpgradeStore.Instance.GetUpgradeValue(8, id);
					if (i == 3) tower.range += GlobalUpgradeStore.Instance.GetUpgradeValue(1, id) / GameRecord.PIXEL_PER_UNIT;
					if (i == 4)
					{
						int reloadReduce2 = GlobalUpgradeStore.Instance.GetUpgradeValue(4, id);
						if (tower.attackSpeed > 0)
						{
							float ms = 1000f / tower.attackSpeed - reloadReduce2;
							if (ms < 100f) ms = 100f;
							tower.attackSpeed = 1000f / ms;
						}
					}
				}
				else if (id == 3)
				{
					if (i == 1) tower.range += GlobalUpgradeStore.Instance.GetUpgradeValue(1, id) / GameRecord.PIXEL_PER_UNIT;
					if (i == 2) tower.damage += GlobalUpgradeStore.Instance.GetUpgradeValue(8, id);
					if (i == 3) tower.critChance += GlobalUpgradeStore.Instance.GetUpgradeValue(6, id);
					if (i == 4)
					{
						tower.slowPercent = GlobalUpgradeStore.Instance.GetUpgradeValue(10, id);
						tower.slowDuration = GlobalUpgradeStore.Instance.GetUpgradeValue(11, id) / 1000f;
					}
				}
			}
			return tower;
		}

		private List<TurretSpec> towers = new List<TurretSpec>();

		private static TowerParameterManager instance;

		[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			instance = null;
		}
	}
}
