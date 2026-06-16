using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_11_1 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero11Ability1Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.magic_damage);
		}

		public List<Hero11Ability1Specs> listParam = new List<Hero11Ability1Specs>();
	}
}
