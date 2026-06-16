using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_8_2 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero8Ability2Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.duration / 1000f);
		}

		public Hero8Ability2Specs getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}

		public List<Hero8Ability2Specs> listParam = new List<Hero8Ability2Specs>();
	}
}
