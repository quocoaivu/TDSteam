using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_6_0 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero6Ability0Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.stun_duration / 1000f);
		}

		public Hero6Ability0Specs getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}

		public List<Hero6Ability0Specs> listParam = new List<Hero6Ability0Specs>();
	}
}
