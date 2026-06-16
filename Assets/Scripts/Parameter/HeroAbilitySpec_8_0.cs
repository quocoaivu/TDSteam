using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_8_0 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero8Ability0Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)(param.physics_damage * param.number_of_projectile));
		}

		public Hero8Ability0Specs getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}

		public List<Hero8Ability0Specs> listParam = new List<Hero8Ability0Specs>();
	}
}
