using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_3_1 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero3Ability1Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.attack_speed_increase);
			mainParam1.Add((float)param.magic_damage_bonus);
		}

		public Hero3Ability1Specs getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}

		public List<Hero3Ability1Specs> listParam = new List<Hero3Ability1Specs>();
	}
}
