using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_5_2 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero5Ability2Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.enemy_affected);
		}

		public Hero5Ability2Specs getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}

		public List<Hero5Ability2Specs> listParam = new List<Hero5Ability2Specs>();
	}
}
