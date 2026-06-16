using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_1_0 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero1Ability0Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.damage);
		}

		public Hero1Ability0Specs getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}

		public List<Hero1Ability0Specs> listParam = new List<Hero1Ability0Specs>();
	}
}
