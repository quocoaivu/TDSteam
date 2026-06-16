using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_0_3 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero0Ability3Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.armor_per_unit);
		}

		public Hero0Ability3Specs getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}

		public List<Hero0Ability3Specs> listParam = new List<Hero0Ability3Specs>();
	}
}
