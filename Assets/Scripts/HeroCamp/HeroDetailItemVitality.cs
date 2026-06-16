using System;
using Parameter;
using UnityEngine;

namespace HeroCamp
{
	public class HeroDetailItemVitality : HeroOverviewItem
	{
		public override void Init(int heroID, int heroLevel)
		{
			base.Init(heroID, heroLevel);
			base.CurrentLevelValue.text = HeroParameterManager.Instance.GetHeroHealth(heroID, heroLevel).ToString();
			if (heroLevel + 1 > base.HeroMaxLevel)
			{
				heroLevel = base.HeroMaxLevel - 1;
			}
			base.NextLevelValue.text = HeroParameterManager.Instance.GetHeroHealth(heroID, heroLevel + 1).ToString();
			if (HeroParameterManager.Instance.GetHeroHealth(heroID, heroLevel + 1) > HeroParameterManager.Instance.GetHeroHealth(heroID, heroLevel))
			{
				base.NextLevelValue.color = Color.green;
			}
			else
			{
				base.NextLevelValue.color = Color.white;
			}
		}
	}
}
