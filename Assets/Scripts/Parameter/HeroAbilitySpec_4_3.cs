using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_4_3 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero4Ability3Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.attack_speed_bonus);
		}

		public Hero4Ability3Specs getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}

		public List<Hero4Ability3Specs> listParam = new List<Hero4Ability3Specs>();
	}
}
