using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_5_3 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero5Ability3Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.arrow_number);
		}

		public Hero5Ability3Specs getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}

		public List<Hero5Ability3Specs> listParam = new List<Hero5Ability3Specs>();
	}
}
