using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_2_3 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero2Ability3Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.damage);
		}

		public Hero2Ability3Specs getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}

		public List<Hero2Ability3Specs> listParam = new List<Hero2Ability3Specs>();
	}
}
