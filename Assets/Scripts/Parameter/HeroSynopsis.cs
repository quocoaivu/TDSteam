using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroSynopsis : Singleton<HeroSynopsis>
	{
		public void ClearData()
		{
			listHeroDes.Clear();
		}

		public void SetHeroParameter(HeroBrief hero)
		{
			int count = listHeroDes.Count;
			if (count <= hero.id)
			{
				List<HeroBrief> list = new List<HeroBrief>();
				list.Insert(hero.skillID, hero);
				listHeroDes.Insert(hero.id, list);
			}
			else
			{
				List<HeroBrief> list2 = listHeroDes[hero.id];
				list2.Insert(hero.skillID, hero);
			}
		}

		public HeroBrief GetHeroParameter(int id, int level)
		{
			if (CheckParameter(id, level))
			{
				return listHeroDes[id][level];
			}
			return default(HeroBrief);
		}

		private bool CheckParameter(int id, int level)
		{
			return id < GetNumberOfHero() && level <= GetNumberOfLevel();
		}

		public int GetNumberOfHero()
		{
			return listHeroDes.Count;
		}

		public int GetNumberOfLevel()
		{
			if (GetNumberOfHero() > 0)
			{
				return listHeroDes[0].Count;
			}
			return 0;
		}

		public string GetHeroName(int heroId)
		{
			if (heroId >= 0 && heroId < listHeroDes.Count)
			{
				return listHeroDes[heroId][0].name;
			}
			return "_";
		}

		public string GetHeroShortDescription(int heroId)
		{
			if (heroId >= 0 && heroId < listHeroDes.Count)
			{
				return listHeroDes[heroId][0].shortDescription;
			}
			return "_";
		}

		public string GetHeroFullDescription(int heroId)
		{
			if (heroId >= 0 && heroId < listHeroDes.Count)
			{
				return listHeroDes[heroId][0].fullDescription;
			}
			return "_";
		}

		public string GetHeroSkillName(int heroId, int skillID)
		{
			if (heroId >= 0 && heroId < listHeroDes.Count)
			{
				return listHeroDes[heroId][skillID].skillName;
			}
			return "_";
		}

		public string GetHeroSkillType(int heroId, int skillID)
		{
			if (heroId >= 0 && heroId < listHeroDes.Count)
			{
				return listHeroDes[heroId][skillID].skillType;
			}
			return "_";
		}

		public string GetHeroSkillDescription(int heroId, int skillID)
		{
			if (heroId >= 0 && heroId < listHeroDes.Count)
			{
				return listHeroDes[heroId][skillID].skillDescription;
			}
			return "_";
		}

		public string GetHeroSkillUnlock(int heroId, int skillID)
		{
			if (heroId >= 0 && heroId < listHeroDes.Count)
			{
				return listHeroDes[heroId][skillID].skillUnlock;
			}
			return "_";
		}

		public List<List<HeroBrief>> listHeroDes = new List<List<HeroBrief>>();
	}
}
