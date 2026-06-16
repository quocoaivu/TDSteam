using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_1_3 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero1Ability3Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.slow_percent);
		}

		public Hero1Ability3Specs getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}

		public List<Hero1Ability3Specs> listParam = new List<Hero1Ability3Specs>();
	}
}
