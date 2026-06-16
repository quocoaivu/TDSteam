using System;
using System.Collections.Generic;
using MetaGame;
using UnityEngine;

namespace Data
{
	public class HeroCampConfigLoader : MonoBehaviour
	{
		private void Awake()
		{
			ReadGameConfig();
		}

		private void ReadGameConfig()
		{
			string text = "Parameters/herocamp_config";
			try
			{
				List<Dictionary<string, object>> list = CSVLoader.Read(text);
				Dictionary<string, object> row = list[0];
				HeroBarracksSetup.Instance.Health_max = (int)row["health_max"];
				HeroBarracksSetup.Instance.Attack_damage_max = (int)row["attack_damage_max"];
				HeroBarracksSetup.Instance.Physics_armor_max = (int)row["physics_armor_max"];
				HeroBarracksSetup.Instance.Attack_speed_max = (int)row["attack_speed_max"];
				HeroBarracksSetup.Instance.Magic_armor_max = (int)row["magic_armor_max"];
				HeroBarracksSetup.Instance.Health_regen_max = (int)row["health_regen"];
				HeroBarracksSetup.Instance.Movement_speed_max = (int)row["move_speed"];
			}
			catch (Exception)
			{
				UnityEngine.Debug.LogError("File " + text + ".csv not Available or Ivalid Data.");
				throw;
			}
		}
	}
}
