using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_9_0 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero9Ability0Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.number_of_minion);
			mainParam1.Add((float)param.minion_lifetime / 1000f);
		}

		public Hero9Ability0Specs getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}

		public List<Hero9Ability0Specs> listParam = new List<Hero9Ability0Specs>();
	}
}
