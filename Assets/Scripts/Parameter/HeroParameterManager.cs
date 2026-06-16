using System;
using System.Collections.Generic;
using Data;
using MetaGame;

namespace Parameter
{
	public class HeroParameterManager
	{
		public static HeroParameterManager Instance
		{
			get
			{
				if (HeroParameterManager.instance == null)
				{
					HeroParameterManager.instance = new HeroParameterManager();
				}
				return HeroParameterManager.instance;
			}
		}

		public void SetHeroParameter(HeroSpec hero)
		{
			int count = listHero.Count;
			if (count <= hero.id)
			{
				List<HeroSpec> list = new List<HeroSpec>();
				list.Insert(hero.level, hero);
				listHero.Insert(hero.id, list);
			}
			else
			{
				List<HeroSpec> list2 = listHero[hero.id];
				list2.Insert(hero.level, hero);
			}
		}

		public HeroSpec GetHeroParameter(int id, int level)
		{
			if (CheckParameter(id, level))
			{
				return listHero[id][level];
			}
			return default(HeroSpec);
		}

		public HeroSpec GetPetParameter(PetSetupRecord petConfigData)
		{
			return new HeroSpec
			{
				id = petConfigData.Id + 1000,
				name = petConfigData.Petname,
				level = 1,
				respawn_time = petConfigData.Respawn_time,
				health = petConfigData.Health,
				health_regen = petConfigData.Health_regen,
				health_regen_cooldown = petConfigData.Health_regen_cooldown,
				armor_physics = petConfigData.Armor_physics,
				armor_magic = petConfigData.Armor_magic,
				critical_strike_change = 0,
				attack_physics_min = petConfigData.Atk_physics_min,
				attack_physics_max = petConfigData.Atk_physics_max,
				attack_magic_min = petConfigData.Atk_magic_min,
				attack_magic_max = petConfigData.Atk_magic_max,
				attack_cooldown = petConfigData.Atk_cooldown,
				attack_range_min = petConfigData.Atk_range_min,
				attack_range_max = petConfigData.Atk_range_max,
				attack_range_average = petConfigData.Atk_range_avg,
				speed = petConfigData.Speed,
				canAttackAir = petConfigData.Can_attack_air
			};
		}

		private bool CheckParameter(int id, int level)
		{
			return id < GetNumberOfHero() && level <= GetNumberOfLevel();
		}

		public int GetNumberOfHero()
		{
			return listHero.Count;
		}

		public int GetNumberOfLevel()
		{
			if (GetNumberOfHero() > 0)
			{
				return listHero[0].Count;
			}
			return 0;
		}

		public List<int> GetListHeroID()
		{
			List<int> list = new List<int>();
			for (int i = 0; i <= listHero.Count - 1; i++)
			{
				list.Add(i);
			}
			return list;
		}

		public List<int> GetListPetID()
		{
			List<int> list = new List<int>();
			List<int> listHeroID = GetListHeroID();
			foreach (int heroID in listHeroID)
			{
				list.Add(GetPetID(heroID));
			}
			return list;
		}

		public string GetHeroName(int heroId)
		{
			if (heroId >= 0 && heroId < listHero.Count)
			{
				return listHero[heroId][0].name;
			}
			return "_";
		}

		public int GetHeroHealth(int heroId, int heroLevel)
		{
			if (heroId >= 0 && heroId < listHero.Count)
			{
				return listHero[heroId][heroLevel].health;
			}
			return 0;
		}

		public int GetHeroHealthRegen(int heroId, int heroLevel)
		{
			if (heroId >= 0 && heroId < listHero.Count)
			{
				return listHero[heroId][heroLevel].health_regen;
			}
			return 0;
		}

		public int GetHeroAttackCooldown(int heroId, int heroLevel)
		{
			if (heroId >= 0 && heroId < listHero.Count)
			{
				return listHero[heroId][heroLevel].attack_cooldown;
			}
			return 0;
		}

		public bool IsPhysicsAttack(int heroId)
		{
			bool result = false;
			if (heroId >= 0 && heroId < listHero.Count)
			{
				result = (listHero[heroId][0].attack_physics_min > 0);
			}
			return result;
		}

		public bool IsMagicAttack(int heroId)
		{
			bool result = false;
			if (heroId >= 0 && heroId < listHero.Count)
			{
				result = (listHero[heroId][0].attack_magic_min > 0);
			}
			return result;
		}

