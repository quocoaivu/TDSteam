using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_6_3 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero6Ability3Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.physics_damage);
		}

		public Hero6Ability3Specs getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}

		public List<Hero6Ability3Specs> listParam = new List<Hero6Ability3Specs>();
	}
}
