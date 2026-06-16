using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_3_2 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero3Ability2Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.damage_burn);
		}

		public Hero3Ability2Specs getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}

		public List<Hero3Ability2Specs> listParam = new List<Hero3Ability2Specs>();
	}
}
