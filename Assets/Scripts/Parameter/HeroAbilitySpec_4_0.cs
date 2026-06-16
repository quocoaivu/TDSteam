using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_4_0 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero4Ability0Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.physics_damage);
		}

		public Hero4Ability0Specs getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}

		public List<Hero4Ability0Specs> listParam = new List<Hero4Ability0Specs>();
	}
}
