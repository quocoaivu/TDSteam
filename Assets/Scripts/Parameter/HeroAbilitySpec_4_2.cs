using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_4_2 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero4Ability2Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.change_to_stun);
		}

		public Hero4Ability2Specs getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}

		public List<Hero4Ability2Specs> listParam = new List<Hero4Ability2Specs>();
	}
}
