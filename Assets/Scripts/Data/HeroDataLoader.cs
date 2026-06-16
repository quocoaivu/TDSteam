using System;
using System.Collections.Generic;
using GameCore;
using Parameter;
using UnityEngine;

namespace Data
{
	public class HeroDataLoader : BaseMonoBehaviour
	{
		private void Awake()
		{
			ReadHeroParameter();
			ReadHeroSkillParameter();
		}

		private void ReadHeroParameter()
		{
			string text = "Parameters/hero_parameter";
			try
			{
				List<Dictionary<string, object>> list = CSVLoader.Read(text);
				for (int i = 0; i < list.Count; i++)
				{
					HeroSpec heroParameter = default(HeroSpec);
					heroParameter.id = (int)list[i]["id"];
					heroParameter.name = (string)list[i]["name"];
					heroParameter.level = (int)list[i]["level"];
					heroParameter.exp_per_level = (int)list[i]["exp_per_level"];
					heroParameter.respawn_time = (int)list[i]["respawn_time"];
					heroParameter.health = (int)list[i]["health"];
					heroParameter.health_regen = (int)list[i]["health_regen"];
					heroParameter.health_regen_cooldown = (int)list[i]["health_regen_cooldown"];
					heroParameter.armor_physics = (int)list[i]["armor_physics"];
					heroParameter.armor_magic = (int)list[i]["armor_magic"];
					heroParameter.critical_strike_change = (int)list[i]["critical_strike_chance"];
					heroParameter.attack_physics_min = (int)list[i]["attack_physics_min"];
					heroParameter.attack_physics_max = (int)list[i]["attack_physics_max"];
					heroParameter.attack_magic_min = (int)list[i]["attack_magic_min"];
					heroParameter.attack_magic_max = (int)list[i]["attack_magic_max"];
					heroParameter.attack_cooldown = (int)list[i]["attack_cooldown"];
					heroParameter.attack_range_min = (int)list[i]["attack_range_min"];
					heroParameter.attack_range_average = (int)list[i]["attack_range_average"];
					heroParameter.attack_range_max = (int)list[i]["attack_range_max"];
					heroParameter.activate_range = (int)list[i]["activate_range"];
					heroParameter.speed = (int)list[i]["speed"];
					heroParameter.canAttackAir = (int)list[i]["can_attack_air"];
					heroParameter.skillPointBonus = (int)list[i]["skill_point_bonus"];
					HeroParameterManager.Instance.SetHeroParameter(heroParameter);
				}
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text);
				throw;
			}
		}

		private void ReadHeroSkillParameter()
		{
			ReadHeroSkillParam_hero00();
			ReadHeroSkillParam_hero01();
			ReadHeroSkillParam_hero02();
			ReadHeroSkillParam_hero03();
			ReadHeroSkillParam_hero04();
			ReadHeroSkillParam_hero05();
			ReadHeroSkillParam_hero06();
			ReadHeroSkillParam_hero07();
			ReadHeroSkillParam_hero08();
			ReadHeroSkillParam_hero09();
			ReadHeroSkillParam_hero10();
			ReadHeroSkillParam_hero11();
		}

