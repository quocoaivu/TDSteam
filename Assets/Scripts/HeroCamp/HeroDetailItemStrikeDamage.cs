using System;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace HeroCamp
{
	public class HeroDetailItemStrikeDamage : HeroOverviewItem
	{
		public override void Init(int heroID, int heroLevel)
		{
			base.Init(heroID, heroLevel);
			base.CurrentLevelValue.text = HeroParameterManager.Instance.GetHeroDamageRange(heroID, heroLevel);
			if (heroLevel + 1 > base.HeroMaxLevel)
			{
				heroLevel = base.HeroMaxLevel - 1;
			}
			base.NextLevelValue.text = HeroParameterManager.Instance.GetHeroDamageRange(heroID, heroLevel + 1);
			if (HeroParameterManager.Instance.GetHeroDamageMax(heroID, heroLevel + 1) > HeroParameterManager.Instance.GetHeroDamageMax(heroID, heroLevel))
			{
				base.NextLevelValue.color = Color.green;
			}
			else
			{
				base.NextLevelValue.color = Color.white;
			}
			if (HeroParameterManager.Instance.IsPhysicsAttack(heroID))
			{
				iconAttack.sprite = physicsAtkSprite;
			}
			else
			{
				iconAttack.sprite = magicAtkSprite;
			}
		}

		[SerializeField]
		private Image iconAttack;

		[SerializeField]
		private Sprite physicsAtkSprite;

		[SerializeField]
		private Sprite magicAtkSprite;
	}
}
