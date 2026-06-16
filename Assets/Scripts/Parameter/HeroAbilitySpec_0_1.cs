using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_0_1 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero0Ability1Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.damage * ((float)param.duration / 1000f) * 4f);
		}

		public Hero0Ability1Specs getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}

		public List<Hero0Ability1Specs> listParam = new List<Hero0Ability1Specs>();
	}
}
