using System;
using System.Collections.Generic;
using GameCore;
using Parameter;
using UnityEngine;

namespace Data
{
	public class TowerDataLoader : BaseMonoBehaviour
	{
		private void Awake()
		{
			ReadTowerParameter();
			ReadTowerDefaultSkillParameter();
			ReadTowerSkillParameter();
			ReadTowerSkillTreeParameter();
			ReadItemParameter();
		}

		private void ReadItemParameter()
		{
			string text = "Parameters/items/tower_item_parameter";
			try
			{
				ItemSpecCatalog.Instance.Clear();
				List<Dictionary<string, object>> list = CSVLoader.Read(text);
				for (int i = 0; i < list.Count; i++)
				{
					ItemSpec spec = default(ItemSpec);
					spec.itemId = (int)list[i]["item_id"];
					spec.towerId = (int)list[i]["tower_id"];
					spec.name = (string)list[i]["name"];
					spec.rarity = (int)list[i]["rarity"];
					spec.icon = list[i].ContainsKey("icon") ? list[i]["icon"].ToString() : string.Empty;
					spec.statTypes = ParseStatTypes(list[i]);
					spec.statValues = ParseStatValues(list[i], spec.statTypes.Length);
					ItemSpecCatalog.Instance.SetParameter(spec);
				}
			}
			catch (Exception)
			{
				TowerDataLoader.ShowError(text);
				throw;
			}
		}

		// Reads stat_type, stat_type_2, stat_type_3 — skips empty optional columns.
		private static Items.StatType[] ParseStatTypes(Dictionary<string, object> row)
		{
			var result = new System.Collections.Generic.List<Items.StatType>();
			result.Add(ParseStatType(row["stat_type"].ToString()));
			if (row.ContainsKey("stat_type_2") && !string.IsNullOrWhiteSpace(row["stat_type_2"]?.ToString()))
			{
				result.Add(ParseStatType(row["stat_type_2"].ToString()));
			}
			if (row.ContainsKey("stat_type_3") && !string.IsNullOrWhiteSpace(row["stat_type_3"]?.ToString()))
			{
				result.Add(ParseStatType(row["stat_type_3"].ToString()));
			}
			return result.ToArray();
		}

		// Reads stat_value, stat_value_2, stat_value_3 matching the number of parsed stat types.
		private static int[] ParseStatValues(Dictionary<string, object> row, int count)
		{
			string[] keys = { "stat_value", "stat_value_2", "stat_value_3" };
			int[] result = new int[count];
			for (int i = 0; i < count; i++)
			{
				if (row.ContainsKey(keys[i]) && int.TryParse(row[keys[i]]?.ToString(), out int v))
				{
					result[i] = v;
				}
			}
			return result;
		}

		private static Items.StatType ParseStatType(string raw)
		{
			switch (raw)
			{
			case "AttackSpeed":
				return Items.StatType.AttackSpeed;
			case "Crit":
				return Items.StatType.Crit;
			default:
				return Items.StatType.Damage;
			}
		}

		private void ReadTowerSkillTreeParameter()
		{
			string text = "Parameters/tower_skilltree_parameter";
			try
			{
				TowerSkillTreeSpec.Instance.Clear();
				List<Dictionary<string, object>> list = CSVLoader.Read(text);
				for (int i = 0; i < list.Count; i++)
				{
					TowerSkillNode node = default(TowerSkillNode);
					node.towerID = (int)list[i]["tower_id"];
					node.nodeID = (int)list[i]["node_id"];
					node.name = (string)list[i]["name"];
					node.cost = (int)list[i]["cost"];
					// Read via ToString so a single-id prereq parsed as int (e.g. "0") still works.
					node.prerequisites = list[i]["prerequisites"].ToString();
					node.dmgAdd = (int)list[i]["dmg_add"];
					node.rangeAdd = (int)list[i]["range_add"];
					node.reloadReduce = (int)list[i]["reload_reduce"];
					node.critAdd = (int)list[i]["crit_add"];
					node.pierceAdd = (int)list[i]["pierce_add"];
					node.healthAdd = (int)list[i]["health_add"];
					node.armorAdd = (int)list[i]["armor_add"];
					node.goldAdd = (int)list[i]["gold_add"];
					node.autocollectReduce = (int)list[i]["autocollect_reduce"];
					node.posX = (int)list[i]["pos_x"];
					node.posY = (int)list[i]["pos_y"];
					TowerSkillTreeSpec.Instance.SetParameter(node);
				}
			}
			catch (Exception)
			{
				TowerDataLoader.ShowError(text);
				throw;
			}
		}

		private void ReadTowerDefaultSkillParameter()
		{
			string text = "Parameters/tower_default_skill_parameter";
			try
			{
				List<Dictionary<string, object>> list = CSVLoader.Read(text);
				for (int i = 0; i < list.Count; i++)
				{
					TurretDefaultAbility parameter = default(TurretDefaultAbility);
					parameter.id = (int)list[i]["id"];
					parameter.towerID = (int)list[i]["tower_id"];
					parameter.towerLevel = (int)list[i]["level"];
					parameter.name = (string)list[i]["name"];
					parameter.skillParam0 = (int)list[i]["skill_param_0"];
					parameter.skillParam1 = (int)list[i]["skill_param_1"];
					parameter.skillParam2 = (int)list[i]["skill_param_2"];
					parameter.skillParam3 = (int)list[i]["skill_param_3"];
					parameter.skillParam4 = (int)list[i]["skill_param_4"];
					parameter.skillName = (string)list[i]["skill_name"];
					TurretDefaultAbilitySpec.Instance.SetParameter(parameter);
				}
			}
			catch (Exception)
			{
				TowerDataLoader.ShowError(text);
				throw;
			}
		}

