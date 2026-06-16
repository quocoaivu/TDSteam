using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_11_3 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero11Ability3Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.heal_duration * 0.001f);
		}

		public List<Hero11Ability3Specs> listParam = new List<Hero11Ability3Specs>();
	}
}
