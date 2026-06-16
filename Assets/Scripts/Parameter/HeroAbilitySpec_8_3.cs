using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_8_3 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero8Ability3Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.attack_range_bonus_percentage);
		}

		public Hero8Ability3Specs getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}

		public List<Hero8Ability3Specs> listParam = new List<Hero8Ability3Specs>();
	}
}
