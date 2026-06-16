using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_2_0 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero2Ability0Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.parameter_Scale);
		}

		public Hero2Ability0Specs getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}

		public List<Hero2Ability0Specs> listParam = new List<Hero2Ability0Specs>();
	}
}
