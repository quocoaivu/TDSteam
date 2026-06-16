using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_10_3 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero10Ability3Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.magic_damage);
		}

		public List<Hero10Ability3Specs> listParam = new List<Hero10Ability3Specs>();
	}
}
