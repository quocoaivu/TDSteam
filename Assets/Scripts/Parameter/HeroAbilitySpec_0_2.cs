using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_0_2 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero0Ability2Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.armor);
		}

		public Hero0Ability2Specs getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}

		public List<Hero0Ability2Specs> listParam = new List<Hero0Ability2Specs>();
	}
}
