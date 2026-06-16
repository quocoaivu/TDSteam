using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_9_1 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero9Ability1Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.enemy_affected);
			mainParam1.Add((float)param.knock_back_duration / 1000f);
		}

		public Hero9Ability1Specs getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}

		public List<Hero9Ability1Specs> listParam = new List<Hero9Ability1Specs>();
	}
}