		public bool CanAttackAir(int heroId)
		{
			bool result = false;
			if (heroId >= 0 && heroId < listHero.Count)
			{
				result = (listHero[heroId][0].canAttackAir == 1);
			}
			return result;
		}

		public int GetHeroDamageMax(int heroId, int heroLevel)
		{
			int result;
			if (heroId >= 0 && heroId < listHero.Count)
			{
				if (IsPhysicsAttack(heroId))
				{
					result = listHero[heroId][heroLevel].attack_physics_max;
				}
				else
				{
					result = listHero[heroId][heroLevel].attack_magic_max;
				}
			}
			else
			{
				result = 0;
			}
			return result;
		}

		public string GetHeroDamageRange(int heroId, int heroLevel)
		{
			string result = string.Empty;
			if (heroId >= 0 && heroId < listHero.Count)
			{
				if (IsPhysicsAttack(heroId))
				{
					result = listHero[heroId][heroLevel].attack_physics_min + " - " + listHero[heroId][heroLevel].attack_physics_max;
				}
				else
				{
					result = listHero[heroId][heroLevel].attack_magic_min + " - " + listHero[heroId][heroLevel].attack_magic_max;
				}
			}
			else
			{
				result = "_";
			}
			return result;
		}

		public int GetHeroAttackRange(int heroId, int heroLevel)
		{
			if (heroId >= 0 && heroId < listHero.Count)
			{
				return listHero[heroId][heroLevel].attack_range_max;
			}
			return 0;
		}

		public int GetHeroPhysicsArmor(int heroId, int heroLevel)
		{
			if (heroId >= 0 && heroId < listHero.Count)
			{
				return listHero[heroId][heroLevel].armor_physics;
			}
			return 0;
		}

		public int GetHeroMagicArmor(int heroId, int heroLevel)
		{
			if (heroId >= 0 && heroId < listHero.Count)
			{
				return listHero[heroId][heroLevel].armor_magic;
			}
			return 0;
		}

		public int GetHeroMovementSpeed(int heroId, int heroLevel)
		{
			if (heroId >= 0 && heroId < listHero.Count)
			{
				return listHero[heroId][heroLevel].speed;
			}
			return 0;
		}

		public int GetSkillPoint(int heroId, int heroLevel, int skillId)
		{
			int result = -1;
			GameFormat gameMode = FormatDirector.Instance.gameMode;
			if (gameMode != GameFormat.CampaignMode)
			{
				if (gameMode != GameFormat.DailyTrialMode)
				{
					if (gameMode == GameFormat.TournamentMode)
					{
						if (heroId >= 0 && heroId < listHero.Count)
						{
							result = HeroStore.Instance.GetSkillPoint(heroId, skillId);
						}
						else
						{
							result = -1;
						}
					}
				}
				else
				{
					result = HeroStore.maxSkillLevel;
				}
			}
			else if (heroId >= 0 && heroId < listHero.Count)
			{
				result = HeroStore.Instance.GetSkillPoint(heroId, skillId);
			}
			else
			{
				result = -1;
			}
			return result;
		}

		public int GetTotalSkillPoint(int heroID, int heroLevel)
		{
			int num = 0;
			for (int i = 0; i <= heroLevel; i++)
			{
				num += listHero[heroID][i].skillPointBonus;
			}
			return num;
		}

		public int GetEXPForCurrentLevel(int heroId, int heroLevel)
		{
			if (heroId >= 0 && heroId < listHero.Count)
			{
				return listHero[heroId][heroLevel].exp_per_level;
			}
			return 0;
		}

		public int GetEXPForNextLevel(int heroId, int heroLevel)
		{
			if (heroId < 0 || heroId >= listHero.Count)
			{
				return 0;
			}
			if (heroLevel == 0)
			{
				return listHero[heroId][heroLevel].exp_per_level;
			}
			return listHero[heroId][heroLevel].exp_per_level - listHero[heroId][heroLevel - 1].exp_per_level;
		}

		public int GetPetID(int heroID)
		{
			return heroID + 1000;
		}

		public List<List<HeroSpec>> listHero = new List<List<HeroSpec>>();

		private static HeroParameterManager instance;
		[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			instance = null;
		}
	}
}
