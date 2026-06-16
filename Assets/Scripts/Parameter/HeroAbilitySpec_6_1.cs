using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_6_1 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero6Ability1Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.percent_attack_damage_bonus);
			mainParam1.Add((float)param.duration / 1000f);
		}

		public Hero6Ability1Specs getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}

		public List<Hero6Ability1Specs> listParam = new List<Hero6Ability1Specs>();
	}
}
