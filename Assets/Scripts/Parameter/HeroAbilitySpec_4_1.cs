using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_4_1 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero4Ability1Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.duration / 1000f);
		}

		public Hero4Ability1Specs getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}

		public List<Hero4Ability1Specs> listParam = new List<Hero4Ability1Specs>();
	}
}
