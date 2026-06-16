using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_9_2 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero9Ability2Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.health_amount);
		}

		public Hero9Ability2Specs getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}

		public List<Hero9Ability2Specs> listParam = new List<Hero9Ability2Specs>();
	}
}
