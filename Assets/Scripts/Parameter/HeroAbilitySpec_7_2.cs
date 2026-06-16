using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_7_2 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero7Ability2Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.chance_to_cast);
			mainParam1.Add((float)param.physics_damage);
		}

		public Hero7Ability2Specs getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}

		public List<Hero7Ability2Specs> listParam = new List<Hero7Ability2Specs>();
	}
}
