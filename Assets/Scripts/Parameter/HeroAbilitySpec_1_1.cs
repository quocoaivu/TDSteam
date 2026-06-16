using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_1_1 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero1Ability1Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)(param.bonus_crit * 2));
		}

		public Hero1Ability1Specs getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}

		public List<Hero1Ability1Specs> listParam = new List<Hero1Ability1Specs>();
	}
}
