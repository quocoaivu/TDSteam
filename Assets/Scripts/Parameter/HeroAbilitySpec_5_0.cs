using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_5_0 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero5Ability0Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.heal_amount);
		}

		public Hero5Ability0Specs getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}

		public List<Hero5Ability0Specs> listParam = new List<Hero5Ability0Specs>();
	}
}
