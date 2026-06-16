using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_5_1 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero5Ability1Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.bonus_damage);
		}

		public Hero5Ability1Specs getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}

		public List<Hero5Ability1Specs> listParam = new List<Hero5Ability1Specs>();
	}
}
