using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec_9_3 : HeroAbilitySpecBasic
	{
		public void AddParamToList(Hero9Ability3Specs param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.parameter_Scale);
			mainParam1.Add((float)param.duration / 1000f);
		}

		public Hero9Ability3Specs getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}

		public List<Hero9Ability3Specs> listParam = new List<Hero9Ability3Specs>();
	}
}
