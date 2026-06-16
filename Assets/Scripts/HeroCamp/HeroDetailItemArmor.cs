using System;
using Data;
using Parameter;
using UnityEngine;

namespace HeroCamp
{
	public class HeroDetailItemArmor : HeroOverviewItem
	{
		public override void Init(int heroID, int heroLevel)
		{
			base.Init(heroID, heroLevel);
			valueCurrentLevel = AbilityRankDescriber.Instance.GetArmorDescriptionByValue(HeroParameterManager.Instance.GetHeroPhysicsArmor(heroID, heroLevel));
			base.CurrentLevelValue.text = valueCurrentLevel;
			if (heroLevel + 1 > base.HeroMaxLevel)
			{
				heroLevel = base.HeroMaxLevel - 1;
			}
			valueNextLevel = AbilityRankDescriber.Instance.GetArmorDescriptionByValue(HeroParameterManager.Instance.GetHeroPhysicsArmor(heroID, heroLevel + 1));
			base.NextLevelValue.text = valueNextLevel;
			if (valueNextLevel != valueCurrentLevel)
			{
				base.NextLevelValue.color = Color.green;
			}
			else
			{
				base.NextLevelValue.color = Color.white;
			}
		}

		private string valueCurrentLevel;

		private string valueNextLevel;
	}
}
