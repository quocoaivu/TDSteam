using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_7_1 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero7Ability1Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.percent_health_activate);
			mainParam1.Add((float)param.slow_percent);
		}

		public Hero7Ability1Specs getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}

		public List<Hero7Ability1Specs> listParam = new List<Hero7Ability1Specs>();
	}
}
