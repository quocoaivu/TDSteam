using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_6_2 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero6Ability2Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.physics_damage);
		}

		public Hero6Ability2Specs getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}

		public List<Hero6Ability2Specs> listParam = new List<Hero6Ability2Specs>();
	}
}
