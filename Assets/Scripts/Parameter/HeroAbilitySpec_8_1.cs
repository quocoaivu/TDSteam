using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_8_1 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero8Ability1Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.physics_damage);
			mainParam1.Add((float)param.attack_damage_decrease_percentage);
		}

		public Hero8Ability1Specs getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}

		public List<Hero8Ability1Specs> listParam = new List<Hero8Ability1Specs>();
	}
}
