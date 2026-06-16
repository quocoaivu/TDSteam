using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_1_2 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero1Ability2Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.duration / 1000f);
		}

		public Hero1Ability2Specs getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}

		public List<Hero1Ability2Specs> listParam = new List<Hero1Ability2Specs>();
	}
}