		private void ReadTowerSkillParameter()
		{
			string text = "Parameters/TowerSkills/tower_skill_parameter";
			try
			{
				TurretAbilitySpec.Instance.ClearTowerSkillData();
				List<Dictionary<string, object>> list = CSVLoader.Read(text);
				for (int i = 0; i < list.Count; i++)
				{
					TurretAbilitySpecs parameter = default(TurretAbilitySpecs);
					parameter.towerID = (int)list[i]["tower_id"];
					parameter.ultimateBranch = (int)list[i]["ultimate_branch"];
					parameter.skillID = (int)list[i]["skill_id"];
					parameter.skillLevel = (int)list[i]["skill_level"];
					parameter.towerName = ((string)list[i]["tower_name"]).Replace('$', '\n');
					parameter.skillName = ((string)list[i]["skill_name"]).Replace('$', '\n');
					parameter.upgradeCost = (int)list[i]["upgrade_cost"];
					parameter.paramPreview = (string)list[i]["param_preview"];
					parameter.param0 = (int)list[i]["param_0"];
					parameter.param1 = (int)list[i]["param_1"];
					parameter.param2 = (int)list[i]["param_2"];
					parameter.param3 = (int)list[i]["param_3"];
					parameter.param4 = (int)list[i]["param_4"];
					TurretAbilitySpec.Instance.SetParameter(parameter);
				}
			}
			catch (Exception)
			{
				TowerDataLoader.ShowError(text);
				throw;
			}
		}

		private void ReadTowerParameter()
		{
			string text = "Parameters/tower_parameter";
			try
			{
				List<Dictionary<string, object>> list2 = CSVLoader.Read(text);
				for (int i = 0; i < list2.Count; i++)
				{
					TurretSpec towerParameter = default(TurretSpec);
					towerParameter.id = (int)list2[i]["id"];
					towerParameter.name = (string)list2[i]["name"];
					towerParameter.level = 0;
					towerParameter.damage = (int)list2[i]["damage"];
					towerParameter.attackSpeed = ParseFloat(list2[i]["attack_speed"]);
					towerParameter.range = ParseFloat(list2[i]["range"]);
					towerParameter.projectileSpeed = ParseFloat(list2[i]["projectile_speed"]);
					towerParameter.damageType = ParseDamageType(list2[i]["damage_type"].ToString());
					towerParameter.targetPriority = ParseTargetPriority(list2[i]["target_priority"].ToString());
					towerParameter.canTargetAir = list2[i]["can_target_air"].ToString().ToLower() == "true";
					towerParameter.pierceCount = (int)list2[i]["pierce_count"];
					towerParameter.buildCost = (int)list2[i]["build_cost"];
					towerParameter.sellValue = (int)list2[i]["sell_value"];
					towerParameter.slowPercent = ParseFloat(list2[i]["slow_percent"]);
					towerParameter.slowDuration = ParseFloat(list2[i]["slow_duration"]);
					towerParameter.poisonDPS = ParseFloat(list2[i]["poison_dps"]);
					towerParameter.poisonDuration = ParseFloat(list2[i]["poison_duration"]);
					towerParameter.critChance = ParseFloat(list2[i]["crit_chance"]);
					towerParameter.critMultiplier = ParseFloat(list2[i]["crit_multiplier"]);
					towerParameter.aoeRadius = ParseFloat(list2[i]["aoe_radius"]);
					towerParameter.isRoundAttack = list2[i]["is_round_attack"].ToString().ToLower() == "true";
					towerParameter.unit_health = (int)list2[i]["unit_health"];
					towerParameter.unit_armor = (int)list2[i]["unit_armor"];
					towerParameter.unit_moveSpeed = (int)list2[i]["unit_move_speed"];
					towerParameter.unit_attackRange = (int)list2[i]["unit_attack_range"];
					towerParameter.unit_attackCooldown = (int)list2[i]["unit_attack_cooldown"];
					towerParameter.goldProduce = (int)list2[i]["gold_produce"];
					towerParameter.autoCollectTime = (int)list2[i]["auto_collect_time"];
					TowerParameterManager.Instance.SetTowerParameter(towerParameter);
				}
			}
			catch (Exception)
			{
				TowerDataLoader.ShowError(text);
				throw;
			}
		}

		private static float ParseFloat(object value)
		{
			if (value == null) return 0f;
			if (float.TryParse(value.ToString(), System.Globalization.NumberStyles.Float,
				System.Globalization.CultureInfo.InvariantCulture, out float result))
			{
				return result;
			}
			return 0f;
		}

		private static Parameter.DamageType ParseDamageType(string raw)
		{
			if (raw == "Magic") return Parameter.DamageType.Magic;
			if (raw == "True") return Parameter.DamageType.True;
			return Parameter.DamageType.Physical;
		}

		private static Parameter.TargetPriority ParseTargetPriority(string raw)
		{
			if (raw == "Last") return Parameter.TargetPriority.Last;
			if (raw == "Strongest") return Parameter.TargetPriority.Strongest;
			if (raw == "Weakest") return Parameter.TargetPriority.Weakest;
			if (raw == "Closest") return Parameter.TargetPriority.Closest;
			return Parameter.TargetPriority.First;
		}

		private static void ShowError(string filePath)
		{
			UnityEngine.Debug.LogError("Tower parameter file not found or invalid: " + filePath);
		}
	}
}