		private void ReadHeroSkillParam_hero00()
		{
			int num = 0;
			int num2 = 0;
			string text = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list = CSVLoader.Read(text);
				HeroAbilitySpec_0_0 heroSkillParameter_0_ = new HeroAbilitySpec_0_0();
				for (int i = 0; i < list.Count; i++)
				{
					heroSkillParameter_0_.AddParamToList(new Hero0Ability0Specs
					{
						skill_level = (int)list[i]["skill_level"],
						number_clone = (int)list[i]["number_clone"],
						parameter_Scale = (int)list[i]["parameter_scale"],
						duration = (int)list[i]["duration"],
						cooldown_time = (int)list[i]["cooldown_time"],
						description = (string)list[i]["description"],
						use_type = (string)list[i]["use_type"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_0_);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text);
				throw;
			}
			num = 0;
			num2 = 1;
			string text2 = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list2 = CSVLoader.Read(text2);
				HeroAbilitySpec_0_1 heroSkillParameter_0_2 = new HeroAbilitySpec_0_1();
				for (int j = 0; j < list2.Count; j++)
				{
					heroSkillParameter_0_2.AddParamToList(new Hero0Ability1Specs
					{
						skill_level = (int)list2[j]["skill_level"],
						damage = (int)list2[j]["damage"],
						duration = (int)list2[j]["duration"],
						change_percent = (int)list2[j]["change_percent"],
						aoeRange = (int)list2[j]["aoe_range"],
						description = (string)list2[j]["description"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_0_2);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text2);
				throw;
			}
			num = 0;
			num2 = 2;
			string text3 = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list3 = CSVLoader.Read(text3);
				HeroAbilitySpec_0_2 heroSkillParameter_0_3 = new HeroAbilitySpec_0_2();
				for (int k = 0; k < list3.Count; k++)
				{
					heroSkillParameter_0_3.AddParamToList(new Hero0Ability2Specs
					{
						skill_level = (int)list3[k]["skill_level"],
						armor = (int)list3[k]["armor"],
						description = (string)list3[k]["description"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_0_3);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text3);
				throw;
			}
			num = 0;
			num2 = 3;
			string text4 = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list4 = CSVLoader.Read(text4);
				HeroAbilitySpec_0_3 heroSkillParameter_0_4 = new HeroAbilitySpec_0_3();
				for (int l = 0; l < list4.Count; l++)
				{
					heroSkillParameter_0_4.AddParamToList(new Hero0Ability3Specs
					{
						skill_level = (int)list4[l]["skill_level"],
						skill_range = (int)list4[l]["skill_range"],
						armor_per_unit = (int)list4[l]["armor_per_unit"],
						armor_max = (int)list4[l]["armor_max"],
						description = (string)list4[l]["description"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_0_4);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text4);
				throw;
			}
		}

		private void ReadHeroSkillParam_hero01()
		{
			int num = 1;
			int num2 = 0;
			string text = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list = CSVLoader.Read(text);
				HeroAbilitySpec_1_0 heroSkillParameter_1_ = new HeroAbilitySpec_1_0();
				for (int i = 0; i < list.Count; i++)
				{
					heroSkillParameter_1_.AddParamToList(new Hero1Ability0Specs
					{
						skill_level = (int)list[i]["skill_level"],
						number_of_projectile = (int)list[i]["number_of_projectile"],
						range = (int)list[i]["range"],
						offsetHigh = (int)list[i]["offset_high"],
						duration = (int)list[i]["duration"],
						delayTime = (int)list[i]["delay_time"],
						damage = (int)list[i]["damage"],
						cooldown_time = (int)list[i]["cooldown_time"],
						description = (string)list[i]["description"],
						use_type = (string)list[i]["use_type"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_1_);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text);
				throw;
			}
			num = 1;
			num2 = 1;
			string text2 = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list2 = CSVLoader.Read(text2);
				HeroAbilitySpec_1_1 heroSkillParameter_1_2 = new HeroAbilitySpec_1_1();
				for (int j = 0; j < list2.Count; j++)
				{
					heroSkillParameter_1_2.AddParamToList(new Hero1Ability1Specs
					{
						skill_level = (int)list2[j]["skill_level"],
						bonus_crit = (int)list2[j]["critical_strike_chance_bonus"],
						description = (string)list2[j]["description"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_1_2);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text2);
				throw;
			}
			num = 1;
			num2 = 2;
			string text3 = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list3 = CSVLoader.Read(text3);
				HeroAbilitySpec_1_2 heroSkillParameter_1_3 = new HeroAbilitySpec_1_2();
				for (int k = 0; k < list3.Count; k++)
				{
					heroSkillParameter_1_3.AddParamToList(new Hero1Ability2Specs
					{
						skill_level = (int)list3[k]["skill_level"],
						attack_speed_increase = (int)list3[k]["attack_speed_increase"],
						duration = (int)list3[k]["duration"],
						cooldown_time = (int)list3[k]["cooldown_time"],
						description = (string)list3[k]["description"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_1_3);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text3);
				throw;
			}
			num = 1;
			num2 = 3;
			string text4 = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list4 = CSVLoader.Read(text4);
				HeroAbilitySpec_1_3 heroSkillParameter_1_4 = new HeroAbilitySpec_1_3();
				for (int l = 0; l < list4.Count; l++)
				{
					heroSkillParameter_1_4.AddParamToList(new Hero1Ability3Specs
					{
						skill_level = (int)list4[l]["skill_level"],
						chance_to_cast = (int)list4[l]["chance_to_cast"],
						number_of_projectile = (int)list4[l]["number_of_projectile"],
						damage = (int)list4[l]["damage"],
						slow_percent = (int)list4[l]["slow_percent"],
						slow_time = (int)list4[l]["slow_time"],
						description = (string)list4[l]["description"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_1_4);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text4);
				throw;
			}
		}

		private void ReadHeroSkillParam_hero02()
		{
			int num = 2;
			int num2 = 0;
			string text = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list = CSVLoader.Read(text);
				HeroAbilitySpec_2_0 heroSkillParameter_2_ = new HeroAbilitySpec_2_0();
				for (int i = 0; i < list.Count; i++)
				{
					heroSkillParameter_2_.AddParamToList(new Hero2Ability0Specs
					{
						skill_level = (int)list[i]["skill_level"],
						number_clone = (int)list[i]["number_clone"],
						parameter_Scale = (int)list[i]["parameter_scale"],
						duration = (int)list[i]["duration"],
						cooldown_time = (int)list[i]["cooldown_time"],
						description = (string)list[i]["description"],
						use_type = (string)list[i]["use_type"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_2_);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text);
				throw;
			}
			num = 2;
			num2 = 1;
			string text2 = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list2 = CSVLoader.Read(text2);
				HeroAbilitySpec_2_1 heroSkillParameter_2_2 = new HeroAbilitySpec_2_1();
				for (int j = 0; j < list2.Count; j++)
				{
					heroSkillParameter_2_2.AddParamToList(new Hero2Ability1Specs
					{
						skill_level = (int)list2[j]["skill_level"],
						armorBonus = (int)list2[j]["armor_bonus"],
						duration = (int)list2[j]["duration"],
						cooldown_time = (int)list2[j]["cooldown_time"],
						description = (string)list2[j]["description"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_2_2);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text2);
				throw;
			}
			num = 2;
			num2 = 2;
			string text3 = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list3 = CSVLoader.Read(text3);
				HeroAbilitySpec_2_2 heroSkillParameter_2_3 = new HeroAbilitySpec_2_2();
				for (int k = 0; k < list3.Count; k++)
				{
					heroSkillParameter_2_3.AddParamToList(new Hero2Ability2Specs
					{
						skill_level = (int)list3[k]["skill_level"],
						count_crit = (int)list3[k]["count_crit"],
						description = (string)list3[k]["description"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_2_3);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text3);
				throw;
			}
			num = 2;
			num2 = 3;
			string text4 = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list4 = CSVLoader.Read(text4);
				HeroAbilitySpec_2_3 heroSkillParameter_2_4 = new HeroAbilitySpec_2_3();
				for (int l = 0; l < list4.Count; l++)
				{
					heroSkillParameter_2_4.AddParamToList(new Hero2Ability3Specs
					{
						skill_level = (int)list4[l]["skill_level"],
						aoeRange = (int)list4[l]["aoe_range"],
						damage = (int)list4[l]["damage"],
						slow_value = (int)list4[l]["slow_value"],
						slow_duration = (int)list4[l]["slow_duration"],
						cooldown_time = (int)list4[l]["cooldown"],
						description = (string)list4[l]["description"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_2_4);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text4);
				throw;
			}
		}

		private void ReadHeroSkillParam_hero03()
		{
			int num = 3;
			int num2 = 0;
			string text = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list = CSVLoader.Read(text);
				HeroAbilitySpec_3_0 heroSkillParameter_3_ = new HeroAbilitySpec_3_0();
				for (int i = 0; i < list.Count; i++)
				{
					heroSkillParameter_3_.AddParamToList(new Hero3Ability0Specs
					{
						skill_level = (int)list[i]["skill_level"],
						physics_damage = (int)list[i]["physics_damage"],
						magic_damage = (int)list[i]["magic_damage"],
						skill_range = (int)list[i]["skill_range"],
						meteor_speed = (int)list[i]["meteor_speed"],
						duration = (int)list[i]["duration"],
						cooldown_time = (int)list[i]["cooldown_time"],
						description = (string)list[i]["description"],
						use_type = (string)list[i]["use_type"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_3_);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text);
				throw;
			}
			num = 3;
			num2 = 1;
			string text2 = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list2 = CSVLoader.Read(text2);
				HeroAbilitySpec_3_1 heroSkillParameter_3_2 = new HeroAbilitySpec_3_1();
				for (int j = 0; j < list2.Count; j++)
				{
					heroSkillParameter_3_2.AddParamToList(new Hero3Ability1Specs
					{
						skill_level = (int)list2[j]["skill_level"],
						magic_damage_bonus = (int)list2[j]["magic_damage_bonus"],
						attack_speed_increase = (int)list2[j]["attack_speed_increase"],
						duration = (int)list2[j]["duration"],
						cooldown_time = (int)list2[j]["cooldown_time"],
						description = (string)list2[j]["description"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_3_2);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text2);
				throw;
			}
			num = 3;
			num2 = 2;
			string text3 = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list3 = CSVLoader.Read(text3);
				HeroAbilitySpec_3_2 heroSkillParameter_3_3 = new HeroAbilitySpec_3_2();
				for (int k = 0; k < list3.Count; k++)
				{
					heroSkillParameter_3_3.AddParamToList(new Hero3Ability2Specs
					{
						skill_level = (int)list3[k]["skill_level"],
						skill_range = (int)list3[k]["skill_range"],
						slow_percent = (int)list3[k]["slow_percent"],
						damage_burn = (int)list3[k]["damage_burn"],
						duration = (int)list3[k]["duration"],
						cooldown_time = (int)list3[k]["cooldown_time"],
						description = (string)list3[k]["description"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_3_3);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text3);
				throw;
			}
			num = 3;
			num2 = 3;
			string text4 = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list4 = CSVLoader.Read(text4);
				HeroAbilitySpec_3_3 heroSkillParameter_3_4 = new HeroAbilitySpec_3_3();
				for (int l = 0; l < list4.Count; l++)
				{
					heroSkillParameter_3_4.AddParamToList(new Hero3Ability3Specs
					{
						skill_level = (int)list4[l]["skill_level"],
						skill_range = (int)list4[l]["skill_range"],
						physics_damage = (int)list4[l]["physics_damage"],
						magic_damage = (int)list4[l]["magic_damage"],
						time_step = (int)list4[l]["time_step"],
						cooldown_time = (int)list4[l]["cooldown_time"],
						description = (string)list4[l]["description"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_3_4);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text4);
				throw;
			}
		}

		private void ReadHeroSkillParam_hero04()
		{
			int num = 4;
			int num2 = 0;
			string text = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list = CSVLoader.Read(text);
				HeroAbilitySpec_4_0 heroSkillParameter_4_ = new HeroAbilitySpec_4_0();
				for (int i = 0; i < list.Count; i++)
				{
					heroSkillParameter_4_.AddParamToList(new Hero4Ability0Specs
					{
						skill_level = (int)list[i]["skill_level"],
						physics_damage = (int)list[i]["physics_damage"],
						skill_range = (int)list[i]["skill_range"],
						duration = (int)list[i]["duration"],
						cooldown_time = (int)list[i]["cooldown_time"],
						description = (string)list[i]["description"],
						use_type = (string)list[i]["use_type"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_4_);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text);
				throw;
			}
			num = 4;
			num2 = 1;
			string text2 = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list2 = CSVLoader.Read(text2);
				HeroAbilitySpec_4_1 heroSkillParameter_4_2 = new HeroAbilitySpec_4_1();
				for (int j = 0; j < list2.Count; j++)
				{
					heroSkillParameter_4_2.AddParamToList(new Hero4Ability1Specs
					{
						skill_level = (int)list2[j]["skill_level"],
						physics_armor_bonus = (int)list2[j]["physics_armor_bonus"],
						magic_armor_bonus = (int)list2[j]["magic_armor_bonus"],
						duration = (int)list2[j]["duration"],
						cooldown_time = (int)list2[j]["cooldown_time"],
						description = (string)list2[j]["description"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_4_2);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text2);
				throw;
			}
			num = 4;
			num2 = 2;
			string text3 = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list3 = CSVLoader.Read(text3);
				HeroAbilitySpec_4_2 heroSkillParameter_4_3 = new HeroAbilitySpec_4_2();
				for (int k = 0; k < list3.Count; k++)
				{
					heroSkillParameter_4_3.AddParamToList(new Hero4Ability2Specs
					{
						skill_level = (int)list3[k]["skill_level"],
						change_to_stun = (int)list3[k]["change_to_stun"],
						duration = (int)list3[k]["duration"],
						description = (string)list3[k]["description"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_4_3);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text3);
				throw;
			}
			num = 4;
			num2 = 3;
			string text4 = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list4 = CSVLoader.Read(text4);
				HeroAbilitySpec_4_3 heroSkillParameter_4_4 = new HeroAbilitySpec_4_3();
				for (int l = 0; l < list4.Count; l++)
				{
					heroSkillParameter_4_4.AddParamToList(new Hero4Ability3Specs
					{
						skill_level = (int)list4[l]["skill_level"],
						general_attack_damage_bonus = (int)list4[l]["general_attack_damage_bonus"],
						attack_speed_bonus = (int)list4[l]["attack_speed_bonus"],
						movement_speed_bonus = (int)list4[l]["movement_speed_bonus"],
						duration = (int)list4[l]["duration"],
						cooldown_time = (int)list4[l]["cooldown_time"],
						description = (string)list4[l]["description"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_4_4);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text4);
				throw;
			}
		}

		private void ReadHeroSkillParam_hero05()
		{
			int num = 5;
			int num2 = 0;
			string text = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list = CSVLoader.Read(text);
				HeroAbilitySpec_5_0 heroSkillParameter_5_ = new HeroAbilitySpec_5_0();
				for (int i = 0; i < list.Count; i++)
				{
					heroSkillParameter_5_.AddParamToList(new Hero5Ability0Specs
					{
						skill_level = (int)list[i]["skill_level"],
						heal_amount = (int)list[i]["heal_amount"],
						skill_range = (int)list[i]["skill_range"],
						cooldown_time = (int)list[i]["cooldown_time"],
						description = (string)list[i]["description"],
						use_type = (string)list[i]["use_type"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_5_);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text);
				throw;
			}
			num = 5;
			num2 = 1;
			string text2 = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list2 = CSVLoader.Read(text2);
				HeroAbilitySpec_5_1 heroSkillParameter_5_2 = new HeroAbilitySpec_5_1();
				for (int j = 0; j < list2.Count; j++)
				{
					heroSkillParameter_5_2.AddParamToList(new Hero5Ability1Specs
					{
						skill_level = (int)list2[j]["skill_level"],
						bonus_damage = (int)list2[j]["bonus_damage"],
						duration = (int)list2[j]["duration"],
						description = (string)list2[j]["description"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_5_2);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text2);
				throw;
			}
			num = 5;
			num2 = 2;
			string text3 = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list3 = CSVLoader.Read(text3);
				HeroAbilitySpec_5_2 heroSkillParameter_5_3 = new HeroAbilitySpec_5_2();
				for (int k = 0; k < list3.Count; k++)
				{
					heroSkillParameter_5_3.AddParamToList(new Hero5Ability2Specs
					{
						skill_level = (int)list3[k]["skill_level"],
						skill_range = (int)list3[k]["skill_range"],
						enemy_affected = (int)list3[k]["enemy_affected"],
						enemy_min = (int)list3[k]["enemy_min"],
						duration = (int)list3[k]["duration"],
						cooldown_time = (int)list3[k]["cooldown_time"],
						description = (string)list3[k]["description"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_5_3);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text3);
				throw;
			}
			num = 5;
			num2 = 3;
			string text4 = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list4 = CSVLoader.Read(text4);
				HeroAbilitySpec_5_3 heroSkillParameter_5_4 = new HeroAbilitySpec_5_3();
				for (int l = 0; l < list4.Count; l++)
				{
					heroSkillParameter_5_4.AddParamToList(new Hero5Ability3Specs
					{
						skill_level = (int)list4[l]["skill_level"],
						arrow_number = (int)list4[l]["arrow_number"],
						skill_range = (int)list4[l]["skill_range"],
						damage_physics = (int)list4[l]["damage_physics"],
						duration = (int)list4[l]["duration"],
						delay_time = (int)list4[l]["delay_time"],
						cooldown_time = (int)list4[l]["cooldown_time"],
						description = (string)list4[l]["description"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_5_4);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text4);
				throw;
			}
		}

		private void ReadHeroSkillParam_hero06()
		{
			int num = 6;
			int num2 = 0;
			string text = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list = CSVLoader.Read(text);
				HeroAbilitySpec_6_0 heroSkillParameter_6_ = new HeroAbilitySpec_6_0();
				for (int i = 0; i < list.Count; i++)
				{
					heroSkillParameter_6_.AddParamToList(new Hero6Ability0Specs
					{
						skill_level = (int)list[i]["skill_level"],
						physics_damage = (int)list[i]["physics_damage"],
						magic_damage = (int)list[i]["magic_damage"],
						stun_duration = (int)list[i]["stun_duration"],
						skill_range = (int)list[i]["skill_range"],
						cooldown_time = (int)list[i]["cooldown_time"],
						description = (string)list[i]["description"],
						use_type = (string)list[i]["use_type"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_6_);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text);
				throw;
			}
			num = 6;
			num2 = 1;
			string text2 = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list2 = CSVLoader.Read(text2);
				HeroAbilitySpec_6_1 heroSkillParameter_6_2 = new HeroAbilitySpec_6_1();
				for (int j = 0; j < list2.Count; j++)
				{
					heroSkillParameter_6_2.AddParamToList(new Hero6Ability1Specs
					{
						skill_level = (int)list2[j]["skill_level"],
						percent_attack_damage_bonus = (int)list2[j]["percent_attack_damage_bonus"],
						duration = (int)list2[j]["duration"],
						cooldown_time = (int)list2[j]["cooldown_time"],
						description = (string)list2[j]["description"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_6_2);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text2);
				throw;
			}
			num = 6;
			num2 = 2;
			string text3 = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list3 = CSVLoader.Read(text3);
				HeroAbilitySpec_6_2 heroSkillParameter_6_3 = new HeroAbilitySpec_6_2();
				for (int k = 0; k < list3.Count; k++)
				{
					heroSkillParameter_6_3.AddParamToList(new Hero6Ability2Specs
					{
						skill_level = (int)list3[k]["skill_level"],
						aoe_range = (int)list3[k]["aoe_range"],
						physics_damage = (int)list3[k]["physics_damage"],
						magic_damage = (int)list3[k]["magic_damage"],
						cooldown_time = (int)list3[k]["cooldown_time"],
						description = (string)list3[k]["description"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_6_3);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text3);
				throw;
			}
			num = 6;
			num2 = 3;
			string text4 = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list4 = CSVLoader.Read(text4);
				HeroAbilitySpec_6_3 heroSkillParameter_6_4 = new HeroAbilitySpec_6_3();
				for (int l = 0; l < list4.Count; l++)
				{
					heroSkillParameter_6_4.AddParamToList(new Hero6Ability3Specs
					{
						skill_level = (int)list4[l]["skill_level"],
						physics_damage = (int)list4[l]["physics_damage"],
						magic_damage = (int)list4[l]["magic_damage"],
						skill_range = (int)list4[l]["skill_range"],
						cooldown_time = (int)list4[l]["cooldown_time"],
						description = (string)list4[l]["description"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_6_4);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text4);
				throw;
			}
		}

		private void ReadHeroSkillParam_hero07()
		{
			int num = 7;
			int num2 = 0;
			string text = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list = CSVLoader.Read(text);
				HeroAbilitySpec_7_0 heroSkillParameter_7_ = new HeroAbilitySpec_7_0();
				for (int i = 0; i < list.Count; i++)
				{
					heroSkillParameter_7_.AddParamToList(new Hero7Ability0Specs
					{
						skill_level = (int)list[i]["skill_level"],
						physics_damage = (int)list[i]["physics_damage"],
						magic_damage = (int)list[i]["magic_damage"],
						skill_range = (int)list[i]["skill_range"],
						cooldown_time = (int)list[i]["cooldown_time"],
						description = (string)list[i]["description"],
						use_type = (string)list[i]["use_type"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_7_);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text);
				throw;
			}
			num = 7;
			num2 = 1;
			string text2 = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list2 = CSVLoader.Read(text2);
				HeroAbilitySpec_7_1 heroSkillParameter_7_2 = new HeroAbilitySpec_7_1();
				for (int j = 0; j < list2.Count; j++)
				{
					heroSkillParameter_7_2.AddParamToList(new Hero7Ability1Specs
					{
						skill_level = (int)list2[j]["skill_level"],
						percent_health_activate = (int)list2[j]["percent_health_activate"],
						amount_of_trap = (int)list2[j]["amount_of_trap"],
						trap_life_time = (int)list2[j]["trap_life_time"],
						physics_damage = (int)list2[j]["physics_damage"],
						magic_damage = (int)list2[j]["magic_damage"],
						slow_percent = (int)list2[j]["slow_percent"],
						slow_duration = (int)list2[j]["slow_duration"],
						skill_range = (int)list2[j]["skill_range"],
						cooldown_time = (int)list2[j]["cooldown_time"],
						description = (string)list2[j]["description"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_7_2);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text2);
				throw;
			}
			num = 7;
			num2 = 2;
			string text3 = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list3 = CSVLoader.Read(text3);
				HeroAbilitySpec_7_2 heroSkillParameter_7_3 = new HeroAbilitySpec_7_2();
				for (int k = 0; k < list3.Count; k++)
				{
					heroSkillParameter_7_3.AddParamToList(new Hero7Ability2Specs
					{
						skill_level = (int)list3[k]["skill_level"],
						chance_to_cast = (int)list3[k]["chance_to_cast"],
						skill_range = (int)list3[k]["skill_range"],
						physics_damage = (int)list3[k]["physics_damage"],
						magic_damage = (int)list3[k]["magic_damage"],
						cooldown_time = (int)list3[k]["cooldown_time"],
						description = (string)list3[k]["description"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_7_3);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text3);
				throw;
			}
			num = 7;
			num2 = 3;
			string text4 = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list4 = CSVLoader.Read(text4);
				HeroAbilitySpec_7_3 heroSkillParameter_7_4 = new HeroAbilitySpec_7_3();
				for (int l = 0; l < list4.Count; l++)
				{
					heroSkillParameter_7_4.AddParamToList(new Hero7Ability3Specs
					{
						skill_level = (int)list4[l]["skill_level"],
						chance_to_cast = (int)list4[l]["chance_to_cast"],
						physics_damage = (int)list4[l]["physics_damage"],
						magic_damage = (int)list4[l]["magic_damage"],
						skill_range = (int)list4[l]["skill_range"],
						countdown_time = (int)list4[l]["countdown_time"],
						cooldown_time = (int)list4[l]["cooldown_time"],
						description = (string)list4[l]["description"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_7_4);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text4);
				throw;
			}
		}

		private void ReadHeroSkillParam_hero08()
		{
			int num = 8;
			int num2 = 0;
			string text = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list = CSVLoader.Read(text);
				HeroAbilitySpec_8_0 heroSkillParameter_8_ = new HeroAbilitySpec_8_0();
				for (int i = 0; i < list.Count; i++)
				{
					heroSkillParameter_8_.AddParamToList(new Hero8Ability0Specs
					{
						skill_level = (int)list[i]["skill_level"],
						number_of_projectile = (int)list[i]["number_of_projectile"],
						physics_damage = (int)list[i]["physics_damage"],
						magic_damage = (int)list[i]["magic_damage"],
						skill_range = (int)list[i]["skill_range"],
						cooldown_time = (int)list[i]["cooldown_time"],
						description = (string)list[i]["description"],
						use_type = (string)list[i]["use_type"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_8_);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text);
				throw;
			}
			num = 8;
			num2 = 1;
			string text2 = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list2 = CSVLoader.Read(text2);
				HeroAbilitySpec_8_1 heroSkillParameter_8_2 = new HeroAbilitySpec_8_1();
				for (int j = 0; j < list2.Count; j++)
				{
					heroSkillParameter_8_2.AddParamToList(new Hero8Ability1Specs
					{
						skill_level = (int)list2[j]["skill_level"],
						physics_damage = (int)list2[j]["physics_damage"],
						magic_damage = (int)list2[j]["magic_damage"],
						attack_damage_decrease_percentage = (int)list2[j]["attack_damage_decrease_percentage"],
						duration = (int)list2[j]["duration"],
						knock_back_distance = (int)list2[j]["knock_back_distance"],
						skill_range = (int)list2[j]["skill_range"],
						cooldown_time = (int)list2[j]["cooldown_time"],
						description = (string)list2[j]["description"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_8_2);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text2);
				throw;
			}
			num = 8;
			num2 = 2;
			string text3 = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list3 = CSVLoader.Read(text3);
				HeroAbilitySpec_8_2 heroSkillParameter_8_3 = new HeroAbilitySpec_8_2();
				for (int k = 0; k < list3.Count; k++)
				{
					heroSkillParameter_8_3.AddParamToList(new Hero8Ability2Specs
					{
						skill_level = (int)list3[k]["skill_level"],
						skill_range = (int)list3[k]["skill_range"],
						duration = (int)list3[k]["duration"],
						cooldown_time = (int)list3[k]["cooldown_time"],
						description = (string)list3[k]["description"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_8_3);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text3);
				throw;
			}
			num = 8;
			num2 = 3;
			string text4 = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list4 = CSVLoader.Read(text4);
				HeroAbilitySpec_8_3 heroSkillParameter_8_4 = new HeroAbilitySpec_8_3();
				for (int l = 0; l < list4.Count; l++)
				{
					heroSkillParameter_8_4.AddParamToList(new Hero8Ability3Specs
					{
						skill_level = (int)list4[l]["skill_level"],
						attack_range_bonus_percentage = (int)list4[l]["attack_range_bonus_percentage"],
						description = (string)list4[l]["description"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_8_4);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text4);
				throw;
			}
		}

		private void ReadHeroSkillParam_hero09()
		{
			int num = 9;
			int num2 = 0;
			string text = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list = CSVLoader.Read(text);
				HeroAbilitySpec_9_0 heroSkillParameter_9_ = new HeroAbilitySpec_9_0();
				for (int i = 0; i < list.Count; i++)
				{
					heroSkillParameter_9_.AddParamToList(new Hero9Ability0Specs
					{
						skill_level = (int)list[i]["skill_level"],
						physics_damage = (int)list[i]["physics_damage"],
						magic_damage = (int)list[i]["magic_damage"],
						number_of_minion = (int)list[i]["number_of_minion"],
						minion_attack_range = (int)list[i]["minion_attack_range"],
						minion_attack_cooldown = (int)list[i]["minion_attack_cooldown"],
						minion_lifetime = (int)list[i]["minion_lifetime"],
						skill_range = (int)list[i]["skill_range"],
						cooldown_time = (int)list[i]["cooldown_time"],
						description = (string)list[i]["description"],
						use_type = (string)list[i]["use_type"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_9_);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text);
				throw;
			}
			num = 9;
			num2 = 1;
			string text2 = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list2 = CSVLoader.Read(text2);
				HeroAbilitySpec_9_1 heroSkillParameter_9_2 = new HeroAbilitySpec_9_1();
				for (int j = 0; j < list2.Count; j++)
				{
					heroSkillParameter_9_2.AddParamToList(new Hero9Ability1Specs
					{
						skill_level = (int)list2[j]["skill_level"],
						enemy_affected = (int)list2[j]["enemy_affected"],
						knock_back_distance = (int)list2[j]["knock_back_distance"],
						knock_back_duration = (int)list2[j]["knock_back_duration"],
						skill_range = (int)list2[j]["skill_range"],
						cooldown_time = (int)list2[j]["cooldown_time"],
						description = (string)list2[j]["description"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_9_2);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text2);
				throw;
			}
			num = 9;
			num2 = 2;
			string text3 = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list3 = CSVLoader.Read(text3);
				HeroAbilitySpec_9_2 heroSkillParameter_9_3 = new HeroAbilitySpec_9_2();
				for (int k = 0; k < list3.Count; k++)
				{
					heroSkillParameter_9_3.AddParamToList(new Hero9Ability2Specs
					{
						skill_level = (int)list3[k]["skill_level"],
						health_percentage_active = (int)list3[k]["health_percentage_active"],
						health_amount = (int)list3[k]["health_amount"],
						duration = (int)list3[k]["duration"],
						cooldown_time = (int)list3[k]["cooldown_time"],
						description = (string)list3[k]["description"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_9_3);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text3);
				throw;
			}
			num = 9;
			num2 = 3;
			string text4 = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list4 = CSVLoader.Read(text4);
				HeroAbilitySpec_9_3 heroSkillParameter_9_4 = new HeroAbilitySpec_9_3();
				for (int l = 0; l < list4.Count; l++)
				{
					heroSkillParameter_9_4.AddParamToList(new Hero9Ability3Specs
					{
						skill_level = (int)list4[l]["skill_level"],
						number_clone = (int)list4[l]["number_clone"],
						parameter_Scale = (int)list4[l]["parameter_Scale"],
						duration = (int)list4[l]["duration"],
						cooldown_time = (int)list4[l]["cooldown_time"],
						description = (string)list4[l]["description"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_9_4);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text4);
				throw;
			}
		}

		private void ReadHeroSkillParam_hero10()
		{
			int num = 10;
			int num2 = 0;
			string text = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list = CSVLoader.Read(text);
				HeroAbilitySpec_10_0 heroSkillParameter_10_ = new HeroAbilitySpec_10_0();
				for (int i = 0; i < list.Count; i++)
				{
					heroSkillParameter_10_.AddParamToList(new Hero10Ability0Specs
					{
						skill_level = (int)list[i]["skill_level"],
						skill_range = (int)list[i]["skill_range"],
						duration = (int)list[i]["duration"],
						magic_damage = (int)list[i]["magic_damage"],
						cooldown_time = (int)list[i]["cooldown_time"],
						use_type = (string)list[i]["use_type"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_10_);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text);
				throw;
			}
			num2 = 1;
			string text2 = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list2 = CSVLoader.Read(text2);
				HeroAbilitySpec_10_1 heroSkillParameter_10_2 = new HeroAbilitySpec_10_1();
				for (int j = 0; j < list2.Count; j++)
				{
					heroSkillParameter_10_2.AddParamToList(new Hero10Ability1Specs
					{
						skill_level = (int)list2[j]["skill_level"],
						skill_range = (int)list2[j]["skill_range"],
						magic_damage = (int)list2[j]["magic_damage"],
						def_down_duration = (int)list2[j]["def_down_duration"],
						def_down_percent = (int)list2[j]["def_down_percent"],
						cooldown_time = (int)list2[j]["cooldown_time"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_10_2);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text2);
				throw;
			}
			num2 = 2;
			string text3 = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list3 = CSVLoader.Read(text3);
				HeroAbilitySpec_10_2 heroSkillParameter_10_3 = new HeroAbilitySpec_10_2();
				for (int k = 0; k < list3.Count; k++)
				{
					heroSkillParameter_10_3.AddParamToList(new Hero10Ability2Specs
					{
						skill_level = (int)list3[k]["skill_level"],
						physic_damage = (int)list3[k]["physic_damage"],
						cooldown_time = (int)list3[k]["cooldown_time"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_10_3);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text3);
				throw;
			}
			num2 = 3;
			string text4 = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list4 = CSVLoader.Read(text4);
				HeroAbilitySpec_10_3 heroSkillParameter_10_4 = new HeroAbilitySpec_10_3();
				for (int l = 0; l < list4.Count; l++)
				{
					heroSkillParameter_10_4.AddParamToList(new Hero10Ability3Specs
					{
						skill_level = (int)list4[l]["skill_level"],
						magic_damage = (int)list4[l]["magic_damage"],
						skill_range = (int)list4[l]["skill_range"],
						stun_duration = (int)list4[l]["stun_duration"],
						cooldown_time = (int)list4[l]["cooldown_time"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_10_4);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text4);
				throw;
			}
		}

		private void ReadHeroSkillParam_hero11()
		{
			int num = 11;
			int num2 = 0;
			string text = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list = CSVLoader.Read(text);
				HeroAbilitySpec_11_0 heroSkillParameter_11_ = new HeroAbilitySpec_11_0();
				for (int i = 0; i < list.Count; i++)
				{
					heroSkillParameter_11_.AddParamToList(new Hero11Ability0Specs
					{
						skill_level = (int)list[i]["skill_level"],
						skill_range = (int)list[i]["skill_range"],
						fire_road_duration = (int)list[i]["fire_road_duration"],
						magic_damage = (int)list[i]["magic_damage"],
						cooldown_time = (int)list[i]["cooldown_time"],
						use_type = (string)list[i]["use_type"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_11_);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text);
				throw;
			}
			num2 = 1;
			string text2 = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list2 = CSVLoader.Read(text2);
				HeroAbilitySpec_11_1 heroSkillParameter_11_2 = new HeroAbilitySpec_11_1();
				for (int j = 0; j < list2.Count; j++)
				{
					heroSkillParameter_11_2.AddParamToList(new Hero11Ability1Specs
					{
						skill_level = (int)list2[j]["skill_level"],
						skill_range = (int)list2[j]["skill_range"],
						magic_damage = (int)list2[j]["magic_damage"],
						cooldown_time = (int)list2[j]["cooldown_time"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_11_2);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text2);
				throw;
			}
			num2 = 2;
			string text3 = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list3 = CSVLoader.Read(text3);
				HeroAbilitySpec_11_2 heroSkillParameter_11_3 = new HeroAbilitySpec_11_2();
				for (int k = 0; k < list3.Count; k++)
				{
					heroSkillParameter_11_3.AddParamToList(new Hero11Ability2Specs
					{
						skill_level = (int)list3[k]["skill_level"],
						magic_damage = (int)list3[k]["magic_damage"],
						explode_range = (int)list3[k]["explode_range"],
						minion_lifetime = (int)list3[k]["minion_lifetime"],
						minion_quantity = (int)list3[k]["minion_quantity"],
						cooldown_time = (int)list3[k]["cooldown_time"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_11_3);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text3);
				throw;
			}
			num2 = 3;
			string text4 = string.Format("Parameters/HeroSkills/hero_skill_{0}_{1}", num, num2);
			try
			{
				List<Dictionary<string, object>> list4 = CSVLoader.Read(text4);
				HeroAbilitySpec_11_3 heroSkillParameter_11_4 = new HeroAbilitySpec_11_3();
				for (int l = 0; l < list4.Count; l++)
				{
					heroSkillParameter_11_4.AddParamToList(new Hero11Ability3Specs
					{
						skill_level = (int)list4[l]["skill_level"],
						total_heal = (int)list4[l]["total_heal"],
						heal_range = (int)list4[l]["heal_range"],
						heal_duration = (int)list4[l]["heal_duration"],
						cooldown_time = (int)list4[l]["cooldown_time"]
					});
				}
				HeroAbilitySpec.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_11_4);
			}
			catch (Exception)
			{
				HeroDataLoader.ShowError(text4);
				throw;
			}
		}

		private static void ShowError(string filePath)
		{
			UnityEngine.Debug.LogError("File " + filePath + ".csv khÃ´ng tá»“n táº¡i hoáº·c dá»¯ liá»‡u trong file khÃ´ng Ä‘Ãºng Ä‘á»‹nh dáº¡ng.");
		}
	}
}
