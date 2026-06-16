using System;
using System.Collections.Generic;
using GameCore;
using Parameter;
using UnityEngine;

namespace Data
{
	public class EnemyDataLoader : BaseMonoBehaviour
	{
		private void Awake()
		{
			ReadEnemyParameter();
		}

		private void ReadEnemyParameter()
		{
			string text = "Parameters/enemy_parameter";
			try
			{
				List<Dictionary<string, object>> list = CSVLoader.Read(text);
				for (int i = 0; i < list.Count; i++)
				{
					EnemyParameter enemyParameter = default(EnemyParameter);
					enemyParameter.id = (int)list[i]["id"];
					enemyParameter.name = (string)list[i]["name"];
					enemyParameter.level = (int)list[i]["level"];
					enemyParameter.health = (int)list[i]["health"];
					enemyParameter.size = (int)list[i]["size"];
					enemyParameter.body_size = (int)list[i]["body_size"];
					enemyParameter.armor_physics = (int)list[i]["armor_physics"];
					enemyParameter.armor_magic = (int)list[i]["armor_magic"];
					enemyParameter.attack_physics_min = (int)list[i]["attack_physics_min"];
					enemyParameter.attack_physics_max = (int)list[i]["attack_physics_max"];
					enemyParameter.attack_magic_min = (int)list[i]["attack_magic_min"];
					enemyParameter.attack_magic_max = (int)list[i]["attack_magic_max"];
					enemyParameter.attack_cooldown = (int)list[i]["attack_cooldown"];
					enemyParameter.attack_range_average = (int)list[i]["attack_range_average"];
					enemyParameter.attack_range_max = (int)list[i]["attack_range_max"];
					enemyParameter.change_avoid_melee = (int)list[i]["change_avoid_melee"];
					enemyParameter.speed = (int)list[i]["speed"];
					enemyParameter.value = (int)list[i]["value"];
					enemyParameter.lifeTaken = (int)list[i]["life_taken"];
					enemyParameter.lifeCount = (int)list[i]["life_count"];
					enemyParameter.isBoss = ((int)list[i]["boss"] > 0);
					enemyParameter.isAir = ((int)list[i]["air"] > 0);
					enemyParameter.isUnderGround = ((int)list[i]["underground"] > 0);
					enemyParameter.dropGemPercent = (int)list[i]["drop_gem_percent"];
					enemyParameter.dropGemAmountMin = (int)list[i]["drop_gem_amount_min"];
					enemyParameter.dropGemAmountMax = (int)list[i]["drop_gem_amount_max"];
					EnemyDatabase.Instance.SetEnemyParameter(enemyParameter);
				}
			}
			catch (Exception)
			{
				EnemyDataLoader.ShowError(text);
				throw;
			}
		}

		private static void ShowError(string filePath)
		{
			UnityEngine.Debug.LogError("File " + filePath + ".csv khÃ´ng tá»“n táº¡i hoáº·c dá»¯ liá»‡u trong file khÃ´ng Ä‘Ãºng Ä‘á»‹nh dáº¡ng.");
		}
	}
}
