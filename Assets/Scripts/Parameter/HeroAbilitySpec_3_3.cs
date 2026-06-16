using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_3_3 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero3Ability3Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.physics_damage);
		}

		public Hero3Ability3Specs getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}

		public List<Hero3Ability3Specs> listParam = new List<Hero3Ability3Specs>();
	}
}
