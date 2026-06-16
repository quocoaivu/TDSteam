using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_11_0 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero11Ability0Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.magic_damage);
		}

		public List<Hero11Ability0Specs> listParam = new List<Hero11Ability0Specs>();
	}
}
