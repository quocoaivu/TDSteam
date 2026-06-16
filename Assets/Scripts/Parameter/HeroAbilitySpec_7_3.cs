using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_7_3 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero7Ability3Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.physics_damage);
			mainParam1.Add((float)param.countdown_time / 1000f);
		}

		public Hero7Ability3Specs getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}

		public List<Hero7Ability3Specs> listParam = new List<Hero7Ability3Specs>();
	}
}
