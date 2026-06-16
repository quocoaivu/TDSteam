using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_10_0 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero10Ability0Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.duration * 0.001f);
			mainParam1.Add((float)param.magic_damage);
		}

		public List<Hero10Ability0Specs> listParam = new List<Hero10Ability0Specs>();
	}
}
