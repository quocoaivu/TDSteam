using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_2_2 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero2Ability2Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.count_crit);
		}

		public Hero2Ability2Specs getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}

		public List<Hero2Ability2Specs> listParam = new List<Hero2Ability2Specs>();
	}
}
