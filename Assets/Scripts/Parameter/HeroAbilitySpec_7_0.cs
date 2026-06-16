using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_7_0 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero7Ability0Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.physics_damage);
		}

		public Hero7Ability0Specs getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}

		public List<Hero7Ability0Specs> listParam = new List<Hero7Ability0Specs>();
	}
}
