using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_10_2 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero10Ability2Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.physic_damage);
		}

		public List<Hero10Ability2Specs> listParam = new List<Hero10Ability2Specs>();
	}
}
