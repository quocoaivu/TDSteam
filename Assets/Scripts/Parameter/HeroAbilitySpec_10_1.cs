using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_10_1 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero10Ability1Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.magic_damage);
			mainParam1.Add((float)param.def_down_percent);
		}

		public List<Hero10Ability1Specs> listParam = new List<Hero10Ability1Specs>();
	}
}
