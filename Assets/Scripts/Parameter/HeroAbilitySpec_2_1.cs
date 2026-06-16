using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_2_1 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero2Ability1Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.armorBonus);
		}

		public Hero2Ability1Specs getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}

		public List<Hero2Ability1Specs> listParam = new List<Hero2Ability1Specs>();
	}
}
