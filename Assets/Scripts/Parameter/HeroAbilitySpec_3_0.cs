using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_3_0 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero3Ability0Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.physics_damage);
		}

		public Hero3Ability0Specs getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}

		public List<Hero3Ability0Specs> listParam = new List<Hero3Ability0Specs>();
	}
}
