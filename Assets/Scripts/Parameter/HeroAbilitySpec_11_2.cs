using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_11_2 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero11Ability2Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.magic_damage);
		}

		public List<Hero11Ability2Specs> listParam = new List<Hero11Ability2Specs>();
	}
}
