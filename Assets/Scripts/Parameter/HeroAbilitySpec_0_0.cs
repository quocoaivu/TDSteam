using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_0_0 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero0Ability0Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.number_clone);
			mainParam1.Add((float)param.parameter_Scale);
		}

		public Hero0Ability0Specs getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}

		public List<Hero0Ability0Specs> listParam = new List<Hero0Ability0Specs>();
	}
}
